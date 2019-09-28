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

        public SmartStringBuilder Append(params string[] text)
        {
            _text.Add(new StringSection(StringSectionType.Normal,text));
            return this;
        }

        public SmartStringBuilder AppendLine(params string[] text)
        {
            return Append(text).Append(Environment.NewLine);
        }

        public SmartStringBuilder AppendFatal(params string[] text)
        {
            _text.Add(new StringSection(StringSectionType.Fatal,text));
            return this;
        }

        public SmartStringBuilder AppendError(params string[] text)
        {
            _text.Add(new StringSection(StringSectionType.Error,text));
            return this;
        }

        public SmartStringBuilder AppendHighlighted(params string[] text)
        {
            _text.Add(new StringSection(StringSectionType.Highlighted, text));
            return this;
        }

        public SmartStringBuilder AppendHighlighted(object text)
        {
            if(text is IEnumerable enumerable)
            {
                List<string> strings = new List<string>();
                foreach (object item in enumerable)
                    strings.Add(item.ToString());

                AppendHighlighted(strings.ToArray());
            }
            else    
                _text.Add(new StringSection(StringSectionType.Highlighted, text.ToString()));
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
