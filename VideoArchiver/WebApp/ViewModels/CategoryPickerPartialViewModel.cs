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

    public void SetCategories(IEnumerable<CategoryWithCreator> categories)
    {
        Categories = new Dictionary<EPlatform, ICollection<CategoryWithCreator>>();
        foreach (var category in categories)
        {
            Categories.TryAdd(category.Platform, new List<CategoryWithCreator>());
            Categories[category.Platform].Add(category);
        }
    }

    public void SetSelectedCategoryIds(IEnumerable<Guid> ids)
    {
        var i = 0;
        foreach (var id in ids)
        {
            SelectedCategoryIds[i++] = id;
        }
    }
}