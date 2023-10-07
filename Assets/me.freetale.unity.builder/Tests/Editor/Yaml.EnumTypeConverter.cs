#if FTBUILDER_YAML
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FreeTale.Unity.Builder.Tests
{
    public class EnumTypeConverterTests
    {
        private enum TestEnum
        {
            Unknown = 0,
            A = 1,
            B = 2,
            C = 4,
        }
        private class TestClass
        {
            public TestEnum value;
        }

        [Test]
        public void DeserializeString()
        {
            string serialized = "value: A";
            var parser = new YamlDotNet.Core.MergingParser(new YamlDotNet.Core.Parser(new StringReader(serialized)));
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithTypeConverter(new EnumTypeConverter())
                .Build();
            var result = deserializer.Deserialize<TestClass>(parser);
            Assert.AreEqual(TestEnum.A, result.value);
        }


        [Test]
        public void DeserializeArray()
        {
            string serialized = "value:\n - A\n - C";
            var parser = new YamlDotNet.Core.MergingParser(new YamlDotNet.Core.Parser(new StringReader(serialized)));
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithTypeConverter(new EnumTypeConverter())
                .Build();
            var result = deserializer.Deserialize<TestClass>(parser);
            Assert.AreEqual(TestEnum.A | TestEnum.C, result.value);
        }
    }
}
#endif