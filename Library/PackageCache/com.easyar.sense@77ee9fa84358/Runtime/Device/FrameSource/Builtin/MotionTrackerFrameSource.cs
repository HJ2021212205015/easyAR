//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="MotionTrackerCameraDevice"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="MotionTrackerCameraDevice"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// </summary>
    public class MotionTrackerFrameSource : FrameSource, FrameSource.ISenseBuiltinFrameSource, FrameSource.IMotionTrackingDevice
    {
        private MotionTrackerCameraDevice device;
        [SerializeField, HideInInspector]
        private Camera cameraCandidate;
        [HideInInspector, SerializeField]
        private bool hasDesiredFocusMode;
        [HideInInspector, SerializeField]
        MotionTrackerCameraDeviceFocusMode desiredFocusMode;
        [HideInInspector, SerializeField]
        private bool hasDesiredResolution;
        [HideInInspector, SerializeField]
        MotionTrackerCameraDeviceResolution desiredResolution;
        [HideInInspector, SerializeField]
        private bool hasDesiredFrameRate;
        [HideInInspector, SerializeField]
        MotionTrackerCameraDeviceFPS desiredFrameRate;
        [HideInInspector, SerializeField]
        private bool hasDesiredMinQualityLevel;
        [HideInInspector, SerializeField]
        MotionTrackerCameraDeviceQualityLevel desiredMinQualityLevel = MotionTrackerCameraDeviceQualityLevel.Bad;
        [HideInInspector, SerializeField]
        private bool hasDesiredTrackingMode;
        [HideInInspector, SerializeField]
        MotionTrackerCameraDeviceTrackingMode desiredTrackingMode = MotionTrackerCameraDeviceTrackingMode.Anchor;

        private bool willOpen;
        private Optional<bool> delayedOpen;
        private bool started;
        private ARSession session;
        private MotionTrackerCameraDeviceTrackingMode trackingMode;
        private MotionTrackerCameraDeviceQualityLevel minQualityLevel;

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
        public bool Opened => device != null;

        /// <summary>
        /// <para xml:lang="en">Get the quality of Motion Tracking on the device. You can decide whether to start Motion Tracking.</para>
        /// <para xml:lang="zh">获取设备上Motion Tracking的质量，结合应用场景，可以通过此值判断是否启动Motion Tracking。</para>
        /// </summary>
        public static MotionTrackerCameraDeviceQualityLevel DeviceQualityLevel => MotionTrackerCameraDevice.getQualityLevel();

        /// <summary>
        /// <para xml:lang="en">Frame rate range. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">帧率范围。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public Vector2 FrameRateRange => new((float)device.frameRateRangeLower(), (float)device.frameRateRangeUpper());

        /// <summary>
        /// <para xml:lang="en">Current preview size. Only usable when both <see cref="Opened"/> and <see cref="MonoBehaviour"/>.enabled is true.</para>
        /// <para xml:lang="zh">当前图像大小。仅在<see cref="Opened"/>和<see cref="MonoBehaviour"/>.enabled都为true时可用。</para>
        /// </summary>
        public Vector2Int Size => device.size().ToUnityVector();

        /// <summary>
        /// <para xml:lang="en">the vector of point cloud coordinate. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">当前点云数据位置信息。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public List<Vector3> LocalPointCloud => device.getLocalPointsCloud().Select(p => new Vector3(p.data_0, p.data_1, -p.data_2)).ToList();

        /// <summary>
        /// <para xml:lang="en">Desired focus mode. Only effective if modified before the <see cref="DeviceOpened"/> event or OnEnable.</para>
        /// <para xml:lang="en">Note: focus switch may not work on some devices due to hardware or system limitation.</para>
        /// <para xml:lang="zh">期望的对焦模式，在<see cref="DeviceOpened"/>事件或OnEnable前修改才有效。</para>
        /// <para xml:lang="zh">注意：受硬件或系统限制，对焦开关在一些设备上可能无效。</para>
        /// </summary>
        public Optional<MotionTrackerCameraDeviceFocusMode> DesiredFocusMode
        {
            get => hasDesiredFocusMode ? desiredFocusMode : Optional<MotionTrackerCameraDeviceFocusMode>.Empty;
            set
            {
                hasDesiredFocusMode = value.OnSome;
                if (value.OnSome) { desiredFocusMode = value.Value; }
            }
        }
        /// <summary>
        /// <para xml:lang="en">Desired frame resolution. Only effective if modified before the <see cref="DeviceOpened"/> event or OnEnable.</para>
        /// <para xml:lang="en">If the image size is modified during the <see cref="FrameRecorder"/> recording process, the recording data will stop updating. You will need to stop and restart the recording.</para>
        /// <para xml:lang="zh">期望的分辨率，在<see cref="DeviceOpened"/>事件或OnEnable前修改才有效。</para>
        /// <para xml:lang="zh">如果在<see cref="FrameRecorder"/>录制过程中修改了图像大小，录制数据将停止更新，需要关闭之后重新录制。</para>
        /// </summary>
        public Optional<MotionTrackerCameraDeviceResolution> DesiredResolution
        {
            get => hasDesiredResolution ? desiredResolution : Optional<MotionTrackerCameraDeviceResolution>.Empty;
            set
            {
                hasDesiredResolution = value.OnSome;
                if (value.OnSome) { desiredResolution = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Desired device frame rate. Only effective if modified before the <see cref="DeviceOpened"/> event or OnEnable.</para>
        /// <para xml:lang="zh">期望的设备帧率，在<see cref="DeviceOpened"/>事件或OnEnable前修改才有效。</para>
        /// </summary>
        public Optional<MotionTrackerCameraDeviceFPS> DesiredFrameRate
        {
            get => hasDesiredFrameRate ? desiredFrameRate : Optional<MotionTrackerCameraDeviceFPS>.Empty;
            set
            {
                hasDesiredFrameRate = value.OnSome;
                if (value.OnSome) { desiredFrameRate = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Desired minimum allowed quality level on the device. Only effective if modified before <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="zh">期望的最低允许的质量级别，在<see cref="ARSession.Assemble"/>前修改才有效。</para>
        /// </summary>
        public Optional<MotionTrackerCameraDeviceQualityLevel> DesiredMinQualityLevel
        {
            get => hasDesiredMinQualityLevel ? desiredMinQualityLevel : Optional<MotionTrackerCameraDeviceQualityLevel>.Empty;
            set
            {
                hasDesiredMinQualityLevel = value.OnSome;
                if (value.OnSome) { desiredMinQualityLevel = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Desired tracking mode. Only effective if modified before the session starts.</para>
        /// <para xml:lang="zh">期望的跟踪模式，在session启动前修改才有效。</para>
        /// </summary>
        public Optional<MotionTrackerCameraDeviceTrackingMode> DesiredTrackingMode
        {
            get => hasDesiredTrackingMode ? desiredTrackingMode : Optional<MotionTrackerCameraDeviceTrackingMode>.Empty;
            set
            {
                hasDesiredTrackingMode = value.OnSome;
                if (value.OnSome) { desiredTrackingMode = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en"><see cref="Camera"/> candidate. Only effective if Unity XR Origin is not used. Camera.main will be used if not specified.</para>
        /// <para xml:lang="zh"><see cref="Camera"/>的备选，仅当未使用Unity XR Origin时有效，如未设置会使用Camera.main。</para>
        /// </summary>
        public Camera CameraCandidate { get => cameraCandidate; set => cameraCandidate = value; }

        internal override bool IsManuallyDisabled => DeviceQualityLevel < minQualityLevel && DeviceQualityLevel != MotionTrackerCameraDeviceQualityLevel.NotSupported;
        GameObject IMotionTrackingDevice.Origin => XROriginCache.DefaultOrigin(true);
        internal protected override bool IsHMD => false;
        internal protected override Camera Camera => XROriginCache.DefaultCamera(true) ? XROriginCache.DefaultCamera(false) : (cameraCandidate ? cameraCandidate : Camera.main);
        internal protected override bool IsCameraUnderControl => true;
        internal protected override IDisplay Display => easyar.Display.DefaultSystemDisplay;
        internal protected override Optional<bool> IsAvailable => MotionTrackerCameraDevice.isAvailable();
        internal protected override bool CameraFrameStarted => started;
        internal protected override List<FrameSourceCamera> DeviceCameras => Opened && CameraFrameStarted ? new List<FrameSourceCamera> { new(device.type(), device.cameraOrientation(), Size, FrameRateRange) } : new();

        internal override int BufferCapacity
        {
            get => device?.bufferCapacity() ?? base.BufferCapacity;
            set
            {
                base.BufferCapacity = value;
                device?.setBufferCapacity(value);
            }
        }

        /// <summary>
        /// <para xml:lang="en">Start/Stop video stream capture when <see cref="ARSession"/> is running. Capture will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止采集视频流数据。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始采集。</para>
        /// </summary>
        private void OnEnable()
        {
            if (delayedOpen == true) { Open(); }
            if (device != null)
            {
                if (DesiredFocusMode.OnSome)
                {
                    device.setFocusMode(DesiredFocusMode.Value);
                }
                if (DesiredResolution.OnSome)
                {
                    device.setFrameResolutionType(DesiredResolution.Value);
                }
                if (DesiredFrameRate.OnSome)
                {
                    device.setFrameRateType(DesiredFrameRate.Value);
                }
                device.setTrackingMode(trackingMode);
                started = device.start();
            }
        }

        private void OnDisable()
        {
            started = false;
            device?.stop();
        }

        private void OnDestroy()
        {
            OnSessionStop();
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

                device = new MotionTrackerCameraDevice();

                if (base.BufferCapacity != 0)
                {
                    device.setBufferCapacity(base.BufferCapacity);
                }

                using (var source = device.inputFrameSource())
                {
                    ((ISenseBuiltinFrameSource)this).ConnectFrom(source);
                }

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
            if (device != null)
            {
                OnDisable();
                device.close();
                device.Dispose();
                device = null;
                DeviceClosed?.Invoke();
            }
        }

        /// <summary>
        /// <para xml:lang="en">Performs ray cast from the user's device in the direction of given screen point. Intersections with horizontal plane is detected in real time in the current field of view,and return the 3D point nearest to ray on horizontal plane. <paramref name="pointInView"/> should be normalized to [0, 1]^2.</para>
        /// <para xml:lang="zh">在当前视野内实时检测到的水平面上进行Hit Test,点击到某个水平面后返回该平面上距离Hit Test射线最近的3D点的位置坐标。<paramref name="pointInView"/> 需要被归一化到[0, 1]^2。</para>
        /// </summary>
        public List<Vector3> HitTestAgainstHorizontalPlane(Vector2 pointInView)
        {
            var points = new List<Vector3>();
            if (device == null || !session)
            {
                return points;
            }

            var coord = session.ImageCoordinatesFromScreenCoordinates(pointInView);
            if (coord.OnNone)
            {
                return points;
            }
            var hitPoints = device.hitTestAgainstHorizontalPlane(coord.Value.ToEasyARVector());

            foreach (var p in hitPoints)
            {
                points.Add(new Vector3(p.data_0, p.data_1, -p.data_2));
            }

            return points;
        }

        /// <summary>
        /// <para xml:lang="en">Perform hit test against the point cloud and return the nearest 3D point. <paramref name="pointInView"/> should be normalized to [0, 1]^2.</para>
        /// <para xml:lang="zh">在当前点云中进行Hit Test,得到距离相机从近到远一条射线上的最近的一个3D点位置坐标。<paramref name="pointInView"/> 需要被归一化到[0, 1]^2。</para>
        /// </summary>
        public List<Vector3> HitTestAgainstPointCloud(Vector2 pointInView)
        {
            var points = new List<Vector3>();
            if (device == null || !session)
            {
                return points;
            }

            var coord = session.ImageCoordinatesFromScreenCoordinates(pointInView);
            if (coord.OnNone)
            {
                return points;
            }
            var hitPoints = device.hitTestAgainstPointCloud(coord.Value.ToEasyARVector());

            foreach (var p in hitPoints)
            {
                points.Add(new Vector3(p.data_0, p.data_1, -p.data_2));
            }

            return points;
        }

        internal protected override void OnSessionStart(ARSession session)
        {
            this.session = session;
            if (delayedOpen.OnNone) { delayedOpen = true; }

            DetermineTrackingMode();

            if (enabled) { OnEnable(); }
        }

        internal protected override void OnSessionStop()
        {
            Close();
            delayedOpen = Optional<bool>.Empty;
            session = null;
        }

        internal override string DumpLite()
        {
            var data = string.Empty;
            data += $"Source: {ARSessionFactory.DefaultName(GetType())} ({enabled}), {(Opened ? Size : "-")}, {(Opened ? FrameRateRange : "-")}, {DeviceQualityLevel}" + Environment.NewLine;
            return data;
        }

        internal override void Connect(InputFrameSink val)
        {
            base.Connect(val);
            if (device != null)
            {
                using (var source = device.inputFrameSource())
                {
                    source.connect(val);
                }
            }
        }

        internal override void Disconnect()
        {
            base.Disconnect();
            if (device != null)
            {
                using (var source = device.inputFrameSource())
                {
                    source.disconnect();
                }
            }
        }

        internal void DetermineMinQualityLevel(bool hasMega)
        {
            minQualityLevel = hasMega ? MotionTrackerCameraDeviceQualityLevel.Limited : MotionTrackerCameraDeviceQualityLevel.NotSupported;

            if (DesiredMinQualityLevel.OnSome)
            {
                if ((hasMega && DesiredMinQualityLevel.Value > MotionTrackerCameraDeviceQualityLevel.Limited) || !hasMega)
                {
                    minQualityLevel = DesiredMinQualityLevel.Value;
                }
            }
        }

        private void DetermineTrackingMode()
        {
            if (DesiredTrackingMode.OnSome)
            {
                trackingMode = DesiredTrackingMode.Value;
                if (session.Assembly.FrameFilters.Where(f => f is MegaTrackerFrameFilter || f is CloudLocalizerFrameFilter).Any() && trackingMode != MotionTrackerCameraDeviceTrackingMode.LargeScale)
                {
                    trackingMode = MotionTrackerCameraDeviceTrackingMode.LargeScale;
                    Debug.LogWarning($"TrackingMode has been forced to {trackingMode} when using {nameof(MegaTrackerFrameFilter)} or {nameof(CloudLocalizerFrameFilter)}");
                }
            }
            else
            {
                if (session.Assembly.FrameFilters.Where(f => f is MegaTrackerFrameFilter || f is CloudLocalizerFrameFilter).Any())
                {
                    trackingMode = MotionTrackerCameraDeviceTrackingMode.LargeScale;
                }
                else if (session.Assembly.FrameFilters.Where(f => f is SparseSpatialMapWorkerFrameFilter || f is DenseSpatialMapBuilderFrameFilter).Any())
                {
                    trackingMode = MotionTrackerCameraDeviceTrackingMode.SLAM;
                }
                else
                {
                    trackingMode = MotionTrackerCameraDeviceTrackingMode.Anchor;
                }
            }
        }
    }
}
