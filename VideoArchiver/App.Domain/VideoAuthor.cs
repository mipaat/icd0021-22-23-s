using Domain.Base;
using Domain.Enums;

namespace Domain;

public class VideoAuthor : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }
    public EAuthorRole Role { get; set; } = EAuthorRole.Publisher;
}