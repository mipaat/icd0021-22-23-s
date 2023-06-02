using App.BLL.Contracts.Services;
using App.BLL.Identity.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Swashbuckle.AspNetCore.Annotations;
using WebApp.MyLibraries;

namespace WebApp.ApiControllers;

/// <summary>
/// Api controller for handling categories.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/Categories/[action]")]
public class CategoryApiController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly UserService _userService;
    private readonly CategoryMapper _categoryMapper;

    /// <summary>
    /// Construct a new CategoryApiController.
    /// </summary>
    /// <param name="categoryService">BLL service for managing categories.</param>
    /// <param name="userService">BLL service for handling users.</param>
    /// <param name="mapper">Automapper for mapping BLL entities to API DTOs.</param>
    public CategoryApiController(ICategoryService categoryService, UserService userService, IMapper mapper)
    {
        _categoryService = categoryService;
        _userService = userService;
        _categoryMapper = new CategoryMapper(mapper);
    }

    /// <summary>
    /// List all available categories.
    /// If no author ID is provided, will return only public categories.
    /// If author ID is provided, will also include categories created by that author.
    /// </summary>
    /// <param name="authorId">ID of the author whose created categories should be included in the result.</param>
    /// <returns>List of categories.</returns>
    /// <response code="200">Categories fetched successfully.</response>
    /// <response code="400">Provided author ID does not belong to authenticated user.</response>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerResponse(StatusCodes.Status200OK, null, typeof(List<CategoryWithCreator>))]
    [SwaggerRestApiErrorResponse(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<List<CategoryWithCreator>>> ListAllCategories(Guid? authorId)
    {
        if (authorId != null && !await _userService.IsUserSubAuthor(authorId.Value, User))
        {
            return BadRequest(new RestApiErrorResponse
            {
                Error = $"Author {authorId.Value} is not a sub-author of {User}",
                ErrorType = EErrorType.UserAuthorMismatch,
            });
        }

        return Ok((await _categoryService.GetAllCategoriesFilterableForAuthorAsync(authorId))
            .Select(c => _categoryMapper.Map(c)!).ToList());
    }
}