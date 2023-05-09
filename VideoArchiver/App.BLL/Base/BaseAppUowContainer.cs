using App.Contracts.DAL;

namespace App.BLL.Base;

public class BaseAppUowContainer : global::Base.BLL.BaseAppUowContainer<IAppUnitOfWork>
{
    public BaseAppUowContainer(IAppUnitOfWork uow) : base(uow)
    {
    }
}