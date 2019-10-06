using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Tests;
using ProjectSanitizerConsole.Models;
using ProjectSanitizerConsole.Services;

namespace ProjectSanitizer.Console.Tests.Services.Commands
{
    public  class DetectCommandHandlerTests :TestBase
    {

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln",7)]
        public void DetectProblemsFromCommandLine(string slnPath, int expectedProblems)
        {
            var sln = TestPaths.GetFileRelativeToProjectDir(slnPath);
            var args = new CommandLineArgs
            (
                command: "detect",
                solutionFile: sln.FullName,
                outputPath: string.Empty
            );

            var handler = DIRegistrar.GetInstance<CommandLineHandler>();
            var result = handler.ExecuteCommand(args);
            Assert.AreEqual(expectedProblems, result.DetectedProblems.Count);
        }
    }
}
