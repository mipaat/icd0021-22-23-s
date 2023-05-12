using App.BLL.Base;
using App.BLL.Identity.Services;
using App.Contracts.DAL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Identity;

public class IdentityUow : BaseAppUowContainer
{
    private readonly IServiceProvider _services;

    public IdentityUow(IAppUnitOfWork uow, IServiceProvider services) : base(uow)
    {
        _services = services;
    }

    private SignInManager<User>? _signInManager;
    public SignInManager<User> SignInManager => _signInManager ??= _services.GetRequiredService<SignInManager<User>>();
    private UserManager<User>? _userManager;
    public UserManager<User> UserManager => _userManager ??= _services.GetRequiredService<UserManager<User>>();

    private UserService? _userService;
    public UserService UserService => _userService ??= _services.GetRequiredService<UserService>();

    private TokenService? _tokenService;
    public TokenService TokenService => _tokenService ??= _services.GetRequiredService<TokenService>();
}