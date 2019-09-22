using Microsoft.Extensions.DependencyInjection;
using ProjectSanitizer.Base;
using ProjectSanitizerConsole.Services;
using ProjectSanitizerConsole.Services.Commands;
using System;

namespace ProjectSanitizerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DIRegistrar.RegisterTypes += (services => services
                .AddImplementationsOf<ICommandHandler>()
                .AddSingleton<CommandLineHandler>());

            var consoleArgs = new CommandLineParser().Parse(args);
            var handler = DIRegistrar.GetInstance<CommandLineHandler>();
            handler.ExecuteCommand(consoleArgs);
        }
    }
}
