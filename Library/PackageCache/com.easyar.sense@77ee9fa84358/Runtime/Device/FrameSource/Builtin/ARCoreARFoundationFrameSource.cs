//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">A custom frame source which connects AR Foundation ARCore output to EasyAR input in the scene, providing AR Foundation support using custom camera feature of EasyAR Sense.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">``AR Foundation`` is required to use this frame source, you need to setup AR Foundation according to official documents.</para>
    /// <para xml:lang="zh">在场景中将AR Foundation 的ARCore输出连接到EasyAR输入的自定义frame source。通过EasyAR Sense的自定义相机功能提供AR Foundation支持。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// <para xml:lang="zh">为了使用这个frame source， ``AR Foundation`` 是必需的。你需要根据官方文档配置AR Foundation。</para>
    /// </summary>
    public class ARCoreARFoundationFrameSource : ARFoundationFrameSource
    {
        /// <summary>
        /// <para xml:lang="en">The default configuration used by AR Foundation is not optimal for EasyAR Mega. This option allows for runtime optimization of ARCameraManager.currentConfiguration.</para>
        /// <para xml:lang="en">Warning: Some phones, such as the Xiaomi Mi 10, have inherent bugs that prevent image acquisition after configuration modifications. EasyAR will be unable to function in such cases. When using this option, you need to avoid similar devices or implement suitable handling measures.</para>
        /// <para xml:lang="en">If the image size is modified during the <see cref="FrameRecorder"/> recording process, the recording data will stop updating. You will need to stop and restart the recording.</para>
        /// <para xml:lang="zh">AR Foundation默认使用的配置对EasyAR Mega来说并不是最优的，这个选项可以在运行时优化ARCameraManager.currentConfiguration。</para>
        /// <para xml:lang="zh">警告：部分手机自身（比如小米10）存在bug，在修改配置之后无法获取图像，EasyAR将无法使用。使用该选项时你需要避开类似的手机或进行合理处理。</para>
        /// <para xml:lang="zh">如果在<see cref="FrameRecorder"/>录制过程中修改了图像大小，录制数据将停止更新，需要关闭之后重新录制。</para>
        /// </summary>
        [Tooltip("The default configuration used by AR Foundation is not optimal for EasyAR Mega. This option allows for runtime optimization of ARCameraManager.currentConfiguration.")]
        [SerializeField, HideInInspector]
        public bool OptimizeConfigurationForTracking;

        private Optional<bool> enableColorInput;
        private bool loadLibrary;

        /// <summary>
        /// <para xml:lang="en">ARCameraManager.currentConfiguration choosed event when <see cref="OptimizeConfigurationForTracking"/> is true.</para>
        /// <para xml:lang="zh"><see cref="OptimizeConfigurationForTracking"/>为true时，ARCameraManager.currentConfiguration选择的事件。</para>
        /// </summary>
#pragma warning disable 67
        public event Action ConfigurationChoosed;
#pragma warning restore 67

        internal protected override Optional<bool> IsAvailable =>
            Application.platform != RuntimePlatform.Android ? false :
            !UnityXRManager.IsARCoreLoaderActive() ? false :
            base.IsAvailable;

        private protected override bool EnableColorInput
        {
            get
            {
                if (enableColorInput.OnSome) { return enableColorInput.Value; }

                if (!loadLibrary)
                {
                    ARCoreFrameSource.LoadLibrary();
                    loadLibrary = true;
                }
                enableColorInput = ARCoreCameraDevice.isAvailable();
                return enableColorInput.Value;
            }
        }

        private protected override IEnumerator ChooseConfig(arfoundation.CameraHandler cameraHandler)
        {
            if (!OptimizeConfigurationForTracking) { yield break; }
            yield return cameraHandler.ChooseConfig(960);
            ConfigurationChoosed?.Invoke();
        }

        private protected override int CameraOrientation(bool isFront)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cameraInfo = new AndroidJavaObject("android.hardware.Camera$CameraInfo"))
            using (var cameraClass = new AndroidJavaClass("android.hardware.Camera"))
            {
                cameraClass.CallStatic("getCameraInfo", isFront ? 1 : 0, cameraInfo);
                return cameraInfo.Get<int>("orientation");
            }
#else
            return 0;
#endif
        }
    }
}
