using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models.Renderer;
using ProjectSanitizer.Models.SmartString;
using System.IO;
using System.Text;

namespace ProjectSanitizer.Services.ProblemRenderers
{
    public class HTMLProblemRenderer : FileProblemRenderer
    {
        public HTMLProblemRenderer(FileInfo outputFile) : base(outputFile) { }

    
        protected override void Render(FileRenderContext context, StringSection stringSection)
        {
            var writer = context.Writer;

            if (stringSection.IsMultiLine)
            {
                writer.Write($"<ul class='{stringSection.SectionType}'>");
                foreach (var item in stringSection.Lines)
                {
                    writer.Write("<li>");
                    writer.Write(item.ToString());
                    writer.Write("</li>");
                }
                writer.Write("</ul>");
            }
            else
            {
                writer.Write($"<span class='{stringSection.SectionType}'>{stringSection.Text}</span>");
            }
        }

        protected override void BeginReport(FileRenderContext context)
        {
            var writer = context.Writer;
            writer.Write("<html><head>");
            writer.Write("<style type='text/css'>");
            writer.Write("span.Highlighted { font-weight:bold; color:red } ");
            writer.Write("</style>");
            writer.Write("</head><body>");
        }

        protected override void EndReport(FileRenderContext context)
        {
            var writer = context.Writer;
            writer.Write("</body></html>");
        }

        protected override void BeginRenderProblems(FileRenderContext context, string sectionTitle)
        {
            var writer = context.Writer;
            writer.Write("<h1>");
            writer.Write(sectionTitle);
            writer.Write("</h1><ul>");
        }

        protected override void EndRenderProblems(FileRenderContext context)
        {
            var writer = context.Writer;
            writer.Write("</ul>");
        }

        protected override void BeginRenderProblem(FileRenderContext context, Problem problem)
        {
            var writer = context.Writer;
            writer.Write("<li>");
        }
        protected override void EndRenderProblem(FileRenderContext context, Problem problem)
        {
            var writer = context.Writer;
            writer.Write("</li>");
        }
    }
}
