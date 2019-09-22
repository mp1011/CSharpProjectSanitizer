namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class ProjectLine
    {
        public ProjectLine(string name, string relativePath)
        {
            Name = name;
            RelativePath = relativePath;
        }

        public string Name { get; }
        public string RelativePath { get; }
    }
}
