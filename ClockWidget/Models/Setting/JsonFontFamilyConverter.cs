using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace ClockWidget.Models.Setting
{
    internal class JsonFontFamilyConverter : JsonConverter<FontFamily>
    {
        public override FontFamily Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return new FontFamily(string.Empty);

            if (reader.TokenType != JsonTokenType.String) throw new JsonException($"Expected string, got {reader.TokenType}.");

            var fontFamilyString = reader.GetString();

            if (string.IsNullOrEmpty(fontFamilyString)) return new FontFamily(string.Empty);

            return new FontFamily(fontFamilyString);
        }

        public override void Write(Utf8JsonWriter writer, FontFamily value, JsonSerializerOptions options)
        {
            if(value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Source);
            }
        }
    }
}
