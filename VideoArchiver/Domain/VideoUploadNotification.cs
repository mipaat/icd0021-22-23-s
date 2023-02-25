using Domain.Base;

namespace Domain;

public class VideoUploadNotification : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    public Author? Receiver { get; set; }
    public Guid ReceiverId { get; set; }

    public int Priority { get; set; }

    public DateTime SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}