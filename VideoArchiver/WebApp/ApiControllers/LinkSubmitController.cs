using App.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;

namespace WebApp.ApiControllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
[Authorize(Roles = UrlSubmissionHandler.AllowedToSubmitRoles,
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LinkSubmitController : ControllerBase
{
    private readonly UrlSubmissionHandler _urlSubmissionHandler;
    private readonly IAppUnitOfWork _uow;

    public LinkSubmitController(IAppUnitOfWork uow, UrlSubmissionHandler urlSubmissionHandler)
    {
        _urlSubmissionHandler = urlSubmissionHandler;
        _uow = uow;
    }

    [HttpPost]
    public async Task Submit([FromBody] LinkSubmission link)
    {
        await _urlSubmissionHandler.SubmitGenericUrlAsync(link.Link, User);
        await _uow.SaveChangesAsync();
    }
}