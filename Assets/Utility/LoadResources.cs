using UnityEngine;

namespace act.utility
{
    public static class LoadResources
    {
        public static GameObject LoadPrefab(string path, bool checkExisted = false)
        {
            Object obj = Resources.Load(path) as GameObject;
            if (checkExisted)
            {
                if (obj == null)
                {
                    return null;
                }
            }
            return InstantiateObject(obj);
        }
        public static GameObject LoadPrefab(string path, Transform parent, bool checkExisted = false)
        {
            Object obj = Resources.Load(path) as GameObject;
            if (checkExisted)
            {
                if (obj == null)
                {
                    return null;
                }
            }
            return InstantiateObject(obj, parent);
        }
        public static GameObject InstantiateObject(Object obj)
        {
            return Object.Instantiate(obj) as GameObject;
        }
        public static GameObject InstantiateObject(Object obj, Transform parent)
        {
            GameObject go = InstantiateObject(obj);
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            return go;
        }

        public static T LoadAsset<T>(string path) where T : Object
        {
            Object obj = Resources.Load(path, typeof(T));
            return obj as T;
        }

        //TODO: ?会导致在Object.Instantiate时卡1-2秒？？？
        public static ResourceRequest LoadAssetAsync(string path)
        {
            return Resources.LoadAsync(path);
        }
    }
}