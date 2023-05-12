using Domain.Base;

namespace App.DAL.DTO.Entities;

public class VideoRating : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }

    public int Rating { get; set; }
    public string? Comment { get; set; }

    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }
}