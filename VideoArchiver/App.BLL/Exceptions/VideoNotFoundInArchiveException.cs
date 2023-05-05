using App.Domain;
using App.Domain.Enums;

namespace App.BLL.Exceptions;

public class VideoNotFoundInArchiveException : EntityNotFoundInArchiveException
{
    public VideoNotFoundInArchiveException(Guid id) : base(id, typeof(Video))
    {
    }

    public VideoNotFoundInArchiveException(string idOnPlatform, Platform platform) :
        base(idOnPlatform, platform, typeof(Video))
    {
    }

    public VideoNotFoundInArchiveException(Guid id, string idOnPlatform, Platform platform) :
        base(id, idOnPlatform, platform, typeof(Video))
    {
    }
}