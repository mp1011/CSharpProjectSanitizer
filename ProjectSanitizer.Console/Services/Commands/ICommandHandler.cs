using ProjectSanitizerConsole.Models;

namespace ProjectSanitizerConsole.Services.Commands
{
    public interface ICommandHandler
    {
        string CommandName { get; }
        CommandOutput Execute(CommandLineArgs arg);
    }
}
