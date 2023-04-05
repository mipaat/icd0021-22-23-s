using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain;

public class Game : AbstractIdDatabaseEntity
{
    [MaxLength(16)] public string IgdbId { get; set; } = default!;
    [MaxLength(512)] public string Name { get; set; } = default!;
    [MaxLength(4096)] public string? BoxArtUrl { get; set; }
    
    [MaxLength(4096)] public string? Etag { get; set; }
    public DateTime LastFetched { get; set; }
    public DateTime? LastSuccessfulFetch { get; set; }

    public ICollection<VideoGame>? VideoGames { get; set; }
}