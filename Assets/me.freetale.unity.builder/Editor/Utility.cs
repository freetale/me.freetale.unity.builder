using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FreeTale.Unity.Builder
{
    internal static class Utility
    {
        public static BuildPlayerOptions ParseBuildPlayerOptions(JObject obj)
        {
            BuildPlayerOptions options = new BuildPlayerOptions();
            options.scenes = OptionalStrings(obj, nameof(BuildPlayerOptions.scenes))?.ToArray();
            options.locationPathName = RequireString(obj, nameof(BuildPlayerOptions.locationPathName));
            options.assetBundleManifestPath = OptionalString(obj, nameof(BuildPlayerOptions.assetBundleManifestPath));
            options.targetGroup = RequireEnum<BuildTargetGroup>(obj, nameof(BuildPlayerOptions.targetGroup));
            options.target = RequireEnum<BuildTarget>(obj, nameof(BuildPlayerOptions.target));
            options.options = OptionalEnum<BuildOptions>(obj, nameof(BuildPlayerOptions.options));

            return options;
        }

        public static List<StaticProperty> ParseStaticProperties(JObject obj)
        {
            if (obj == null)
            {
                return null;
            }
            List<StaticProperty> staticProperties = new List<StaticProperty>();
            foreach (var item in obj.Properties())
            {
                staticProperties.AddRange(ParseStaticPropertiesType(item));
            }
            return staticProperties;
        }

        public static IEnumerable<StaticProperty> ParseStaticPropertiesType(JProperty prop)
        {
            var type = Type.GetType(prop.Name);
            var obj = (JObject)prop.Value;
            return obj.Properties().Select(i => ParseStaticPropertyProperty(type, i));
        }

        internal static StaticProperty ParseStaticPropertyProperty(Type type, JProperty property)
        {
            var propertyInfo = type.GetProperty(property.Name);
            if (propertyInfo == null)
            {
                throw new UnexpectPropertyException(property, type.Name);
            }
            var value = property.Value.ToObject(propertyInfo.PropertyType);
            return new StaticProperty
            {
                PropertyInfo = propertyInfo,
                Value = value,
            };
        }

        /// <summary>
        /// must contains string, or throw parse error
        /// </summary>
        /// <param name="obj">base object</param>
        /// <param name="property">json property</param>
        /// <exception cref="UnexpectConfigException">on missing or invalid type</exception>
        /// <returns>string value</returns>
        public static string RequireString(JObject obj, string property)
        {
            if (obj.TryGetValue(property, out JToken token))
            {
                if (TryParseString(token, out var result))
                    return result;
                throw new UnexpectConfigException(token, property, "string", obj);
            }
            throw new UnexpectConfigException(null, property, "string", obj);
        }

        /// <summary>
        /// must contains string, or throw parse error
        /// </summary>
        /// <param name="obj">base object</param>
        /// <param name="property">json property</param>
        /// <returns>string value</returns>
        public static string OptionalString(JObject obj, string property)
        {
            if (obj.TryGetValue(property, out JToken token))
            {
                if (TryParseString(token, out var result))
                    return result;
            }
            return null;
        }

        internal static bool TryParseString(JToken token, out string result)
        {
            if (token != null && token.Type == JTokenType.String)
            {
                result = token.Value<string>();
                return true;
            }
            result = default;
            return false;
        }

        public static IEnumerable<string> OptionalStrings(JObject obj, string property)
        {
            if (obj == null)
            {
                return null;
            }
            JToken values = obj.GetValue(property);
            if (values == null)
            {
                return null;
            }
            List<string> results = new List<string>();
            foreach (var item in values.Children())
            {
                if (TryParseString(item, out var result))
                {
                    results.Add(result);
                }
                else
                {
                    throw new UnexpectConfigException(item, property, "string", (JObject)values);
                }
            }
            return results;
        }

        public static T RequireEnum<T>(JObject obj, string property) where T : struct
        {
            if (obj.TryGetValue(property, out JToken token))
            {
                if (TryParseEnum(token, out T result))
                {
                    return result;
                }
                throw new UnexpectConfigException(token, property, "int|string|string[]", obj);
            }
            throw new UnexpectConfigException(null, property, "int|string|string[]", obj);
        }

        public static T OptionalEnum<T>(JObject obj, string property) where T : struct
        {
            if (obj.TryGetValue(property, out JToken token))
            {
                if (TryParseEnum<T>(token, out T result))
                {
                    return result;
                }
                return default;
            }
            return default;
        }

        internal static bool TryParseEnum<T>(JToken token, out T result) where T : struct
        {
            if (TryParseEnumInternal(token, typeof(T), out var r))
            {
                result = (T)r;
                return true;
            }
            if (token.Type == JTokenType.Array)
            {
                bool success = true;
                long flag = 0L;
                foreach (var item in token.Children())
                {
                    if (TryParseEnumInternal(item, typeof(T), out var k))
                    {
                        flag |= Convert.ToInt64(k);
                    }
                    success = false;
                };

                if (!success)
                {
                    result = (T)Enum.ToObject(typeof(T), flag);
                    return true;
                }
            }
            result = default;
            return false;
        }

        private static bool TryParseEnumInternal(JToken token, Type enumType, out object result)
        {
            if (token.Type == JTokenType.String)
            {
                var value = token.Value<string>();
                if (Enum.IsDefined(enumType, value))
                {
                    result = Enum.Parse(enumType, value);
                    return true;
                }
            }
            if (token.Type == JTokenType.Integer)
            {
                var value = token.Value<long>();
                result = Enum.ToObject(enumType, value);
                return true;
            }
            result = default;
            return false;
        }

        public static JObject RequireObject(JObject obj, string property)
        {
            var result = obj.GetValue(property);
            if (result == null)
            {
                throw new UnexpectConfigException(result, property, "object", obj);
            }
            return (JObject)result;
        }

        public static JObject OptionalObject(JObject obj, string property)
        {
            var result = obj.GetValue(property);
            if (result == null)
            {
                return null;
            }
            return (JObject)result;
        }

    }
}
