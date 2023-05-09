using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Domain;

/// <summary>
/// Data for a Game entity
/// </summary>
public class GameWithoutId
{
    /// <summary>
    /// The game's ID in IGDB (Internet Game Database)
    /// </summary>
    [MaxLength(16)] public string IgdbId { get; set; } = default!;
    /// <summary>
    /// The game's name
    /// </summary>
    [MaxLength(512)] public string Name { get; set; } = default!;
    /// <summary>
    /// URL for the game's box-art
    /// </summary>
    [MaxLength(4096)] public string? BoxArtUrl { get; set; }
    
    /// <summary>
    /// String identifying this version of the game's data
    /// </summary>
    [MaxLength(4096)] public string? Etag { get; set; }
    /// <summary>
    /// When the game's data was last attempted to be fetched from IGDB (in UTC)
    /// </summary>
    public DateTime LastFetched { get; set; }
    /// <summary>
    /// When the game's data was last successfully fetched from IGDB (in UTC)
    /// </summary>
    public DateTime? LastSuccessfulFetch { get; set; }
}