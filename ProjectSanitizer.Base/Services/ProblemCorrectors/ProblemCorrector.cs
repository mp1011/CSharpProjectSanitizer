using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models;
using ProjectSanitizer.Services.Interfaces;

namespace ProjectSanitizer.Services.ProblemCorrectors
{
    public abstract class ProblemCorrector<TProblem> : IProblemCorrector
        where TProblem : Problem
    {
        public CorrectionResult TryCorrectProblem(Problem problem)
        {
            if (problem is TProblem t)
            {
                try
                {
                    return TryCorrectProblem(t);
                }
                catch
                {
                    return new CorrectionResult(problem, Resolution.FailedToSolve);
                }
            }
            else
                return new CorrectionResult(problem, Resolution.NoActionTaken);
        }

        protected abstract CorrectionResult TryCorrectProblem(TProblem problem);
    }
}
