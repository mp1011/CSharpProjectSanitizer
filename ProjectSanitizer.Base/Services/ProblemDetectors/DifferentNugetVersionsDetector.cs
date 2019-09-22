using System.Collections.Generic;
using System.Linq;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Services.Interfaces;

namespace ProjectSanitizer.Base.Services.ProblemDetectors
{
    public class DifferentNugetVersionsDetector : IProblemDetector<SolutionGraph>
    {
        public IEnumerable<Problem> DetectProblems(SolutionGraph item)
        {
            foreach(var packageGroup in item.AllNodes.Values
                                .SelectMany(p=>p.NugetPackageRequirements)
                                .GroupBy(p=>p.Package.ID))
            {
                var versions = packageGroup.Select(g => g.Version).Distinct().ToArray();
                if (versions.Length > 1)
                    yield return new Problem($"Multiple versions of {packageGroup.Key} found");
            }
        }
    }
}
