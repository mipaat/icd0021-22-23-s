using App.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;

namespace WebApp.ApiControllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
[Authorize(Roles = UrlSubmissionHandler.AllowedToSubmitRoles, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SubmitLinkController : ControllerBase
{
    private readonly UrlSubmissionHandler _urlSubmissionHandler;
    private readonly IAppUnitOfWork _uow;
 
    public SubmitLinkController(IAppUnitOfWork uow, App.BLL.YouTube.SubmitService youTubeSubmitService)
    {
        _urlSubmissionHandler = new UrlSubmissionHandler(youTubeSubmitService);
        _uow = uow;
    }

    [HttpPost]
    public async Task Submit([FromBody] LinkSubmission link)
    {
        await _urlSubmissionHandler.SubmitGenericUrlAsync(link.Link, User);
        await _uow.SaveChangesAsync();
    }
}