using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Services;

namespace ProjectSanitizer.Models.Problems
{
    public class NugetReferenceNotInPackagesFolder : Problem
    {
        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("The nuget package ").AppendHighlighted(NugetReference.Package.ID)
            .AppendError(" in project ").AppendHighlighted(Project.Name)
            .AppendError(" is not found within the solution's packages folder");

        public NugetReference NugetReference { get; }
        public Project  Project { get; }

        public NugetReferenceNotInPackagesFolder(NugetReference reference, Project project)
        {
            NugetReference = reference;
            Project = project;
        }
    }
}
