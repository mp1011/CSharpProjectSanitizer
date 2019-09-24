using NUnit.Framework;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Tests.MockServices;

namespace ProjectSanitizer.Tests.ServicesTests
{
    public class SolutionReaderTests
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln",5)]
        public void CanReadNumberOfProjectsInSolution(string relativeSlnPath, int expectedProjects)
        {
            var slnPath = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            var service = new SolutionReader(new EmptyProjectReader());

            var sln = service.ReadSolution(slnPath);
            Assert.AreEqual(expectedProjects, sln.Projects.Length);
        }
    }
}
