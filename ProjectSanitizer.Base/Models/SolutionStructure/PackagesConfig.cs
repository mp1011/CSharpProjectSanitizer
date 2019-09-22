using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class PackagesConfig
    {
        private readonly VerifiedFile _packagesConfigFile;
        private DotNetXMLDoc _xml;

        public Package[] Packages { get; }

        public PackagesConfig(VerifiedFile file, INugetReferenceReader nugetReader)
        {
            _packagesConfigFile = file;
            _xml = new DotNetXMLDoc(_packagesConfigFile);

            Packages = nugetReader.ExtractPackages(_xml).ToArray();
        }

        public override string ToString()
        {
            return _packagesConfigFile.Name;
        }

        public Package FindPackage(string id)
        {
            return Packages.FirstOrDefault(p => p.ID == id);
        }
    }
}
