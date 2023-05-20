using Domain.Base;

namespace App.DAL.DTO.Entities;

public class AuthorCategoryOnlyIds : AbstractIdDatabaseEntity
{
    public Guid AuthorId { get; set; }
    public Guid CategoryId { get; set; }

    public bool AutoAssign { get; set; }
    public Guid? AssignedById { get; set; }
}