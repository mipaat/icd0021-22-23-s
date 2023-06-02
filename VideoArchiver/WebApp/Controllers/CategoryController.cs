using System.Net;
using App.BLL.Config;
using App.BLL.Contracts.Services;
using App.BLL.DTO.Entities;
using App.BLL.Identity.Services;
using App.Common;
using Base.WebHelpers;
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
    private readonly ICategoryService _categoryService;
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public CategoryController(ICategoryService categoryService, IConfiguration configuration, UserService userService)
    {
        _categoryService = categoryService;
        _configuration = configuration;
        _userService = userService;
    }

    public IActionResult Create(CategoryFormViewModel viewModel)
    {
        viewModel.Category = new CategoryData();
        viewModel.SupportedUiCultures = _configuration.GetSupportedUiCultureNames();
        return View(viewModel);
    }

    [HttpPost]
    [ActionName("Create")]
    public async Task<IActionResult> CreatePost([FromForm] CategoryFormViewModel viewModel)
    {
        viewModel.SupportedUiCultures = _configuration.GetSupportedUiCultureNames();
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        if (!User.IsAllowedToCreatePublicCategory())
        {
            viewModel.Category.IsPublic = false;
        }

        var authorId = HttpContext.Request.GetSelectedAuthorId();
        if (authorId == null || !await _userService.IsUserSubAuthor(authorId.Value, User))
        {
            HttpContext.Response.ClearSelectedAuthorCookies();
            return RedirectToPage(nameof(SelectAuthor));
        }

        var categoryDataWithCreatorId = new CategoryDataWithCreatorId
        {
            CreatorId = authorId,
            IsPublic = viewModel.Category.IsPublic,
            Name = viewModel.Category.Name,
        };

        _categoryService.CreateCategory(categoryDataWithCreatorId);
        await _categoryService.ServiceUow.SaveChangesAsync();

        if (viewModel.ReturnUrl != null) return LocalRedirect(viewModel.ReturnUrl);
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
            Category = new CategoryData
            {
                IsPublic = category.IsPublic,
                Name = category.Name,
            }
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
            if (!category.IsPublic && !await _userService.IsUserSubAuthor(category.Creator.Id, User))
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

    public async Task<IActionResult> Delete(Guid id, string returnUrl = "~")
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        if (category.Creator == null) return Forbid();
        if (!await _userService.IsUserSubAuthor(category.Creator.Id, User)) return Forbid();
        return View(new CategoryDeleteViewModel
        {
            Category = category,
            ReturnUrl = returnUrl,
        });
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeletePost(Guid id, string returnUrl)
    {
        await _categoryService.DeleteAsync(id);
        await _categoryService.ServiceUow.SaveChangesAsync();
        return LocalRedirect("/");
    }

    private LocalRedirectResult RedirectToSelectAuthor =>
        LocalRedirect($"~/Identity/Account/SelectAuthor?returnUrl={WebUtility.UrlEncode(HttpContext.GetFullPath())}");

    private LocalRedirectResult RedirectToSelectAuthorFromForm(CategoryManageEntityCategoriesViewModel model) =>
        LocalRedirect(
            $"~/Identity/Account/SelectAuthor?returnUrl={WebUtility.UrlEncode(HttpContext.Request.Path + model.ToReturnQueryString)}");

    public async Task<IActionResult> ManageEntityCategories(CategoryManageEntityCategoriesViewModel model)
    {
        var selectedAuthorId = HttpContext.Request.GetSelectedAuthorId();
        if (selectedAuthorId == null) return RedirectToSelectAuthor;
        if (!await _userService.IsUserSubAuthor(selectedAuthorId.Value, User)) return RedirectToSelectAuthor;

        model.Categories = await _categoryService.GetAllAssignableCategoriesForAuthor(selectedAuthorId);
        model.SetSelectedCategoryIds(
            await _categoryService.GetAllAssignedCategoryIds(selectedAuthorId.Value, model.Id, model.EntityType));

        return View(model);
    }

    [HttpPost]
    [ActionName("ManageEntityCategories")]
    public async Task<IActionResult> ManageEntityCategoriesPost(CategoryManageEntityCategoriesViewModel model)
    {
        var selectedAuthorId = HttpContext.Request.GetSelectedAuthorId();
        if (selectedAuthorId == null)
            return RedirectToSelectAuthorFromForm(model); // TODO: add relevant form data to query parameters
        if (!await _userService.IsUserSubAuthor(selectedAuthorId.Value, User))
            return RedirectToSelectAuthorFromForm(model);

        await _categoryService.AddToCategories(selectedAuthorId.Value, model.Id, model.EntityType,
            model.SelectedCategoryIds);
        await _categoryService.ServiceUow.SaveChangesAsync();

        return RedirectToAction(nameof(ManageEntityCategories),
            new { model.Id, model.ReturnUrl, model.EntityType });
    }

    [Authorize(Roles = RoleNames.AdminOrSuperAdmin)]
    public async Task<IActionResult> ManageEntityCategoriesPublic(CategoryManageEntityCategoriesViewModel model)
    {
        model.Categories = await _categoryService.GetAllAssignableCategoriesForAuthor(null);
        model.SetSelectedCategoryIds(
            await _categoryService.GetAllAssignedCategoryIds(null, model.Id, model.EntityType));

        return View(model);
    }

    [Authorize(Roles = RoleNames.AdminOrSuperAdmin)]
    [HttpPost]
    [ActionName("ManageEntityCategoriesPublic")]
    public async Task<IActionResult> ManageEntityCategoriesPublicPost(CategoryManageEntityCategoriesViewModel model)
    {
        await _categoryService.AddToCategories(null, model.Id, model.EntityType, model.SelectedCategoryIds);
        await _categoryService.ServiceUow.SaveChangesAsync();

        return RedirectToAction(nameof(ManageEntityCategoriesPublic),
            new { model.Id, model.ReturnUrl, model.EntityType });
    }
}