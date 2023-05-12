using Domain.Base;

namespace App.DAL.DTO.Entities.Identity;

public class User : IIdDatabaseEntity
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = default!;

    public bool IsApproved { get; set; }
}