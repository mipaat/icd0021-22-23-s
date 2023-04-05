using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class AuthorRepository : BaseEntityRepository<Author, AbstractAppDbContext>, IAuthorRepository
{
    public AuthorRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}