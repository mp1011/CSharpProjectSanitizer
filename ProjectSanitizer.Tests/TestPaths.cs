using NUnit.Framework;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Models.FileModels;
using System;
using System.IO;
using System.Linq;

namespace ProjectSanitizer.Tests
{
    public static class TestPaths
    {
        public static VerifiedFolder TestProjectDirectory { get; }

        static TestPaths()
        {
            var binFolder = new VerifiedFolder(TestContext.CurrentContext.TestDirectory);
            if(binFolder.FullName.Contains("Console.Tests"))
            {
                TestProjectDirectory = binFolder
                                        .GetFirstAncestor("ProjectSanitizer.Console.Tests")
                                        .Parent
                                        .GetRelativeFolder("ProjectSanitizer.Tests");
            }
            else
            {
                TestProjectDirectory = binFolder.GetFirstAncestor("ProjectSanitizer.Tests");
            }  
        }

        public static void RevertAllCsProjAndPackagesConfigFiles()
        {
            var folder = TestPaths.GetFolderRelativeToProjectDir("ExampleBrokenSolutions");
            RevertAllCsProjAndPackagesConfigFiles(folder);
        }

        public static void RevertAllCsProjAndPackagesConfigFiles(VerifiedFolder directory)
        {
            var csprojFile = directory.GetFiles()
                .FirstOrDefault(f=>f.FullName.EndsWith("csproj"));

            if (csprojFile != null)
            {
                RevertFileState(csprojFile);

                var packagesConfigFile = directory.GetRelativeFile("packages.config");
                RevertFileState(packagesConfigFile);
            }

            foreach (var folder in directory.GetDirectories())
                RevertAllCsProjAndPackagesConfigFiles(folder);
        }

        private static VerifiedFile RevertFileState(IFile file)
        { 
            if (!file.Exists)
            { 
                File.WriteAllText($"{file.FullName}.delete", "...");
                return null;
            }
            else if (File.Exists($"{file.FullName}.delete"))
            {
                file.Delete();
                return null;
            }
            else
            {
                var backupFile = new FileInfo($"{file.FullName}.backup");
                if (!backupFile.Exists)
                    Assert.Fail($"Expected backup file {file.FullName}.backup does not exist");

                backupFile.CopyTo(file.FullName, overwrite: true);
            }

            return file as VerifiedFile;
        }

        public static IFile GetFileRelativeToProjectDir(string relativePath)
        {
            return TestProjectDirectory.GetRelativeFile(relativePath);
        }

        public static VerifiedFile GetVerifiedFileRelativeToProjectDir(string relativePath)
        {
            if (TestProjectDirectory.GetRelativeFile(relativePath) is VerifiedFile v)
                return v;
            else
                throw new Exception($"Expected file {relativePath} does not exist"); 
        }

        public static VerifiedFolder GetFolderRelativeToProjectDir(string relativePath)
        {
            if (Directory.Exists(relativePath))
                return new VerifiedFolder(relativePath);
            else
                return TestProjectDirectory.GetRelativeFolder(relativePath);
        }
    }
}
