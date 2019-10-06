using Microsoft.Extensions.CommandLineUtils;
using ProjectSanitizerConsole.Models;

namespace ProjectSanitizerConsole.Services
{
    public class CommandLineParser
    {
        public CommandLineArgs Parse(string[] args)
        {
            CommandLineArgs commandLineArgs = null;

            var app = new CommandLineApplication();

            var command = app.Argument("Command", "");

            var solutionPath = app.Option("-s|--sln", "Path to sln file", CommandOptionType.SingleValue);
            var htmlOutput = app.Option("--html", "HTML Output file", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                commandLineArgs = new CommandLineArgs(
                    command: command.Value, 
                    solutionFile: solutionPath.Value(),
                    outputPath: htmlOutput.Value()
                    );

                return 0;
            });

            try
            {
                app.Execute(args);

                return commandLineArgs ?? new CommandLineArgs("", "", "");
            }
            catch
            {
                return new CommandLineArgs("help", null, null);
            }
        }
    }
}
