using ProjectSanitizer.Base.Models;
using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Base.Models.ProjectGraph;
using ProjectSanitizer.Base.Models.SolutionStructure;
using ProjectSanitizer.Base.Services;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Extensions;
using ProjectSanitizer.Models.SolutionStructure;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace ProjectSanitizer.Services
{
    public class NugetPackageModifier
    {
        private INugetReferenceReader _nugetReader;

        public NugetPackageModifier(INugetReferenceReader nugetReader)
        {
            _nugetReader = nugetReader;
        }

        public NugetReference GetLatestNugetReference(string packageID, bool includePrereleases, SolutionGraph solution)
        {
            return solution.AllNodes.Values
                .SelectMany(proj => proj.NugetPackageRequirements)
                .Where(nuget => nuget.Package.ID == packageID && (includePrereleases || !nuget.Package.Version.HasSuffix))
                .Where(nuget=>nuget.Version == nuget.Package.Version)
                .OrderByDescending(nuget => nuget.Version)
                .FirstOrDefault();
        }

        public void AddOrModifyNugetReference(NugetReference referenceToCopy, Project project, VerifiedFolder packagesDirectory)
        {
            AddOrModifyReferencePackagesConfig(referenceToCopy, project);
            AddOrModifyReferenceInProjectFile(referenceToCopy, project, packagesDirectory);
        }

        public PackagesConfig CreateNewPackagesConfig(VerifiedFolder projectDirectory)
        {
            string[] lines = new string[]
            {
                @"<?xml version=""1.0"" encoding=""utf-8""?>",
                "<packages>",
                "</packages>"
            };

            var filePath = projectDirectory.FullName + @"\packages.config"; 
            File.WriteAllLines(filePath, lines);
            return _nugetReader.TryReadPackagesConfig(projectDirectory);
        }

        public void ChangeNugetPackageVersionInPackagesConfig(Project project, string packageID, DotNetVersion dotNetVersion, VersionWithSuffix newVersion)
        {
            var packagesConfig = _nugetReader.TryReadPackagesConfig(project.ProjectDirectory) ??
               CreateNewPackagesConfig(project.ProjectDirectory);

            var xmlNode = packagesConfig.GetPackageNode(packageID);
            if (xmlNode == null)
            {
                var packagesNode = packagesConfig.GetPackagesNodes();
                xmlNode = packagesNode.OwnerDocument.CreateElement("package");
                packagesNode.AppendChild(xmlNode);
                xmlNode.AddAttribute("id", packageID);
                xmlNode.AddAttribute("version");
                xmlNode.AddAttribute("targetFramework");
            }

            xmlNode.Attributes["version"].Value = newVersion.ToString();
            xmlNode.Attributes["targetFramework"].Value = project.DotNetVersion.ToPackagesConfigString();


            packagesConfig.SaveChanges();
        }

        public void AddOrModifyReferencePackagesConfig(NugetReference referenceToCopy, Project project)
        {
            var packagesConfig = _nugetReader.TryReadPackagesConfig(project.ProjectDirectory) ??
                CreateNewPackagesConfig(project.ProjectDirectory);
             
            var xmlNode = packagesConfig.GetPackageNode(referenceToCopy.Package.ID);
            if (xmlNode == null)
            {
                var packagesNode = packagesConfig.GetPackagesNodes();
                xmlNode = packagesNode.OwnerDocument.CreateElement("package");
                packagesNode.AppendChild(xmlNode);
                xmlNode.AddAttribute("id", referenceToCopy.Package.ID);
                xmlNode.AddAttribute("version");
                xmlNode.AddAttribute("targetFramework");
            }

            xmlNode.Attributes["version"].Value = referenceToCopy.Package.Version.ToString();
            xmlNode.Attributes["targetFramework"].Value = project.DotNetVersion.ToPackagesConfigString();
            

            packagesConfig.SaveChanges();
        }

        public void AddOrModifyReferenceInProjectFile(NugetReference referenceToCopy, Project project, VerifiedFolder packagesDirectory)
        {
            var srcNode = referenceToCopy.Project.GetReferenceNode(referenceToCopy.Package.ID)
                   ?? throw new System.Exception("Unable to read source reference node for package " + referenceToCopy.Package.ID);

            var xmlNode = project.GetReferenceNode(referenceToCopy.Package.ID);
            if (xmlNode != null)
            {
                var copy = xmlNode.OwnerDocument.ImportNode(srcNode, true);
                xmlNode.ParentNode.ReplaceChild(newChild: copy, oldChild: xmlNode);
                project.SaveChanges();
            }
            else
            {
                var itemGroupNode = project.GetReferenceItemGroupNode();
                var copy = itemGroupNode.OwnerDocument.ImportNode(srcNode, true);
                itemGroupNode.AppendChild(copy);
            }

            var hintPathNode = project.GetReferenceHintPathNode(referenceToCopy.Package.ID);
            if (hintPathNode != null)
                hintPathNode.InnerText = CorrectPackagesPath(project.ProjectDirectory, packagesDirectory, hintPathNode.InnerText);

            project.SaveChanges();
        }

        public string CorrectPackagesPath(VerifiedFolder projectDirectory, VerifiedFolder packagesDirectory, string path)
        {
            if (!path.ToLower().Contains("packages"))
                return path;

            path = path.Substring(path.IndexOf("packages"));
         
            var folder = projectDirectory;
            while(folder != null && folder.GetRelativeFolderOrDefault("packages") == null)
            {
                folder = folder.Parent;
                path = @"..\" + path;
            }

            return path;
            
        }
    }
}
