using ProjectSanitizer.Base.Models.FileModels;
using System.IO;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Solution
    {
        private readonly VerifiedFile _slnFile;

        public VerifiedFolder SolutionDirectory => _slnFile.Directory;

        public Solution(VerifiedFile slnFile, Project[] projects, ProjectLine[] projectLines)
        {
            _slnFile = slnFile;
            Projects = projects;
            ProjectLines = projectLines;
        }

        public Project[] Projects { get; }

        public ProjectLine[] ProjectLines { get; }

        public override string ToString()
        {
            return _slnFile.Name;
        }
    }
}
