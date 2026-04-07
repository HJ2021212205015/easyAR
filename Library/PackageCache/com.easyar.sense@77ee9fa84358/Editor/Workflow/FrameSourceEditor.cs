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
    [CustomEditor(typeof(FrameSource), true)]
    public class FrameSourceEditor : Editor
    {
        private bool showAdvanced;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawNormalOptions();
            showAdvanced = EditorGUILayout.Toggle("Advanced Options", showAdvanced);
            if (showAdvanced)
            {
                EditorGUI.indentLevel += 1;
                DrawAdvancedOptions();
                EditorGUI.indentLevel -= 1;
            }
        }

        protected virtual void DrawNormalOptions() { }

        protected virtual void DrawAdvancedOptions() { }
    }
}
