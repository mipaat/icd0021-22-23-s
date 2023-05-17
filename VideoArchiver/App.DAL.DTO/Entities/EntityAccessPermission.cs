using Domain.Base;

namespace App.DAL.DTO.Entities;

public class EntityAccessPermission : AbstractIdDatabaseEntity
{
    public Guid UserId { get; set; }
    public Guid? VideoId { get; set; }
    public Guid? PlaylistId { get; set; }
    public Guid? AuthorId { get; set; }
}