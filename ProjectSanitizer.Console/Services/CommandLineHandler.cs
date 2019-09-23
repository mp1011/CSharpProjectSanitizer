using ProjectSanitizerConsole.Models;
using ProjectSanitizerConsole.Services.Commands;
using System;
using System.Linq;

namespace ProjectSanitizerConsole.Services
{
    class CommandLineHandler
    {
        public ICommandHandler[] _handlers;

        public CommandLineHandler(ICommandHandler[] handlers)
        {
            _handlers = handlers;
        }

        public void ExecuteCommand(CommandLineArgs args)
        {
            var handler = _handlers.SingleOrDefault(h => h.CommandName == args.Command);
            if (handler == null)
                throw new Exception("Invalid command");

            var result = handler.Execute(args);
            foreach (var textOutput in result.TextOutput)
                SmartStringWriter.WriteToConsole(textOutput);
        }
    }
}
