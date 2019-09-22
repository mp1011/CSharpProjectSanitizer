using ProjectSanitizer.Base.Models.SolutionStructure;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Models.ProjectGraph
{
    public class ProjectGraphNode
    {
        public Project Project { get; }

        public List<ProjectGraphNode> ProjectRequirements { get; } = new List<ProjectGraphNode>();
        public List<ReferencedFile> FileRequirements { get; } = new List<ReferencedFile>();
        public List<NugetReference> NugetPackageRequirements { get; } = new List<NugetReference>();

        public override string ToString()
        {
            return Project.FullPath;
        }

        public ProjectGraphNode(Project project)
        {
            Project = project;
        }
    }
}
