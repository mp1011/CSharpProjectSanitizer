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
    public class ProblemDetectorTests :TestBase
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "MissingProject")]
        public void CanIdentifyMissingProjectFiles(string relativeSlnPath, string missingProjectName)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var service = new SolutionReader(new EmptyProjectReader());
            var sln = service.ReadSolution(slnPath);

            var missingProjectDetector = new MissingProjectDetector();

            var problem = missingProjectDetector.DetectProblems(sln).FirstOrDefault() as MissingProject;
            Assert.AreEqual(missingProjectName, problem.Project.Name);
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "AnotherProject")]
        public void CanIdentifyNugetVersionMismatch(string slnPath, string project)
        {
            var sln = TestHelpers.GetSolution(slnPath); 
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(sln);
            var node = graph.SolutionProjects.First(p => p.Project.Name == project);

            var nugetMismatchDetector = new NugetVersionMismatchDetector();
            var problem = nugetMismatchDetector.DetectProblems(node.NugetPackageRequirements[0]).First();
            Assert.AreEqual(typeof(NugetVersionMismatch), problem.GetType());
        }


        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanIdentifyInconsistentNugetPackageVersions(string slnPath)
        {
            var sln = TestHelpers.GetSolution(slnPath);
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(sln);

            var nugetMismatchDetector = new DifferentNugetVersionsDetector();
            var problem = nugetMismatchDetector.DetectProblems(graph).First();
            Assert.AreEqual(typeof(MultipleNugetVersions), problem.GetType());
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", 7)]

        public void CanFindAllProblemsWithSolution(string relativeSlnPath, int expectedNumberOfProblems)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetectorService>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);
            Assert.AreEqual(expectedNumberOfProblems, problems.Length);
        }


        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "OlderDotNet")]
        public void CanDetectProjectDependingOnHigherDotNetVersion(string relativeSlnPath, string project)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(solution);
            var projectNode = graph.AllNodes.Values.Single(p => p.Project.Name == project);

            var dependsOnHigherFrameworkDetector = new DependsOnIncompatibleFrameworkDetector();
            var problem = dependsOnHigherFrameworkDetector.DetectProblems(projectNode).First();
            Assert.AreEqual(typeof(DependsOnIncompatibleFramework), problem.GetType());
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln","FirstProject")]
        public void CanDetectFileReferenceWhereProjectReferenceIsMoreAppropriate(string relativeSlnPath, string project)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(solution);
            var projectNode = graph.AllNodes.Values.Single(p => p.Project.Name == project);

            var detector = new FileReferenceInsteadOfProjectReferenceDetector();
            var problem = detector.DetectProblems(projectNode).First();
            Assert.AreEqual("Project FirstProject has a file reference instead of a project reference to AnotherProject", problem.Description.ToString());
        }


        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "VS2017Project")]
        public void CanDetectFileReferenceWhereNugetReferenceIsMoreAppropriate(string relativeSlnPath, string project)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(solution);
            var projectNode = graph.AllNodes.Values.Single(p => p.Project.Name == project);

            var detector = new FileReferenceInsteadOfNugetReferenceDetector();
            var problem = detector.DetectProblems(projectNode).First();
            Assert.AreEqual("Project VS2017Project has a file reference instead of a nuget reference to Newtonsoft.Json", problem.Description.ToString());
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanDetectSameDLLReferencedInDifferentPaths(string relativeSlnPath)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(solution);

            var detector = new FileReferencedInMultipleWaysDetector();
            var problem = detector.DetectProblems(graph).First();
            Assert.That(problem.Description.ToString().StartsWith("The file Newtonsoft.Json is referenced in multiple paths"));
        }
    }
}