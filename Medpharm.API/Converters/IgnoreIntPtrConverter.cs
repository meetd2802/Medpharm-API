using System.Text.Json;
using System.Text.Json.Serialization;

public class ExceptionConverter : JsonConverter<Exception>
{
    public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implement if necessary to handle reading exceptions from JSON
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("message", value.Message);
        writer.WriteString("stackTrace", value.StackTrace);

        // Exclude MethodBase or other properties you don't need
        writer.WriteEndObject();
    }
}