using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSanitizer.Base.Services
{
    public class SolutionReader : ISolutionReader
    {
        private IProjectReader _projectReader;

        public SolutionReader(IProjectReader projectReader)
        {
            _projectReader = projectReader;
        }

        public Solution ReadSolution(VerifiedFile slnFile)
        {
            var projectLines = slnFile.ReadAllLines()
                .Select(line => TryParseProjectLine(line))
                .Where(line => line != null)
                .ToArray();

            var projects = projectLines.Select(line =>
            {
                var file = slnFile.GetRelativeFileOrDefault(line.RelativePath);
                if (file != null)
                    return _projectReader.ReadProject(file);
                else
                    return null;
            })
                .Where(p => p != null)
                .ToArray();

            var sln = new Solution(slnFile, projects, projectLines);
            return sln;
        }

        /// <summary>
        /// Parses a line of the form Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "SampleProject", "..\SampleProject\SampleProject.csproj", "{06470DC6-A1EB-438B-A6BB-DA2ABF039508}"
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private ProjectLine TryParseProjectLine(string line)
        {
            if (!line.Trim().StartsWith("Project"))
                return null;

            var eqSplit = line.Split('=');
            if (eqSplit.Length != 2)
                return null;

            var commaSplit = eqSplit[1].Split(',');

            if (commaSplit.Length < 2)
                return null;

            var relativePath = commaSplit[1].Trim().Trim('"');
            return new ProjectLine(commaSplit[0].Trim().Trim('"'), relativePath);
        }
    }
}
