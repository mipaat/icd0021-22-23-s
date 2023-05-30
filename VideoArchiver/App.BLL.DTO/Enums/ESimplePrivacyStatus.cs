using System.ComponentModel.DataAnnotations;

namespace App.BLL.DTO.Enums;

public enum ESimplePrivacyStatus
{
    [Display(Name = nameof(Private), ResourceType = typeof(Resources.App.BLL.DTO.Enums.ESimplePrivacyStatus))]
    Private,
    [Display(Name = nameof(Unlisted), ResourceType = typeof(Resources.App.BLL.DTO.Enums.ESimplePrivacyStatus))]
    Unlisted,
    [Display(Name = nameof(Public), ResourceType = typeof(Resources.App.BLL.DTO.Enums.ESimplePrivacyStatus))]
    Public,
}