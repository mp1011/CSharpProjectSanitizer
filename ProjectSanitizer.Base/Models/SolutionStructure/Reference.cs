using ProjectSanitizer.Base.Models.FileModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSanitizer.Base.Models.SolutionStructure
{
    public class Reference
    {
        public VerifiedFolder BaseDirectory { get; }

        public ReferenceInclude Include { get; }

        public Reference(VerifiedFolder baseDirectory, ReferenceInclude include, string relativePath)
        {
            BaseDirectory = baseDirectory;
            RelativePath = relativePath;
            Include = include; 
        }

        public string RelativePath { get; }

        public VerifiedFile TryGetFile()
        {
            return BaseDirectory.GetRelativeFileOrDefault(RelativePath);
        }
    }
}
