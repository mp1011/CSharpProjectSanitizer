using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models;
using ProjectSanitizer.Services;
using ProjectSanitizerConsole.Models;
using System.Linq;

namespace ProjectSanitizerConsole.Services.Commands
{
    public class CorrectCommandHandler : ICommandHandler
    {
        public string CommandName => "correct";

        private ProblemDetectorService _problemDetector;
        private ISolutionReader _solutionReader;
        private ProblemCorrectorService _correctorService;

        public CorrectCommandHandler(ProblemDetectorService problemDetector, ISolutionReader solutionReader, ProblemCorrectorService correctorService)
        {
            _problemDetector = problemDetector;
            _solutionReader = solutionReader;
            _correctorService = correctorService;
        }

        public CommandOutput Execute(CommandLineArgs arg)
        {
            var output = new CommandOutput();

            var slnFile = VerifiedFile.GetFileIfExisting(arg.SolutionFile);
            if (slnFile == null)
            {
                output.Messages.Add(new SmartStringBuilder()
                                            .AppendFatal("Unable to find solution ")
                                            .AppendHighlighted(arg.SolutionFile));
                return output;
            }

            int previousProblemCount = int.MaxValue;
            
            while(true)
            {
                var solution = _solutionReader.ReadSolution(slnFile);
                var problems = _problemDetector.DetectAllSolutionProblems(solution);
                output.DetectedProblems.Clear();
                output.DetectedProblems.AddRange(problems); 

                if(problems.Length >= previousProblemCount)
                {
                    output.Messages.Add(new SmartStringBuilder()
                       .Append("Unable to solve ")
                       .AppendHighlighted(problems.Length)
                       .Append(" problems"));

                    return output;
                }

                previousProblemCount = problems.Length;

                var result = problems
                    .Select(p => _correctorService.TryCorrectProblem(p))
                    .FirstOrDefault(p => p.Resolution != Resolution.NoActionTaken);

                if (result != null && result.Resolution == Resolution.FailedToSolve)
                {
                    output.Messages.Add(new SmartStringBuilder()
                        .AppendFatal("Error trying to solve problem: ")
                        .AppendHighlighted(result.Problem.GetType().Name));

                    return output;
                }
                else if (result != null && result.Resolution == Resolution.Solved)
                {
                    output.CorrectedProblems.Add(result.Problem);
                }
            }
           
        }
    }
}
