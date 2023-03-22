using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class AuthorRepository : AbstractEntityRepository<Author, AbstractAppDbContext>
{
    public AuthorRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}