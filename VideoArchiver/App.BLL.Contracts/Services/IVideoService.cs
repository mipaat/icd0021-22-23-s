using App.BLL.DTO.Enums;

namespace App.BLL.Contracts.Services;

public interface IVideoService : IBaseService
{
    public Task SetInternalPrivacyStatus(Guid videoId, ESimplePrivacyStatus status);
}