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

        [Test]
        public void TestVersionEquality()
        {
            var version = new VersionWithSuffix("1.2.3-beta5");
            var sameVersion = new VersionWithSuffix("1.2.3-beta5");
            var differentVersion = new VersionWithSuffix("1.2.2-beta5");

            Assert.That(version.Equals(sameVersion));
            Assert.That(version == sameVersion);
            Assert.That(!version.Equals(differentVersion));
            Assert.That(version != differentVersion);
        }
    }
}
