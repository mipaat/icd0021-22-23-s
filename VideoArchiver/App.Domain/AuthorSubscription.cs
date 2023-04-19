using App.Domain.Enums;
using Domain.Base;

namespace App.Domain;

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