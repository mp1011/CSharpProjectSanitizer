using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    class ProjectGraphBuilderTests
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "FirstProject", "VS2017Project,AnotherProject")]
        public void CanBuildProjectGraph(string sln, string project, string expectedDependencyPath)
        {
            var node = TestHelpers.GetProjectNode(sln, project);
            foreach (var expectedDependency in expectedDependencyPath.Split(','))
            {
                var nextNode = node.ProjectRequirements.FirstOrDefault(proj => proj.Project.Name == expectedDependency);
                Assert.IsNotNull(nextNode);
                node = nextNode;
            }
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln",6)]
        public void CanBuildProjectGraphFromSolution(string relativeSlnPath, int expectedProjects)
        {
            var slnFile = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnFile);
            var graph = DIRegistrar.GetInstance<IProjectGraphBuilder>().BuildGraph(solution);

            Assert.AreEqual(expectedProjects, graph.SolutionProjects.Count);
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "FirstProject", "Newtonsoft.Json")]
        public void CanFindNugetReferencesInProjectGraph(string slnPath, string project, string expectedPackage)
        {
            var graphNode = TestHelpers.GetProjectNode(slnPath, project); 

            var reference = graphNode.NugetPackageRequirements
                .FirstOrDefault(p => p.Package.ID == expectedPackage);

            Assert.IsNotNull(reference);
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void AllNugetReferencesHavePaths(string sln)
        {
            var graph = TestHelpers.GetSolutionGraph(sln);

            foreach (var project in graph.AllNodes.Values)
            {
                foreach (var req in project.NugetPackageRequirements)
                {
                    Assert.IsNotNull(req.File);
                }
            }
        }

    }
}
