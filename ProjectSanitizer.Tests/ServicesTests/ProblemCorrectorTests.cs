using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Services;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    public class ProblemCorrectorTests
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanCorrectSolutionProblems(string slnPath)
        {
            TestPaths.RevertAllCsProjAndPackagesConfigFiles(slnPath);
            var correctorService = DIRegistrar.GetInstance<ProblemCorrectorService>();
            var detectorService = DIRegistrar.GetInstance<ProblemDetectorService>();

            var sln = TestHelpers.GetSolution(slnPath);

            var problems = detectorService.DetectAllSolutionProblems(sln);
            var corrections = problems
                .Select(p => correctorService.TryCorrectProblem(p))
                .ToArray();

            Assert.That(corrections.Any(c => c.Resolution == Models.Resolution.Solved));
            TestPaths.RevertAllCsProjAndPackagesConfigFiles(slnPath);
        }
    }
}
