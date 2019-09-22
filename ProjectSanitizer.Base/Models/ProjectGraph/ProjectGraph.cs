using ProjectSanitizer.Base.Extensions;
using ProjectSanitizer.Base.Models.SolutionStructure;
using System.Collections.Generic;

namespace ProjectSanitizer.Base.Models.ProjectGraph
{
    public class ProjectGraph
    {
        public ProjectGraph(ProjectGraphNode root)
        {
            Root = AddNode(root);            
        }

        public ProjectGraphNode Root { get; }

        public Dictionary<string, ProjectGraphNode> AllNodes { get; } = new Dictionary<string, ProjectGraphNode>();

        public ProjectGraphNode AddNode(ProjectGraphNode node)
        {
            AllNodes[node.ToString()] = node;
            return node;
        }

        public ProjectGraphNode GetOrAdd(Project project)
        {
            return AllNodes.TryGet(project.FullPath) ??
                AddNode(new ProjectGraphNode(project));
        }
    }
}
