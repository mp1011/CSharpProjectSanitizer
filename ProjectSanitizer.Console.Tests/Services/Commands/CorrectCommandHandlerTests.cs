using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Tests;
using ProjectSanitizerConsole.Models;
using ProjectSanitizerConsole.Services;
using System.Linq;

namespace ProjectSanitizer.Console.Tests.Services.Commands
{
    public  class CorrectCommandHandlerTests :TestBase
    {

        [TestCase(@"ExampleBrokenSolutions\ExampleBrokenSolution.sln")]
        public void CorrectProblemsFromCommandLine(string slnPath)
        {
            var sln = TestPaths.GetFileRelativeToProjectDir(slnPath);
            var args = new CommandLineArgs
            (
                command: "correct",
                solutionFile: sln.FullName,
                outputPath: string.Empty
            );

            var handler = DIRegistrar.GetInstance<CommandLineHandler>();
            var result = handler.ExecuteCommand(args);
            Assert.That(result.CorrectedProblems.Any());
        }
    }
}
