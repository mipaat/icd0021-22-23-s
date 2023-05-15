using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;

namespace App.BLL.Exceptions;

public class PlaylistNotFoundOnPlatformException : EntityNotFoundOnPlatformException
{
    public PlaylistNotFoundOnPlatformException(string idOnPlatform, EPlatform platform) : base(idOnPlatform, platform, typeof(Playlist))
    {
    }
}