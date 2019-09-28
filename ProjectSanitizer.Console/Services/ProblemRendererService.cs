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
            RenderResults(new ConsoleProblemRenderer(), output);
           

            if (!string.IsNullOrEmpty(args.OutputPath) && args.OutputPath.ToLower().EndsWith(".html"))
            {
                var outFile = new FileInfo(args.OutputPath);
                if (outFile.Exists)
                    outFile.Delete();

                using (var fileStream = outFile.OpenWrite())
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        RenderResults(new HTMLProblemRenderer(writer), output);
                    }
                }
            }
        }

        private void RenderResults(IProblemRenderer renderer, CommandOutput result)
        {
            renderer.RenderErrors(result.ErrorMessages);
            renderer.RenderProblems(result.DetectedProblems);
        }
    }
}
