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
    [CustomEditor(typeof(InertialCameraDeviceFrameSource), true)]
    class InertialCameraDeviceFrameSourceEditor : FrameSourceEditor
    {
        string[] propertyType = new string[] { "Default", "Input" };

        protected override void DrawNormalOptions()
        {
            base.DrawNormalOptions();

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
            var fs = target as InertialCameraDeviceFrameSource;
            var cameraCandidate = fs.CameraCandidate;
            fs.CameraCandidate = (Camera)EditorGUILayout.ObjectField(new GUIContent("Camera Candidate", "Only effective if Unity XR Origin is not used. Camera.main will be used if not specified."), fs.CameraCandidate, typeof(Camera), true);
            if (cameraCandidate != fs.CameraCandidate)
            {
                EditorUtility.SetDirty(fs);
            }
        }
    }
}
