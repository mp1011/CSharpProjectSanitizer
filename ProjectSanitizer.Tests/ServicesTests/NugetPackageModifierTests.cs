using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Services;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class NugetPackageModifierTests
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln","Newtonsoft.Json", true, "12.0.3-beta1")]
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "Newtonsoft.Json", false, "8.0.2")]
        public void CanGetLatestNugetPackageFromSolution(string slnPath, string nugetPackage, bool includePrerelease, string expectedVersion)
        {
            var solution = TestHelpers.GetSolutionGraph(slnPath);
            var nugetService = DIRegistrar.GetInstance<NugetPackageModifier>();

            var latestRef = nugetService.GetLatestNugetReference(nugetPackage, includePrerelease, solution);
            Assert.AreEqual(latestRef.Package.Version, latestRef.Version);

            Assert.AreEqual(expectedVersion, latestRef.Package.Version.ToString());
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", @"ExampleBrokenSolutions\FirstProject\FirstProject.csproj",
             @"ExampleBrokenSolutions\FirstProject\packages.config","Newtonsoft.Json", "12.0.1")]
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", @"ExampleBrokenSolutions\AnotherFolder\ProjectInAnotherFolder\ProjectInAnotherFolder.csproj",
             @"ExampleBrokenSolutions\AnotherFolder\ProjectInAnotherFolder\packages.config", "Newtonsoft.Json", "12.0.1")]

        public void CanModifyNugetPackage(string slnPath, string projectPath, string packagesConfigPath, string nugetPackage, string expectedVersion)
        {
            TestPaths.BackupOrRestore(projectPath);
            var packagesConfigFile = TestPaths.BackupOrRestore(packagesConfigPath);

            try
            {
                var sln = TestHelpers.GetSolutionGraph(slnPath);

                var nugetService = DIRegistrar.GetInstance<NugetPackageModifier>();
                var package = nugetService.GetLatestNugetReference(nugetPackage, false, sln);

                var project = sln.FindProject(p => p.Project.FullPath.EndsWith(projectPath)).First().Project;
                nugetService.AddOrModifyNugetReference(package, project, sln.Solution.PackagesDirectory);


                sln = TestHelpers.GetSolutionGraph(slnPath);
                var reloadedProject = sln.FindProject(p => p.Project.FullPath.EndsWith(projectPath)).First();
                package = reloadedProject.NugetPackageRequirements.FirstOrDefault(p => p.Package.ID == nugetPackage);
                Assert.AreEqual(expectedVersion, package.Version.ToString());
            }
            finally
            {
                TestPaths.BackupOrRestore(projectPath);

                if(packagesConfigFile != null)
                    TestPaths.BackupOrRestore(packagesConfigPath);
                else
                {
                    packagesConfigFile = TestPaths.GetFileRelativeToProjectDir(packagesConfigPath);
                    if (packagesConfigFile != null)
                        packagesConfigFile.Delete();
                }
            }
        }

        [TestCase(@"ExampleBrokenSolutions\packages", @"ExampleBrokenSolutions\AnotherFolder\ProjectInAnotherFolder", 
            @"..\packages\Newtonsoft.Json.8.0.2\lib\net45\Newtonsoft.Json.dll</HintPath>",
            @"..\..\packages\Newtonsoft.Json.8.0.2\lib\net45\Newtonsoft.Json.dll</HintPath>")]
        public void CanAdjustPackesConfigPaths(string packagesFolder, string projectDirectory, string text, string expectedModifiedText)
        {
            var packagesDir = TestPaths.GetFolderRelativeToProjectDir(packagesFolder);
            var projectDir = TestPaths.GetFolderRelativeToProjectDir(projectDirectory);
            var modifier = DIRegistrar.GetInstance<NugetPackageModifier>();
            var modifiedText = modifier.CorrectPackagesPath(projectDir, packagesDir, text);
            Assert.AreEqual(expectedModifiedText, modifiedText);
        }
    }
}
