//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEngine;
using UnityEditor;

namespace easyar
{
    [CustomEditor(typeof (CameraDeviceFrameSource), true)]
    class CameraDeviceFrameSourceEditor : FrameSourceEditor
    {
        string[] propertyType = new string[] { "Default", "Input" };

        protected override void DrawNormalOptions()
        {
            base.DrawNormalOptions();
            var fs = (CameraDeviceFrameSource)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(fs.CameraOpenMethod)), true);
            EditorGUI.indentLevel += 1;
            if (fs.CameraOpenMethod == CameraDeviceFrameSource.CameraDeviceOpenMethod.DeviceIndex)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(fs.CameraOpenIndex)), new GUIContent("Index"), true);
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(fs.CameraOpenType)), new GUIContent("Type"), true);
            }
            EditorGUI.indentLevel -= 1;

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
                var hasDesiredSize = serializedObject.FindProperty("hasDesiredSize");
                hasDesiredSize.boolValue = EditorGUILayout.Popup("Desired Size", hasDesiredSize.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredSize.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredSize"), new GUIContent(), true);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected override void DrawAdvancedOptions()
        {
            base.DrawAdvancedOptions();
            var fs = target as CameraDeviceFrameSource;

            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasDesiredCameraPreference = serializedObject.FindProperty("hasDesiredCameraPreference");
                hasDesiredCameraPreference.boolValue = EditorGUILayout.Popup("Desired Camera Preference", hasDesiredCameraPreference.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredCameraPreference.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredCameraPreference"), new GUIContent(), true);
                }
            }
            EditorGUI.indentLevel += 1;
            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasDesiredAndroidCameraApiType = serializedObject.FindProperty("hasDesiredAndroidCameraApiType");
                hasDesiredAndroidCameraApiType.boolValue = EditorGUILayout.Popup("Desired Android Camera Api", hasDesiredAndroidCameraApiType.boolValue ? 1 : 0, propertyType) == 1;
                if (hasDesiredAndroidCameraApiType.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("desiredAndroidCameraApiType"), new GUIContent(), true);
                }
            }
            EditorGUI.indentLevel -= 1;

            var cameraCandidate = fs.CameraCandidate;
            fs.CameraCandidate = (Camera)EditorGUILayout.ObjectField(new GUIContent("Camera Candidate", "Camera.main will be used if not specified."), fs.CameraCandidate, typeof(Camera), true);
            if (cameraCandidate != fs.CameraCandidate)
            {
                EditorUtility.SetDirty(fs);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
