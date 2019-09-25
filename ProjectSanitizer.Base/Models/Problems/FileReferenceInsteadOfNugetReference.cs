using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Services;

namespace ProjectSanitizer.Models.Problems
{
    public class FileReferenceInsteadOfNugetReference : Problem<ProjectGraphNode>
    {
        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("Project ").AppendHighlighted(Item.Project)
            .AppendError(" has a file reference instead of a nuget reference to ")
            .AppendHighlighted(MissingReference.Package.ID);

        public NugetReference MissingReference { get; }

        public FileReferenceInsteadOfNugetReference(ProjectGraphNode projectNode, NugetReference missingReference)
            : base(projectNode)
        {
            MissingReference = missingReference;
        }
    }

}
