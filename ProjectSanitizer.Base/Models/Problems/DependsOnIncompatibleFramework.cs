using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models.SolutionStructure;
using ProjectSanitizer.Services;

namespace ProjectSanitizer.Models.Problems
{
    public class DependsOnIncompatibleFramework : Problem
    {
        public DotNetVersion ProjectVersion { get; }
        public DotNetVersion DependsOnVersion { get; }

        public override SmartStringBuilder Description => throw new System.NotImplementedException();

        public DependsOnIncompatibleFramework(DotNetVersion projectVersion, DotNetVersion dependsOnVersion)
        {
            ProjectVersion = projectVersion;
            DependsOnVersion = dependsOnVersion;
        }
    }
}
