namespace Utils;

public class UniqueQueue<T>
{
    private readonly HashSet<T> _hashSet;
    private readonly Queue<T> _queue;

    public event EventHandler<T>? NewItemEnqueued;

    public UniqueQueue()
    {
        _hashSet = new HashSet<T>();
        _queue = new Queue<T>();
    }

    public int Count => _hashSet.Count;
    public bool IsEmpty => Count == 0;

    public void Clear()
    {
        _hashSet.Clear();
        _queue.Clear();
    }

    public bool Contains(T item)
    {
        return _hashSet.Contains(item);
    }

    public void Enqueue(T item)
    {
        if (_hashSet.Add(item))
        {
            NewItemEnqueued?.Invoke(null, item);
            _queue.Enqueue(item);
        }
    }

    public T Dequeue()
    {
        var item = _queue.Dequeue();
        _hashSet.Remove(item);
        return item;
    }

    public bool TryDequeue(out T? result)
    {
        if (_queue.TryDequeue(out result))
        {
            _hashSet.Remove(result);
            return true;
        }

        return false;
    }

    public T Peek()
    {
        return _queue.Peek();
    }

    public bool TryPeek(out T? result)
    {
        return _queue.TryPeek(out result);
    }
}