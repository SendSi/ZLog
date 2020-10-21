using UnityEditor;
using UnityEngine;

public class F12_CtrlD : EditorWindow
{
    [MenuItem("GameTools/小工具/刷新并运行 _F12")]
    public static void ShowPlayF12()
    {
        AssetDatabase.Refresh();
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    [MenuItem("GameTools/小工具/清空playerPrefs %&c")]
    public static void ClearPrefab()
    {
        Debug.Log("清空playerPrefs了");
        PlayerPrefs.DeleteAll();
    }
    //根据当前有没有选中物体来判断可否用快捷键
    [MenuItem("GameTools/小工具/选中某物 Active(c.s_d) %#d", true)]
    public static bool ValidateSelectEnableDisable()
    {
        GameObject[] go = GetSelectedGameObjects() as GameObject[];

        if (go == null || go.Length == 0)
            return false;
        return true;
    }

    [MenuItem("GameTools/小工具/选中某物Active %#d")]
    static void SeletEnable()
    {
        bool enable = false;
        GameObject[] gos = GetSelectedGameObjects() as GameObject[];

        foreach (GameObject go in gos)
        {
            enable = !go.activeInHierarchy;
            EnableGameObject(go, enable);
        }
    }

    //获得选中的物体
    static GameObject[] GetSelectedGameObjects()
    {
        return Selection.gameObjects;
    }

    //激活或关闭当前选中物体
    public static void EnableGameObject(GameObject parent, bool enable)
    {
        parent.gameObject.SetActive(enable);
    }
}
