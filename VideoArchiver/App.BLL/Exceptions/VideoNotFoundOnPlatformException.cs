using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;

namespace App.BLL.Exceptions;

public class VideoNotFoundOnPlatformException : EntityNotFoundOnPlatformException
{
    public VideoNotFoundOnPlatformException(string idOnPlatform, Platform platform) : base(idOnPlatform, platform, typeof(Video))
    {
    }
}