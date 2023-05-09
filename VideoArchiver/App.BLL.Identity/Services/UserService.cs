using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace App.BLL.Identity.Services;

public class UserService
{
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public UserService(SignInManager<User> signInManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _configuration = configuration;
    }

    private bool AutoApproveRegistration => _configuration.GetValue<bool>("AutoApproveRegistration");

    public async Task<(IdentityResult identityResult, User user)> CreateUser(string username, string password)
    {
        var user = new User
        {
            UserName = username,
            IsApproved = AutoApproveRegistration,
        };

        var result = await _signInManager.UserManager.CreateAsync(user, password);
        return (result, user);
    }

    public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var user = await _signInManager.UserManager.FindByNameAsync(username);
        return user switch
        {
            { IsApproved: false } => SignInResult.NotAllowed,
            null => SignInResult.Failed,
            _ => await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure)
        };
    }
}