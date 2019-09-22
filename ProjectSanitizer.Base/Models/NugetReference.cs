using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;

namespace ProjectSanitizer.Base.Models
{
    public class NugetReference : ReferencedFile
    {
        public Package Package { get; }

        public NugetReference(Package package, VerifiedFile file, VersionWithSuffix version) 
            : base(file,version)
        {
            Package = package;
        }
    }
}
