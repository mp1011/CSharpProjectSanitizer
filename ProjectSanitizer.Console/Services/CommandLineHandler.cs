using ProjectSanitizerConsole.Models;
using ProjectSanitizerConsole.Services.Commands;
using System;
using System.IO;
using System.Linq;

namespace ProjectSanitizerConsole.Services
{
    class CommandLineHandler
    {
        private ICommandHandler[] _handlers;
        private ProblemRendererService _problemRendererService;

        public CommandLineHandler(ICommandHandler[] handlers, ProblemRendererService problemRendererFactory)
        {
            _handlers = handlers;
            _problemRendererService = problemRendererFactory;
        }

        public void ExecuteCommand(CommandLineArgs args)
        {
            var handler = _handlers.SingleOrDefault(h => h.CommandName == args.Command);
            if (handler == null)
                throw new Exception("Invalid command");

            var result = handler.Execute(args);
            _problemRendererService.RenderResults(result, args);
        }
    }
}
