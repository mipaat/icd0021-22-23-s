using Domain.Base;

namespace App.Domain;

public class PlaylistVideoPositionHistory : AbstractIdDatabaseEntity
{
    public PlaylistVideo? PlaylistVideo { get; set; }
    public Guid PlaylistVideoId { get; set; }
    public int Position { get; set; }

    public DateTime? ValidSince { get; set; }
    public DateTime ValidUntil { get; set; }
}