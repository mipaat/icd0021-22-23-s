using DAL.Repositories.EntityRepositories;

namespace DAL;

public class RepositoryContext : IDisposable, IAsyncDisposable
{
    public readonly AbstractAppDbContext DbContext;

    public RepositoryContext(AbstractAppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public AuthorRepository Authors => new(DbContext);

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
}