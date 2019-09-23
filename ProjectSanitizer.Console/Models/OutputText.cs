using System;

namespace ProjectSanitizerConsole.Models
{
    public class OutputText
    {
        public OutputText(string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Text = text;
            TextColor = textColor;
            BackgroundColor = bgColor;
        }

        public string Text { get; }
        public ConsoleColor TextColor { get; } 
        public ConsoleColor BackgroundColor { get; }

    }
}
