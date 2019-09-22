using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Tests.MockServices;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class ProblemDetectorTests
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "MissingProject")]
        public void CanIdentifyMissingProjectFiles(string relativeSlnPath, string missingProjectName)
        {
            var slnPath = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            var service = new SolutionReader(new EmptyProjectReader());
            var sln = service.ReadSolution(slnPath);

            var missingProjectDetector = DIRegistrar
                .GetImplementations<IProblemDetector>()
                .OfType<IProblemDetector<Solution>>()
                .First();

            var problem = missingProjectDetector.DetectProblems(sln).FirstOrDefault();
            Assert.AreEqual($"Project \"{missingProjectName}\" was not found", problem.Description);
        }
    }
}
