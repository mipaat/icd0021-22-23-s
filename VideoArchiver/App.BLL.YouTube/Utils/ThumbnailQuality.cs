namespace App.BLL.YouTube.Utils;

public record ThumbnailQuality(string Name, string ShortName, int Width, int Height)
{
    public static readonly ThumbnailQuality Default = new("", "", 120, 90);
    public static readonly ThumbnailQuality Medium = new("medium", "mq", 320, 180);
    public static readonly ThumbnailQuality High = new("high", "hq", 480, 360);
    public static readonly ThumbnailQuality Standard = new("standard", "sd", 640, 480);
    public static readonly ThumbnailQuality MaxRes = new("maxres", "maxres", 1280, 720);

    public static readonly IReadOnlyCollection<ThumbnailQuality> AllQualities = new[]
    {
        Default,
        Medium,
        High,
        Standard,
        MaxRes,
    };

    public static readonly IReadOnlyCollection<ThumbnailQuality> DefaultQualities = new[]
    {
        Default,
        Medium,
        High,
    };
}