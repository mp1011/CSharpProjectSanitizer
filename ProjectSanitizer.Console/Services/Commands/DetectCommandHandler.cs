using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Services;
using ProjectSanitizerConsole.Models;
using System;
using System.Linq;

namespace ProjectSanitizerConsole.Services.Commands
{
    public class DetectCommandHandler : ICommandHandler
    {
        public string CommandName => "detect";

        private ProblemDetectorService _problemDetector;
        private ISolutionReader _solutionReader;

        public DetectCommandHandler(ProblemDetectorService problemDetector, ISolutionReader solutionReader)
        {
            _problemDetector = problemDetector;
            _solutionReader = solutionReader;
        }

        public CommandOutput Execute(CommandLineArgs arg)
        {
            var output = new CommandOutput();

            var slnFile = VerifiedFile.GetFileIfExisting(arg.SolutionFile);
            if (slnFile == null)
            {
                output.ErrorMessages.Add(new SmartStringBuilder()
                                            .AppendFatal("Unable to find solution ")
                                            .AppendHighlighted(arg.SolutionFile));
                return output;
            }

            var solution = _solutionReader.ReadSolution(slnFile);
            var problems = _problemDetector.DetectAllSolutionProblems(solution);

            foreach (var problem in problems)
                output.DetectedProblems.Add(problem);

            return output;
        }
    }
}
