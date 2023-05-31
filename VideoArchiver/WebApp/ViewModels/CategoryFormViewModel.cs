using App.BLL.DTO.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class CategoryFormViewModel
{
    [ValidateNever]
    public ICollection<string> SupportedUiCultures { get; set; } = default!;
    public string? ReturnUrl { get; set; }
    public CategoryData Category { get; set; } = default!;
}