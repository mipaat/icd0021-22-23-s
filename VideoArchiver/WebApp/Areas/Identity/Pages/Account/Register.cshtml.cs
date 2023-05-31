#pragma warning disable 1591
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using App.BLL.Identity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly UserService _userService;

        public RegisterModel(
            ILogger<RegisterModel> logger,
            UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(App.Resources.WebApp.Validation.Required), ErrorMessageResourceName = "ErrorMessage")]
            [Display(Name = nameof(UserName), Prompt = nameof(UserName) + "Prompt", ResourceType = typeof(App.Resources.WebApp.Areas.Identity.Pages.Account.RegisterModel))]
            public string UserName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(App.Resources.WebApp.Validation.Required), ErrorMessageResourceName = "ErrorMessage")]
            [StringLength(100, ErrorMessageResourceType = typeof(App.Resources.WebApp.Validation.StringLength), ErrorMessageResourceName = "ErrorMessage", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = nameof(Password), Prompt = nameof(Password) + "Prompt", ResourceType = typeof(App.Resources.WebApp.Areas.Identity.Pages.Account.RegisterModel))]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "ConfirmPassword", Prompt = "PasswordPrompt", ResourceType = typeof(App.Resources.WebApp.Areas.Identity.Pages.Account.RegisterModel))]
            [Compare("Password", ErrorMessageResourceType = typeof(App.Resources.WebApp.Areas.Identity.Pages.Account.RegisterModel), ErrorMessageResourceName = "ComparePasswordErrorMessage")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = await _userService.GetExternalAuthenticationSchemesAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = await _userService.GetExternalAuthenticationSchemesAsync();
            if (ModelState.IsValid)
            {
                var (result, user) = await _userService.CreateUser(Input.UserName, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!user.IsApproved)
                    {
                        return RedirectToPage("./AwaitingApproval");
                    }
                    await _userService.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
