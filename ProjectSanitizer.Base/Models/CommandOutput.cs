using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Services;
using System.Collections.Generic;

namespace ProjectSanitizer.Models
{
    public class CommandOutput
    {
        public List<Problem> DetectedProblems { get; } = new List<Problem>();
        public List<Problem> CorrectedProblems { get; } = new List<Problem>();

        public List<SmartStringBuilder> Messages { get; } = new List<SmartStringBuilder>();

    }
}
