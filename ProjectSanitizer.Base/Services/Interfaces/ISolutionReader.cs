using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;

namespace ProjectSanitizer.Base.Services.Interfaces
{
    public interface ISolutionReader
    {
        Solution ReadSolution(VerifiedFile slnFile);
    }
}
