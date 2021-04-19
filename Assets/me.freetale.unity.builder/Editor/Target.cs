using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace FreeTale.Unity.Builder
{
    public class StaticProperty
    {
        public PropertyInfo PropertyInfo;
        public object Value;
    }

    public class Target
    {
        public string Name;

        public BuildPlayerOptions BuildPlayerOptions;

        public List<StaticProperty> StaticProperties;

        public string[] ScriptingDefineSymbols;

        public static Target FromJObject(JObject targetObject)
        {
            Target target = new Target();
            target.Name = Utility.RequireString(targetObject, "Name");
            target.StaticProperties = new List<StaticProperty>();
            target.BuildPlayerOptions = Utility.ParseBuildPlayerOptions(Utility.RequireObject(targetObject, "BuildPlayerOptions"));
            target.StaticProperties = Utility.ParseStaticProperties(Utility.OptionalObject(targetObject, "StaticProperties"));
            target.ScriptingDefineSymbols = Utility.OptionalStrings(targetObject, "ScriptingDefineSymbols")?.ToArray();
            return target;
        }

        public void ApplyConfigure()
        {
            if (ScriptingDefineSymbols != null)
            {
                var defines = string.Join(";", ScriptingDefineSymbols);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPlayerOptions.targetGroup, defines);
            }
            if (StaticProperties != null)
            {
                foreach (var item in StaticProperties)
                {
                    item.PropertyInfo.SetValue(null, item.Value);
                }
            }
        }
    }
}
