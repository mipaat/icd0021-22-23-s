using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1.Domain;

namespace WebApp.ApiControllers.Crud;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/crud/[controller]/[action]")]
[Authorize(Roles = RoleNames.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GamesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;
    private readonly GameMapper _gameMapper;

    public GamesController(IAppUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _gameMapper = new GameMapper(mapper);
    }

    public IGameRepository Entities => _uow.Games;

    [HttpGet]
    public async Task<ActionResult<ICollection<GameWithId>>> ListAll()
    {
        var entities = await Entities.GetAllAsync();
        return entities.Select(e => _gameMapper.Map(e)!).ToList();
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

        Entities.Update(_gameMapper.Map(entity)!);
        await _uow.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GameWithoutId entity)
    {
        Entities.Add(_gameMapper.MapWithoutId(entity));
        await _uow.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<GameWithId>> GetById(Guid id)
    {
        var entity = await Entities.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return _gameMapper.Map(entity)!;
    }
}