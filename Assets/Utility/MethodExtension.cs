using System;
using UnityEngine;

public static class MethodExtension
{
    static public T GetOrAddComponent<T>(this Component comp) where T : Component
    {
        T result = comp.GetComponent<T>();
        if (result == null)
        {
            result = comp.gameObject.AddComponent<T>();
        }
        return result;
    }

    static public T GetOrAddComponent<T>(this UnityEngine.GameObject go) where T : Component
    {
        T result = go.GetComponent<T>();
        if (result == null)
        {
            result = go.AddComponent<T>();
        }
        return result;
    }

    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.DeclaredOnly;
        System.Reflection.PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }

        System.Reflection.FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }
}
