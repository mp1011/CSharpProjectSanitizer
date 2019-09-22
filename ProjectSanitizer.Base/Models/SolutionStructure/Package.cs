namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Package
    {
        public string ID { get; }

        public VersionWithSuffix Version { get; }

        public Package(string id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return ID;
        }
    }
}
