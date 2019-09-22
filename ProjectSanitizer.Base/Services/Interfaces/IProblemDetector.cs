using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Services.Interfaces
{
    public interface IProblemDetector
    {
      
    }

    public interface IProblemDetector<T> : IProblemDetector
    {
        IEnumerable<Problem> DetectProblems(T item);
    }
}
