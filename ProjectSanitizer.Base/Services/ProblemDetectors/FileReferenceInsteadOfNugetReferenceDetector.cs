using System.Collections.Generic;
using System.Linq;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.Problems;

namespace ProjectSanitizer.Services.ProblemDetectors
{
    public class FileReferenceInsteadOfNugetReferenceDetector : IProblemDetector<ProjectGraphNode>
    {
        public IEnumerable<Problem> DetectProblems(ProjectGraphNode item)
        {
            foreach(var fileRef in item.FileRequirements.Where(f=>f.File != null))
            {
                //do any projects reference this file through nuget?
                var nugetRef = item.SolutionGraph.AllNodes.Values
                    .SelectMany(node => node.NugetPackageRequirements)
                    .FirstOrDefault(ng => ng.File.Name == fileRef.File.Name);

                if(nugetRef != null)
                    yield return new FileReferenceInsteadOfNugetReference(item, nugetRef);
            }
        }
    }
}
