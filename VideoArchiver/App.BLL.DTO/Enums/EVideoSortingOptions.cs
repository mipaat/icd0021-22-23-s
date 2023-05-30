using System.ComponentModel.DataAnnotations;

namespace App.BLL.DTO.Enums;

public enum EVideoSortingOptions
{
    [Display(Name = nameof(CreatedAt), ResourceType = typeof(Resources.App.BLL.DTO.Enums.EVideoSortingOptions))]
    CreatedAt,
    [Display(Name = nameof(Duration), ResourceType = typeof(Resources.App.BLL.DTO.Enums.EVideoSortingOptions))]
    Duration,
}