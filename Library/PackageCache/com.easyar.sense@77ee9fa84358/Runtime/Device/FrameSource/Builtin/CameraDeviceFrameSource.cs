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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="CameraDevice"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">This frame source is not a motion tracking device, and will not output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="CameraDevice"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">这个frame source不是运动跟踪设备，在<see cref="ARSession"/>中不会输出运动数据。</para>
    /// </summary>
    [RequireComponent(typeof(CameraDeviceDisplay))]
    public class CameraDeviceFrameSource : FrameSource, FrameSource.ISenseBuiltinFrameSource
    {
        /// <summary>
        /// <para xml:lang="en">Camera open method. Only effective if modified before the <see cref="DeviceOpened"/> event.</para>
        /// <para xml:lang="zh">打开相机时使用的方法，在<see cref="DeviceOpened"/>事件前修改才有效。</para>
        /// </summary>
        [HideInInspector, SerializeField]
        public CameraDeviceOpenMethod CameraOpenMethod = CameraDeviceOpenMethod.PreferredType;

        /// <summary>
        /// <para xml:lang="en">Camera open type used when <see cref="CameraOpenMethod"/> == <see cref="CameraDeviceOpenMethod.PreferredType"/> or <see cref="CameraDeviceOpenMethod.SpecificType"/>. Only effective if modified before the <see cref="DeviceOpened"/> event.</para>
        /// <para xml:lang="zh">打开相机时使用的Camera类型，<see cref="CameraOpenMethod"/> == <see cref="CameraDeviceOpenMethod.PreferredType"/>或<see cref="CameraDeviceOpenMethod.SpecificType"/>时使用，在<see cref="DeviceOpened"/>事件前修改才有效。</para>
        /// </summary>
        [HideInInspector, SerializeField]
        public CameraDeviceType CameraOpenType = CameraDeviceType.Back;

        /// <summary>
        /// <para xml:lang="en">Camera open index used when <see cref="CameraOpenMethod"/> == <see cref="CameraDeviceOpenMethod.DeviceIndex"/>. Only effective if modified before the <see cref="DeviceOpened"/> event.</para>
        /// <para xml:lang="zh">打开相机时使用的设备索引，<see cref="CameraOpenMethod"/> == <see cref="CameraDeviceOpenMethod.DeviceIndex"/>时使用，在<see cref="DeviceOpened"/>事件前修改才有效。</para>
        /// </summary>
        [HideInInspector, SerializeField]
        public int CameraOpenIndex;

        private CameraDevice device;
        private static IReadOnlyList<ARSession.ARCenterMode> availableCenterMode = new List<ARSession.ARCenterMode> { ARSession.ARCenterMode.FirstTarget, ARSession.ARCenterMode.Camera, ARSession.ARCenterMode.SpecificTarget };
        [HideInInspector, SerializeField]
        private bool hasDesiredAndroidCameraApiType;
        [HideInInspector, SerializeField]
        AndroidCameraApiType desiredAndroidCameraApiType;
        [HideInInspector, SerializeField]
        private bool hasDesiredFocusMode;
        [HideInInspector, SerializeField]
        CameraDeviceFocusMode desiredFocusMode = CameraDeviceFocusMode.Continousauto;
        [HideInInspector, SerializeField]
        private bool hasDesiredSize;
        [HideInInspector, SerializeField]
        Vector2Int desiredSize = new(1280, 960);
        [HideInInspector, SerializeField]
        private bool hasDesiredCameraPreference;
        [HideInInspector, SerializeField]
        CameraDevicePreference desiredCameraPreference;
        [SerializeField, HideInInspector]
        private Camera cameraCandidate;

        private bool willOpen;
        private Optional<bool> delayedOpen;
        private bool started;
        private ARSession session;
        bool isLargeScale;

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
        /// <para xml:lang="en">Event when device unavailable like disconnected or preempted. It is only available on Windows.</para>
        /// <para xml:lang="zh">设备断开或抢占等无法使用事件（仅Windows）。</para>
        /// </summary>
        public event Action<CameraState> DeviceStateChanged;

        /// <summary>
        /// <para xml:lang="en">Open method of <see cref="CameraDevice"/>.</para>
        /// <para xml:lang="zh"><see cref="CameraDevice"/>开启方式。</para>
        /// </summary>
        public enum CameraDeviceOpenMethod
        {
            /// <summary>
            /// <para xml:lang="en">Opens a camera by camera device type. If no camera is matched, the first camera will be used.</para>
            /// <para xml:lang="zh">按照camera设备类型打开camera设备，如果没有匹配的类型则会尝试打开第一个camera设备。</para>
            /// </summary>
            PreferredType,
            /// <summary>
            /// <para xml:lang="en">Opens a camera by index.</para>
            /// <para xml:lang="zh">按照camera索引打开camera设备。</para>
            /// </summary>
            DeviceIndex,
            /// <summary>
            /// <para xml:lang="en">Opens a camera by specific camera device type. If no camera is matched, false will be returned. On Mac, camera device types can not be distinguished.</para>
            /// <para xml:lang="zh">按照精确的camera设备类型打开camera设备，如果没有匹配的类型则会返回false。在Mac上，camera类型无法判别。</para>
            /// </summary>
            SpecificType,
        }

        /// <summary>
        /// <para xml:lang="en">Gets count of cameras recognized by the operating system.</para>
        /// <para xml:lang="zh">获得操作系统识别到的camera数量。</para>
        /// </summary>
        public static int CameraCount => CameraDevice.cameraCount();

        /// <summary>
        /// <para xml:lang="en">Whether camera is opened.</para>
        /// <para xml:lang="zh">相机是否打开。</para>
        /// </summary>
        public bool Opened => device != null;

        /// <summary>
        /// <para xml:lang="en">Camera type. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">camera类型。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public CameraDeviceType CameraType => device.type();

        /// <summary>
        /// <para xml:lang="en">Camera index. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">camera索引。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public int Index => device.index();

        /// <summary>
        /// <para xml:lang="en">Gets current camera API (camera1 or camera2) on Android. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">在Android上，可用于获得使用的Camera API（camera1或camera2）。。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public AndroidCameraApiType AndroidCameraApiType => device.androidCameraApiType();

        /// <summary>
        /// <para xml:lang="en">Supported preview size of current device. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">当前设备支持的所有图像大小。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public List<Vector2Int> SupportedSize
        {
            get
            {
                List<Vector2Int> list = new();
                for (var i = 0; i < device.supportedSizeCount(); ++i)
                {
                    list.Add(device.supportedSize(i).ToUnityVector());
                }
                return list;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Supported frame rate ranges of current device. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">当前设备支持的所有帧率范围。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public List<Vector2> SupportedFrameRateRange
        {
            get
            {
                List<Vector2> list = new();
                for (var i = 0; i < device.supportedFrameRateRangeCount(); ++i)
                {
                    list.Add(new Vector2(device.supportedFrameRateRangeLower(i), device.supportedFrameRateRangeUpper(i)));
                }
                return list;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Frame rate range. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">帧率范围。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public Vector2 FrameRateRange => FrameRateRangeIndex == -1 ? new Vector2(device.supportedFrameRateRangeLower(-1), device.supportedFrameRateRangeUpper(-1)) : SupportedFrameRateRange[FrameRateRangeIndex];

        /// <summary>
        /// <para xml:lang="en">Current preview size. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="en">The nearest value from <see cref="SupportedSize"/> will be used when set.</para>
        /// <para xml:lang="en">If the image size is modified during the <see cref="FrameRecorder"/> recording process, the recording data will stop updating. You will need to stop and restart the recording.</para>
        /// <para xml:lang="zh">当前图像大小。仅在<see cref="Opened"/>为true时可用。</para>
        /// <para xml:lang="zh">set会使用<see cref="SupportedSize"/>中数值最接近的大小。</para>
        /// <para xml:lang="zh">如果在<see cref="FrameRecorder"/>录制过程中修改了图像大小，录制数据将停止更新，需要关闭之后重新录制。</para>
        /// </summary>
        public Vector2Int Size
        {
            get => device.size().ToUnityVector();
            set => device.setSize(value.ToEasyARVector());
        }

        /// <summary>
        /// <para xml:lang="en">Current index of frame rate range. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">设备的当前帧率范围的索引。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public int FrameRateRangeIndex
        {
            get => device.frameRateRange();
            set => device.setFrameRateRange(value);
        }

        /// <summary>
        /// <para xml:lang="en">Camera parameters. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">camera参数。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public CameraParameters Parameters
        {
            get => device?.cameraParameters();
            set => device?.setCameraParameters(value);
        }

        /// <summary>
        /// <para xml:lang="en">Focus mode. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">对焦模式。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public CameraDeviceFocusMode FocusMode
        {
            set => device.setFocusMode(value);
        }

        /// <summary>
        /// <para xml:lang="en">Desired focus mode. Only effective if modified before the <see cref="DeviceOpened"/> event.</para>
        /// <para xml:lang="en">Note: focus switch may not work on some devices due to hardware or system limitation. Default value will be chosen according to <see cref="DesiredCameraPreference"/> will be used if not set.</para>
        /// <para xml:lang="zh">期望的对焦模式，在<see cref="DeviceOpened"/>事件前修改才有效。</para>
        /// <para xml:lang="zh">注意：受硬件或系统限制，对焦开关在一些设备上可能无效。未设置将根据<see cref="DesiredCameraPreference"/>进行选择。</para>
        /// </summary>
        public Optional<CameraDeviceFocusMode> DesiredFocusMode
        {
            get => hasDesiredFocusMode ? desiredFocusMode : Optional<CameraDeviceFocusMode>.Empty;
            set
            {
                hasDesiredFocusMode = value.OnSome;
                if (value.OnSome) { desiredFocusMode = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Desired Camera Api on Android. Only effective if modified before the <see cref="DeviceOpened"/> event.</para>
        /// <para xml:lang="en">Default value will be chosen according to <see cref="DesiredCameraPreference"/> will be used if not set.</para>
        /// <para xml:lang="zh">期望的Android Camera Api，在<see cref="DeviceOpened"/>事件前修改才有效。</para>
        /// <para xml:lang="zh">未设置将根据<see cref="DesiredCameraPreference"/>进行选择。</para>
        /// </summary>
        public Optional<AndroidCameraApiType> DesiredAndroidCameraApiType
        {
            get => hasDesiredAndroidCameraApiType ? desiredAndroidCameraApiType : Optional<AndroidCameraApiType>.Empty;
            set
            {
                hasDesiredAndroidCameraApiType = value.OnSome;
                if (value.OnSome) { desiredAndroidCameraApiType = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Desired camera preview size. Only effective if modified before the <see cref="DeviceOpened"/> event.</para>
        /// <para xml:lang="en">The nearest value from <see cref="SupportedSize"/> will be used. Default value will be used if not set.</para>
        /// <para xml:lang="zh">期望的相机图像大小，在<see cref="DeviceOpened"/>事件前修改才有效。</para>
        /// <para xml:lang="zh">会使用<see cref="SupportedSize"/>中数值最接近的大小。未设置将使用默认值。</para>
        /// </summary>
        public Optional<Vector2Int> DesiredSize
        {
            get => hasDesiredSize ? desiredSize : Optional<Vector2Int>.Empty;
            set
            {
                hasDesiredSize = value.OnSome;
                if (value.OnSome) { desiredSize = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Camera preference used to create camera device. Only effective if modified before the session starts.</para>
        /// <para xml:lang="zh">创建相机设备时使用的偏好设置，在session启动前修改才有效。</para>
        /// </summary>
        public Optional<CameraDevicePreference> DesiredCameraPreference
        {
            get => hasDesiredCameraPreference ? desiredCameraPreference : Optional<CameraDevicePreference>.Empty;
            set
            {
                hasDesiredCameraPreference = value.OnSome;
                if (value.OnSome) { desiredCameraPreference = value.Value; }
            }
        }

        /// <summary>
        /// <para xml:lang="en"><see cref="Camera"/> candidate. Camera.main will be used if not specified.</para>
        /// <para xml:lang="zh"><see cref="Camera"/>的备选，如未设置会使用Camera.main。</para>
        /// </summary>
        public Camera CameraCandidate { get => cameraCandidate; set => cameraCandidate = value; }

        internal override bool IsManuallyDisabled => false;
        internal protected override bool IsHMD => false;
        internal protected override Camera Camera => cameraCandidate ? cameraCandidate : Camera.main;
        internal protected override IDisplay Display => GetComponent<CameraDeviceDisplay>();
        internal protected override bool IsCameraUnderControl => true;
        internal protected override Optional<bool> IsAvailable => CameraDevice.isAvailable();
        internal protected override IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode => availableCenterMode;
        internal protected override bool CameraFrameStarted => started;
        internal protected override List<FrameSourceCamera> DeviceCameras => Opened ? new List<FrameSourceCamera> { new(CameraType, device.cameraOrientation(), Size, FrameRateRange) } : new();
        internal override int BufferCapacity
        {
            get => device?.bufferCapacity() ?? base.BufferCapacity;
            set
            {
                base.BufferCapacity = value;
                device?.setBufferCapacity(value);
            }
        }

        private void Awake()
        {
            // for backward compatibility
            if (!GetComponent<CameraDeviceDisplay>()) { gameObject.AddComponent<CameraDeviceDisplay>(); }
        }

        /// <summary>
        /// <para xml:lang="en">Start/Stop video stream capture when <see cref="ARSession"/> is running. Capture will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止采集视频流数据。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始采集。</para>
        /// </summary>
        private void OnEnable()
        {
            if (delayedOpen == true) { Open(); }
            started = device?.start() ?? false;
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
        /// <para xml:lang="en">Can only be used after session start.</para>
        /// <para xml:lang="zh">打开设备。如果未手动调用<see cref="Open"/>和<see cref="Close"/>，<see cref="ARSession"/>启动后会自动<see cref="Open"/>。</para>
        /// <para xml:lang="zh">在session启动后才能使用。</para>
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
                if (session == null)
                {
                    DeviceOpened?.Invoke(false, status, "session not started");
                    return;
                }
                if (status != PermissionStatus.Granted)
                {
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, $"Camera permission {status}, {msg}");
                    DeviceOpened?.Invoke(false, status, msg);
                    return;
                }

                Close();
                var cameraPreference = DetermineCameraPreference();
                device = CameraDeviceSelector.createCameraDevice(cameraPreference);
                device.setStateChangedCallback(EasyARController.Scheduler, (Action<CameraState>)((state) =>
                {
                    var error = state == CameraState.Disconnected ? $"Camera disconnected." : $"Camera unavailable or currently in use by another application.";
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, error);
                    DeviceStateChanged?.Invoke(state);
                }));

                if (DesiredAndroidCameraApiType.OnSome)
                {
                    device.setAndroidCameraApiType(DesiredAndroidCameraApiType.Value);
                }
                bool openResult = false;
                var openMethod = string.Empty;
                switch (CameraOpenMethod)
                {
                    case CameraDeviceOpenMethod.PreferredType:
                        openMethod = nameof(CameraDevice.openWithPreferredType);
                        openResult = device.openWithPreferredType(CameraOpenType);
                        break;
                    case CameraDeviceOpenMethod.DeviceIndex:
                        openMethod = nameof(CameraDevice.openWithIndex);
                        openResult = device.openWithIndex(CameraOpenIndex);
                        break;
                    case CameraDeviceOpenMethod.SpecificType:
                        openMethod = nameof(CameraDevice.openWithSpecificType);
                        openResult = device.openWithSpecificType(CameraOpenType);
                        break;
                    default:
                        break;
                }
                if (!openResult)
                {
                    device.Dispose();
                    device = null;
                    var error = $"{nameof(CameraDevice)}.{openMethod} fail";
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, error);
                    DeviceOpened?.Invoke(false, status, error);
                    return;
                }

                device.setFocusMode(DesiredFocusMode.OnSome ? DesiredFocusMode.Value : (isLargeScale ? CameraDeviceFocusMode.Continousauto : CameraDeviceSelector.getFocusMode(cameraPreference)));
                if (DesiredSize.OnSome)
                {
                    device.setSize(DesiredSize.Value.ToEasyARVector());
                }
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
        /// <para xml:lang="en">调用一次自动对焦。仅在FocusMode为Normal或Macro时才能使用。</para>
        /// <para xml:lang="zh">Does auto focus once. It is only available when FocusMode is Normal or Macro.</para>
        /// </summary>
        public bool AutoFocus() => device?.autoFocus() ?? false;

        /// <summary>
        /// <para xml:lang="en">Sets flash torch mode. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">设置flash torch模式。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public bool SetFlashTorch(bool on) => device?.setFlashTorchMode(on) ?? false;


        internal protected override void OnSessionStart(ARSession session)
        {
            this.session = session;
            if (delayedOpen.OnNone) { delayedOpen = true; }
            isLargeScale = session.Assembly.FrameFilters.Where(f => f is MegaTrackerFrameFilter || f is CloudLocalizerFrameFilter).Any();
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
            data += $"Source: {ARSessionFactory.DefaultName(GetType())} ({enabled}), {(Opened ? Index : "-")}/{CameraCount}, {(Opened ? Size : "-")}, {(Opened ? FrameRateRange : "-")}" + Environment.NewLine;
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

        private CameraDevicePreference DetermineCameraPreference()
        {
            CameraDevicePreference preference;
            if (DesiredCameraPreference.OnSome)
            {
                preference = DesiredCameraPreference.Value;
            }
            else
            {
                if (session.Assembly.FrameFilters.Where(f => f is MegaTrackerFrameFilter || f is CloudLocalizerFrameFilter).Any())
                {
                    preference = CameraDevicePreference.PreferMotionTracking;
                }
                else if (session.Assembly.FrameFilters.Where(f => f is SurfaceTrackerFrameFilter).Any())
                {
                    preference = CameraDevicePreference.PreferSurfaceTracking;
                }
                else
                {
                    preference = CameraDevicePreference.PreferObjectSensing;
                }
            }
            return preference;
        }
    }
}
