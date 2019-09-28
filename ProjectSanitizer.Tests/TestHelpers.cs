using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;

namespace ProjectSanitizer.Tests
{
    public static class TestHelpers
    {
        public static Solution GetSolution(string relativeSlnPath)
        {
            var path = TestPaths.GetFileRelativeToProjectDir(relativeSlnPath);
            return DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(path);
        }

        public static ProjectGraphNode GetProjectNode(string relativeSlnPath, string project)
        {
            var graph = GetSolutionGraph(relativeSlnPath);
            return graph.AllNodes.Values.First(p => p.Project.Name == project);
        }

        public static SolutionGraph GetSolutionGraph(string relativeSlnPath)
        {
            var solution = GetSolution(relativeSlnPath);
            var graphBuilder = DIRegistrar.GetInstance<IProjectGraphBuilder>();
            var graph = graphBuilder.BuildGraph(solution);
            return graph;
        }
    }
}
