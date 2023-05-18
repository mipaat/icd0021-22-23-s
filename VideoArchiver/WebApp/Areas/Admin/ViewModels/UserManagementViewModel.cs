using App.BLL.DTO.Entities.Identity;

namespace WebApp.Areas.Admin.ViewModels;

public class UserManagementViewModel
{
    public ICollection<UserWithRoles> Users { get; set; } = default!;
    public bool IncludeOnlyNotApproved { get; set; }
    public string? NameQuery { get; set; }
}