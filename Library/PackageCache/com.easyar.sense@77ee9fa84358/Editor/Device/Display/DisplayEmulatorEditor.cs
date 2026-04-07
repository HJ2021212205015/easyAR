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
    [CustomEditor(typeof(CameraDeviceDisplay), true)]
    class CameraDeviceDisplayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var emulator = target as CameraDeviceDisplay;
            if (emulator.Mode == CameraDeviceDisplay.DisplayMode.Emulator)
            {
                var rotationOld = (DisplayEmulator.RotationMode)emulator.Rotation;
                var rotation = (DisplayEmulator.RotationMode)EditorGUILayout.EnumPopup("Rotation", rotationOld);
                if (rotation != rotationOld)
                {
                    emulator.EmulateRotation(rotation);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(emulator);
                }
            }
        }
    }
}
