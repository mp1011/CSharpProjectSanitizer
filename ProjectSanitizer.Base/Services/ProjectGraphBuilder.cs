using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;

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
            var graph = new SolutionGraph(null);
            var root = new ProjectGraphNode(project,graph);
            graph.AddNode(root, isSolutionProject: true);
            ExpandGraphNode(graph, root);
            return graph;
        }

        private void ExpandGraphNode(SolutionGraph graph, ProjectGraphNode node)
        {
            if (node.IsAlreadyExpanded)
                return;

            foreach (var projectRef in node.Project.ProjectReferences)
            {
                var projectRefNode = TryCreateGraphNode(graph, projectRef);
                if(projectRefNode != null)
                    node.ProjectRequirements.Add(projectRefNode);
            }

            var nugetPackages = _nugetReferenceReader.TryReadPackagesConfig(node.Project.ProjectDirectory);

            foreach (var fileReference in node.Project.FileReferences)
            {
                var nugetPackage = nugetPackages?.FindPackage(fileReference.Include.ID);
                if (nugetPackage != null)
                {
                    var nugetReference = new NugetReference(node.Project, nugetPackage, fileReference.GetFile(), fileReference.VersionFromPath);
                    node.NugetPackageRequirements.Add(nugetReference);
                }
                else
                {
                    var reference = new ReferencedFile(node.Project, fileReference.GetFile(), fileReference.Include.Version);
                    node.FileRequirements.Add(reference);
                }
            } 
        }

        private ProjectGraphNode TryCreateGraphNode(SolutionGraph graph, ProjectReference projectReference)
        {
            var projectFile = projectReference.GetFile();
            if (projectFile == null)
                return null;

            var file = projectFile.TryVerify();
            if (file == null)
                return null;

            var project = _projectReader.ReadProject(file);
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
