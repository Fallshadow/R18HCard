using data;
using System.Collections.Generic;

namespace act.data
{
    //用enum為dictionary的key時，做Equals時，內部會自動產生Object物件再比較
    //故實作此Comparer避免產生Object
    public class CONFIGPATHEnumComparer : IEqualityComparer<CONFIG_PATH>
    {
        public bool Equals(CONFIG_PATH sfx1, CONFIG_PATH sfx2)
        {
            return sfx1 == sfx2;
        }

        public int GetHashCode(CONFIG_PATH sfx)
        {
            return (int)sfx;
        }
    }

    public static class ConfigDataMgr
    {
        public const int SITE_RATIO = 1000;
        public const int MILLISECOND = 1000;
        public const int TEN_THOUSAND = 10000;

        public const float THOUSANDTH = 0.001f;
        public const float ONE_IN_TEN_THOUSAND = 0.0001f;

        private static Config<CONFIG_PATH> config = new Config<CONFIG_PATH>();
        private static Dictionary<CONFIG_PATH, object> configDataDictionary = new Dictionary<CONFIG_PATH, object>(new CONFIGPATHEnumComparer());

        /// <summary>
        /// 讀取表單 並且回傳list格式
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="list">回傳的list</param>
        /// <param name="path">目標表單的名稱 會根據enum的Description 去抓資料</param>
        public static void GetDataList<TData>(this List<TData> list, CONFIG_PATH path) where TData : new()
        {
            List<TData> tempList = new List<TData>();
            list.Clear();

            if (configDataDictionary.ContainsKey(path))
            {
                tempList = configDataDictionary[path] as List<TData>;
            }

            if (tempList == null || !configDataDictionary.ContainsKey(path))
            {
                ReleaseConfigData(path);
                LoadConfigData<TData>(path);
                tempList = configDataDictionary[path] as List<TData>;
            }

            if (tempList != null)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    list.Add(tempList[i]);
                }
            }
        }

        public static void ReleaseConfigData(CONFIG_PATH path)
        {
            if (configDataDictionary.ContainsKey(path))
            {
                configDataDictionary.Remove(path);
            }
        }

        public static void ReleaseAllConfig()
        {
            configDataDictionary.Clear();
        }
        /// <summary>
        /// 把表單用list的方式先讀取進來
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="path">目標表單的名稱 會根據enum的Description 去抓資料</param>
        public static void LoadConfigData<TData>(CONFIG_PATH path) where TData : new()
        {
            if (!configDataDictionary.ContainsKey(path))
            {
                List<TData> tempList = new List<TData>();

                tempList = GetDisposableDataList<TData>(path);
                configDataDictionary.Add(path, tempList);
            }
        }

        private static List<TData> GetDisposableDataList<TData>(CONFIG_PATH path) where TData : new()
        {
            ConfigTable configTable = config.getCfg(path);

            if (configTable == null)
            {
                // act.debug.PrintSystem.LogError("config is missing : " + path);
                UnityEngine.Debug.LogError("config is missing : " + path);
                return null;
            }

            List<TData> dataList = ConfigDataParser.ParseTable<TData>(configTable);

            config.releaseCfg(path);

            return dataList;
        }

        public static List<TData> GetDisposableDataList<TData>(string path) where TData : new()
        {
            ConfigTable configTable = new ConfigTable();
            if (!configTable.initByPath(path))
            {
                act.debug.PrintSystem.Log(string.Format("read config {0}  failed!", path));
            }

            if (configTable == null)
            {
                // act.debug.PrintSystem.LogError("config is missing : " + path);
                UnityEngine.Debug.LogError("config is missing : " + path);
                return null;
            }

            List<TData> dataList = ConfigDataParser.ParseTable<TData>(configTable);

            return dataList;
        }

        /// <summary>
        /// 讀取表單 並且回傳Dictionary格式
        /// </summary>
        /// <typeparam name="TKey">dictionary key的資料結構</typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="dictionary">回傳的dictionary</param>
        /// <param name="path">目標表單的名稱 會根據enum的Description 去抓資料</param>
        public static void GetDataDictionary<TKey, TData>(this Dictionary<TKey, TData> dictionary, CONFIG_PATH path)
            where TKey : new()
            where TData : new()
        {
            Dictionary<TKey, TData> tempList = new Dictionary<TKey, TData>();
            dictionary.Clear();

            if (configDataDictionary.ContainsKey(path))
            {
                tempList = configDataDictionary[path] as Dictionary<TKey, TData>;
            }

            if (!configDataDictionary.ContainsKey(path) || tempList == null)
            {
                ReleaseConfigData(path);
                tempList = GetDisposableDataDictionary<TKey, TData>(path);
                configDataDictionary.Add(path, tempList);

                //dictionary = configDataDictionary[path] as Dictionary<TKey, TData>;


            }
            if (tempList != null)
            {
                foreach (KeyValuePair<TKey, TData> kvp in tempList)
                {
                    dictionary.Add(kvp.Key, kvp.Value);
                }
            }
        }

        private static Dictionary<TKey, TData> GetDisposableDataDictionary<TKey, TData>(CONFIG_PATH path)
            where TKey : new()
            where TData : new()
        {
            ConfigTable configTable = config.getCfg(path);

            Dictionary<TKey, TData> dataDictionary = ConfigDataParser.ParseTable<TKey, TData>(configTable);

            config.releaseCfg(path);

            return dataDictionary;
        }
        /// <summary>
        /// 讀取表單 並且回傳Dictionary格式
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="keyName">傳入key的Field name用來尋找key是哪筆資料</param>
        /// <param name="path">目標表單的名稱 會根據enum的Description 去抓資料</param>
        /// <param name="onAddToDictCallback"></param>
        public static void GetDataDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string keyName, data.CONFIG_PATH path, System.Action<TValue> onAddToDictCallback = null)
            where TValue : new()
        {
            dictionary.Clear();

            List<TValue> dataList = new List<TValue>();
            dataList.GetDataList(path);

            if (dataList != null)
            {
                System.Type t = typeof(TValue);
                foreach (TValue valueData in dataList)
                {
                    TKey key = (TKey)t.GetField(keyName).GetValue(valueData);
                    if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                        continue;

                    dictionary.Add(key, valueData);
                    if (onAddToDictCallback != null)
                        onAddToDictCallback(valueData);
                }
            }
        }

        public static void GetDataDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string keyName, string path, System.Action<TValue> onAddToDictCallback = null)
            where TValue : new()
        {
            dictionary.Clear();

            List<TValue> dataList = new List<TValue>();
            dataList = GetDisposableDataList<TValue>(path);

            if (dataList != null)
            {
                System.Type t = typeof(TValue);
                foreach (TValue valueData in dataList)
                {
                    TKey key = (TKey)t.GetField(keyName).GetValue(valueData);
                    if (key.GetType() != typeof(int))
                    {
                        if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                            continue;
                    }

                    dictionary.Add(key, valueData);
                    if (onAddToDictCallback != null)
                        onAddToDictCallback(valueData);
                }
            }
        }

        public static void GetDataDictionary<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, string keyName, data.CONFIG_PATH path, System.Action<TValue> onAddToDictCallback = null)
                where TValue : new()
        {
            dictionary.Clear();

            List<TValue> dataList = new List<TValue>();
            dataList.GetDataList(path);
            if (dataList != null)
            {
                System.Type t = typeof(TValue);
                foreach (TValue valueData in dataList)
                {
                    TKey key = (TKey)t.GetField(keyName).GetValue(valueData);
                    if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                        continue;

                    List<TValue> listValue;
                    if (!dictionary.TryGetValue(key, out listValue))
                    {
                        listValue = new List<TValue>();
                        dictionary.Add(key, listValue);
                    }

                    listValue.Add(valueData);

                    if (onAddToDictCallback != null)
                        onAddToDictCallback(valueData);
                }
            }

        }
    }
}

