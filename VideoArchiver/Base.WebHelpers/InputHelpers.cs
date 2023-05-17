using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Base.WebHelpers;

public static class InputHelpers
{
    private static ModelExpressionProvider? GetExpressionProvider(this IHtmlHelper htmlHelper)
    {
        return htmlHelper.ViewContext.HttpContext.RequestServices.GetService<ModelExpressionProvider>();
    }
    
    private static string? GetExpressionText<TModel, TProperty>(this IHtmlHelper htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
    {
        return GetExpressionProvider(htmlHelper)?.GetExpressionText(expression);
    }
    
    private static string? GetPropertyName<TModel, TProperty>(this IHtmlHelper htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
    {
        return htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(GetExpressionText(htmlHelper,
            expression));
    }

    public static IHtmlContent HiddenValueFor<TModel, TProperty>(
        this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, TProperty value)
    {
        var propertyName = htmlHelper.GetPropertyName(expression);

        return htmlHelper.Hidden(propertyName, value);
    }
}