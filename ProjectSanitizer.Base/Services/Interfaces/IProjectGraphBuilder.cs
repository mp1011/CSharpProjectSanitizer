using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;

namespace ProjectSanitizer.Base.Services.Interfaces
{
    public interface IProjectGraphBuilder
    {
        SolutionGraph BuildGraph(Solution solution);
    }
}
