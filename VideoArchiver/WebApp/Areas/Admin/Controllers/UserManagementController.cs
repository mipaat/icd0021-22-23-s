using App.BLL.DTO;
using App.BLL.DTO.Exceptions;
using App.BLL.Identity;
using App.Common;
using Base.WebHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = RoleNames.Admin)]
public class UserManagementController : Controller
{
    private readonly IdentityUow _identityUow;

    public UserManagementController(IdentityUow identityUow)
    {
        _identityUow = identityUow;
    }

    [BindProperty(SupportsGet = true)] public bool IncludeOnlyNotApproved { get; set; }
    [BindProperty(SupportsGet = true)] public string? NameQuery { get; set; }

    public async Task<IActionResult> Index()
    {
        return View(new UserManagementViewModel
        {
            Users = (await _identityUow.UserService.GetUsersWithRoles(includeOnlyRequiringApproval: IncludeOnlyNotApproved, nameQuery: NameQuery))
                .Where(u => u.Id != User.GetUserId()).ToList(),
            IncludeOnlyNotApproved = IncludeOnlyNotApproved,
            NameQuery = NameQuery,
        });
    }

    [HttpPost]
    public async Task<IActionResult> ApproveRegistration(Guid id)
    {
        try
        {
            await _identityUow.UserService.ApproveRegistration(id);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        await _identityUow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(Guid userId, string roleName)
    {
        await _identityUow.UserService.AddUserToRole(userId, roleName);
        await _identityUow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
    {
        await _identityUow.UserService.RemoveUserFromRole(userId, roleName);
        // No SaveChanges, this already calls ExecuteDeleteAsync(). Reassess if not using EF Core.
        return RedirectToAction(nameof(Index));
    }
}