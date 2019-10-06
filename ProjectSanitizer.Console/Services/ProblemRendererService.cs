using ProjectSanitizer.Models;
using ProjectSanitizer.Services.Interfaces;
using ProjectSanitizer.Services.ProblemRenderers;
using ProjectSanitizerConsole.Models;
using System.Collections.Generic;
using System.IO;

namespace ProjectSanitizerConsole.Services
{
    public class ProblemRendererService
    {
        public void RenderResults(CommandOutput output, CommandLineArgs args)
        {
            foreach(var renderer in GetRenderers(args))
            {
                renderer.RenderOutput(output);
            }
        }

        public IEnumerable<IProblemRenderer> GetRenderers(CommandLineArgs args)
        {
            if (args.RenderToConsole)
                yield return new ConsoleProblemRenderer();

            if (!string.IsNullOrEmpty(args.OutputPath) && args.OutputPath.ToLower().EndsWith(".html"))
                yield return new HTMLProblemRenderer(new FileInfo(args.OutputPath));
        }
    }
}
