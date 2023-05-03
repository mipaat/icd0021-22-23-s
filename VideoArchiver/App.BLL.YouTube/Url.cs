using System.Text.RegularExpressions;

namespace App.BLL.YouTube;

public static class Url
{
    private static readonly Regex VideoRegex = new(
        @"(?:https?://)?(?:(?:(?:www\.)?(?:youtube\.com)/(?:watch\?.*v=(?<id>(?:(?![&=\?])[\S]){11})))|(?:shorts/(?<id>(?:(?![&=\?])[\S]){11}))|(?:youtu\.be/(?<id>(?:(?![&=\?])[\S]){11})))");

    private static readonly Regex PlaylistRegex = new(
        @"(?:https?://)?(?:www\.)?youtube\.com/(?:(?:playlist\?list=(?<id>(?:(?![&=\?])[\S])+))|(?:watch.*list=(?<id>(?:(?![&=\?])[\S])+)))");

    private static readonly Regex AuthorHandleRegex = new(
        @"(?:https?://)?(?:www\.)youtube\.com/@(?<handle>(?:(?![&=\?/])[\S])+)");

    private static bool IsRegexUrl(string url, Regex regex, string valueGroupName, out string? value)
    {
        value = null;
        var match = regex.Match(url);
        if (!match.Success) return false;
        if (match.Groups.TryGetValue(valueGroupName, out var valueGroup))
        {
            if (valueGroup.Success)
            {
                value = valueGroup.Value;
                return true;
            }
        }

        return false;
    }

    public static bool IsYouTubeUrl(string url)
    {
        return IsPlaylistUrl(url) || IsVideoUrl(url) || IsAuthorUrl(url);
    }

    public static bool IsVideoUrl(string url) => IsVideoUrl(url, out _);

    public static bool IsVideoUrl(string url, out string? id) =>
        IsRegexUrl(url, VideoRegex, "id", out id);

    public static bool IsPlaylistUrl(string url) => IsPlaylistUrl(url, out _);

    public static bool IsPlaylistUrl(string url, out string? id) =>
        IsRegexUrl(url, PlaylistRegex, "id", out id);

    public static bool IsAuthorHandleUrl(string url, out string? handle) =>
        IsRegexUrl(url, AuthorHandleRegex, "handle", out handle);

    public static bool IsAuthorUrl(string url)
    {
        // TODO
        return false;
    }

    public static string ToVideoUrl(string id)
    {
        return $"https://www.youtube.com/watch?v={id}";
    }

    public static string ToPlaylistUrl(string id)
    {
        return $"https://www.youtube.com/playlist?list={id}";
    }
}