using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pataya.QuikFeedback
{
    /*
    [CustomPropertyDrawer(typeof(ShakeTransform))]
    public class ShakeTransformDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight * 10;
            }
            else
            {
                return base.GetPropertyHeight(property, label);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty enabled = property.FindPropertyRelative("enabled");
            EditorGUI.PropertyField(position, enabled, label, true);

            SerializedProperty x = property.FindPropertyRelative("x");
            SerializedProperty y = property.FindPropertyRelative("y");
            SerializedProperty z = property.FindPropertyRelative("z");
            if(enabled.boolValue)
            {
                EditorGUI.PropertyField(position, x, label, true);
                EditorGUI.PropertyField(position, y, label, true);
                EditorGUI.PropertyField(position, z, label, true);
            }
        }
    }
    */
}