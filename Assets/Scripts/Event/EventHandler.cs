using System;
using System.Collections.Generic;

namespace act.evt
{
    // Note: 事件的簽章會以第一個註冊的函式為準。
    // Note: 如果Delegate被清空之後註冊一個不同簽章的函式，不會導致exception，但會使原來發送的事件接收不到。
    public class EventHandler
    {
        private Dictionary<int, Delegate> callbackDict = new Dictionary<int, Delegate>(521);

        public void Register(int id, Action callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del == null)
            {
                callbackDict[id] = callback;
                return;
            }

            if (del is Action callbacks)
            {
                callbackDict[id] = callbacks + callback;
            }
            else
            {
                debug.PrintSystem.LogError("[EventHandler] Cannot register different types of callback functions in the same Event ID");
            }
        }

        public void Unregister(int id, Action callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del is Action callbacks)
            {
                callbackDict[id] = callbacks - callback;
            }
        }

        public void Send(int id)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            (del as Action)?.Invoke();
        }

        public void Register<T>(int id, Action<T> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del == null)
            {
                callbackDict[id] = callback;
                return;
            }

            if (del is Action<T> callbacks)
            {
                callbackDict[id] = callbacks + callback;
            }
            else
            {
                debug.PrintSystem.LogError("[EventHandler] Cannot register different types of callback functions in the same Event ID");
            }
        }

        public void Unregister<T>(int id, Action<T> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del is Action<T> callbacks)
            {
                callbackDict[id] = callbacks - callback;
            }
        }

        public void Send<T>(int id, T arg)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            (del as Action<T>)?.Invoke(arg);
        }

        public void Register<T1, T2>(int id, Action<T1, T2> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del == null)
            {
                callbackDict[id] = callback;
                return;
            }

            if (del is Action<T1, T2> callbacks)
            {
                callbackDict[id] = callbacks + callback;
            }
            else
            {
                debug.PrintSystem.LogError("[EventHandler] Cannot register different types of callback functions in the same Event ID");
            }
        }

        public void Unregister<T1, T2>(int id, Action<T1, T2> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del is Action<T1, T2> callbacks)
            {
                callbackDict[id] = callbacks - callback;
            }
        }

        public void Send<T1, T2>(int id, T1 arg1, T2 arg2)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            (del as Action<T1, T2>)?.Invoke(arg1, arg2);
        }

        public void Register<T1, T2, T3>(int id, Action<T1, T2, T3> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del == null)
            {
                callbackDict[id] = callback;
                return;
            }

            if (del is Action<T1, T2, T3> callbacks)
            {
                callbackDict[id] = callbacks + callback;
            }
            else
            {
                debug.PrintSystem.LogError("[EventHandler] Cannot register different types of callback functions in the same Event ID");
            }
        }

        public void Register<T1, T2, T3,T4>(int id, Action<T1, T2, T3,T4> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del == null)
            {
                callbackDict[id] = callback;
                return;
            }

            if (del is Action<T1, T2, T3,T4> callbacks)
            {
                callbackDict[id] = callbacks + callback;
            }
            else
            {
                debug.PrintSystem.LogError("[EventHandler] Cannot register different types of callback functions in the same Event ID");
            }
        }

        public void Register<T1, T2, T3, T4, T5>(int id, Action<T1, T2, T3, T4, T5> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del == null)
            {
                callbackDict[id] = callback;
                return;
            }

            if (del is Action<T1, T2, T3, T4, T5> callbacks)
            {
                callbackDict[id] = callbacks + callback;
            }
            else
            {
                debug.PrintSystem.LogError("[EventHandler] Cannot register different types of callback functions in the same Event ID");
            }
        }

        public void Unregister<T1, T2, T3>(int id, Action<T1, T2, T3> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del is Action<T1, T2, T3> callbacks)
            {
                callbackDict[id] = callbacks - callback;
            }
        }

        public void Unregister<T1, T2, T3,T4>(int id, Action<T1, T2, T3,T4> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del is Action<T1, T2, T3,T4> callbacks)
            {
                callbackDict[id] = callbacks - callback;
            }
        }

        public void Unregister<T1, T2, T3, T4,T5>(int id, Action<T1, T2, T3, T4,T5> callback)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            if (del is Action<T1, T2, T3, T4, T5> callbacks)
            {
                callbackDict[id] = callbacks - callback;
            }
        }


        public void Send<T1, T2, T3>(int id, T1 arg1, T2 arg2, T3 arg3)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            (del as Action<T1, T2, T3>)?.Invoke(arg1, arg2, arg3);
        }

        public void Send<T1, T2, T3,T4>(int id, T1 arg1, T2 arg2, T3 arg3,T4 arg4)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            (del as Action<T1, T2, T3,T4>)?.Invoke(arg1, arg2, arg3,arg4);
        }

        public void Send<T1, T2, T3, T4, T5>(int id, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            callbackDict.TryGetValue(id, out Delegate del);
            (del as Action<T1, T2, T3, T4, T5>)?.Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        public void Clear()
        {
            callbackDict.Clear();
        }
    }
}