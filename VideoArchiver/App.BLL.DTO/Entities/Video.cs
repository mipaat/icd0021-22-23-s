using App.Common;
using App.Common.Enums;

namespace App.BLL.DTO.Entities;

public class Video
{
    public Guid Id { get; set; }
    public LangString? Title { get; set; }
    public LangString? Description { get; set; }
    public string? Url { get; set; }
    public string? EmbedUrl { get; set; }
    public bool IsAvailable { get; set; }

    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;
}