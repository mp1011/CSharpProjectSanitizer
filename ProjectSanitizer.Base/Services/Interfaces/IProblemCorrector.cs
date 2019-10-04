using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models;

namespace ProjectSanitizer.Services.Interfaces
{
    public  interface IProblemCorrector
    {
        CorrectionResult TryCorrectProblem(Problem problem);
    }
}
