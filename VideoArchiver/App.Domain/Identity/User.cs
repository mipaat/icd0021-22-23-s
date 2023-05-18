using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class User : IdentityUser<Guid>, IIdDatabaseEntity
{
    public bool IsApproved { get; set; }

    public ICollection<UserRole>? UserRoles { get; set; }
    public ICollection<Author>? Authors { get; set; }
    [InverseProperty(nameof(QueueItem.AddedBy))]
    public ICollection<QueueItem>? AddedQueueItems { get; set; }
    [InverseProperty(nameof(QueueItem.ApprovedBy))]
    public ICollection<QueueItem>? ApprovedQueueItems { get; set; }
    public ICollection<ExternalUserToken>? ExternalUserTokens { get; set; }
    public ICollection<StatusChangeNotification>? StatusChangeNotifications { get; set; }

    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}