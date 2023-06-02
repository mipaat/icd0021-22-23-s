using Contracts.BLL;
using Contracts.DAL;

namespace Base.BLL;

public abstract class BaseAppUowContainer<TAppUow> : IAppUowContainer where TAppUow : IBaseUnitOfWork
{
    public TAppUow Uow { get; }

    protected BaseAppUowContainer(TAppUow uow)
    {
        Uow = uow;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}