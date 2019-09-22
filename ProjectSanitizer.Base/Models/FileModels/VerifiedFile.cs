using System.IO;

namespace ProjectSanitizer.Base.Models.FileModels
{
    public class VerifiedFile
    {
        private readonly FileInfo _file;

        public VerifiedFolder Directory { get; }

        public static VerifiedFile GetFileIfExisting(string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
                return new VerifiedFile(file);
            else
                return null;
        }

        public VerifiedFile(FileInfo file)
        {
            _file = file;
            if (!_file.Exists)
                throw new FileNotFoundException($"Could not find the file {file.FullName}");

            Directory = new VerifiedFolder(file.Directory);
        }

        public VerifiedFile(string path) : this(new FileInfo(path))
        {
        }

        public string Name => Path.GetFileNameWithoutExtension(_file.Name);
        public string FullName => _file.FullName;

        public string[] ReadAllLines()
        {
            return File.ReadAllLines(_file.FullName);
        }

        public VerifiedFile GetRelativeFileOrDefault(string relativePath)
        {
            var path = Path.Combine(_file.Directory.FullName, relativePath);
            return GetFileIfExisting(path);
        }
    }
}
