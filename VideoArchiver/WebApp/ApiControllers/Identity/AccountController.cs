using System.Configuration;
using System.Net;
using App.BLL.DTO.Exceptions.Identity;
using App.BLL.Identity;
using Base.WebHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using Public.DTO.v1.Identity;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// API controller for user account management endpoints
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IdentityUow _identityUow;

    /// <summary>
    /// Construct a new AccountController
    /// </summary>
    /// <param name="identityUow">Container for identity-related services</param>
    /// <exception cref="ConfigurationErrorsException">Provided configuration doesn't contain expected JWT settings</exception>
    public AccountController(IdentityUow identityUow)
    {
        _identityUow = identityUow;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationData">Required data for registering a new user.</param>
    /// <param name="expiresInSeconds">The amount of seconds the created JWT should be valid for.</param>
    /// <returns>New JWT and refresh token (with expiration date) for the registered account, if registration doesn't require further approval.</returns>
    /// <response code="200">The registration was successful.</response>
    /// <response code="400">User with provided username is already registered.</response>
    /// <response code="202">The registration was successful, but must be approved by an administrator before the account can be used.</response>
    [HttpPost]
    public async Task<ActionResult<JwtResponse?>> Register([FromBody] Register registrationData,
        [FromQuery] int? expiresInSeconds = null)
    {
        try
        {
            var jwtResult = await _identityUow.UserService.RegisterUserAsync(
                registrationData.Username,
                registrationData.Password,
                expiresInSeconds);
            await _identityUow.SaveChangesAsync();
            if (jwtResult == null)
            {
                return Accepted();
            }

            return new JwtResponse
            {
                Jwt = jwtResult.Jwt,
                RefreshToken = jwtResult.RefreshToken.RefreshToken,
                RefreshTokenExpiresAt = jwtResult.RefreshToken.ExpiresAt,
            };
        }
        catch (UserAlreadyRegisteredException e)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = e.Message,
            });
        }
        catch (InvalidJwtExpirationRequestedException e)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = e.Message,
            });
        }
    }

    /// <summary>
    /// Log in as an existing user, using password authentication
    /// </summary>
    /// <param name="loginData">Required data for logging in</param>
    /// <param name="expiresInSeconds">The amount of seconds the created JWT should be valid for.</param>
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
            var jwtResult =
                await _identityUow.UserService.SignInJwtAsync(loginData.Username, loginData.Password, expiresInSeconds);
            await _identityUow.SaveChangesAsync();
            return Ok(new JwtResponse
            {
                Jwt = jwtResult.Jwt,
                RefreshToken = jwtResult.RefreshToken.RefreshToken,
                RefreshTokenExpiresAt = jwtResult.RefreshToken.ExpiresAt,
            });
        }
        catch (UserNotFoundException)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "Username/password problem",
            });
        }
        catch (WrongPasswordException)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "Username/password problem",
            });
        }
        catch (UserNotApprovedException)
        {
            return Unauthorized(new RestApiErrorResponse
            {
                Status = HttpStatusCode.Unauthorized,
                Error = "This user account must be approved by an administrator before it can be used.",
            });
        }
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
            var jwtResult = await _identityUow.TokenService.RefreshToken(refreshTokenModel.Jwt,
                refreshTokenModel.RefreshToken,
                expiresInSeconds);
            await _identityUow.SaveChangesAsync();
            return Ok(new JwtResponse
            {
                Jwt = jwtResult.Jwt,
                RefreshToken = jwtResult.RefreshToken.RefreshToken,
                RefreshTokenExpiresAt = jwtResult.RefreshToken.ExpiresAt,
            });
        }
        catch (InvalidJwtException)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = "Invalid JWT",
            });
        }
        catch (NoRefreshTokensException)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = "Invalid refresh token (probably expired)",
            });
        }
    }

    /// <summary>
    /// Log out user by deleting provided refresh token.
    /// User access will be refused when JWT expires.
    /// </summary>
    /// <param name="logout">The refresh token to delete.</param>
    /// <response code="200">Refresh token deleted successfully.</response>
    /// <response code="404">Refresh token's user not found.</response>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Logout(
        [FromBody] Logout logout)
    {
        try
        {
            await _identityUow.UserService.SignOutTokenAsync(User.GetUserId(), logout.RefreshToken);
            await _identityUow.SaveChangesAsync();
            return Ok();
        }
        catch (UserNotFoundException e)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = e.Message,
            });
        }
    }
}