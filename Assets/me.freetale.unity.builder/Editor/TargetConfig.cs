using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using YamlDotNet.Core.Tokens;

namespace FreeTale.Unity.Builder
{
    public struct BuildPlayerOptionsConfig
    {
        public string[] scenes;
        public string locationPathName;
        public string assetBundleManifestPath;
        public BuildTargetGroup targetGroup;
        public BuildTarget target;
        public BuildOptions options;
    }

    public class TargetConfig
    {
        public string Name;

        public BuildPlayerOptionsConfig BuildPlayerOptions;

        public Dictionary<string, Dictionary<string, Scalar>> StaticProperties;

        public string[] ScriptingDefineSymbols;

        public Target ToTarget()
        {
            var target = new Target()
            {
                BuildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = BuildPlayerOptions.scenes,
                    target = BuildPlayerOptions.target,
                    targetGroup = BuildPlayerOptions.targetGroup,
                    locationPathName = BuildPlayerOptions.locationPathName,
                    assetBundleManifestPath = BuildPlayerOptions.assetBundleManifestPath,
                    options = BuildPlayerOptions.options,
                }
            };
            if (ScriptingDefineSymbols != null)
            {
                target.ScriptingDefineSymbols = string.Join(";", ScriptingDefineSymbols);
            }
            return target;
        }
    }
}
