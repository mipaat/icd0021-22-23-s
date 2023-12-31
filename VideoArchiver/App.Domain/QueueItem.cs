using System.ComponentModel.DataAnnotations;
using App.Common.Enums;
using App.Domain.Identity;
using Domain.Base;

namespace App.Domain;

public class QueueItem : AbstractIdDatabaseEntity
{
    public EPlatform Platform { get; set; }
    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;
    public EEntityType EntityType { get; set; }

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
    public bool GrantAccess { get; set; } = true;

    public DateTime? CompletedAt { get; set; }

    public Author? Author { get; set; }
    public Guid? AuthorId { get; set; }
    public Video? Video { get; set; }
    public Guid? VideoId { get; set; }
    public Playlist? Playlist { get; set; }
    public Guid? PlaylistId { get; set; }

    public QueueItem()
    {
    }

    public QueueItem(string id, Guid submitterId, bool autoSubmit, EPlatform platform)
    {
        Platform = platform;
        IdOnPlatform = id;

        AddedById = submitterId;
        AddedAt = DateTime.UtcNow;

        ApprovedById = autoSubmit ? submitterId : null;
        ApprovedAt = autoSubmit ? DateTime.UtcNow : null;
    }

    public QueueItem(Guid submitterId, bool autoSubmit, Video video) :
        this(video.IdOnPlatform, submitterId, autoSubmit, video.Platform)
    {
        Video = video;
        VideoId = video.Id;

        CompletedAt = DateTime.UtcNow;
    }

    public QueueItem(Guid submitterId, bool autoSubmit, Author author) :
        this(author.IdOnPlatform, submitterId, autoSubmit, author.Platform)
    {
        Author = author;
        AuthorId = author.Id;

        CompletedAt = DateTime.UtcNow;
    }

    public QueueItem(Guid submitterId, bool autoSubmit, Playlist playlist) :
        this(playlist.IdOnPlatform, submitterId, autoSubmit, playlist.Platform)
    {
        Playlist = playlist;
        PlaylistId = playlist.Id;

        CompletedAt = DateTime.UtcNow;
    }
}