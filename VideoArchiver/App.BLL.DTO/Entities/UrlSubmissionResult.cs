namespace App.BLL.DTO.Entities;

public class UrlSubmissionResult
{
    public readonly QueueItem? QueueItem;
    public readonly Entity? Entity;

    public bool AlreadyAdded = false;

    private UrlSubmissionResult(QueueItem queueItem)
    {
        QueueItem = queueItem;
    }

    private UrlSubmissionResult(Entity entity)
    {
        Entity = entity;
    }

    public static implicit operator UrlSubmissionResult(QueueItem queueItem) => new(queueItem);
    public static implicit operator UrlSubmissionResult(Entity entity) => new(entity);
    public static implicit operator UrlSubmissionResult(Video entity) => new(entity);
    public static implicit operator UrlSubmissionResult(Author entity) => new(entity);
    public static implicit operator UrlSubmissionResult(Playlist entity) => new(entity);
}