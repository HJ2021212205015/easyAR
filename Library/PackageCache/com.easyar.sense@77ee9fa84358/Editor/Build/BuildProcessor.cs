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
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
#if UNITY_IOS || UNITY_VISIONOS
using UnityEditor.iOS.Xcode;
#endif

namespace easyar
{
    class BuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 1;

        static string[] shaderNames = new string[] {
            "EasyAR/CameraImage_RGB",
            "EasyAR/CameraImage_BGR",
            "EasyAR/CameraImage_Gray",
            "EasyAR/CameraImage_YUV_I420_YV12",
            "EasyAR/CameraImage_YUV_NV12",
            "EasyAR/CameraImage_YUV_NV21",
            "EasyAR/DenseSpatialMapDepth",
        };

        public void OnPreprocessBuild(BuildReport report)
        {
            try
            {
                PackageChecker.CheckPath();
                PackageChecker.CheckOldAssets();
                PackageChecker.CheckEasyARSettings();
                if (AssetDatabase.IsValidFolder("Assets/HiddenEasyAR"))
                {
                    AssetDatabase.DeleteAsset("Assets/HiddenEasyAR");
                }
                if (DetectNRSDK())
                {
                    throw new NotSupportedException($"require XREAL SDK >= 3.1");
                }

                var settings = EasyARSettings.ConfigObject;
                if (settings == null)
                {
                    throw new FileNotFoundException("EasyAR Settings Asset");
                }

                if (settings.VerifyLicenseWhenBuild)
                {
                    var os = Optional<EngineOperatingSystem>.Empty;
                    switch (report.summary.platform)
                    {
                        case BuildTarget.iOS:
                            os = EngineOperatingSystem.iOS;
                            break;
                        case BuildTarget.Android:
                            os = EngineOperatingSystem.Android;
                            break;
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            os = EngineOperatingSystem.Windows;
                            break;
                        case BuildTarget.StandaloneOSX:
                            os = EngineOperatingSystem.MacOS;
                            break;
#if UNITY_VISIONOS
                        case BuildTarget.VisionOS:
                            os = EngineOperatingSystem.VisionOS;
                            break;
#endif
                        default:
                            break;
                    }
                    if (string.IsNullOrEmpty(settings.LicenseKey))
                    {
                        Debug.LogWarning("EasyAR Sense license key is not set in Project Settings.");
                    }
                    if (os.OnSome && !string.IsNullOrEmpty(settings.LicenseKey))
                    {
                        var packageName = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(report.summary.platformGroup));
                        var license = settings.LicenseKey.Trim();
                        if (!Engine.isLicenseKeyMatched(license, packageName, Engine.variant(), os.Value))
                        {
                            var key = license;
                            if (key.Length > 10)
                            {
                                key = key.Substring(0, 5) + "..." + key.Substring(key.Length - 5, 5);
                            }
                            var idStr = "";
                            if (os.Value == EngineOperatingSystem.Android)
                            {
                                idStr = "package name";
                            }
                            else if (os.Value == EngineOperatingSystem.iOS || os.Value == EngineOperatingSystem.VisionOS)
                            {
                                idStr = "bundle identifier";
                            }

                            var idMessage = !string.IsNullOrEmpty(idStr) ? $"{idStr} ({packageName}) on" : "";
                            var message = $"\"{key}\" is not a valid EasyAR Sense license key or it does not match {idMessage} {os.Value} platform with EasyAR Sense {(Engine.variant().Replace("community", ""))}.";
                            message += $" Please make sure the license key can be used in {os.Value} platform (go to www.easyar.com).";
                            if (!string.IsNullOrEmpty(idStr))
                            {
                                message += $" Please make sure {idStr} in Unity Player Settings (Project Settings > Player > {idStr}) match {idStr} in the license key (go to www.easyar.com).";
                            }

                            message += Environment.NewLine + $"If you setup license in a difference place,";
                            if (!string.IsNullOrEmpty(idStr))
                            {
                                message += $" or if you need to change {idStr} after Unity build process,";
                            }
                            message += " you can turn off VerifyLicenseWhenBuild option (EasyAR > Sense > Configuration > VerifyLicenseWhenBuild).";

                            var selection = EditorUtility.DisplayDialogComplex(
                                "Fail to verify EasyAR Sense license key",
                                message,
                                "Cancel build", "Continue anyway", "Continue and don't warn me again");

                            switch (selection)
                            {
                                case 0:
                                    SettingsService.OpenProjectSettings("Project/EasyAR");
                                    throw new BuildFailedException(message);
                                case 1:
                                    break;
                                case 2:
                                    settings.VerifyLicenseWhenBuild = false;
                                    break;
                            }
                        }
                    }
                }

#if EASYAR_USE_SEPERATE_BINDING
#error seperate binding cannot be used with this package
#else
                var bindingDir = $"Packages/{UnityPackage.Name}/Runtime/Binding/";
#endif

                var permissions = settings.Permissions;

                if (report.summary.platform == BuildTarget.iOS)
                {
                    foreach (var variant in Enum.GetValues(typeof(EasyARSettings.LibVariantConfig.IOSVariant)).Cast<EasyARSettings.LibVariantConfig.IOSVariant>())
                    {
                        var lib = bindingDir + variant switch
                        {
                            EasyARSettings.LibVariantConfig.IOSVariant.Normal => "Lib",
                            EasyARSettings.LibVariantConfig.IOSVariant.Full => "LibFull",
                            _ => throw new ArgumentOutOfRangeException(),
                        } + "/Apple/iOS/ios-arm64/easyar.framework";
                        PrepareOptionalNativePlugin(lib, variant == settings.LibVariants.IOS);
                    }

                    if (permissions.Camera && string.IsNullOrEmpty(PlayerSettings.iOS.cameraUsageDescription))
                    {
                        throw new BuildFailedException("EasyAR Camera Device requires a Camera Usage Description (Player Settings > iOS > Other Settings > Camera Usage Description). You can turn this feature off if not used (EasyAR > Sense > Configuration > Permissions > Camera Device).");
                    }
                    if (permissions.Location && string.IsNullOrEmpty(PlayerSettings.iOS.locationUsageDescription))
                    {
                        throw new BuildFailedException("EasyAR Mega requires a Location Usage Description (Player Settings > iOS > Other Settings > Location Usage Description). You can turn this feature off if not used (EasyAR > Sense > Configuration > Permissions > Mega).");
                    }
                }
                if (IsVisionOS(report))
                {
                    if (permissions.Location && string.IsNullOrEmpty(PlayerSettings.iOS.locationUsageDescription))
                    {
                        throw new BuildFailedException("EasyAR Mega requires a Location Usage Description (Player Settings > iOS > Other Settings > Location Usage Description). You can turn this feature off if not used (EasyAR > Sense > Configuration > Permissions > Mega).");
                    }
                }
                if (report.summary.platform == BuildTarget.Android)
                {
                    foreach (var variant in Enum.GetValues(typeof(EasyARSettings.LibVariantConfig.AndroidVariant)).Cast<EasyARSettings.LibVariantConfig.AndroidVariant>())
                    {
                        var lib = bindingDir + variant switch
                        {
                            EasyARSettings.LibVariantConfig.AndroidVariant.Normal => "Lib",
                            EasyARSettings.LibVariantConfig.AndroidVariant.Full => "LibFull",
                            EasyARSettings.LibVariantConfig.AndroidVariant.VideoRecording => "LibR",
                            _ => throw new ArgumentOutOfRangeException(),
                        } + "/Android/EasyAR.aar";
                        PrepareOptionalNativePlugin(lib, variant == settings.LibVariants.Android);
                    }

                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/EasyAR-proguard.aar", true);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/UnityAndroidPermissions-proguard.aar", true);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/permission.CAMERA.aar", permissions.Camera);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/permission.RECORD_AUDIO.aar", permissions.AndroidMicrophone);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/permission.ACCESS_FINE_LOCATION.aar", permissions.Location);
                    var arcoreLoader = false;
                    var arcore = settings.ARCoreSDK;
                    var arengine = settings.AREngineSDK;
#if EASYAR_ENABLE_XRMANAGEMENT && EASYAR_ENABLE_UNITYARCORE
                    var generalSettings = UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget));
                    if (generalSettings)
                    {
#if EASYAR_ENABLE_XRMANAGEMENT_3
                        foreach (var loader in generalSettings.Manager.loaders)
#else
                        foreach (var loader in generalSettings.Manager.activeLoaders)
#endif
                        {
                            if (loader is UnityEngine.XR.ARCore.ARCoreLoader)
                            {
                                arcoreLoader = true;
                                if (arcore == EasyARSettings.ARCoreType.Optional || arcore == EasyARSettings.ARCoreType.Required)
                                {
                                    Debug.LogWarning($"ARCoreLoader is active, ARCoreSDK (current = {arcore}) is forced to {EasyARSettings.ARCoreType.External}");
                                }
                                arcore = EasyARSettings.ARCoreType.External;
                                break;
                            }
                        }
                    }
#endif
                    if (arcore == EasyARSettings.ARCoreType.ARFoundationOrOptional)
                    {
                        arcore = EasyARSettings.ARCoreType.Optional;
                    }
                    if (arcore != EasyARSettings.ARCoreType.External || arengine == EasyARSettings.AREngineType.AREngineInterop)
                    {
                        if (arcore != EasyARSettings.ARCoreType.External)
                        {
                            var minSdkVersion = arcore == EasyARSettings.ARCoreType.Optional ? 14 : 24;
                            if ((int)PlayerSettings.Android.minSdkVersion < minSdkVersion)
                            {
                                throw new BuildFailedException($"ARCore {arcore} apps require a minimum SDK version of {minSdkVersion}. Currently set to {PlayerSettings.Android.minSdkVersion}");
                            }
                        }
                    }
                    if (arcoreLoader || arcore != EasyARSettings.ARCoreType.External)
                    {
                        if (PlayerSettings.GetScriptingBackend(NamedBuildTarget.Android) != ScriptingImplementation.IL2CPP || !PlayerSettings.Android.targetArchitectures.HasFlag(AndroidArchitecture.ARM64))
                        {
                            var errorMessage = "ARCore has removed support for 32-bit-only ARCore-enabled apps running on 64-bit devices. Support for 32-bit apps running on 32-bit devices is unaffected. 32-bit-only ARCore-enabled apps that are not updated may crash when attempting to start an (ARCore) AR session. See https://developers.google.com/ar/64bit for further details.";
                            if (settings.Verify32bitOnlyARCoreWhenBuild)
                            {
                                var selection = EditorUtility.DisplayDialogComplex(
                                    "ARCore may not work in 32-bit-only apps",
                                    errorMessage,
                                    "Cancel build", "Continue anyway", "Continue and don't warn me again");

                                switch (selection)
                                {
                                    case 0:
                                        throw new BuildFailedException(errorMessage);
                                    case 1:
                                        break;
                                    case 2:
                                        settings.Verify32bitOnlyARCoreWhenBuild = false;
                                        break;
                                }
                            }
                            else
                            {
                                Debug.LogWarning(errorMessage);
                            }
                        }
                    }

                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/onnxruntime-android-1.22.0.aar", settings.UseBundledONNXRuntime);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/com.google.ar.core-1.46.0.aar", arcore != EasyARSettings.ARCoreType.External);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/com.google.ar.core-required.aar", arcore == EasyARSettings.ARCoreType.Required);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/Android/com.google.ar.core-optional.aar", arcore == EasyARSettings.ARCoreType.Optional);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/BindingAREngine/Android/AREngineInterop.aar", arengine != EasyARSettings.AREngineType.Disabled);
                    PrepareOptionalNativePlugin($"Packages/{UnityPackage.Name}/Runtime/BindingAREngine/Android/arenginesdk-3.7.0.3.aar", arengine == EasyARSettings.AREngineType.AREngineInterop);
                }
                AddShader();
                AddSettings(settings);
            }
            catch (Exception e)
            {
                throw new BuildFailedException(e);
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            try
            {
                RemoveSettings();
                RemoveShader();
                if (AssetDatabase.IsValidFolder("Assets/HiddenEasyAR"))
                {
                    AssetDatabase.DeleteAsset("Assets/HiddenEasyAR");
                }

                if (report.summary.platform == BuildTarget.iOS)
                {
#if UNITY_IOS
                    var proj = new PBXProject();
                    var projPath = PBXProject.GetPBXProjectPath(report.summary.outputPath);
                    proj.ReadFromFile(projPath);
                    proj.SetBuildProperty(proj.GetUnityFrameworkTargetGuid(), "ENABLE_BITCODE", "NO");
                    proj.SetBuildProperty(proj.GetUnityMainTargetGuid(), "ENABLE_BITCODE", "NO");
                    proj.WriteToFile(projPath);
#endif
                }
#if UNITY_VISIONOS
                if (report.summary.platform == BuildTarget.VisionOS)
                {
                    //add descriptions to plist
                    var plist = new PlistDocument();
                    var plistPath = report.summary.outputPath + "/Info.plist";
                    plist.ReadFromFile(plistPath);
                    plist.root.SetString("NSEnterpriseMCAMUsageDescription", "Application requires access to the camera for providing an enhanced augmented reality experience.");
                    plist.WriteToFile(plistPath);
                }
#endif
            }
            catch (Exception e)
            {
                throw new BuildFailedException(e);
            }
        }

        private void PrepareOptionalNativePlugin(string asset, bool enable)
        {
            var plugin = AssetImporter.GetAtPath(asset) as PluginImporter;
            plugin.SetIncludeInBuildDelegate(path => enable);
        }

        private static void AddShader()
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            var changed = false;

            foreach (var shaderName in shaderNames)
            {
                var shader = Shader.Find(shaderName);
                if (!shader)
                {
                    throw new Exception($"Cannot find shader '{shaderName}'");
                }
                if (!preloadedAssets.Where(a => shader.Equals(a)).Any())
                {
                    preloadedAssets.Add(shader);
                    changed = true;
                }
            }
            if (changed)
            {
                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            }
        }

        private static void RemoveShader()
        {
            var shaders = shaderNames.Select(s => Shader.Find(s));
            PlayerSettings.SetPreloadedAssets(PlayerSettings.GetPreloadedAssets().Where(a => !shaders.Contains(a)).ToArray());
        }

        private static void AddSettings(EasyARSettings settings)
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();

            if (!preloadedAssets.Contains(settings))
            {
                var assets = preloadedAssets.ToList();
                assets.Add(settings);
                PlayerSettings.SetPreloadedAssets(assets.ToArray());
            }
        }

        private static void RemoveSettings()
        {
            PlayerSettings.SetPreloadedAssets(PlayerSettings.GetPreloadedAssets().Where(a => (a == null || a.GetType() != typeof(EasyARSettings))).ToArray());
        }

        private static bool DetectNRSDK()
        {
#if !EASYAR_ENABLE_XREAL && !EASYAR_XREAL_NOTSUPPORT
            try
            {
                return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.FullName == "NRKernal.NRVersionInfo")).Any();
            }
            catch (Exception)
            {
                return false;
            }
#else
            return false;
#endif
        }

        private bool IsVisionOS(BuildReport report)
        {
#if UNITY_VISIONOS
            return report.summary.platform == BuildTarget.VisionOS;
#else
            return false;
#endif
        }
    }
}
