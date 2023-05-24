using App.Common.Enums;
using Domain.Base;

namespace App.DAL.DTO.Entities.Playlists;

public class PlaylistAuthor : AbstractIdDatabaseEntity
{
    public Guid PlaylistId { get; set; }
    public Guid AuthorId { get; set; }
    public EAuthorRole Role { get; set; } = EAuthorRole.Publisher;
}