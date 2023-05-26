#pragma warning disable 1591

using App.BLL.DTO.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApp.ViewModels;

public class CategoryCreateViewModel
{
    [ValidateNever]
    public ICollection<string> SupportedUiCultures { get; set; } = default!;

    public CategoryDataWithCreatorId Category { get; set; } = default!;
    public string? ReturnUrl { get; set; }
}