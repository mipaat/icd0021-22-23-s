using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.BLL.DTO.Entities.Identity;
using App.BLL.DTO.Exceptions.Identity;
using App.BLL.DTO.Mappers;
using App.BLL.Identity.Config;
using App.Contracts.DAL;
using AutoMapper;
using Base.WebHelpers;
using Microsoft.Extensions.Configuration;
using RefreshToken = App.DAL.DTO.Entities.Identity.RefreshToken;

namespace App.BLL.Identity.Services;

public class TokenService
{
    private IdentityUow _identityUow;
    private readonly JwtSettings _jwtSettings;
    private readonly RefreshTokenMapper _refreshTokenMapper;

    public TokenService(IdentityUow identityUow, IConfiguration configuration, IMapper mapper)
    {
        _identityUow = identityUow;
        _jwtSettings = configuration.GetRequiredSection(JwtSettings.SectionKey).Get<JwtSettings>() ??
                       throw new ConfigurationErrorsException($"{nameof(JwtSettings)} not found in config!");
        _refreshTokenMapper = new RefreshTokenMapper(mapper);
    }

    protected IAppUnitOfWork Uow => _identityUow.Uow;

    public async Task DeleteRefreshTokenAsync(string jwt, string refreshToken)
    {
        ClaimsPrincipal principal;
        try
        {
            principal = IdentityHelpers.GetClaimsPrincipal(jwt, _jwtSettings.Key, _jwtSettings.Issuer, _jwtSettings.Audience);
        }
        catch (Exception)
        {
            throw new InvalidJwtException();
        }

        var userId = principal.GetUserIdIfExists() ?? throw new InvalidJwtException();

        var refreshTokens = await Uow.RefreshTokens.GetAllByUserIdAsync(userId, r =>
            r.RefreshToken == refreshToken ||
            r.PreviousRefreshToken == refreshToken);

        foreach (var appRefreshToken in refreshTokens)
        {
            Uow.RefreshTokens.Remove(appRefreshToken);
        }
    }

    public async Task DeleteExpiredRefreshTokensAsync(Guid userId)
    {
        var refreshTokens = await Uow.RefreshTokens.GetAllFullyExpiredByUserIdAsync(userId);
        foreach (var refreshToken in refreshTokens)
        {
            Uow.RefreshTokens.Remove(refreshToken);
        }
    }
    
    private int ValidateExpiresInSeconds(int? expiresInSeconds)
    {
        if (expiresInSeconds == null)
        {
            if (_jwtSettings.ExpiresInSeconds <= 0)
            {
                throw new ConfigurationErrorsException(
                    $"Configured JWT expiration time must be greater than 0 seconds, but was {_jwtSettings.ExpiresInSeconds}");
            }
            
            expiresInSeconds = _jwtSettings.ExpiresInSeconds;
        } else if (expiresInSeconds <= 0)
        {
            throw new InvalidJwtExpirationRequestedException(expiresInSeconds.Value);
        }

        return expiresInSeconds.Value;
    }

    public string GenerateJwt(ClaimsPrincipal claimsPrincipal, int? expiresInSeconds)
    {
        return IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _jwtSettings.Key,
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            ValidateExpiresInSeconds(expiresInSeconds)
        );
    }

    public App.BLL.DTO.Entities.Identity.RefreshToken CreateAndAddRefreshToken(Guid userId)
    {
        var refreshToken = new RefreshToken(_jwtSettings.RefreshTokenExpiresInDays)
        {
            UserId = userId
        };
        Uow.RefreshTokens.Add(refreshToken);
        return _refreshTokenMapper.Map(refreshToken)!;
    }

    public async Task<JwtResult> RefreshToken(string jwt, string refreshToken, int? expiresInSeconds = null)
    {
        JwtSecurityToken jwtToken;
        // get user info from jwt
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(jwt) ?? throw new InvalidJwtException();
        }
        catch (ArgumentException)
        {
            throw new InvalidJwtException();
        }

        if (!IdentityHelpers.ValidateToken(jwt, _jwtSettings.Key, _jwtSettings.Issuer, _jwtSettings.Audience))
        {
            throw new InvalidJwtException();
        }

        var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? throw new InvalidJwtException();

        var user = await _identityUow.UserManager.FindByNameAsync(userName) ?? throw new UserNotFoundException(userName);

        var userRefreshTokens = await Uow.RefreshTokens.GetAllValidByUserIdAndRefreshTokenAsync(user.Id, refreshToken);

        if (userRefreshTokens.Count == 0)
        {
            throw new NoRefreshTokensException();
        }

        if (userRefreshTokens.Count > 1)
        {
            throw new TooManyRefreshTokensException();
        }

        var claimsPrincipal = await _identityUow.SignInManager.CreateUserPrincipalAsync(user);
        jwt = GenerateJwt(claimsPrincipal, expiresInSeconds);

        var userRefreshToken = userRefreshTokens.First();
        userRefreshToken.Refresh(TimeSpan.FromMinutes(_jwtSettings.ExtendOldRefreshTokenExpirationByMinutes),
            TimeSpan.FromDays(_jwtSettings.RefreshTokenExpiresInDays));
        Uow.RefreshTokens.Update(userRefreshToken);

        return new JwtResult
        {
            Jwt = jwt,
            RefreshToken = _refreshTokenMapper.Map(userRefreshToken)!,
        };
    }
}