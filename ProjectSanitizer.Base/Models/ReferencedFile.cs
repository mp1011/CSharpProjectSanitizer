using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;

namespace ProjectSanitizer.Base.Models
{
    public class ReferencedFile
    {
        public Project Project { get; }

        public ReferencedFile(Project project, VerifiedFile file, VersionWithSuffix version)
        {
            Project = project;
            File = file;
            Version = version;
        }

        public VerifiedFile File { get; }
        public VersionWithSuffix Version { get; }

        public override string ToString()
        {
            return File.ToString();
        }
    }
}
