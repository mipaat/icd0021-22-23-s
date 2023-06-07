using System.Security.Claims;
using App.Common;

namespace Tests.Helpers;

public static class TestUsers
{
    public static readonly ClaimsPrincipal EmptyUser = new(new[]
    {
        new ClaimsIdentity()
    });
    
    public static readonly ClaimsPrincipal AdminUser = new(new[]
    {
        new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, RoleNames.Admin),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        })
    });

    public static readonly ClaimsPrincipal SuperAdminUser = new(new[]
    {
        new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, RoleNames.SuperAdmin),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        })
    });

    public static readonly ClaimsPrincipal HelperUser = new(new[]
    {
        new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, RoleNames.Helper),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        })
    });

    public static readonly ClaimsPrincipal NoRoleUser = new(new[]
    {
        new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, RoleNames.Helper),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        })
    });
}