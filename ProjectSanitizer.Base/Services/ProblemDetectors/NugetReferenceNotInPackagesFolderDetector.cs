using System.Collections.Generic;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Extensions;
using ProjectSanitizer.Models.Problems;

namespace ProjectSanitizer.Services.ProblemDetectors
{
    public class NugetReferenceNotInPackagesFolderDetector : IProblemDetector<ProjectGraphNode>
    {
        public IEnumerable<Problem> DetectProblems(ProjectGraphNode item)
        {
            var packagesFolder = item.SolutionGraph.Solution.PackagesDirectory;

            foreach(var nugetRef in item.NugetPackageRequirements)
            {
                if (!nugetRef.File.PathBeginsWith(packagesFolder))
                    yield return new NugetReferenceNotInPackagesFolder(nugetRef, item.Project);
            }
        }
    }
}
