using System.Security.Claims;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Entities.Identity;
using App.BLL.DTO.Exceptions.Identity;
using App.BLL.DTO.Mappers;
using App.Contracts.DAL;
using AutoMapper;
using Base.WebHelpers;
using Contracts.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace App.BLL.Identity.Services;

public class UserService : IAppUowContainer
{
    private readonly IConfiguration _configuration;
    private readonly IdentityUow _identityUow;
    private readonly AuthorMapper _authorMapper;
    private readonly Random _rnd = new();

    public UserService(IConfiguration configuration, IdentityUow identityUow, IMapper mapper)
    {
        _configuration = configuration;
        _identityUow = identityUow;
        _authorMapper = new AuthorMapper(mapper);
    }

    private IAppUnitOfWork Uow => _identityUow.Uow;

    private SignInManager<App.Domain.Identity.User> SignInManager => _identityUow.SignInManager;
    private UserManager<App.Domain.Identity.User> UserManager => _identityUow.UserManager;

    private bool AutoApproveRegistration => _configuration.GetValue<bool>("AutoApproveRegistration");

    public async Task<(IdentityResult identityResult, App.Domain.Identity.User user)> CreateUser(string username,
        string password)
    {
        var user = new App.Domain.Identity.User
        {
            UserName = username,
            IsApproved = AutoApproveRegistration,
        };

        var result = await SignInManager.UserManager.CreateAsync(user, password);
        return (result, user);
    }

    public async Task<SignInResult> SignInAsync(string username, string password, bool isPersistent,
        bool lockoutOnFailure)
    {
        var user = await SignInManager.UserManager.FindByNameAsync(username);
        return user switch
        {
            { IsApproved: false } => SignInResult.NotAllowed,
            null => SignInResult.Failed,
            _ => await SignInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure)
        };
    }

    public async Task<JwtResult> SignInJwtAsync(string username, string password, int? expiresInSeconds = null,
        bool lockOutOnFailure = false)
    {
        var user = await UserManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new UserNotFoundException(username);
        }

        if (!user.IsApproved)
        {
            throw new UserNotApprovedException();
        }

        var result = await SignInManager.CheckPasswordSignInAsync(user, password, lockOutOnFailure);
        if (!result.Succeeded)
        {
            await DelayRandom();
            throw new WrongPasswordException(username);
        }

        await _identityUow.TokenService.DeleteExpiredRefreshTokensAsync(user.Id);
        var refreshToken = _identityUow.TokenService.CreateAndAddRefreshToken(user.Id);

        var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(user);
        var jwt = _identityUow.TokenService.GenerateJwt(claimsPrincipal, expiresInSeconds);

        return new JwtResult
        {
            Jwt = jwt,
            RefreshToken = refreshToken,
        };
    }

    private async Task DelayRandom(int minValueMs = 100, int maxValueMs = 1000)
    {
        await Task.Delay(_rnd.Next(minValueMs, maxValueMs));
    }

    public const string SelectedUserAuthorCookieKey = "VideoArchiverSelectedUserAuthor";

    public async Task<ICollection<Author>> GetAllUserSubAuthorsAsync(ClaimsPrincipal user)
    {
        return (await Uow.Authors.GetAllUserSubAuthors(user.GetUserId()))
            .Select(e => _authorMapper.Map(e)!)
            .ToList();
    }

    public async Task<bool> IsUserSubAuthor(Guid authorId, ClaimsPrincipal user)
    {
        return await Uow.Authors.IsUserSubAuthor(authorId, user.GetUserId());
    }

    public static void ClearSelectedAuthorCookies(HttpResponse httpResponse)
    {
        httpResponse.Cookies.Delete(SelectedUserAuthorCookieKey);
    }

    public static void SetSelectedAuthorCookies(HttpResponse httpResponse, Guid authorId)
    {
        ClearSelectedAuthorCookies(httpResponse);
        httpResponse.Cookies.Append(SelectedUserAuthorCookieKey, authorId.ToString());
    }

    public async Task SignOutAsync()
    {
        await SignInManager.SignOutAsync();
        ClearSelectedAuthorCookies(SignInManager.Context.Response);
    }

    public async Task SignOutTokenAsync(Guid userId, string refreshToken)
    {
        // Delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt - that would require pipeline modification and checking against db on every request
        // So client can actually continue to use the jwt until it expires (keep the jwt expiration time short ~1 min)

        var user = await Uow.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        await _identityUow.TokenService.DeleteRefreshTokenAsync(user.Id, refreshToken);
    }

    public Author CreateAuthor(ClaimsPrincipal user)
    {
        var id = Guid.NewGuid();
        var author = new App.DAL.DTO.Entities.Author
        {
            Platform = DAL.DTO.Enums.Platform.This,
            Id = id,
            IdOnPlatform = id.ToString(),
            DisplayName = user.Identity?.Name,
            UserName = user.Identity?.Name,
            UserId = user.GetUserId(),
        };
        return _authorMapper.Map(Uow.Authors.Add(author))!;
    }

    public async Task<JwtResult?> RegisterUserAsync(string username, string password, int? expiresInSeconds)
    {
        var user = await UserManager.FindByNameAsync(username);
        if (user != null)
        {
            throw new UserAlreadyRegisteredException(username);
        }

        var (result, _) = await CreateUser(username, password);
        if (!result.Succeeded)
        {
            throw new IdentityOperationFailedException(result.Errors);
        }

        user = await UserManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new IdentityOperationFailedException($"User with username {username} not found after registration");
        }

        if (!user.IsApproved)
        {
            return null;
        }

        var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(user);
        var jwt = _identityUow.TokenService.GenerateJwt(claimsPrincipal, expiresInSeconds);
        var refreshToken = _identityUow.TokenService.CreateAndAddRefreshToken(user.Id);

        return new JwtResult
        {
            Jwt = jwt,
            RefreshToken = refreshToken,
        };
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _identityUow.SaveChangesAsync();
    }
}