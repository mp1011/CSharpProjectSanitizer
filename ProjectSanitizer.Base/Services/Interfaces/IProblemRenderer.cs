using ProjectSanitizer.Base.Models;
using System.Collections.Generic;
using System.IO;

namespace ProjectSanitizer.Services.Interfaces
{
    public interface IProblemRenderer
    {
        void Render(Problem problem);
        void RenderProblems(IEnumerable<Problem> problems);
        void RenderErrors(IEnumerable<SmartStringBuilder> errorMessages);
    }
}
