using ProjectSanitizer.Services;
using System.Collections.Generic;

namespace ProjectSanitizerConsole.Models
{
    public class CommandOutput
    {
        public List<SmartStringBuilder> TextOutput { get; } = new List<SmartStringBuilder>();
    }
}
