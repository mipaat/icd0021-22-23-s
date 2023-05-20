using Domain.Base;

namespace App.Domain;

public class VideoCategory : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }

    public Author? AssignedBy { get; set; }
    public Guid? AssignedById { get; set; }
}