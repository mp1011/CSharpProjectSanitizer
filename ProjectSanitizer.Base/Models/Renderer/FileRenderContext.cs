using System.IO;

namespace ProjectSanitizer.Models.Renderer
{
    public class FileRenderContext : IRenderContext
    {
        public FileRenderContext(TextWriter writer)
        {
            Writer = writer;
        }

        public TextWriter Writer { get; }
    }
}
