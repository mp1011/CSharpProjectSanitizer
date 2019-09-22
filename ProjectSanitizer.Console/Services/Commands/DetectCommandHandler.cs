using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizerConsole.Models;
using System;
using System.Linq;

namespace ProjectSanitizerConsole.Services.Commands
{
    public class DetectCommandHandler : ICommandHandler
    {
        public string CommandName => "detect";

        private ProblemDetector _problemDetector;
        private ISolutionReader _solutionReader;

        public DetectCommandHandler(ProblemDetector problemDetector, ISolutionReader solutionReader)
        {
            _problemDetector = problemDetector;
            _solutionReader = solutionReader;
        }

        public CommandOutput Execute(CommandLineArgs arg)
        {
            var slnFile = VerifiedFile.GetFileIfExisting(arg.SolutionFile);
            if (slnFile == null)
                return new CommandOutput($"Unable to find solution {arg.SolutionFile}");

            var solution = _solutionReader.ReadSolution(slnFile);
            var problems = _problemDetector.DetectAllSolutionProblems(solution);

            var text = string.Join(Environment.NewLine, problems.Select(p => p.Description).ToArray());
            return new CommandOutput(text);
        }
    }
}
