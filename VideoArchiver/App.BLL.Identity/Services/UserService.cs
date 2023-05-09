using System.Security.Claims;
using App.BLL.Base;
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Enums;
using App.Domain.Identity;
using Base.WebHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace App.BLL.Identity.Services;

public class UserService : BaseAppUowContainer
{
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public UserService(SignInManager<User> signInManager, IConfiguration configuration, IAppUnitOfWork uow) : base(uow)
    {
        _signInManager = signInManager;
        _configuration = configuration;
    }

    private bool AutoApproveRegistration => _configuration.GetValue<bool>("AutoApproveRegistration");

    public async Task<(IdentityResult identityResult, User user)> CreateUser(string username, string password, params RefreshToken[] refreshTokens)
    {
        var user = new User
        {
            UserName = username,
            IsApproved = AutoApproveRegistration,
        };

        if (refreshTokens.Length > 0)
        {
            user.RefreshTokens = new List<RefreshToken>(refreshTokens);
            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.User = user;
            }
        }

        var result = await _signInManager.UserManager.CreateAsync(user, password);
        return (result, user);
    }

    public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent,
        bool lockoutOnFailure)
    {
        var user = await _signInManager.UserManager.FindByNameAsync(username);
        return user switch
        {
            { IsApproved: false } => SignInResult.NotAllowed,
            null => SignInResult.Failed,
            _ => await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure)
        };
    }

    public const string SelectedUserAuthorCookieKey = "VideoArchiverSelectedUserAuthor";

    public async Task<ICollection<Author>> GetAllUserSubAuthorsAsync(ClaimsPrincipal user)
    {
        return await Uow.Users.GetAllUserSubAuthors(user.GetUserId());
    }

    public async Task<bool> IsUserSubAuthor(Guid authorId, ClaimsPrincipal user)
    {
        return await Uow.Users.IsUserSubAuthor(authorId, user.GetUserId());
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
        await _signInManager.SignOutAsync();
        ClearSelectedAuthorCookies(_signInManager.Context.Response);
    }

    public Author CreateAuthor(ClaimsPrincipal user)
    {
        var id = Guid.NewGuid();
        var author = new Author
        {
            Platform = Platform.This,
            Id = id,
            IdOnPlatform = id.ToString(),
            DisplayName = user.Identity?.Name,
            UserName = user.Identity?.Name,
            UserId = user.GetUserId(),
        };
        return Uow.Authors.Add(author);
    }
}