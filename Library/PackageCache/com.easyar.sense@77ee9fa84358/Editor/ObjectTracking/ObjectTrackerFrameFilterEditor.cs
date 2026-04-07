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
    [CustomEditor(typeof(ObjectTrackerFrameFilter), true)]
    class ObjectTrackerFrameFilterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var filter = (ObjectTrackerFrameFilter)target;
            var simultaneousNum = filter.SimultaneousNum;
            var enableMotionFusion = filter.EnableMotionFusion;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("simultaneousNum"), new GUIContent("Simultaneous Target Number"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enableMotionFusion"));

            serializedObject.ApplyModifiedProperties();

            if (Application.isPlaying)
            {
                if (simultaneousNum != filter.SimultaneousNum)
                {
                    filter.SimultaneousNum = filter.SimultaneousNum;
                }
                if (enableMotionFusion != filter.EnableMotionFusion)
                {
                    filter.EnableMotionFusion = filter.EnableMotionFusion;
                }
            }
        }
    }
}
