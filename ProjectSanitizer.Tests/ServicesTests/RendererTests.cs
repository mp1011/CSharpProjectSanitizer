using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Models;
using ProjectSanitizer.Services;
using ProjectSanitizer.Services.ProblemRenderers;
using System;
using System.IO;
using System.Text;

namespace ProjectSanitizer.Tests.ServicesTests
{
    [TestFixture]
    public class RendererTests :TestBase
    {
        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanRenderProblemsToConsole(string relativeSlnPath)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetectorService>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);

            var renderer = new ConsoleProblemRenderer();

            var stringBuilder = new StringBuilder();

            var output = new CommandOutput();
            output.DetectedProblems.AddRange(problems);

            using (var writer = new StringWriter(stringBuilder))
            {
                Console.SetOut(writer);
                renderer.RenderOutput(output);

                Assert.That(stringBuilder.ToString().Contains("Multiple versions of Newtonsoft.Json exist"));
            }
        }

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CanRenderProblemsToHTMLFile(string relativeSlnPath)
        {
            var slnPath = TestPaths.GetVerifiedFileRelativeToProjectDir(relativeSlnPath);
            var solution = DIRegistrar.GetInstance<ISolutionReader>().ReadSolution(slnPath);

            var problemDetector = DIRegistrar.GetInstance<ProblemDetectorService>();
            var problems = problemDetector.DetectAllSolutionProblems(solution);

            var outFile = new FileInfo("testout.html");
            if (outFile.Exists)
                outFile.Delete();

            var renderer = new HTMLProblemRenderer(outFile);
            var output = new CommandOutput();
            output.DetectedProblems.AddRange(problems);

            renderer.RenderOutput(output);

            var fileText = File.ReadAllText(outFile.FullName);
            Assert.That(fileText.Contains(@"<span class='Highlighted'>Newtonsoft.Json</span>"));
        }
    }
}
