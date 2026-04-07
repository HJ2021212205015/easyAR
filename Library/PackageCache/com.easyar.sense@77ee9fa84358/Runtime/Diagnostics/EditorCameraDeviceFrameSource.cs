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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="CameraDevice"/> in the Unity Editor, only used for diagnostics purpose during developing. Usually everything you see does not represent real effect on your device when this frame source is used. You can develop app logic which is not relevant to AR effects, but can not judge AR running (detecting, tracking, localizing, etc.) effect  according to the output</para>
    /// <para xml:lang="en">This frame source is not a motion tracking device, and will not output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="zh">在编辑器中控制<see cref="CameraDevice"/>的<see cref="MonoBehaviour"/>，仅用来提供开发诊断使用。通常当这个frame source在使用的时候，你看到的所有效果都和设备上运行是不同的。你可以使用它做一些AR效果无关的应用逻辑开发，但不能凭它判断AR运行（检测、跟踪、定位等）的效果。</para>
    /// <para xml:lang="zh">这个frame source不是运动跟踪设备，在<see cref="ARSession"/>中不会输出运动数据。</para>
    /// </summary>
    public class EditorCameraDeviceFrameSource : CameraDeviceFrameSource
    {
        internal protected override Optional<bool> IsAvailable => Application.isEditor && CameraDevice.isAvailable();

        internal protected override void OnSessionStart(ARSession session)
        {
            base.OnSessionStart(session);

            session.Diagnostics.EnqueueCaution($"{ARSessionFactory.DefaultName<EditorCameraDeviceFrameSource>()} is enabled for diagnostics in Unity editor. Everything you see does not represent real effect on your device.");
        }
    }
}
