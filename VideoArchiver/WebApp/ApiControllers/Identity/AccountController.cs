using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.BLL.Identity.Services;
using App.Contracts.DAL;
using App.Domain.Identity;
using Base.WebHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using Public.DTO.v1.Identity;
using WebApp.Config;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// API controller for user account management endpoints
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private const int MaximumJwtExpirationTime = 10 * 60;

    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly UserService _userService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AccountController> _logger;
    private readonly Random _rnd = new();
    private readonly IAppUnitOfWork _uow;

    /// <summary>
    /// Construct a new AccountController
    /// </summary>
    /// <param name="signInManager">Object for handling user sign-in functionality</param>
    /// <param name="userManager">Object for managing users</param>
    /// <param name="configuration">Configuration object containing necessary settings for generating JWTs</param>
    /// <param name="logger">Logger for this AccountController</param>
    /// <param name="uow">Unit of Work object containing DAL repositories</param>
    /// <param name="userService">Custom service for managing users</param>
    /// <exception cref="ConfigurationErrorsException">Provided configuration doesn't contain expected JWT settings</exception>
    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager,
        IConfiguration configuration, ILogger<AccountController> logger,
        IAppUnitOfWork uow, UserService userService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtSettings = configuration.GetRequiredSection(JwtSettings.SectionKey).Get<JwtSettings>() ??
                       throw new ConfigurationErrorsException($"{nameof(JwtSettings)} not found in config!");
        _logger = logger;
        _uow = uow;
        _userService = userService;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationData">Required data for registering a new user.</param>
    /// <param name="expiresInSeconds">The amount of seconds the created JWT should be valid for.
    /// Cannot be larger than maximum expiration time (configured in settings).</param>
    /// <returns>New JWT and refresh token (with expiration date) for the registered account, if registration doesn't require further approval.</returns>
    /// <response code="200">The registration was successful.</response>
    /// <response code="400">Invalid registration data. (User with provided username is already registered or something else went wrong).</response>
    /// <response code="202">The registration was successful, but must be approved by an administrator before the account can be used.</response>
    [HttpPost]
    public async Task<ActionResult<JwtResponse?>> Register([FromBody] Register registrationData,
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
        var user = await _userManager.FindByNameAsync(registrationData.Username);
        if (user != null)
        {
            _logger.LogWarning("User with username {} is already registered", registrationData.Username);
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with username {registrationData.Username} is already registered"
            });
        }

        // register user
        var refreshToken = new RefreshToken(expiresInDays: _jwtSettings.RefreshTokenExpiresInDays);
        var (result, newUser) =
            await _userService.CreateUser(registrationData.Username, registrationData.Password, refreshToken);
        user = newUser;

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
        user = await _userManager.FindByNameAsync(user.UserName!);
        if (user == null)
        {
            _logger.LogWarning("User with username {} is not found after registration", registrationData.Username);
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with username {registrationData.Username} is not found after registration"
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
        return user.IsApproved ? Ok(res) : Accepted(res);
    }

    /// <summary>
    /// Log in as an existing user, using password authentication
    /// </summary>
    /// <param name="loginData">Required data for logging in</param>
    /// <param name="expiresInSeconds">The amount of seconds the created JWT should be valid for.
    /// Cannot be larger than maximum expiration time (configured in settings).</param>
    /// <returns>New JWT and refresh token (with expiration date), if login was successful.</returns>
    /// <response code="200">Login was successful.</response>
    /// <response code="404">Username or password was invalid.</response>
    /// <response code="401">Login isn't allowed, because the user account hasn't been approved yet.</response>
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
        var user = await _userManager.FindByNameAsync(loginData.Username);
        if (user == null)
        {
            _logger.LogWarning("WebApi login failed, username {} not found", loginData.Username);
            await DelayRandom();

            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        if (!user.IsApproved)
        {
            return Unauthorized();
        }

        // verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Username);
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

    /// <summary>
    /// Get a new JWT and refresh token, using existing JWT and refresh token.
    /// </summary>
    /// <param name="refreshTokenModel">Tokens to refresh</param>
    /// <param name="expiresInSeconds">The amount of seconds the created JWT should be valid for.
    /// Cannot be larger than maximum expiration time (configured in settings).</param>
    /// <returns>Refreshed JWT and refresh token (with expiration date), if refreshing was successful.</returns>
    /// <response code="200">Token refresh was successful.</response>
    /// <response code="400">Provided token/tokens was/were invalid.</response>
    /// <response code="404">JWT user not found or one matching refresh token not found.</response>
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

        var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (userName == null)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = "No username in JWT"
            });
        }

        // get user and tokens
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = $"User with username {userName} not found"
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

    /// <summary>
    /// Log out user by deleting provided refresh token.
    /// User access will be refused when JWT expires.
    /// </summary>
    /// <returns>Amount of refresh tokens deleted.</returns>
    /// <param name="logout">The refresh token to delete.</param>
    /// <response code="200">Refresh token deleted successfully.</response>
    /// <response code="404">Refresh token's user not found.</response>
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

        var refreshTokens = await _uow.RefreshTokens.GetAllByUserIdAsync(user.Id, r =>
            r.RefreshToken == logout.RefreshToken ||
            r.PreviousRefreshToken == logout.RefreshToken);

        foreach (var appRefreshToken in refreshTokens)
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