using App.Common.Enums;
using App.Common;

namespace App.BLL.YouTube.Utils;

public static class ThumbnailUtils
{
    public const string Default = "default";
    public const string Alt1 = "1";
    public const string Alt2 = "2";
    public const string Alt3 = "3";

    public static readonly IReadOnlyCollection<string> AllTags = new[]
    {
        Default,
        Alt1,
        Alt2,
        Alt3,
    };

    public static ImageFileList GetAllPotentialThumbnails(string videoId)
    {
        return new ImageFileList(
            AllTags.SelectMany(t => GetByTagAndQualities(videoId, ThumbnailQuality.AllQualities, t)));
    }

    public static IEnumerable<ImageFile> GetByTagAndQualities(string videoId, IEnumerable<ThumbnailQuality> qualities,
        string tag = Default) => qualities.Select(q => GetByTagAndQuality(videoId, q, tag));

    public static ImageFile GetByTagAndQuality(string videoId, ThumbnailQuality quality, string tag = Default) =>
        new()
        {
            Url = $"https://i.ytimg.com/vi/{videoId}/{quality.ShortName}{tag}.jpg",
            Platform = EPlatform.YouTube,
            Width = quality.Width,
            Height = quality.Height,
            Key = tag,
            Quality = quality.Name,
        };
}