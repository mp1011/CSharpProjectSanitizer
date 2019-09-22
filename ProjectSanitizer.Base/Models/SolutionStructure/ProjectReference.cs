using ProjectSanitizer.Base.Models.FileModels;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class ProjectReference : Reference
    {
        public ProjectReference(VerifiedFolder baseDirectory, ReferenceInclude include, string relativePath) 
            : base(baseDirectory,include,relativePath)
        {
        } 
    }
}
