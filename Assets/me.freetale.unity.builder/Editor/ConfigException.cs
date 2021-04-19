using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace FreeTale.Unity.Builder
{
    [Serializable]
    public class ConfigException : Exception
    {
        public ConfigException() { }

        public ConfigException(string message, JContainer context) : base($"{message} at {context.Path}")
        {
            Data.Add("context", context.ToString());
        }
        public ConfigException(string message, JContainer context, Exception inner) : base($"{message} at {context.Path}", inner)
        {
            Data.Add("context", context.ToString());
        }
        protected ConfigException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class UnexpectConfigException : ConfigException
    {
        public UnexpectConfigException(JToken actual, string property, string expect, JContainer context) : base($"unexpect config of {property} expect '{expect}' actual '{actual}'", context) { }
    }

    [Serializable]
    public class UnexpectPropertyException : ConfigException
    {
        public UnexpectPropertyException(JProperty actual, string type) : base($"unexpect config no property {actual.Name} in {type} value '{actual.Value}'", actual.Parent) { }
    }
}
