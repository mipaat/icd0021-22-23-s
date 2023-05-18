using Microsoft.Extensions.Configuration;

namespace App.BLL.Config;

public static class ConfigurationExtensions
{
    public static ICollection<string> GetSupportedUiCultures(this IConfiguration configuration)
    {
        return configuration.GetSection("SupportedUICultures").Get<ICollection<string>>() ?? new List<string>
        {
            "en-US"
        };
    }
}