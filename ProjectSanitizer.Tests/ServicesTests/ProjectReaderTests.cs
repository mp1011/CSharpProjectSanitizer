using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class ProjectReaderTests
    {

        [TestCase(@"ExampleBrokenSolutions\FirstProject\FirstProject.csproj", @"..\AnotherProject\AnotherProject.csproj")]
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
    }
}
