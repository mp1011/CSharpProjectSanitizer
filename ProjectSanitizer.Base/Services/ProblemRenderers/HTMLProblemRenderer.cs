using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models.SmartString;
using System.IO;
using System.Text;

namespace ProjectSanitizer.Services.ProblemRenderers
{
    public class HTMLProblemRenderer : ProblemRenderer
    {
        private TextWriter _writer;

        public HTMLProblemRenderer(TextWriter writer)
        {
            _writer = writer;
        }

        protected override void Render(StringSection stringSection)
        {
            if (stringSection.IsMultiLine)
            {
                _writer.Write($"<ul class='{stringSection.SectionType}'>");
                foreach (var item in stringSection.Lines)
                {
                    _writer.Write("<li>");
                    _writer.Write(item.ToString());
                    _writer.Write("</li>");
                }
                _writer.Write("</ul>");
            }
            else
            {
                _writer.Write($"<span class='{stringSection.SectionType}'>{stringSection.Text}</span>");
            }
        }

        protected override void BeginRenderProblems()
        {
            _writer.Write("<html><head>");
            WriteCSSRules();
            _writer.Write("</head><body><ul>");
        }

        protected override void EndRenderProblems()
        {
            _writer.Write("</ul></body></html>");
        }

        protected override void BeginRenderProblem(Problem problem)
        {
            _writer.Write("<li>");
        }
        protected override void EndRenderProblem(Problem problem)
        {
            _writer.Write("</li>");
        }

        private void WriteCSSRules()
        {
            _writer.Write("<style type='text/css'>");
            _writer.Write("span.Highlighted { font-weight:bold; color:red } ");
            _writer.Write("</style>");


        }
    }
}
