using App.Common.Enums;

namespace App.Common;

public static class AppPaths
{
    public const string DownloadsDirectory = "downloads";
    public static readonly string ThumbnailsDirectory = Path.Combine(DownloadsDirectory, "thumbnails");
    public static readonly string ProfileImagesDirectory = Path.Combine(DownloadsDirectory, "profile_images");
    public static readonly string VideosDirectory = Path.Combine(DownloadsDirectory, "videos");

    public static string GetThumbnailsDirectory(EPlatform platform) =>
        Path.Combine(ThumbnailsDirectory, platform.ToString());
    public static string GetProfileImagesDirectory(EPlatform platform) =>
        Path.Combine(ProfileImagesDirectory, platform.ToString());
    public static string GetVideosDirectory(EPlatform platform) =>
        Path.Combine(VideosDirectory, platform.ToString());
}