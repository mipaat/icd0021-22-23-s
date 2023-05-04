using Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base;

public class BaseUnitOfWork<TDbContext> : IBaseUnitOfWork where TDbContext : DbContext
{
    public readonly TDbContext DbContext;

    public BaseUnitOfWork(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public int SaveChanges()
    {
        var result = DbContext.SaveChanges();
        SuccessfullySaved?.Invoke(null, EventArgs.Empty);
        return result;
    }

    public async Task<int> SaveChangesAsync()
    {
        var result = await DbContext.SaveChangesAsync();
        SuccessfullySaved?.Invoke(null, EventArgs.Empty);
        return result;
    }

    public event EventHandler? SuccessfullySaved;
}