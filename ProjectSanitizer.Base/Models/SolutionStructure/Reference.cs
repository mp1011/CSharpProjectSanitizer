using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Models.FileModels;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Reference
    {
        public override string ToString()
        {
            return $"{Include.ID} {Version}";
        }
        public VerifiedFolder BaseDirectory { get; }

        public ReferenceInclude Include { get; }

        public VersionWithSuffix VersionFromPath { get; }

        public VersionWithSuffix Version => VersionFromPath ?? Include.Version;

        public Reference(VerifiedFolder baseDirectory, ReferenceInclude include, string relativePath)
        {
            BaseDirectory = baseDirectory;
            RelativePath = relativePath;
            Include = include;
            VersionFromPath = VersionWithSuffix.TryParseFromPath(relativePath);
        }

        public string RelativePath { get; }

        public RelativeFile GetFile()
        {
            return new RelativeFile(BaseDirectory, RelativePath); 
        }
    }
}
