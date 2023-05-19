using System.Globalization;
using Base.WebHelpers;
using Microsoft.Extensions.Configuration;

namespace App.BLL.Config;

public static class ConfigurationExtensions
{
    public static ICollection<string> GetSupportedUiCultureNames(this IConfiguration configuration)
    {
        return configuration.GetSection("SupportedUICultures").Get<ICollection<string>>() ?? new List<string>
        {
            "en-US"
        };
    }

    public static List<CultureInfo> GetSupportedUiCultures(this IConfiguration configuration)
    {
        return configuration.GetSupportedUiCultureNames()
            .Select(n => CultureInfo.GetCultureInfo(n).UseConstantDateTime()).ToList();
    }
}