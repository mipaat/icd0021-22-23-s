using System.ComponentModel.DataAnnotations;
using App.Common;
using App.Common.Validation;

namespace App.BLL.DTO.Entities;

public class CategoryDataWithCreatorId
{
    [Display(Name = nameof(Name), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryDataWithCreatorId))]
    [LangStringNotEmpty] public LangString Name { get; set; } = new();
    [Display(Name = nameof(IsPublic) ,ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.CategoryDataWithCreatorId))]
    public bool IsPublic { get; set; }
    public Guid? CreatorId { get; set; }
}