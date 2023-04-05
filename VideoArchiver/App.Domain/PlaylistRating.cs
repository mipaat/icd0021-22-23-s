using Domain.Base;

namespace Domain;

public class PlaylistRating : AbstractIdDatabaseEntity
{
    public Playlist? Playlist { get; set; }
    public Guid PlaylistId { get; set; }
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }

    public int Rating { get; set; }
    public string? Comment { get; set; }

    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }
}