//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEditor;
using UnityEngine;

namespace easyar
{
    [CustomEditor(typeof(ExternalDeviceFrameSource), true)]
    class ExternalDeviceFrameSourceEditor : FrameSourceEditor
    {
        protected override void DrawNormalOptions()
        {
            base.DrawNormalOptions();
            var fs = target as ExternalDeviceFrameSource;
            ExternalDeviceFrameSource.DeviceOriginType OriginType;
            try
            {
                OriginType = fs.OriginType;
            }
            catch (Exception e)
            {
                EditorGUILayout.HelpBox("Invalid frame source: " + e.Message, MessageType.Error);
                return;
            }

            switch (OriginType)
            {
                case ExternalDeviceFrameSource.DeviceOriginType.XROrigin:
#if !EASYAR_ENABLE_XRORIGIN
                    EditorGUILayout.HelpBox($"missing package: com.unity.xr.core-utils >= 2.0.0", MessageType.Error);
#endif
                    break;
            }
        }
    }
}
