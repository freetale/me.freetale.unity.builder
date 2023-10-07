#if FTBUILDER_YAML
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace FreeTale.Unity.Builder
{

    public class EnumTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type.IsEnum;

        public object ReadYaml(IParser parser, Type type)
        {
            var scalar = parser.Consume<Scalar>();
            try
            {
                return Enum.Parse(type, scalar.Value);
            }
            catch (Exception ex)
            {
                // Return the default value of the enum if the value is not present in the YAML file.
                return Enum.GetValues(type).GetValue(0);
            }
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(value.ToString()));
        }
    }
}
#endif