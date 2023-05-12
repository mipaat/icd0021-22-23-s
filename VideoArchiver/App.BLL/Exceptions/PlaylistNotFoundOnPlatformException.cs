using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;

namespace App.BLL.Exceptions;

public class PlaylistNotFoundOnPlatformException : EntityNotFoundOnPlatformException
{
    public PlaylistNotFoundOnPlatformException(string idOnPlatform, Platform platform) : base(idOnPlatform, platform, typeof(Playlist))
    {
    }
}