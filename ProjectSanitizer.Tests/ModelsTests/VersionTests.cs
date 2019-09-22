using NUnit.Framework;
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

            versions.Add(new VersionWithSuffix("9.3.2"));
            versions.Add(new VersionWithSuffix("9.3.2-alpha3"));
            versions.Add(new VersionWithSuffix("9.3.2-alpha1"));
            versions.Add(new VersionWithSuffix("9.2"));

            var sorted = versions.OrderByDescending(v => v).ToArray();

            Assert.AreEqual(sorted[0].ToString(), "9.3.2");
            Assert.AreEqual(sorted[1].ToString(), "9.3.2-alpha3");
            Assert.AreEqual(sorted[2].ToString(), "9.3.2-alpha1");
            Assert.AreEqual(sorted[3].ToString(), "9.2");
        }

        [TestCase("1.2.3-beta5", "1.2.3-beta5", true)]
        [TestCase("1.2.3-beta5", "1.2.3", false)]
        [TestCase("1.2.0", "1.2", true)]
        [TestCase("2.2.0", "1.2.0", false)]
        public void TestVersionEquality(string v1, string v2, bool shouldBeEqual)
        {
            var version1 = new VersionWithSuffix(v1);
            var version2 = new VersionWithSuffix(v2);

            Assert.AreEqual(shouldBeEqual, version1.Equals(version2));
            Assert.AreEqual(shouldBeEqual, version1 == version2);
            Assert.AreNotEqual(shouldBeEqual, version1 != version2);
        }
    }
}
