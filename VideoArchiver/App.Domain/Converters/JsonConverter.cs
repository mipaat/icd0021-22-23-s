using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain.Converters;

public class JsonConverter<TValue> : ValueConverter<TValue, string>
{
    public JsonConverter() : base(value => Convert(value),
        serializedValue => Convert(serializedValue))
    {
    }

    private static TValue Convert(string serializedValue) =>
        JsonSerializer.Deserialize<TValue>(serializedValue)
        ?? throw new ConversionException();

    private static string Convert(TValue value) => JsonSerializer.Serialize(value);
}