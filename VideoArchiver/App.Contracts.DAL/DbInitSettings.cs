using Microsoft.Extensions.Configuration;

namespace App.Contracts.DAL;

public class DbInitSettings
{
    public const string SectionKey = "DatabaseInit";
    
    public bool Migrate { get; set; }
    public bool EnsureCreated { get; set; } = true;
    public bool DropDatabase { get; set; }
}

public static class ConfigurationExtensions
{
    public static DbInitSettings GetDbInitSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(DbInitSettings.SectionKey).Get<DbInitSettings>() ?? new DbInitSettings();
    }
}