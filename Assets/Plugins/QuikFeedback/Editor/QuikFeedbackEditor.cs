using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pataya.QuikFeedback
{
    [CustomEditor(typeof(QuikFeedback))]
    public class QuikFeedbackEditor : Editor
    {
        private QuikFeedback f;

        private void OnEnable()
        {
            f = target as QuikFeedback;
        }

        public override void OnInspectorGUI()
        {
            CustomEditorUtility.DrawTitle("Quik Feedback");

            DrawGeneralSettings();
            EditorGUILayout.Space();
            DrawFeedbacks();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGeneralSettings()
        {
            EditorGUILayout.BeginVertical("box");

            CustomEditorUtility.QuickSerializeObject("feedbackName", serializedObject);
            CustomEditorUtility.QuickSerializeObject("enableDebug", serializedObject);
            if (f.enableDebug)
            {
                CustomEditorUtility.QuickSerializeObject("debugKey", serializedObject);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawFeedbacks()
        {
            DrawPropertyCondition("useShake", "shakeFeedback", f.useShake);
            DrawPropertyCondition("useZoom", "zoomFeedback", f.useZoom);
            DrawPropertyCondition("usePostProcess", "postProcessFeedback", f.usePostProcess);
            DrawPropertyCondition("useParticleSystem", "particleFeedback", f.useParticleSystem);
            DrawPropertyCondition("useAnimation", "animationFeedback", f.useAnimation);
            DrawPropertyCondition("useMaterial", "materialFeedback", f.useMaterial);
            DrawPropertyCondition("useBounce", "bounceFeedback", f.useBounce);
            DrawPropertyCondition("useFreezeFrame", "freezeFrameFeedback", f.useFreezeFrame);
        }

        private void DrawPropertyCondition(string conditionName, string name, bool cond)
        {
            EditorGUILayout.BeginVertical("box");
            SerializedProperty property = serializedObject.FindProperty(name);
            SerializedProperty condition = serializedObject.FindProperty(conditionName);

            EditorGUILayout.PropertyField(condition, true);

            if (cond)
            {
                EditorGUILayout.PropertyField(property, true);
            }

            EditorGUILayout.EndVertical();
        }

        [MenuItem("Tools/Initialize Quik Feedback")]
        public static void CreateFeedbackManager()
        {
            //test
            GameObject go = new GameObject("QuikFeedbackManager");
            go.AddComponent<QuikFeedbackManager>();
            Selection.activeTransform = go.transform;
            EditorGUIUtility.PingObject(go);
        }

        [MenuItem("GameObject/Effects/QuikFeedback")]
        public static QuikFeedback CreateFeedback()
        {
            GameObject go = new GameObject();
            QuikFeedback fb = go.AddComponent<QuikFeedback>();
            Selection.activeTransform = go.transform;
            EditorGUIUtility.PingObject(go);
            return fb;
        }
    }
}
