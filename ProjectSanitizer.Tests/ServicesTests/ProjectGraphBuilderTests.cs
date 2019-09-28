﻿using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    class ProjectGraphBuilderTests
    {
        [TestCase(@"ExampleBrokenSolutions\FirstProject\FirstProject.csproj", "VS2017Project,AnotherProject")]
        public void CanBuildProjectGraph(string relativeCsProjPath, string expectedDependencyPath)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var projectReader = DIRegistrar.GetInstance<IProjectReader>();
            var project = projectReader.ReadProject(csProjPath);

            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var node = graphBuilder.BuildGraph(project).SolutionProjects.Single();

            foreach (var expectedDependency in expectedDependencyPath.Split(','))
            {
                var nextNode = node.ProjectRequirements.FirstOrDefault(proj => proj.Project.Name == expectedDependency);
                Assert.IsNotNull(nextNode);
                node = nextNode;
            }
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln",5)]
        public void CanBuildProjectGraphFromSolution(string relativeSlnPath, int expectedProjects)
        {
            var slnFile = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnFile);
            var graph = DIRegistrar.GetInstance<IProjectGraphBuilder>().BuildGraph(solution);

            Assert.AreEqual(expectedProjects, graph.SolutionProjects.Count);
        }

        [TestCase(@"ExampleBrokenSolutions\FirstProject\FirstProject.csproj", "Newtonsoft.Json")]
        public void CanFindNugetReferencesInProjectGraph(string relativeCsProjPath, string expectedPackage)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var projectReader = DIRegistrar.GetInstance<IProjectReader>();
            var project = projectReader.ReadProject(csProjPath);

            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graphNode = graphBuilder.BuildGraph(project);

            var reference = graphNode.AllNodes
                .Values
                .SelectMany(node => node.NugetPackageRequirements)
                .FirstOrDefault(p => p.Package.ID == expectedPackage);

            Assert.IsNotNull(reference);
        }

        [TestCase(@"ExampleBrokenSolutions\FirstProject\FirstProject.csproj")]
        [TestCase(@"ExampleBrokenSolutions\ThirdProject\ThirdProject.csproj")]
        public void AllNugetReferencesHavePaths(string relativeCsProjPath)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var service = DIRegistrar.GetInstance<IProjectReader>();
            var proj = service.ReadProject(csProjPath);

            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(proj).AllNodes[proj.FullPath];

            foreach (var req in graph.NugetPackageRequirements)
            {
                Assert.IsNotNull(req.File);
            }
        }

    }
}
