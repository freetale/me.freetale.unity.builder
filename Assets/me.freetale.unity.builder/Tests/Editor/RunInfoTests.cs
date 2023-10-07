using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTale.Unity.Builder.Tests
{
    public class RunInfoTests
    {
        [Test]
        public void FromArgsTest()
        {
            RunInfo expect = new RunInfo
            {
                Config = "config.json",
                Target = "test-target",
            };

            var actual = RunInfo.FromArgs(new[] { "--target=test-target", "--config=config.json", "--setting=other" });
            Assert.AreEqual(expect.Config, actual.Config);
            Assert.AreEqual(expect.Target, actual.Target);
        }

        [TestCase("--set=path.to.config=4", "path.to.config", "4")]
        public void GetSettingTest(string input, string name, string value)
        {
            _ = RunInfo.GetSetting(input, "--set=", out var actualName, out var actualValue);
            Assert.AreEqual(name, actualName);
            Assert.AreEqual(value, actualValue);
        }

    }
}
