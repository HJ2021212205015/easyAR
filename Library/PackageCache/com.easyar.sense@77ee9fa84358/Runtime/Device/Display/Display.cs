//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Display device interface.</para>
    /// <para xml:lang="zh">显示设备接口。</para>
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// <para xml:lang="en">Device rotation.</para>
        /// <para xml:lang="zh">设备旋转信息。</para>
        /// </summary>
        int Rotation { get; }
    }

    /// <summary>
    /// <para xml:lang="en">Display device.</para>
    /// <para xml:lang="zh">显示设备。</para>
    /// </summary>
    public static class Display
    {
        private static readonly DefaultHMDDisplayProvider hmdDisplayProvider = new DefaultHMDDisplayProvider();

        /// <summary>
        /// <para xml:lang="en">Default system display. User on desktop or mobile phone.</para>
        /// <para xml:lang="zh">默认的系统显示。在桌面设备或手机上使用。</para>
        /// </summary>
        public static IDisplay DefaultSystemDisplay => EasyARController.DefaultSystemDisplay;
        /// <summary>
        /// <para xml:lang="en">Default head mounted display. User on HMD. Display rotation is 0.</para>
        /// <para xml:lang="zh">默认的头戴显示。在头戴设备上使用。屏幕旋转为0。</para>
        /// </summary>
        public static IDisplay DefaultHMDDisplay => hmdDisplayProvider;
    }

    /// <summary>
    /// <para xml:lang="en">Display emulator.</para>
    /// <para xml:lang="zh">旋转模拟器。</para>
    /// </summary>
    public class DisplayEmulator : IDisplay
    {
        private int rotation;

        /// <summary>
        /// <para xml:lang="en">Rotation mode.</para>
        /// <para xml:lang="zh">旋转模式。</para>
        /// </summary>
        public enum RotationMode
        {
            /// <summary>
            /// <para xml:lang="en">Rotate 0 degrees.</para>
            /// <para xml:lang="zh">旋转0度。</para>
            /// </summary>
            Rotation_0 = 0,
            /// <summary>
            /// <para xml:lang="en">Rotate 90 degrees.</para>
            /// <para xml:lang="zh">旋转90度。</para>
            /// </summary>
            Rotation_90 = 90,
            /// <summary>
            /// <para xml:lang="en">Rotate 180 degrees.</para>
            /// <para xml:lang="zh">旋转180度。</para>
            /// </summary>
            Rotation_180 = 180,
            /// <summary>
            /// <para xml:lang="en">Rotate 270 degrees.</para>
            /// <para xml:lang="zh">旋转270度。</para>
            /// </summary>
            Rotation_270 = 270,
        }

        public int Rotation => rotation;

        /// <summary>
        /// <para xml:lang="en">Emulate screen rotation to <paramref name="mode"/>.</para>
        /// <para xml:lang="zh">模拟屏幕旋转为<paramref name="mode"/>。</para>
        /// </summary>
        public void EmulateRotation(RotationMode mode)
        {
            EmulateRotation((int)mode);
        }

        internal void EmulateRotation(int value)
        {
            rotation = value;
        }
    }

    internal class DefaultSystemDisplayProvider : IDisplay, IDisposable
    {
        private Dictionary<int, int> rotations = new Dictionary<int, int>();
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaObject defaultDisplay;
#endif

        public DefaultSystemDisplayProvider()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                InitializeAndroid();
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                InitializeIOS();
            }
        }

        ~DefaultSystemDisplayProvider()
        {
            DeleteAndroidJavaObjects();
        }

        public int Rotation
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                    var rotation = defaultDisplay?.Call<int>("getRotation") ?? 0;
                    return rotations[rotation];
#endif
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return rotations[(int)Screen.orientation];
                }
                return 0;
            }
        }

        public void Dispose()
        {
            DeleteAndroidJavaObjects();
            GC.SuppressFinalize(this);
        }

        private void InitializeIOS()
        {
            rotations[(int)ScreenOrientation.Portrait] = 0;
            rotations[(int)ScreenOrientation.LandscapeLeft] = 90;
            rotations[(int)ScreenOrientation.PortraitUpsideDown] = 180;
            rotations[(int)ScreenOrientation.LandscapeRight] = 270;
        }

        private void InitializeAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var surfaceClass = new AndroidJavaClass("android.view.Surface"))
            using (var contextClass = new AndroidJavaClass("android.content.Context"))
            using (var windowService = contextClass.GetStatic<AndroidJavaObject>("WINDOW_SERVICE"))
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var systemService = currentActivity.Call<AndroidJavaObject>("getSystemService", windowService))
            {
                defaultDisplay = systemService.Call<AndroidJavaObject>("getDefaultDisplay");
                rotations[surfaceClass.GetStatic<int>("ROTATION_0")] = 0;
                rotations[surfaceClass.GetStatic<int>("ROTATION_90")] = 90;
                rotations[surfaceClass.GetStatic<int>("ROTATION_180")] = 180;
                rotations[surfaceClass.GetStatic<int>("ROTATION_270")] = 270;
            }
#endif
        }

        private void DeleteAndroidJavaObjects()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (defaultDisplay != null)
            {
                defaultDisplay.Dispose();
                defaultDisplay = null;
            }
#endif
        }
    }

    internal class DefaultHMDDisplayProvider : IDisplay
    {
        public int Rotation => 0;
    }
}
