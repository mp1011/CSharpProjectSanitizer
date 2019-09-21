using System.IO;

namespace ProjectSanitizer.Base.Models
{
    public class Project
    {
        private readonly FileInfo _csProjFile;

        public Project(FileInfo csProjFile)
        {
            _csProjFile = csProjFile;
        }

        public override string ToString()
        {
            return _csProjFile.Name;
        }
    }
}
