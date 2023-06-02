namespace App.BLL.Contracts;

public interface IServiceContext
{
    public event EventHandler<Guid>? NewQueueItemApproved;
    public void QueueNewQueueItem(Guid queueItemId);
}