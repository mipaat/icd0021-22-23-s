using App.Common.Enums;
using App.DAL.DTO.Entities;

namespace App.BLL.DTO.Entities;

public class UrlSubmissionResult
{
    public Guid QueueItemId { get; set; }
    public EEntityType Type { get; set; }
    public Guid? EntityId { get; set; }
    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; }
    public bool AlreadyAdded { get; set; }

    public UrlSubmissionResult(QueueItem queueItem, bool alreadyAdded)
    {
        QueueItemId = queueItem.Id;
        Type = queueItem.EntityType;
        EntityId = queueItem.EntityType switch
        {
            EEntityType.Video => queueItem.VideoId,
            EEntityType.Playlist => queueItem.PlaylistId,
            EEntityType.Author => queueItem.AuthorId,
            _ => throw new ArgumentOutOfRangeException(),
        };
        Platform = queueItem.Platform;
        IdOnPlatform = queueItem.IdOnPlatform;
        AlreadyAdded = alreadyAdded;
    }
}