using System.Text.Json.Serialization;
using App.Common.Enums.Converters;

namespace App.Common.Enums;

[JsonConverter(typeof(PlatformJsonConverter))]
public enum EPlatform
{
    This,
    YouTube,
}