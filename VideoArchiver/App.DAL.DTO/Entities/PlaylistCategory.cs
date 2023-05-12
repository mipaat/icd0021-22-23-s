using Domain.Base;

namespace App.DAL.DTO.Entities;

public class PlaylistCategory : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
}