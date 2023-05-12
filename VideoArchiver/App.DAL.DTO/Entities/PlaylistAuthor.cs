using App.DAL.DTO.Enums;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class PlaylistAuthor : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }
    public EAuthorRole Role { get; set; } = EAuthorRole.Publisher;
}