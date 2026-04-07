//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">A external frame source which only accept image stream input.</para>
    /// <para xml:lang="en">This frame source is not a motion tracking device, and will not output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">You can inherent it to implenment custom camera which has image input only. It cannot be used for HMD input. You need to get image and input into EasyAR by yourself from hardware other places like usb connected camera on Android. EasyAR does not provide the capability to grab data from those devices, it does provide the capability to run EasyAR features after you put data into EasyAR.</para>
    /// <para xml:lang="en">EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.</para>
    /// <para xml:lang="zh">一个只接收图像流输入的外部frame source。</para>
    /// <para xml:lang="zh">这个frame source不是运动跟踪设备，在<see cref="ARSession"/>中不会输出运动数据。</para>
    /// <para xml:lang="zh">你可以通过继承它来实现只有图像输入的自定义相机，它不能用于实现头显输入。你需要自己从硬件或其它地方（比如usb连接的Android相机）获取图像并输入到EasyAR，EasyAR不提供这些数据的获取能力，但提供将这些数据输入EasyAR之后运行EasyAR功能的能力。</para>
    /// <para xml:lang="zh">在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。</para>
    /// </summary>
    public abstract class ExternalImageStreamFrameSource : ExternalFrameSource
    {
        private protected override CameraTransformType CameraTransformType => CameraTransformType.ZeroDof;

        /// <summary>
        /// <para xml:lang="en">Input camera frame data.</para>
        /// <para xml:lang="zh">输入相机帧数据。</para>
        /// </summary>
        protected bool HandleCameraFrameData(double timestamp, Image image, CameraParameters cameraParameters)
        {
#if EASYAR_DEBUG_EXTERNAL_FRAME_SOURCE
            UnityEngine.Debug.LogWarning($"{nameof(HandleCameraFrameData)}: {cameraParameters.cameraModelType()}, ({cameraParameters.size().data_0}, {cameraParameters.size().data_1}), ({cameraParameters.focalLength().data_0}, {cameraParameters.focalLength().data_1}), ({cameraParameters.principalPoint().data_0}, {cameraParameters.principalPoint().data_1})");
            UnityEngine.Debug.LogWarning($"{nameof(HandleCameraFrameData)}: {timestamp}");
#endif
            if (!CameraFrameStarted) { return false; }
            using (var frame = InputFrameHelper.CreateZeroDof(timestamp, image, cameraParameters))
            {
                return HandleCameraFrameData(frame, CameraTransformType);
            }
        }
    }
}
