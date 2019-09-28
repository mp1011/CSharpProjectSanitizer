using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Models.FileModels;
using System;

namespace ProjectSanitizer.Base.Models
{
    public class NugetReference : ReferencedFile
    {
        public Package Package { get; }

        public NugetReference(Project project, Package package, RelativeFile file, VersionWithSuffix version) 
            : base(project, file,version)
        {
            if (version == null)
                throw new ArgumentNullException();
            Package = package;
        }
    }
}
