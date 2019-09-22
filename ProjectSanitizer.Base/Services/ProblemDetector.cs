using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSanitizer.Base.Services
{
    public class ProblemDetector
    {
        private IProblemDetector[] _problemDetectors;
        private IProjectGraphBuilder _graphBuilder;
        private INugetReferenceReader _nugetReferenceReader;

        public ProblemDetector(IProblemDetector[] problemDetectors, IProjectGraphBuilder graphBuilder,
            INugetReferenceReader nugetReferenceReader)
        {
            _problemDetectors = problemDetectors;
            _graphBuilder = graphBuilder;
            _nugetReferenceReader = nugetReferenceReader;
        }

        public Problem[] DetectProblems<T>(T item)
        {
            List<Problem> problems = new List<Problem>();
            foreach(var detector in _problemDetectors.OfType<IProblemDetector<T>>())
                problems.AddRange(detector.DetectProblems(item));
            return problems.ToArray();
        }

        public Problem[] DetectAllSolutionProblems(Solution solution)
        {
            List<Problem> problems = new List<Problem>();
            var solutionGraph = _graphBuilder.BuildGraph(solution);

            problems.AddRange(DetectProblems(solutionGraph));
            foreach(var node in solutionGraph.AllNodes.Values)
            {
                problems.AddRange(DetectProblems(node));
                foreach (var nugetRef in node.NugetPackageRequirements)
                    problems.AddRange(DetectProblems(nugetRef));
            }

            foreach (var project in solution.Projects)
            {
                problems.AddRange(DetectProblems(project));

                foreach(var reference in project.FileReferences)
                    problems.AddRange(DetectProblems(reference));

                foreach (var reference in project.ProjectReferences)
                    problems.AddRange(DetectProblems(reference));

                var packagesConfig = _nugetReferenceReader.TryReadPackagesConfig(project.ProjectDirectory);
                if (packagesConfig != null)
                {
                    problems.AddRange(DetectProblems(packagesConfig));
                    foreach (var package in packagesConfig.Packages)
                        problems.AddRange(DetectProblems(package));
                }
            }

          

            return problems.ToArray();
        }
    }
}
