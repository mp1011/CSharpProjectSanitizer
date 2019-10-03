using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.Problems;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Services.ProblemDetectors
{
    public class MissingProjectDetector : IProblemDetector<Solution>
    {
        public IEnumerable<Problem> DetectProblems(Solution solution)
        {
            foreach(var project in solution.ProjectLines)
            {
                var file = solution.SolutionDirectory.GetRelativeFile(project.RelativePath);
                if (!file.Exists)
                    yield return new MissingProject(solution, project);
            }
        }
    }
}
