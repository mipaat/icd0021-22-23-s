using Domain.Base;

namespace App.DAL.DTO.Entities.Playlists;

public class PlaylistCategory : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public CategoryWithCreator? Category { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
}