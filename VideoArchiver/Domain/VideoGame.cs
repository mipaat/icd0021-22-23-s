using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Domain.Enums;

namespace Domain;

public class VideoGame : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    public Game? Game { get; set; }
    public Guid? GameId { get; set; }

    [MaxLength(16)] public string? IgdbId { get; set; }
    [MaxLength(64)] public Platform? Platform { get; set; } = default!;
    [MaxLength(64)] public string? IdOnPlatform { get; set; } = default!;

    [MaxLength(512)] public string? Name { get; set; }
    [MaxLength(4096)] public string? BoxArtUrl { get; set; }

    public TimeSpan? FromTimecode { get; set; }
    public TimeSpan? ToTimecode { get; set; }

    public DateTime? ValidSince { get; set; }
    public DateTime? ValidUntil { get; set; }
}