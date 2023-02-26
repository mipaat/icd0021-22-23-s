using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Domain.Enums;
using Domain.Identity;

namespace Domain;

public class QueueItem : AbstractIdDatabaseEntity
{
    [MaxLength(4096)] public string? Url { get; set; }
    public Platform? Platform { get; set; }
    [MaxLength(64)] public string? IdOnPlatform { get; set; }

    public bool Monitor { get; set; } = true;
    public bool Download { get; set; } = true;

    [MaxLength(4096)] public string? WebHookUrl { get; set; }
    [MaxLength(512)] public string? WebhookSecret { get; set; }
    public string? WebhookData { get; set; }

    public User? AddedBy { get; set; }
    public Guid AddedById { get; set; }
    public DateTime AddedAt { get; set; }

    public User? ApprovedBy { get; set; }
    public Guid? ApprovedById { get; set; }
    public DateTime? ApprovedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
    
    public Author? Author { get; set; }
    public Guid? AuthorId { get; set; }
    public Video? Video { get; set; }
    public Guid? VideoId { get; set; }
    public Playlist? Playlist { get; set; }
    public Guid? PlaylistId { get; set; }
}