using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Editor = UnityEditor.Editor;

[CanEditMultipleObjects]
[CustomEditor(typeof(RectTransform), true)]
public class RectTransformInspector : Editor
{
    static Type s_InspectorType;
    static MethodInfo s_OnDisableMethod;
    static MethodInfo s_OnEnableMethod;
    static MethodInfo s_OnSceneGUIMethod;
    static FieldInfo s_ShowLayoutOptionsField;

    Editor m_InspectorInstance;
    Action m_Inspecotr_OnDisable;
    Action m_Inspecotr_OnEnable;
    Action m_Inspecotr_OnSceneGUI;

    static RectTransformInspector()
    {
        var type = typeof(Editor).Assembly.GetType("UnityEditor.RectTransformEditor");
        s_InspectorType = type;

        s_OnDisableMethod = type.GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance);
        s_OnEnableMethod = type.GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
        s_OnSceneGUIMethod = type.GetMethod("OnSceneGUI", BindingFlags.NonPublic | BindingFlags.Instance);
        s_ShowLayoutOptionsField = type.GetField("m_ShowLayoutOptions", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    void OnEnable()
    {
        m_InspectorInstance = CreateEditor(targets, s_InspectorType);
        m_Inspecotr_OnDisable = (Action)Delegate.CreateDelegate(typeof(Action), m_InspectorInstance, s_OnDisableMethod, true);
        m_Inspecotr_OnEnable = (Action)Delegate.CreateDelegate(typeof(Action), m_InspectorInstance, s_OnEnableMethod, true);
        m_Inspecotr_OnSceneGUI = (Action)Delegate.CreateDelegate(typeof(Action), m_InspectorInstance, s_OnSceneGUIMethod, true);

        m_Inspecotr_OnEnable?.Invoke();
    }

    void OnDisable()
    {
        m_Inspecotr_OnDisable?.Invoke();

        if (m_InspectorInstance)
        {
            UnityEngine.Object.DestroyImmediate(m_InspectorInstance);
            m_InspectorInstance = null;
        }
    }

    void OnSceneGUI()
    {
        m_Inspecotr_OnSceneGUI?.Invoke();
    }

    public override void OnInspectorGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            using (new EditorGUI.IndentLevelScope(1))
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    m_InspectorInstance?.OnInspectorGUI();

                    var indentPerLevel = 15F;
                    var lastRect = GUILayoutUtility.GetLastRect();
                    var singleLineHeight = EditorGUIUtility.singleLineHeight;

                    lastRect = DrawButton(lastRect, EditorGUI.indentLevel * indentPerLevel, 0, singleLineHeight, "S", x => x.localScale = Vector3.one);
                    lastRect = DrawButton(lastRect, EditorGUI.indentLevel * indentPerLevel, singleLineHeight + 2, singleLineHeight, "R", x => x.localEulerAngles = Vector3.zero);
                    lastRect = DrawButton(lastRect, EditorGUI.indentLevel * indentPerLevel, singleLineHeight + 10, singleLineHeight, "P", x => x.pivot = Vector3.one * 0.5F);

                    var showLayoutOptions = (bool)(s_ShowLayoutOptionsField?.GetValue(m_InspectorInstance) ?? false);
                    if (showLayoutOptions)
                    {
                        lastRect = DrawButton(lastRect, (EditorGUI.indentLevel + 1) * indentPerLevel, singleLineHeight + 4, singleLineHeight, "a", x => x.anchorMax = Vector3.one * 0.5F);
                        lastRect = DrawButton(lastRect, (EditorGUI.indentLevel + 1) * indentPerLevel, singleLineHeight + 2, singleLineHeight, "i", x => x.anchorMin = Vector3.one * 0.5F);
                    }

                    lastRect = DrawButton(lastRect, EditorGUIUtility.labelWidth, singleLineHeight + 20, singleLineHeight, "S", x => x.sizeDelta = new Vector2(100, 100));
                    lastRect = DrawButton(lastRect, EditorGUIUtility.labelWidth, singleLineHeight + 18, singleLineHeight, "P", x => x.anchoredPosition3D = Vector3.zero);
                }
            }
        }
    }

    Rect DrawButton(Rect previousRect, float x, float offsetY, float height, string name, Action<RectTransform> callback)
    {
        var rect = previousRect;
        rect.x = x;
        rect.y -= offsetY;
        rect.width = rect.height = height;
        if (GUI.Button(rect, name))
        {
            Undo.RecordObjects(targets, "Inspector");
            foreach (RectTransform item in targets)
            {
                callback(item);
            }
        }

        return rect;
    }
}