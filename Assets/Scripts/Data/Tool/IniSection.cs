using System.Collections.Generic;

namespace act.data
{
    public class IniSection
    {
        private const char KeyValueSeparator = '=';

        public string Name { get; private set; }

        private List<string> contents;

        public IniSection(string name)
        {
            Name = name;
            contents = new List<string>();
        }

        public void AddContent(string content)
        {
            contents.Add(content);
        }

        public string[] ToArray()
        {
            string[] result = new string[contents.Count];
            contents.CopyTo(result);
            return result;
        }

        public void GetKeyValuePairs(Dictionary<string, string> result)
        {
            int separatorIndex = 0;
            string key = null;
            string value = null;
            for (int i = 0; i < contents.Count; ++i)
            {
                separatorIndex = contents[i].IndexOf(KeyValueSeparator);
                if (separatorIndex < 1)
                {
                    debug.PrintSystem.LogError("[IniSection] Parse content error: " + contents[i]);
                    continue;
                }

                key = contents[i].Substring(0, separatorIndex);
                key = key.Trim();
                value = contents[i].Substring(separatorIndex + 1);
                value = value.Replace("\\n", "\n");
                result.Add(key, value);
            }
        }
    }
}