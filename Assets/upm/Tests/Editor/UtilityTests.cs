using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FreeTale.Unity.Builder.Tests
{
    public class UtilityTests
    {
        [TestCase]
        //[TestCaseSource(nameof(GetBuildPlayerOptionTestCase))]
        public void GetBuildPlayerOptionTest()
        {
            JObject obj = new JObject
            {
                { "scenes", new JArray("scene1")},
                { "locationPathName", "path/test.data" }
            };
            BuildPlayerOptions expect = new BuildPlayerOptions
            {
                scenes = new string[] { "scene1" },
                locationPathName = "path/test.data",
            };
            var actual = Utility.ParseBuildPlayerOptions(obj);

            Assert.AreEqual(expect.scenes, actual.scenes);
            Assert.AreEqual(expect.locationPathName, actual.locationPathName);
            Assert.AreEqual(expect.assetBundleManifestPath, actual.assetBundleManifestPath);
            Assert.AreEqual(expect.targetGroup, actual.targetGroup);
            Assert.AreEqual(expect.target, actual.target);
            Assert.AreEqual(expect.options, actual.options);
        }

        public static object GetBuildPlayerOptionTestCase()
        {
            return new object[]{
                new object[]
                {
                    new JObject(new JProperty("scenes", new JArray("scene1"))),
                    new BuildPlayerOptions{ scenes = new []{ "scene1" }},
                },
                new object[]{
                    new JObject(),
                    new BuildPlayerOptions{ scenes = null, },
                },
                new object[]
                {
                    new JObject(new JProperty("locationPathName", "path/test.data")),
                    new BuildPlayerOptions{ locationPathName = "path/test.data" },
                },
                new object[]
                {
                    new JObject(new JProperty("assetBundleManifestPath", "testPath.json")),
                    new BuildPlayerOptions{ assetBundleManifestPath = "testPath.json" },
                },
                new object[]
                {
                    new JObject(new JProperty("targetGroup", "Standalone")),
                    new BuildPlayerOptions{ targetGroup = BuildTargetGroup.Standalone },
                },
                new object[]
                {
                    new JObject(new JProperty("target", "StandaloneWindows")),
                    new BuildPlayerOptions{ target = BuildTarget.StandaloneWindows },
                },
                new object[]
                {
                    new JObject(new JProperty("options", new JArray("AllowDebugging", "AutoRunPlayer"))),
                    new BuildPlayerOptions{ options = BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer },
                },
                new object[]
                {
                    new JObject(new JProperty("options", "AllowDebugging")),
                    new BuildPlayerOptions{ options = BuildOptions.AllowDebugging },
                },
            };
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
