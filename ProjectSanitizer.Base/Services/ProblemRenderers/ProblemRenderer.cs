using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Models;
using ProjectSanitizer.Models.Renderer;
using ProjectSanitizer.Models.SmartString;
using ProjectSanitizer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSanitizer.Services.ProblemRenderers
{
    public abstract class ProblemRenderer<TContext> : IProblemRenderer
        where TContext: IRenderContext
    {
        public void RenderOutput(CommandOutput output)
        {
            RenderOutput(output, (o, c) => RenderOutput(o, c));
        }

        private void RenderOutput(CommandOutput output, TContext context)
        {
            BeginReport(context);
            RenderMessages(context, output.Messages);
            if (output.CorrectedProblems.Any())
                RenderProblems(context, "Corrected Problems", output.CorrectedProblems);
            RenderProblems(context, "Detected Problems", output.DetectedProblems);
            EndReport(context);
        }

        protected abstract void RenderOutput(CommandOutput output, Action<CommandOutput,TContext> renderOutput);

        private void Render(TContext context, Problem problem)
        {
            BeginRenderProblem(context, problem);

            foreach (var textPart in problem.Description)
                Render(context, textPart);

            EndRenderProblem(context, problem);
        }

        protected abstract void Render(TContext context, StringSection stringSection);

        protected void RenderProblems(TContext context, string sectionTitle, IEnumerable<Problem> problems)
        {
            BeginRenderProblems(context, sectionTitle);

            foreach (var problem in problems)
                Render(context, problem);

            EndRenderProblems(context);
        }

        protected virtual void BeginReport(TContext context) { }
        protected virtual void EndReport(TContext context) { }

        protected virtual void EndRenderProblems(TContext context)
        {
        }

        protected virtual void BeginRenderProblems(TContext context, string sectionTitle)
        {
        }

        protected virtual void EndRenderProblem(TContext context, Problem problem)
        {
        }

        protected virtual void BeginRenderProblem(TContext context, Problem problem)
        {
        }

        public void RenderMessages(TContext context, IEnumerable<SmartStringBuilder> errorMessages)
        {
            foreach (var message in errorMessages)
            {
                foreach (var textPart in message)
                    Render(context, textPart);
            }
        }

      }
}
