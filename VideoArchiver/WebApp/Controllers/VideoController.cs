#pragma warning disable 1591
using App.BLL.Identity.Services;
using App.BLL.Services;
using App.Common;
using Base.WebHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class VideoController : Controller
{
    private readonly VideoPresentationService _videoPresentationService;
    private readonly AuthorizationService _authorizationService;
    private readonly CategoryService _categoryService;
    private readonly UserService _userService;

    public VideoController(VideoPresentationService videoPresentationService, AuthorizationService authorizationService,
        CategoryService categoryService, UserService userService)
    {
        _videoPresentationService = videoPresentationService;
        _authorizationService = authorizationService;
        _categoryService = categoryService;
        _userService = userService;
    }

    [Authorize]
    public async Task<IActionResult> Search(VideoSearchViewModel model)
    {
        model.CategoryPickerViewModel ??= new CategoryPickerPartialViewModel();
        var authorId = HttpContext.Request.GetSelectedAuthorId();
        if (authorId == null || !await _userService.IsUserSubAuthor(authorId.Value, User))
        {
            return RedirectToPage("SelectAuthor", new { ReturnUrl = HttpContext.GetFullPath() });
        }

        model.Limit = PaginationUtils.ClampLimit(model.Limit);
        model.Page = PaginationUtils.ClampPage(null, model.Limit, model.Page);
        model.CategoryPickerViewModel.ActiveAuthorId = HttpContext.Request.GetSelectedAuthorId();
        model.CategoryPickerViewModel.Prefix = nameof(VideoSearchViewModel.CategoryPickerViewModel);
        model.CategoryPickerViewModel.SetCategories(
            await _categoryService.GetAllCategoriesFilterableForAuthorAsync(authorId));
        model.Videos =
            await _videoPresentationService.SearchVideosAsync(
                platformQuery: model.PlatformQuery, nameQuery: model.NameQuery, authorQuery: model.AuthorQuery,
                categoryIds: model.CategoryPickerViewModel.SelectedCategoryIds.Select(kvp => kvp.Value).ToList(),
                user: User, userAuthorId: authorId,
                page: model.Page, limit: model.Limit,
                sortingOptions: model.SortingOptions, descending: model.Descending);
        return View(model);
    }

    // GET
    public async Task<IActionResult> Watch(Guid id, bool embedView = false, int commentsPage = 0, int commentsLimit = 50)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, id)) return Forbid();
        commentsLimit = PaginationUtils.ClampLimit(commentsLimit);
        var video = await _videoPresentationService.GetVideoAsync(id, commentsLimit, commentsPage);
        int? total = video.ArchivedRootCommentCount;
        PaginationUtils.ConformValues(ref total, ref commentsLimit, ref commentsPage);
        return View(new VideoViewModel
        {
            Video = video,
            EmbedView = embedView,
            CommentsPage = commentsPage,
            CommentsLimit = commentsLimit,
        });
    }
}