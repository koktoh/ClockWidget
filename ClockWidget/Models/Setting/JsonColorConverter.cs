using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace ClockWidget.Models.Setting
{
    internal class JsonColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var hexValue = reader.GetString();

            if (string.IsNullOrEmpty(hexValue)) return Colors.Transparent;

            return (Color)ColorConverter.ConvertFromString(hexValue);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
