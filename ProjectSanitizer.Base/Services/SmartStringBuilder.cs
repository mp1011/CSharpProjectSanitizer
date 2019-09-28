using ProjectSanitizer.Models.SmartString;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSanitizer.Services
{
    public class SmartStringBuilder : IEnumerable<StringSection>
    {
        private List<StringSection> _text = new List<StringSection>();

        public SmartStringBuilder Append(object text)
        {
            _text.Add(new StringSection(text, StringSectionType.Normal));
            return this;
        }

        public SmartStringBuilder AppendLine(object text)
        {
            return Append(text).Append(Environment.NewLine);
        }

        public SmartStringBuilder AppendFatal(object text)
        {
            _text.Add(new StringSection(text, StringSectionType.Fatal));
            return this;
        }

        public SmartStringBuilder AppendError(object text)
        {
            _text.Add(new StringSection(text, StringSectionType.Error));
            return this;
        }

        public SmartStringBuilder AppendIndentedList(string[] listItems)
        {
            Append(Environment.NewLine);
            foreach (var item in listItems)
                AppendHighlighted("\t" + item + Environment.NewLine);
            Append(Environment.NewLine);
            return this;
        }

        public SmartStringBuilder AppendHighlighted(object text)
        {
            _text.Add(new StringSection(text, StringSectionType.Highlighted));
            return this;
        }

        public override string ToString()
        {
            return string.Join("", _text.Select(t => t.ToString()));
        }

        public IEnumerator<StringSection> GetEnumerator()
        {
            return ((IEnumerable<StringSection>)_text).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<StringSection>)_text).GetEnumerator();
        }
    }
}
