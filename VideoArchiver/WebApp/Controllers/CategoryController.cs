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
            viewModel.Category.IsPublic = false;
        }

        var authorId = UserService.GetSelectedAuthorId(HttpContext.Request);
        if (authorId == null || !await _userService.IsUserSubAuthor(authorId.Value, User))
        {
            UserService.ClearSelectedAuthorCookies(HttpContext.Response);
            return RedirectToPage(nameof(SelectAuthor));
        }

        viewModel.Category.CreatorId = authorId;

        _categoryService.CreateCategory(viewModel.Category);
        await _categoryService.ServiceUow.SaveChangesAsync();

        if (viewModel.ReturnUrl != null) return Redirect(viewModel.ReturnUrl);
        return RedirectToAction(nameof(Details));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        if (category.Creator == null) return Forbid();
        if (!await _userService.IsUserSubAuthor(category.Creator.Id, User)) return Forbid();
        return View(new CategoryEditViewModel
        {
            Id = id,
            SupportedUiCultures = _configuration.GetSupportedUiCultureNames(),
        });
    }

    [HttpPost]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(CategoryEditViewModel model)
    {
        
        model.SupportedUiCultures = _configuration.GetSupportedUiCultureNames();
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = await _categoryService.GetByIdAsync(model.Id);
        if (category == null) return NotFound();
        if (category.Creator == null) return Forbid();
        if (!await _userService.IsUserSubAuthor(category.Creator.Id, User)) return Forbid();
        
        if (!User.IsAllowedToCreatePublicCategory())
        {
            model.Category.IsPublic = false;
        }

        _categoryService.Update(model.Id, model.Category);
        await _categoryService.ServiceUow.SaveChangesAsync();

        return RedirectToAction(nameof(Details));
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        var userIsCreator = false;
        if (category.Creator != null)
        {
            if (!category.IsPublic)
            {
                return Forbid();
            }
            userIsCreator = await _userService.IsUserSubAuthor(category.Creator.Id, User);
        }
        return View(new CategoryDetailsViewModel
        {
            Category = category,
            SupportedUiCultures = _configuration.GetSupportedUiCultureNames(),
            UserIsCreator = userIsCreator,
        });
    }
}