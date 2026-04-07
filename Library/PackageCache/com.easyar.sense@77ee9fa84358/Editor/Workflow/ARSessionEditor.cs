//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace easyar
{
    [CustomEditor(typeof(ARSession), true)]
    class ARSessionEditor : Editor
    {
        private void OnEnable()
        {
            var session = target as ARSession;
            // for backward compatibility
            if (!session.GetComponent<DiagnosticsController>())
            {
                session.gameObject.AddComponent<DiagnosticsController>();
                EditorUtility.SetDirty(session);
            }
            if (!session.GetComponent<FrameRecorder>())
            {
                session.gameObject.AddComponent<FrameRecorder>();
                EditorUtility.SetDirty(session);
            }
            if (!session.GetComponent<FramePlayer>())
            {
                session.gameObject.AddComponent<FramePlayer>();
                EditorUtility.SetDirty(session);
            }
            if (!session.GetComponent<CameraImageRenderer>())
            {
                session.gameObject.AddComponent<CameraImageRenderer>();
                EditorUtility.SetDirty(session);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var session = target as ARSession;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(session.AutoStart)));

            EditorGUILayout.LabelField("Assemble Options");
            ++EditorGUI.indentLevel;
            using (_ = session.State < ARSession.SessionState.Assembling ? new EnableScope() : new EnableScope(false))
            {
                DrawAssembleOptions(session, session.AssembleOptions, "assembleOptions");
            }
            --EditorGUI.indentLevel;

            if (session.AvailableCenterMode.Count <= 0)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("CenterMode"), new GUIContent("Center"));
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var idx = 0;
                foreach (var mode in session.AvailableCenterMode)
                {
                    if (mode == session.CenterMode) { break; }
                    ++idx;
                }
                idx = EditorGUILayout.Popup("Center", idx, session.AvailableCenterMode.Select(m => m.ToString()).ToArray());
                if (idx < session.AvailableCenterMode.Count)
                {
                    session.CenterMode = session.AvailableCenterMode[idx];
                }
            }

            ++EditorGUI.indentLevel;
            if (session.CenterMode == ARSession.ARCenterMode.SpecificTarget)
            {
                var center = session.SpecificTargetCenter;
                session.SpecificTargetCenter = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Target",
                    "The Target must be: " +
#if EASYAR_ENABLE_MEGA
                    $"{nameof(EasyAR.Mega.Scene.BlockRootController)} or " +
#endif
                    $"{nameof(TargetController)}."
                    ),
                    session.SpecificTargetCenter, typeof(GameObject), true);
                if (center != session.SpecificTargetCenter)
                {
                    EditorUtility.SetDirty(session);
                }
            }
            if (Application.isPlaying)
            {
                var guiEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUILayout.ObjectField("Current", session.CenterObject, typeof(GameObject), true);
                GUI.enabled = guiEnabled;
            }
            --EditorGUI.indentLevel;

            var flip = serializedObject.FindProperty(nameof(session.HorizontalFlip));
            flip.isExpanded = EditorGUILayout.Foldout(flip.isExpanded, "Horizontal Flip");
            if (flip.isExpanded)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(session.HorizontalFlip)}.{nameof(session.HorizontalFlip.BackCamera)}"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{nameof(session.HorizontalFlip)}.{nameof(session.HorizontalFlip.FrontCamera)}"));
                EditorGUI.indentLevel -= 1;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAssembleOptions(ARSession session, AssembleOptions options, string propertyName)
        {
            if (session.Diagnostics && session.Diagnostics.IsValidateFrame)
            {
                var guiColor = GUI.color;
                GUI.color = Color.yellow;
                var validationMessage = "Frame Player of Session Validation is On: Frame Source is forced to 'FramePlayer' when running in Editor.";
                EditorGUILayout.LabelField(new GUIContent(validationMessage, validationMessage));
                GUI.color = guiColor;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty($"{propertyName}.{nameof(options.EnableCustomCamera)}"), new GUIContent("Enable Custom Camera", "AR Engine, ARFoundation and all HMD support are implemented as custom camera.\nEasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.\n\nAR Engine、ARFoundation及各种头显的支持都是通通过自定义相机实现的。\n在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty($"{propertyName}.{nameof(options.FrameSource)}"));
            serializedObject.ApplyModifiedProperties();
            if (options.FrameSource == AssembleOptions.FrameSourceSelection.Manual)
            {
                ++EditorGUI.indentLevel;
                GUILayout.BeginVertical(GUI.skin.box);
                options.SpecifiedFrameSource = (FrameSource)EditorGUILayout.ObjectField(options.SpecifiedFrameSource, typeof(FrameSource), true);
                if (!options.EnableCustomCamera && FrameSource.IsCustomCamera(options.SpecifiedFrameSource))
                {
                    options.SpecifiedFrameSource = null;
                }
                GUILayout.EndVertical();
                --EditorGUI.indentLevel;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty($"{propertyName}.{nameof(options.FrameFilter)}"));
            serializedObject.ApplyModifiedProperties();
            if (options.FrameFilter == AssembleOptions.FrameFilterSelection.Manual)
            {
                ++EditorGUI.indentLevel;
                GUILayout.BeginVertical(GUI.skin.box);
                ShowListPropertyField("List", $"{propertyName}.{nameof(options.SpecifiedFrameFilters)}");
                GUILayout.EndVertical();
                --EditorGUI.indentLevel;
            }

            var list = serializedObject.FindProperty($"{propertyName}.{nameof(options.DeviceList)}");
            list.isExpanded = EditorGUILayout.Foldout(list.isExpanded, "Device List Update Options");
            if (list.isExpanded)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{propertyName}.{nameof(options.DeviceList)}.{nameof(options.DeviceList.Timeout)}"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{propertyName}.{nameof(options.DeviceList)}.{nameof(options.DeviceList.WaitTime)}"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"{propertyName}.{nameof(options.DeviceList)}.{nameof(options.DeviceList.IgnoreCache)}"));
                --EditorGUI.indentLevel;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowListPropertyField(string label, string propertyPath)
        {
            var list = serializedObject.FindProperty(propertyPath);
            list.isExpanded = EditorGUILayout.Foldout(list.isExpanded, label);
            ++EditorGUI.indentLevel;
            if (list.isExpanded)
            {
                int count = Mathf.Max(0, EditorGUILayout.IntField("Size", list.arraySize));
                while (count < list.arraySize) { list.DeleteArrayElementAtIndex(list.arraySize - 1); }
                while (count > list.arraySize) { list.InsertArrayElementAtIndex(list.arraySize); }
                for (int i = 0; i < list.arraySize; i++) { EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i)); }
            }
            --EditorGUI.indentLevel;
        }
    }
}
