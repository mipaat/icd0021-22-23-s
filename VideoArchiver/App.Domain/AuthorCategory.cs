using Domain.Base;

namespace App.Domain;

public class AuthorCategory : AbstractIdDatabaseEntity
{
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
    public Author? AssignedBy { get; set; }
    public Guid? AssignedById { get; set; }
}