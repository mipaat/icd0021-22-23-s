using App.BLL.Base;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.Common;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class AuthorService : BaseService<AuthorService>, IAuthorService
{
    public AuthorService(IServiceUow serviceUow, ILogger<AuthorService> logger) : base(serviceUow, logger)
    {
    }

    public async Task<ImageFile?> GetProfileImageAsync(Guid authorId, bool large)
    {
        var imageFiles = await Uow.Authors.GetProfileImagesForAuthor(authorId);
        if (imageFiles == null || imageFiles.Count == 0) return null;
        var orderedImageFiles = imageFiles.OrderBy(i => i.Width * i.Height);
        return large ? orderedImageFiles.Last() : orderedImageFiles.First();
    }
}