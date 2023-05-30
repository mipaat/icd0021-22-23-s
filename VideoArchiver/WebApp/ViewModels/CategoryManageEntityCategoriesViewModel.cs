using App.BLL.DTO.Entities;
using App.Common.Enums;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class CategoryManageEntityCategoriesViewModel
{
    public EEntityType EntityType { get; set; }
    public Guid Id { get; set; }
    public string ReturnUrl { get; set; } = default!;
    public List<CategoryWithCreator> Categories { get; set; } = default!;
    public Dictionary<Guid, bool> SelectedCategoryIds { get; set; } = default!;

    public void SetSelectedCategoryIds(ICollection<Guid> ids)
    {
        SelectedCategoryIds = new Dictionary<Guid, bool>();
        foreach (var category in Categories)
        {
            SelectedCategoryIds[category.Id] = ids.Contains(category.Id);
        }
    }

    public QueryString ToReturnQueryString => QueryString.Create(new List<KeyValuePair<string, string?>>
    {
        new(nameof(ReturnUrl), ReturnUrl),
        new(nameof(EntityType), EntityType.ToString()),
        new(nameof(Id), Id.ToString()),
    });
}