using Domain.Base;

namespace Domain;

public class CommentReplyNotification : AbstractIdDatabaseEntity
{
    public Comment? Reply { get; set; }
    public Guid ReplyId { get; set; }
    public Comment? Comment { get; set; }
    public Guid CommentId { get; set; }
    public Author? Receiver { get; set; }
    public Guid ReceiverId { get; set; }

    public DateTime SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}