using System;
using System.Text.Json;
using ClockWidget.Models.Weather.Amedas.Json.Data;

namespace ClockWidget.Models.Weather.Amedas.Json.Converters
{
    internal class JsonAmedasDoubleDataElementConverter : JsonAmedasArrayElementConverterBase<AmedasDataElement<double>>
    {
        public override AmedasDataElement<double> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null || reader.TokenType != JsonTokenType.StartArray) return null;

            AQC? aqc = null;

            reader.Read();

            if (reader.TokenType != JsonTokenType.Number || !reader.TryGetDouble(out var data))
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

            return new AmedasDataElement<double>
            {
                Data = data,
                AQC = aqc,
            };
        }

        public override void Write(Utf8JsonWriter writer, AmedasDataElement<double> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Data);

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
