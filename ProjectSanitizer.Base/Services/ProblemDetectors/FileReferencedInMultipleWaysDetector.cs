using System.Collections.Generic;
using System.Linq;
using ProjectSanitizer.Base.Extensions;
using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models;
using ProjectSanitizer.Models.FileModels;
using ProjectSanitizer.Models.Problems;

namespace ProjectSanitizer.Services.ProblemDetectors
{
    public class FileReferencedInMultipleWaysDetector : IProblemDetector<SolutionGraph>
    {
        public IEnumerable<Problem> DetectProblems(SolutionGraph item)
        {
            Dictionary<string, List<IFile>> referencesByName = new Dictionary<string, List<IFile>>();
            foreach(var project in item.AllNodes.Values)
            {
                foreach (var fileRef in project.FileRequirements)
                    AddReferenceType(fileRef.File, referencesByName);

                foreach (var fileRef in project.NugetPackageRequirements)
                    AddReferenceType(fileRef.File, referencesByName);
            }

            foreach(var multiRef in referencesByName.Where(r=>r.Value.Count() > 1))
            {
                yield return new FileReferencedInMultipleWays(multiRef.Key);
            }
        }

        private void AddReferenceType(IFile file, Dictionary<string, List<IFile>> referencesByName)
        {
            if (file == null)
                return;

            var list = referencesByName.TryGet(file.Name);
            if(list == null)
            {
                list = new List<IFile>();
                referencesByName[file.Name] = list;
            }

            if (!list.Contains(file))
                list.Add(file);
        }
    }
}
