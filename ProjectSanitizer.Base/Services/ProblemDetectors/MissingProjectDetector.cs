using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Services.ProblemDetectors
{
    public class MissingProjectDetector : IProblemDetector<Solution>
    {
        public IEnumerable<Problem> DetectProblems(Solution solution)
        {
            foreach(var project in solution.ProjectLines)
            {
                var file = solution.SolutionDirectory.GetRelativeFileOrDefault(project.RelativePath);
                if(file == null)
                    yield return new Problem($"Project {project.Name} was not found");
            }
        }
    }
}
