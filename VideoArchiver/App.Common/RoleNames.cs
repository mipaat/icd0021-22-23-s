namespace App.Common;

public static class RoleNames
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Helper = "Helper";

    public static readonly string[] AllAsList = {
        SuperAdmin,
        Admin,
        Helper
    };
}