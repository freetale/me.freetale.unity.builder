using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;

namespace FreeTale.Unity.Builder
{
    public class StaticProperty
    {
        public PropertyInfo PropertyInfo;
        public object Value;
    }

    public class BuildPlayerOptions {
        public string[] Scenes;
        public string LocationPathName;
        public string AssetBundleManifestPath;
        public BuildTargetGroup TargetGroup;
        public BuildTarget Target;
        public int Subtarget;
        public BuildOptions Options;
        public string[] ExtraScriptingDefines;

        public UnityEditor.BuildPlayerOptions ToEditorOptions()
        {
            return new UnityEditor.BuildPlayerOptions
            {
                scenes = Scenes,
                locationPathName = LocationPathName,
                assetBundleManifestPath = AssetBundleManifestPath,
                targetGroup = TargetGroup,
                target = Target,
                subtarget = Subtarget,
                options = Options,
                extraScriptingDefines = ExtraScriptingDefines,
            };
        }
    }

    public class Target
    {
        public string Name;

        public BuildPlayerOptions BuildPlayerOptions;

        /// <summary>
        /// static properties to set, in format ClassName => PropertyName => Value
        /// </summary>
        public Dictionary<string, Dictionary<string, object>> StaticProperties;

        public void ApplyOverride(Dictionary<string, string> kv)
        {
            foreach (var prop in kv)
            {

                this.GetType().GetMember(prop.Key);

            }
        }

        public void ApplyConfigure()
        {
            if (BuildPlayerOptions.ExtraScriptingDefines != null)
            {
                var defines = string.Join(";", BuildPlayerOptions.ExtraScriptingDefines);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPlayerOptions.TargetGroup, defines);
            }
            foreach (var classToProperty in StaticProperties)
            {
                foreach (var prop in classToProperty.Value)
                {
                    PropertyInfo propertyInfo = GetPropertyInfo(classToProperty.Key, prop.Key);
                    propertyInfo.SetValue(this, prop.Value, null);
                }
            }
        }

        internal static PropertyInfo GetPropertyInfo(string className, string propertyName)
        {
            Type type;
            type = Type.GetType(className);
            string unityClassName = "UnityEditor." + className + ",UnityEditor";
            if (type == null)
                type = Type.GetType(unityClassName);
            if (type == null)
            {
                throw new Exception($"can't read class name {className} or {unityClassName}");
            }
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new Exception($"can't read property {className}.{propertyName}");
            }
            return propertyInfo;
        }

    }
}
