//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

#if EASYAR_DISABLE_ARFOUNDATION && EASYAR_ENABLE_ARFOUNDATION
#undef EASYAR_ENABLE_ARFOUNDATION
#endif

using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
#if EASYAR_ENABLE_ARFOUNDATION
using System.Linq;
using UnityEngine.XR.ARFoundation;
#endif

#if EASYAR_ARFOUNDATION_NOTSUPPORT && !EASYAR_DISABLE_ARFOUNDATION
#error EasyAR support only AR Foundation 5.0.0 or newer. Please update your AR Foundation package.
#if UNITY_2021_3
#warning Please follow instuctions from https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.2/manual/project-setup/edit-your-project-manifest.html
#endif
#endif

[assembly: InternalsVisibleTo("EasyAR.Sense")]

namespace easyar.arfoundation
{
    internal class SceneManager
    {
        public static List<MonoBehaviour> GetARFoundationComponents() =>
#if EASYAR_ENABLE_ARFOUNDATION
#if UNITY_2022_3_OR_NEWER
            GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
#else
            GameObject.FindObjectsOfType<MonoBehaviour>()
#endif
            .Where(c => c && c.GetType() != null && !string.IsNullOrEmpty(c.GetType().Namespace) && (c.GetType().Namespace.StartsWith("UnityEngine.XR.ARFoundation") || c.GetType().Namespace.StartsWith("UnityEngine.XR.ARSubsystems") || c.GetType().Namespace.StartsWith("UnityEngine.XR.Simulation"))).ToList();
#else
            new();
#endif
        public static List<MonoBehaviour> GetARSessions() =>
#if EASYAR_ENABLE_ARFOUNDATION
#if UNITY_2022_3_OR_NEWER
            GameObject.FindObjectsByType<ARSession>(FindObjectsSortMode.None)
#else
            GameObject.FindObjectsOfType<ARSession>()
#endif
            .Select(s => s as MonoBehaviour).ToList();
#else
            new();
#endif
    }
}
