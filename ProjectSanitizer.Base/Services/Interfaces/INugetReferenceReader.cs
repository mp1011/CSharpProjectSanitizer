using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Services.Interfaces
{
    public interface INugetReferenceReader
    {
        PackagesConfig TryReadPackagesConfig(VerifiedFolder folder);

        IEnumerable<Package> ExtractPackages(DotNetXMLDoc xmlDoc);
    }
}
