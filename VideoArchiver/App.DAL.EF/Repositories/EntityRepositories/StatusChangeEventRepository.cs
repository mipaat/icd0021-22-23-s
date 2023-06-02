using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class StatusChangeEventRepository : BaseAppEntityRepository<App.Domain.StatusChangeEvent, StatusChangeEvent>,
    IStatusChangeEventRepository
{
    public StatusChangeEventRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }
}