using Domain.Base;

namespace App.DAL.DTO.Entities.Playlists;

public class PlaylistVideoPositionHistory : AbstractIdDatabaseEntity
{
    public BasicPlaylistVideo PlaylistVideo { get; set; } = default!;
    public int Position { get; set; }

    public DateTime? ValidSince { get; set; }
    public DateTime ValidUntil { get; set; }
}