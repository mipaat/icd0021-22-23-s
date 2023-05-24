using App.Common;
using App.Common.Enums;
using Domain.Base;

namespace App.BLL.DTO.Entities;

public class BasicVideoWithAuthor : AbstractIdDatabaseEntity
{
    public LangString? Title { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus? PrivacyStatus { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }

    public ImageFileList? Thumbnails { get; set; }
    public ImageFile? Thumbnail { get; set; }
    
    public TimeSpan? Duration { get; set; }

    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;

    public Author Author { get; set; } = default!;
}