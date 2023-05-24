using App.Common.Enums;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class VideoAuthor : AbstractIdDatabaseEntity
{
    public Guid VideoId { get; set; }
    public Guid AuthorId { get; set; }
    public EAuthorRole Role { get; set; } = EAuthorRole.Publisher;
}