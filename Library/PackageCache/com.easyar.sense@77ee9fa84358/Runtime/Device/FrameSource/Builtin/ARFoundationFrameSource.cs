//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">A custom frame source which connects AR Foundation output to EasyAR input in the scene, providing AR Foundation support using custom camera feature of EasyAR Sense.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">``AR Foundation`` is required to use this frame source, you need to setup AR Foundation according to official documents.</para>
    /// <para xml:lang="zh">在场景中将AR Foundation 的输出连接到EasyAR输入的自定义frame source。通过EasyAR Sense的自定义相机功能提供AR Foundation支持。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// <para xml:lang="zh">为了使用这个frame source， ``AR Foundation`` 是必需的。你需要根据官方文档配置AR Foundation。</para>
    /// </summary>
    public abstract class ARFoundationFrameSource : FrameSource, FrameSource.ISenseExternalFrameSource, FrameSource.IMotionTrackingDevice
    {
        private static IReadOnlyList<ARSession.ARCenterMode> centerModes = new List<ARSession.ARCenterMode> { ARSession.ARCenterMode.SessionOrigin, ARSession.ARCenterMode.FirstTarget, ARSession.ARCenterMode.SpecificTarget };
        private arfoundation.CameraHandler cameraHandler;
        private Action<Pose?> newFrame;
        private Optional<bool> isAvailable;
        private ARSession session;
        private FrameSourceCamera deviceCamera;
        private int cameraOrientation;
        private bool configChoosed;
        private BufferPoolWrapper bufferPool = new BufferPoolWrapper();

#if UNITY_VISIONOS
        internal override bool IsManuallyDisabled => true;
#else
        internal override bool IsManuallyDisabled => false;
#endif
        GameObject IMotionTrackingDevice.Origin => XROriginCache.XROrigin(true);
        internal protected override bool IsCameraUnderControl => false;
        internal protected override bool IsHMD => false;
        internal protected override IDisplay Display => easyar.Display.DefaultSystemDisplay;
        internal protected override Optional<bool> IsAvailable => isAvailable;
        internal protected override IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode => centerModes;
        internal protected override bool CameraFrameStarted => deviceCamera != null;
        internal protected override List<FrameSourceCamera> DeviceCameras => new List<FrameSourceCamera> { deviceCamera };

        internal protected override Camera Camera => XROriginCache.XRCamera(true)
            && arfoundation.CameraHandler.ValidateCamera(XROriginCache.XRCamera(false))
            ? XROriginCache.XRCamera(false) : null;

        internal override int BufferCapacity
        {
            get => base.BufferCapacity;
            set
            {
                base.BufferCapacity = value;
                bufferPool.BufferCapacity = value;
            }
        }

        private protected virtual bool EnableColorInput => true;

        /// <summary>
        /// <para xml:lang="en">Start/Stop send image stream to EasyAR when <see cref="ARSession"/> is running. Sending will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止发送图像流数据到EasyAR。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始发送。</para>
        /// </summary>
        private protected virtual void OnEnable()
        {
            if (cameraHandler != null)
            {
                cameraHandler.Start();
                Application.onBeforeRender += OnBeforeRender;
            }
        }

        [BeforeRenderOrder(100)]
        private protected virtual void OnBeforeRender()
        {
            newFrame?.Invoke((!session || session.Assembly == null || !session.Assembly.Camera) ? null : new Pose(session.Assembly.Camera.transform.localPosition, session.Assembly.Camera.transform.localRotation));
            newFrame = null;
        }

        private protected virtual void OnDisable()
        {
            cameraHandler?.Stop();
            Application.onBeforeRender -= OnBeforeRender;
            deviceCamera?.Dispose();
            deviceCamera = null;
        }

        private protected virtual void OnDestroy()
        {
            bufferPool?.Dispose();
            OnSessionStop();
        }

        private protected virtual IEnumerator ChooseConfig(arfoundation.CameraHandler cameraHandler) => null;
        private protected virtual int CameraOrientation(bool isFront) => 0;

        internal protected override void OnSessionStart(ARSession session)
        {
            this.session = session;
            cameraOrientation = CameraOrientation(false);
            cameraHandler = arfoundation.CameraHandler.Create(session.Assembly.Camera, OnCameraData);
            StartCoroutine(ChooseARFoundationConfig());
            if (enabled)
            {
                OnEnable();
            }
        }

        internal protected override void OnSessionStop()
        {
            OnDisable();
            StopAllCoroutines();
            configChoosed = false;
            session = null;
            cameraHandler = null;
        }

        internal protected override IEnumerator CheckAvailability()
        {
            yield return arfoundation.CameraHandler.CheckAvailability((available) =>
            {
                isAvailable = available;
            });
        }

        private unsafe void OnCameraData(double timestamp, List<arfoundation.ImagePlane> planes, arfoundation.CameraDescription cameraDescription, bool isTracking)
        {
            if (!session || session.Assembly == null) { return; }
            if (configChoosed && deviceCamera == null)
            {
                deviceCamera = new FrameSourceCamera(CameraDeviceType.Back, cameraOrientation, cameraDescription.size, new Vector2(cameraDescription.frameRate, cameraDescription.frameRate));
            }

            var screenRotation = session.Assembly.Display.Rotation;
            var trackingStatus = isTracking ? MotionTrackingStatus.Tracking : MotionTrackingStatus.NotTracking;
            Vector2Int size = cameraDescription.size;
            Vector2Int pixelSize;
            PixelFormat pixelFormat;
            Buffer buffer;

            {
                var Y = new IntPtr(planes[0].data.GetUnsafePtr());
                var U = IntPtr.Zero;
                var V = IntPtr.Zero;
                var YLen = planes[0].data.Length;
                var ULen = 0;
                var VLen = 0;

                if (!EnableColorInput || planes.Count == 1)
                {
                    pixelSize = new Vector2Int(planes[0].rowStride, size.y);
                    pixelFormat = PixelFormat.Gray;
                }
                else if (planes.Count == 2)
                {
                    ULen = planes[1].data.Length;
                    U = new IntPtr(planes[1].data.GetUnsafePtr());
                    pixelSize = ImageUtil.GetPixelSize(Y, U, U + 1, planes[0].rowStride);
                    pixelFormat = PixelFormat.YUV_NV12;
                }
                else if (planes.Count == 3)
                {
                    ULen = planes[1].data.Length;
                    VLen = planes[2].data.Length;
                    U = new IntPtr(planes[1].data.GetUnsafePtr());
                    V = new IntPtr(planes[2].data.GetUnsafePtr());
                    pixelSize = ImageUtil.GetPixelSize(Y, U, V, planes[0].rowStride);
                    pixelFormat = ImageUtil.CheckPixelFormat(Y, U, V, planes[0].pixelStride, planes[1].pixelStride, planes[2].pixelStride, planes[0].rowStride, planes[1].rowStride, planes[2].rowStride);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported format");
                }
                var yLen = pixelSize.x * pixelSize.y;
                var uvLen = yLen / 2;
                var bufferBlockSize = (pixelFormat == PixelFormat.Gray) ? yLen : uvLen * 3;

                var bufferO = bufferPool.TryAcquireBuffer(bufferBlockSize);
                if (bufferO.OnNone) { return; }

                buffer = bufferO.Value;
                ImageUtil.FillImageBuffer(Tuple.Create(Y, YLen), Tuple.Create(U, ULen), Tuple.Create(V, VLen), pixelSize, pixelFormat, buffer);
            }

            newFrame?.Invoke(null);
            newFrame = (pose) =>
            {
                if (!pose.HasValue)
                {
                    using (buffer)
                    {
                        return;
                    }
                }
                using (var cameraParameters = new CameraParameters(size.ToEasyARVector(), cameraDescription.focalLength.ToEasyARVector(), cameraDescription.principalPoint.ToEasyARVector(), CameraDeviceType.Back, cameraOrientation))
                using (buffer)
                using (var image = Image.create(buffer, pixelFormat, size.x, size.y, pixelSize.x, pixelSize.y))
                {
                    var displayCompensation = Quaternion.Euler(0, 0, -cameraParameters.imageOrientation(screenRotation));
                    var pu = new Pose(Vector3.zero, displayCompensation).GetTransformedBy(pose.Value);
                    var frame = InputFrameHelper.TryCreateSixDof(timestamp * 1e-9, image, cameraParameters, pu, trackingStatus).ValueOrDefault(null);
                    if (frame == null)
                    {
                        session.Diagnostics.EnqueueWarning("fail to create input frame");
                        return;
                    }
                    using (frame)
                    {
                        ((ISenseExternalFrameSource)this).HandleCameraFrameData(frame);
                    }
                }
            };
        }

        IEnumerator ChooseARFoundationConfig()
        {
            if (configChoosed) { yield break; }
            var choose = ChooseConfig(cameraHandler);
            if (choose != null)
            {
                yield return choose;
            }
            configChoosed = true;
        }

        internal override string DumpLite()
        {
            var data = string.Empty;
            data += $"Source: {ARSessionFactory.DefaultName(GetType())} ({enabled}), {(CameraFrameStarted ? deviceCamera.FrameSize : "-")}, {(CameraFrameStarted ? deviceCamera.FrameRateRange.x : "-")}" + Environment.NewLine;
            return data;
        }
    }
}
