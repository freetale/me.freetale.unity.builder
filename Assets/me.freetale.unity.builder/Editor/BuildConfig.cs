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
            var text = File.ReadAllText(file);
            var config = JObject.Parse(text);
            return FromJObject(config);
        }

        public static BuildConfig FromJObject(JObject obj)
        {
            var config = new BuildConfig();
            config.Targets = new List<Target>();
            foreach (var target in obj.GetValue("Targets").Children())
            {
                config.Targets.Add(Target.FromJObject((JObject)target));
            };
            return config;
        }
    }
}
