//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">External frame source which represents an external device which has 3DOF rotation tracking capability. Usually represents HMD where camera rendering and device tracking are handled in device SDK.</para>
    /// <para xml:lang="en">This frame source is one type of 3DOF rotation tracking device, and will output rotation data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">You can inherent it to implenment HMD and other device input. You have to make your own implenmentation of tracking, EasyAR 3DOF rotation tracking will not directly work on exteranl devices. You need to get image and rotation data and input into EasyAR by yourself from hardware other places. EasyAR does not provide the capability to grab data from those devices, it does provide the capability to run EasyAR features after you put data into EasyAR.</para>
    /// <para xml:lang="en">EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.</para>
    /// <para xml:lang="zh">表示具有3DOF旋转跟踪能力的外部设备的frame source。通常表示头戴设备，其中相机渲染和设备跟踪都由设备SDK完成。</para>
    /// <para xml:lang="zh">这个frame source是一种3DOF旋转跟踪设备，在<see cref="ARSession"/>中会输出旋转数据。</para>
    /// <para xml:lang="zh">你可以通过继承它来实现头显等设备输入，但你必须自己完成跟踪功能，EasyAR的3DOF旋转跟踪并不能直接在外部设备上使用。你需要自己从硬件或其它地方获取图像和旋转数据并输入到EasyAR，EasyAR不提供这些数据的获取能力，但提供将这些数据输入EasyAR之后运行EasyAR功能的能力。</para>
    /// <para xml:lang="zh">在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。</para>
    /// </summary>
    public abstract class ExternalDeviceRotationFrameSource : ExternalDeviceFrameSource
    {
        private protected override CameraTransformType CameraTransformType => CameraTransformType.ThreeDofRotOnly;
        /// <summary>
        /// <para xml:lang="en">Input render frame data.</para>
        /// <para xml:lang="en">Make sure to call every frame after device data ready, do not skip.These data should match the one driving current Unity rendering camera in the same frame.</para>
        /// <para xml:lang="zh">输入渲染帧数据。</para>
        /// <para xml:lang="zh">请确保在设备数据准备好之后每帧调用，不能跳帧。这些数据需要与驱动同一帧内当前Unity渲染相机的数据一致。</para>
        /// </summary>
        protected bool HandleRenderFrameData(double timestamp, Quaternion rotation) => OnRenderFrameData(timestamp, new Pose(Vector3.zero, rotation), MotionTrackingStatus.NotTracking);

        /// <summary>
        /// <para xml:lang="en">Input camera frame data.</para>
        /// <para xml:lang="en">Suggest to input 30 or 60 fps data. Acceptable minimum frame rate = 2, but response time of some algorithms will be affected. It can be called in any thread if all your APIs are thread safe. These data should match the one when camera sensor exposure. It is recommended to input the color data into EasyAR Sense as long as it can be obtained, which is helpful for the effect of EasyAR Mega.To get a better performance, you can design the whole data chain to let raw YUV data passthrough directly from shared memory and pass data pointer directly into EasyAR Sense.Be careful with data ownership.</para>
        /// <para xml:lang="zh">输入相机帧数据。</para>
        /// <para xml:lang="zh">建议输入30或60fps的数据。最小可接受帧率 = 2，但部分算法响应时间会受影响。它可以在任何线程调用，只要你的API都是线程安全的即可。这些数据需要与相机传感器曝光时的数据一致。只要可以获取，建议输入色彩数据到EasyAR Sense，这对EasyAR Mega的效果是有帮助的。为实现最佳效率，你可以设计整个数据链条让原始YUV数据直接通过共享内存透传，并直接使用数据指针传入EasyAR Sense。请注意数据所有权。</para>
        /// </summary>
        protected bool HandleCameraFrameData(DeviceFrameSourceCamera deviceCamera, double timestamp, Image image, CameraParameters cameraParameters, Quaternion deviceRotation)
        {
#if EASYAR_DEBUG_EXTERNAL_FRAME_SOURCE
            UnityEngine.Debug.LogWarning($"{nameof(HandleCameraFrameData)}: {cameraParameters.cameraModelType()}, ({cameraParameters.size().data_0}, {cameraParameters.size().data_1}), ({cameraParameters.focalLength().data_0}, {cameraParameters.focalLength().data_1}), ({cameraParameters.principalPoint().data_0}, {cameraParameters.principalPoint().data_1}), ({deviceCamera.AxisSystem}, {deviceCamera.Extrinsics.Inverse.position}, {deviceCamera.Extrinsics.Inverse.rotation.eulerAngles})");
            UnityEngine.Debug.LogWarning($"{nameof(HandleCameraFrameData)}: {timestamp}, {deviceRotation.eulerAngles}");
#endif
            if (cameraParameters.cameraDeviceType() != CameraDeviceType.Back)
            {
                throw new Exception($"CameraDeviceType {cameraParameters.cameraDeviceType()} not allowed");
            }

            if (!CameraFrameStarted) { return false; }
            var firstCamera = ((DeviceCameras != null && deviceCamera != null && DeviceCameras.Contains(deviceCamera)) ? DeviceCameras[0] as DeviceFrameSourceCamera : null) ?? throw new Exception($"{GetType()} implementation error, please contact the plugin developer.");
            if (deviceCamera != firstCamera)
            {
                Debug.LogWarning($"Ignore camera frame data from extra camera: multi-cam input is not supported in this version, please contact EasyAR for details.");
                return false;
            }

            if (deviceCamera.Extrinsics.Inverse != Pose.identity)
            {
                DiagnosticsController.TryShowDiagnosticsError(gameObject, "non-identity extrinsics is not supported yet, please wait for a new release");
                return false;
            }

            var frame = InputFrameHelper.TryCreateThreeDofRotOnly(timestamp, image, cameraParameters, deviceRotation);
            if (frame.OnNone)
            {
                DiagnosticsController.TryShowDiagnosticsError(gameObject, "fail to create input frame");
                return false;
            }
            using (frame.Value)
            {
                return HandleCameraFrameData(frame.Value, CameraTransformType);
            }
        }
    }
}
