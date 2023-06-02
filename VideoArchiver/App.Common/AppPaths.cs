using App.Common.Enums;
using Microsoft.Extensions.Configuration;

namespace App.Common;

public static class AppPaths
{
    private const string DownloadsDirectory = "downloads";

    public const string DownloadsPathConfigKey = "DownloadsPath";
    public static string GetDownloadsDirectory(IConfiguration? config = null)
    {
        return Path.Combine(config?.GetValue<string>(DownloadsPathConfigKey) ?? DownloadsDirectory);
    }

    public const string Thumbnails = "thumbnails";
    public const string ProfileImages = "profile_images";
    public const string Videos = "videos";

    public static string GetThumbnailsDirectory(EPlatform platform, IConfiguration? config = null) =>
        Path.Combine(GetDownloadsDirectory(config), Thumbnails, platform.ToString());
    public static string GetProfileImagesDirectory(EPlatform platform, IConfiguration? config = null) =>
        Path.Combine(GetDownloadsDirectory(config), ProfileImages, platform.ToString());
    public static string GetVideosDirectory(EPlatform platform, IConfiguration? config = null) =>
        Path.Combine(GetDownloadsDirectory(config), Videos, platform.ToString());
}