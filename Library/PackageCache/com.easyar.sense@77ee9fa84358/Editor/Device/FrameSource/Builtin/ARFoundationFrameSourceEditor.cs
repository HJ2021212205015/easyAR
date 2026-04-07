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
    [CustomEditor(typeof(ARFoundationFrameSource), true)]
    class ARFoundationFrameSourceEditor : FrameSourceEditor
    {
        protected override void DrawNormalOptions()
        {
            base.DrawNormalOptions();
#if EASYAR_DISABLE_ARFOUNDATION
            EditorGUILayout.HelpBox($"Frame source disabled: EasyAR ARFoundation support is disabled", MessageType.Warning);
#endif
#if !EASYAR_ENABLE_ARFOUNDATION
            EditorGUILayout.HelpBox($"Frame source disabled: missing AR Foundation 5+ package", MessageType.Warning);
#endif
        }
    }
}
