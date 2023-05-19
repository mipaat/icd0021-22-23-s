using App.BLL.DTO.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class CategoryEditViewModel
{
    [ValidateNever]
    public ICollection<string> SupportedUiCultures { get; set; } = default!;

    public Guid Id { get; set; }
    public CategoryData Category { get; set; } = default!;
}