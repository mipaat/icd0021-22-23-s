using System.ComponentModel.DataAnnotations;
using App.Common;
using App.Common.Enums;
using Domain.Base;

namespace App.Domain;

public class PlaylistHistory : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }

    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public LangString? Title { get; set; }
    public LangString? Description { get; set; }

    public ImageFileList? Thumbnails { get; set; }
    public List<string>? Tags { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime LastValidAt { get; set; }
}