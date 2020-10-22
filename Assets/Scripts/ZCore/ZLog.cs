using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ZLog
{
    public static void Log(object obj)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.Log("[LogInfo]:" + obj);
#endif
    }
    public static void LogError(object obj)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogError("[LogInfo]:" + obj);
#endif
    }
    public static void LogWarning(object obj)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogWarning("[LogInfo]:" + obj);
#endif
    }
}

//利用反射去取值
//内置函数 OnOpenAsset
//打开内容        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(strPath, line);
//正则 matches.NextMatch();  // 向上再提高一层 做进入;
public class DebugCodeLocation
{
#if UNITY_EDITOR

    // 处理asset打开的callback函数
    [UnityEditor.Callbacks.OnOpenAsset(0)]
    static bool OnOpenAsset(int instance, int line)
    {
        string stackStr = GetStackTraceStr();
        if (!string.IsNullOrEmpty(stackStr)) // 可以自定义标签 在这里添加;原有代码混乱不做修改,需要自己定位;
        {
            string strLower = stackStr.ToLower();
            if (strLower.Contains("[loginfo]"))//小写的
            {
                Match matches = Regex.Match(stackStr, @"\(at(.+)\)", RegexOptions.IgnoreCase);
                string pathline = "";
                if (matches.Success)
                {
                    pathline = matches.Groups[1].Value;
                    matches = matches.NextMatch();  // 向上再提高一层 做进入;
                    if (matches.Success)
                    {
                        pathline = matches.Groups[1].Value;
                        pathline = pathline.Replace(" ", "");

                        int split_index = pathline.LastIndexOf(":");
                        string path = pathline.Substring(0, split_index);
                        line = Convert.ToInt32(pathline.Substring(split_index + 1));
                        string fullpath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                        fullpath = fullpath + path;
                        string strPath = fullpath.Replace('/', '\\');
                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(strPath, line);

                        var pathTxt = Application.dataPath.Replace("Assets", "Library/Log");
                        if (Directory.Exists(pathTxt) == false)
                        {
                            Directory.CreateDirectory(pathTxt);
                        }
                        var fullTxt = $"{Application.dataPath.Replace("Assets", "Library/Log/")}{DateTime.Now.ToString("HHmmss_ff")}.txt";
                        var fs = new FileStream(fullTxt, FileMode.Create);
                        var fs2 = new StreamWriter(fs);
                        fs2.Write(stackStr);
                        fs2.Close();
                        fs.Close();
                        System.Diagnostics.Process.Start("notepad++.exe", fullTxt);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("DebugCodeLocation OnOpenAsset, Error StackTrace");
                    }
                    matches = matches.NextMatch();
                }
                return true;
            }
        }
        return false;
    }

    //看https://github.com/Unity-Technologies/UnityCsReference.git 里的ConsoleWindow.cs 里面的字段,单例名 利用反射去取值
    private static string GetStackTraceStr()
    {
        var consoleWindow = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        var ms_Console = consoleWindow.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);//单例

#pragma warning disable
        if (ms_Console != null && UnityEditor.EditorWindow.focusedWindow == ms_Console)
        {
            var text = consoleWindow.GetField("m_ActiveText", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            var t = text.GetValue(ms_Console).ToString();
            return t;
        }
#pragma warning restore
        return null;
    }


#endif

}



