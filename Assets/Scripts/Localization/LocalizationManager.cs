using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace act.localization
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private const string ConfigFileName = "language";
        private const string LanguageSectionName = "Language";

        // 當前使用的語系所對應的字串 (e.g. 简体中文)
        public string CurrentLanguage
        {
            get
            {
                return languageDict[currentLanguageKey];
            }

            set
            {
                Dictionary<string, string>.Enumerator item = languageDict.GetEnumerator();
                while (item.MoveNext())
                {
                    if (item.Current.Value == value)
                    {
                        currentLanguageKey = item.Current.Key;
                        break;
                    }
                }
            }
        }

        // 當前使用的語系 (即language.txt中的第一個語系, e.g. _CN)
        private string currentLanguageKey = string.Empty;

        // key: _CN; value: 简体中文
        private Dictionary<string, string> languageDict = new Dictionary<string, string>();

        // key: 檔名(e.g. StoreUi_CN)
        private Dictionary<string, LanguageModule> languageModuleDict = new Dictionary<string, LanguageModule>();

        private Dictionary<string, TMP_FontAsset> fontDict = new Dictionary<string, TMP_FontAsset>(2);
        private Dictionary<string, Material> materialDict = new Dictionary<string, Material>(2);

        protected override void init()
        {
            // 讀取配置文件
            if (!loadConfigFile(data.ResourcesPathSetting.LocalizationConfig + ConfigFileName))
            {
                return;
            }

            // 讀取當前語系
            if (languageDict.Count == 0)
            {
                return;
            }

            Dictionary<string, string>.Enumerator enumerator = languageDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                currentLanguageKey = enumerator.Current.Key;
                break;
            }

            if (string.IsNullOrEmpty(currentLanguageKey))
            {
                return;
            }

            loadFontAsset();
        }

        private bool loadConfigFile(string configPath)
        {
            TextAsset file = null;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                file = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/" + configPath + ".txt");
            }
            else

#endif
            {
                file = utility.LoadResources.LoadAsset<TextAsset>(configPath);
            }
            if (file == null)
            {
                return false;
            }

            List<data.IniSection> sections = new List<data.IniSection>();
            data.IniReader reader = new data.IniReader();
            reader.Read(file.bytes, sections);
            if (sections.Count == 0)
            {
                return false;
            }

            // Parse
            for (int i = 0; i < sections.Count; ++i)
            {
                if (sections[i].Name != LanguageSectionName)
                {
                    // TODO: 待修改ini輸出格式，暫時不讀取語系檔案列表。
                    //languageModuleDict.Add(sections[i].Name, sections[i].ToArray());
                    continue;
                }

                sections[i].GetKeyValuePairs(languageDict);
            }
            return true;
        }

        private void loadFontAsset()
        {
            string path = null;
            TMP_FontAsset font = null;
            Material material = null;
            for (int i = 0; i < ui.UiStaticText.FontNames.Length; i++)
            {
                path = string.Format(
                    data.ResourcesPathSetting.LocalizationFontAssetPath,
                    ui.UiStaticText.FontNames[i],
                    currentLanguageKey);
                font = utility.LoadResources.LoadAsset<TMP_FontAsset>(path);
                fontDict.Add(path, font);

                // NOTE: Ignore default.
                for (int j = 1; j < ui.UiStaticText.MaterialNames.Length; j++)
                {
                    path = string.Format(
                        data.ResourcesPathSetting.LocalizationMaterialPath,
                        ui.UiStaticText.FontNames[i],
                        ui.UiStaticText.MaterialNames[j],
                        currentLanguageKey);

                    material = utility.LoadResources.LoadAsset<Material>(path);
                    if (material != null)
                    {
                        materialDict.Add(path, material);
                    }
                }
            }
        }

        public LanguageModule LoadModule(string moduleName)
        {
            string fileName = moduleName + currentLanguageKey;
            if (languageModuleDict.TryGetValue(fileName, out LanguageModule module))
            {
                return module;
            }
            TextAsset file = null;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                file = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/" + data.ResourcesPathSetting.LocalizationConfig + fileName + ".txt");
            }
            else
#endif
            {
                file = utility.LoadResources.LoadAsset<TextAsset>(data.ResourcesPathSetting.LocalizationConfig + fileName);
            }
            if (file == null)
            {
                return null;
            }

            module = new LanguageModule(file.bytes);
            if (module.IsEmpty)
            {
                return null;
            }

            languageModuleDict.Add(fileName, module);
            return module;
        }

        public void UnloadModule(string moduleName)
        {
            languageModuleDict.Remove(moduleName + currentLanguageKey);
        }

        public TMP_FontAsset GetLocalizedFont(string fontName)
        {
            string path = string.Format(
                data.ResourcesPathSetting.LocalizationFontAssetPath,
                fontName,
                currentLanguageKey);

            fontDict.TryGetValue(path, out TMP_FontAsset font);
            if (font == null)
            {
                //debug.PrintSystem.LogWarning("[LocalizaionManager] Can't find TMP_FontAsset: " + path);
            }

            return font;
        }

        public Material GetLocalizedMaterial(string materialName, string fontName)
        {
            if (materialName == ui.UiStaticText.MaterialNames[0])
            {
                return null;
            }

            string path = string.Format(
                data.ResourcesPathSetting.LocalizationMaterialPath,
                fontName,
                materialName,
                currentLanguageKey);

            materialDict.TryGetValue(path, out Material material);
            if (material == null)
            {
                //debug.PrintSystem.LogWarning("[LocalizaionManager] Can't find Font Material: " + path);
            }

            return material;
        }

        public string GetLocalizedString(string key, string moduleName)
        {
            LanguageModule module = LoadModule(moduleName);
            if (module == null)
            {
                return string.Format("#{0}_{1}", key, moduleName);
            }

            return module.GetString(key);
        }
    }
}