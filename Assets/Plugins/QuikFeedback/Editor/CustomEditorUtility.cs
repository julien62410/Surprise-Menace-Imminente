using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomEditorUtility
{
    public static SerializedProperty QuickSerializeRelative(string name, SerializedProperty parent)
    {
        SerializedProperty p = parent.FindPropertyRelative(name);
        EditorGUILayout.PropertyField(p, true);
        return p;
    }

    public static SerializedProperty QuickSerializeObject(string name, SerializedObject obj)
    {
        SerializedProperty p = obj.FindProperty(name);
        EditorGUILayout.PropertyField(p, true);
        return p;
    }

    public static void DrawTitle(string name)
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        /*
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        */

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 20;
        titleStyle.alignment = TextAnchor.MiddleCenter;
#if UNITY_PRO_LICENSE
        titleStyle.normal.textColor = Color.white;
#endif
        EditorGUILayout.LabelField(name, titleStyle);
        /*
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        */

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    public static void CustomMinMaxSlider(ref float min, ref float max, float minLimit, float maxLimit, string name, Object record)
    {
        //EditorGUILayout.BeginVertical("box");

        Undo.RecordObject(record, "Slider");
        EditorGUILayout.LabelField(name + ":");
        EditorGUILayout.BeginHorizontal();
        min = EditorGUILayout.FloatField(min, GUILayout.MaxWidth(80));
        GUILayout.FlexibleSpace();
        max = EditorGUILayout.FloatField(max, GUILayout.MaxWidth(80));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);

        //EditorGUILayout.EndVertical();
    }
}
