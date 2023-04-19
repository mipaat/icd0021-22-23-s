using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1.Domain;

namespace WebApp.ApiControllers.Crud;

[ApiController]
[Route("api/v1/crud/[controller]/[action]")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GamesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;
    
    public GamesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    public IGameRepository Entities => _uow.Games;

    public async Task<ActionResult<ICollection<GameWithId>>> ListAll()
    {
        var entities = await Entities.GetAllAsync();
        var result = new List<GameWithId>();
        foreach (var entity in entities)
        {
            var gameWithId = new GameWithId();
            gameWithId.FromDomainEntity(entity);
            result.Add(gameWithId);
        }
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await Entities.GetByIdAsync(id);
        if (entity == null) return NotFound();
        Entities.Remove(entity);

        await _uow.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] GameWithId entity)
    {
        if (!await Entities.ExistsAsync(entity.Id)) return NotFound();
        
        Entities.Update(entity.ToDomainEntity());
        await _uow.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Game entity)
    {
        Entities.Add(entity.ToDomainEntity());
        await _uow.SaveChangesAsync();
        return Ok();
    }

    public async Task<ActionResult<GameWithId>> GetById(Guid id)
    {
        var entity = await Entities.GetByIdAsync(id);
        if (entity == null) return NotFound();
        var dtoEntity = new GameWithId();
        dtoEntity.FromDomainEntity(entity);
        return Ok(dtoEntity);
    }
}