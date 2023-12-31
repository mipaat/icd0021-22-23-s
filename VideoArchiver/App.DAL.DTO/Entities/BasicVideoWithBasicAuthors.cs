using App.Common;
using App.Common.Enums;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class BasicVideoWithBasicAuthors : AbstractIdDatabaseEntity
{
    public LangString? Title { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus? PrivacyStatus { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }

    public ImageFileList? Thumbnails { get; set; } = default!;

    public TimeSpan? Duration { get; set; }
    
    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;

    public ICollection<AuthorBasic> Authors { get; set; } = default!;

    public DateTime? CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime AddedToArchiveAt { get; set; }
}