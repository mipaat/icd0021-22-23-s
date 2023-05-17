using Microsoft.AspNetCore.Html;

namespace Base.WebHelpers;

public static class DateTimeHelpers
{
    public static IHtmlContent SpanFor(DateTime? value)
    {
        return new HtmlString($"<span class='date-time-local'>{value.ToString()} UTC</span>");
    }
}