using System.IO;

namespace ProjectSanitizer.Base.Models
{
    public class Solution
    {
        private readonly FileInfo _slnFile;

        public Solution(FileInfo slnFile, Project[] projects)
        {
            _slnFile = slnFile;
            Projects = projects;
        }

        public Project[] Projects { get; }

        public override string ToString()
        {
            return _slnFile.Name;
        }
    }
}
