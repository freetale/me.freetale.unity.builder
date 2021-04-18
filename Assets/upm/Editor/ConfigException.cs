using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTale.Unity.Builder
{

    [System.Serializable]
    public class ConfigException : System.Exception
    {
        public ConfigException() { }

        public ConfigException(string message, JObject context) : base($"{message} at {context.Path}") {
            Data.Add("context", context.ToString());
        }
        public ConfigException(string message, JObject context, System.Exception inner) : base($"{message} at {context.Path}", inner) {
            Data.Add("context", context.ToString());
        }
        protected ConfigException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [System.Serializable]
    public class UnexpectConfigException : ConfigException
    {
        public UnexpectConfigException() { }
        public UnexpectConfigException(JToken actual, string property, string expect, JObject context) : base($"unexpect config of {property} expect '{expect}' actual '{actual}'", context) { }
        protected UnexpectConfigException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class UnexpectEnumException : ConfigException
    {
        public UnexpectEnumException() { }
        public UnexpectEnumException(JToken actual, string property, string expect, JObject context) : base($"unexpect config of '{property}' expect '{expect}' actual '{actual}'", context) { }
        protected UnexpectEnumException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
