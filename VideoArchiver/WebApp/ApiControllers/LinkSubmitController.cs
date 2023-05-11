using System.Net;
using App.BLL;
using App.BLL.Exceptions;
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;

namespace WebApp.ApiControllers;

/// <summary>
/// API controller containing methods for submitting a new link to the archive.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Authorize(Roles = UrlSubmissionHandler.AllowedToSubmitRoles,
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LinkSubmitController : ControllerBase
{
    private readonly UrlSubmissionHandler _urlSubmissionHandler;
    private readonly IAppUnitOfWork _uow;

    /// <summary>
    /// Construct a new LinkSubmitController.
    /// </summary>
    /// <param name="uow">Unit of Work object containing DAL repositories.</param>
    /// <param name="urlSubmissionHandler">BLL object for handling link submissions.</param>
    public LinkSubmitController(IAppUnitOfWork uow, UrlSubmissionHandler urlSubmissionHandler)
    {
        _urlSubmissionHandler = urlSubmissionHandler;
        _uow = uow;
    }

    /// <summary>
    /// Submit the provided link to the archive.
    /// </summary>
    /// <param name="link"></param>
    /// <returns>List containing information about added entities.
    /// These may be newly added entities, or previously added entities that match the submission.</returns>
    /// <response code="200">The submission was processed successfully.</response>
    /// <response code="400">The submitted URL didn't match any supported URL patterns.</response>
    [HttpPost]
    public async Task<ActionResult<List<Public.DTO.v1.SubmissionResult>>> Submit([FromBody] Public.DTO.v1.LinkSubmission link)
    {
        UrlSubmissionResults bllSubmissionResults;
        try
        {
            bllSubmissionResults = await _urlSubmissionHandler.SubmitGenericUrlAsync(link.Link, User);
        }
        catch (UnrecognizedUrlException e)
        {
            return BadRequest(new Public.DTO.v1.RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = e.Message,
            });
        }
        await _uow.SaveChangesAsync();
        return Ok(SubmissionResultMapper.Map(bllSubmissionResults));
    }
}