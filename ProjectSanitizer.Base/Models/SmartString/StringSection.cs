using System.Linq;

namespace ProjectSanitizer.Models.SmartString
{
    public class StringSection
    {
        public StringSection(StringSectionType sectionType, params string[] lines)
        {
            SectionType = sectionType;
            Lines = lines;
        }

        public bool IsMultiLine => Lines.Count() > 1;

        public string[] Lines;
        public string Text => Lines.FirstOrDefault() ?? "(null)";
        public StringSectionType SectionType { get; }

        public override string ToString()
        {
            return Text;
        }
    }
}
