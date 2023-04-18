using App.Contracts.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1.Domain;

namespace WebApp.ApiControllers.Crud;

[ApiController]
[Route("api/v1/crud/[controller]/[action]")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GameController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;
    
    public GameController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<ActionResult<ICollection<Game>>> ListAll()
    {
        return Ok(await _uow.Games.GetAllAsync());
    }
}