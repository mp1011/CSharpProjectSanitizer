using System.IO;

namespace ProjectSanitizer.Base.Models.FileModels
{
    public class VerifiedFolder
    {
        private DirectoryInfo _directory;
        public VerifiedFolder Parent { get; }

        public string FullName => _directory.FullName;

        public VerifiedFolder(string path) : this(new DirectoryInfo(path)) { }

        public VerifiedFolder(DirectoryInfo path)
        {
            _directory = path;
            if (!_directory.Exists)
                throw new FileNotFoundException($"Unable to find the path {path}");

            if (_directory.Parent != null)
                Parent = new VerifiedFolder(_directory.Parent);
        }

        public VerifiedFolder GetFirstAncestor(string name)
        {
            if (_directory.Name == name)
                return this;
            else if (_directory.Parent == null)
                throw new FileNotFoundException($"Unable to find an ancestor {name} in {_directory.FullName}");
            else
                return Parent.GetFirstAncestor(name);
        }

        public VerifiedFile GetRelativeFile(string relativePath)
        {
            var path = Path.Combine(_directory.FullName, relativePath);
            return new VerifiedFile(path);
        }

        public VerifiedFile GetRelativeFileOrDefault(string relativePath)
        {
            var path = Path.Combine(_directory.FullName, relativePath);
            return VerifiedFile.GetFileIfExisting(path);
        }

        public VerifiedFolder GetRelativeFolder(string relativePath)
        {
            var path = Path.Combine(_directory.FullName, relativePath);
            return new VerifiedFolder(path);
        }

        public VerifiedFolder GetRelativeFolderOrDefault(string relativePath)
        {
            var path = Path.Combine(_directory.FullName, relativePath);
            if (Directory.Exists(path))
                return new VerifiedFolder(path);
            else
                return null;
        }
    }
}
