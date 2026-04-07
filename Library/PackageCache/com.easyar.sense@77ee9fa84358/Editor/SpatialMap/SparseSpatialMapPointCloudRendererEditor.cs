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
    [CustomEditor(typeof(SparseSpatialMapPointCloudRenderer), true)]
    class SparseSpatialMapPointCloudRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var controller = (SparseSpatialMapPointCloudRenderer)target;
            var show = controller.Show;
            var startColor = controller.ParticleParameter.StartColor;
            var startSize = controller.ParticleParameter.StartSize;
            var startLifetime = controller.ParticleParameter.StartLifetime;
            var remainingLifetime = controller.ParticleParameter.RemainingLifetime;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("show"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pointCloudParticleSystem"), true);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particleParameter.StartColor"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particleParameter.StartSize"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particleParameter.StartLifetime"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particleParameter.RemainingLifetime"), true);
            EditorGUI.indentLevel -= 1;

            serializedObject.ApplyModifiedProperties();

            if (Application.isPlaying)
            {
                if (show != controller.Show)
                {
                    controller.Show = controller.Show;
                }
                if (startColor.r != controller.ParticleParameter.StartColor.r || startColor.g != controller.ParticleParameter.StartColor.g || startColor.b != controller.ParticleParameter.StartColor.b || startColor.a != controller.ParticleParameter.StartColor.a || startSize != controller.ParticleParameter.StartSize || startLifetime != controller.ParticleParameter.StartLifetime || remainingLifetime != controller.ParticleParameter.RemainingLifetime)
                {
                    controller.ParticleParameter = controller.ParticleParameter;
                }
            }
        }
    }
}
