using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Entities.Identity;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;
using Domain.Base;

namespace App.DAL.DTO.Entities;

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

    public User AddedBy { get; set; } = default!;
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

    public QueueItem()
    {
    }

    public QueueItem(string idOnPlatform, EPlatform platform, EEntityType entityType, Guid submitterId, bool autoSubmit)
    {
        Platform = platform;
        IdOnPlatform = idOnPlatform;
        EntityType = entityType;

        AddedById = submitterId;
        AddedAt = DateTime.UtcNow;

        ApprovedById = autoSubmit ? submitterId : null;
        ApprovedAt = autoSubmit ? DateTime.UtcNow : null;
    }

    public QueueItem(Video video, Guid submitterId, bool autoSubmit) :
        this(video.IdOnPlatform, video.Platform, EEntityType.Video, submitterId, autoSubmit)
    {
        Video = video;
        VideoId = video.Id;

        CompletedAt = DateTime.UtcNow;
    }

    public QueueItem(Playlist playlist, Guid submitterId, bool autoSubmit) :
        this(playlist.IdOnPlatform, playlist.Platform, EEntityType.Playlist, submitterId, autoSubmit)
    {
        Playlist = playlist;
        PlaylistId = playlist.Id;

        CompletedAt = DateTime.UtcNow;
    }

    public QueueItem(Author author, Guid submitterId, bool autoSubmit) :
        this(author.IdOnPlatform, author.Platform, EEntityType.Author, submitterId, autoSubmit)
    {
        Author = author;
        AuthorId = author.Id;

        CompletedAt = DateTime.UtcNow;
    }
}