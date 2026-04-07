//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEditor;
using UnityEngine;

namespace easyar
{
    [CustomEditor(typeof(CloudRecognizerFrameFilter), true)]
    class CloudRecognizerFrameFilterEditor : Editor
    {
        string token;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var filter = (CloudRecognizerFrameFilter)target;

            var accessSource = (CloudRecognizerFrameFilter.ServiceAccessSourceType)EditorGUILayout.EnumPopup("Service Access", filter.ServiceAccessSource);
            if (filter.ServiceAccessSource != accessSource)
            {
                filter.ServiceAccessSource = accessSource;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(filter);
            }

            EditorGUI.indentLevel += 1;
            if (accessSource == CloudRecognizerFrameFilter.ServiceAccessSourceType.APIKey)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.AppID"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.ServerAddress"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.APIKey"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.APISecret"), true);
            }
            else if (accessSource == CloudRecognizerFrameFilter.ServiceAccessSourceType.AppSecret)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("appSecretAccessData.AppID"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("appSecretAccessData.ServerAddress"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("appSecretAccessData.AppSecret"), true);
            }
            else if (accessSource == CloudRecognizerFrameFilter.ServiceAccessSourceType.Token)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tokenAccessData.AppID"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tokenAccessData.ServerAddress"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tokenAccessData.Token"), true);
            }
            EditorGUI.indentLevel -= 1;

            serializedObject.ApplyModifiedProperties();

            if (filter.ServiceAccessSource != CloudRecognizerFrameFilter.ServiceAccessSourceType.Token) { return; }

            EditorGUILayout.Space();
            using (_ = new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUILayout.Label("Test Area", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
                if (!Application.isPlaying)
                {
                    EditorGUILayout.HelpBox("Available in play mode", MessageType.Info);
                }

                if (!Application.isPlaying) { GUI.enabled = false; }

                if (filter.ServiceAccessSource == CloudRecognizerFrameFilter.ServiceAccessSourceType.Token)
                {
                    token = EditorGUILayout.TextField("Token", token);
                    if (GUILayout.Button("Update Token", GUILayout.Height(30)))
                    {
                        filter.UpdateToken(token);
                    }
                }

                if (!Application.isPlaying) { GUI.enabled = true; }
            }
        }
    }
}
