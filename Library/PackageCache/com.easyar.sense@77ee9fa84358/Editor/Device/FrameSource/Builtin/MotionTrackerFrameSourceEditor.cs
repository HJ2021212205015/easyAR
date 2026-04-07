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
    [CustomEditor(typeof(MotionTrackerFrameSource), true)]
    class MotionTrackerFrameSourceEditor : FrameSourceEditor
    {
        string[] propertyType = new string[] { "Default", "Input" };

        protected override void DrawNormalOptions()
        {
            base.DrawNormalOptions();

            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasDesiredFocusMode = serializedObject.FindProperty("hasDesiredFocusMode");
                hasDesiredFocusMode.boolValue = EditorGUILayout.Popup("Desired Focus Mode", hasDesiredFocusMode.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredFocusMode.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredFocusMode"), new GUIContent(), true);
                }
            }
            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasDesiredResolution = serializedObject.FindProperty("hasDesiredResolution");
                hasDesiredResolution.boolValue = EditorGUILayout.Popup("Desired Resolution", hasDesiredResolution.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredResolution.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredResolution"), new GUIContent(), true);
                }
            }
            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasDesiredFrameRate = serializedObject.FindProperty("hasDesiredFrameRate");
                hasDesiredFrameRate.boolValue = EditorGUILayout.Popup("Desired Frame Rate", hasDesiredFrameRate.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredFrameRate.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredFrameRate"), new GUIContent(), true);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected override void DrawAdvancedOptions()
        {
            base.DrawAdvancedOptions();

            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasDesiredMinQualityLevel = serializedObject.FindProperty("hasDesiredMinQualityLevel");
                hasDesiredMinQualityLevel.boolValue = EditorGUILayout.Popup("Desired Min Quality Level", hasDesiredMinQualityLevel.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredMinQualityLevel.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredMinQualityLevel"), new GUIContent(), true);
                }
            }
            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasDesiredTrackingMode = serializedObject.FindProperty("hasDesiredTrackingMode");
                hasDesiredTrackingMode.boolValue = EditorGUILayout.Popup("Desired Tracking Mode", hasDesiredTrackingMode.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredTrackingMode.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredTrackingMode"), new GUIContent(), true);
                }
            }

            var fs = target as MotionTrackerFrameSource;
            var cameraCandidate = fs.CameraCandidate;
            fs.CameraCandidate = (Camera)EditorGUILayout.ObjectField(new GUIContent("Camera Candidate", "Only effective if Unity XR Origin is not used. Camera.main will be used if not specified."), fs.CameraCandidate, typeof(Camera), true);
            if (cameraCandidate != fs.CameraCandidate)
            {
                EditorUtility.SetDirty(fs);
            }
        }
    }
}
