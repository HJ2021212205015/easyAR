//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEngine;

namespace easyar
{
    internal static class ObjectUtil
    {
        public static T FindAnyObjectByType<T>() where T : Object =>
#if UNITY_2022_3_OR_NEWER
            Object.FindAnyObjectByType<T>();
#else
            Object.FindObjectOfType<T>();
#endif

        public static T FindAnyObjectByType<T>(bool includeInactive) where T : Object =>
#if UNITY_2022_3_OR_NEWER
            Object.FindAnyObjectByType<T>(includeInactive? FindObjectsInactive.Include : FindObjectsInactive.Exclude);
#else
            Object.FindObjectOfType<T>(includeInactive);
#endif

        public static T FindFirstObjectByType<T>() where T : Object =>
#if UNITY_2022_3_OR_NEWER
            Object.FindFirstObjectByType<T>();
#else
            Object.FindObjectOfType<T>();
#endif

        public static T FindFirstObjectByType<T>(bool includeInactive) where T : Object =>
#if UNITY_2022_3_OR_NEWER
            Object.FindFirstObjectByType<T>(includeInactive? FindObjectsInactive.Include : FindObjectsInactive.Exclude);
#else
            Object.FindObjectOfType<T>(includeInactive);
#endif

        public static T[] FindObjectsByType<T>() where T : Object =>
#if UNITY_2022_3_OR_NEWER
            Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else
            Object.FindObjectsOfType<T>();
#endif

        public static T[] FindObjectsByType<T>(bool includeInactive) where T : Object =>
#if UNITY_2022_3_OR_NEWER
            Object.FindObjectsByType<T>(includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            Object.FindObjectsOfType<T>(includeInactive);
#endif
    }
}
