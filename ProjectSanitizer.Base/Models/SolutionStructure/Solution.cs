using ProjectSanitizer.Base.Models.FileModels;
using System.IO;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Solution
    {
        private readonly VerifiedFile _slnFile;

        public VerifiedFolder SolutionDirectory => _slnFile.Directory;

        public VerifiedFolder PackagesDirectory { get; }

        public Solution(VerifiedFile slnFile, Project[] projects, ProjectLine[] projectLines)
        {
            _slnFile = slnFile;
            Projects = projects;
            ProjectLines = projectLines;

            PackagesDirectory = slnFile.Directory.GetRelativeFolder("packages");
        }

        public Project[] Projects { get; }

        public ProjectLine[] ProjectLines { get; }

        public override string ToString()
        {
            return _slnFile.Name;
        }
    }
}
