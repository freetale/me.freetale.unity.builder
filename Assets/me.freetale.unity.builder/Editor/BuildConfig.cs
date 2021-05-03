using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace FreeTale.Unity.Builder
{
    public struct BuildConfig
    {
        public List<Target> Targets;

        public static BuildConfig FromFile(string file)
        {
            return FromFile(file, null);
        }

        public static BuildConfig FromFile(string file, JObject sets)
        {
            var text = File.ReadAllText(file);
            var config = JObject.Parse(text);
            return FromJObject(config, sets);
        }

        public static BuildConfig FromJObject(JObject obj, JObject sets)
        {
            var config = new BuildConfig();
            config.Targets = new List<Target>();
            JToken targets = obj.GetValue("Targets");
            foreach (JObject target in targets.Children())
            {
                target.Merge(sets);
                config.Targets.Add(Target.FromJObject(target));
            };
            return config;
        }
    }
}
