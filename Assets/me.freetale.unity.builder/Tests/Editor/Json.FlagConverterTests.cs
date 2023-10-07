#if FTBUILDER_JSON
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTale.Unity.Builder.Tests
{
    public class FlagConverterTests
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
        public void SerializeDeserialize()
        {
            var converter = new FlagConverter();
            string serialized = JsonConvert.SerializeObject(new TestClass
            {
                value = TestEnum.A | TestEnum.C, 
            }, converter);

            Debug.Log(serialized);  

            TestClass result = JsonConvert.DeserializeObject<TestClass>(serialized, converter);
            Assert.AreEqual(TestEnum.A | TestEnum.C, result.value);
        }


        [Test]
        public void DeserializeString()
        {
            var converter = new FlagConverter();
            string serialized = "{\"value\":\"A\"}";
            TestClass result = JsonConvert.DeserializeObject<TestClass>(serialized, converter);
            Assert.AreEqual(TestEnum.A, result.value);
        }


        [Test]
        public void DeserializeArray()
        {
            var converter = new FlagConverter();
            string serialized = "{\"value\":[\"A\", \"C\"]}";
            TestClass result = JsonConvert.DeserializeObject<TestClass>(serialized, converter);
            Assert.AreEqual(TestEnum.A | TestEnum.C, result.value);
        }
    }
}
#endif