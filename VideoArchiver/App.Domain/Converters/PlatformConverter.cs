using App.Common.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Domain.Converters;

public class PlatformConverter : ValueConverter<EPlatform, string>
{
    public PlatformConverter() : base(
        p => p.ToString(),
        s => Enum.Parse<EPlatform>(s))
    {
    }
}