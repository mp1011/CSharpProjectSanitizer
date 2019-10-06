using System;
using System.Collections.Generic;
using System.Text;
using ProjectSanitizer.Models;
using ProjectSanitizer.Services;
using ProjectSanitizerConsole.Models;

namespace ProjectSanitizerConsole.Services.Commands
{
    public class HelpCommandHandler : ICommandHandler
    {
        public string CommandName => "help";

        public CommandOutput Execute(CommandLineArgs arg)
        {
            var output = new CommandOutput();
            output.Messages.Add(new SmartStringBuilder()
                .Append("dotnet [path to ")
                .AppendHighlighted("ProjectSanitizerConsole.dll")
                .Append("] [action: ")
                .AppendHighlighted("detect|correct|help")
                .Append("] --sln [")
                .AppendHighlighted("Path to .sln file to inspect")
                .Append("] --html [")
                .AppendHighlighted("Optional path to html results file")
                .Append("]"));
            return output;

        }
    }
}
