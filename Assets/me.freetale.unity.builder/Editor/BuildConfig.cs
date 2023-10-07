using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace FreeTale.Unity.Builder
{
    public class BuildConfig
    {
        public object Anchor; // allow yaml anchor to deserialize

        public List<Target> Targets;

        public static BuildConfig FromFile(string file)
        {
            var text = File.ReadAllText(file);
#if FTBUILDER_YAML
            var parser = new YamlDotNet.Core.MergingParser(new YamlDotNet.Core.Parser(new StringReader(text)));
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithTypeConverter(new EnumTypeConverter())
                .Build();
            return deserializer.Deserialize<BuildConfig>(parser);
#elif FTBUILDER_JSON
            return Newtonsoft.Json.JsonConvert.DeserializeObject<BuildConfig>(text, new FlagConverter());
#else

            throw new NotSupportedException("No support type please spacify FTBUILDER_YAML or FTBUILDER_JSON in script define symbols");
#endif
        }
    }
}
