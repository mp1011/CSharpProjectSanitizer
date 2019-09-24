using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Base.Services.ProblemDetectors;
using ProjectSanitizer.Models.Problems;
using ProjectSanitizer.Services.ProblemDetectors;
using ProjectSanitizer.Tests.MockServices;
using System;
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

            var problem = missingProjectDetector.DetectProblems(sln).FirstOrDefault() as MissingProject;
            Assert.AreEqual(missingProjectName, problem.Project.Name);
        }

        [TestCase(@"ExampleBrokenSolutions\AnotherProject\AnotherProject.csproj")]
        public void CanIdentifyNugetVersionMismatch(string projectFile)
        {
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(TestPaths.GetFileRelativeToProjectDir(projectFile));
            var node = graph.SolutionProjects.Single();

            var nugetMismatchDetector = new NugetVersionMismatchDetector();
            var problem = nugetMismatchDetector.DetectProblems(node.NugetPackageRequirements[0]).First();
            Assert.AreEqual(typeof(NugetVersionMismatch), problem.GetType());
        }


        [TestCase(@"ExampleBrokenSolutions\FirstProject\FirstProject.csproj")]
        public void CanIdentifyInconsistentNugetPackageVersions(string projectFile)
        {
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(TestPaths.GetFileRelativeToProjectDir(projectFile));

            var nugetMismatchDetector = new DifferentNugetVersionsDetector();
            var problem = nugetMismatchDetector.DetectProblems(graph).First();
            Assert.AreEqual(typeof(MultipleNugetVersions), problem.GetType());
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", 4)]
        public void CanFindAllProblemsWithSolution(string relativeSlnPath, int expectedNumberOfProblems)
        {
            var slnPath = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetector>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);
            Assert.AreEqual(expectedNumberOfProblems, problems.Length);
        }


        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "OlderDotNet")]
        public void CanDetectProjectDependingOnHigherDotNetVersion(string relativeSlnPath, string project)
        {
            var slnPath = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(solution);
            var projectNode = graph.AllNodes.Values.Single(p => p.Project.Name == project);

            var dependsOnHigherFrameworkDetector = new DependsOnIncompatibleFrameworkDetector();
            var problem = dependsOnHigherFrameworkDetector.DetectProblems(projectNode).First();
            Assert.AreEqual(typeof(DependsOnIncompatibleFramework), problem.GetType());
        }
    }
}