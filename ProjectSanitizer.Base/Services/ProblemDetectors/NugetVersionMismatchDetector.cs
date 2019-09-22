using System.Collections.Generic;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Services.Interfaces;

namespace ProjectSanitizer.Base.Services.ProblemDetectors
{
    public class NugetVersionMismatchDetector : IProblemDetector<NugetReference>
    {
        public IEnumerable<Problem> DetectProblems(NugetReference item)
        {
            if (item.Version != item.Package.Version)
                yield return new Problem($"Package {item.Package.ID} should have version {item.Package.Version}, but version {item.Version} is referenced");
        }
    }
}
