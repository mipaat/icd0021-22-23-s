using System.Threading.Channels;

namespace Utils;

public class BackgroundTaskQueue<T>
{
    private readonly Channel<Func<CancellationToken, T>> _queue;

    public BackgroundTaskQueue(int capacity)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, T>>(options);
    }

    public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, T> workItem)
    {
        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, T> workItem, CancellationToken cancellationToken)
    {
        await _queue.Writer.WriteAsync(workItem, cancellationToken);
    }

    public async ValueTask<Func<CancellationToken, T>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
