using ProjectSanitizer.Models.SolutionStructure;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Package
    {
        public string ID { get; }

        public VersionWithSuffix Version { get; }

        public DotNetVersion TargetFramework { get; }

        public Package(string id, VersionWithSuffix version, DotNetVersion dotNetVersion)
        {
            ID = id;
            Version = version;
            TargetFramework = dotNetVersion;
        }

        public override string ToString()
        {
            return ID;
        }
    }
}
