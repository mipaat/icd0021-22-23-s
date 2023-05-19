using App.BLL.Config;
using App.BLL.Identity.Services;
using App.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Identity.Pages.Account;
using WebApp.Authorization;
using WebApp.ViewModels;

#pragma warning disable CS1591

namespace WebApp.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public CategoryController(CategoryService categoryService, IConfiguration configuration, UserService userService)
    {
        _categoryService = categoryService;
        _configuration = configuration;
        _userService = userService;
    }

    public IActionResult Create(CategoryCreateViewModel viewModel)
    {
        viewModel.SupportedUiCultures = _configuration.GetSupportedUiCultureNames();
        return View(viewModel);
    }

    [HttpPost]
    [ActionName("Create")]
    public async Task<IActionResult> CreatePost([FromForm] CategoryCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.SupportedUiCultures = _configuration.GetSupportedUiCultureNames();
            return View(viewModel);
        }

        if (!User.IsAllowedToCreatePublicCategory())
        {
            viewModel.IsPublic = false;
        }

        var authorId = UserService.GetSelectedAuthorId(HttpContext.Request);
        if (authorId == null || !await _userService.IsUserSubAuthor(authorId.Value, User))
        {
            UserService.ClearSelectedAuthorCookies(HttpContext.Response);
            return RedirectToPage(nameof(SelectAuthor));
        }

        _categoryService.CreateCategory(viewModel.Name, viewModel.IsPublic, authorId.Value);
        await _categoryService.ServiceUow.SaveChangesAsync();

        throw new NotImplementedException();
    }
}