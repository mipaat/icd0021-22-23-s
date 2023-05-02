using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Domain.Converters;

public class JsonValueConverter<TValue> : ValueConverter<TValue, string>
{
    public JsonValueConverter() : base(
        value => Convert(value),
        serializedValue => Convert(serializedValue)
    )
    {
    }

    private static TValue Convert(string serializedValue) =>
        JsonSerializer.Deserialize<TValue>(serializedValue)
        ?? throw new ConversionException();

    private static string Convert(TValue value) =>
        JsonSerializer.Serialize(value);
}