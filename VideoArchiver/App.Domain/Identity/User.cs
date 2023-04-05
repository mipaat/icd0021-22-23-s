using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class User : IdentityUser<Guid>, IIdDatabaseEntity
{
    public ICollection<Author>? Authors { get; set; }
    [InverseProperty(nameof(QueueItem.AddedBy))]
    public ICollection<QueueItem>? AddedQueueItems { get; set; }
    [InverseProperty(nameof(QueueItem.ApprovedBy))]
    public ICollection<QueueItem>? ApprovedQueueItems { get; set; }
    public ICollection<ExternalUserToken>? ExternalUserTokens { get; set; }
    public ICollection<StatusChangeNotification>? StatusChangeNotifications { get; set; }
}