using ProjectSanitizer.Models;
using ProjectSanitizer.Models.Problems;
using System.Linq;

namespace ProjectSanitizer.Services.ProblemCorrectors
{
    public class DifferentNugetVersionsCorrector : ProblemCorrector<MultipleNugetVersions>
    {
        private NugetPackageModifier _nugetPackageModifier;

        public DifferentNugetVersionsCorrector(NugetPackageModifier nugetPackageModifier)
        {
            _nugetPackageModifier = nugetPackageModifier;
        }

        protected override CorrectionResult TryCorrectProblem(MultipleNugetVersions problem)
        {
            var solution = problem.Item;
            var latestVersion = problem.DistinctVersions.Max();

            var packageToCopy = solution.SolutionProjects
                .SelectMany(p => p.NugetPackageRequirements)
                .Where(p => p.Package.ID == problem.PackageID && p.Version == latestVersion)
                .First();

            foreach (var project in solution.SolutionProjects)
            {
                if(project.NugetPackageRequirements.Any(p=>p.Package.ID == problem.PackageID))
                    _nugetPackageModifier.AddOrModifyNugetReference(packageToCopy, project.Project, solution.Solution.PackagesDirectory);
            }

            return new CorrectionResult(problem, Resolution.Solved);
        }
    }
}
