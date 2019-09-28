using ProjectSanitizer.Base.Models.FileModels;
using System.IO;

namespace ProjectSanitizer.Models.FileModels
{
    public class RelativeFile : IFile
    {
        private FileInfo _file;

        public string Name => Path.GetFileNameWithoutExtension(_file.Name);

        public string FullName => _file.FullName;

        public RelativeFile(VerifiedFolder projectDirectory, string relativePath)
        {
            _file = new FileInfo(Path.Combine(projectDirectory.FullName, relativePath));
        }

        public VerifiedFile TryVerify()
        {
            return VerifiedFile.GetFileIfExisting(_file.FullName);
        }

        public bool PathBeginsWith(VerifiedFolder folder)
        {
            if (folder == null)
                return false;

            var thisPath = FullName.ToLower();
            var folderPath = folder.FullName.ToLower();
            return thisPath.StartsWith(folderPath);
        }
    }
}
