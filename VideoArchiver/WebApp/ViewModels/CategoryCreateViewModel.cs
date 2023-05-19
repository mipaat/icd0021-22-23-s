#pragma warning disable 1591

using App.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WebApp.Validation;

namespace WebApp.ViewModels;

public class CategoryCreateViewModel
{
    [ValidateNever]
    public ICollection<string> SupportedUiCultures { get; set; } = default!;
    [LangStringNotEmpty] public LangString Name { get; set; } = new();
    public bool IsPublic { get; set; }
}