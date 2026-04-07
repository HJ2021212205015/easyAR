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

namespace easyar
{
    internal class UnityXRSwitcher
    {
        private bool enabled;
        private List<MonoBehaviour> disabledARFoundationComponents = new();
        private List<MonoBehaviour> disabledGeneralXRComponents = new();
        private MonoBehaviour session;
        private List<string> warnings = new();

        public UnityXRSwitcher()
        {
            var options = EasyARSettings.Instance ? (EasyARSettings.Instance.UnityXR?.UnityXRAutoSwitch?.Player ?? new()) : new();
            if (SystemUtil.IsVisionOS())
            {
                enabled = false;
                Debug.Log($"{nameof(UnityXRSwitcher)} is disabled in VisionOS");
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
            {
                enabled = options.EnableIfDesktop;
                Debug.Log($"{nameof(UnityXRSwitcher)} is {(enabled ? "enabled" : "disabled")} on Desktop");
            }
            else if (!string.IsNullOrEmpty(UnityXRManager.ActiveLoaderName()))
            {
                enabled = options.EnableIfMobileAROnStartup && (UnityXRManager.IsARCoreLoaderActive() || UnityXRManager.IsARKitLoaderActive());
                Debug.Log($"{nameof(UnityXRSwitcher)} is {(enabled ? "enabled" : "disabled")} with active {UnityXRManager.ActiveLoaderName()}");
            }
            else
            {
                enabled = true;
                if (options.DisableIfNonMobileARPostStartup)
                {
                    if (UnityXRManager.ActiveLoaderNames().Any(n => n != UnityXRManager.ARCoreLoaderName || n != UnityXRManager.ARKitLoaderName))
                    {
                        Debug.Log($"{nameof(UnityXRSwitcher)} is disabled since non-ARCore/ARKit XR loaders found");
                        enabled = false;
                    }
                }
                Debug.Log($"{nameof(UnityXRSwitcher)} is enabled");
            }

            if (!enabled)
            {
                if (options.RestoreARSessionWhenDisabled)
                {
                    foreach (var session in arfoundation.SceneManager.GetARSessions().Where(s => !s.enabled))
                    {
                        Debug.LogWarning($"{nameof(UnityXRSwitcher)}: enable (restore) AR Foundation Session ({session.GetInstanceID()})");
                        session.enabled = true;
                    }
                }
                return;
            }

            try
            {
                CheckSession();
                Switch(false, false, null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                warnings.Add($"{nameof(UnityXRSwitcher)}: exception during startup: {e}");
            }
        }

        public static void DisableARSession()
        {
            foreach (var session in arfoundation.SceneManager.GetARSessions().Where(s => s.enabled))
            {
                Debug.LogWarning($"{nameof(UnityXRSwitcher)}: disable AR Foundation Session ({session.GetInstanceID()})");
                session.enabled = false;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(session);
#endif
            }
        }

        public void Switch(bool isARFoundation, bool isXROriginRequired, DiagnosticsController diagnostics)
        {
            if (!enabled) { return; }
            if (diagnostics)
            {
                foreach (var w in warnings)
                {
                    diagnostics.EnqueueWarning(w);
                }
                warnings = new();
            }

            if (isARFoundation)
            {
                foreach (var c in disabledARFoundationComponents.Where(i => i))
                {
                    Debug.Log($"{nameof(UnityXRSwitcher)}: enable {c} ({c.GetInstanceID()})");
                    c.enabled = true;
                }
                disabledARFoundationComponents = new();
            }
            else
            {
                var components = arfoundation.SceneManager.GetARFoundationComponents().Append(session).Where(c => c && c.enabled);
                foreach (var c in components)
                {
                    Debug.Log($"{nameof(UnityXRSwitcher)}: disable {c} ({c.GetInstanceID()})");
                    c.enabled = false;
                    disabledARFoundationComponents.Add(c);
                }
            }
            if (isXROriginRequired)
            {
                foreach (var c in disabledGeneralXRComponents.Where(i => i))
                {
                    Debug.Log($"{nameof(UnityXRSwitcher)}: enable {c} ({c.GetInstanceID()})");
                    c.enabled = true;
                }
                disabledGeneralXRComponents = new();
            }
            else
            {
                var components = GetGeneralXRComponents().Where(c => c && c.enabled);
                foreach (var c in components)
                {
                    Debug.Log($"{nameof(UnityXRSwitcher)}: disable {c} ({c.GetInstanceID()})");
                    c.enabled = false;
                    disabledGeneralXRComponents.Add(c);
                }
            }
        }

        private void CheckSession()
        {
            var sessions = arfoundation.SceneManager.GetARSessions();
            if (sessions.Count() > 1)
            {
                warnings.Add($"{nameof(UnityXRSwitcher)}: multiple AR Foundation Sessions found when EasyAR ARSession awake, {nameof(UnityXRSwitcher)} may not work correctly");
            }
            var sessionsEnabled = sessions.Where(s => s.enabled);
            if (sessionsEnabled.Any())
            {
                session = sessionsEnabled.First();
                warnings.Add($"{nameof(UnityXRSwitcher)}: AR Foundation Session is enabled when EasyAR ARSession awake, {nameof(UnityXRSwitcher)} may not work correctly");
            }
            if (!session)
            {
                session = sessions.FirstOrDefault();
            }
            if (session && !session.enabled)
            {
                disabledARFoundationComponents.Add(session);
            }
        }

        private static List<MonoBehaviour> GetGeneralXRComponents() => ObjectUtil.FindObjectsByType<MonoBehaviour>().Where(c => c && c.GetType() != null && !string.IsNullOrEmpty(c.GetType().Namespace) && (c.GetType().Namespace.StartsWith("Unity.XR.CoreUtils") || c.GetType().Namespace.StartsWith("UnityEngine.InputSystem.XR"))).ToList();

    }

    internal static class UnityXRManager
    {
        public const string ARCoreLoaderName = "UnityEngine.XR.ARCore.ARCoreLoader";
        public const string ARKitLoaderName = "UnityEngine.XR.ARKit.ARKitLoader";

        public static bool IsARCoreLoaderActive() => Application.platform == RuntimePlatform.Android && ActiveLoaderName() == ARCoreLoaderName;
        public static bool IsARKitLoaderActive() => Application.platform == RuntimePlatform.IPhonePlayer && ActiveLoaderName() == ARKitLoaderName;

        public static string ActiveLoaderName()
        {
#if EASYAR_ENABLE_XRMANAGEMENT
            if (UnityEngine.XR.Management.XRGeneralSettings.Instance && UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager)
            {
                var loader = UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.activeLoader;
                if (loader && loader.GetType() != null)
                {
                    return loader.GetType().FullName;
                }
            }
#endif
            return string.Empty;
        }

        public static List<string> ActiveLoaderNames()
        {
#if EASYAR_ENABLE_XRMANAGEMENT
            if (UnityEngine.XR.Management.XRGeneralSettings.Instance && UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager)
            {
                return UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.
#if EASYAR_ENABLE_XRMANAGEMENT_3
                    loaders
#else
                    activeLoaders
#endif
                    .Where(l => l && l.GetType() != null).Select(l => l.GetType().FullName).ToList();
            }
#endif
            return new();
        }
    }
}
