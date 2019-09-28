using NUnit.Framework;
using ProjectSanitizer.Base.Models.FileModels;
using System.IO;

namespace ProjectSanitizer.Tests
{
    public static class TestPaths
    {
        public static VerifiedFolder ProjectDirectory { get; }

        static TestPaths()
        {
            ProjectDirectory = new VerifiedFolder(TestContext.CurrentContext.TestDirectory)
                .GetFirstAncestor("ProjectSanitizer.Tests");
        }

        public static VerifiedFile GetFileRelativeToProjectDir(string relativePath)
        {
            return ProjectDirectory.GetRelativeFile(relativePath);
        }

        public static VerifiedFolder GetFolderRelativeToProjectDir(string relativePath)
        {
            if (Directory.Exists(relativePath))
                return new VerifiedFolder(relativePath);
            else
                return ProjectDirectory.GetRelativeFolder(relativePath);
        }
    }
}
