using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models.SmartString;
using ProjectSanitizer.Services.Interfaces;
using System.Collections.Generic;

namespace ProjectSanitizer.Services.ProblemRenderers
{
    public abstract class ProblemRenderer : IProblemRenderer
    {
        public void Render(Problem problem)
        {
            BeginRenderProblem(problem);

            foreach (var textPart in problem.Description)
                Render(textPart);

            EndRenderProblem(problem);
        }

        protected abstract void Render(StringSection stringSection);

        public void RenderProblems(IEnumerable<Problem> problems)
        {
            BeginRenderProblems();

            foreach (var problem in problems)
                Render(problem);

            EndRenderProblems();
        }

        protected virtual void EndRenderProblems()
        {
        }

        protected virtual void BeginRenderProblems()
        {
        }

        protected virtual void EndRenderProblem(Problem problem)
        {
        }
        protected virtual void BeginRenderProblem(Problem problem)
        {
        }

        public void RenderErrors(IEnumerable<SmartStringBuilder> errorMessages)
        {
            foreach (var message in errorMessages)
            {
                foreach (var textPart in message)
                    Render(textPart);
            }
        }
    }
}
