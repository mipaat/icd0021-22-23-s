using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using Base.BLL;

namespace App.BLL.Services;

public class BasicGameCrudService : BaseCrudService<IAppUnitOfWork, IGameRepository, Domain.Game, App.DAL.DTO.Entities.Game, Game, GameMapper>
{
    public BasicGameCrudService(IAppUnitOfWork uow, GameMapper mapper) : base(uow, mapper)
    {
    }

    protected override IGameRepository Repository => Uow.Games;

    public async Task UpdateAsync(Game entity)
    {
        await UpdateAsync(entity.Id, entity);
    }
}