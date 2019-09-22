using ProjectSanitizer.Base.Models;
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

        public ProjectGraph BuildGraph(Project project)
        {
            var root = new ProjectGraphNode(project);
            var graph = new ProjectGraph(root);
            ExpandGraphNode(graph, root);
            return graph;
        }

        private void ExpandGraphNode(ProjectGraph graph, ProjectGraphNode node)
        {
            foreach (var projectRef in node.Project.ProjectReferences)
                node.ProjectRequirements.Add(CreateGraphNode(graph, projectRef));

            var nugetPackages = _nugetReferenceReader.TryReadPackagesConfig(node.Project.ProjectDirectory);

            foreach (var fileReference in node.Project.FileReferences)
            {
                var nugetPackage = nugetPackages?.FindPackage(fileReference.Include.ID);
                if (nugetPackage != null)
                {
                    var nugetReference = new NugetReference(nugetPackage, fileReference.TryGetFile(), fileReference.Include.Version);
                    node.NugetPackageRequirements.Add(nugetReference);
                }
                else
                {
                    var reference = new ReferencedFile(fileReference.TryGetFile(), fileReference.Include.Version);
                    node.FileRequirements.Add(reference);
                }
            } 
        }

        private ProjectGraphNode CreateGraphNode(ProjectGraph graph, ProjectReference projectReference)
        {
            var projectFile = projectReference.TryGetFile();
            if (projectFile == null)
                throw new NotImplementedException("Missing projects are not handled yet");

            var project = _projectReader.ReadProject(projectFile);
            var graphNode = graph.GetOrAdd(project);
            ExpandGraphNode(graph, graphNode);
            return graphNode;
        }
    }
}
