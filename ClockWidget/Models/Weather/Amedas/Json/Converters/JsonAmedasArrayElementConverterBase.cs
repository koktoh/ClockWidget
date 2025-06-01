using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClockWidget.Models.Weather.Amedas.Json.Converters
{
    internal abstract class JsonAmedasArrayElementConverterBase<T> : JsonConverter<T>
    {
        protected void ReadToArrayEnd(ref Utf8JsonReader reader)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray) break;
            }
        }
    }
}
