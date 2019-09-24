using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Models.SolutionStructure;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Services.Interfaces
{
    public interface IProjectReader
    {
        Project ReadProject(VerifiedFile file);
        IEnumerable<ProjectReference> ExtractProjectReferences(DotNetXMLDoc csProjXML);
        IEnumerable<Reference> ExtractFileReferences(DotNetXMLDoc csProjXML);
        ReferenceInclude ParseReferenceInclude(string include);
        DotNetVersion ExtractDotNetVersion(DotNetXMLDoc csProjXML);
    }
}
