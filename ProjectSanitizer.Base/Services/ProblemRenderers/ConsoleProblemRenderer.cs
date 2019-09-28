using ProjectSanitizer.Models.SmartString;
using System;

namespace ProjectSanitizer.Services.ProblemRenderers
{
    public class ConsoleProblemRenderer : ProblemRenderer
    {
        protected override void Render(StringSection text)
        {
            switch (text.SectionType)
            {
                case StringSectionType.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case StringSectionType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case StringSectionType.Fatal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case StringSectionType.Highlighted:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }

            if(text.IsMultiLine)
            {
                Console.WriteLine();
                foreach (var item in text.Lines)
                {
                    Console.Write("\t");
                    Console.WriteLine(item);
                }
                Console.WriteLine();
            }
            else 
                Console.Write(text.Text);


            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

    }
}
