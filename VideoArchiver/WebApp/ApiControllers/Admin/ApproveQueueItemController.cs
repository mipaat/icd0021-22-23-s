using App.BLL;
using App.BLL.DTO.Exceptions;
using App.Common;
using AutoMapper;
using Base.WebHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;

namespace WebApp.ApiControllers.Admin;

/// <summary>
/// API controller for managing queue items waiting for approval.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/admin/[controller]/[action]")]
[Authorize(Roles = RoleNames.AdminOrSuperAdmin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ApproveQueueItemController : ControllerBase
{
    private readonly ServiceUow _serviceUow;
    private readonly QueueItemMapper _queueItemMapper;

    /// <summary>
    /// Construct new ApproveQueueItemController.
    /// </summary>
    /// <param name="serviceUow">UOW container for general BLL services.</param>
    /// <param name="mapper">Automapper for mapping BLL entities to public API DTOs.</param>
    public ApproveQueueItemController(ServiceUow serviceUow, IMapper mapper)
    {
        _serviceUow = serviceUow;
        _queueItemMapper = new QueueItemMapper(mapper);
    }

    /// <summary>
    /// Fetch a list of all queue items that haven't been approved by an administrator yet.
    /// </summary>
    /// <returns>List of queue items.</returns>
    /// <response code="200">Queue items fetched successfully.</response>
    [HttpGet]
    public async Task<ActionResult<List<QueueItemForApproval>>> ListAll()
    {
        return Ok((await _serviceUow.QueueItemService.GetAllAwaitingApprovalAsync())
            .Select(e => _queueItemMapper.Map(e)));
    }

    /// <summary>
    /// Approve a queue item's addition to the archive.
    /// </summary>
    /// <param name="input">Necessary parameters for performing the approval action.</param>
    /// <response code="200">Queue item approved successfully.</response>
    /// <response code="404">Queue item with provided ID not found.</response>
    [HttpPost]
    public async Task<IActionResult> ApproveQueueItem([FromBody] ApproveQueueItemInput input)
    {
        try
        {
            await _serviceUow.QueueItemService.ApproveAsync(input.Id, User.GetUserId(), input.GrantAccess);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        await _serviceUow.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Delete a queue item.
    /// </summary>
    /// <param name="id">The ID of the queue item to delete.</param>
    /// <response code="200">Queue item deleted successfully.</response>
    /// <response code="404">Queue item with provided ID not found.</response>
    [HttpDelete]
    public async Task<IActionResult> DeleteQueueItem([FromBody] Guid id)
    {
        try
        {
            await _serviceUow.QueueItemService.DeleteAsync(id);
        }
        catch (Exception)
        {
            return NotFound();
        }

        return Ok();
    }
}