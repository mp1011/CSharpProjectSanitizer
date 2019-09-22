namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Package
    {
        public string ID { get; }

        public VersionWithSuffix Version { get; }

        public Package(string id, VersionWithSuffix version)
        {
            ID = id;
            Version = version;
        }

        public override string ToString()
        {
            return ID;
        }
    }
}
