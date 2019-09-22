using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Base.Services.ProblemDetectors;
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

            var missingProjectDetector = new MissingProjectDetector();

            var problem = missingProjectDetector.DetectProblems(sln).FirstOrDefault();
            Assert.AreEqual($"Project \"{missingProjectName}\" was not found", problem.Description);
        }

        [TestCase(@"ExampleBrokenSolutions\AnotherProject\AnotherProject.csproj", "Package Newtonsoft.Json should have version 12.0.1, but version 12.0 is referenced")]
        public void CanIdentifyNugetVersionMismatch(string projectFile, string expectedError)
        {
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(TestPaths.GetFileRelativeToProjectDir(projectFile));
            var node = graph.SolutionProjects.Single();

            var nugetMismatchDetector = new NugetVersionMismatchDetector();
            var problem = nugetMismatchDetector.DetectProblems(node.NugetPackageRequirements[0]).First();
            Assert.AreEqual(expectedError, problem.Description);
        }


        [TestCase(@"ExampleBrokenSolutions\FirstProject\FirstProject.csproj", "Multiple versions of Newtonsoft.Json found")]
        public void CanIdentifyInconsistentNugetPackageVersions(string projectFile, string expectedError)
        {
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(TestPaths.GetFileRelativeToProjectDir(projectFile));

            var nugetMismatchDetector = new DifferentNugetVersionsDetector();
            var problem = nugetMismatchDetector.DetectProblems(graph).First();
            Assert.AreEqual(expectedError, problem.Description);
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", 6)]
        public void CanFindAllProblemsWithSolution(string relativeSlnPath, int expectedNumberOfProblems)
        {
            var slnPath = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetector>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);
            Assert.AreEqual(expectedNumberOfProblems, problems.Length);
        }
    }
}