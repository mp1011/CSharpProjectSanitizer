using ProjectSanitizer.Models.SmartString;
using ProjectSanitizer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSanitizerConsole.Services
{
    public class SmartStringWriter
    {
        public static void WriteToConsole(SmartStringBuilder stringBuilder)
        {
            foreach(var textPart in stringBuilder)
                WriteToConsole(textPart);

            Console.WriteLine();
        }

        public static void WriteToConsole(StringSection text)
        {
            switch(text.SectionType)
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

            Console.Write(text);

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
