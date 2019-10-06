using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ProjectSanitizer.Base;
using ProjectSanitizer.Tests;
using ProjectSanitizerConsole.Services;
using ProjectSanitizerConsole.Services.Commands;

namespace ProjectSanitizer.Console.Tests
{
    [TestFixture]
    public class TestBase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            DIRegistrar.RegisterTypes += (services => services
                .AddImplementationsOf<ICommandHandler>()
                .AddSingleton<ProblemRendererService>()
                .AddSingleton<CommandLineHandler>());
        }

        [SetUp]
        public void TestSetup()
        {
            TestPaths.RevertAllCsProjAndPackagesConfigFiles();
        }

        [TearDown]
        public void TestTeardown()
        {
            TestPaths.RevertAllCsProjAndPackagesConfigFiles();
        }
    }


}
