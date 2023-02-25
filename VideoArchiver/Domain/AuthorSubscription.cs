using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Domain.Enums;

namespace Domain;

public class AuthorSubscription : AbstractIdDatabaseEntity
{
    [MaxLength(64)] public Platform Platform { get; set; } = default!;
    public Author? Subscriber { get; set; }
    public Guid SubscriberId { get; set; }
    public Author? SubscriptionTarget { get; set; }
    public Guid SubscriptionTargetId { get; set; }

    public DateTime? LastFetched { get; set; }
    public int Priority { get; set; }
}