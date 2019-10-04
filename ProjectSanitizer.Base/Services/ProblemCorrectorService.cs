using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models;
using ProjectSanitizer.Services.Interfaces;
using System;

namespace ProjectSanitizer.Services
{
    public class ProblemCorrectorService
    {
        private IProblemCorrector[] _problemCorrectors;

        public ProblemCorrectorService(IProblemCorrector[] problemCorrector)
        {
            _problemCorrectors = problemCorrector;
        }

        public CorrectionResult TryCorrectProblem(Problem problem)
        {
            foreach(var corrector in _problemCorrectors)
            {
                var result = corrector.TryCorrectProblem(problem);
                if (result.Resolution != Resolution.NoActionTaken)
                    return result;
            }
             
            return new CorrectionResult(problem, Resolution.NoActionTaken);
        }
    }
}
