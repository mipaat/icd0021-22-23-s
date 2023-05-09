using App.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;

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
    [HttpPost]
    public async Task Submit([FromBody] LinkSubmission link)
    {
        await _urlSubmissionHandler.SubmitGenericUrlAsync(link.Link, User);
        await _uow.SaveChangesAsync();
    }
}