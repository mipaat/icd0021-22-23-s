using Domain.Base;

namespace App.DAL.DTO.Entities.Playlists;

public class PlaylistCategoryOnlyIds : AbstractIdDatabaseEntity
{
    public Guid PlaylistId { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
    public Guid? AssignedById { get; set; }
}