using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>  </summary>
public class testAB : MonoBehaviour
{
    public Renderer cube;
    void Start()
    {
        StartCoroutine(LoadUITexture());
        StartCoroutine(LoadCube());
        //AssetBundle.LoadFromFileAsync(path);
    }

    private IEnumerator LoadUITexture()
    {
        //var url = $"file:\\{Application.streamingAssetsPath}/pic.unity3d";
        var url = $"{Application.streamingAssetsPath}/pic.unity3d";
        var www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();
        if (www.isDone && www.isNetworkError == false)
        {
            var ab = DownloadHandlerAssetBundle.GetContent(www);
            if (ab != null)
            {
                cube.material.mainTexture = (Texture)ab.LoadAsset(ab.GetAllAssetNames()[0]);
            }
            ab.Unload(false);
        }
    }

    private IEnumerator LoadCube()
    {
        var url = $"{Application.streamingAssetsPath}/cube.unity3d";
        var www = AssetBundle.LoadFromFileAsync(url);
        yield return www;
        AssetBundle ab = www.assetBundle;
        // 使用里面的资源
        var obj = ab.LoadAllAssets<GameObject>();//加载出来放入数组中
         //创建出来
        foreach (var o in obj)
        {
            Instantiate(o);
        }
    }


    //private IEnumerator LoadCube()
    //{
    //    var url = $"file:\\{Application.streamingAssetsPath}/cube.unity3d";
    //    var www = UnityWebRequestAssetBundle.GetAssetBundle(url);
    //    yield return www.SendWebRequest();
    //    if (www.isDone && www.isNetworkError == false)
    //    {
    //        var ab = DownloadHandlerAssetBundle.GetContent(www);
    //        if (ab != null)
    //        {
    //            var clone = Instantiate((GameObject)ab.LoadAsset(ab.GetAllAssetNames()[0]));
    //            clone.transform.localPosition = Vector3.zero;
    //        }
    //        ab.Unload(true);
    //    }
    //}



    void Update()
    {

    }
}
