using Domain.Base;

namespace App.DAL.DTO.Entities;

public class VideoCategoryOnlyIds : AbstractIdDatabaseEntity
{
    public Guid VideoId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? AssignedById { get; set; }
}