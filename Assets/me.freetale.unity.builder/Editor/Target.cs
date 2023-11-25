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
                ApplyOverride(prop.Key, prop.Value);
            }
        }

        internal void ApplyOverride(string pathKey, object value)
        {
            if (pathKey.StartsWith(nameof(BuildPlayerOptions)))
            {
                var field = typeof(BuildPlayerOptions).GetField(pathKey.Substring(nameof(BuildPlayerOptions).Length + 1));
                if (BuildPlayerOptions == null)
                {
                    BuildPlayerOptions = new BuildPlayerOptions();
                }
                field.SetValue(BuildPlayerOptions, value);
            }
            if (pathKey.StartsWith(nameof(StaticProperties)))
            {
                var lastIndex = pathKey.LastIndexOf('.');
                var first = nameof(StaticProperties).Length + 1;
                var className = pathKey.Substring(first, lastIndex - first).Trim('"');
                var propertyName = pathKey.Substring(lastIndex + 1);
                if (StaticProperties == null)
                {
                    StaticProperties = new Dictionary<string, Dictionary<string, object>>();
                }
                if (!StaticProperties.TryGetValue(className, out var propertyToValue) || propertyToValue == null)
                {
                    StaticProperties[className] = new Dictionary<string, object>();
                }
                StaticProperties[className][propertyName] = value;
            }
        }

        public void ApplyConfigureForEdit()
        {
            ApplyScriptDefineSymbols();
            ApplyStaticProperties();
        }
        public void ApplyConfigureForBuild()
        {
            //ApplyScriptDefineSymbols();
            ApplyStaticProperties();
        }

        private void ApplyScriptDefineSymbols()
        {
            if (BuildPlayerOptions.ExtraScriptingDefines == null)
            {
                return;
            }
            var defines = string.Join(";", BuildPlayerOptions.ExtraScriptingDefines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPlayerOptions.TargetGroup, defines);
        }

        private void ApplyStaticProperties()
        {
            if (StaticProperties == null)
            {
                return;
            }
            foreach (var classToProperty in StaticProperties)
            {
                foreach (var prop in classToProperty.Value)
                {
                    PropertyInfo propertyInfo = GetPropertyInfo(classToProperty.Key, prop.Key);
                    ApplyValue(prop.Value, propertyInfo);
                }
            }
        }

        private void ApplyValue(object value, PropertyInfo property)
        {
            if (property.PropertyType == typeof(bool) && value is string v)
            {
                property.SetValue(this, bool.Parse(v), null);
            }
            else
            {
                property.SetValue(this, value, null);
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
