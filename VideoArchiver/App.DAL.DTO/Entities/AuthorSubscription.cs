using App.DAL.DTO.Enums;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class AuthorSubscription : AbstractIdDatabaseEntity
{
    public Platform Platform { get; set; } = default!;
    public Author? Subscriber { get; set; }
    public Guid SubscriberId { get; set; }
    public Author? SubscriptionTarget { get; set; }
    public Guid SubscriptionTargetId { get; set; }

    public DateTime? LastFetched { get; set; }
    public int Priority { get; set; }
}