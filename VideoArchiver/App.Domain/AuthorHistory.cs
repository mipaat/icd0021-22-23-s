using System.ComponentModel.DataAnnotations;
using App.Domain.NotMapped;
using Domain.Base;

namespace App.Domain;

public class AuthorHistory : AbstractIdDatabaseEntity
{
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }

    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public LangString? Bio { get; set; }

    public int? SubscriberCount { get; set; }

    public ImageFileList? ProfileImages { get; set; }
    public ImageFileList? Banners { get; set; }
    public ImageFileList? Thumbnails { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public DateTime LastValidAt { get; set; }
}