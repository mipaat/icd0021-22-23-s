namespace Contracts.BLL;

public interface IAppUowContainer : IDisposable, IAsyncDisposable
{
    public Task<int> SaveChangesAsync();
}