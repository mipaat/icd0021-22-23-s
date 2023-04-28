using System.Text.RegularExpressions;

namespace App.BLL.YouTube;

public static class Url
{
    private static readonly Regex VideoRegex = new(
        @"(?:https?://)?(?:(?:(?:www\.)?(?:youtube\.com)/watch\?.*v=(?<id>(?:(?![&=\?])[\S]){11}))|(?:youtu\.be/(?<id>(?:(?![&=\?])[\S]){11})))");

    private static readonly Regex PlaylistRegex = new(
        @"(?:https?://)?(?:www\.)?youtube\.com/(?:(?:playlist\?list=(?<id>(?:(?![&=\?])[\S])+))|(?:watch.*list=(?<id>(?:(?![&=\?])[\S])+)))");

    public static bool IsYouTubeUrl(string url)
    {
        return IsPlaylistUrl(url) || IsVideoUrl(url) || IsAuthorUrl(url);
    }

    public static bool IsVideoUrl(string url) => IsVideoUrl(url, out _);

    public static bool IsVideoUrl(string url, out string? id)
    {
        id = null;
        var match = VideoRegex.Match(url);
        if (!match.Success) return false;
        if (match.Groups.TryGetValue("id", out var idGroup))
        {
            if (idGroup.Success)
            {
                id = idGroup.Value;
                return true;
            }
        }

        return false;
    }

    public static bool IsPlaylistUrl(string url) => IsPlaylistUrl(url, out _);

    public static bool IsPlaylistUrl(string url, out string? id)
    {
        id = null;
        var match = PlaylistRegex.Match(url);
        if (!match.Success) return false;
        if (match.Groups.TryGetValue("id", out var idGroup))
        {
            if (idGroup.Success)
            {
                id = idGroup.Value;
                return true;
            }
        }

        return false;
    }

    public static bool IsAuthorUrl(string url)
    {
        // TODO
        return false;
    }
}