using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FreeTale.Unity.Builder
{
    public class Builder
    {
        /// <summary>
        /// entry point for non unity setting
        /// </summary>
        public static void BuildMain()
        {
            Default.DoAll();
            
        }

        public static Builder Default { get; } = new Builder();

        public RunInfo RunInfo { get; set; }

        public void DoAll()
        {
            RunInfo = RunInfo.FromArgs();
            Debug.Log($"loading config from {RunInfo.Config}");
            var buildConfig = BuildConfig.FromFile(RunInfo.Config, RunInfo.ToJObject(RunInfo.Sets));
            var target = buildConfig.Targets.Find(i => i.Name == RunInfo.Target);
            if (target == null)
            {
                var targetNames = string.Join(",", buildConfig.Targets.Select(i => i.Name).ToArray());
                Debug.LogError($"cannot find target {RunInfo.Target}, valid target is [{targetNames}]");
                EditorApplication.Exit(1);
            }
            target.ApplyConfigure();
            var report = BuildPipeline.BuildPlayer(target.BuildPlayerOptions);
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Failed)
            {
                Debug.LogError("Build operation fail, see log for more details");
                EditorApplication.Exit(1);
            }
        }
    }
}
