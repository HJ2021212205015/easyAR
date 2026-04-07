//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

#if !EASYAR_ENABLE_SENSE
#error DO NOT DECOMPRESS .TGZ FILE!
#error EasyAR package was not correctly imported by UNITY PACKAGE MANAGER! Please read online documents for how to use.
#warning If you are breaking package on purpose, just comment out these errors. But NO official support will be provided for possible building or runtime errors caused by this kind of usage.
#endif
#if EASYAR_HAVE_MEGA && !EASYAR_ENABLE_MEGA
#error Incompatible EasyAR Mega Studio version detected. Use the version included in the same ZIP package or one officially supported by EasyAR.
#error 检测到不支持的EasyAR Mega Studio版本，请使用同一个zip压缩包中的文件或EasyAR声明可兼容的版本!
#endif
#if EASYAR_XREAL_NOTSUPPORT
#error Incompatible XREAL SDK detected, require XREAL SDK >= 3.1!
#endif
#if EASYAR_DISABLE_ARFOUNDATION
#warning EasyAR AR Foundation Support is disabled. To enable, please change settings in Project Settings > EasyAR > Sense.
#endif

using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace easyar
{
    internal static class PackageChecker
    {
        static readonly string[] oldScripts = new string[] {
            "EasyAR/Scripts/csapi.cs",
            "EasyAR/Scripts/EasyAR.Unity.dll",
        };
        static readonly string[] oldNativePluginFiles = new string[] {
            "Plugins/Android/libs/arm64-v8a/libEasyAR.so",
            "Plugins/Android/libs/arm64-v8a/libEasyARUnity.so",
            "Plugins/Android/libs/armeabi-v7a/libEasyAR.so",
            "Plugins/Android/libs/armeabi-v7a/libEasyARUnity.so",
            "Plugins/Android/libs/EasyAR.jar",
            "Plugins/iOS/EasyARAppController.mm",
            "Plugins/iOS/libEasyARUnity.a",
            "Plugins/x64/bin/EasyAR.dll",
            "Plugins/x86/bin/EasyAR.dll",
            "Plugins/x86/EasyARUnity.dll",
            "Plugins/x86_64/EasyARUnity.dll",
        };
        static readonly string[] oldNativePluginFolders = new string[] {
            "Plugins/EasyAR.bundle",
            "Plugins/EasyARUnity.bundle",
            "Plugins/iOS/easyar.framework",
        };
        static readonly string[] oldNativePluginFilesWarn = new string[] {
            "Plugins/Android/libs/arcore-classes.jar",
            "Plugins/Android/libs/arm64-v8a/libarcore_sdk_c.so",
            "Plugins/Android/libs/armeabi-v7a/libarcore_sdk_c.so",
        };

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void OnLoadCheck()
        {
            CheckPath();
            CheckOldAssets();
            CheckEasyARSettings();
        }
#endif

        public static void CheckPath([System.Runtime.CompilerServices.CallerFilePath] string filepath = "")
        {
#if UNITY_EDITOR
            if (!UnityEditor.AssetDatabase.GUIDToAssetPath("977ba8ebe7b21294b84bb418b677fd63").StartsWith("Packages") || filepath.Replace("\\", "/").Contains(Application.dataPath.Replace("\\", "/")))
            {
                Debug.LogError("DO NOT DECOMPRESS .TGZ FILE!");
                throw new Exception("EasyAR package was not correctly imported by UNITY PACKAGE MANAGER! Please read online documents for how to use.");
            }
#endif
        }

        public static void CheckOldAssets()
        {
#if UNITY_EDITOR
            var message = "Old EasyAR package files detected at Assets/{0}. These files are not compatible with current package. Please remove all files extracted from old packages!";
            foreach (var _ in oldScripts.Where(file => File.Exists(Path.Combine(Application.dataPath, file))))
            {
                throw new Exception(string.Format(message, "EasyAR"));
            }
            foreach (var file in oldNativePluginFiles.Where(file => File.Exists(Path.Combine(Application.dataPath, file))))
            {
                throw new Exception(string.Format(message, file));
            }
            foreach (var file in oldNativePluginFolders.Where(file => Directory.Exists(Path.Combine(Application.dataPath, file))))
            {
                throw new Exception(string.Format(message, file));
            }
            foreach (var file in oldNativePluginFilesWarn.Where(file => File.Exists(Path.Combine(Application.dataPath, file))))
            {
                Debug.LogWarning($"Possible old EasyAR package files detected at Assets/{file}. Please remove this file if it was from EasyAR packages, except you are doing this on purpose.");
                break;
            }
#endif
        }

        public static void CheckEasyARSettings()
        {
#if UNITY_EDITOR
            var settings = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(EasyARSettings)}").Select(asset => UnityEditor.AssetDatabase.GUIDToAssetPath(asset));
            if (settings.Count() > 1)
            {
                var paths = string.Empty;
                foreach (var path in settings)
                {
                    if (!string.IsNullOrEmpty(paths))
                    {
                        paths += Environment.NewLine;
                    }
                    paths += path;
                }
                throw new InvalidOperationException($"Multiple {nameof(EasyARSettings)} asset NOT allowed:" + Environment.NewLine + paths);
            }
            foreach (var path in settings)
            {
                var p = Path.GetDirectoryName(path);
                while (!string.IsNullOrEmpty(p))
                {
                    if (Path.GetFileName(p) == "Resources")
                    {
                        Debug.LogWarning($"EasyAR Settings is no longer loaded as resource. Suggest moving EasyAR Settings asset \"{path}\" out of \"Resources\" folder.");
                    }
                    p = Path.GetDirectoryName(p);
                }
            }
#endif
        }
    }
}
