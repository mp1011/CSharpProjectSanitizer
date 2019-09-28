using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    class SuperTest
    {
        [TestCase(@"C:\Users\Miko\Documents\GitHub\vdata\main\WebApps\API\ValiantAPI.sln")]
        public void Foo(string bar)
        {
            var slnFile = new VerifiedFile(bar);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnFile);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetector>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);
            Assert.Fail("Dont commit me");
        }
    }
}
