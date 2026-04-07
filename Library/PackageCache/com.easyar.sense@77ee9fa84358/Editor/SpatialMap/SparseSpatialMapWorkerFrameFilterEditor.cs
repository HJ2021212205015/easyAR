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
    [CustomEditor(typeof(SparseSpatialMapWorkerFrameFilter), true)]
    class SparseSpatialMapWorkerFrameFilterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var filter = (SparseSpatialMapWorkerFrameFilter)target;

            var accessSource = (SparseSpatialMapWorkerFrameFilter.ServiceAccessSourceType)EditorGUILayout.EnumPopup("Service Access", filter.ServiceAccessSource);
            if (filter.ServiceAccessSource != accessSource)
            {
                filter.ServiceAccessSource = accessSource;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(filter);
            }

            EditorGUI.indentLevel += 1;
            if (accessSource == SparseSpatialMapWorkerFrameFilter.ServiceAccessSourceType.APIKey)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedAddressAPIKeyAccessData.AppID"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedAddressAPIKeyAccessData.APIKey"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedAddressAPIKeyAccessData.APISecret"), true);
            }
            EditorGUI.indentLevel -= 1;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
