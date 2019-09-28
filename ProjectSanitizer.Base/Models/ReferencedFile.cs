using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Models.FileModels;

namespace ProjectSanitizer.Base.Models
{
    public class ReferencedFile
    {
        public Project Project { get; }

        public ReferencedFile(Project project, IFile file, VersionWithSuffix version)
        {
            Project = project;
            File = file;
            Version = version;
        }

        public IFile File { get; }
        public VersionWithSuffix Version { get; }

        public override string ToString()
        {
            return File.ToString();
        }
    }
}
