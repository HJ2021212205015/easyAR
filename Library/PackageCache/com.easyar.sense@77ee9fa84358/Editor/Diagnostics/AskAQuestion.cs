//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace easyar
{
    class AskAQuestion : EditorWindow
    {
        bool opened;
        string lang;
        Vector2 leftScrollPos;
        Vector2 leftScrollPos2;
        Vector2 rightScrollPos;
        string deviceInfo = string.Empty;
        string easyarInfo;
        string hostInfo;
        bool isHostWin = false;
        bool isDoneSample;
        bool isDoneDocument;
        bool isDoneLog;
        bool isDoneLatest;
        bool isDoneAll;
        bool isSampleSimple;
        string sampleName;
        Optional<string> packageListEasyAR;
        Optional<string> packageListUnityXR;
        Optional<string> packageListOther;
        string engineName;
        string engineVersion;
        readonly bool[] features = new bool[(int)Feature.None];
        readonly bool[] platforms = new bool[(int)PlatformType.None];
        ListRequest listRequest;
        PlatformType platform = PlatformType.None;

        enum PlatformType
        {
            Editor_Win,
            Editor_Mac,
            Windows,
            MacOS,
            Android,
            iOS,
            AndroidHMD,
            Other,
            WebGL,
            Hololens,
            None,
        }

        enum Feature
        {
            ImageTracking,
            ObjectTracking,
            CloudRecognition,
            MotionTracking,
            SurfaceTracking,
            VideoRecording,
            SparseSpatialMap,
            DenseSpatialMap,
            Mega,
            Other,
            None,
        }

        [MenuItem("EasyAR/Sense/Ask a Question", priority = 100)]
        private static void DocumentQaAEn()
        {
            var win = GetWindow<AskAQuestion>(true, "", true);
            if (!win.opened)
            {
                win.minSize = new Vector2(1100, 720);
                win.opened = true;
            }
            win.lang = "en";
            win.titleContent = new GUIContent("Ask a Question");
        }

        [MenuItem("EasyAR/Sense/提问", priority = 100)]
        private static void DocumentQaAZh()
        {
            var win = GetWindow<AskAQuestion>(true, "", true);
            if (!win.opened)
            {
                win.minSize = new Vector2(1100, 720);
                win.opened = true;
            }
            win.lang = "zh";
            win.titleContent = new GUIContent("提问");
        }

        private void OnEnable()
        {
#if UNITY_EDITOR_WIN
            isHostWin = true;
#endif

            if (packageListEasyAR.OnNone && listRequest == null)
            {
                listRequest = Client.List(true, true);
            }

            try
            {
                engineName = Engine.name();
                engineVersion = Engine.versionString();
            }
            catch (Exception)
            {
                engineName = "<Fail to get>";
                engineVersion = "<Fail to get>";
            }

            easyarInfo = $"{UnityPackage.DisplayName} ({UnityPackage.Version})" + Environment.NewLine +
                $"EasyAR Sense ({engineVersion})";

            hostInfo = SystemInfo.operatingSystem + Environment.NewLine +
                engineName + Environment.NewLine +
                $"Unity {Application.unityVersion}" + Environment.NewLine +
                $"Pipeline: {SystemUtil.RenderPipeline}";
        }

        private void OnGUI()
        {
            if (packageListEasyAR.OnNone)
            {
                if (listRequest == null || (listRequest.IsCompleted && listRequest.Status != StatusCode.Success))
                {
                    packageListEasyAR = "Error list packages";
                }
                if (listRequest.IsCompleted && listRequest.Status == StatusCode.Success)
                {
                    packageListEasyAR = string.Empty;
                    foreach (var p in listRequest.Result.Where(p => p.name.StartsWith("com.easyar")))
                    {
                        packageListEasyAR += Environment.NewLine + $"{p.name}-{p.version} ({p.source})";
                    }
                    packageListUnityXR = string.Empty;
                    foreach (var p in listRequest.Result.Where(p => p.name.StartsWith("com.unity.xr")))
                    {
                        packageListUnityXR += Environment.NewLine + $"{p.name}-{p.version} ({p.source})";
                    }
                    packageListOther = string.Empty;
                    foreach (var p in listRequest.Result.Where(p => !p.name.StartsWith("com.easyar") && !p.name.StartsWith("com.unity.xr")))
                    {
                        packageListOther += Environment.NewLine + $"{p.name}-{p.version} ({p.source})";
                    }
                }
            }
            var essentialInfo = "EasyAR Products:" + Environment.NewLine +
                easyarInfo + Environment.NewLine +
                Environment.NewLine +
                "Development Environment:" + Environment.NewLine +
                hostInfo + Environment.NewLine +
                Environment.NewLine +
                $"EasyAR Packages: " + packageListEasyAR + Environment.NewLine +
                Environment.NewLine +
                $"Unity XR Packages: " + packageListUnityXR + Environment.NewLine +
                Environment.NewLine +
                $"Ohter Packages: " + packageListOther + Environment.NewLine +
                Environment.NewLine +
                "Runtime Environment:" + Environment.NewLine +
                $"Platform: {platform}" + Environment.NewLine;

            if (platform < PlatformType.WebGL)
            {
                essentialInfo += deviceInfo + Environment.NewLine;
            }

            var feat = Environment.NewLine + "Features: ";
            for (int i = 0; i < features.Length; ++i)
            {
                if (features[i])
                {
                    feat += $"<{(Feature)i}>";
                }
            }
            essentialInfo += feat + Environment.NewLine;

            essentialInfo += Environment.NewLine +
                $"Break down:" + Environment.NewLine +
                $"Tried Latest version: {isDoneLatest}" + Environment.NewLine +
                $"Read Document: {isDoneDocument}" + Environment.NewLine +
                $"Read Log: {isDoneLog}" + Environment.NewLine +
                $"Tried Sample: {isDoneSample} ({sampleName}{(isSampleSimple ? ", but is too simple" : "")})" + Environment.NewLine +
                Environment.NewLine;

            using (_ = new GUILayout.VerticalScope())
            {
                using (_ = new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(10);
                    using (_ = new GUILayout.VerticalScope())
                    {
                        GUILayout.Space(10);
                        GUILayout.Label(lang == "en" ? "EasyAR Products" : "EasyAR 产品");
                        GUILayout.TextArea(easyarInfo);

                        GUILayout.Space(10);
                        GUILayout.Label(lang == "en" ? "Development Environment" : "开发环境");
                        using (var scroll = new GUILayout.ScrollViewScope(leftScrollPos2, false, true, GUILayout.Height(50)))
                        {
                            leftScrollPos2 = scroll.scrollPosition;
                            GUILayout.TextArea(hostInfo, GUILayout.ExpandHeight(true));
                        }

                        GUILayout.Space(10);
                        GUILayout.Label(lang == "en" ? "Runtime Environment (Please choose one of the following options)" : "运行环境（单选）");
                        using (_ = new GUILayout.VerticalScope(GUI.skin.box))
                        {
                            using (_ = new GUILayout.HorizontalScope())
                            {
                                foreach (var type in new PlatformType[] { PlatformType.Editor_Win, PlatformType.Editor_Mac, PlatformType.Windows, PlatformType.MacOS, PlatformType.Android, PlatformType.iOS })
                                {
                                    platforms[(int)type] = GUILayout.Toggle(platforms[(int)type], type.ToString(), GUILayout.Width(100));
                                }
                            }
                            using (_ = new GUILayout.HorizontalScope())
                            {
                                foreach (var type in new PlatformType[] { PlatformType.AndroidHMD, PlatformType.WebGL, PlatformType.Hololens, PlatformType.Other })
                                {
                                    platforms[(int)type] = GUILayout.Toggle(platforms[(int)type], type.ToString(), GUILayout.Width(100));
                                }
                            }
                            SelectPlatform();
                            foreach (var type in new PlatformType[] { PlatformType.WebGL, PlatformType.Hololens })
                            {
                                if (platforms[(int)type])
                                {
                                    using (_ = new ColorScope(Color.red))
                                    {
                                        GUILayout.Label(lang == "en" ? $"({type} is not supported in this version)" : $"（这个版本不支持 {type}）");
                                    }
                                    break;
                                }
                            }

                            if (platform < PlatformType.WebGL)
                            {
                                var label = lang == "en" ? "Device Infomation (Please set `DiagnosticsController.MessageOutput.SessionDump` to `Log`, copy one frame output and paste result bellow)" : "设备信息（请设置`DiagnosticsController.MessageOutput.SessionDump`为`Log`，复制一帧的输出并填写结果到下方）";
                                if (platform < PlatformType.Android)
                                {
                                    if ((isHostWin && (platform == PlatformType.Editor_Win || platform == PlatformType.Windows)) || (!isHostWin && (platform == PlatformType.Editor_Mac || platform == PlatformType.MacOS)))
                                    {
                                        label = lang == "en" ? "Device Infomation (Please confirm, or set `DiagnosticsController.MessageOutput.SessionDump` to `Log`, copy one frame output and paste result bellow)" : "设备信息（请确认，或设置`DiagnosticsController.MessageOutput.SessionDump`为`Log`，复制一帧的输出并填写结果到下方）";
                                    }
                                }
                                GUILayout.Space(10);
                                GUILayout.Label(label);
                                using (var scroll = new GUILayout.ScrollViewScope(leftScrollPos, false, true, GUILayout.Height(50)))
                                {
                                    leftScrollPos = scroll.scrollPosition;
                                    deviceInfo = GUILayout.TextArea(deviceInfo, GUILayout.ExpandHeight(true));
                                }
                            }
                        }


                        GUILayout.Space(10);
                        GUILayout.Label(lang == "en" ? "Features in use (Please choose one or more of the following options)" : "使用的功能（复选）");
                        using (_ = new GUILayout.VerticalScope(GUI.skin.box))
                        {
                            using (_ = new GUILayout.HorizontalScope())
                            {
                                foreach (var type in new Feature[] { Feature.ImageTracking, Feature.CloudRecognition, Feature.ObjectTracking, Feature.VideoRecording })
                                {
                                    features[(int)type] = GUILayout.Toggle(features[(int)type], type.ToString(), GUILayout.Width(130));
                                }
                            }
                            using (_ = new GUILayout.HorizontalScope())
                            {
                                foreach (var type in new Feature[] { Feature.MotionTracking, Feature.SparseSpatialMap, Feature.DenseSpatialMap, Feature.SurfaceTracking })
                                {
                                    features[(int)type] = GUILayout.Toggle(features[(int)type], type.ToString(), GUILayout.Width(130));
                                }
                            }
                            using (_ = new GUILayout.HorizontalScope())
                            {
                                foreach (var type in new Feature[] { Feature.Mega, Feature.Other })
                                {
                                    features[(int)type] = GUILayout.Toggle(features[(int)type], type.ToString(), GUILayout.Width(130));
                                }
                            }
                        }
                        if (platform < PlatformType.WebGL && features.Contains(true))
                        {
                            GUILayout.Space(10);
                            GUILayout.Label(lang == "en" ? "Breakdown the Problem" : "问题分解");
                            using (_ = new GUILayout.VerticalScope(GUI.skin.box))
                            {
                                isDoneLatest = GUILayout.Toggle(isDoneLatest, lang == "en" ? $"I have tried latest {UnityPackage.DisplayName} release" : $"我已试过最新版本的{UnityPackage.DisplayName}");
                                if (isDoneLatest)
                                {
                                    using (_ = new ColorScope(Color.yellow))
                                    {
                                        GUILayout.Label(lang == "en" ? $"(There are usually bug fixes and new features in new versions, please consider upgrade first)" : $"（新版本中通常包含bug修复及新功能，建议先升级到最新版本尝试）");
                                    }
                                }

                                GUILayout.Space(10);
                                using (_ = new GUILayout.HorizontalScope())
                                {
                                    isDoneDocument = GUILayout.Toggle(isDoneDocument, lang == "en" ? "I have read documents" : "我已阅读过文档");
                                    if (GUILayout.Button(lang == "en" ? "Documents" : "查看文档", GUILayout.Width(100)))
                                    {
                                        Application.OpenURL($"https://www.easyar.{(lang == "en" ? "com" : "cn")}/view/support.html");
                                    }
                                    if (GUILayout.Button(lang == "en" ? "FAQ" : "常见问题", GUILayout.Width(100)))
                                    {
                                        Application.OpenURL($"https://www.easyar.{(lang == "en" ? "com" : "cn")}/view/question.html");
                                    }
                                }
                                if (isDoneDocument)
                                {
                                    using (_ = new ColorScope(Color.yellow))
                                    {
                                        GUILayout.Label(lang == "en" ? $"(Please read documents and FAQs if you are not familar with EasyAR)" : $"（如果你对EasyAR不了解，建议查看文档及常见问题）");
                                    }
                                }

                                GUILayout.Space(10);
                                using (_ = new GUILayout.HorizontalScope())
                                {
                                    isDoneLog = GUILayout.Toggle(isDoneLog, lang == "en" ? "I have read system and Unity logs" : "我已阅读过系统及Unity日志");
                                    using (_ = new DisabledScope())
                                    {
                                        if (platform == PlatformType.Android || platform == PlatformType.AndroidHMD)
                                        {
                                            GUILayout.Label(lang == "en" ? "Please try: `adb logcat`, do not read Unity tag only" : "请尝试：`adb logcat`，不要只看Unity标签");
                                        }
                                        else if (platform == PlatformType.iOS)
                                        {
                                            GUILayout.Label(lang == "en" ? "Please try: `XCode` or `Console` App" : "请尝试：`XCode` 或 `控制台`应用");
                                        }
                                    }
                                }
                                if (isDoneLog)
                                {
                                    using (_ = new ColorScope(Color.yellow))
                                    {
                                        GUILayout.Label(lang == "en" ? $"(Please attach full log when ask a question)" : $"（建议在提问时提供完整日志）");
                                    }
                                }

                                GUILayout.Space(10);
                                isDoneSample = GUILayout.Toggle(isDoneSample, lang == "en" ? "I have tried to reproduce the problem in samples (inside an empty Unity project)" : "我已尝试过在Sample中复现问题（使用空的Unity工程）");
                                if (isDoneSample)
                                {
                                    using (_ = new GUILayout.HorizontalScope())
                                    {
                                        GUILayout.Space(20);
                                        using (_ = new GUILayout.VerticalScope())
                                        {
                                            using (_ = new GUILayout.HorizontalScope())
                                            {
                                                GUILayout.Label(lang == "en" ? "Sample Name" : "Sample 名称", GUILayout.Width(100));
                                                sampleName = GUILayout.TextField(sampleName);
                                            }
                                            isSampleSimple = GUILayout.Toggle(isSampleSimple, lang == "en" ? "Samples are too simple to reproduce my problem" : "Sample 太简单，无法用于复现问题");
                                            if (!isSampleSimple)
                                            {
                                                using (_ = new ColorScope(Color.yellow))
                                                {
                                                    GUILayout.Label(lang == "en" ? $"(Please describe how to reproduce the problem in samples when ask a question)" : $"（建议在提问时描述如何在Sample中复现问题）");
                                                }
                                            }
                                        }
                                    }
                                }

                                GUILayout.Space(10);
                            }

                            isDoneAll = isDoneSample && isDoneDocument && isDoneLog && isDoneLatest;
                            isDoneAll = GUILayout.Toggle(isDoneAll, lang == "en" ? "I have tried all above methods, but the problem is still there" : "我已尝试上面所有方法，但问题仍然存在");
                            if (isDoneAll)
                            {
                                isDoneSample = true;
                                isDoneDocument = true;
                                isDoneLog = true;
                                isDoneLatest = true;
                            }
                        }
                    }
                    using (_ = new GUILayout.VerticalScope())
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(">", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
                        GUILayout.FlexibleSpace();
                    }
                    using (_ = new GUILayout.VerticalScope())
                    {
                        GUILayout.Space(5);
                        using (_ = new GUILayout.HorizontalScope())
                        {
                            using (_ = new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                GUILayout.Label(lang == "en" ? "Essential Infomation" : "基本信息");
                            }
                            if (GUILayout.Button(lang == "en" ? "Copy" : "复制", GUILayout.Width(100), GUILayout.Height(20)))
                            {
                                GUIUtility.systemCopyBuffer = essentialInfo;
                            }
                        }
                        using (var scroll = new GUILayout.ScrollViewScope(rightScrollPos, false, true, GUILayout.Height(620)))
                        using (_ = new DisabledScope())
                        {
                            rightScrollPos = scroll.scrollPosition;
                            GUILayout.TextArea(essentialInfo, GUILayout.ExpandHeight(true));
                        }
                        GUILayout.Label(lang == "en" ? "Please provide above information when ask a question" : "请在提问时提供这些信息");
                    }
                    GUILayout.Space(10);
                }

                GUILayout.FlexibleSpace();

                using (_ = new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("中/EN", GUILayout.Width(60), GUILayout.Height(20)))
                    {
                        lang = lang == "en" ? "zh" : "en";
                        titleContent = new GUIContent(lang == "en" ? "Ask a Question" : "提问");
                    }
                    GUILayout.FlexibleSpace();
                    using (_ = new DisabledScope(platform >= PlatformType.WebGL || !isDoneAll))
                    {
                        if (GUILayout.Button(lang == "en" ? "Goto EasyAR Q&A" : "前往EasyAR问答", GUILayout.Width(400), GUILayout.Height(30)))
                        {
                            GUIUtility.systemCopyBuffer = essentialInfo;
                            Application.OpenURL($"https://answers.easyar.{(lang == "en" ? "com" : "cn")}/");
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.Space(10);
            }

            void SelectPlatform()
            {
                var hasPlatform = (int)platform < platforms.Length && platforms[(int)platform];
                for (int i = 0; i < platforms.Length; ++i)
                {
                    if (platforms[i] && i != (int)platform)
                    {
                        platform = (PlatformType)i;
                        if (platform < PlatformType.Android)
                        {
                            if ((isHostWin && (platform == PlatformType.Editor_Win || platform == PlatformType.Windows)) || (!isHostWin && (platform == PlatformType.Editor_Mac || platform == PlatformType.MacOS)))
                            {
                                deviceInfo = $"{UnityPackage.DisplayName} Version {UnityPackage.Version}" + Environment.NewLine +
                                    $"{engineName} Version {engineVersion}" + Environment.NewLine +
                                    $"Platform: {SystemInfo.operatingSystem}";
                            }
                            else
                            {
                                deviceInfo = "EasyAR Sense Unity Plugin Version ?" + Environment.NewLine +
                                    "? Version ?" + Environment.NewLine +
                                    $"Platform: ?";
                            }
                        }
                        else if (platform < PlatformType.WebGL)
                        {
                            deviceInfo = "EasyAR Sense Unity Plugin Version ?" + Environment.NewLine +
                                "? Version ?" + Environment.NewLine +
                                $"Platform: ?, ?";
                        }
                        hasPlatform = true;
                        break;
                    }
                }
                if (!hasPlatform)
                {
                    platform = PlatformType.None;
                }
                for (int i = 0; i < platforms.Length; ++i)
                {
                    platforms[i] = (int)platform == i;
                }
            }
        }

        private class DisabledScope : IDisposable
        {
            private bool enabled;

            public DisabledScope(bool disable = true)
            {
                enabled = GUI.enabled;
                GUI.enabled = !disable;
            }

            public void Dispose()
            {
                GUI.enabled = enabled;
            }
        }

        private class ColorScope : IDisposable
        {
            private Color color;

            public ColorScope(Color c)
            {
                color = GUI.color;
                GUI.color = c;
            }

            public void Dispose()
            {
                GUI.color = color;
            }
        }
    }
}
