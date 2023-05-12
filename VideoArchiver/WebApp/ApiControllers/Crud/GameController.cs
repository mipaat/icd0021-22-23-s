using System.Net;
using App.BLL.DTO;
using App.BLL.Services;
using AutoMapper;
using Base.BLL.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
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
    private readonly BasicGameCrudService _gameService;
    private readonly GameMapper _gameMapper;

    /// <summary>
    /// Create a new GamesController instance.
    /// </summary>
    /// <param name="mapper">Mapper for converting between public DTOs and internal DTOs.</param>
    /// <param name="gameService">Basic BLL service for performing CRUD operations on Game entities.</param>
    public GamesController(IMapper mapper, BasicGameCrudService gameService)
    {
        _gameService = gameService;
        _gameMapper = new GameMapper(mapper);
    }

    /// <summary>
    /// Get a list of all games in the database.
    /// </summary>
    /// <returns>List of all games in the database.</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<GameWithId>>> ListAll()
    {
        var entities = await _gameService.GetAllAsync();
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
        try
        {
            await _gameService.DeleteAsync(id);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = $"Game with ID {id} not found",
            });
        }

        await _gameService.SaveChangesAsync();
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
        try
        {
            await _gameService.UpdateAsync(_gameMapper.Map(entity)!);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = $"Game with ID {entity.Id} not found",
            });
        }

        await _gameService.SaveChangesAsync();
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
        _gameService.Create(_gameMapper.MapWithoutId(entity));
        await _gameService.SaveChangesAsync();
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
        var entity = await _gameService.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return _gameMapper.Map(entity)!;
    }
}