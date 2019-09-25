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

        public override string ToString()
        {
            return _file.Name;
        }

        private string GetStandardizedName()
        {
            return _file.FullName.ToLower();
        }

        public override bool Equals(object obj)
        {
            if (obj is VerifiedFile otherFile)
                return GetStandardizedName() == otherFile.GetStandardizedName();

            return false;
        }

        public override int GetHashCode()
        {
            return GetStandardizedName().GetHashCode();
        }

        public static bool operator ==(VerifiedFile first, VerifiedFile second)
        {
            bool firstNull = Equals(first, null);
            bool secondNull = Equals(second, null);
            if (firstNull && secondNull)
                return true;
            else if (firstNull != secondNull)
                return false;
            else
                return first.Equals(second);
        }

        public static bool operator !=(VerifiedFile first, VerifiedFile second)
        {
            bool firstNull = Equals(first, null);
            bool secondNull = Equals(second, null);
            if (firstNull && secondNull)
                return false;
            else if (firstNull != secondNull)
                return true;
            else
                return !first.Equals(second);
        }
    }
}
