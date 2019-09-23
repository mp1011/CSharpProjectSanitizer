using System.Collections.Generic;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.Problems;

namespace ProjectSanitizer.Base.Services.ProblemDetectors
{
    public class NugetVersionMismatchDetector : IProblemDetector<NugetReference>
    {
        public IEnumerable<Problem> DetectProblems(NugetReference item)
        {
            if (item.Version != item.Package.Version)
                yield return new NugetVersionMismatch(item);
        }
    }
}
