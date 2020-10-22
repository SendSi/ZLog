using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
/// <summary>  </summary>
public class BuildAssetBundle
{
    [MenuItem("BuildAssetBundle/BuildAB")]
    static void Start()
    {
        string path = Application.streamingAssetsPath;
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
