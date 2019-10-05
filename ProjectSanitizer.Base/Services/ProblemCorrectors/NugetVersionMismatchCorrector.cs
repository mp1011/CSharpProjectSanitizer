using ProjectSanitizer.Models;
using ProjectSanitizer.Models.Problems;

namespace ProjectSanitizer.Services.ProblemCorrectors
{
    public class NugetVersionMismatchCorrector : ProblemCorrector<NugetVersionMismatch>
    {
        private NugetPackageModifier _nugetPackageModifier;

        public NugetVersionMismatchCorrector
            (NugetPackageModifier nugetPackageModifier)
        {
            _nugetPackageModifier = nugetPackageModifier;
        }

        protected override CorrectionResult TryCorrectProblem(NugetVersionMismatch problem)
        {
            var badReference = problem.Item;

            _nugetPackageModifier.ChangeNugetPackageVersionInPackagesConfig(
                project: badReference.Project,
                packageID: badReference.Package.ID,
                dotNetVersion: badReference.Project.DotNetVersion,
                newVersion: badReference.Version);

            return new CorrectionResult(problem, Resolution.Solved);
        }
    }
}
