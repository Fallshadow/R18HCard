using System.Collections.Generic;
using System.IO;

namespace act.data
{
    public class IniReader
    {
        private const string SectionStartTag = "[";
        private const string SectionEndTag = "]";
        private const string CommentTag = "//";

        public void Read(byte[] bytes, List<IniSection> result)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (StreamReader sr = new StreamReader(ms))
            {
                result.Clear();
                IniSection section = new IniSection(string.Empty);
                result.Add(section);
                string line = null;
                while (true)
                {
                    line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    if (line == string.Empty || line.StartsWith(CommentTag))
                    {
                        continue;
                    }

                    if (line.StartsWith(SectionStartTag) && line.EndsWith(SectionEndTag))
                    {
                        section = new IniSection(line.Substring(1, line.Length - 2));
                        result.Add(section);
                        continue;
                    }

                    section.AddContent(line);
                }
            }
        }
    }
}