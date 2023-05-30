using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591
namespace WebApp.Controllers;

public class SelectCultureController : Controller
{
    public IActionResult SetCulture(string culture, string returnUrl = "~")
    {
        var requestCulture = new RequestCulture(culture);
        HttpContext.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
        HttpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(requestCulture),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
            });
        return LocalRedirect(returnUrl);
    }
}