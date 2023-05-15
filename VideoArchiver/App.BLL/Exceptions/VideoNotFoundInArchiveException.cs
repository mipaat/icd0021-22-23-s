using App.DAL.DTO.Entities;
using App.Common.Enums;

namespace App.BLL.Exceptions;

public class VideoNotFoundInArchiveException : EntityNotFoundInArchiveException
{
    public VideoNotFoundInArchiveException(Guid id) : base(id, typeof(Video))
    {
    }

    public VideoNotFoundInArchiveException(string idOnPlatform, EPlatform platform) :
        base(idOnPlatform, platform, typeof(Video))
    {
    }

    public VideoNotFoundInArchiveException(Guid id, string idOnPlatform, EPlatform platform) :
        base(id, idOnPlatform, platform, typeof(Video))
    {
    }
}