using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FreeTale.Unity.Builder
{
    public struct PropertyEntry
    {
        public string PropertyPath;
        public JValue Value;
    }

    public class Target
    {
        public string Name;

        public BuildPlayerOptions BuildPlayerOptions;

        public List<PropertyEntry> PropertyEntries;

        public static Target FromJObject(JObject targetObject)
        {
            Target target = new Target();
            target.Name = Utility.RequireString(targetObject, "Name");
            target.PropertyEntries = new List<PropertyEntry>();
            target.BuildPlayerOptions = Utility.ParseBuildPlayerOptions(Utility.RequireObject(targetObject, "BuildPlayerOptions"));
            return target;
        }
    }
}
