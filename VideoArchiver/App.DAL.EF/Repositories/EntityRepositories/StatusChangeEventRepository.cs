using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class StatusChangeEventRepository : BaseEntityRepository<StatusChangeEvent, AbstractAppDbContext>,
    IStatusChangeEventRepository
{
    public StatusChangeEventRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<StatusChangeEvent> DefaultIncludes(DbSet<StatusChangeEvent> entities)
    {
        entities
            .Include(s => s.Author)
            .Include(s => s.Playlist)
            .Include(s => s.Video);
        return entities;
    }
}