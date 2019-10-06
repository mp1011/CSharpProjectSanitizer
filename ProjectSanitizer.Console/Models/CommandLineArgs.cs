namespace ProjectSanitizerConsole.Models
{
    public class CommandLineArgs
    {
        public CommandLineArgs(string command, string solutionFile, string outputPath)
        {
            Command = command;
            SolutionFile = solutionFile;
            OutputPath = outputPath;
        }

        public bool RenderToConsole => string.IsNullOrEmpty(OutputPath);

        public string Command { get; }
        public string SolutionFile { get; }
        public string OutputPath { get; }
    }
}
