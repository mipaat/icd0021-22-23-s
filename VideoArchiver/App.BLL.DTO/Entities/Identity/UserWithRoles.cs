namespace App.BLL.DTO.Entities.Identity;

public class UserWithRoles : User
{
    public ICollection<Role> Roles { get; set; } = default!;
}