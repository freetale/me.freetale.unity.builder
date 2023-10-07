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

        public ConfigException(string message, string context, string path) : base($"{message} at {path}")
        {
            Data.Add("context", context);
        }
        public ConfigException(string message, string context, string path, Exception inner) : base($"{message} at {path}", inner)
        {
            Data.Add("context", context);
        }
        protected ConfigException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
