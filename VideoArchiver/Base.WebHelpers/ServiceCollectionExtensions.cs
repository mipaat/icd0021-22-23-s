using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Base.WebHelpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection DisableApiErrorRedirects(this IServiceCollection services,
        string apiPrefix = "/api")
    {
        services.ConfigureApplicationCookie(options =>
        {
            var oldRedirectToLogin = options.Events.OnRedirectToLogin;
            options.Events.OnRedirectToLogin = async context =>
            {
                if (context.Request.Path.StartsWithSegments(apiPrefix))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                await oldRedirectToLogin(context);
            };

            var oldRedirectToAccessDenied = options.Events.OnRedirectToAccessDenied;
            options.Events.OnRedirectToAccessDenied = async context =>
            {
                if (context.Request.Path.StartsWithSegments(apiPrefix))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }

                await oldRedirectToAccessDenied(context);
            };
        });

        return services;
    }
}