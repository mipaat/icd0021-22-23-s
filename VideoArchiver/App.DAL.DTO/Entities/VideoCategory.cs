using Domain.Base;

namespace App.DAL.DTO.Entities;

public class VideoCategory : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
}