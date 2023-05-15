using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Common.Enums.Converters;

public class PlatformJsonConverter : JsonConverter<EPlatform>
{
    public override EPlatform Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonString = reader.GetString();
        if (jsonString == null) throw new ArgumentNullException($"Can't convert null to {typeof(EPlatform)}");
        return Enum.Parse<EPlatform>(jsonString);
    }

    public override void Write(Utf8JsonWriter writer, EPlatform value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}