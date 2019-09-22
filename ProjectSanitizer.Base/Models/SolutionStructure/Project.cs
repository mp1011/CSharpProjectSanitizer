﻿using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Linq;
using System.Xml;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Project
    {
        private readonly VerifiedFile _csProjFile;
        private DotNetXMLDoc _xml;

        public ProjectReference[] ProjectReferences { get; }

        public Reference[] FileReferences { get; }

        public Project(VerifiedFile csProjFile, IProjectReader projectReader)
        {
            _csProjFile = csProjFile;
            _xml = new DotNetXMLDoc(csProjFile);

            ProjectReferences = projectReader.ExtractProjectReferences(_xml).ToArray();
            FileReferences = projectReader.ExtractFileReferences(_xml).ToArray();
        }

        public string FullPath => _csProjFile.FullName;
        public string Name => _csProjFile.Name;

        public VerifiedFolder ProjectDirectory => _csProjFile.Directory;

        public override string ToString()
        {
            return _csProjFile.Name;
        }
    }
}
