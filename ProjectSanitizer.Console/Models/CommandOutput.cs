using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Services;
using System.Collections.Generic;

namespace ProjectSanitizerConsole.Models
{
    public class CommandOutput
    {
        public List<Problem> DetectedProblems { get; } = new List<Problem>();

        public List<SmartStringBuilder> ErrorMessages { get; } = new List<SmartStringBuilder>();
    }
}
