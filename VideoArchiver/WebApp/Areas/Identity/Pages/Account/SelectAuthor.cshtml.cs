#pragma warning disable 1591
using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Entities;
using App.BLL.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account;

public class SelectAuthor : PageModel
{
    private readonly UserService _userService;
    
    public SelectAuthor(UserService userService)
    {
        _userService = userService;
    }

    public ICollection<Author> Authors { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; } = default!;

    public class InputModel
    {
        [Required]
        public Guid AuthorId { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        Authors = await _userService.GetAllUserSubAuthorsAsync(User);
        if (Authors.Count == 0)
        {
            Authors.Add(_userService.CreateAuthor(User));
            await _userService.SaveChangesAsync();
        }
        if (Authors.Count == 1)
        {
            return SetSelectedAuthor(Authors.First().Id, returnUrl);
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid authorId, string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        if (!await _userService.IsUserSubAuthor(authorId, User))
        {
            ModelState.AddModelError(string.Empty, $"Invalid author selected for user {User.Identity?.Name}");
            return await OnGetAsync(returnUrl);
        }

        return SetSelectedAuthor(authorId, returnUrl);
    }

    private IActionResult SetSelectedAuthor(Guid authorId, string? returnUrl)
    {
        UserService.SetSelectedAuthorCookies(Response, authorId);
        return LocalRedirect(returnUrl ?? Url.Content("~/"));
    }
}