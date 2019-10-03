using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Services.Interfaces;
using ProjectSanitizer.Services.ProblemRenderers;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class RendererTests
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanRenderProblemsToConsole(string relativeSlnPath)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetector>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);

            var renderer = new ConsoleProblemRenderer();

            var stringBuilder = new StringBuilder();

            using (var writer = new StringWriter(stringBuilder))
            {
                Console.SetOut(writer);
                renderer.RenderProblems(problems);

                Assert.That(stringBuilder.ToString().Contains("Multiple versions of Newtonsoft.Json exist"));
            }
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanRenderProblemsToHTMLFile(string relativeSlnPath)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetector>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);

            var outFile = new FileInfo("testout.html");
            if (outFile.Exists)
                outFile.Delete();

            using(var fileStream = outFile.OpenWrite())
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    var renderer = new HTMLProblemRenderer(writer);
                    renderer.RenderProblems(problems);
                }
            }

            var fileText = File.ReadAllText(outFile.FullName);
            Assert.That(fileText.Contains(@"<span class='Highlighted'>Newtonsoft.Json</span>"));
        }
    }
}
