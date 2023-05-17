namespace App.BLL;

public class ServiceContext
{
    public event EventHandler<Guid>? NewQueueItemApproved;
    public void QueueNewQueueItem(Guid queueItemId) => Task.Run(() => NewQueueItemApproved?.Invoke(null, queueItemId));
}