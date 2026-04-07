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
    /// <para xml:lang="en">A custom frame source which connects AREngine camera device output to EasyAR input in the scene, providing AR Engine support using custom camera feature of EasyAR Sense.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">``Huawei AR Engine Unity SDK`` is NOT required to use this frame source.</para>
    /// <para xml:lang="zh">在场景中将AREngine相机设备的输出连接到EasyAR输入的自定义frame source。通过EasyAR Sense的自定义相机功能提供华为AR Engine支持。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// <para xml:lang="zh">这个frame source不使用 ``华为 AR Engine Unity SDK`` ，无需添加。</para>
    /// </summary>
    public class AREngineFrameSource : FrameSource, FrameSource.ISenseExternalFrameSource, FrameSource.IMotionTrackingDevice
    {
        /// <summary>
        /// <para xml:lang="en">Desired focus mode. Only effective if modified before the <see cref="DeviceOpened"/> event or OnEnable.</para>
        /// <para xml:lang="en">Note: focus switch may not work on some devices due to hardware or system limitation.</para>
        /// <para xml:lang="zh">期望的对焦模式，在<see cref="DeviceOpened"/>事件或OnEnable前修改才有效。</para>
        /// <para xml:lang="zh">注意：受硬件或系统限制，对焦开关在一些设备上可能无效。</para>
        /// </summary>
        public AREngineCameraDeviceFocusMode DesiredFocusMode;

        [SerializeField, HideInInspector]
        private Camera cameraCandidate;

        private bool willOpen;
        private Optional<bool> delayedOpen;
#if UNITY_ANDROID && !UNITY_EDITOR
        private bool started;
        private arengineinterop.AREngineCameraDevice device;
#endif

        /// <summary>
        /// <para xml:lang="en">Event when device opened. The bool value indicates if open success.</para>
        /// <para xml:lang="zh">设备打开的事件，bool值表示是否成功。</para>
        /// </summary>
        public event Action<bool, PermissionStatus, string> DeviceOpened;
        /// <summary>
        /// <para xml:lang="en">Event when device closed.</para>
        /// <para xml:lang="zh">设备关闭的事件。</para>
        /// </summary>
        public event Action DeviceClosed;

        /// <summary>
        /// <para xml:lang="en">Whether camera is opened.</para>
        /// <para xml:lang="zh">相机是否打开。</para>
        /// </summary>
        public bool Opened =>
#if UNITY_ANDROID && !UNITY_EDITOR
            device != null;
#else
            default;
#endif

        /// <summary>
        /// <para xml:lang="en">Frame rate range. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">帧率范围。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public Vector2 FrameRateRange =>
#if UNITY_ANDROID && !UNITY_EDITOR
            new((float)device.frameRateRangeLower(), (float)device.frameRateRangeUpper());
#else
            default;
#endif

        /// <summary>
        /// <para xml:lang="en">Current preview size. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">当前图像大小。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public Optional<Vector2Int> Size
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                var size = device.size();
                if (size.OnNone) { return Optional<Vector2Int>.Empty; }
                return new Vector2Int(size.Value.data_0, size.Value.data_1);
#else
                return Optional<Vector2Int>.Empty;
#endif
            }
        }

        /// <summary>
        /// <para xml:lang="en"><see cref="Camera"/> candidate. Only effective if Unity XR Origin is not used. Camera.main will be used if not specified.</para>
        /// <para xml:lang="zh"><see cref="Camera"/>的备选，仅当未使用Unity XR Origin时有效，如未设置会使用Camera.main。</para>
        /// </summary>
        public Camera CameraCandidate { get => cameraCandidate; set => cameraCandidate = value; }

        internal override bool IsManuallyDisabled => Application.platform == RuntimePlatform.Android && EasyARSettings.Instance && EasyARSettings.Instance.AREngineSDK == EasyARSettings.AREngineType.Disabled;
        GameObject IMotionTrackingDevice.Origin => XROriginCache.DefaultOrigin(true);
        internal protected override bool IsHMD => false;
        internal protected override Camera Camera => XROriginCache.DefaultCamera(true) ? XROriginCache.DefaultCamera(false) : (cameraCandidate ? cameraCandidate : Camera.main);
        internal protected override bool IsCameraUnderControl => true;
        internal protected override IDisplay Display => easyar.Display.DefaultSystemDisplay;
        internal protected override bool CameraFrameStarted =>
#if UNITY_ANDROID && !UNITY_EDITOR
            started && device != null && device.size().OnSome;
#else
            false;
#endif
        internal protected override List<FrameSourceCamera> DeviceCameras =>
#if UNITY_ANDROID && !UNITY_EDITOR
            Opened && CameraFrameStarted && Size.OnSome ? new List<FrameSourceCamera> { new((CameraDeviceType)device.type(), device.cameraOrientation(), Size.Value, FrameRateRange) } : new();
#else
            new();
#endif

        internal protected override Optional<bool> IsAvailable =>
#if UNITY_ANDROID && !UNITY_EDITOR
            arengineinterop.AREngineCameraDevice.isAvailable();
#else
            false;
#endif
        internal override int BufferCapacity
        {
            get =>
#if UNITY_ANDROID && !UNITY_EDITOR
                device?.bufferCapacity() ??
#endif
                base.BufferCapacity;

            set
            {
                base.BufferCapacity = value;
#if UNITY_ANDROID && !UNITY_EDITOR
                device?.setBufferCapacity(value);
#endif
            }
        }

        /// <summary>
        /// <para xml:lang="en">Start/Stop video stream capture when <see cref="ARSession"/> is running. Capture will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止采集视频流数据。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始采集。</para>
        /// </summary>
        private void OnEnable()
        {
            if (delayedOpen == true) { Open(); }
#if UNITY_ANDROID && !UNITY_EDITOR
            device?.setFocusMode((arengineinterop.AREngineCameraDeviceFocusMode)DesiredFocusMode);
            started = device?.start() ?? false;
#endif
        }

        private void OnDisable()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            started = false;
            device?.stop();
#endif
        }

        private void OnApplicationPause(bool pause)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (pause)
            {
                device?.onPause();
            }
            else
            {
                device?.onResume();
            }
#endif
        }

        /// <summary>
        /// <para xml:lang="en">Open device. If neither <see cref="Open"/> nor <see cref="Close"/> is called manually, <see cref="Open"/> will be automatically invoked upon <see cref="ARSession"/> startup.</para>
        /// <para xml:lang="zh">打开设备。如果未手动调用<see cref="Open"/>和<see cref="Close"/>，<see cref="ARSession"/>启动后会自动<see cref="Open"/>。</para>
        /// </summary>
        public void Open()
        {
            delayedOpen = false;
            willOpen = true;
            CameraDevice.requestPermissions(EasyARController.Scheduler, (Action<PermissionStatus, string>)((status, msg) =>
            {
                if (!willOpen)
                {
                    DeviceOpened?.Invoke(false, status, "closed");
                    return;
                }
                if (status != PermissionStatus.Granted)
                {
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, $"Camera permission {status}, {msg}");
                    DeviceOpened?.Invoke(false, status, msg);
                    return;
                }

                Close();

#if UNITY_ANDROID && !UNITY_EDITOR
                device = new arengineinterop.AREngineCameraDevice();
                device.setHighResMode(true);

                if (base.BufferCapacity != 0)
                {
                    device.setBufferCapacity(base.BufferCapacity);
                }

                device.setInputFrameHandler((aFrame) =>
                {
                    using (var aImage = aFrame.image())
                    {
                        var aBuffer = aImage.buffer();
                        using (var buffer = Buffer.wrap(aBuffer.data(), aBuffer.size(), () => aBuffer.Dispose()))
                        using (var image = Image.create(buffer, (PixelFormat)aImage.format(), aImage.width(), aImage.height(), aImage.pixelWidth(), aImage.pixelHeight()))
                        using (var acp = aFrame.cameraParameters())
                        {
                            var acpSize = acp.size();
                            var acpFocalLength = acp.focalLength();
                            var acpPrincipalPoint = acp.principalPoint();
                            using (var cp = new CameraParameters(new Vec2I(acpSize.data[0], acpSize.data[1]), new Vec2F(acpFocalLength.data[0], acpFocalLength.data[1]), new Vec2F(acpPrincipalPoint.data[0], acpPrincipalPoint.data[1]), (CameraDeviceType)acp.cameraDeviceType(), acp.cameraOrientation()))
                            {
                                var act = aFrame.cameraTransform();
                                var ct = new Matrix44F(
                                    act.data[0], act.data[1], act.data[2], act.data[3],
                                    act.data[4], act.data[5], act.data[6], act.data[7],
                                    act.data[8], act.data[9], act.data[10], act.data[11],
                                    act.data[12], act.data[13], act.data[14], act.data[15]
                                );
                                using (var frame = InputFrame.tryCreate(image, cp, aFrame.timestamp(), ct, CameraTransformType.SixDof, (MotionTrackingStatus)aFrame.trackingStatus()).Value)
                                {
                                    ((ISenseExternalFrameSource)this).HandleCameraFrameData(frame);
                                }
                            }
                        }
                    }
                });
#endif
                DeviceOpened?.Invoke(true, status, msg);

                if (enabled)
                {
                    OnEnable();
                }
            }));
        }

        /// <summary>
        /// <para xml:lang="en">Close device.</para>
        /// <para xml:lang="zh">关闭设备。</para>
        /// </summary>
        public void Close()
        {
            delayedOpen = false;
            willOpen = false;
#if UNITY_ANDROID && !UNITY_EDITOR
            if (device != null)
#endif
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                OnDisable();
                device.close();
                device.Dispose();
                device = null;
#endif
                DeviceClosed?.Invoke();
            }
        }

        internal protected override void OnSessionStart(ARSession session)
        {
            if (delayedOpen.OnNone) { delayedOpen = true; }
            if (enabled) { OnEnable(); }
        }

        internal protected override void OnSessionStop()
        {
            Close();
            delayedOpen = Optional<bool>.Empty;
        }

        internal override string DumpLite()
        {
            var data = string.Empty;
            data += $"Source: {ARSessionFactory.DefaultName(GetType())} ({enabled}), {(Opened ? Size : "-")}, {(Opened ? FrameRateRange : "-")}" + Environment.NewLine;
            return data;
        }
    }

    public enum AREngineCameraDeviceFocusMode
    {
        /// <summary>
        /// <para xml:lang="en">Auto focus mode</para>
        /// <para xml:lang="zh">自动对焦模式</para>
        /// </summary>
        Auto = 0,
        /// <summary>
        /// <para xml:lang="en">Fixed focus mode</para>
        /// <para xml:lang="zh">固定对焦模式</para>
        /// </summary>
        Fixed = 1,
    }
}
