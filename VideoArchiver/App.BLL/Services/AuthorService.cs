using App.BLL.Base;
using App.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class AuthorService : BaseService<AuthorService>
{
    public AuthorService(ServiceUow serviceUow, ILogger<AuthorService> logger, IMapper mapper) : base(serviceUow, logger, mapper)
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