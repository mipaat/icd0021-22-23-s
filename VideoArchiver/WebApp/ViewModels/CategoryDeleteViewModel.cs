using App.BLL.DTO.Entities;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class CategoryDeleteViewModel
{
    public string ReturnUrl { get; set; } = "~";
    public CategoryWithCreator Category { get; set; } = default!;
}