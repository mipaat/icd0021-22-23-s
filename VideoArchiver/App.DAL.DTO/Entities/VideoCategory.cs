using Domain.Base;

namespace App.DAL.DTO.Entities;

public class VideoCategory : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    public CategoryWithCreator? Category { get; set; }
    public Guid CategoryId { get; set; }

    public AuthorBasic? AssignedBy { get; set; }
    public Guid? AssignedById { get; set; }
}