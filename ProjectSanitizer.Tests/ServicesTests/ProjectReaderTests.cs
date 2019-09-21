using NUnit.Framework;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class ProjectReaderTests
    {
        [TestCase("...")]
        public void CanReadProject(string csProjPath)
        {
            Assert.Fail();
        }
    }
}
