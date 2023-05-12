using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class CommentHistory : AbstractIdDatabaseEntity
{
    public Comment? Comment { get; set; }
    public Guid CommentId { get; set; }

    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public string? Content { get; set; }
    public int? LikeCount { get; set; }
    public int? DislikeCount { get; set; }
    public int? ReplyCount { get; set; }
    public bool? IsFavorited { get; set; }

    public DateTime LastValidAt { get; set; }
}