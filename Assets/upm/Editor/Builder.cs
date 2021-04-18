using System.Collections;
using System.Collections.Generic;
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

        public static Builder Default { get; } = new Builder()
        {
            RunInfo = RunInfo.FromArgs(),
        };

        public RunInfo RunInfo { get; set; }

        public void DoAll()
        {
            var buildConfig = BuildConfig.FromFile(RunInfo.Config);
            var target = buildConfig.Targets.Find(i => i.Name == RunInfo.Target);
        }
    }
}
