namespace App.BLL.YouTube;

public class YouTubeSettings
{
    public const string SectionKey = "YouTube";
    
    public string? ApplicationName { get; set; }
    public string ApiKey { get; set; } = default!;
}