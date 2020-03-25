using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace act.data
{
    /// <summary>
    /// 使用BinaryFormatter來做序列化/反序列化，目標類跟繼承的基類都要加上[Serializable]。
    /// NOTE: 暫時沒加上壓縮跟加密。
    /// </summary>
    public static class DataArchiver
    {
        public static void Save(object obj, string fileName)
        {
            string filePath = Application.persistentDataPath + "/" + fileName;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using(MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, obj);
                    File.WriteAllBytes(filePath, ms.ToArray()); // NOTE: 檔案存在的話會直接覆寫
                }
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                //TODO:debug
                //debug.PrintSystem.LogError(e.Message);
            }
        }

        public static T Load<T>(string fileName) where T : class
        {
            string filePath = Application.persistentDataPath + "/" + fileName;
            T obj = null;
            try
            {
                byte[] serializedData = File.ReadAllBytes(filePath);
                BinaryFormatter formatter = new BinaryFormatter();
                using(MemoryStream ms = new MemoryStream(serializedData))
                {
                    obj = (T)formatter.Deserialize(ms);
                }
            }
            catch(Exception e)
            {
                debug.PrintSystem.LogWarning(e.Message);
            }
            return obj;
        }
    }

    public class DataArchiverMgr : Singleton<DataArchiverMgr>
    {
        Dictionary<int, object> serializeData = new Dictionary<int, object>();

        protected override void init()
        {
            base.init();
        }

        T load<T>() where T : class
        {
            string fileName = getTypeName(typeof(T));
            if(!string.IsNullOrEmpty(fileName))
            {
                T data = DataArchiver.Load<T>(fileName);
                return data;
            }
            return null;
        }

        public void Save<T>(T newData)
        {
            Type t = typeof(T);
            DataArchiver.Save(newData, getTypeName(t));
        }

        public T Load<T>() where T : class
        {
            Type t = typeof(T);
            string fileName = getTypeName(t);
            int hash = fileName.GetHashCode();
            object obj = null;
            if(serializeData.TryGetValue(hash, out obj))
            {
                return (T)obj;
            }
            obj = load<T>();
            if(obj != null)
            {
                serializeData.Add(hash, obj);
                return (T)obj;
            }
            return null;
        }

        public bool Delete<T>() where T : class
        {
            Type t = typeof(T);
            string fileName = getTypeName(t);
            int hash = fileName.GetHashCode();
            object obj = null;
            if(serializeData.TryGetValue(hash, out obj))
            {
                serializeData.Remove(hash);
            }
            obj = load<T>();
            if(obj != null)
            {
                string filePath = Application.persistentDataPath + "/" + fileName;
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        string getTypeName(Type t)
        {
            string typeName = t.Name;
            if(t.IsGenericType)
            {
                Type genericType = t.GetGenericTypeDefinition();
                if(genericType == typeof(List<>) || genericType == typeof(IList))
                {
                    typeName = t.GetGenericArguments()[0].Name;
                }
                else if(genericType == typeof(IDictionary) || genericType == typeof(Dictionary<,>))
                {
                    Type[] arguments = t.GetGenericArguments();
                    Type keyType = arguments[0];
                    Type valueType = arguments[1];
                    typeName = keyType.Name + "_" + valueType.Name;
                }
            }
            return typeName;
        }
    }
}