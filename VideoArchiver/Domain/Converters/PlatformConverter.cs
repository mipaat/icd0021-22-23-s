using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain.Converters;

public class PlatformConverter : ValueConverter<Platform, string>
{
    public PlatformConverter() : base(
        p => p,
        s => s)
    {
    }
}