using App.DAL.DTO.Entities;

namespace App.BLL.Contracts.Services;

public interface IStatusChangeService : IBaseService
{
    public Task Push(StatusChangeEvent statusChangeEvent);
}