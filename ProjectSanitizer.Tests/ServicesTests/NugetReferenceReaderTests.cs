using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class NugetReferenceReaderTests
    {
        [TestCase(@"ExampleBrokenSolutions\AnotherProject\", "Newtonsoft.Json")]
        public void CanReadNugetReference(string relativePackagesConfigPath, string expectedReference)
        {
            var projectFolder = TestPaths.GetFolderRelativeToProjectDir(relativePackagesConfigPath);
            var service = DIRegistrar.GetInstance<INugetReferenceReader>();
            var packagesConfigFile = service.TryReadPackagesConfig(projectFolder);

            var dependency = packagesConfigFile.Packages.FirstOrDefault(pk=>pk.ID == expectedReference);
            Assert.IsNotNull(dependency);
        }


    }
}
