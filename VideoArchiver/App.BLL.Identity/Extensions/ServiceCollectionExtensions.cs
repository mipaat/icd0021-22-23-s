using App.BLL.Identity.Services;
using App.DAL.EF;
using App.Domain.Identity;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AbstractAppDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
        serviceCollection.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
        serviceCollection.AddScoped<UserService>();
        serviceCollection.AddScoped<TokenService>();
        serviceCollection.AddScoped<IdentityUow>();
        return serviceCollection;
    }
}