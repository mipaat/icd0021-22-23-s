using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Entities.Identity;
using App.BLL.DTO.Enums;
using Domain.Base;

namespace App.BLL.DTO.Entities;

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

    public DAL.DTO.Entities.Author? Author { get; set; }
    public Guid? AuthorId { get; set; }
    public Video? Video { get; set; }
    public Guid? VideoId { get; set; }
    public Playlist? Playlist { get; set; }
    public Guid? PlaylistId { get; set; }

    public QueueItem()
    {
    }

    public QueueItem(string id, Guid submitterId, bool autoSubmit, Platform platform)
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

    public QueueItem(Guid submitterId, bool autoSubmit, DAL.DTO.Entities.Author author) :
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