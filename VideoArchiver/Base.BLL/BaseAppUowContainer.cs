using Contracts.BLL;
using Contracts.DAL;

namespace Base.BLL;

public abstract class BaseAppUowContainer<TAppUow> : IAppUowContainer where TAppUow : IBaseUnitOfWork
{
    public readonly TAppUow Uow;

    protected BaseAppUowContainer(TAppUow uow)
    {
        Uow = uow;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}