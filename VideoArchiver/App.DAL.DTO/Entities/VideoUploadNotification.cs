using Domain.Base;

namespace App.DAL.DTO.Entities;

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