using System.Collections.Generic;
using System.Linq;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models.Problems;

namespace ProjectSanitizer.Services.ProblemDetectors
{
    public class FileReferenceInsteadOfProjectReferenceDetector : IProblemDetector<ProjectGraphNode>
    {
        public IEnumerable<Problem> DetectProblems(ProjectGraphNode item)
        {
            foreach(var fileRef in item.FileRequirements.Where(f=>f.File != null))
            {
                var matchingProject = item.SolutionGraph
                    .FindProject(p => p.Project.AssemblyName == fileRef.File.Name)
                    .FirstOrDefault();

                if (matchingProject != null)
                    yield return new FileReferenceInsteadOfProjectReference(item, matchingProject.Project);
            }

        }
    }
}
