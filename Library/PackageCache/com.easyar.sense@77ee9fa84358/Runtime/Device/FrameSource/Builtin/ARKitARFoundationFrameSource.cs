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
    /// <summary>
    /// <para xml:lang="en">A custom frame source which connects AR Foundation ARKit output to EasyAR input in the scene, providing AR Foundation support using custom camera feature of EasyAR Sense.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">``AR Foundation`` is required to use this frame source, you need to setup AR Foundation according to official documents.</para>
    /// <para xml:lang="zh">在场景中将AR Foundation 的ARKit输出连接到EasyAR输入的自定义frame source。通过EasyAR Sense的自定义相机功能提供AR Foundation支持。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// <para xml:lang="zh">为了使用这个frame source， ``AR Foundation`` 是必需的。你需要根据官方文档配置AR Foundation。</para>
    /// </summary>
    public class ARKitARFoundationFrameSource : ARFoundationFrameSource
    {
        internal protected override Optional<bool> IsAvailable =>
            Application.platform != RuntimePlatform.IPhonePlayer ? false :
            !UnityXRManager.IsARKitLoaderActive() ? false :
            base.IsAvailable;

        private protected override int CameraOrientation(bool isFront) => 90;
    }
}
