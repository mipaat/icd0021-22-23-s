using Domain.Base;

namespace App.DAL.DTO.Entities.Playlists;

public class PlaylistVideo : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }

    public int Position { get; set; }

    public DateTime? AddedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}