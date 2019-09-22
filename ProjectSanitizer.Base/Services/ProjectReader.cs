using ProjectSanitizer.Base.Extensions;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ProjectSanitizer.Base.Services
{
    public class ProjectReader : IProjectReader
    {
        public Project ReadProject(VerifiedFile file)
        {
            var project = new Project(file, this);
            return project;
        }

        public IEnumerable<ProjectReference> ExtractProjectReferences(DotNetXMLDoc csProjXML)
        {
            foreach (XmlNode projNode in csProjXML.SelectNodes("//ProjectReference"))
            {
                var relativePath = projNode.Attributes["Include"].Value;
                yield return new ProjectReference(csProjXML.File.Directory, ParseReferenceInclude(relativePath), relativePath);
            }
        }

        public IEnumerable<Reference> ExtractFileReferences(DotNetXMLDoc csProjXML)
        {
            foreach (XmlNode refNode in csProjXML.SelectNodes("//Reference"))
            {
                var include = refNode.Attributes["Include"].Value;
                var hintPath = csProjXML.SelectSingleNode(refNode, "HintPath");
                
                yield return new Reference(csProjXML.File.Directory, ParseReferenceInclude(include), hintPath?.InnerText ?? "");
            }
        }

        public ReferenceInclude ParseReferenceInclude(string include)
        {
            var parts = include.Split(',');

            var nameValues = parts
                                .Select(text => text.Split('='))
                                .Where(nv => nv.Length == 2)
                                .ToDictionary(k => k[0].Trim(), v => v[1].Trim());

            var version = nameValues.TryGet("Version");

            return new ReferenceInclude(parts[0], new VersionWithSuffix(version));
        }
    }
}
