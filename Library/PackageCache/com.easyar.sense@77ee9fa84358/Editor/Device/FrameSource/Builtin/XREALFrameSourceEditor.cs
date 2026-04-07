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
    [CustomEditor(typeof(XREALFrameSource), true)]
    class XREALFrameSourceEditor : FrameSourceEditor
    {
        protected override void DrawNormalOptions()
        {
            base.DrawNormalOptions();
#if !EASYAR_ENABLE_XREAL
            EditorGUILayout.HelpBox($"missing package: com.xreal.xr (XREAL SDK) >= 3.1", MessageType.Warning);
#endif
        }
    }
}
