using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class StatusChangeEventRepository : BaseAppEntityRepository<App.Domain.StatusChangeEvent, StatusChangeEvent>,
    IStatusChangeEventRepository
{
    public StatusChangeEventRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}