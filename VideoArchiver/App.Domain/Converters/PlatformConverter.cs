using System.Text.Json;
using System.Text.Json.Serialization;
using App.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Domain.Converters;

public class PlatformConverter : ValueConverter<Platform, string>
{
    public PlatformConverter() : base(
        p => p,
        s => s)
    {
    }
}

public class PlatformJsonConverter : JsonConverter<Platform>
{
    public override Platform? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonString = reader.GetString();
        if (jsonString == null) return null;
        return jsonString;
    }

    public override void Write(Utf8JsonWriter writer, Platform value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}