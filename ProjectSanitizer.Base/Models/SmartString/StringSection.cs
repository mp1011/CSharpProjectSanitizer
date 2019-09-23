namespace ProjectSanitizer.Models.SmartString
{
    public class StringSection
    {
        public StringSection(object obj, StringSectionType sectionType)
        {
            Object = obj;
            SectionType = sectionType;
        }

        public object Object { get; }
        public string Text => Object?.ToString() ?? "(null)";
        public StringSectionType SectionType { get; }

        public override string ToString()
        {
            return Text;
        }
    }
}
