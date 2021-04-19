using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FreeTale.Unity.Builder
{
    public struct RunInfo
    {
        /// <summary>
        /// build config file, default: BuildConfig.json
        /// </summary>
        public string Config;

        /// <summary>
        /// build target, default: default
        /// </summary>
        public string Target;

        /// <summary>
        /// parse from commandline args
        /// </summary>
        /// <returns></returns>
        public static RunInfo FromArgs()
        {
            return FromArgs(Environment.GetCommandLineArgs());
        }

        /// <summary>
        /// parse runinfo from <paramref name="args"/>, other are ignore
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static RunInfo FromArgs(string[] args)
        {
            var info = new RunInfo
            {
                Config = "BuildConfig.json",
                Target = "default",
            };

            for (int i = 0; i < args.Length; i++)
            {
                if (TrimString(args[i], "--config=", out var config))
                {
                    info.Config = config;
                }
                if (TrimString(args[i], "--target=", out var target))
                {
                    info.Target = target;
                }
            }

            return info;
        }

        private static bool TrimString(string input, string startwith, out string value)
        {
            if (input.StartsWith(startwith))
            {
                value = input.Substring(startwith.Length);
                return true;
            }
            value = default;
            return false;
        }
    }
}
