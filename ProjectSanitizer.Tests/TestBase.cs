using NUnit.Framework;

namespace ProjectSanitizer.Tests
{
    [TestFixture]
    public class TestBase
    {
        [SetUp]
        public void Setup()
        {
            TestPaths.RevertAllCsProjAndPackagesConfigFiles();
        }
    }
}
