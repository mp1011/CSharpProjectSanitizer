using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Models.SolutionStructure;
using ProjectSanitizer.Services;

namespace ProjectSanitizer.Models.Problems
{
    public class DependsOnIncompatibleFramework : Problem
    {
        public DotNetVersion ProjectVersion { get; }
        public DotNetVersion DependsOnVersion { get; }

        public Project Project { get; }

        public Project DependsOnProject { get; }

        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("Project ").AppendHighlighted($"{Project} ({ProjectVersion})")
            .AppendError(" depends on Project ").AppendHighlighted($"{DependsOnProject} ({DependsOnVersion})");

        public DependsOnIncompatibleFramework(Project project, Project dependsOnProject, DotNetVersion projectVersion, DotNetVersion dependsOnVersion)
        {
            ProjectVersion = projectVersion;
            DependsOnVersion = dependsOnVersion;
        }
    }
}
