using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTale.Unity.Builder.Tests
{
    public class TargetTests
    {
        [Test]
       public void ApplyOverrideBuildPlayerOptionsTest()
        {
            var target = new Target();
            target.ApplyOverride("BuildPlayerOptions.LocationPathName", "localtion");
            Assert.AreEqual("localtion", target.BuildPlayerOptions.LocationPathName);
        }


        [Test]
        public void ApplyOverrideStaticPropertiesTest()
        {
            var target = new Target();
            target.ApplyOverride("StaticProperties.\"UnityEditor.PlayerSettings,UnityEditor\".bundleVersion", "5");
            Assert.AreEqual("5", target.StaticProperties["UnityEditor.PlayerSettings,UnityEditor"]["bundleVersion"]);
        }
    }
}
