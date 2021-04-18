using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FreeTale.Unity.Builder
{
    public struct BuildConfig
    {
        public List<Target> Targets;

        public static BuildConfig FromFile(string file)
        {
            var config = JObject.Parse(file);
            return FromJObject(config);

        }

        public static BuildConfig FromJObject(JObject obj)
        {
            var config = new BuildConfig();
            config.Targets = new List<Target>();
            obj.GetValue("targets");
            return config;
        }
    }
}
