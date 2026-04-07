//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Diagnostics controller for current <see cref="ARSession"/>. Let it be your develop partner, not the first component to disable.</para>
    /// <para xml:lang="zh">当前<see cref="ARSession"/>的诊断控制器。让它成为你的开发伙伴而非第一个关闭的组件。</para>
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class DiagnosticsController : MonoBehaviour
    {
        /// <summary>
        /// <para xml:lang="en">Developer mode switch. You can use the default switch (tap screen 8 times) or define your custom switch or equivalent function to replace develop mode.</para>
        /// <para xml:lang="en">If you silently disable developer mode and do not provide a replacement, issue feedbacks to EasyAR may be rejected, expecially for Mega users.</para>
        /// <para xml:lang="zh">开发者模式开关，你可以使用默认的开关（点击屏幕8次触发）或自定义一个开关，或提供开发者模式的等价替代。</para>
        /// <para xml:lang="zh">如果你默默地关闭开发模式且没有提供替代，后续给到EasyAR的问题反馈将可能被拒绝，尤其是对Mega用户而言非常重要。</para>
        /// </summary>
        [Tooltip("Developer mode switch. You can use the default switch or define your custom switch or equivalent function to replace develop mode.\nIf you silently disable developer mode and do not provide a replacement, issue feedbacks to EasyAR may be rejected, expecially for Mega users.\n\n开发者模式开关，你可以使用默认的开关（点击屏幕8次触发）或自定义一个开关，或提供开发者模式的等价替代。\n如果你默默地关闭开发者模式且没有提供替代，后续给到EasyAR的问题反馈将可能被拒绝，尤其是对Mega用户而言非常重要。")]
        public DeveloperModeSwitchType DeveloperModeSwitch;

        /// <summary>
        /// <para xml:lang="en">Message output options.</para>
        /// <para xml:lang="zh">消息输出选项。</para>
        /// </summary>
        public MessageOutputOptions MessageOutput = new();

        /// <summary>
        /// <para xml:lang="en">Custom developer mode switch when <see cref="DeveloperModeSwitch"/> is <see cref="DeveloperModeSwitchType.Custom"/>. The system will switch developer mode on/off depending on function return at any time.</para>
        /// <para xml:lang="zh"><see cref="DeveloperModeSwitch"/>为<see cref="DeveloperModeSwitchType.Custom"/>时的自定义的开发者模式开关。系统会根据函数返回随时开关开发者模式。</para>
        /// </summary>
        public Func<bool> CustomDeveloperModeSwitch;

        [SerializeField, HideInInspector]
        internal bool IsValidateFrame;
        [SerializeField, HideInInspector]
        internal bool IsValidateWorkflow = true;
        internal ScriptableObject ValidationWindow;
        internal bool IsExternalTool;

        private Optional<bool> isHMD;
        private GameObject sessionOrigin;
        private ARSession session;
        private DiagnosticsMessageDispatcher messageDispatcher;
        private string dumpMessage;
        private float devCheckTime;
        private int devCheckCount;
        private Func<bool> defaultDeveloperModeSwitch;
        private GameObject devCanvas;
        private DeveloperCache developerCache;
        private DiagnosticsPanel diagnosticsPanel;
        private bool hasSenseErrorDisplayed;
        private bool hasSessionErrorDisplayed;
        private bool hasCaution;

        /// <summary>
        /// <para xml:lang="en">Message output mode.</para>
        /// <para xml:lang="zh">消息输出模式。</para>
        /// </summary>
        public enum MessageOutputMode
        {
            /// <summary>
            /// <para xml:lang="en">Output to UI and log. Display 5 meters in front of eye on head mounted displays.</para>
            /// <para xml:lang="zh">输出到UI和日志。在头戴设备上显示在眼前5米处。</para>
            /// </summary>
            UIAndLog,
            /// <summary>
            /// <para xml:lang="en">Output to system log.</para>
            /// <para xml:lang="zh">输出到系统日志。</para>
            /// </summary>
            Log,
        }

        /// <summary>
        /// <para xml:lang="en">Session dump status output mode.</para>
        /// <para xml:lang="zh">会话状态转储输出模式。</para>
        /// </summary>
        public enum SessionDumpOutputMode
        {
            /// <summary>
            /// <para xml:lang="en">Display on UI and update each frame. Display 5 meters in front of eye on head mounted displays.</para>
            /// <para xml:lang="zh">显示在UI并每帧更新。在头戴设备上，显示在眼前5米处。</para>
            /// </summary>
            UI,
            /// <summary>
            /// <para xml:lang="en">Output to system log. It may have impact on performance due to log output each frame, suggest using when develop or test.</para>
            /// <para xml:lang="zh">输出到系统日志，由于每帧都输出，对运行性能是有影响的，建议在开发或测试时使用。</para>
            /// </summary>
            Log,
            /// <summary>
            /// <para xml:lang="en">Do not output.</para>
            /// <para xml:lang="zh">不输出。</para>
            /// </summary>
            None,
        }

        /// <summary>
        /// <para xml:lang="en">Developer mode switch type.</para>
        /// <para xml:lang="zh">开发者模式开关类型。</para>
        /// </summary>
        public enum DeveloperModeSwitchType
        {
            /// <summary>
            /// <para xml:lang="en">Default. The developer mode will be turned on/off by tapping screen 8 times. Only works on desktop and mobile phones.</para>
            /// <para xml:lang="zh">默认。开发者模式将会在点击屏幕8次后开启/关闭。仅在桌面和手机上有效。</para>
            /// </summary>
            Default,
            /// <summary>
            /// <para xml:lang="en">Custom.</para>
            /// <para xml:lang="zh">自定义</para>
            /// </summary>
            Custom,
        }


        /// <summary>
        /// <para xml:lang="en">Is developer mode on.</para>
        /// <para xml:lang="zh">开发者模式是否打开。</para>
        /// </summary>
        public bool IsDevMode { get; private set; }
        internal bool IsValidationOn => IsValidateFrame || IsValidateWorkflow;
        internal Optional<bool> IsRenderPipelineProperlySetup;
        internal string RenderPipelineSetupError;
        private DiagnosticsMessageDispatcher MessageDispatcherNullable => messageDispatcher ? messageDispatcher : null;

        private void Awake()
        {
            if (!Application.isPlaying) { return; }
            messageDispatcher = new GameObject("MessageDispatcher").AddComponent<DiagnosticsMessageDispatcher>();
            if (SystemUtil.RenderPipeline == SystemUtil.RenderPipelineType.URP)
            {
                RenderPipelineManager.beginContextRendering += CheckURP;
            }
            else
            {
                IsRenderPipelineProperlySetup = true;
            }
        }

        private void Start()
        {
            if (!Application.isPlaying) { return; }
            session = GetComponent<ARSession>();
            if (!session)
            {
                Debug.LogError($"{nameof(DiagnosticsController)} is not attached to a session!");
                return;
            }
            if (session.State >= ARSession.SessionState.Ready)
            {
                OnSessionReady();
            }
            else
            {
                session.StateChanged += (state) =>
                {
                    if (state == ARSession.SessionState.Ready)
                    {
                        OnSessionReady();
                    }
                };
            }
            defaultDeveloperModeSwitch = () =>
            {
#if UNITY_VISIONOS
                //Temporarily disable default developer mode entrance because it's sometimes too easy to trigger on Apple vision pro.
                return false;
#endif
                if (session.Assembly != null && session.Assembly.FrameSource && session.Assembly.FrameSource.IsHMD) { return false; }

                if (GetClick())
                {
                    var time = Time.realtimeSinceStartup;
                    devCheckCount = time - devCheckTime < 0.3f ? devCheckCount + 1 : 0;
                    devCheckTime = time;
                    if (devCheckCount == 4 && !IsDevMode)
                    {
                        EnqueueMessage("You are now 4 steps away from being a developer.", 1);
                    }
                    if (devCheckCount == 8)
                    {
                        EnqueueMessage(!IsDevMode ? "You are now a developer." : "You are not a developer anymore.", 3);
                        return !IsDevMode;
                    }
                }
                return IsDevMode;
            };
#if UNITY_EDITOR
            if (Application.isEditor && IsValidateFrame && !IsExternalTool)
            {
                UnityEditor.EditorUtility.DisplayDialog("Notice",
                    $"Session Validation Tool (Editor only) is running with frame player instead of your application frame source. Please disable 'Frame Player' in validation tool from {nameof(DiagnosticsController)} if you want to run your own app." + Environment.NewLine +
                    Environment.NewLine +
                    $"Session Validation Tool（仅编辑器有效）在使用frame player而不是你应用的FrameSource在运行。如果需要运行你自己的应用，请在{nameof(DiagnosticsController)}中关闭验证工具的'Frame Player'选项。"
                    , "OK");
            }
#endif
        }

#if UNITY_EDITOR
        private void Update()
        {
            CheckARFoundation();
        }
#endif

        private void LateUpdate()
        {
            if (!Application.isPlaying) { return; }
            if (MessageOutput.SessionDump < SessionDumpOutputMode.None)
            {
                var message = DumpSessionLite();
                if (MessageOutput.SessionDump == SessionDumpOutputMode.UI)
                {
                    MessageDispatcherNullable?.GetDumpHandler(isHMD)?.Invoke(message);
                    dumpMessage = message;
                }
                else if (MessageOutput.SessionDump == SessionDumpOutputMode.Log)
                {
                    StopDisplayDumpMessage();
                    Debug.Log(message);
                }
            }
            else
            {
                StopDisplayDumpMessage();
            }

            DetectDeveloperMode();
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying) { return; }
            ExitDeveloperMode();
            if (messageDispatcher && messageDispatcher.gameObject) { Destroy(messageDispatcher.gameObject); }
            if (devCanvas) { Destroy(devCanvas); }
            RenderPipelineManager.beginContextRendering -= CheckURP;
        }

        internal string DumpSessionLite()
        {
            var data = $"{UnityPackage.DisplayName} Version {UnityPackage.Version}" + Environment.NewLine +
                $"{Engine.name()} Version {Engine.versionString()}" + Environment.NewLine;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || SystemUtil.IsVisionOS())
            {
                data += $"Platform: {SystemInfo.operatingSystem}, {SystemUtil.DeviceModel}, {SystemInfo.graphicsDeviceType} ({SystemUtil.RenderPipeline})" + Environment.NewLine;
            }
            else
            {
                data += $"Platform: {SystemInfo.operatingSystem}{(Application.isEditor ? "(Editor)" : string.Empty)}, {SystemInfo.graphicsDeviceType} ({SystemUtil.RenderPipeline})" + Environment.NewLine;
            }
            if (!session) { return data; }
            data += session.DumpLite();
            if (session.Assembly != null)
            {
                if (session.Assembly.FrameSource)
                {
                    data += session.Assembly.FrameSource.DumpLite();
                }
                if (session.Assembly.FrameRecorder.OnSome)
                {
                    data += session.Assembly.FrameRecorder.Value.DumpLite();
                }
                if (session.Assembly.FrameFilters != null)
                {
                    foreach (var filter in session.Assembly.FrameFilters)
                    {
                        var fd = filter.DumpLite();
                        if (!string.IsNullOrEmpty(fd))
                        {
                            data += filter.DumpLite();
                        }
                    }
                }
            }
            return data;
        }

        internal void EnqueueCaution(string message)
        {
            Debug.LogWarning(message);
            MessageDispatcherNullable?.GetCautionHandler(isHMD, GetActiveOrigin())?.Invoke(message);
            hasCaution = true;
        }

        internal void EnqueueWarning(string message, float time = 5)
        {
            Debug.LogWarning(message);
            if (MessageOutput.Warning <= MessageOutputMode.UIAndLog)
            {
                MessageDispatcherNullable?.GetMessageHandler(DiagnosticsMessageType.Warning, isHMD, GetActiveOrigin())?.Invoke(message, time);
            }
        }

        internal void EnqueueError(string message)
        {
            Debug.LogError(message);
            if (MessageOutput.Error <= MessageOutputMode.UIAndLog)
            {
                MessageDispatcherNullable?.GetMessageHandler(DiagnosticsMessageType.Error, isHMD, GetActiveOrigin())?.Invoke(message, 8);
            }
        }

        internal void EnqueueSenseError(string message)
        {
            EnqueueFatalError(message, true, MessageOutput.SenseError, MessageOutput.ShowEditorDialogOnFatal);
        }

        internal void EnqueueSessionError(string message)
        {
            EnqueueFatalError(message, false, MessageOutput.SessionError, MessageOutput.ShowEditorDialogOnFatal);
        }

        private void EnqueueFatalError(string message, bool isLowLevel, MessageOutputMode response, bool showEditorDialogOnFatal)
        {
            Debug.LogError(message);
            if (response <= MessageOutputMode.UIAndLog)
            {
                var origin = GetActiveOrigin();
                MessageDispatcherNullable?.GetMessageHandler(DiagnosticsMessageType.FatalError, isHMD, origin)?.Invoke(message, isLowLevel ? 10000 : 60);
                if (isLowLevel)
                {
                    hasSenseErrorDisplayed = true;
                }
                else
                {
                    hasSessionErrorDisplayed = true;
                }
#if UNITY_EDITOR
                if (showEditorDialogOnFatal)
                {
                    UnityEditor.EditorUtility.DisplayDialog("Fatal Error", message, "OK");
                }
#endif
            }
        }


        internal void RecoverFromSenseError()
        {
            if (!hasSenseErrorDisplayed) { return; }
            RecoverFromFatalError();
        }

        internal void RecoverFromFatalError()
        {
            if (!hasSenseErrorDisplayed && !hasSessionErrorDisplayed) { return; }
            MessageDispatcherNullable?.GetMessageCleaner(DiagnosticsMessageType.FatalError, isHMD)?.Invoke();
            hasSenseErrorDisplayed = false;
            hasSessionErrorDisplayed = false;
        }

        internal void ClearCaution()
        {
            if (!hasCaution) { return; }
            MessageDispatcherNullable?.GetCautionCleaner(isHMD)?.Invoke();
            hasCaution = false;
        }

        internal static DiagnosticsController TryGetDiagnosticsController(GameObject go)
        {
            if (!go) { return null; }
            var session = go.GetComponentInParent<ARSession>(true);
            if (!session)
            {
                session = ARSession.Current;
            }
            if (!session)
            {
                return null;
            }
            return session.Diagnostics;
        }

        internal static void TryShowDiagnosticsError(GameObject go, string error)
        {
            var diagnostics = TryGetDiagnosticsController(go);
            if (diagnostics)
            {
                diagnostics.EnqueueError(error);
            }
            else
            {
                Debug.LogError(error);
            }
        }

        private void OnSessionReady()
        {
            isHMD = session.Assembly.FrameSource.IsHMD;
            sessionOrigin = session.Origin;

            if (!messageDispatcher)
            {
                messageDispatcher = new GameObject("MessageDispatcher").AddComponent<DiagnosticsMessageDispatcher>();
            }
            MessageDispatcherNullable?.Setup(isHMD.Value);
            MessageDispatcherNullable?.transform.SetParent(session.Assembly.Camera.transform, false);

            SetupDeveloperModeRecorderButtons(developerCache?.UI.EifButton, developerCache?.UI.EedButton, developerCache?.UI.EifFormatButton);
        }

        private void StopDisplayDumpMessage()
        {
            if (string.IsNullOrEmpty(dumpMessage)) { return; }
            MessageDispatcherNullable?.GetDumpHandler(isHMD)?.Invoke(null);
            dumpMessage = null;
        }

        private GameObject GetActiveOrigin()
        {
            if (sessionOrigin && sessionOrigin.activeInHierarchy) { return sessionOrigin; }
            return null;
        }

        private void DetectDeveloperMode()
        {
            var mode = IsDevMode;
            IsDevMode = (DeveloperModeSwitch == DeveloperModeSwitchType.Default ? defaultDeveloperModeSwitch?.Invoke() : CustomDeveloperModeSwitch?.Invoke()) ?? mode;
            if (IsDevMode != mode)
            {
                if (IsDevMode)
                {
                    EnterDeveloperMode();
                }
                else
                {
                    ExitDeveloperMode();
                }
            }
        }

        private void EnterDeveloperMode()
        {
            if (EventSystem.current == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
            if (!devCanvas)
            {
                (devCanvas, diagnosticsPanel) = DiagnosticsPanel.CreatePanel();
                SetupPanel();
            }

            devCanvas.SetActive(true);
            developerCache = new DeveloperCache
            {
                RawMessageOutput = new MessageOutputOptions
                {
                    SessionDump = MessageOutput.SessionDump,
                    SenseError = MessageOutput.SenseError,
                    SessionError = MessageOutput.SessionError,
                    Error = MessageOutput.Error,
                    Warning = MessageOutput.Warning,
                },
                UI = diagnosticsPanel ? new DeveloperUICache
                {
                    SessionToggle = diagnosticsPanel.SessionToggle,
                    EifButton = diagnosticsPanel.EifButton,
                    EifFormatButton = diagnosticsPanel.EifFormatButton,
                    EedButton = diagnosticsPanel.EedButton,
                } : null,
            };
            MessageOutput = new MessageOutputOptions();
            developerCache.UI.SessionToggle.isOn = true;
            if (session && session.Assembly != null && session.Assembly.FrameRecorder.OnSome && session.Assembly.FrameRecorder.Value)
            {
                SetupEIFButton(developerCache.UI.EifButton, developerCache.UI.EifFormatButton, session.Assembly.FrameRecorder.Value);
                SetupDeveloperModeRecorder(developerCache, session.Assembly.FrameRecorder.Value);
            }
        }

        private void ExitDeveloperMode()
        {
            if (developerCache == null) { return; }
            if (devCanvas) { devCanvas.SetActive(false); }
            MessageOutput = developerCache.RawMessageOutput;
            if (session && session.Assembly != null && session.Assembly.FrameRecorder.OnSome && session.Assembly.FrameRecorder.Value)
            {
                var recorder = session.Assembly.FrameRecorder.Value;
                if (developerCache.Recorder != null)
                {
                    recorder.AutoStart = developerCache.Recorder.AutoStart;
                    recorder.Configuration.AutoFilePath = developerCache.Recorder.AutoFilePath;
                    recorder.Configuration.Format = developerCache.Recorder.Format;
                }
                if (developerCache.RecorderChanged)
                {
                    recorder.enabled = false;
                }
            }
            if (developerCache.eedRecorder != null)
            {
                RecordEED(false, developerCache);
            }
            developerCache = null;
        }

        private void SetupPanel()
        {
            diagnosticsPanel.SessionToggle.onValueChanged.AddListener((on) =>
            {
                MessageOutput.SessionDump = on ? SessionDumpOutputMode.UI : SessionDumpOutputMode.None;
            });
            diagnosticsPanel.SessionCopyButton.onClick.AddListener(() =>
            {
                GUIUtility.systemCopyBuffer = DumpSessionLite();
                EnqueueMessage($"session info copied to clipboard", 2);
            });
            SetupDeveloperModeRecorderButtons(diagnosticsPanel.EifButton, diagnosticsPanel.EedButton, diagnosticsPanel.EifFormatButton);
        }

        private void SetupDeveloperModeRecorderButtons(Button eifButton, Button eedButton, Button eifDropdown)
        {
            if (!eifButton || !eedButton || !eifDropdown) { return; }
            if (!IsDevMode) { return; }
            if (session.State < ARSession.SessionState.Ready) { return; }
            if (session.Assembly == null || session.Assembly.Display == null) { return; }

            eedButton.interactable = true;
            eedButton.onClick.AddListener(() =>
            {
                if (developerCache == null) { return; }
                RecordEED(eedButton.GetComponentInChildren<Text>().text == "rec", developerCache);
            });

            if (session.Assembly.FrameRecorder.OnNone || !session.Assembly.FrameRecorder.Value) { return; }
            var recorder = session.Assembly.FrameRecorder.Value;

            recorder.OnReady.AddListener(() =>
            {
                if (!this || !IsDevMode) { return; }
                SetupEIFButton(eifButton, eifDropdown, recorder);
            });
            recorder.OnRecording.AddListener((file) =>
            {
                if (!this || !IsDevMode) { return; }
                EnqueueMessage($"Recording {file}", 5);
                GUIUtility.systemCopyBuffer = file;
                SetupEIFButton(eifButton, eifDropdown, recorder);
            });
            recorder.OnFinish.AddListener((status) =>
            {
                if (!this || !IsDevMode) { return; }
                EnqueueMessage($"Recording finished with status {status}", 5);
                SetupEIFButton(eifButton, eifDropdown, recorder);
            });
            eifButton.onClick.AddListener(() =>
            {
                if (developerCache != null)
                {
                    developerCache.RecorderChanged = true;
                }
                recorder.enabled = eifButton.GetComponentInChildren<Text>().text == "rec";
            });
            eifDropdown.onClick.AddListener(() =>
            {
                var formats = new FrameRecorder.InternalFormat[] { FrameRecorder.InternalFormat.Auto }.Concat(recorder.AvailableFormats).ToList();
                var format = recorder.Configuration.Format;
                var index = 0;
                foreach (var f in formats)
                {
                    ++index;
                    if (f == format) { break; }
                }
                index %= formats.Count;
                format = formats[index];
                recorder.Configuration.Format = format;
                eifDropdown.GetComponentInChildren<Text>().text = format.ToString();
            });

            SetupEIFButton(eifButton, eifDropdown, recorder);
            if (developerCache != null)
            {
                SetupDeveloperModeRecorder(developerCache, recorder);
            }
        }

        private void RecordEED(bool on, DeveloperCache cache)
        {
            if (on)
            {
                if (session.Assembly == null || session.Assembly.Display == null) { return; }
                var path = Application.persistentDataPath + "/" + "dump_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.fff") + ".eed";
                EnqueueMessage($"Recording {path}", 5);
                cache.eedRecorder = EventDumpRecorder.create();
                cache.eedRecorder.start(path, session.Assembly.Display.Rotation);
                GUIUtility.systemCopyBuffer = path;
            }
            else
            {
                if (this && gameObject.activeInHierarchy)
                {
                    EnqueueMessage($"Recording finished", 5);
                }
                cache.eedRecorder.stop();
                cache.eedRecorder.Dispose();
                cache.eedRecorder = null;
            }
            if (cache.UI != null)
            {
                cache.UI.EedButton.GetComponentInChildren<Text>().text = on ? "stop" : "rec";
            }
        }

        private static void SetupEIFButton(Button eifButton, Button eifDropdown, FrameRecorder recorder)
        {
            if (!eifButton || !eifDropdown || !recorder) { return; }
            eifButton.interactable = recorder.Status != FrameRecorder.RecorderStatus.Unknown && recorder.Status != FrameRecorder.RecorderStatus.Starting;
            eifButton.GetComponentInChildren<Text>().text = recorder.Status == FrameRecorder.RecorderStatus.Recording || recorder.Status == FrameRecorder.RecorderStatus.RecordingWithoutSupplement ? "stop" : "rec";

            var formatText = recorder.RecordingFormat.ToString();
            if (recorder.Status <= FrameRecorder.RecorderStatus.Ready)
            {
                formatText = string.Empty;
                foreach (var f in new FrameRecorder.InternalFormat[] { FrameRecorder.InternalFormat.Auto }.Concat(recorder.AvailableFormats))
                {
                    if (f == recorder.Configuration.Format) { formatText = f.ToString(); }
                }
                if (string.IsNullOrEmpty(formatText))
                {
                    formatText = "[E]" + recorder.Configuration.Format.ToString();
                }
            }
            eifDropdown.GetComponentInChildren<Text>().text = formatText;
            eifDropdown.interactable = recorder.Status <= FrameRecorder.RecorderStatus.Ready;
        }

        private static void SetupDeveloperModeRecorder(DeveloperCache developerCache, FrameRecorder recorder)
        {
            developerCache.Recorder = new DeveloperCache.RecorderCache
            {
                AutoStart = recorder.AutoStart,
                AutoFilePath = recorder.Configuration.AutoFilePath,
                Format = recorder.Configuration.Format,
            };
            recorder.AutoStart = false;
            recorder.Configuration.AutoFilePath = true;
        }

        private void EnqueueMessage(string message, float time)
        {
            Debug.Log(message);
            MessageDispatcherNullable?.GetMessageHandler(DiagnosticsMessageType.Info, isHMD, GetActiveOrigin())?.Invoke(message, time);
        }

        private void CheckARFoundation()
        {
            if (Application.isPlaying) { return; }
            if (EasyARSettings.Instance && !(EasyARSettings.Instance.UnityXR?.UnityXRAutoSwitch?.Editor?.DisableARSession ?? true)) { return; }
            UnityXRSwitcher.DisableARSession();
        }

        private bool GetClick()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.touches.Where(t => t.phase == UnityEngine.TouchPhase.Began).Any()
                || Input.GetMouseButtonDown(0))
            {
                return true;
            }
#endif

#if ENABLE_INPUT_SYSTEM && EASYAR_ENABLE_INPUTSYSTEM
            if ((UnityEngine.InputSystem.Touchscreen.current != null && UnityEngine.InputSystem.Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                || (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame))
            {
                return true;
            }
#endif
            return false;
        }

        private void CheckURP(ScriptableRenderContext context, List<Camera> cameras)
        {
            RenderPipelineManager.beginContextRendering -= CheckURP;
            IsRenderPipelineProperlySetup = EasyARCameraImageRendererFeature.IsActive;
            if (!EasyARCameraImageRendererFeature.IsActive)
            {
                RenderPipelineSetupError = "URP RenderPipelineAsset not properly setup!" + Environment.NewLine +
                    Environment.NewLine +
                    $"{typeof(EasyARCameraImageRendererFeature)} not found in current ScriptableRenderer." + Environment.NewLine +
                    $"Please add {typeof(EasyARCameraImageRendererFeature)} to your of RenderPipelineAsset in Editor." + Environment.NewLine +
                    "Please refer to both EasyAR Sense Unity Plugin URP configuration documents and Unity URP documents for details.";
            }
        }

        /// <summary>
        /// <para xml:lang="en">Message output options.</para>
        /// <para xml:lang="zh">消息输出选项。</para>
        /// </summary>
        [Serializable]
        public class MessageOutputOptions
        {
            /// <summary>
            /// <para xml:lang="en">Session dump status output mode.</para>
            /// <para xml:lang="zh">会话状态转储输出方式。</para>
            /// </summary>
            public SessionDumpOutputMode SessionDump;
            /// <summary>
            /// <para xml:lang="en">Sense Error output mode. It is usually related to EasyAR Sense license.</para>
            /// <para xml:lang="zh">Sense Error输出方式，通常与EasyAR Sense license有关。</para>
            /// </summary>
            public MessageOutputMode SenseError;
            /// <summary>
            /// <para xml:lang="en">Session Error output mode. It is usually related not supported feature on device or wrong configuration.</para>
            /// <para xml:lang="zh">Session Error输出方式，通常与设备不支持一些功能或错误的配置有关。</para>
            /// </summary>
            public MessageOutputMode SessionError;
            /// <summary>
            /// <para xml:lang="en">Error output mode.</para>
            /// <para xml:lang="zh">Error输出方式。</para>
            /// </summary>
            public MessageOutputMode Error;
            /// <summary>
            /// <para xml:lang="en">Warning output mode.</para>
            /// <para xml:lang="zh">Warning输出方式。</para>
            /// </summary>
            public MessageOutputMode Warning;
            /// <summary>
            /// <para xml:lang="en">Show dialog when Sense Error or Session Error in Editor.</para>
            /// <para xml:lang="zh">编辑器中，Sense Error或Session Error时显示对话框。</para>
            /// </summary>
            public bool ShowEditorDialogOnFatal = true;
        }

        private class DeveloperCache
        {
            public MessageOutputOptions RawMessageOutput;
            public bool RecorderChanged;
            public EventDumpRecorder eedRecorder;
            public DeveloperUICache UI;
            public RecorderCache Recorder;

            public class RecorderCache
            {
                public bool AutoStart;
                public bool AutoFilePath;
                public FrameRecorder.InternalFormat Format;
            }
        }

        private class DeveloperUICache
        {
            public Button EifButton;
            public Button EifFormatButton;
            public Button EedButton;
            public Toggle SessionToggle;
        }
    }

    internal class DiagnosticsMessageException : Exception
    {
        internal DiagnosticsMessageException(string message) : base(message) { }
        public override string ToString() => Message;
    }
}
