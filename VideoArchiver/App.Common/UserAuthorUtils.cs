using Microsoft.AspNetCore.Http;

namespace App.Common;

public static class UserAuthorUtils
{
    public const string SelectedUserAuthorCookieKey = "VideoArchiverSelectedUserAuthor";

    public static void ClearSelectedAuthorCookies(this HttpResponse httpResponse)
    {
        httpResponse.Cookies.Delete(SelectedUserAuthorCookieKey);
    }

    public static void SetSelectedAuthorCookies(this HttpResponse httpResponse, Guid authorId)
    {
        ClearSelectedAuthorCookies(httpResponse);
        httpResponse.Cookies.Append(SelectedUserAuthorCookieKey, authorId.ToString());
    }

    public static Guid? GetSelectedAuthorId(this HttpRequest httpRequest)
    {
        httpRequest.Cookies.TryGetValue(SelectedUserAuthorCookieKey, out var id);
        return id == null ? null : Guid.Parse(id);
    }
}