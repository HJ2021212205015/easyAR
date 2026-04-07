//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEditor;

namespace easyar
{
    [CustomEditor(typeof(FrameRecorder), true)]
    class FrameRecorderEditor : Editor
    {
        private bool showEvents;

        public override void OnInspectorGUI()
        {
            var recorder = target as FrameRecorder;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(recorder.AutoStart)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(recorder.Configuration)}.{nameof(recorder.Configuration.Format)}"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(recorder.Configuration)}.{nameof(recorder.Configuration.AutoFilePath)}"));
            serializedObject.ApplyModifiedProperties();
            if (!recorder.Configuration.AutoFilePath)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(recorder.Configuration)}.{nameof(recorder.Configuration.FilePath)}.{nameof(recorder.Configuration.FilePath.Type)}"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(recorder.Configuration)}.{nameof(recorder.Configuration.FilePath)}.{nameof(recorder.Configuration.FilePath.FolderPath)}"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(recorder.Configuration)}.{nameof(recorder.Configuration.FilePath)}.{nameof(recorder.Configuration.FilePath.FileName)}"));
                EditorGUI.indentLevel -= 1;
            }
            showEvents = EditorGUILayout.Foldout(showEvents, "Events");
            if (showEvents)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(recorder.OnReady)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(recorder.OnRecording)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(recorder.OnFinish)));
                EditorGUI.indentLevel -= 1;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
