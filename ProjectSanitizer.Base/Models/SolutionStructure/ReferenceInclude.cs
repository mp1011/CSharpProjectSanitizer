namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class ReferenceInclude
    {
        public ReferenceInclude(string id, VersionWithSuffix version)
        {
            ID = id;
            Version = version;
        }

        public string ID { get; }
        public VersionWithSuffix Version { get; }

    }
}
