using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class ExternalUserTokenRepository : BaseEntityRepository<ExternalUserToken, AbstractAppDbContext>,
    IExternalUserTokenRepository
{
    public ExternalUserTokenRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<ExternalUserToken> DefaultIncludes(DbSet<ExternalUserToken> entities)
    {
        entities
            .Include(e => e.Author)
            .Include(e => e.User);
        return entities;
    }
}