using System;
using UnityEngine;

namespace act.debug
{
    static public class PrintSystem
    {
        public static readonly bool showParameters = true;
        private static System.Reflection.MethodBase unityLog;

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void Log(string str)
        {
            Debuger.Log(str);
        }

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void Log(string str, Color color)
        {
            string colorString = ColorUtility.ToHtmlStringRGB(color);
            str = String.Format("<color=#{0}> {1} </color>", colorString, str);
            Debuger.Log(str);
        }

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void Log(string str, string color)
        {
            str = String.Format("<color=#{0}> {1} </color>", color, str);
            Debuger.Log(str);
        }

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void LogWarning(string str)
        {
            Debuger.LogWarning(str);
        }

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void LogError(string str)
        {
            Debuger.LogError(str);
        }

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void LogFormat(string str, params object[] args)
        {
            Debuger.Log(string.Format(str, args));
        }

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void Assert(bool condition, string str)
        {
            Debug.Assert(condition, str);
        }


        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void Assert(bool condition)
        {
            Debug.Assert(condition);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("IGG_DEBUG")]
        static public void CustomAssert(bool condition, string assertString, bool pauseOnFail = false)
        {
            if (!condition)
            {
                System.Text.StringBuilder message = new System.Text.StringBuilder();
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
                System.Diagnostics.StackFrame[] stackFrames = stackTrace.GetFrames();
                int line = 0;
                string file = "";
                int col = 0;
                bool foundStart = false;

                //message.Append("<color=white>");
                message.Append(assertString);
                //message.Append("</color>");
                message.Append("\n");

                for (int i = 0; i < stackFrames.Length; i++)
                {
                    System.Reflection.MethodBase mb = stackFrames[i].GetMethod();
                    if (!foundStart && mb.DeclaringType != typeof(PrintSystem))
                    {
                        file = formatFileName(stackFrames[i].GetFileName());
                        line = stackFrames[i].GetFileLineNumber();
                        col = stackFrames[i].GetFileColumnNumber();
                        foundStart = true;
                    }

                    if (foundStart)
                    {
                        //message.Append(mb.DeclaringType.FullName);
                        message.Append(mb.DeclaringType.Namespace);
                        message.Append(".");
                        message.Append(mb.DeclaringType.Name);
                        message.Append(":");
                        message.Append(mb.Name);
                        message.Append("(");
                        if (showParameters)
                        {
                            System.Reflection.ParameterInfo[] paramters = mb.GetParameters();
                            for (int k = 0; k < paramters.Length; k++)
                            {
                                message.Append(paramters[k].ParameterType.Name);
                                if (k + 1 < paramters.Length)
                                    message.Append(", ");
                            }
                        }

                        message.Append(")");

                        message.Append(" (at ");

                        message.Append(formatFileName(stackFrames[i].GetFileName()));
                        message.Append(":");
                        message.Append(stackFrames[i].GetFileLineNumber());
                        message.Append(")");
                        message.Append("\n");
                    }

                }
                unityLog = typeof(UnityEngine.Debug).GetMethod("LogPlayerBuildError",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                unityLog.Invoke(null, new object[] { message.ToString(), file, line, col });

                if (pauseOnFail)
                    Debug.Break();
            }
        }

        private static string formatFileName(string file)
        {
            if (file == null) return string.Empty;
            int len = Application.dataPath.Length - "Assets".Length;

            if (len <= 0)
                return file;
            else if (file.Length < len)
                return file;
            else
                return file.Remove(0, len);
        }

#if UNITY_EDITOR
        [UnityEditor.Callbacks.OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            UnityEngine.Object obj = UnityEditor.EditorUtility.InstanceIDToObject(instanceId);
            if (obj.name != typeof(PrintSystem).Name)
            {
                return false;
            }

            string assetPath = null;
            string consoleLog = GetConsoleLog();
            GetFileInfo(consoleLog, out assetPath, out line);
            if (string.IsNullOrEmpty(assetPath) || line == -1)
            {
                return false;
            }

            obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            UnityEditor.AssetDatabase.OpenAsset(obj, line);
            return true;
        }

        private static string GetConsoleLog()
        {
            const string ConsoleWindowClass = "UnityEditor.ConsoleWindow";
            const string ConsoleWindowField = "ms_ConsoleWindow";
            const System.Reflection.BindingFlags ConsoleWindowBindingFlag = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
            const string ActiveTextField = "m_ActiveText";
            const System.Reflection.BindingFlags ActiveTextBindingFlag = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;

            Type consoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType(ConsoleWindowClass);
            System.Reflection.FieldInfo consoleWindowfield = consoleWindowType.GetField(ConsoleWindowField, ConsoleWindowBindingFlag);
            object consoleWindowInst = consoleWindowfield.GetValue(null);

            string consoleLog = string.Empty;
            if (consoleWindowInst == null)
            {
                return consoleLog;
            }

            if ((System.Object)consoleWindowInst != UnityEditor.EditorWindow.focusedWindow)
            {
                return consoleLog;
            }

            System.Reflection.FieldInfo activeTextfield = consoleWindowType.GetField(ActiveTextField, ActiveTextBindingFlag);
            consoleLog = activeTextfield.GetValue(consoleWindowInst).ToString();
            return consoleLog;
        }

        private static void GetFileInfo(string consoleLog, out string filePath, out int fileLine)
        {
            const string UnityLogTag = "debug.PrintSystem:";
            const string LogStartTag = "(at ";
            const string LogEndTag = ".cs";

            string[] context = consoleLog.Split('\n');
            for (int i = context.Length - 1; i >= 0; i--)
            {
                if (context[i].Contains(UnityLogTag))
                {
                    consoleLog = context[i + 1];
                    break;
                }
            }

            int startIndex = consoleLog.LastIndexOf(LogStartTag);
            int endIndex = consoleLog.LastIndexOf(LogEndTag);
            if (startIndex == -1 || endIndex == -1)
            {
                filePath = null;
                fileLine = -1;
                return;
            }

            startIndex += LogStartTag.Length;
            endIndex += LogEndTag.Length;
            filePath = consoleLog.Substring(startIndex, endIndex - startIndex);

            consoleLog = consoleLog.Substring(++endIndex, consoleLog.Length - endIndex - 1);
            fileLine = int.Parse(consoleLog);
        }
#endif
    }
}
