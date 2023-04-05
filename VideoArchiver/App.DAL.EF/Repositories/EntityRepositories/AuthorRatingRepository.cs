using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class AuthorRatingRepository : BaseEntityRepository<AuthorRating, AbstractAppDbContext>, IAuthorRatingRepository
{
    public AuthorRatingRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<AuthorRating> DefaultIncludes(DbSet<AuthorRating> entities)
    {
        entities
            .Include(ar => ar.Category)
            .Include(ar => ar.Rated)
            .Include(ar => ar.Rater);
        return entities;
    }
}