using App.BLL.DTO.Entities.Identity;
#pragma warning disable CS1591

namespace WebApp.Areas.Admin.ViewModels;

public class UserManagementViewModel
{
    public ICollection<UserWithRoles> Users { get; set; } = default!;
    public bool IncludeOnlyNotApproved { get; set; }
    public string? NameQuery { get; set; }
}