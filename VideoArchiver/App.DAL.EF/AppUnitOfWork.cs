using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using DAL.Repositories.EntityRepositories;

namespace DAL;

public class AppUnitOfWork : BaseUnitOfWork<AbstractAppDbContext>, IAppUnitOfWork
{
    public AppUnitOfWork(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    // TODO: Replace repository return types when C# supports return type covariance for interfaces

    private IAuthorRepository? _authors;
    public IAuthorRepository Authors => _authors ??= new AuthorRepository(DbContext);
}