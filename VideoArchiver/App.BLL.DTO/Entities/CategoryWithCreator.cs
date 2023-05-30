using System.ComponentModel.DataAnnotations;
using App.Common;
using App.Common.Enums;
using Domain.Base;

namespace App.BLL.DTO.Entities;

public class CategoryWithCreator : AbstractIdDatabaseEntity
{
    [Display(Name = nameof(Name), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryWithCreator))]
    public LangString Name { get; set; } = default!;
    [Display(Name = nameof(IsPublic), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryWithCreator))]
    public bool IsPublic { get; set; }
    [Display(Name = nameof(IsAssignable), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryWithCreator))]
    public bool IsAssignable { get; set; }
    [Display(Name = nameof(Platform), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryWithCreator))]
    public EPlatform Platform { get; set; }
    [Display(Name = nameof(IdOnPlatform), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryWithCreator))]
    public string? IdOnPlatform { get; set; }

    [Display(Name = nameof(Creator), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryWithCreator))]
    public Author? Creator { get; set; }
}