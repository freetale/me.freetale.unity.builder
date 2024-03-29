#if FTBUILDER_JSON
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace FreeTale.Unity.Builder
{
    public class FlagConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    return Enum.Parse(objectType, reader.Value as string);
                case JsonToken.StartArray:
                    string value = reader.ReadAsString();
                    var enumValue = 0;
                    while (value != null)
                    {
                        if(Enum.TryParse(objectType, value, out var result))
                        {
                            enumValue |= (int)result;
                        }
                        else
                        {
                            throw new Exception($"Can't convert {value} to {objectType.Name} only [{string.Join(", ",Enum.GetNames(objectType))}] can be used");
                        }
                        value = reader.ReadAsString();
                    }
                    return enumValue;
                default:
                    throw new Exception($"can't convert to {objectType.Name} {reader.Value}");
            }
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var flags = value.ToString()
                .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(f => $"\"{f}\"");

            writer.WriteRawValue($"[{string.Join(", ", flags)}]");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
    }
}
#endif