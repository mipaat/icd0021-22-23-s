using Domain.Base;

namespace App.Domain;

public class PlaylistCategory : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
    public Author? AssignedBy { get; set; }
    public Guid? AssignedById { get; set; }
}