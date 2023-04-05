using App.Contracts.DAL.Repositories.EntityRepositories;
using Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IBaseUnitOfWork
{
    public IAuthorRepository Authors { get; }
}