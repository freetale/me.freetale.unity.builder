using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
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
            Default.DoCurrentTarget();

        }

        public static Builder Default { get; } = new Builder();

        public RunInfo RunInfo { get; set; }

        public void DoCurrentTarget()
        {
            RunInfo = RunInfo.FromArgs();
            Debug.Log($"loading config from {RunInfo.Config}");
            var buildConfig = BuildConfig.FromFile(RunInfo.Config);

            var target = buildConfig.Targets.Find(i => i.Name == RunInfo.Target);
            if (target == null)
            {
                ExitTargetMissing(buildConfig);
            }
            target.ApplyOverride(RunInfo.Sets);
            target.ApplyConfigureForBuild();
            var report = BuildPipeline.BuildPlayer(target.BuildPlayerOptions.ToEditorOptions());
            WriteResultToConsole(report.summary);
            if (report.summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build operation fail, see log for more details");
                EditorApplication.Exit(1);
            }
            if (report.summary.result == BuildResult.Succeeded)
            {
                EditorApplication.Exit(0);
            }

        }

        /// <summary>
        /// reuire for gameci-v3
        /// </summary>
        /// <param name="summary"></param>
        private void WriteResultToConsole(BuildSummary summary)
        {
            // This format is required by the game-ci build action
            Console.WriteLine(
            $"{Environment.NewLine}" +
                    $"###########################{Environment.NewLine}" +
                    $"#      Build results      #{Environment.NewLine}" +
                    $"###########################{Environment.NewLine}" +
                    $"{Environment.NewLine}" +
                    $"Duration: {summary.totalTime}{Environment.NewLine}" +
                    $"Warnings: {summary.totalWarnings}{Environment.NewLine}" +
                    $"Errors: {summary.totalErrors}{Environment.NewLine}" +
                    $"Size: {summary.totalSize} bytes{Environment.NewLine}" +
                    $"{Environment.NewLine}"
            );
        }

        private void ExitTargetMissing(BuildConfig buildConfig)
        {
            var targetNames = string.Join(",", buildConfig.Targets.Select(i => i.Name).ToArray());
            Debug.LogError($"cannot find target {RunInfo.Target}, valid target is [{targetNames}]");
            EditorApplication.Exit(1);
        }

        public void DoTarget(string targetString)
        {
            RunInfo = RunInfo.FromArgs();
            var buildConfig = BuildConfig.FromFile(RunInfo.Config);
            var target = buildConfig.Targets.Find(i => i.Name == targetString);
            if (target == null)
            {
                ExitTargetMissing(buildConfig);
            }
            target.ApplyOverride(RunInfo.Sets);
            var report = BuildPipeline.BuildPlayer(target.BuildPlayerOptions.ToEditorOptions());
        }
    }
}
