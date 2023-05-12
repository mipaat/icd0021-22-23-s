using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class StatusChangeEventRepository : BaseAppEntityRepository<App.Domain.StatusChangeEvent, StatusChangeEvent>,
    IStatusChangeEventRepository
{
    public StatusChangeEventRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}