namespace Contracts.DAL;

public interface IBaseUnitOfWork : IDisposable, IAsyncDisposable
{
    public int SaveChanges();
    public Task<int> SaveChangesAsync();

    public event EventHandler SuccessfullySaved;
}