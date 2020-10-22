using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZEditorAssetBundle
{
    [MenuItem("Assets/标记AB包 %#&B", false, 80)]
    public static void ShowRightMouse()
    {
        var tSelects = Selection.objects;
        if (tSelects.Length > 0)
        {
            for (int i = 0; i < tSelects.Length; i++)
            {
                var tSelect = tSelects[i];
                var tPath = AssetDatabase.GetAssetPath(tSelect);
                var tAsset = AssetImporter.GetAtPath(tPath);
                tAsset.assetBundleName = tSelect.name + ".unity3d"; //设置Bundle文件的名称    
                tAsset.SaveAndReimport();
            }
        }
        else
        {
            Debug.Log("非 GameObject");
        }
        AssetDatabase.Refresh();//刷新        
    }
}
