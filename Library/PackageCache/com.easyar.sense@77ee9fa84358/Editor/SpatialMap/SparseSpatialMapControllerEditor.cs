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
    [CustomEditor(typeof(SparseSpatialMapController), true)]
    class SparseSpatialMapControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var controller = (SparseSpatialMapController)target;

            var sourceType = serializedObject.FindProperty("sourceType");
            EditorGUILayout.PropertyField(sourceType, new GUIContent("Source"), true);
            serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel += 1;
            switch ((SparseSpatialMapController.DataSource)sourceType.enumValueIndex)
            {
                case SparseSpatialMapController.DataSource.MapManager:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("mapManagerSource.ID"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("mapManagerSource.Name"), true);
                    break;
                default:
                    break;
            }
            EditorGUI.indentLevel -= 1;

            var tracker = serializedObject.FindProperty("tracker");
            EditorGUILayout.PropertyField(tracker, new GUIContent("Tracker"), true);

            var trackerHasSet = serializedObject.FindProperty("trackerHasSet");
            if (!trackerHasSet.boolValue)
            {
                if (!tracker.objectReferenceValue)
                {
                    tracker.objectReferenceValue = ObjectUtil.FindFirstObjectByType<SparseSpatialMapTrackerFrameFilter>();
                }
                if (tracker.objectReferenceValue)
                {
                    trackerHasSet.boolValue = true;
                }
            }
            serializedObject.ApplyModifiedProperties();
            controller.Tracker = (SparseSpatialMapTrackerFrameFilter)tracker.objectReferenceValue;
        }
    }
}
