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
            if (BuildConfig?.Targets == null)
            {
                GUILayout.Label($"No target found, setup {RunInfo.Config} for build");
            }
            else
            {
                GUILayout.Label("Apply project configure for target");
                for (int i = 0; i < BuildConfig.Targets.Count; i++)
                {
                    var target = BuildConfig.Targets[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(target.Name, GUILayout.Width(EditorGUIUtility.labelWidth));
                    if (GUILayout.Button("Apply"))
                    {
                        target.ApplyConfigureForEdit();
                        AssetDatabase.SaveAssets();
                        Debug.Log($"apply target {target.Name} complete");
                    }
                    if (GUILayout.Button("Build"))
                    {
                        target.ApplyConfigureForBuild();
                        var report = BuildPipeline.BuildPlayer(target.BuildPlayerOptions.ToEditorOptions());
                        Debug.Log($"build end with result {report.summary.result}");
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}
