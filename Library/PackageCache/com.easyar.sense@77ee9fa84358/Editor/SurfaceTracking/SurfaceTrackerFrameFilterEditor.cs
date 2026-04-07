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
    [CustomEditor(typeof(SurfaceTrackerFrameFilter), true)]
    class SurfaceTrackerFrameFilterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var tracker = target as SurfaceTrackerFrameFilter;
            var targetP = serializedObject.FindProperty("target");
            EditorGUILayout.PropertyField(targetP, true);
            if (Application.isPlaying && targetP.objectReferenceValue != tracker.Target)
            {
                tracker.Target = (SurfaceTargetController)targetP.objectReferenceValue;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
