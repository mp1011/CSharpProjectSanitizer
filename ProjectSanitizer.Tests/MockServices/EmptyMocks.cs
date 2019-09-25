using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.SolutionStructure;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ProjectSanitizer.Tests.MockServices
{
    public class EmptyProjectReader : IProjectReader
    {
        public DotNetVersion ExtractDotNetVersion(DotNetXMLDoc csProjXML)
        {
            return new DotNetVersion(DotNetType.Framework, new Version(1, 0));
        }

        public IEnumerable<Reference> ExtractFileReferences(DotNetXMLDoc csProjXML)
        {
            return new Reference[] { };
        }

        public IEnumerable<ProjectReference> ExtractProjectReferences(DotNetXMLDoc csProjXML)
        {
            return new ProjectReference[] { };
        }

        public ReferenceInclude ParseReferenceInclude(string include)
        {
            return new ReferenceInclude("blank", VersionWithSuffix.TryParse("0.0.0"));
        }

        public Project ReadProject(VerifiedFile file)
        {
            return new Project(file, this);
        }

        public string ExtractAssemblyName(DotNetXMLDoc csProjXML)
        {
            return string.Empty;
        }
    }
}
