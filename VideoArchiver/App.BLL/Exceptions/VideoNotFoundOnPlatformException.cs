using App.DAL.DTO.Entities;
using App.Common.Enums;

namespace App.BLL.Exceptions;

public class VideoNotFoundOnPlatformException : EntityNotFoundOnPlatformException
{
    public VideoNotFoundOnPlatformException(string idOnPlatform, EPlatform platform) : base(idOnPlatform, platform, typeof(Video))
    {
    }
}