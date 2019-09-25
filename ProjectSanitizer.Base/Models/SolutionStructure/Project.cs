using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.SolutionStructure;
using System.Linq;
using System.Xml;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Project
    {
        private readonly VerifiedFile _csProjFile;
        private DotNetXMLDoc _xml;

        public string AssemblyName { get; }

        public DotNetVersion DotNetVersion { get; }
        public ProjectReference[] ProjectReferences { get; }

        public Reference[] FileReferences { get; }

        public Project(VerifiedFile csProjFile, IProjectReader projectReader)
        {
            _csProjFile = csProjFile;
            _xml = new DotNetXMLDoc(csProjFile);

            DotNetVersion = projectReader.ExtractDotNetVersion(_xml);
            ProjectReferences = projectReader.ExtractProjectReferences(_xml).ToArray();
            FileReferences = projectReader.ExtractFileReferences(_xml).ToArray();
            AssemblyName = projectReader.ExtractAssemblyName(_xml);
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
