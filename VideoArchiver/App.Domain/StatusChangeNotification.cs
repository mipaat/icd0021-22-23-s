using App.Domain.Identity;
using Domain.Base;

namespace App.Domain;

public class StatusChangeNotification : AbstractIdDatabaseEntity
{
    public User? Receiver { get; set; }
    public Guid ReceiverId { get; set; }
    public StatusChangeEvent? StatusChangeEvent { get; set; }
    public Guid StatusChangeEventId { get; set; }

    public DateTime SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}