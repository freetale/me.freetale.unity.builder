using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FreeTale.Unity.Builder
{
    public class BuildTargetsWindow : EditorWindow
    {
        [MenuItem("Window/Build Target")]
        private static void MenuItem()
        {
            BuildTargetsWindow window  = GetWindow<BuildTargetsWindow>();
            window.titleContent.text = "Build Target";
        }

        private BuildConfig BuildConfig;

        private RunInfo RunInfo;

        private void OnEnable()
        {
            RunInfo = RunInfo.FromArgs();
            BuildConfig = BuildConfig.FromFile(RunInfo.Config);
            
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Reload"))
            {
                BuildConfig = BuildConfig.FromFile(RunInfo.Config);
            }
            if (BuildConfig.Targets == null)
            {
                GUILayout.Label($"No target found, setup {RunInfo.Config} for build");
            }
            else
            {
                GUILayout.Label("Apply project configure for target");
                for (int i = 0; i < BuildConfig.Targets.Count; i++)
                {
                    var target = BuildConfig.Targets[i];
                    if (GUILayout.Button(target.Name))
                    {
                        target.ApplyConfigure();
                        AssetDatabase.SaveAssets();
                        Debug.Log($"apply target {target.Name} complete");
                    }
                }
            }
        }
    }
}
