using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


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

        public string ScriptingDefineSymbols;

        public void ApplyConfigure()
        {
            if (ScriptingDefineSymbols != null)
            {
                var defines = string.Join(";", ScriptingDefineSymbols);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPlayerOptions.targetGroup, ScriptingDefineSymbols);
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
