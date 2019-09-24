using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;

namespace ProjectSanitizer.Base.Models
{
    public class NugetReference : ReferencedFile
    {
        public Package Package { get; }

        public NugetReference(Project project, Package package, VerifiedFile file, VersionWithSuffix version) 
            : base(project, file,version)
        {
            Package = package;
        }
    }
}
