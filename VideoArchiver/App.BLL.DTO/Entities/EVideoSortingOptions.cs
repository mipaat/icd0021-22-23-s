using System.ComponentModel.DataAnnotations;

namespace App.BLL.DTO.Entities;

public enum EVideoSortingOptions
{
    [Display(Name = nameof(CreatedAt), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.EVideoSortingOptions))]
    CreatedAt,
    [Display(Name = nameof(Duration), ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.EVideoSortingOptions))]
    Duration,
}