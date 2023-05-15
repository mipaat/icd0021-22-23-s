using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;

namespace App.BLL.Exceptions;

public class PlaylistNotFoundInArchiveException : EntityNotFoundInArchiveException
{
    public PlaylistNotFoundInArchiveException(Guid id) : base(id, typeof(Playlist))
    {
    }

    public PlaylistNotFoundInArchiveException(string idOnPlatform, EPlatform platform) : base(idOnPlatform, platform, typeof(Playlist))
    {
    }

    public PlaylistNotFoundInArchiveException(Guid id, string idOnPlatform, EPlatform platform) : base(id, idOnPlatform, platform, typeof(Playlist))
    {
    }
}