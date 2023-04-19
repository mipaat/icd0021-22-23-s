using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Domain.Converters;

public class DateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeConverter() : base(dt => dt.ToUniversalTime(), dt => DateTime.SpecifyKind(dt, DateTimeKind.Utc))
    {
    }
}