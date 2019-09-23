using ProjectSanitizer.Base.Extensions;
using ProjectSanitizer.Base.Models.SolutionStructure;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Models.ProjectGraph
{
    public class SolutionGraph
    {
        public Dictionary<string, ProjectGraphNode> AllNodes { get; } = new Dictionary<string, ProjectGraphNode>();

        public List<ProjectGraphNode> SolutionProjects { get; } = new List<ProjectGraphNode>();

        public Solution Solution { get; }

        public SolutionGraph(Solution solution)
        {
            Solution = solution;
        }

        public ProjectGraphNode AddNode(ProjectGraphNode node, bool isSolutionProject)
        {
            AllNodes[node.ToString()] = node;
            if (isSolutionProject)
                SolutionProjects.Add(node);

            return node;
        }

        public ProjectGraphNode GetOrAdd(Project project, bool isSolutionProject)
        {
            var node = AllNodes.TryGet(project.FullPath) ??
                AddNode(new ProjectGraphNode(project),isSolutionProject);

            if (isSolutionProject && !SolutionProjects.Contains(node))
                SolutionProjects.Add(node);

            return node;
        }
    }
}
