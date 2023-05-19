using Domain.Base;

namespace App.DAL.DTO.Entities;

public class AuthorRating : AbstractIdDatabaseEntity
{
    public Author? Rated { get; set; }
    public Guid RatedId { get; set; }
    public Author? Rater { get; set; }
    public Guid RaterId { get; set; }

    public int Rating { get; set; }
    public string? Comment { get; set; }

    public CategoryWithCreator? Category { get; set; }
    public Guid CategoryId { get; set; }
}