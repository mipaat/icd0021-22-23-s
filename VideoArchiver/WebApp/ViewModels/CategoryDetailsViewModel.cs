using App.BLL.DTO.Entities;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class CategoryDetailsViewModel
{
    public ICollection<string> SupportedUiCultures { get; set; } = default!;
    public CategoryWithCreator Category { get; set; } = default!;
    public bool UserIsCreator { get; set; }
}