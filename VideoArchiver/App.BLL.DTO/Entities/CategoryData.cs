using System.ComponentModel.DataAnnotations;
using App.Common;
using App.Common.Validation;

namespace App.BLL.DTO.Entities;

public class CategoryData
{
    [Display(Name = nameof(Name), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryData))]
    [LangStringNotEmpty] public LangString Name { get; set; } = default!;
    [Display(Name = nameof(IsPublic), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryData))]
    public bool IsPublic { get; set; }
}