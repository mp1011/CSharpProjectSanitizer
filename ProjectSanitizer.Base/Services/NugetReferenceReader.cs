﻿using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Collections.Generic;
using System.Xml;

namespace ProjectSanitizer.Base.Services
{
    public class NugetReferenceReader : INugetReferenceReader
    {
        public PackagesConfig TryReadPackagesConfig(VerifiedFolder folder)
        {
            var file = folder.GetRelativeFileOrDefault("packages.config");
            if (file == null)
                return null;

            return new PackagesConfig(file, this);
        }

        public IEnumerable<Package> ExtractPackages(DotNetXMLDoc xmlDoc)
        {
            foreach (XmlNode packageNode in xmlDoc.SelectNodes("//package"))
            {
                string id = packageNode.Attributes["id"].Value;
                yield return new Package(id);
            }
        }
    }
}