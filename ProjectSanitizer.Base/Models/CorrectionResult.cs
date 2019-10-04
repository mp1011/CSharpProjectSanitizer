using ProjectSanitizer.Base.Models;

namespace ProjectSanitizer.Models
{
    public class CorrectionResult
    {
        public Problem Problem { get; }
        public Resolution Resolution { get; }

        public CorrectionResult(Problem problem, Resolution resolution)
        {
            Problem = problem;
            Resolution = resolution;
        }

        public override string ToString()
        {
            return $"({Resolution}) {Problem.Description.ToString()}";
        }
    }
}
