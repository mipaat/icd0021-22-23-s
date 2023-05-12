using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class StatusChangeEventRepository : BaseAppEntityRepository<App.Domain.StatusChangeEvent, StatusChangeEvent>,
    IStatusChangeEventRepository
{
    public StatusChangeEventRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}