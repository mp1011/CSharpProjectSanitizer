using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSanitizer.Models.Problems
{
    public class NugetVersionMismatch : Problem<NugetReference>
    {
        public override SmartStringBuilder Description => new SmartStringBuilder()
            .AppendError("Package ").AppendHighlighted(Item.Package)
            .AppendError(" should have version ").AppendHighlighted(Item.Package.Version)
            .AppendError(", but version ").AppendHighlighted(Item.Version).AppendError(" is referenced");

        public NugetVersionMismatch(NugetReference reference):base(reference)
        {

        }
    }

   
}
