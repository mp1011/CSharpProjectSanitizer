namespace ProjectSanitizerConsole.Models
{
    public class CommandLineArgs
    {
        public CommandLineArgs(string command, string solutionFile)
        {
            Command = command;
            SolutionFile = solutionFile;
        }

        public string Command { get; }
        public string SolutionFile { get; }
    }
}
