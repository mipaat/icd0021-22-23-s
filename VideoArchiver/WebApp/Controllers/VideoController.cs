#pragma warning disable 1591
using App.BLL;
using App.BLL.Identity.Services;
using App.BLL.Services;
using App.Common;
using Base.WebHelpers;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class VideoController : Controller
{
    private readonly VideoPresentationHandler _videoPresentationHandler;
    private readonly AuthorizationService _authorizationService;
    private readonly CategoryService _categoryService;
    private readonly UserService _userService;

    public VideoController(VideoPresentationHandler videoPresentationHandler, AuthorizationService authorizationService,
        CategoryService categoryService, UserService userService)
    {
        _videoPresentationHandler = videoPresentationHandler;
        _authorizationService = authorizationService;
        _categoryService = categoryService;
        _userService = userService;
    }

    public async Task<IActionResult> Search(VideoSearchViewModel model)
    {
        model.CategoryPickerViewModel ??= new CategoryPickerPartialViewModel();
        var authorId = HttpContext.Request.GetSelectedAuthorId();
        if (authorId != null)
        {
            if (!await _userService.IsUserSubAuthor(authorId.Value, User))
            {
                return RedirectToPage("SelectAuthor", new { ReturnUrl = HttpContext.GetFullPath() });
            }
        }

        model.CategoryPickerViewModel.ActiveAuthorId = HttpContext.Request.GetSelectedAuthorId();
        model.CategoryPickerViewModel.Prefix = nameof(VideoSearchViewModel.CategoryPickerViewModel);
        model.CategoryPickerViewModel.SetCategories(
            await _categoryService.GetAllCategoriesFilterableForAuthorAsync(authorId));
        model.Videos =
            await _videoPresentationHandler.SearchVideosAsync(model.PlatformQuery, model.NameQuery, model.AuthorQuery,
                model.CategoryPickerViewModel.SelectedCategoryIds.Select(kvp => kvp.Value).ToList(), User, authorId);
        return View(model);
    }

    // GET
    public async Task<IActionResult> Watch(Guid id, bool embedView = false)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, id)) return Forbid();
        return View(new VideoViewModel
        {
            Video = await _videoPresentationHandler.GetVideoAsync(id),
            EmbedView = embedView,
        });
    }
}