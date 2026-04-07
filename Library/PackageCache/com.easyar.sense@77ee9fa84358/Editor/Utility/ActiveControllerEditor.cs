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
    [CustomEditor(typeof(ActiveController), true)]
    class ActiveControllerEditor : Editor
    {
        string[] propertyType = new string[] { "Default", "Input" };

        private void OnEnable()
        {
            var controller = target as ActiveController;
            propertyType[0] = "Default" + (controller.GetComponent<XROriginChildController>() ? " (ActiveAfterFirstTracked)" : " (ActiveWhileTracked)");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var controller = target as ActiveController;
            var strategy = controller.OverrideStrategy;
            using (_ = new EditorGUILayout.HorizontalScope())
            {
                var hasOverrideStrategy = serializedObject.FindProperty("hasOverrideStrategy");
                hasOverrideStrategy.boolValue = EditorGUILayout.Popup(new GUIContent("Strategy", "Set enabled to false if you need to turn off active controller."), hasOverrideStrategy.boolValue ? 1 : 0, propertyType) == 1;
                if (hasOverrideStrategy.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("overrideStrategy"), new GUIContent(), true);
                }
            }
            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying && strategy != controller.OverrideStrategy)
            {
                controller.OverrideStrategy = controller.OverrideStrategy;
            }
        }
    }
}
