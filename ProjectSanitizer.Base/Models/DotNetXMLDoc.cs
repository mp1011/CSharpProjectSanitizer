using ProjectSanitizer.Base.Models.FileModels;
using System;
using System.Xml;

namespace ProjectSanitizer.Base.Models
{
    public class DotNetXMLDoc
    {
        private XmlDocument _document;
        private XmlNamespaceManager _namespaceManager;

        public VerifiedFile File { get; }

        public DotNetXMLDoc(VerifiedFile csProjFile)
        {
            File = csProjFile;
            _document = new XmlDocument();
            _document.Load(csProjFile.FullName);

            var xmlns = _document.DocumentElement.GetAttribute("xmlns");
            if (!string.IsNullOrEmpty(xmlns))
            {
                _namespaceManager = new XmlNamespaceManager(_document.NameTable);
                _namespaceManager.AddNamespace("tu", xmlns);
            }
        }

        public XmlNodeList SelectNodes(string xPath)
        {
            return SelectNodes(_document.DocumentElement, xPath);
        }

        public XmlNodeList SelectNodes(XmlNode from, string xPath)
        {
            if (_namespaceManager == null)
                return from.SelectNodes(xPath);
            else
            {
                if (xPath.StartsWith("//"))
                    xPath = $"//tu:" + xPath.Substring(2);
                else
                    xPath = $"tu:{xPath}";

                return from.SelectNodes(xPath, _namespaceManager);
            }
        }

        public XmlNode SelectSingleNode(XmlNode from, string xPath)
        {
            var nodes = SelectNodes(from, xPath);
            if (nodes.Count > 0)
                return nodes[0];
            else
                return null;
        }
    }
}
