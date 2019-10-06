using ProjectSanitizer.Models;

namespace ProjectSanitizer.Services.Interfaces
{
    public interface IProblemRenderer
    {
        void RenderOutput(CommandOutput output);
    }
}
