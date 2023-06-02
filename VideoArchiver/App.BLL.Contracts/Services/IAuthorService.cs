using App.Common;

namespace App.BLL.Contracts.Services;

public interface IAuthorService : IBaseService
{
    public Task<ImageFile?> GetProfileImageAsync(Guid authorId, bool large);
}