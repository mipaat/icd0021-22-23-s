using App.BLL.DTO.Entities.Identity;
using App.Common;

namespace WebApp.Areas.Admin.ViewModels;

public class ManageRolesViewModel
{
    public UserWithRoles User { get; set; } = default!;
    public IEnumerable<string> OtherRoles => RoleNames.AllAsList.Where(r => User.Roles.All(ur => r != ur.Name));
}