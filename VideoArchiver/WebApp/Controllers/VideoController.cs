#pragma warning disable 1591
using App.BLL;
using App.BLL.Services;
using Base.WebHelpers;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class VideoController : Controller
{
    private readonly VideoPresentationHandler _videoPresentationHandler;
    private readonly AuthorizationService _authorizationService;
    private readonly CategoryService _categoryService;

    public VideoController(VideoPresentationHandler videoPresentationHandler, AuthorizationService authorizationService,
        CategoryService categoryService)
    {
        _videoPresentationHandler = videoPresentationHandler;
        _authorizationService = authorizationService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Search(VideoSearchViewModel model)
    {
        model.CategoryPickerViewModel ??= new CategoryPickerPartialViewModel();
        model.CategoryPickerViewModel.Prefix = nameof(VideoSearchViewModel.CategoryPickerViewModel);
        model.CategoryPickerViewModel.Categories =
            await _categoryService.GetAllCategoriesGroupedByPlatformAsync(User.GetUserIdIfExists());
        model.Videos =
            await _videoPresentationHandler.SearchVideosAsync(model.PlatformQuery, model.NameQuery, model.AuthorQuery,
                model.CategoryPickerViewModel.SelectedCategoryIds.Select(kvp => kvp.Value).ToList());
        return View(model);
    }

    // GET
    public async Task<IActionResult> Watch(Guid id)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, id)) return Forbid();
        return View(new VideoViewModel
        {
            Video = await _videoPresentationHandler.GetVideoAsync(id)
        });
    }
}