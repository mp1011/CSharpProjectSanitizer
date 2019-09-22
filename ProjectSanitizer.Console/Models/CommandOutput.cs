namespace ProjectSanitizerConsole.Models
{
    public class CommandOutput
    {
        public CommandOutput(string text)
        {
            Text = text;
        }

        public string Text { get; } //can do better later, but just text for now

        public override string ToString()
        {
            return Text;
        }
    }
}
