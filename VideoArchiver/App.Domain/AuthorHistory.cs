using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Domain.Enums;
using Domain.NotMapped;

namespace Domain;

public class AuthorHistory : AbstractIdDatabaseEntity
{
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }

    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public LangString? Bio { get; set; }

    public int? SubscriberCount { get; set; }

    public List<ImageFile>? ProfileImages { get; set; }
    public List<ImageFile>? Banners { get; set; }
    public List<ImageFile>? Thumbnails { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public DateTime LastValidAt { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }
}