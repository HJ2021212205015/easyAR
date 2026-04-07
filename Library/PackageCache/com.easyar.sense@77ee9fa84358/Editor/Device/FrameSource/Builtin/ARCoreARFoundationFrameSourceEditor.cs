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
    [CustomEditor(typeof(ARCoreARFoundationFrameSource), true)]
    class ARCoreARFoundationFrameSourceEditor : ARFoundationFrameSourceEditor
    {
        protected override void DrawAdvancedOptions()
        {
            base.DrawAdvancedOptions();
            var fs = (ARCoreARFoundationFrameSource)target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(fs.OptimizeConfigurationForTracking)), true);
            serializedObject.ApplyModifiedProperties();
            if (fs.OptimizeConfigurationForTracking)
            {
                EditorGUILayout.HelpBox($"Some phones, such as the Xiaomi Mi 10, have inherent bugs that prevent image acquisition after configuration modifications. EasyAR will be unable to function in such cases. When using this option, you need to avoid similar devices or implement suitable handling measures.", MessageType.Warning);
            }
        }
    }
}
