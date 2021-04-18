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

    public struct Target
    {
        public string Name;

        public BuildPlayerOptions BuildPlayerOptions;

        public List<PropertyEntry> PropertyEntries;

        public static Target FromJObject(JObject targetObject)
        {
            Target target = new Target();
            target.Name = targetObject.Value<string>("name");
            target.PropertyEntries = new List<PropertyEntry>();
            BuildPlayerOptions options = new BuildPlayerOptions();
            options.locationPathName = "";
            options.target = BuildTarget.StandaloneWindows;
            options.targetGroup = BuildTargetGroup.Standalone;
            options.options = BuildOptions.AllowDebugging;
            BuildPipeline.BuildPlayer(options);
            return target;
        }

    }
}
