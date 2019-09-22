using ProjectSanitizer.Base.Models.FileModels;

namespace ProjectSanitizer.Base.Models
{
    public class ReferencedFile
    {
        public ReferencedFile(VerifiedFile file, VersionWithSuffix version)
        {
            File = file;
            Version = version;
        }

        public VerifiedFile File { get; }
        public VersionWithSuffix Version { get; }
    }
}
