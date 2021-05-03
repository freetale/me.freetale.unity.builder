using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace FreeTale.Unity.Builder
{
    public struct BuildConfig
    {
        public List<TargetConfig> Targets;

        public static BuildConfig FromFile(string file)
        {
            using (var reader = new StreamReader(file))
            {
                var parser = new Parser(reader);
                var mergeParser = new MergingParser(parser);
                var deserializer = new Deserializer();
                return deserializer.Deserialize<BuildConfig>(mergeParser);
            }
            
            var config = JObject.Parse(text);
            return FromJObject(config, sets);
        }

        public static BuildConfig FromJObject(JObject obj, JObject sets)
        {
            var config = new BuildConfig();
            config.Targets = new List<TargetConfig>();
            foreach (var target in obj.GetValue("Targets").Children())
            {
                config.Targets.Add(TargetConfig.FromJObject((JObject)target));
            };
            return config;
        }

        public BuildInfo ToBuildInfo()
        {
            
        }
    }

    public struct BuildInfo
    {
        public Target[] Targets;
    }
}
