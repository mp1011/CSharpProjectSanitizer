using ProjectSanitizer.Base.Extensions;
using ProjectSanitizer.Base.Models.SolutionStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSanitizer.Base.Models.ProjectGraph
{
    public class SolutionGraph
    {
        public Dictionary<string, ProjectGraphNode> AllNodes { get; } = new Dictionary<string, ProjectGraphNode>();

        public List<ProjectGraphNode> SolutionProjects { get; } = new List<ProjectGraphNode>();

        public Solution Solution { get; }

        public SolutionGraph(Solution solution)
        {
            Solution = solution ?? throw new ArgumentNullException();
        }


        public IEnumerable<ProjectGraphNode> FindProject(Func<ProjectGraphNode,bool> condition)
        {
            return AllNodes.Values.Where(condition);
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
                AddNode(new ProjectGraphNode(project,this),isSolutionProject);

            if (isSolutionProject && !SolutionProjects.Contains(node))
                SolutionProjects.Add(node);

            return node;
        }
    }
}
