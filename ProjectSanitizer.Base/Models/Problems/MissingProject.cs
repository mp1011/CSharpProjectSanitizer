using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Services;

namespace ProjectSanitizer.Models.Problems
{
    public class MissingProject : Problem<Solution>
    {
        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("Missing project ").AppendHighlighted(Project.Name)
            .AppendError(" in solution ").AppendHighlighted(Item);

        public ProjectLine Project { get; }

        public MissingProject(Solution solution, ProjectLine project):base(solution)
        {
            Project = project;
        }
    }
}
