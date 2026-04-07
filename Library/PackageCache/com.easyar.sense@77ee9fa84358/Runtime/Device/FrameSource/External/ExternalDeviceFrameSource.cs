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
    /// <para xml:lang="en">External frame source which represents an external device. Usually represents HMD where camera rendering and device tracking are handled in device SDK.</para>
    /// <para xml:lang="en">You can inherent sub types of <see cref="ExternalDeviceFrameSource"/> to implenment custom camera, but you cannot inherent <see cref="ExternalDeviceFrameSource"/> directly. Custom camera represnets a new device or data input method usaully.</para>
    /// <para xml:lang="en">EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.</para>
    /// <para xml:lang="zh">表示外部设备的frame source。通常表示头戴设备，其中相机渲染和设备跟踪都由设备SDK完成。</para>
    /// <para xml:lang="zh">你可以通过继承<see cref="ExternalDeviceFrameSource"/>的子类型来实现自定义相机，但你不能直接继承<see cref="ExternalDeviceFrameSource"/>。自定义相机通常表达一个新的设备或新的数据输入方式。</para>
    /// <para xml:lang="zh">在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。</para>
    /// </summary>
    public abstract class ExternalDeviceFrameSource : ExternalFrameSource, FrameSource.IMotionTrackingDevice, FrameSource.ISyncMotionSource
    {
        private readonly IReadOnlyList<ARSession.ARCenterMode> centerModeHasOrigin = new List<ARSession.ARCenterMode> { ARSession.ARCenterMode.SessionOrigin, ARSession.ARCenterMode.FirstTarget, ARSession.ARCenterMode.SpecificTarget };
        private readonly IReadOnlyList<ARSession.ARCenterMode> centerModeNoOrigin = new List<ARSession.ARCenterMode> { ARSession.ARCenterMode.SessionOrigin };
        private Action<MotionInputData> motionUpdate;

        event Action<MotionInputData> ISyncMotionSource.MotionUpdate
        {
            add => motionUpdate += value;
            remove => motionUpdate -= value;
        }

        /// <summary>
        /// <para xml:lang="en">Device origin type.</para>
        /// <para xml:lang="en">The session origin is used to setup transform base in <see cref="ARSession.ARCenterMode.SessionOrigin"/> center mode and to transform camera-origin pair together in other center modes.If your SDK is based on Untiy XR framework, you would be familiar with XR.CoreUtils.XROrigin. Origin is something similar to XR.CoreUtils.XROrigin, it holds the rendering camera as its child.Users can move origin in the scene without losing local motion defined by camera-origin pair. EasyAR will make use of camera-origin pair to handle target center mode, especially useful when Mega is running which defines the real-world coordinate of the planet, while camera-origin pair usually defines VIO coordinate relative to some start point.</para>
        /// <para xml:lang="zh">设备原点类型。</para>
        /// <para xml:lang="zh">session origin用于设置<see cref="ARSession.ARCenterMode.SessionOrigin"/>中心模式下的 transform 基准点，并用于在其它中心模式下同时变换 camera-origin 对。如果你的SDK是基于Untiy XR框架进行设计的，你会对 XR.CoreUtils.XROrigin 比较熟悉。Origin就是类似 XR.CoreUtils.XROrigin 的东西，渲染相机是它的儿子节点。用户可以在场景中移动 origin ，而不损失由 camera-origin对定义的局部运动关系。EasyAR会使用camera-origin对来处理target中心模式，这在运行Mega时尤其有用，因为在Mega中定义了整个地球的现实世界坐标系，而 camera-origin对通常定义的是相对于某个启动点的VIO坐标系。</para>
        /// </summary>
        internal protected enum DeviceOriginType
        {
            /// <summary>
            /// <para xml:lang="en">Device SDK does not define origin. Origin will be selected from scene or created, but it will never move.</para>
            /// <para xml:lang="en">You will lose flexibility especially how the object moves and support less center mode.App developers have to be careful about where their virtual objects are because EasyAR objects will always move when using this class. Objects put directly under Unity world coordinates will never show in the right place in any configuration.</para>
            /// <para xml:lang="zh">设备SDK未定义原点。这时原点将会被自动从场景中选择或创建，但不会移动。</para>
            /// <para xml:lang="zh">你会损失一些灵活性，尤其是只能支持有限的中心模式，物体的移动方式也会随之受限。应用开发者必须对于他们如何摆放虚拟物体十分小心，因为在使用这个类的时候EasyAR节点和物体永远都会动。所有放在Unity世界坐标系下的物体在任何配置下都永远不可能显示在正确的位置。</para>
            /// </summary>
            None,
            /// <summary>
            /// <para xml:lang="en">Device SDK define its own origin.</para>
            /// <para xml:lang="zh">设备SDK定义了自己的原点。</para>
            /// </summary>
            Custom,
            /// <summary>
            /// <para xml:lang="en">Device SDK use Unity.XR.CoreUtils.XROrigin as origin.</para>
            /// <para xml:lang="zh">设备SDK使用 Unity.XR.CoreUtils.XROrigin 作为原点。</para>
            /// </summary>
            XROrigin,
        }

        GameObject IMotionTrackingDevice.Origin => OriginType switch
        {
            DeviceOriginType.None => XROriginCache.DefaultOrigin(true),
            DeviceOriginType.XROrigin => XROriginCache.XROrigin(true),
            DeviceOriginType.Custom => XROriginCache.ExternalOrigin(Origin, true),
            _ => throw new NotSupportedException($"{OriginType}"),
        };
        internal protected override IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode => OriginType == DeviceOriginType.None ? centerModeNoOrigin : centerModeHasOrigin;
        internal protected override bool IsCameraUnderControl => false;

        internal protected override Camera Camera => OriginType switch
        {
            DeviceOriginType.XROrigin => XROriginCache.XRCamera(true),
            _ => throw new NotImplementedException($"Please implement {nameof(FrameSource)}.Camera"),
        };

        /// <summary>
        /// <para xml:lang="en">Device origin type.</para>
        /// <para xml:lang="zh">设备原点类型。</para>
        /// </summary>
        internal protected abstract DeviceOriginType OriginType { get; }
        /// <summary>
        /// <para xml:lang="en">Device origin. You need to define your own origin when <see cref="OriginType "/> is <see cref="DeviceOriginType.Custom"/>. It is not required to re-define origin in other cases.</para>
        /// <para xml:lang="zh">设备原点。你需要在<see cref="OriginType "/>为<see cref="DeviceOriginType.Custom"/>时定义自己的原点，其它时候不需要重新定义。</para>
        /// </summary>
        protected virtual GameObject Origin => OriginType switch
        {
            DeviceOriginType.None => ((IMotionTrackingDevice)this).Origin,
            DeviceOriginType.XROrigin => ((IMotionTrackingDevice)this).Origin,
            _ => throw new NotImplementedException($"Please implement {nameof(ExternalDeviceFrameSource)}.Origin"),
        };

        // NOTICE: EasyAR Sense API may change in the near future
        private protected bool OnRenderFrameData(double timestamp, Pose deviceToOriginTransform, MotionTrackingStatus trackingStatus)
        {
#if EASYAR_DEBUG_EXTERNAL_FRAME_SOURCE
            Debug.LogWarning($"{nameof(OnRenderFrameData)}: {timestamp}, {trackingStatus}, {deviceToOriginTransform.position}, {deviceToOriginTransform.rotation.eulerAngles}");
#endif
            if (!CameraFrameStarted) { return false; }
            var deviceCamera = ((DeviceCameras != null && DeviceCameras.Count > 0) ? DeviceCameras[0] as DeviceFrameSourceCamera : null) ?? throw new Exception($"{GetType()} implementation error, please contact the plugin developer.");

            var extrinsics = deviceCamera.Extrinsics.Inverse;
            Pose cameraTransform;
            if (deviceCamera.AxisSystem == AxisSystemType.Unity)
            {
                cameraTransform = extrinsics.GetTransformedBy(deviceToOriginTransform);
            }
            else if (deviceCamera.AxisSystem == AxisSystemType.EasyAR)
            {
                cameraTransform = PoseUtility.createPoseWithHardwareOffset(deviceToOriginTransform.position.ToEasyARVector(), deviceToOriginTransform.rotation.ToEasyARQuaternion(), extrinsics.position.ToEasyARVector(), extrinsics.rotation.ToEasyARQuaternion()).Value.ToUnityPose();
            }
            else
            {
                throw new NotSupportedException(deviceCamera.AxisSystem.ToString());
            }
            var ePose = cameraTransform.ToEasyARAxisSystem();
            var frame = MotionInputData.tryCreateSixDof(timestamp, ePose.position.ToEasyARVector(), ePose.rotation.ToEasyARQuaternion(), trackingStatus);
            if (frame.OnNone)
            {
                DiagnosticsController.TryShowDiagnosticsError(gameObject, "fail to create motion input frame");
                return false;
            }

            using (frame.Value)
            {
                motionUpdate?.Invoke(frame.Value);
            }
            return true;
        }
    }
}
