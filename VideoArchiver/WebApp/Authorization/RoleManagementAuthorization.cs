using System.Security.Claims;
using App.Common;

#pragma warning disable 1591

namespace WebApp.Authorization;

public static class RoleManagementAuthorization
{
    public static bool IsAllowedToManageRole(this ClaimsPrincipal user, string roleName)
    {
        if (roleName == RoleNames.SuperAdmin) return false;
        if (user.IsInRole(RoleNames.SuperAdmin)) return true;
        return user.IsInRole(RoleNames.Admin) && roleName != RoleNames.Admin;
    }
}