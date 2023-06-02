using App.DAL.Contracts;

namespace App.BLL.Base;

public class BaseAppUowContainer : global::Base.BLL.BaseAppUowContainer<IAppUnitOfWork>
{
    protected BaseAppUowContainer(IAppUnitOfWork uow) : base(uow)
    {
    }
}