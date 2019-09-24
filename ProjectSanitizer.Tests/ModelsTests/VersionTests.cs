﻿using NUnit.Framework;
using ProjectSanitizer.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSanitizer.Tests.ModelsTests
{
    [TestFixture]
    public class VersionTests
    {
        [Test]
        public void TestVersionOrdering()
        {
            List<VersionWithSuffix> versions = new List<VersionWithSuffix>();

            versions.Add(VersionWithSuffix.TryParse("9.3.2"));
            versions.Add(VersionWithSuffix.TryParse("9.3.2-alpha3"));
            versions.Add(VersionWithSuffix.TryParse("9.3.2-alpha1"));
            versions.Add(VersionWithSuffix.TryParse("9.2"));

            var sorted = versions.OrderByDescending(v => v).ToArray();

            Assert.AreEqual(sorted[0].ToString(), "9.3.2");
            Assert.AreEqual(sorted[1].ToString(), "9.3.2-alpha3");
            Assert.AreEqual(sorted[2].ToString(), "9.3.2-alpha1");
            Assert.AreEqual(sorted[3].ToString(), "9.2");
        }

        [TestCase(@"\packages\Newtonsoft.Json.12.0.1\lib","12.0.1")]
        [TestCase(@"\packages\Newtonsoft.Json.12\lib", null)]
        [TestCase(@"\packages\Newtonsoft.Json.12.0-beta2\lib", "12.0-beta2")]
        [TestCase(@"\packages\Newtonsoft.Json.12.0.5-alpha64\lib", "12.0.5-alpha64")]

        public void CanParseVersionWithinPath(string path, string expectedVersion)
        {
            var version = VersionWithSuffix.TryParseFromPath(path);
            Assert.AreEqual(expectedVersion, version?.ToString());
        }

        [TestCase("1.2.3-beta5", "1.2.3-beta5", true)]
        [TestCase("1.2.3-beta5", "1.2.3", false)]
        [TestCase("1.2.0", "1.2", true)]
        [TestCase("2.2.0", "1.2.0", false)]
        public void TestVersionEquality(string v1, string v2, bool shouldBeEqual)
        {
            var version1 = VersionWithSuffix.TryParse(v1);
            var version2 = VersionWithSuffix.TryParse(v2);

            Assert.AreEqual(shouldBeEqual, version1.Equals(version2));
            Assert.AreEqual(shouldBeEqual, version1 == version2);
            Assert.AreNotEqual(shouldBeEqual, version1 != version2);
        }
    }
}
