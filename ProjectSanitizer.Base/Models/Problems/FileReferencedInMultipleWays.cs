using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Services;

namespace ProjectSanitizer.Models.Problems
{
    public class FileReferencedInMultipleWays : Problem
    {
        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("The file ").AppendHighlighted(Name)
            .AppendError(" is referenced in multiple paths");

        public string Name { get; }

        public FileReferencedInMultipleWays(string name)
        {
            Name = name;
        }
    }
}
