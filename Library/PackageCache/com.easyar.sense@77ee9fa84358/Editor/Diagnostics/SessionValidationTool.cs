//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace easyar
{
    class SessionValidationTool : EditorWindow
    {
        private ARSession Session;
        private ToolCache cache;
        [SerializeField]
        private int sessionInstanceId;

        private static double playerTime;
        private static ARSession.SessionState sessionState;
        private static SessionWorkflowValidator workflow;

        private void OnEnable()
        {
            cache = new ToolCache(true);
            EditorApplication.playModeStateChanged += OnPlayModeChange;
        }

        private void Update()
        {
            if (CheckForUpdate(Session))
            {
                Repaint();
            }
        }

        private void OnGUI()
        {
            using (_ = new GUILayout.HorizontalScope())
            {
                DrawPlayModeSwitchButton();
                EditorGUILayout.Space(5, false);
                SetSession((ARSession)EditorGUILayout.ObjectField(Session, typeof(ARSession), true));
            }
            if (!Session) { return; }
            minSize = new Vector2(304, EditorGUIUtility.currentViewWidth < 800 ? 500 : 260);

            DrawTool(Session, cache);
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChange;
        }

        public static SessionValidationTool Get(ARSession session)
        {
            var win = GetWindow<SessionValidationTool>(false, "Session Validation Tool", true);
            var texLogo = EditorGUIUtility.Load($"Packages/{UnityPackage.Name}/Editor/Icons/icon.png") as Texture2D;
            win.titleContent = new GUIContent("Session Validation Tool", texLogo);
            win.minSize = new Vector2(304, 260);
            win.SetSession(session);
            return win;
        }

        public bool IsBound(ARSession session)
        {
            return Session == session;
        }

        public void CloseSession(ARSession session)
        {
            if (!IsBound(session)) { return; }
            ClearSession();
        }

        public static void DrawPlayModeSwitchButton()
        {
            if (!EditorApplication.isPlaying)
            {
                if (GUILayout.Button("▶", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    EditorApplication.EnterPlaymode();
                }
            }
            else
            {
                if (GUILayout.Button("■", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    EditorApplication.ExitPlaymode();
                }
            }
        }

        public static void DrawTool(ARSession session, ToolCache cache)
        {
            if (!EditorApplication.isPlaying)
            {
                using (_ = new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.HelpBox("This tool is available in Unity play mode. Click '▶' (play button) in Unity toolbar to start.", MessageType.Info);
                    if (GUILayout.Button("?", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        Application.OpenURL($"https://docs.unity3d.com/Manual/Toolbar.html");
                    }
                }
                EditorGUILayout.Space(10);
            }


            var diagnostics = session.GetComponent<DiagnosticsController>();

            using (_ = new GUILayout.HorizontalScope())
            {
                using (_ = new GUILayout.VerticalScope())
                {
                    using (_ = new EnableScope(!EditorApplication.isPlaying))
                    {
                        var isValidateFrame = diagnostics.IsValidateFrame;
                        diagnostics.IsValidateFrame = GUILayout.Toggle(diagnostics.IsValidateFrame, "Frame Player");
                        if (isValidateFrame != diagnostics.IsValidateFrame)
                        {
                            EditorUtility.SetDirty(diagnostics);
                        }
                    }
                    var player = session.GetComponent<FramePlayer>();
                    if (player)
                    {
                        using (_ = EditorApplication.isPlaying && diagnostics.IsValidateFrame ? new EnableScope() : new EnableScope(false))
                        {
                            DrawPlayer(session);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox($"Missing {nameof(FramePlayer)}", MessageType.Error);
                    }
                    using (_ = new EnableScope(!EditorApplication.isPlaying))
                    {
                        var isValidateWorkflow = diagnostics.IsValidateWorkflow;
                        diagnostics.IsValidateWorkflow = GUILayout.Toggle(diagnostics.IsValidateWorkflow, "Session Workflow");
                        if (isValidateWorkflow != diagnostics.IsValidateWorkflow)
                        {
                            EditorUtility.SetDirty(diagnostics);
                        }
                    }
                    using (_ = EditorApplication.isPlaying && diagnostics.IsValidateWorkflow ? new EnableScope() : new EnableScope(false))
                    {
                        DrawSessionWorkflow(session, cache);
                        if (diagnostics.IsValidateWorkflow)
                        {
                            if (EditorApplication.isPlaying && EditorGUIUtility.currentViewWidth < 800)
                            {
                                EditorGUILayout.Space(10);
                                DrawSessionDetails(session, cache, diagnostics.IsValidateFrame);
                            }
                        }
                    }
                }
                if (EditorGUIUtility.currentViewWidth >= 800)
                {
                    if (diagnostics.IsValidateWorkflow)
                    {
                        EditorGUILayout.Space(10);
                        using (_ = new GUILayout.VerticalScope())
                        {
                            if (EditorApplication.isPlaying && diagnostics.IsValidateWorkflow)
                            {
                                EditorGUILayout.Space(10);
                                DrawSessionDetails(session, cache, diagnostics.IsValidateFrame);
                            }
                        }
                    }
                }
            }
        }

        public static void DrawPlayer(ARSession session)
        {
            if (!session) { return; }

            using (_ = new GUILayout.VerticalScope(GUI.skin.box))
            using (_ = EditorApplication.isPlaying && session.State >= ARSession.SessionState.Ready ? new EnableScope() : new EnableScope(false))
            {
                var player = session.GetComponent<FramePlayer>();
                if (!player) { return; }
                var playTime = EditorApplication.isPlaying ? (float)player.Time : default;
                using (_ = player.IsSeekable ? new EnableScope() : new EnableScope(false))
                {
                    var time = GUILayout.HorizontalSlider(playTime, 0, player.Length.OnSome ? (float)player.Length.Value : default);
                    if (player.IsSeekable && Mathf.Abs(time - playTime) > float.Epsilon)
                    {
                        player.Seek(time);
                    }
                }
                EditorGUILayout.Space(10);

                using (_ = new GUILayout.HorizontalScope())
                {
                    string timeStr = $"{(int)player.Time / 60:00}:{(int)player.Time % 60:00}";
                    string lengthStr = player.Length.OnSome ? $"{(int)player.Length / 60:00}:{(int)player.Length % 60:00}" : "00:00";
                    GUILayout.Label($"{timeStr} / {lengthStr}", EditorStyles.largeLabel);
                    EditorGUILayout.Space(10);
                    GUILayout.Label($"{Math.Round(player.Speed * 10) / 10:F1}x", EditorStyles.largeLabel);
                    EditorGUILayout.Space(10);
                    GUILayout.FlexibleSpace();
                }
                using (_ = new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent(player.IsStarted && player.enabled ? "▮▮" : "▶", "Play"), GUILayout.Width(40), GUILayout.Height(40)))
                    {
                        if (player.IsStarted)
                        {
                            player.enabled = !player.enabled;
                        }
                        else
                        {
                            var path = player.FilePath;
                            if (player.FilePathType == WritablePathType.PersistentDataPath)
                            {
                                path = Application.persistentDataPath + "/" + path;
                            }
                            if (!File.Exists(path))
                            {
                                try
                                {
                                    if (Open(player, $"Open eif file (File not found: {path})"))
                                    {
                                        Reset(session);
                                        Play(session, player);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Debug.LogException(e, player);
                                }
                                finally
                                {
                                    GUIUtility.ExitGUI();
                                }
                            }
                            else
                            {
                                Play(session, player);
                            }
                        }
                    }
                    if (GUILayout.Button(new GUIContent("■", "Stop"), new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Width(40), GUILayout.Height(40)))
                    {
                        session.enabled = false;
                        player.Stop();
                    }
                    using (_ = player.IsSeekable ? new EnableScope() : new EnableScope(false))
                    {
                        if (GUILayout.Button(new GUIContent("▮◀", "Seek backward"), GUILayout.Width(40), GUILayout.Height(40)))
                        {
                            player.Seek(player.Time - 5);
                        }
                    }
                    using (_ = player.IsSpeedChangeable ? new EnableScope() : new EnableScope(false))
                    {
                        if (GUILayout.Button(new GUIContent("◀◀", "Decrease speed"), GUILayout.Width(40), GUILayout.Height(40)))
                        {
                            player.Speed -= 0.1;
                        }
                        if (GUILayout.Button(new GUIContent("▶▶", "Increase speed"), GUILayout.Width(40), GUILayout.Height(40)))
                        {
                            player.Speed += 0.1;
                        }
                    }
                    using (_ = player.IsSeekable ? new EnableScope() : new EnableScope(false))
                    {
                        if (GUILayout.Button(new GUIContent("▶▮", "Seek forward"), GUILayout.Width(40), GUILayout.Height(40)))
                        {
                            player.Seek(player.Time + 5);
                        }
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent("▲", "Open file"), GUILayout.Width(40), GUILayout.Height(40)))
                    {
                        try
                        {
                            if (Open(player, "Open eif file"))
                            {
                                Reset(session);
                                Play(session, player);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e, player);
                        }
                        finally
                        {
                            GUIUtility.ExitGUI();
                        }
                    }
                }
                var filePath = player.FilePath;
                if (!string.IsNullOrEmpty(filePath) && player.FilePathType == WritablePathType.PersistentDataPath)
                {
                    filePath = Application.persistentDataPath + "/" + filePath;
                }
                GUILayout.Label(new GUIContent($"{Path.GetFileName(filePath)}", filePath));
                EditorGUILayout.Space(10);

                if (EditorApplication.isPlaying && !player.IsStarted)
                {
                    EditorGUILayout.HelpBox("Click ▲ button to open new file.", MessageType.Info);
                }
            }
            if (EditorApplication.isPlaying && SystemUtil.RenderPipeline == SystemUtil.RenderPipelineType.URP)
            {
                EditorGUILayout.HelpBox("URP detected." + Environment.NewLine +
                    $"If you do not see image when running eif, please add `EasyARCameraImageRendererFeature` in your active URP pipeline assets, or turn URP off." + Environment.NewLine +
                    $"如果你在播放eif时看不到画面，请在你激活的URP管线资产上添加`EasyARCameraImageRendererFeature`，或关闭URP。" + Environment.NewLine +
                    $"Project Settings > Quality > (Choose Level the PC icon use) > Rendering Pipeline Asset", MessageType.Info);
            }
        }

        private static bool Open(FramePlayer player, string title)
        {
            var playerEnabled = player.enabled;
            if (playerEnabled) { player.enabled = false; }
            var path = EditorUtility.OpenFilePanel(title, "", "eif,mkveif");
            if (playerEnabled) { player.enabled = true; }
            if (!string.IsNullOrEmpty(path))
            {
                player.FilePathType = WritablePathType.Absolute;
                player.FilePath = path;
                return true;
            }
            return false;
        }

        private static void Play(ARSession session, FramePlayer player)
        {
            session.enabled = true;
            player.Play();
        }

        private static void Reset(ARSession session)
        {
            if (!session || session.Assembly == null) { return; }
            foreach (var filter in session.Assembly.FrameFilters)
            {
                if (filter is MegaTrackerFrameFilter megaTracker)
                {
                    megaTracker.ResetTracker();
                }
            }
        }

        private static void DrawSessionWorkflow(ARSession session, ToolCache cache)
        {
            if (!session) { return; }
            if (workflow == null || (EditorApplication.isPlaying && !workflow.IsValid(session)))
            {
                workflow = EditorApplication.isPlaying ? new SessionWorkflowValidator(session) : new SessionWorkflowValidator(null);
            }

            using (_ = new GUILayout.VerticalScope(GUI.skin.box))
            {
                const int minWidth = 28;
                using (_ = new GUILayout.HorizontalScope())
                {
                    var color = GUI.backgroundColor;
                    GUILayout.Label($"EasyAR Sense:", GUILayout.MinWidth(minWidth));
                    GUI.color = EditorApplication.isPlaying ? (EasyARController.IsReady ? Color.green : Color.red) : color;
                    GUILayout.Label(EasyARController.IsReady ? "Ready" : "Not Ready");
                    GUI.color = color;
                    EditorGUILayout.Space(20);
                    GUILayout.Label("AR Session:", GUILayout.MinWidth(minWidth));
                    GUI.color = session.State == ARSession.SessionState.Broken ? Color.red : (session.State == ARSession.SessionState.Assembled ? Color.yellow : (session.State >= ARSession.SessionState.Ready ? Color.green : color));
                    GUILayout.Label($"{session.State}");
                    GUI.color = color;
                    GUILayout.FlexibleSpace();
                }

                if (workflow.CanInitialize)
                {
                    using (_ = new GUILayout.HorizontalScope())
                    {
                        using (_ = workflow.CanInitialize ? new EnableScope() : new EnableScope(false))
                        {
                            GUILayout.Label("License: ");
                            cache.InitializeIndex = EditorGUILayout.Popup(cache.InitializeIndex, new string[] { "Project Settings", "Custom Input" }, GUILayout.Width(120));
                            var license = GUILayout.TextField(cache.InitializeIndex == 0 ? (EasyARSettings.Instance != null ? EasyARSettings.Instance.LicenseKey : string.Empty) : cache.CustomLicense, GUILayout.Width(300), GUILayout.MinWidth(minWidth));
                            if (cache.InitializeIndex == 1)
                            {
                                cache.CustomLicense = license;
                            }
                            GUILayout.FlexibleSpace();
                        }
                    }
                }
                using (_ = new GUILayout.HorizontalScope())
                {
                    using (_ = new GUILayout.VerticalScope())
                    {
                        using (_ = workflow.CanInitialize ? new EnableScope() : new EnableScope(false))
                        {
                            if (GUILayout.Button("Initialize", GUILayout.MinWidth(minWidth)))
                            {
                                if (cache.InitializeIndex == 0)
                                {
                                    workflow.Initialize();
                                }
                                else if (cache.InitializeIndex == 1)
                                {
                                    workflow.Initialize(cache.CustomLicense);
                                }
                            }
                        }
                    }

                    using (_ = new GUILayout.VerticalScope())
                    {
                        GUILayout.Label("──>");
                        GUILayout.Label(" └>");
                    }

                    using (_ = new GUILayout.VerticalScope())
                    {
                        using (_ = new GUILayout.HorizontalScope())
                        {
                            using (_ = workflow.CanAssemble ? new EnableScope() : new EnableScope(false))
                            {
                                if (GUILayout.Button("Assemble", GUILayout.MinWidth(minWidth)))
                                {
                                    workflow.Assemble();
                                }
                            }
                            GUILayout.Label("─>");
                            using (_ = workflow.CanStartAssembledSession ? new EnableScope() : new EnableScope(false))
                            {
                                if (GUILayout.Button("StartSession (Assembled)", GUILayout.MinWidth(minWidth)))
                                {
                                    workflow.StartSession();
                                }
                            }
                        }
                        using (_ = new GUILayout.HorizontalScope())
                        {
                            using (_ = workflow.CanAssemble ? new EnableScope() : new EnableScope(false))
                            {
                                if (GUILayout.Button("StartSession", GUILayout.MinWidth(minWidth)))
                                {
                                    workflow.StartSession();
                                }
                            }
                        }
                    }

                    using (_ = new GUILayout.VerticalScope())
                    {
                        GUILayout.Label("───>");
                        GUILayout.Label("┘ └> ");
                    }

                    using (_ = new GUILayout.VerticalScope())
                    {
                        using (_ = workflow.CanStopSession ? new EnableScope() : new EnableScope(false))
                        {
                            if (GUILayout.Button("StopSession", GUILayout.MinWidth(minWidth)))
                            {
                                workflow.StopSession(false);
                            }
                        }
                        using (_ = workflow.CanStopSession ? new EnableScope() : new EnableScope(false))
                        {
                            if (GUILayout.Button("StopSession (keep image)", GUILayout.MinWidth(minWidth)))
                            {
                                workflow.StopSession(true);
                            }
                        }
                    }

                    using (_ = new GUILayout.VerticalScope())
                    {
                        GUILayout.Label("──>");
                        GUILayout.Label("┘  ");
                    }

                    using (_ = new GUILayout.VerticalScope())
                    {
                        using (_ = workflow.CanDeinitialize ? new EnableScope() : new EnableScope(false))
                        {
                            if (GUILayout.Button("Deinitialize", GUILayout.MinWidth(minWidth)))
                            {
                                workflow.Deinitialize();
                            }
                        }
                    }

                    GUILayout.FlexibleSpace();
                }
            }
        }

        private static void DrawSessionDetails(ARSession session, ToolCache cache, bool isValidateFrame)
        {
            if (!session) { return; }
            GUILayout.Label("Session Assembly");
            using (_ = new GUILayout.VerticalScope(GUI.skin.box))
            {
                session.enabled = GUILayout.Toggle(session.enabled, "AR Session");
                if (session.Assembly != null)
                {
                    if (session.Assembly.CameraImageRenderer.OnSome && session.Assembly.CameraImageRenderer.Value)
                    {
                        session.Assembly.CameraImageRenderer.Value.enabled = GUILayout.Toggle(session.Assembly.CameraImageRenderer.Value.enabled, "Image Rendereer");
                    }
                    if (session.Assembly.Camera)
                    {
                        session.Assembly.Camera.enabled = GUILayout.Toggle(session.Assembly.Camera.enabled, "Camera: " + session.Assembly.Camera.name);
                    }
                    if (session.Assembly.FrameSource)
                    {
                        if (session.Assembly.FrameSource is FramePlayer)
                        {
                            using (_ = new EnableScope(false))
                            {
                                GUILayout.Toggle(session.Assembly.FrameSource.enabled, "Frame Source: Frame Player");
                            }
                        }
                        else
                        {
                            session.Assembly.FrameSource.enabled = GUILayout.Toggle(session.Assembly.FrameSource.enabled, "Frame Source: " + session.Assembly.FrameSource.name);
                        }
                    }
                    foreach (var filter in session.Assembly.FrameFilters)
                    {
                        if (filter)
                        {
                            filter.enabled = GUILayout.Toggle(filter.enabled, "Frame Filter: " + filter.name);
                        }
                    }
                    if (session.Assembly.FrameRecorder.OnSome && session.Assembly.FrameRecorder.Value)
                    {
                        session.Assembly.FrameRecorder.Value.enabled = GUILayout.Toggle(session.Assembly.FrameRecorder.Value.enabled, "Frame Recorder");
                    }
                    GUILayout.Label("Available Center Mode");
                    foreach (var mode in session.Assembly.AvailableCenterMode)
                    {
                        GUILayout.Label($"    {mode}");
                    }
                }
            }

            if (session.Report != null)
            {
                EditorGUILayout.Space(10);
                GUILayout.Label($"Session Report (Editor{(isValidateFrame ? ", force FramePlayer" : "")})");

                using (var scroll = new GUILayout.ScrollViewScope(cache.ScrollPos, GUI.skin.box, GUILayout.MinHeight(40), GUILayout.MaxHeight(cache.IsWindow ? 150 : 100), GUILayout.ExpandHeight(true)))
                {
                    cache.ScrollPos = scroll.scrollPosition;
                    if (session.Report.BrokenReason.OnSome)
                    {
                        GUILayout.Label($"Broken reason: {session.Report.BrokenReason}");
                        if (session.Report.Exception != null)
                        {
                            using (_ = new GUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button("❐", GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    GUIUtility.systemCopyBuffer = session.Report.Exception.ToString();
                                }
                                GUILayout.Label(session.Report.Exception.ToString());
                            }
                        }
                    }
                    if (session.Report.Availability != null)
                    {
                        GUILayout.Label("Availability report:");
                        var report = string.Empty;
                        foreach (var item in session.Report.Availability.FrameSources.Concat(session.Report.Availability.FrameFilters))
                        {
                            report += $"{ARSessionFactory.DefaultName(item.Component.GetType())}: {item.Availability}" + Environment.NewLine;
                        }
                        using (_ = new GUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("❐", GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                GUIUtility.systemCopyBuffer = report;
                            }
                            GUILayout.Label(report, new GUIStyle(GUI.skin.label) { wordWrap = true });
                        }
                    }
                }
            }
        }

        public static bool CheckForUpdate(ARSession session)
        {
            if (!EditorApplication.isPlaying || !session) { return false; }
            if (sessionState != session.State)
            {
                sessionState = session.State;
                return true;
            }
            var player = session.GetComponent<FramePlayer>();
            if (player && playerTime != player.Time)
            {
                playerTime = player.Time;
                return true;
            }
            return false;
        }

        private void OnPlayModeChange(PlayModeStateChange s)
        {
            if (s == PlayModeStateChange.EnteredEditMode || s == PlayModeStateChange.EnteredPlayMode)
            {
                SetSession(EditorUtility.InstanceIDToObject(sessionInstanceId) as ARSession);
            }
        }

        private void SetSession(ARSession session)
        {
            if ((object)session == null)
            {
                if ((object)Session != null)
                {
                    ClearSession();
                }
                return;
            }
            if (!session) { return; }
            if (!session.gameObject.scene.IsValid()) { return; }
            var diag = session.Diagnostics;
            if (!diag) { return; }
            if (!diag.IsValidationOn || diag.IsExternalTool)
            {
                ClearSession();
                return;
            }
            if (Session == session && (bool)Session == (bool)session) { return; }

            ClearSession();
            Session = session;
            sessionInstanceId = session.GetInstanceID();
            cache = new ToolCache(true);
            diag.ValidationWindow = this;
            DiagnosticsControllerEditor.Repaint(diag);

            Repaint();
        }

        private void ClearSession()
        {
            if (Session)
            {
                var diag = Session.Diagnostics;
                if (diag)
                {
                    diag.ValidationWindow = null;
                    DiagnosticsControllerEditor.Repaint(diag);
                }
            }
            Session = null;
            sessionInstanceId = 0;
            cache = new ToolCache(true);
            playerTime = default;
            sessionState = default;
            Repaint();
        }


        public class ToolCache
        {
            public bool IsWindow;
            public Vector2 ScrollPos;
            public int InitializeIndex;
            public string CustomLicense;

            public ToolCache(bool isWin)
            {
                IsWindow = isWin;
            }
        }
    }
}
