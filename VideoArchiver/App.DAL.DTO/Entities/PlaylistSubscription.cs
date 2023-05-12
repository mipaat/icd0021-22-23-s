using Domain.Base;

namespace App.DAL.DTO.Entities;

public class PlaylistSubscription : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public Author? Subscriber { get; set; }
    public Guid SubscriberId { get; set; }

    public int Priority { get; set; }
}