using App.BLL.DTO.Exceptions;
using App.BLL.Identity;
using App.Common;
using App.Common.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Public.DTO.v1.Identity;
using WebApp.Authorization;

namespace WebApp.ApiControllers.Admin;

/// <summary>
/// API controller for administrators to manage archive users.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/admin/[controller]/[action]")]
[Authorize(Roles = RoleNames.AdminOrSuperAdmin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ManageUsersController : ControllerBase
{
    private readonly IdentityUow _identityUow;
    private readonly UserMapper _userMapper;

    /// <summary>
    /// Construct a new ManageUsersController.
    /// </summary>
    /// <param name="identityUow">UOW object providing access to identity-related BLL services.</param>
    /// <param name="mapper">Automapper for mapping BLL entities to public API DTOs.</param>
    public ManageUsersController(IdentityUow identityUow, IMapper mapper)
    {
        _identityUow = identityUow;
        _userMapper = new UserMapper(mapper);
    }

    /// <summary>
    /// Get a list of all users with their roles.
    /// </summary>
    /// <param name="filter">Filtering conditions to filter the fetched users by.</param>
    /// <returns>List of users with roles.</returns>
    /// <response code="200">Users fetched successfully.</response>
    [HttpGet]
    public async Task<ActionResult<List<UserWithRoles>>> ListAll([FromQuery] UserFilter filter)
    {
        var users = await _identityUow.UserService.GetUsersWithRoles(
            includeOnlyRequiringApproval: filter.IncludeOnlyNotApproved, nameQuery: filter.NameQuery);
        return Ok(users.Select(u => _userMapper.Map(u)));
    }

    /// <summary>
    /// Get a list of the names of all roles on the platform.
    /// </summary>
    /// <returns>List of role names.</returns>
    /// <response code="200">Role names fetched successfully.</response>
    [HttpGet]
    public ActionResult<List<string>> ListAllRoleNames()
    {
        return Ok(RoleNames.AllAsList);
    }

    /// <summary>
    /// Approve a new user's account registration.
    /// </summary>
    /// <param name="id">The ID of the user being approved.</param>
    /// <response code="200">User approved successfully.</response>
    /// <response code="404">User not found.</response>
    [HttpPut]
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
        return Ok();
    }

    /// <summary>
    /// Remove a role from a user.
    /// </summary>
    /// <param name="userId">The ID of the user to remove the role from.</param>
    /// <param name="roleName">The name of the role to remove.</param>
    /// <response code="200">Role removed from user / User already wasn't in role.</response>
    /// <response code="403">The user making this request isn't allowed to remove the specified role from the specified user.</response>
    [HttpDelete]
    public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
    {
        if (!User.IsAllowedToManageRole(roleName)) return Forbid();
        await _identityUow.UserService.RemoveUserFromRole(userId, roleName);
        return Ok();
    }

    /// <summary>
    /// Add a role to a user.
    /// </summary>
    /// <param name="userId">The ID of the user to add the role to.</param>
    /// <param name="roleName">The name of the role to add.</param>
    /// <response code="200">Role added to user / User already was in role.</response>
    /// <response code="403">The user making this request isn't allowed to add the specified role to the specified user.</response>
    [HttpPost]
    public async Task<IActionResult> AddRole(Guid userId, string roleName)
    {
        if (!User.IsAllowedToManageRole(roleName)) return Forbid();
        await _identityUow.UserService.AddUserToRole(userId, roleName);
        await _identityUow.SaveChangesAsync();
        return Ok();
    }
}