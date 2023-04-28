using App.Domain;

namespace App.DTO;

public class EntityOrQueueItem
{
    public readonly QueueItem? QueueItem;
    public readonly Entity? Entity;

    public bool AlreadyAdded = false;

    private EntityOrQueueItem(QueueItem queueItem)
    {
        QueueItem = queueItem;
    }

    private EntityOrQueueItem(Entity entity)
    {
        Entity = entity;
    }

    public static implicit operator EntityOrQueueItem(QueueItem queueItem) => new(queueItem);
    public static implicit operator EntityOrQueueItem(Entity entity) => new(entity);
    public static implicit operator EntityOrQueueItem(Video entity) => new(entity);
    public static implicit operator EntityOrQueueItem(Author entity) => new(entity);
    public static implicit operator EntityOrQueueItem(Playlist entity) => new(entity);
}