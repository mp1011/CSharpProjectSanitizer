using ProjectSanitizer.Models;
using ProjectSanitizerConsole.Models;
using ProjectSanitizerConsole.Services.Commands;
using System;
using System.IO;
using System.Linq;

namespace ProjectSanitizerConsole.Services
{
    public class CommandLineHandler
    {
        private ICommandHandler[] _handlers;
        private ProblemRendererService _problemRendererService;

        public CommandLineHandler(ICommandHandler[] handlers, ProblemRendererService problemRendererFactory)
        {
            _handlers = handlers;
            _problemRendererService = problemRendererFactory;
        }

        public CommandOutput ExecuteCommand(CommandLineArgs args)
        {
            var handler = _handlers.SingleOrDefault(h => h.CommandName == args.Command) ??
                new HelpCommandHandler();

            var result = handler.Execute(args);
            return result;
        }
    }
}
