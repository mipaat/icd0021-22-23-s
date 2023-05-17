using App.Domain.Identity;
using Domain.Base;

namespace App.Domain;

public class EntityAccessPermission : AbstractIdDatabaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid? VideoId { get; set; }
    public Video? Video { get; set; }
    public Guid? PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }
    public Guid? AuthorId { get; set; }
    public Author? Author { get; set; }
}