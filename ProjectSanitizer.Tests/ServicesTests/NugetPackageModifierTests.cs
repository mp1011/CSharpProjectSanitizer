using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.SolutionStructure;
using ProjectSanitizer.Services;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    public class NugetPackageModifierTests :TestBase
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln","Newtonsoft.Json", true, "12.0.3-beta1")]
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln", "Newtonsoft.Json", false, "12.0.1")]
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
       
            var packagesConfigFile = TestPaths.GetFileRelativeToProjectDir(packagesConfigPath);

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

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln","AnotherProject", "Newtonsoft.Json", "net472", "12.0.3-beta1")]
        public void CanChangeVersionInPackagesConfig(string solutionPath, string projectName, string packageName, string dotNetVersion, string newVersion)
        {
            var solution = TestHelpers.GetSolution(solutionPath);
            var project = solution.Projects.FirstOrDefault(p => p.Name == projectName);
            var modifier = DIRegistrar.GetInstance<NugetPackageModifier>();

            modifier.ChangeNugetPackageVersionInPackagesConfig(
                project, 
                packageName, 
                DotNetVersion.TryParse(dotNetVersion), 
                VersionWithSuffix.TryParse(newVersion));

            var reader = DIRegistrar.GetInstance<INugetReferenceReader>();
            var packagesConfig = reader.TryReadPackagesConfig(project.ProjectDirectory);
            var package = packagesConfig.Packages.FirstOrDefault(p => p.ID == packageName);
            Assert.AreEqual(newVersion, package.Version.ToString());
        }
    }

}
