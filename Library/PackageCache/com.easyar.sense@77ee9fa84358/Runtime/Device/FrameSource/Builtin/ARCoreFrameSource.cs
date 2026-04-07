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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls ARCore camera device (<see cref="ARCoreCameraDevice"/>) in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="zh">在场景中控制ARCore相机设备（<see cref="ARCoreCameraDevice"/>）的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// </summary>
    public class ARCoreFrameSource : FrameSource, FrameSource.ISenseBuiltinFrameSource, FrameSource.IMotionTrackingDevice
    {
        /// <summary>
        /// <para xml:lang="en">Desired focus mode. Only effective if modified before the <see cref="DeviceOpened"/> event or OnEnable.</para>
        /// <para xml:lang="en">Note: focus switch may not work on some devices due to hardware or system limitation.</para>
        /// <para xml:lang="zh">期望的对焦模式，在<see cref="DeviceOpened"/>事件或OnEnable前修改才有效。</para>
        /// <para xml:lang="zh">注意：受硬件或系统限制，对焦开关在一些设备上可能无效。</para>
        /// </summary>
        public ARCoreCameraDeviceFocusMode DesiredFocusMode;

        private ARCoreCameraDevice device;
        [SerializeField, HideInInspector]
        private Camera cameraCandidate;

        private bool willOpen;
        private Optional<bool> delayedOpen;
        private bool loadLibrary;
        private bool started;

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
        /// <para xml:lang="en">Frame rate range. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">帧率范围。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public Vector2 FrameRateRange => new((float)device.frameRateRangeLower(), (float)device.frameRateRangeUpper());

        /// <summary>
        /// <para xml:lang="en">Current preview size. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">当前图像大小。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public Vector2Int Size => device.size().ToUnityVector();

        /// <summary>
        /// <para xml:lang="en"><see cref="Camera"/> candidate. Only effective if Unity XR Origin is not used. Camera.main will be used if not specified.</para>
        /// <para xml:lang="zh"><see cref="Camera"/>的备选，仅当未使用Unity XR Origin时有效，如未设置会使用Camera.main。</para>
        /// </summary>
        public Camera CameraCandidate { get => cameraCandidate; set => cameraCandidate = value; }

        internal override bool IsManuallyDisabled => false;
        GameObject IMotionTrackingDevice.Origin => XROriginCache.DefaultOrigin(true);
        internal protected override bool IsHMD => false;
        internal protected override Camera Camera => XROriginCache.DefaultCamera(true) ? XROriginCache.DefaultCamera(false) : (cameraCandidate ? cameraCandidate : Camera.main);
        internal protected override bool IsCameraUnderControl => true;
        internal protected override IDisplay Display => easyar.Display.DefaultSystemDisplay;
        internal protected override bool CameraFrameStarted => started;
        internal protected override List<FrameSourceCamera> DeviceCameras => Opened ? new List<FrameSourceCamera> { new(device.type(), device.cameraOrientation(), Size, FrameRateRange) } : new();

        internal protected override Optional<bool> IsAvailable
        {
            get
            {
                if (!loadLibrary)
                {
                    LoadLibrary();
                    loadLibrary = true;
                }
                return ARCoreCameraDevice.isAvailable();
            }
        }

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
            device?.setFocusMode(DesiredFocusMode);
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

                device = new ARCoreCameraDevice();

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

        internal static void LoadLibrary()
        {
            if (Application.platform != RuntimePlatform.Android) { return; }
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using var arCoreCameraDeviceClass = new AndroidJavaClass("cn.easyar.ARCoreCameraDevice");
                arCoreCameraDeviceClass.CallStatic("loadLibraries");
            }
            catch (Exception e)
            {
                Debug.LogError($"Fail to load ARCore library: {e}");
            }
#endif
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
    }
}
