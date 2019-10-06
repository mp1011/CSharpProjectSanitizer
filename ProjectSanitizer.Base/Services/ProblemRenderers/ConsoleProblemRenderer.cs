using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models;
using ProjectSanitizer.Models.Renderer;
using ProjectSanitizer.Models.SmartString;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSanitizer.Services.ProblemRenderers
{
    public class ConsoleProblemRenderer : ProblemRenderer<ConsoleRenderContext>
    {
        protected override void RenderOutput(CommandOutput output, Action<CommandOutput, ConsoleRenderContext> renderOutput)
        {
            var ctx = new ConsoleRenderContext();
            renderOutput(output, ctx);
        }

        protected override void EndRenderProblem(ConsoleRenderContext context, Problem problem)
        {
            Console.WriteLine();
        }

        protected override void BeginRenderProblems(ConsoleRenderContext context, string sectionTitle)
        {
            Console.WriteLine();
            Console.WriteLine(sectionTitle);
        }

        protected override void Render(ConsoleRenderContext context, StringSection text)
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
