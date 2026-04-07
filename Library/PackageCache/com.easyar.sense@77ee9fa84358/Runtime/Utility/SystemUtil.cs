//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEngine;
using UnityEngine.Rendering;

namespace easyar
{
    internal static class SystemUtil
    {
        public enum RenderPipelineType
        {
            Builtin,
            URP,
            Unsupported,
        }

        private static string deviceModel;
        public static string DeviceModel
        {
            get
            {
                if (string.IsNullOrEmpty(deviceModel))
                {
                    deviceModel = GetDeviceModel();
                }
                return deviceModel;
            }
        }

        public static RenderPipelineType RenderPipeline
        {
            get
            {
                if (GraphicsSettings.currentRenderPipeline == null)
                {
                    return RenderPipelineType.Builtin;
                }
#if EASYAR_URP_ENABLE
                else if (GraphicsSettings.currentRenderPipeline is UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)
                {
                    return RenderPipelineType.URP;
                }
#endif
                return RenderPipelineType.Unsupported;
            }
        }

        public static bool IsVisionOS()
        {
#if UNITY_VISIONOS
            return Application.platform == RuntimePlatform.VisionOS;
#else
            return false;
#endif
        }

        public static bool IsGLES()
        {
            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3) { return true; }
#if !UNITY_2023_1_OR_NEWER
            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2) { return true; }
#endif
            return false;
        }

        private static string GetDeviceModel()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Application.platform == RuntimePlatform.Android)
            {
                try
                {
                    using (var buildClass = new AndroidJavaClass("android.os.Build"))
                    {
                        return $"{buildClass.GetStatic<string>("BRAND")} (device={buildClass.GetStatic<string>("DEVICE")}, model={buildClass.GetStatic<string>("MODEL")})";
                    }
                }
                catch (System.Exception e) { return e.Message; }
            }
#endif
            return SystemInfo.deviceModel;
        }
    }
}
