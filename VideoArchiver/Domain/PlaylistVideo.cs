using Domain.Base;

namespace Domain;

public class PlaylistVideo : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }

    public int Position { get; set; }

    public DateTime? AddedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
    public Author? AddedBy { get; set; }
    public Guid? AddedById { get; set; }
    public Author? RemovedBy { get; set; }
    public Guid? RemovedById { get; set; }

    public ICollection<PlaylistVideoPositionHistory>? PositionHistories { get; set; }
}