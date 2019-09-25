using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Services;

namespace ProjectSanitizer.Models.Problems
{

    public class FileReferenceInsteadOfProjectReference : Problem<ProjectGraphNode>
    {
        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("Project ").AppendHighlighted(Item.Project)
            .AppendError(" has a file reference instead of a project reference to ")
            .AppendHighlighted(MissingProject);

        public Project MissingProject { get; }

        public FileReferenceInsteadOfProjectReference(ProjectGraphNode projectNode, Project missingReference)
            : base(projectNode)
        {
            MissingProject = missingReference;
        }
    }
}
