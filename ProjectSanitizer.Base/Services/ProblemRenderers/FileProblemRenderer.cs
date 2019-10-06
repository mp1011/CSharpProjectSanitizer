using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models;
using ProjectSanitizer.Models.Renderer;
using ProjectSanitizer.Models.SmartString;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectSanitizer.Services.ProblemRenderers
{
    public abstract class FileProblemRenderer : ProblemRenderer<FileRenderContext>
    {

        private FileInfo _outputFile;

        public FileProblemRenderer(FileInfo outputFile)
        {
            _outputFile = outputFile;
        }

        protected override void RenderOutput(CommandOutput output, Action<CommandOutput, FileRenderContext> renderOutput)
        {
            if (_outputFile.Exists)
                _outputFile.Delete();

            using (var fileStream = _outputFile.OpenWrite())
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    var context = new FileRenderContext(writer);
                    renderOutput(output, context);
                }
            }
        }
        
    }
}
