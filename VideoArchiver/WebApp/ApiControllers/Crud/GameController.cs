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

/// <summary>
/// Basic CRUD API controller for Game entities.
/// Likely temporary, for testing/homework purposes.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/crud/[controller]/[action]")]
[Authorize(Roles = RoleNames.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GamesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;
    private readonly GameMapper _gameMapper;

    /// <summary>
    /// Create a new GamesController instance.
    /// </summary>
    /// <param name="uow">Unit of Work object containing DAL repositories.</param>
    /// <param name="mapper">Mapper for converting between public DTOs and internal DTOs.</param>
    public GamesController(IAppUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _gameMapper = new GameMapper(mapper);
    }

    private IGameRepository Entities => _uow.Games;

    /// <summary>
    /// Get a list of all games in the database.
    /// </summary>
    /// <returns>List of all games in the database.</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<GameWithId>>> ListAll()
    {
        var entities = await Entities.GetAllAsync();
        return entities.Select(e => _gameMapper.Map(e)!).ToList();
    }

    /// <summary>
    /// Delete a game from the database.
    /// </summary>
    /// <param name="id">The unique ID of the game to remove.</param>
    /// <response code="200">The game was deleted successfully.</response>
    /// <response code="404">No game with the provided ID was found.</response>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await Entities.GetByIdAsync(id);
        if (entity == null) return NotFound();
        Entities.Remove(entity);

        await _uow.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Update an existing game's data.
    /// </summary>
    /// <param name="entity">The new data for the game, with its unique identifier to specify which game's information to update.</param>
    /// <response code="200">The game's data was successfully updated.</response>
    /// <response code="404">No game with the provided ID was found.</response>
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] GameWithId entity)
    {
        if (!await Entities.ExistsAsync(entity.Id)) return NotFound();

        Entities.Update(_gameMapper.Map(entity)!);
        await _uow.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Create a new game with the provided data.
    /// </summary>
    /// <param name="entity">Data to initialize the new game with.</param>
    /// <response code="200">The game was created successfully.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GameWithoutId entity)
    {
        Entities.Add(_gameMapper.MapWithoutId(entity));
        await _uow.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Get information about an existing game.
    /// </summary>
    /// <param name="id">The unique ID of the game to fetch information about.</param>
    /// <returns>The requested game information.</returns>
    /// <response code="200">The requested game with the provided ID was found.</response>
    /// <response code="404">No game with the provided ID was found.</response>
    [HttpGet]
    public async Task<ActionResult<GameWithId>> GetById(Guid id)
    {
        var entity = await Entities.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return _gameMapper.Map(entity)!;
    }
}