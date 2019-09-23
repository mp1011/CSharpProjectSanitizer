﻿using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Services;
using System.Linq;

namespace ProjectSanitizer.Models.Problems
{
    public class MultipleNugetVersions : Problem<SolutionGraph>
    {
        public VersionWithSuffix[] DistinctVersions { get; }

        public string PackageID { get; }

        public MultipleNugetVersions(SolutionGraph solution, string packageID, VersionWithSuffix[] distinctVersions)
            : base(solution)
        {
            DistinctVersions = distinctVersions;
            PackageID = packageID;
        }

        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("Multiple versions of ").AppendHighlighted(PackageID)
            .AppendError(" exist within ").AppendHighlighted(Item.Solution);
    }
}
