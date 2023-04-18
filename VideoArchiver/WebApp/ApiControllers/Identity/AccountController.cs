﻿using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.Contracts.DAL;
using Base.WebHelpers;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using Public.DTO.v1.Identity;
using WebApp.Config;

namespace WebApp.ApiControllers.Identity;

[ApiController]
[Route("api/v1/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private const int MaximumJwtExpirationTime = 10 * 60;

    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AccountController> _logger;
    private readonly Random _rnd = new();
    private readonly IAppUnitOfWork _uow;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager,
        IConfiguration configuration, ILogger<AccountController> logger,
        IAppUnitOfWork uow)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtSettings = configuration.GetRequiredSection(JwtSettings.SectionKey).Get<JwtSettings>() ??
                       throw new ConfigurationErrorsException($"{nameof(JwtSettings)} not found in config!");
        _logger = logger;
        _uow = uow;
    }

    public async Task<ActionResult<JwtResponse>> Register([FromBody] Register registrationData,
        [FromQuery] int? expiresInSeconds = null)
    {
        try
        {
            expiresInSeconds = ValidateExpiresInSeconds(expiresInSeconds);
        }
        catch (ActionResultException e)
        {
            return e.ActionResult;
        }

        // is user already registered
        var user = await _userManager.FindByEmailAsync(registrationData.Email);
        if (user != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with email {registrationData.Email} is already registered"
            });
        }

        // register user
        var refreshToken = new RefreshToken(expiresInDays: _jwtSettings.RefreshTokenExpiresInDays);
        user = new User
        {
            Email = registrationData.Email,
            UserName = registrationData.Email,
            RefreshTokens = new List<RefreshToken> { refreshToken }
        };
        refreshToken.User = user;

        var result = await _userManager.CreateAsync(user, registrationData.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }

        // // How to add claims:
        // result = await _userManager.AddClaimsAsync(User, new List<Claim>()
        // {
        //     new(ClaimTypes.GivenName, User.FirstName),
        //     new(ClaimTypes.Surname, User.LastName)
        // });

        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }

        // get full user from system with fixed data (maybe there is something generated by identity that we might need
        user = await _userManager.FindByEmailAsync(user.Email);
        if (user == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationData.Email);
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with email {registrationData.Email} is not found after registration"
            });
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        var jwt = GenerateJwt(claimsPrincipal, expiresInSeconds.Value);
        var res = new JwtResponse
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt,
        };
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] Login loginData,
        [FromQuery] int? expiresInSeconds = null)
    {
        try
        {
            expiresInSeconds = ValidateExpiresInSeconds(expiresInSeconds);
        }
        catch (ActionResultException e)
        {
            return e.ActionResult;
        }

        // verify username
        var user = await _userManager.FindByEmailAsync(loginData.Email);
        if (user == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await DelayRandom();

            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Email);
            await DelayRandom();
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

        var userRefreshTokens = await _uow.RefreshTokens.GetAllByUserIdAsync(user.Id);

        // remove expired tokens
        foreach (var userRefreshToken in userRefreshTokens)
        {
            if (userRefreshToken.IsFullyExpired)
            {
                _uow.RefreshTokens.Remove(userRefreshToken);
            }
        }

        var refreshToken = new RefreshToken()
        {
            UserId = user.Id
        };
        _uow.RefreshTokens.Add(refreshToken);

        await _uow.SaveChangesAsync();

        var jwt = GenerateJwt(claimsPrincipal, expiresInSeconds.Value);

        var res = new JwtResponse
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt,
        };

        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponse>> RefreshToken(
        [FromBody] RefreshTokenModel refreshTokenModel,
        [FromQuery] int? expiresInSeconds = null)
    {
        try
        {
            expiresInSeconds = ValidateExpiresInSeconds(expiresInSeconds);
        }
        catch (ActionResultException e)
        {
            return e.ActionResult;
        }

        JwtSecurityToken jwtToken;
        // get user info from jwt
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                return BadRequest(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No token"
                });
            }
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"Cant parse the token, {e.Message}"
            });
        }

        if (!IdentityHelpers.ValidateToken(
                refreshTokenModel.Jwt,
                _jwtSettings.Key,
                _jwtSettings.Issuer,
                _jwtSettings.Audience
            ))
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"JWT validation fail"
            });
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = "No email in JWT"
            });
        }

        // get user and tokens
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = $"User with email {userEmail} not found"
            });
        }

        // load and compare refresh tokens
        var userRefreshTokens = await _uow.RefreshTokens.GetAllByUserIdAsync(user.Id,
            r =>
                (r.RefreshToken == refreshTokenModel.RefreshToken && r.ExpiresAt > DateTime.UtcNow) ||
                (r.PreviousRefreshToken == refreshTokenModel.RefreshToken && r.PreviousExpiresAt > DateTime.UtcNow));

        if (userRefreshTokens.Count == 0)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "No refresh tokens found!"
            });
        }

        if (userRefreshTokens.Count > 1)
        {
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "More than one valid refresh token found"
            });
        }

        // generate new jwt

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

        var jwt = GenerateJwt(claimsPrincipal, expiresInSeconds.Value);

        // make new refresh token, keep old one still valid for some time
        var refreshToken = userRefreshTokens.First();
        if (refreshToken.RefreshToken == refreshTokenModel.RefreshToken)
        {
            refreshToken.Refresh(TimeSpan.FromMinutes(_jwtSettings.ExtendOldRefreshTokenExpirationByMinutes),
                TimeSpan.FromDays(_jwtSettings.RefreshTokenExpiresInDays));
            _uow.RefreshTokens.Update(refreshToken);

            await _uow.SaveChangesAsync();
        }

        var res = new JwtResponse
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt,
        };

        return Ok(res);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Logout(
        [FromBody] Logout logout)
    {
        // delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt - that would require pipeline modification and checking against db on every request
        // so client can actually continue to use the jwt until it expires (keep the jwt expiration time short ~1 min)

        var userId = User.GetUserId();

        var user = await _uow.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        await _uow.RefreshTokens.GetAllByUserIdAsync(user.Id, r =>
            r.RefreshToken == logout.RefreshToken ||
            r.PreviousRefreshToken == logout.RefreshToken);

        foreach (var appRefreshToken in user.RefreshTokens!)
        {
            _uow.RefreshTokens.Remove(appRefreshToken);
        }

        var deleteCount = await _uow.SaveChangesAsync();

        return Ok(new { TokenDeleteCount = deleteCount });
    }

    private int ValidateExpiresInSeconds(int? expiresInSeconds)
    {
        if (expiresInSeconds == null)
        {
            expiresInSeconds = _jwtSettings.ExpiresInSeconds;

            if (expiresInSeconds <= 0)
            {
                _logger.LogError($"Configured JWT expiration was {expiresInSeconds} <= 0");
                throw new ActionResultException(new StatusCodeResult(StatusCodes.Status500InternalServerError));
            }

            if (expiresInSeconds > MaximumJwtExpirationTime)
            {
                _logger.LogError($"Configured JWT expiration was {expiresInSeconds} > {MaximumJwtExpirationTime}");
                throw new ActionResultException(new StatusCodeResult(StatusCodes.Status500InternalServerError));
            }
        }

        if (expiresInSeconds <= 0)
        {
            throw new ActionResultException(BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"Requested JWT expiration time was {expiresInSeconds} <= 0"
            }));
        }

        if (expiresInSeconds > MaximumJwtExpirationTime)
        {
            throw new ActionResultException(BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"Requested JWT expiration time was {expiresInSeconds} > {MaximumJwtExpirationTime}"
            }));
        }

        return expiresInSeconds.Value;
    }

    private async Task DelayRandom(int minValueMs = 100, int maxValueMs = 1000)
    {
        await Task.Delay(_rnd.Next(minValueMs, maxValueMs));
    }

    private string GenerateJwt(ClaimsPrincipal claimsPrincipal, int expiresInSeconds)
    {
        return IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _jwtSettings.Key,
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            expiresInSeconds
        );
    }
}

internal class ActionResultException : Exception
{
    public readonly ActionResult ActionResult;

    public ActionResultException(ActionResult actionResult)
    {
        ActionResult = actionResult;
    }
}