using Domain.Base;

namespace App.DAL.DTO.Entities;

public class AuthorCategory : AbstractIdDatabaseEntity
{
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }
    public CategoryWithCreator? Category { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
}