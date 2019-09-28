using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Models.FileModels;

namespace ProjectSanitizer.Base.Models
{
    public class NugetReference : ReferencedFile
    {
        public Package Package { get; }

        public NugetReference(Project project, Package package, RelativeFile file, VersionWithSuffix version) 
            : base(project, file,version)
        {
            Package = package;
        }
    }
}
