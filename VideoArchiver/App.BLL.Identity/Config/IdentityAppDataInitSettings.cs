using Microsoft.Extensions.Configuration;

namespace App.BLL.Identity.Config;

public class IdentityAppDataInitSettings
{
    public const string SectionKey = "IdentityInit";

    public bool SeedIdentity { get; set; } = true;
    public bool SeedDemoIdentity { get; set; }
    public string? AdminPassword { get; set; }
    public string? SuperAdminPassword { get; set; }
}

public static class ConfigurationExtensions {
    public static IdentityAppDataInitSettings GetIdentityAppDataInitSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(IdentityAppDataInitSettings.SectionKey).Get<IdentityAppDataInitSettings>() ?? new IdentityAppDataInitSettings();
    }
}