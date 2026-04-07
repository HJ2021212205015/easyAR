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
    [CustomEditor(typeof(EasyARSettings), true)]
    class EasyARSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = 260;
            var settings = target as EasyARSettings;

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.InitializeOnStartup)));

            EditorGUILayout.LabelField("EasyAR Sense License", EditorStyles.boldLabel);

            using (_ = new IndentScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.LicenseKey)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.VerifyLicenseWhenBuild)), new GUIContent("Verify When Build"), true);
            }

            EditorGUILayout.LabelField("Lib Variants", EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
#if EASYAR_ENABLE_MEGA
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"libVariantsMega.{nameof(settings.LibVariants.Android)}"), new GUIContent("Android"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"libVariantsMega.{nameof(settings.LibVariants.IOS)}"), new GUIContent("iOS"), true);
#else
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"libVariants.{nameof(settings.LibVariants.Android)}"), new GUIContent("Android"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"libVariants.{nameof(settings.LibVariants.IOS)}"), new GUIContent("iOS"), true);
#endif
            }

            EditorGUILayout.LabelField("Permissions", EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"permissions.{nameof(settings.Permissions.Camera)}"), new GUIContent("Camera"), true);
                using (_ = new EnableScope(false))
                {
                    EditorGUILayout.Toggle("Location (for Mega)", settings.Permissions.Location);
                    EditorGUILayout.Toggle("Microphone (Android)", settings.Permissions.AndroidMicrophone);
                }
            }


            ExpandedPropertyField("Unity XR", serializedObject.FindProperty(nameof(settings.UnityXR)));

            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), Color.gray);

            EditorGUILayout.LabelField("Mega", EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
                using (_ = new EnableScope(false))
                {
                    EditorGUILayout.Toggle($"InertialCameraDevice support", settings.HasBundledONNXRuntime);
                    using (_ = new IndentScope())
                    {
                        if (settings.HasBundledONNXRuntime)
                        {
                            EditorGUILayout.LabelField($"ONNX Runtime: {(settings.UseBundledONNXRuntime ? "Bundled (1.22.0)" : "External")}");
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"To enable, set 'Lib Variants > Android' to {EasyARSettings.LibVariantConfig.AndroidVariant.Full}");
                        }
                    }
                }
                EditorGUILayout.LabelField($"Mega Block");
                using (_ = new IndentScope())
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.GlobalMegaBlockLocalizationServiceConfig)), new GUIContent("Localization Service Access [Global]"), true);
                }
                EditorGUILayout.LabelField($"Mega Landmark");
                using (_ = new IndentScope())
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.GlobalMegaLandmarkLocalizationServiceConfig)), new GUIContent("Localization Service Access [Global]"), true);
                }
            }

            EditorGUILayout.LabelField("Spatial Map", EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.GlobalSpatialMapServiceConfig)), new GUIContent("Service Access [Global]"), true);
            }

            EditorGUILayout.LabelField("Image Tracking", EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(settings.GizmoConfig)}.{nameof(settings.GizmoConfig.ImageTarget)}"), new GUIContent("Target Gizmo"), true);
                EditorGUILayout.LabelField("Cloud Recognition (CRS)");
                using (_ = new IndentScope())
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.GlobalCloudRecognizerServiceConfig)), new GUIContent("Service Access [Global]"), true);
                }
            }

            EditorGUILayout.LabelField("Object Tracking", EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(settings.GizmoConfig)}.{nameof(settings.GizmoConfig.ObjectTarget)}"), new GUIContent("Target Gizmo"), true);
            }

            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), Color.gray);

            EditorGUILayout.LabelField("Third-Party Libraries", EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.ARCoreSDK)));
                using (_ = new IndentScope())
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.Verify32bitOnlyARCoreWhenBuild)), new GUIContent("Warn 32-bit-only ARCore-enabled build"), true);
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(settings.AREngineSDK)));
                if (settings.HasBundledONNXRuntime)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty($"useBundledONNXRuntime"), new GUIContent("ONNX Runtime (Bundled)"), true);
                }
                else
                {
                    using (_ = new EnableScope(false))
                    {
                        EditorGUILayout.Toggle("ONNX Runtime (Bundled)", false);
                    }
                }
            }
            ExpandedPropertyField("Workaround For Unity", serializedObject.FindProperty(nameof(settings.WorkaroundForUnity)));

            serializedObject.ApplyModifiedProperties();
        }

        private void ExpandedPropertyField(string label, SerializedProperty property)
        {
            EditorGUILayout.LabelField(new GUIContent(label, property.tooltip), EditorStyles.boldLabel);
            using (_ = new IndentScope())
            {
                SerializedProperty iterator = property.Copy();
                SerializedProperty endProperty = iterator.GetEndProperty();

                iterator.NextVisible(true);

                while (!SerializedProperty.EqualContents(iterator, endProperty))
                {
                    EditorGUILayout.PropertyField(iterator, true);
                    iterator.NextVisible(false);
                }
            }
        }

        [MenuItem("EasyAR/Sense/Configuration", priority = 0)]
        private static void ConfigurationMenu()
        {
            SettingsService.OpenProjectSettings("Project/EasyAR/Sense");
        }

        [MenuItem("EasyAR/Sense/Document", priority = 100)]
        private static void DocumentEn()
        {
            Application.OpenURL("https://www.easyar.com/view/support.html");
        }

        [MenuItem("EasyAR/Sense/文档", priority = 100)]
        private static void DocumentZh()
        {
            Application.OpenURL("https://www.easyar.cn/view/support.html");
        }
    }
}
