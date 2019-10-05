using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Models;
using ProjectSanitizer.Models.Problems;
using ProjectSanitizer.Services;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    public class ProblemCorrectorTests : TestBase
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanCorrectSolutionProblems(string slnPath)
        {
            var correctorService = DIRegistrar.GetInstance<ProblemCorrectorService>();
            var detectorService = DIRegistrar.GetInstance<ProblemDetectorService>();

        
           
            int remainingProblemCount = int.MaxValue;
            int previousRemainingProblemCount=0;

            while(remainingProblemCount > 0)
            {
                var sln = TestHelpers.GetSolution(slnPath);

                if (remainingProblemCount == previousRemainingProblemCount)
                    Assert.Fail("Did not solve any problems in this loop");

                previousRemainingProblemCount = remainingProblemCount;

                var problems = detectorService.DetectAllSolutionProblems(sln);
                remainingProblemCount = problems.OfType<MultipleNugetVersions>().Count() +
                                        problems.OfType<NugetVersionMismatch>().Count();

                var correction = problems.Select(p => correctorService.TryCorrectProblem(p))
                    .FirstOrDefault(p => p.Resolution != Resolution.NoActionTaken);
             
                if (correction == null)
                {
                    if (remainingProblemCount > 0)
                        Assert.Fail("Did not solve all problems");
                    else
                        return;
                }

                Assert.AreNotEqual(Resolution.FailedToSolve, correction.Resolution);
            }
        }
    }
}
