using Base.BLL.Exceptions;
using Contracts.BLL;
using Contracts.DAL;
using Domain.Base;

namespace Base.BLL;

public abstract class BaseCrudService<TAppUow, TRepository, TDomainEntity, TDalEntity, TBllEntity, TMapper> :
    BaseCrudService<TAppUow, TRepository, TDomainEntity, TDalEntity, TBllEntity, Guid, TMapper>
    where TAppUow : IBaseUnitOfWork
    where TRepository : IBaseEntityRepository<TDomainEntity, TDalEntity, Guid>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TDalEntity : IIdDatabaseEntity<Guid>
    where TMapper : IMapper<TDalEntity, TBllEntity>
{
    protected BaseCrudService(TAppUow uow, TMapper mapper) : base(uow, mapper)
    {
    }
}

public abstract class BaseCrudService<TAppUow, TRepository, TDomainEntity, TDalEntity, TBllEntity, TKey, TMapper> :
    IAppUowContainer
    where TAppUow : IBaseUnitOfWork
    where TRepository : IBaseEntityRepository<TDomainEntity, TDalEntity, TKey>
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
    where TDalEntity : IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TMapper : IMapper<TDalEntity, TBllEntity>
{
    protected readonly TAppUow Uow;
    protected readonly TMapper Mapper;

    protected BaseCrudService(TAppUow uow, TMapper mapper)
    {
        Uow = uow;
        Mapper = mapper;
    }

    protected abstract TRepository Repository { get; }

    public async Task<ICollection<TBllEntity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).Select(e => Mapper.Map(e)!).ToList();
    }

    public async Task DeleteAsync(TKey id)
    {
        var entity = await Repository.GetByIdAsync(id);
        if (entity == null) throw new EntityNotFoundException();
        Repository.Remove(entity);
    }

    public async Task UpdateAsync(TKey id, TBllEntity entity)
    {
        if (!await Repository.ExistsAsync(id)) throw new EntityNotFoundException();

        Repository.Update(Mapper.Map(entity)!);
    }

    public void Create(TBllEntity entity)
    {
        Repository.Add(Mapper.Map(entity)!);
    }

    public async Task<TBllEntity?> GetByIdAsync(TKey id)
    {
        return Mapper.Map(await Repository.GetByIdAsync(id));
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}