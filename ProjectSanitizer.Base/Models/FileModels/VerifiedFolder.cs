﻿using ProjectSanitizer.Models.FileModels;
using System.Collections.Generic;
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

        public IFile GetRelativeFile(string relativePath)
        {
            var path = Path.Combine(_directory.FullName, relativePath);
            var file = VerifiedFile.GetFileIfExisting(path);
            if (file == null)
                return new MissingFile(this, relativePath);
            else
                return file;
        }

        public IEnumerable<VerifiedFile> GetFiles()
        {
            foreach (var file in _directory.GetFiles())
                yield return new VerifiedFile(file);
        }

        public IEnumerable<VerifiedFolder> GetDirectories()
        {
            foreach (var folder in _directory.GetDirectories())
                yield return new VerifiedFolder(folder);
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
