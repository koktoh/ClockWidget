using System;
using System.Text.Json;
using ClockWidget.Models.Weather.Amedas.Json.Data;

namespace ClockWidget.Models.Weather.Amedas.Json.Converters
{
    internal class JsonAmedasWeatherCodeDataElementConverter : JsonAmedasArrayElementConverterBase<AmedasDataElement<WeatherCode>>
    {
        public override AmedasDataElement<WeatherCode> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null || reader.TokenType != JsonTokenType.StartArray) return null;

            var code = WeatherCode.Unknown;
            AQC? aqc = null;

            reader.Read();

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var value))
            {
                if ((int)WeatherCode.Thunder < value && value < (int)WeatherCode.Unknown)
                {
                    code = WeatherCode.Pending;
                }
                else if (value <= (int)WeatherCode.Missing)
                {
                    code = (WeatherCode)value;
                }
                else
                {
                    this.ReadToArrayEnd(ref reader);

                    return null;
                }
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

            return new AmedasDataElement<WeatherCode>
            {
                Data = code,
                AQC = aqc,
            };
        }

        public override void Write(Utf8JsonWriter writer, AmedasDataElement<WeatherCode> value, JsonSerializerOptions options)
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
