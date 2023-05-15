using Domain.Base;

namespace App.DAL.DTO.Entities.Playlists;

public class BasicPlaylistVideo : IIdDatabaseEntity
{
    public Guid Id { get; set; }
    public BasicVideoData Video { get; set; } = default!;
    
    public int Position { get; set; }
    
    public DateTime? AddedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}