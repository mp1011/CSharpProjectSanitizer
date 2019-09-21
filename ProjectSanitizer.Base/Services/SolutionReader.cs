using ProjectSanitizer.Base.Models;
using System;
using System.IO;

namespace ProjectSanitizer.Lib.Services
{
    public class SolutionReader
    {
        private ProjectReader _projectReader;

        public SolutionReader(ProjectReader projectReader)
        {
            _projectReader = projectReader;
        }

        public Solution ReadSolution(FileInfo file)
        {
            throw new NotImplementedException();
        }
    }
}
