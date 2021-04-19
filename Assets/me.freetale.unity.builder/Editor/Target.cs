using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

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

        public static Target FromJObject(JObject targetObject)
        {
            Target target = new Target();
            target.Name = Utility.RequireString(targetObject, "Name");
            target.StaticProperties = new List<StaticProperty>();
            target.BuildPlayerOptions = Utility.ParseBuildPlayerOptions(Utility.RequireObject(targetObject, "BuildPlayerOptions"));
            target.StaticProperties = Utility.ParseStaticProperties(Utility.OptionalObject(targetObject, "StaticProperties"));
            return target;
        }

        public void ApplyStaticProperty()
        {
            foreach (var item in StaticProperties)
            {
                item.PropertyInfo.SetValue(null, item.Value);
            }
        }
    }
}
