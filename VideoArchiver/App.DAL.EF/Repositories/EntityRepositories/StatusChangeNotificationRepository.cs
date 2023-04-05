using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class StatusChangeNotificationRepository : BaseEntityRepository<StatusChangeNotification, AbstractAppDbContext>,
    IStatusChangeNotificationRepository
{
    public StatusChangeNotificationRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<StatusChangeNotification> DefaultIncludes(DbSet<StatusChangeNotification> entities)
    {
        entities
            .Include(s => s.Receiver)
            .Include(s => s.StatusChangeEvent);
        return entities;
    }
}