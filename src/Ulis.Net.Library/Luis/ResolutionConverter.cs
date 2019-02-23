using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ulis.Net.Library.Luis
{
    internal class ResolutionConverter : JsonConverter
    {
        private const string UnexpectedEndError = "Unexpected end when reading IDictionary<string, object>";

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IDictionary<string, object>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadValue(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        private static object ReadArray(JsonReader reader)
        {
            IList<object> list = new List<object>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndArray:
                        return list;

                    default:
                        var value = ReadValue(reader);

                        list.Add(value);
                        break;
                }
            }

            throw new JsonSerializationException(UnexpectedEndError);
        }

        private static object ReadObject(JsonReader reader)
        {
            var dictionary = new Dictionary<string, object>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        var propertyName = reader.Value.ToString();

                        if (!reader.Read())
                        {
                            throw new JsonSerializationException(UnexpectedEndError);
                        }

                        var value = ReadValue(reader);

                        dictionary[propertyName] = value;
                        break;

                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndObject:
                        return dictionary;
                }
            }

            throw new JsonSerializationException(UnexpectedEndError);
        }

        private static object ReadValue(JsonReader reader)
        {
            while (reader.TokenType == JsonToken.Comment)
            {
                if (!reader.Read())
                {
                    throw new JsonSerializationException("Unexpected token when converting IDictionary<string, object>");
                }
            }

            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return ReadObject(reader);

                case JsonToken.StartArray:
                    return ReadArray(reader);

                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Undefined:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return reader.Value;

                default:
                    throw new JsonSerializationException(
                        string.Format("Unexpected token when converting IDictionary<string, object>: {0}", reader.TokenType));
            }
        }
    }
}