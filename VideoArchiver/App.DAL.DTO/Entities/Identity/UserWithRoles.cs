using Domain.Base;

namespace App.DAL.DTO.Entities.Identity;

public class UserWithRoles : AbstractIdDatabaseEntity
{
    public string UserName { get; set; } = default!;
    public bool IsApproved { get; set; }

    public ICollection<Role> Roles { get; set; } = default!;
}