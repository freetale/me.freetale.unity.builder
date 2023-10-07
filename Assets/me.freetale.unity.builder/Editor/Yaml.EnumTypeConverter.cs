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
            Scalar scalar;
            if (parser.TryConsume(out scalar))
            {
                return ParseEnum(type, scalar);
            }
            if (parser.TryConsume<SequenceStart>(out _))
            {
                var enumValue = 0;
                while (parser.TryConsume(out scalar))
                {
                    enumValue |= (int)ParseEnum(type, scalar);
                }
                parser.Consume<SequenceEnd>();
                return enumValue;
            }
            throw new Exception($"Can't convert {parser.Current} to {type.Name} only [{string.Join(", ", Enum.GetNames(type))}] can be used");
        }

        private static object ParseEnum(Type type, Scalar scalar)
        {
            if (Enum.TryParse(type, scalar.Value, out var result))
            {
                return result;
            }
            else
            {
                throw new Exception($"Can't convert {scalar} to {type.Name} only [{string.Join(", ", Enum.GetNames(type))}] can be used");
            }
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(value.ToString()));
        }
    }
}
#endif