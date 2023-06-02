using System.Collections;

namespace Base.Tests;

public abstract class TestDataGenerator : IEnumerable<object[]>
{
    protected abstract List<object[]> Data { get; }
    public IEnumerator<object[]> GetEnumerator() => Data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}