using System;
using System.Text.Json;
using ClockWidget.Models.Weather.Amedas.Json.Data;

namespace ClockWidget.Models.Weather.Amedas.Json.Converters
{
    internal class JsonAmedasWindDirectionDataElementConverter : JsonAmedasArrayElementConverterBase<AmedasDataElement<WindDirection>>
    {
        public override AmedasDataElement<WindDirection> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null || reader.TokenType != JsonTokenType.StartArray) return null;

            var direction = WindDirection.N;
            AQC? aqc = null;

            reader.Read();

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var value) && value <= (int)WindDirection.NNW)
            {
                direction = (WindDirection)value;
            }
            else
            {
                this.ReadToArrayEnd(ref reader);

                return null;
            }

            reader.Read();

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var aqcValue))
            {
                aqc = (AQC)aqcValue;
            }

            this.ReadToArrayEnd(ref reader);

            return new AmedasDataElement<WindDirection>
            {
                Data = direction,
                AQC = aqc,
            };
        }

        public override void Write(Utf8JsonWriter writer, AmedasDataElement<WindDirection> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue((int)value.Data);

            if (value.AQC.HasValue)
            {
                writer.WriteNumberValue((int)value.AQC);
            }
            else
            {
                writer.WriteNullValue();
            }

            writer.WriteEndArray();
        }
    }
}
