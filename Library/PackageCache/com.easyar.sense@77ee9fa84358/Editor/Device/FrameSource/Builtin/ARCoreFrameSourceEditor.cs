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
    [CustomEditor(typeof(ARCoreFrameSource), true)]
    class ARCoreFrameSourceEditor : FrameSourceEditor
    {
        protected override void DrawAdvancedOptions()
        {
            base.DrawAdvancedOptions();
            var fs = target as ARCoreFrameSource;
            var cameraCandidate = fs.CameraCandidate;
            fs.CameraCandidate = (Camera)EditorGUILayout.ObjectField(new GUIContent("Camera Candidate", "Only effective if Unity XR Origin is not used. Camera.main will be used if not specified."), fs.CameraCandidate, typeof(Camera), true);
            if (cameraCandidate != fs.CameraCandidate)
            {
                EditorUtility.SetDirty(fs);
            }
        }
    }
}
