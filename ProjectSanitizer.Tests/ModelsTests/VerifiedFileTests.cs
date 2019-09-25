using NUnit.Framework;

namespace ProjectSanitizer.Tests.ModelsTests
{
    [TestFixture]
    class VerifiedFileTests
    {
        [TestCase(@"ModelsTests\VerifiedFileTests.cs", @"ModelsTests\verifiedfiletests.cs", true)]
        [TestCase(@"ModelsTests\VerifiedFileTests.cs", @"ModelsTests\VersionTests.cs", false)]
        [TestCase(@"ModelsTests\..\TestPaths.cs", @"TestPaths.cs", true)]
        public void CheckFileEquality(string filePath1, string filePath2, bool shouldBeEqual)
        {
            var file1 = TestPaths.GetFileRelativeToProjectDir(filePath1);
            var file2 = TestPaths.GetFileRelativeToProjectDir(filePath2);

            if(shouldBeEqual)
            {
                Assert.AreEqual(file1, file2);
                Assert.That(file1 == file2);
            }
            else
            {
                Assert.AreNotEqual(file1, file2);
                Assert.That(file1 != file2);
            }
        }
    }
}
