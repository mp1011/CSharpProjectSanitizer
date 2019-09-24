using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.Problems;
using System.Collections.Generic;

namespace ProjectSanitizer.Services.ProblemDetectors
{
    public class DependsOnIncompatibleFrameworkDetector : IProblemDetector<ProjectGraphNode>
    {
        public IEnumerable<Problem> DetectProblems(ProjectGraphNode item)
        {
            var thisVersion = item.Project.DotNetVersion;
            foreach(var projectRef in item.ProjectRequirements)
            {
                var otherVersion = projectRef.Project.DotNetVersion;
                if (thisVersion.DotNetType != otherVersion.DotNetType)
                    yield return new DependsOnIncompatibleFramework(thisVersion, otherVersion);
                else if(otherVersion.Version > thisVersion.Version)
                    yield return new DependsOnIncompatibleFramework(thisVersion, otherVersion);
            }
        }
    }
}
