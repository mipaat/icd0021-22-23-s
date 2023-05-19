using App.BLL.DTO.Entities;
using App.Common.Enums;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class CategoryPickerPartialViewModel
{
    public string Prefix { get; set; } = "";
    public Dictionary<EPlatform, ICollection<CategoryWithCreator>> Categories { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public Dictionary<int, Guid> SelectedCategoryIds { get; set; } = new();
    public Guid? ActiveAuthorId { get; set; }
}