using System;
using System.Text.Json;
using ClockWidget.Models.Weather.Amedas.Json.Location;

namespace ClockWidget.Models.Weather.Amedas.Json.Converters
{
    internal class JsonAmedasDmsCoordinateConverter : JsonAmedasArrayElementConverterBase<AmedasDmsCoordinate>
    {
        public override AmedasDmsCoordinate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null || reader.TokenType != JsonTokenType.StartArray) return null;


            reader.Read();
            if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out var degree))
            {
                this.ReadToArrayEnd(ref reader);
                return null;
            }
            reader.Read();
            if (reader.TokenType != JsonTokenType.Number || !reader.TryGetDouble(out var minute))
            {
                this.ReadToArrayEnd(ref reader);
                return null;
            }

            this.ReadToArrayEnd(ref reader);

            return new AmedasDmsCoordinate
            {
                Degree = degree,
                Minute = minute
            };
        }

        public override void Write(Utf8JsonWriter writer, AmedasDmsCoordinate value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Degree);
            writer.WriteNumberValue(value.Minute);
            writer.WriteEndArray();
        }

    }
}
