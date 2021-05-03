using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FreeTale.Unity.Builder.Tests
{
    public class UtilityTests
    {
        [TestCase]
        public void GetBuildPlayerOptionTest()
        {
            JObject obj = new JObject
            {
                { "scenes", new JArray("scene1")},
                { "locationPathName", "path/test.data" },
                { "targetGroup", "Standalone" },
                { "target", 5 }, //BuildTarget.StandaloneWindows
            };
            BuildPlayerOptions expect = new BuildPlayerOptions
            {
                scenes = new string[] { "scene1" },
                locationPathName = "path/test.data",
                targetGroup = BuildTargetGroup.Standalone,
                target = BuildTarget.StandaloneWindows,
            };
            var actual = Utility.ParseBuildPlayerOptions(obj);

            Assert.AreEqual(expect.scenes, actual.scenes);
            Assert.AreEqual(expect.locationPathName, actual.locationPathName);
            Assert.AreEqual(expect.assetBundleManifestPath, actual.assetBundleManifestPath);
            Assert.AreEqual(expect.targetGroup, actual.targetGroup);
            Assert.AreEqual(expect.target, actual.target);
            Assert.AreEqual(expect.options, actual.options);
        }

        [Test]
        public void ParseStaticPropertiesTest()
        {
            var obj = new JObject(
                new JProperty("PlayerSettings", new JObject(
                    new JProperty("keystorePass", "test-keystore"))));

            var actual = Utility.ParseStaticProperties(obj);
            var expectType = typeof(PlayerSettings);
            StaticProperty expect = new StaticProperty
            {
                PropertyInfo = expectType.GetProperty("keystorePass"),
                Value = "test-keystore",
            };
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(expect.PropertyInfo, actual[0].PropertyInfo);
            Assert.AreEqual(expect.Value, actual[0].Value);
        }

        [Test]
        public void ParseStaticPropertiesTest2()
        {
            var obj = new JObject(
                new JProperty("UnityEditor.PlayerSettings,UnityEditor", new JObject(
                    new JProperty("keystorePass", "test-keystore"))));

            var actual = Utility.ParseStaticProperties(obj);
            var expectType = typeof(PlayerSettings);
            StaticProperty expect = new StaticProperty
            {
                PropertyInfo = expectType.GetProperty("keystorePass"),
                Value = "test-keystore",
            };
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(expect.PropertyInfo, actual[0].PropertyInfo);
            Assert.AreEqual(expect.Value, actual[0].Value);
        }

        [Test]
        public void ParseStaticPropertyPropertyTest()
        {
            JProperty prop = new JProperty("bundleVersionCode", 1);
            var type = typeof(PlayerSettings.Android);
            var actual = Utility.ParseStaticPropertyProperty(type, prop);
            var expectType = typeof(PlayerSettings);
            StaticProperty expect = new StaticProperty
            {
                PropertyInfo = type.GetProperty("bundleVersionCode"),
                Value = 1,
            };
            Assert.AreEqual(expect.PropertyInfo, actual.PropertyInfo);
            Assert.AreEqual(expect.Value, actual.Value);

        }


        [Test]
        public void RequireStringError()
        {
            JObject obj = new JObject();
            Assert.Throws(typeof(UnexpectConfigException), () => Utility.RequireString(obj, "no-value"));
        }

        [Test]
        public void RequireStringOk()
        {
            JObject obj = new JObject(new JProperty("value", "value1"));
            var actual = Utility.RequireString(obj, "value");
            Assert.AreEqual("value1", actual);
        }


        [Test]
        public void OptionalStringError()
        {
            JObject obj = new JObject();
            var actual = Utility.OptionalString(obj, "no-value");
            Assert.AreEqual(null, actual);
        }

        [Test]
        public void OptionalStringOk()
        {
            JObject obj = new JObject(new JProperty("value", "value1"));
            var actual = Utility.OptionalString(obj, "value");
            Assert.AreEqual("value1", actual);
        }

        [System.Flags]
        private enum TestEnum
        {
            Value0 = 0,
            Value1 = 1,
            Value2 = 2,
        }

        [Test]
        public void RequireEnumError()
        {
            JObject obj = new JObject();
            Assert.Throws(typeof(UnexpectConfigException), () => Utility.RequireEnum<TestEnum>(obj, "value"));
        }

        [Test]
        public void OptionalEnumFlag()
        {
            JObject obj = new JObject
            {
                { "value", new JArray{
                    "Value1",
                    "Value2",
                }}
            };
            var actual = Utility.OptionalEnum<TestEnum>(obj, "value");
            Assert.AreEqual(TestEnum.Value1 | TestEnum.Value2, actual);
        }

        [Test]
        public void TryParseEnumTestString()
        {
            var token = "Value1";
            var actual = Utility.TryParseEnum<TestEnum>(token, out var result);
            Assert.IsTrue(actual);
            Assert.AreEqual(TestEnum.Value1, result);
        }

        [Test]
        public void TryParseEnumTestInt()
        {
            var token = 1;
            var actual = Utility.TryParseEnum<TestEnum>(token, out var result);
            Assert.IsTrue(actual);
            Assert.AreEqual(TestEnum.Value1, result);
        }

        [Test]
        public void TryParseEnumTestArrayString()
        {
            var token = new JArray{
                    "Value1",
                    "Value2",
                };
            var actual = Utility.TryParseEnum<TestEnum>(token, out var result);
            Assert.IsTrue(actual);
            Assert.AreEqual(TestEnum.Value1 | TestEnum.Value2, result);
        }
        [Test]
        public void TryParseEnumTestArrayInt()
        {
            var token = new JArray{
                    1,
                    2,
                };
            var actual = Utility.TryParseEnum<TestEnum>(token, out var result);
            Assert.IsTrue(actual);
            Assert.AreEqual(TestEnum.Value1 | TestEnum.Value2, result);
        }
    }
}
