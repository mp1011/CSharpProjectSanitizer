using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.SolutionStructure;
using System.Collections.Generic;
using System.Xml;

namespace ProjectSanitizer.Base.Services
{
    public class NugetReferenceReader : INugetReferenceReader
    {
        public PackagesConfig TryReadPackagesConfig(VerifiedFolder folder)
        {
            var file = folder.GetRelativeFile("packages.config");
            if (file is VerifiedFile verifiedFile)
                return new PackagesConfig(verifiedFile, this);
            else
                return null;
        }


        public IEnumerable<Package> ExtractPackages(DotNetXMLDoc xmlDoc)
        {
            foreach (XmlNode packageNode in xmlDoc.SelectNodes("//package"))
            {
                string id = packageNode.Attributes["id"].Value;
                string version = packageNode.Attributes["version"].Value;
                string dotNetVersion = packageNode.Attributes["targetFramework"]?.Value;

                yield return new Package(id, VersionWithSuffix.TryParse(version), DotNetVersion.TryParse(dotNetVersion));
            }
        }
    }
}
