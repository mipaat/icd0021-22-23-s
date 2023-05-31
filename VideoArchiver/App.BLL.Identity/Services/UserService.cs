using System.Security.Claims;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Entities.Identity;
using App.BLL.DTO.Exceptions;
using App.BLL.DTO.Exceptions.Identity;
using App.BLL.DTO.Mappers;
using App.Common;
using App.Common.Enums;
using App.Common.Exceptions;
using App.Contracts.DAL;
using App.Resources.WebApp.Areas.Identity.Pages.Account;
using AutoMapper;
using Base.WebHelpers;
using Contracts.BLL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace App.BLL.Identity.Services;

public class UserService : IAppUowContainer
{
    private readonly IConfiguration _configuration;
    private readonly IdentityUow _identityUow;
    private readonly AuthorMapper _authorMapper;
    private readonly UserMapper _userMapper;
    private readonly Random _rnd = new();

    public UserService(IConfiguration configuration, IdentityUow identityUow, IMapper mapper)
    {
        _configuration = configuration;
        _identityUow = identityUow;
        _authorMapper = new AuthorMapper(mapper);
        _userMapper = new UserMapper(mapper);
    }

    private IAppUnitOfWork Uow => _identityUow.Uow;

    private SignInManager<App.Domain.Identity.User> SignInManager => _identityUow.SignInManager;
    private UserManager<App.Domain.Identity.User> UserManager => _identityUow.UserManager;
    private RoleManager<App.Domain.Identity.Role> RoleManager => _identityUow.RoleManager;

    private bool AutoApproveRegistration => _configuration.GetValue<bool>("AutoApproveRegistration");

    public async Task<(IdentityResult identityResult, User user)> CreateUser(string username,
        string password)
    {
        var user = new User
        {
            UserName = username,
            IsApproved = AutoApproveRegistration,
        };

        var result = await SignInManager.UserManager.CreateAsync(Uow.Users.Map(_userMapper.Map(user)!), password);
        return (result, user);
    }

    public async Task SignInAsync(User user, bool isPersistent)
    {
        var dalUser = _userMapper.Map(user)!;
        var domainUser = Uow.Users.GetTrackedEntity(user.Id);
        if (domainUser != null)
        {
            domainUser = Uow.Users.Map(dalUser, domainUser);
        }
        else
        {
            domainUser = Uow.Users.Map(dalUser);
        }

        await SignInManager.SignInAsync(domainUser, isPersistent: isPersistent);
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

    public async Task SignOutAsync()
    {
        await SignInManager.SignOutAsync();
        SignInManager.Context.Response.ClearSelectedAuthorCookies();
    }

    public async Task SignOutTokenAsync(string jwt, string refreshToken)
    {
        // Delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt - that would require pipeline modification and checking against db on every request
        // So client can actually continue to use the jwt until it expires (keep the jwt expiration time short ~1 min)
        await _identityUow.TokenService.DeleteRefreshTokenAsync(jwt, refreshToken);
    }

    public Author CreateAuthor(ClaimsPrincipal user)
    {
        var id = Guid.NewGuid();
        var author = new App.DAL.DTO.Entities.Author
        {
            Platform = EPlatform.This,
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

    public async Task<IList<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync() =>
        (await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList();

    public async Task<UserWithRoles?> GetUserWithRoles(Guid id)
    {
        return _userMapper.Map(await Uow.Users.GetByIdWithRolesAsync(id));
    }

    public async Task<ICollection<UserWithRoles>> GetUsersWithRoles(bool includeOnlyRequiringApproval = false,
        string? nameQuery = null)
    {
        if (nameQuery != null && nameQuery.Trim().Length == 0) nameQuery = null;
        return (await Uow.Users.GetAllWithRoles(includeOnlyRequiringApproval, nameQuery))
            .Select(u => _userMapper.Map(u)!).ToList();
    }

    public async Task ApproveRegistration(Guid userId)
    {
        var user = await Uow.Users.GetByIdAsync(userId) ?? throw new NotFoundException();
        user.IsApproved = true;
        Uow.Users.Update(user);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _identityUow.SaveChangesAsync();
    }

    public async Task AddUserToRole(Guid userId, string roleName)
    {
        var role = await RoleManager.FindByNameAsync(roleName) ?? throw new NotFoundException();
        if (await Uow.Users.IsInRoleAsync(userId, role.Id)) return;
        Uow.Users.AddToRoles(userId, role.Id);
    }

    public async Task RemoveUserFromRole(Guid userId, string roleName)
    {
        var role = await RoleManager.FindByNameAsync(roleName) ?? throw new NotFoundException();
        if (!await Uow.Users.IsInRoleAsync(userId, role.Id)) return;
        await Uow.Users.RemoveFromRolesAsync(userId, role.Id);
    }

    public async Task<IdentityResult> DeleteAsync(App.Domain.Identity.User user)
    {
        await Uow.Users.DeleteRelatedEntitiesAsync(user.Id);
        return await UserManager.DeleteAsync(user);
    }
}