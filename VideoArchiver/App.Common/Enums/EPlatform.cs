using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using App.Common.Enums.Converters;

namespace App.Common.Enums;

[JsonConverter(typeof(PlatformJsonConverter))]
public enum EPlatform
{
    [Display(Name = nameof(This), ResourceType = typeof(Resources.App.Common.Enums.EPlatform))]
    This,
    [Display(Name = nameof(YouTube), ResourceType = typeof(Resources.App.Common.Enums.EPlatform))]
    YouTube,
}