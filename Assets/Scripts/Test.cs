using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("双击Log时,定位在到此,且用Notepad++打开了文本内容", GUILayout.Height(40)))
        {
            ZLog.Log("我的Log");
        }
        if (GUILayout.Button("方法使用了 #if UNITY_EDITOR", GUILayout.Height(40)))
        {
            ZLog.LogError("发布时 就没有log了");
        }
    }
}
