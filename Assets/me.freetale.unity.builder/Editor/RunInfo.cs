using Newtonsoft.Json.Linq;
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
        /// sets item before parse build target
        /// </summary>
        public Dictionary<string, string> Sets;

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
                Sets = new Dictionary<string, string>(),
            };


            for (int i = 0; i < args.Length; i++)
            {
                GetConfig(ref info.Config, args[i], "config");
                GetConfig(ref info.Target, args[i], "target");
                if (GetSetting(args[i], "--set=", out var name, out var value))
                {
                    info.Sets.Add(name, value);
                }
            }

            return info;
        }

        private static void GetConfig(ref string value, string input, string name)
        {
            string startwith = "--" + name + "=";
            if (input.StartsWith(startwith))
            {
                value = input.Substring(startwith.Length);
                return;
            }
        }

        internal static bool GetSetting(string input, string startwith, out string name, out string value)
        {
            if (input.StartsWith(startwith))
            {
                var str = input.Substring(startwith.Length);
                int index = str.IndexOf('=');
                name = str.Substring(0, index);
                value = str.Substring(index + 1);
                return true;
            }
            name = default;
            value = default;
            return false;
        }

        public static JObject ToJObject(Dictionary<string,string> sets)
        {
            JObject baseObj = new JObject();
            foreach (var item in sets)
            {
                var value = JToken.Parse(item.Value);
                NewNode(baseObj, item.Key, value);
            }
            return baseObj;
        }

        // code from https://stackoverflow.com/questions/56427214/how-to-add-a-new-jproperty-to-a-json-based-on-path
        private static JToken NewNode(JObject baseObj, string key, JToken value)
        {
            var pathParts = key.Split('.');
            JToken node = baseObj;
            for (int i = 0; i < pathParts.Length; i++)
            {
                var pathPart = pathParts[i];
                var partNode = node.SelectToken(pathPart);
                if (partNode == null && i < pathParts.Length - 1)
                {
                    ((JObject)node).Add(pathPart, new JObject());
                    partNode = node.SelectToken(pathPart);
                }
                else if (partNode == null && i == pathParts.Length - 1)
                {
                    ((JObject)node).Add(pathPart, value);
                    partNode = node.SelectToken(pathPart);
                }
                node = partNode;
            }
            return node;
        }
    }
}
