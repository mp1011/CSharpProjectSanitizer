using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models.FileModels;
using ProjectSanitizer.Services;
using System.Linq;

namespace ProjectSanitizer.Models.Problems
{
    public class FileReferencedInMultipleWays : Problem
    {
        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("The file ").AppendHighlighted(Name)
            .AppendError(" is referenced in multiple paths")
            .AppendIndentedList(Paths);

        public string Name { get; }

        public string[] Paths {get;}

        public FileReferencedInMultipleWays(string filename, IFile[] files)
        {
            Name = filename;
            Paths = files.Select(f => f.FullName)
                            .Distinct()
                            .ToArray();
        }
    }
}
