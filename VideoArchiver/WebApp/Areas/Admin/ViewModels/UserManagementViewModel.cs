using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Entities.Identity;
#pragma warning disable CS1591

namespace WebApp.Areas.Admin.ViewModels;

public class UserManagementViewModel
{
    public ICollection<UserWithRoles> Users { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.WebApp.Areas.Admin.ViewModels.UserManagementViewModel), Name = nameof(IncludeOnlyNotApproved))]
    public bool IncludeOnlyNotApproved { get; set; }
    [Display(ResourceType = typeof(App.Resources.WebApp.Areas.Admin.ViewModels.UserManagementViewModel), Name = nameof(NameQuery), Prompt = nameof(NameQuery) + "Prompt")]
    public string? NameQuery { get; set; }
}