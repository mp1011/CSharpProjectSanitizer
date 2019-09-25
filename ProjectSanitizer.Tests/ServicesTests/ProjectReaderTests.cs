using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class ProjectReaderTests
    {
        [TestCase(@"ExampleBrokenSolutions\AnotherProject\AnotherProject.csproj", @"..\ThirdProject\ThirdProject.csproj")]
        [TestCase(@"ExampleBrokenSolutions\VS2017Project\VS2017Project.csproj", @"..\AnotherProject\AnotherProject.csproj")]

        public void CanReadProjectReferences(string relativeCsProjPath, string expectedReference)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var service = DIRegistrar.GetInstance<IProjectReader>();
            var proj = service.ReadProject(csProjPath);
            var reference = proj.ProjectReferences.FirstOrDefault(rf => rf.RelativePath == expectedReference);
            Assert.IsNotNull(reference);
        }

        [TestCase(@"ExampleBrokenSolutions\AnotherProject\AnotherProject.csproj", "Newtonsoft.Json.dll")]
        public void CanReadFileReferences(string relativeCsProjPath, string expectedReference)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var service = DIRegistrar.GetInstance<IProjectReader>();
            var proj = service.ReadProject(csProjPath);

            var dependency = proj.FileReferences.FirstOrDefault(fr => fr.RelativePath.EndsWith(expectedReference));
            Assert.IsNotNull(dependency);
        }

        [TestCase(@"ExampleBrokenSolutions\AnotherProject\AnotherProject.csproj", "Newtonsoft.Json", "12.0.2")]
        [TestCase(@"ExampleBrokenSolutions\ThirdProject\ThirdProject.csproj", "Newtonsoft.Json", "12.0.3-beta1")]
        public void CanDetectVersionOfFileReferenceFromPath(string relativeCsProjPath, string reference, string expectedFileVersion)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var service = DIRegistrar.GetInstance<IProjectReader>();
            var proj = service.ReadProject(csProjPath);

            var fileRef = proj.FileReferences.FirstOrDefault(r => r.Include.ID == reference);
            Assert.AreEqual(expectedFileVersion,fileRef.Version.ToString());
        }

        [TestCase(@"ExampleBrokenSolutions\AnotherProject\AnotherProject.csproj",".NET Framework v4.7.2")]
        [TestCase(@"ExampleBrokenSolutions\OlderDotNet\OlderDotNet.csproj", ".NET Framework v4.5")]
        public void CanReadDotNetVersionOfProject(string relativeCsProjPath, string expectedVersion)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var service = DIRegistrar.GetInstance<IProjectReader>();
            var proj = service.ReadProject(csProjPath);

            Assert.AreEqual(expectedVersion, proj.DotNetVersion.ToString());
        }

        [TestCase(@"ExampleBrokenSolutions\AnotherProject\AnotherProject.csproj", "AnotherProject")]
        [TestCase(@"ExampleBrokenSolutions\FirstProject\FirstProject.csproj", "FirstProject")]
        [TestCase(@"ExampleBrokenSolutions\ThirdProject\ThirdProject.csproj", "ThirdProject")]
        public void CanReadProjectAssemblyFilename(string relativeCsProjPath, string assemblyName)
        {
            var csProjPath = TestPaths.GetFileRelativeToProjectDir(relativeCsProjPath);
            var service = DIRegistrar.GetInstance<IProjectReader>();
            Assert.AreEqual(assemblyName, service.ExtractAssemblyName(new DotNetXMLDoc(csProjPath)));
        }
    }
}
