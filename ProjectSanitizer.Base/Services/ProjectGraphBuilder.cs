using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using System;

namespace ProjectSanitizer.Base.Services
{
    public class ProjectGraphBuilder : IProjectGraphBuilder
    {
        private readonly IProjectReader _projectReader;
        private readonly INugetReferenceReader _nugetReferenceReader;

        public ProjectGraphBuilder(IProjectReader projectReader, INugetReferenceReader nugetReferenceReader)
        {
            _projectReader = projectReader;
            _nugetReferenceReader = nugetReferenceReader;
        }

        public SolutionGraph BuildGraph(VerifiedFile csProjFile)
        {
            return BuildGraph(_projectReader.ReadProject(csProjFile));
        }

        public SolutionGraph BuildGraph(Project project)
        {
            var root = new ProjectGraphNode(project);
            var graph = new SolutionGraph(null); 
            graph.AddNode(root, isSolutionProject: true);
            ExpandGraphNode(graph, root);
            return graph;
        }

        private void ExpandGraphNode(SolutionGraph graph, ProjectGraphNode node)
        {
            if (node.IsAlreadyExpanded)
                return;

            foreach (var projectRef in node.Project.ProjectReferences)
                node.ProjectRequirements.Add(CreateGraphNode(graph, projectRef));

            var nugetPackages = _nugetReferenceReader.TryReadPackagesConfig(node.Project.ProjectDirectory);

            foreach (var fileReference in node.Project.FileReferences)
            {
                var nugetPackage = nugetPackages?.FindPackage(fileReference.Include.ID);
                if (nugetPackage != null)
                {
                    var nugetReference = new NugetReference(nugetPackage, fileReference.TryGetFile(), fileReference.VersionFromPath);
                    node.NugetPackageRequirements.Add(nugetReference);
                }
                else
                {
                    var reference = new ReferencedFile(fileReference.TryGetFile(), fileReference.Include.Version);
                    node.FileRequirements.Add(reference);
                }
            } 
        }

        private ProjectGraphNode CreateGraphNode(SolutionGraph graph, ProjectReference projectReference)
        {
            var projectFile = projectReference.TryGetFile();
            if (projectFile == null)
                throw new NotImplementedException("Missing projects are not handled yet");

            var project = _projectReader.ReadProject(projectFile);
            var graphNode = graph.GetOrAdd(project, isSolutionProject:false);
            ExpandGraphNode(graph, graphNode);
            return graphNode;
        }

        public SolutionGraph BuildGraph(Solution solution)
        {
            var graph = new SolutionGraph(solution);
            foreach (var project in solution.Projects)
            {
                var projectNode = graph.GetOrAdd(project, isSolutionProject: true);
                ExpandGraphNode(graph, projectNode);
            }

            return graph;
        }
    }
}
