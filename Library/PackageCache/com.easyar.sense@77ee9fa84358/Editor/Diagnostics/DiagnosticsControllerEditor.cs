//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace easyar
{
    [CustomEditor(typeof(DiagnosticsController), true)]
    class DiagnosticsControllerEditor : Editor
    {
        private static readonly List<DiagnosticsControllerEditor> editors = new List<DiagnosticsControllerEditor>();
        private static readonly string previewTooltip = "This is a preview, runtime value may be different.";
        private static readonly string previewFrameSourceTooltip = previewTooltip + " The first available one in transfrom order will be used at runtime. You can change the list by move the GameObject directly in the hierarchy window.";
        private bool showPreview;
        private SessionValidationTool.ToolCache cache;

        private void OnEnable()
        {
            editors.Add(this);
            cache = new SessionValidationTool.ToolCache(false);
        }

        private void OnDisable()
        {
            editors.Remove(this);
        }

        public override void OnInspectorGUI()
        {
            var diagnostics = target as DiagnosticsController;
            if (diagnostics.IsExternalTool)
            {
                EditorGUILayout.HelpBox($"Controlled by External Tool", MessageType.Info);
                return;
            }
            var session = diagnostics.GetComponent<ARSession>();
            if (!session)
            {
                EditorGUILayout.HelpBox($"Require {nameof(ARSession)}", MessageType.Error);
                return;
            }
            DrawDefaultInspector();
            if (diagnostics.DeveloperModeSwitch != DiagnosticsController.DeveloperModeSwitchType.Default)
            {
                EditorGUILayout.HelpBox($"Developer mode switch set to {diagnostics.DeveloperModeSwitch}. Please provide your custom switch or equivalent function to replace develop mode in your app. If you do not provide a replacement, issue feedbacks to EasyAR may be rejected, expecially for Mega users.\n\n开发者模式开关被设置为{diagnostics.DeveloperModeSwitch}。请提供自定义开关，或提供开发者模式的等价替代。如果你没有提供替代，后续给到EasyAR的问题反馈将可能被拒绝，尤其是对Mega用户而言非常重要。", MessageType.Warning);
            }

            if (!Application.isPlaying)
            {
                showPreview = EditorGUILayout.Foldout(showPreview, new GUIContent("Assembly Preview (Will Recalculate at Runtime)", previewTooltip));
                if (showPreview)
                {
                    ++EditorGUI.indentLevel;
                    DrawAssemblyPreview(session, session.AssembleOptions);
                    --EditorGUI.indentLevel;
                }
            }

            DrawSeperater();

            using (_ = new GUILayout.HorizontalScope(GUI.skin.box))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Session Validation Tool", EditorStyles.largeLabel);
                using (_ = new EnableScope(diagnostics.IsValidationOn))
                {
                    SessionValidationTool.DrawPlayModeSwitchButton();
                }
                GUILayout.FlexibleSpace();
                using (_ = new EnableScope(diagnostics.IsValidationOn))
                {
                    if (GUILayout.Button("↗", GUILayout.Width(20), GUILayout.MaxHeight(20)))
                    {
                        diagnostics.ValidationWindow = SessionValidationTool.Get(session);
                        Repaint();
                    }
                }
            }
            if (diagnostics.ValidationWindow is SessionValidationTool tool && !tool.IsBound(session))
            {
                diagnostics.ValidationWindow = null;
                Repaint();
            }
            if (diagnostics.ValidationWindow)
            {
                using (_ = new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.HelpBox($"Opened in 'Session Validation Tool' Window", MessageType.Info);
                    if (GUILayout.Button("↘", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        ClearWindow();
                    }
                }
                using (_ = new EnableScope(!EditorApplication.isPlaying))
                {
                    var isValidationOn = diagnostics.IsValidationOn;
                    var isValidateFrame = diagnostics.IsValidateFrame;
                    diagnostics.IsValidateFrame = GUILayout.Toggle(diagnostics.IsValidateFrame, "Frame Player");
                    if (isValidateFrame != diagnostics.IsValidateFrame)
                    {
                        EditorUtility.SetDirty(diagnostics);
                    }
                    var isValidateWorkflow = diagnostics.IsValidateWorkflow;
                    diagnostics.IsValidateWorkflow = GUILayout.Toggle(diagnostics.IsValidateWorkflow, "Session Workflow");
                    if (isValidateWorkflow != diagnostics.IsValidateWorkflow)
                    {
                        EditorUtility.SetDirty(diagnostics);
                    }
                    if (isValidationOn != diagnostics.IsValidationOn && !diagnostics.IsValidationOn)
                    {
                        ClearWindow();
                    }
                }
            }
            else
            {
                using (_ = new EnableScope(diagnostics.IsValidationOn))
                {
                    SessionValidationTool.DrawTool(session, cache);
                }
            }
        }

        private void OnSceneGUI()
        {
            var diagnostics = target as DiagnosticsController;
            if (SessionValidationTool.CheckForUpdate(diagnostics.GetComponent<ARSession>()))
            {
                Repaint();
            }
        }

        public static void Repaint(DiagnosticsController diag)
        {
            foreach (var editor in editors.Where(e => e.target == diag)) { editor.Repaint(); }
        }

        private void ClearWindow()
        {
            var diagnostics = target as DiagnosticsController;
            var session = diagnostics.GetComponent<ARSession>();
            if (!session) { return; }
            if (diagnostics.ValidationWindow is SessionValidationTool tool)
            {
                tool.CloseSession(session);
            }
            diagnostics.ValidationWindow = null;
        }

        private void DrawSeperater()
        {
            var color = GUI.color;
            EditorGUILayout.Space();
            GUI.color = Color.gray;
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            GUI.color = color;
            EditorGUILayout.Space();
        }

        private static void DrawAssemblyPreview(ARSession session, AssembleOptions options)
        {
            var (frameSources, frameFilters) = Preview(session, options);
            using (_ = new EnableScope(false))
            using (_ = new GUILayout.VerticalScope(GUI.skin.box))
            {
                var onDeviceLabel = string.Empty;
                if (session.Diagnostics && session.Diagnostics.IsValidateFrame)
                {
                    var guiColor = GUI.color;
                    GUI.color = Color.yellow;
                    EditorGUILayout.LabelField(new GUIContent("Frame Player of Session Validation is ON in Editor"));
                    GUI.color = guiColor;
                    EditorGUILayout.ObjectField(new GUIContent("FrameSource in Editor", previewTooltip), session.GetComponent<FramePlayer>(), typeof(FrameSource), true);
                    onDeviceLabel = " on Device";
                }

                if (options.FrameSource == AssembleOptions.FrameSourceSelection.Auto)
                {
                    EditorGUILayout.LabelField(new GUIContent($"FrameSource{onDeviceLabel}", previewFrameSourceTooltip));
                    ++EditorGUI.indentLevel;
                    var guiColor = GUI.color;
                    GUI.color = Color.yellow;
                    EditorGUILayout.LabelField(new GUIContent("Transfrom order, first one available will be used", previewFrameSourceTooltip));
                    GUI.color = guiColor;
                    for (int i = 0; i < frameSources.Count; i++)
                    {
                        EditorGUILayout.ObjectField(new GUIContent($"Order {i}", previewFrameSourceTooltip), frameSources[i], typeof(FrameSource), true);
                    }
                    --EditorGUI.indentLevel;
                }
                else
                {
                    EditorGUILayout.ObjectField(new GUIContent($"FrameSource{onDeviceLabel}", previewTooltip), frameSources.Count >= 1 ? frameSources[0] : null, typeof(FrameSource), true);
                }

                EditorGUILayout.LabelField(label: new GUIContent($"FrameFilter List ({frameFilters.Count})", previewTooltip));
                ++EditorGUI.indentLevel;
                for (int i = 0; i < frameFilters.Count; i++) { EditorGUILayout.ObjectField(new GUIContent($"Element {i}", previewTooltip), frameFilters[i], typeof(FrameFilter), true); }
                --EditorGUI.indentLevel;
            }

            if (options.EnableCustomCamera && frameSources.Where(s => FrameSource.IsCustomCamera(s)).Any())
            {
                EditorGUILayout.HelpBox("Custom camera may be used at runtime. AR Engine, ARFoundation and all HMD support are implemented as custom camera.\n运行时可能会使用自定义相机。AR Engine、ARFoundation及各种头显的支持都是通通过自定义相机实现的。", MessageType.Info);
                EditorGUILayout.HelpBox("EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.\n在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。", MessageType.Warning);
            }
        }

        private static (List<FrameSource> frameSources, List<FrameFilter> frameFilters) Preview(ARSession session, AssembleOptions options)
        {
            var frameSources = GetComponentsInChildrenTransformOrder<FrameSource>(session.transform).Where(fs => !(fs is FramePlayer));
            if (!options.EnableCustomCamera)
            {
                frameSources = frameSources.Where(fs => !FrameSource.IsCustomCamera(fs));
            }

            if (options.FrameSource == AssembleOptions.FrameSourceSelection.Manual)
            {
                frameSources = frameSources.Where(f => f == options.SpecifiedFrameSource).ToList();
            }
            else if (options.FrameSource == AssembleOptions.FrameSourceSelection.FramePlayer)
            {
                frameSources = new List<FrameSource> { session.GetComponent<FramePlayer>() };
            }

            var frameFilters = new List<FrameFilter>(session.GetComponentsInChildren<FrameFilter>());
            if (options.FrameFilter == AssembleOptions.FrameFilterSelection.Manual)
            {
                frameFilters = options.SpecifiedFrameFilters != null ? frameFilters.Where(f => options.SpecifiedFrameFilters.Contains(f)).ToList() : new List<FrameFilter>();
            }

            return (frameSources.ToList(), frameFilters);
        }

        private static List<CType> GetComponentsInChildrenTransformOrder<CType>(Transform transform)
        {
            var list = new List<CType>();
            foreach (Transform t in transform)
            {
                GetComponentsInChildrenTransformOrder(list, t);
            }
            return list;
        }

        private static void GetComponentsInChildrenTransformOrder<CType>(List<CType> transforms, Transform transform)
        {
            if (!transform || !transform.gameObject.activeSelf) { return; }
            transforms.AddRange(transform.GetComponents<CType>());
            foreach (Transform t in transform)
            {
                GetComponentsInChildrenTransformOrder(transforms, t);
            }
        }
    }
}
