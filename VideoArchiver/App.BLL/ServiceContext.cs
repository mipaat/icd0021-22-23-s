using App.BLL.Contracts;

namespace App.BLL;

public class ServiceContext : IServiceContext
{
    public event EventHandler<Guid>? NewQueueItemApproved;
    public void QueueNewQueueItem(Guid queueItemId) => Task.Run(() => NewQueueItemApproved?.Invoke(null, queueItemId));
}