using System.Collections.Generic;

namespace act.localization
{
    public class LanguageModule
    {
        public bool IsEmpty
        {
            get
            {
                return stringDict.Count == 0;
            }
        }

        private Dictionary<string, string> stringDict = new Dictionary<string, string>();

        public LanguageModule(byte[] bytes)
        {
            List<data.IniSection> sections = new List<data.IniSection>();
            data.IniReader reader = new data.IniReader();
            reader.Read(bytes, sections);
            if(sections.Count == 0)
            {
                return;
            }

            sections[0].GetKeyValuePairs(stringDict);
        }

        public LanguageModule(data.IniSection section)
        {
            section.GetKeyValuePairs(stringDict);
        }

        public string GetString(string key)
        {
            if(!stringDict.TryGetValue(key, out string value))
            {
                debug.PrintSystem.LogWarning("[LanguageModule] Can't find string: " + key);
                return string.Format("#{0}", key);
            }

            return value;
        }
    }
}