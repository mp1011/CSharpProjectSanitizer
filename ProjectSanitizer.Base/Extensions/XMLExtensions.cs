using System.Xml;

namespace ProjectSanitizer.Extensions
{
    public static class XMLExtensions
    {
        public static void AddAttribute(this XmlNode node, string name, string value=null)
        {
            var attribute = node.OwnerDocument.CreateAttribute(name);
            node.Attributes.Append(attribute);
            attribute.Value = value;
        }
    }
}
