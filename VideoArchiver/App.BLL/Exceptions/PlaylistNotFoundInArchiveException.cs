using App.Domain;
using App.Domain.Enums;

namespace App.BLL.Exceptions;

public class PlaylistNotFoundInArchiveException : EntityNotFoundInArchiveException
{
    public PlaylistNotFoundInArchiveException(Guid id) : base(id, typeof(Playlist))
    {
    }

    public PlaylistNotFoundInArchiveException(string idOnPlatform, Platform platform) : base(idOnPlatform, platform, typeof(Playlist))
    {
    }

    public PlaylistNotFoundInArchiveException(Guid id, string idOnPlatform, Platform platform) : base(id, idOnPlatform, platform, typeof(Playlist))
    {
    }
}