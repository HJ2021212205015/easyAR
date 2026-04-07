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
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls XREAL camera device (<see cref="XREALCameraDevice"/>) in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.</para>
    /// <para xml:lang="zh">在场景中控制XREAL相机设备（<see cref="XREALCameraDevice"/>）的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// <para xml:lang="zh">在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。</para>
    /// </summary>
    public class XREALFrameSource : FrameSource, FrameSource.ISenseBuiltinFrameSource, FrameSource.IMotionTrackingDevice, FrameSource.ISyncMotionSource
    {
        private static IReadOnlyList<ARSession.ARCenterMode> centerModes = new List<ARSession.ARCenterMode> { ARSession.ARCenterMode.SessionOrigin, ARSession.ARCenterMode.FirstTarget, ARSession.ARCenterMode.SpecificTarget };
        private XREALCameraDevice device;
        [SerializeField, HideInInspector]
        private Camera cameraCandidate;

        private Optional<bool> delayedOpen;
        private bool started;
        private Optional<bool> isAvailable;
        private bool isSessionStarting;
        private Action<MotionInputData> motionUpdate;
        private FrameSourceInspector inspector;

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

        event Action<MotionInputData> ISyncMotionSource.MotionUpdate
        {
            add => motionUpdate += value;
            remove => motionUpdate -= value;
        }

        /// <summary>
        /// <para xml:lang="en">Whether camera is opened.</para>
        /// <para xml:lang="zh">相机是否打开。</para>
        /// </summary>
        public bool Opened => device != null;

        /// <summary>
        /// <para xml:lang="en">Frame rate range. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">帧率范围。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public double FrameRate => device.frameRate();

        /// <summary>
        /// <para xml:lang="en">Current preview size. Only usable when <see cref="Opened"/> is true.</para>
        /// <para xml:lang="zh">当前图像大小。仅在<see cref="Opened"/>为true时可用。</para>
        /// </summary>
        public Vector2Int Size => device.size().ToUnityVector();

        /// <summary>
        /// <para xml:lang="en">Received frame count. Usually used for debug. There are usually hardware issues if this value stop to increase, and re-plug may help.</para>
        /// <para xml:lang="zh">获取到的帧计数。通常在debug中使用。如果这个数值停止增长，通常是硬件问题，重新插拔可能能解决。</para>
        /// </summary>
        public int ReceivedFrameCount => device?.receivedFrameCount() ?? 0;

        internal override bool IsManuallyDisabled =>
#if EASYAR_ENABLE_XREAL
            false;
#else
            true;
#endif
        GameObject IMotionTrackingDevice.Origin => XROriginCache.XROrigin(true);
        internal protected override bool IsHMD => true;
        internal protected override Camera Camera => XROriginCache.XRCamera(true);
        internal protected override bool IsCameraUnderControl => false;
        internal protected override IDisplay Display => easyar.Display.DefaultHMDDisplay;
        internal protected override Optional<bool> IsAvailable => isAvailable;
        internal protected override IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode => centerModes;
        internal protected override bool CameraFrameStarted => started;
        internal protected override List<FrameSourceCamera> DeviceCameras => Opened ? new List<FrameSourceCamera> { new(device.type(), device.cameraOrientation(), Size, new Vector2((float)FrameRate, (float)FrameRate)) } : new();

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
            inspector = new FrameSourceInspector(this);
        }

        /// <summary>
        /// <para xml:lang="en">Start/Stop video stream capture when <see cref="ARSession"/> is running. Capture will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止采集视频流数据。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始采集。</para>
        /// </summary>
        private void OnEnable()
        {
            if (delayedOpen == true)
            {
                Open();
                return;
            }
            if (device != null)
            {
                inspector.ReceivedFrameCount = ReceivedFrameCount;
                started = device.start();
                inspector.Start();
            }
        }

        private void Update()
        {
            inspector.ReceivedFrameCount = ReceivedFrameCount;
            UpdateMotion();
        }

        private void OnDisable()
        {
            started = false;
            inspector.Stop();
            device?.stop();
        }

        private void OnApplicationPause(bool pause)
        {
            inspector.Reset();
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
            Close();

            device = new XREALCameraDevice();
            var openResult = device.open();
            if (!openResult)
            {
                device.Dispose();
                device = null;
                var error = $"XREAL camera open failed";
                if (!isSessionStarting)
                {
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, error);
                }
                DeviceOpened?.Invoke(false, PermissionStatus.Granted, error);
                throw new Exception(error);
            }
            if (base.BufferCapacity != 0)
            {
                device.setBufferCapacity(base.BufferCapacity);
            }
            using (var source = device.inputFrameSource())
            {
                ((ISenseBuiltinFrameSource)this).ConnectFrom(source);
            }

            DeviceOpened?.Invoke(true, PermissionStatus.Granted, string.Empty);

            if (enabled)
            {
                OnEnable();
            }
        }

        /// <summary>
        /// <para xml:lang="en">Close device.</para>
        /// <para xml:lang="zh">关闭设备。</para>
        /// </summary>
        public void Close()
        {
            delayedOpen = false;
            if (device != null)
            {
                OnDisable();
                device.close();
                device.Dispose();
                device = null;
                DeviceClosed?.Invoke();
            }
        }

        internal protected override IEnumerator CheckAvailability()
        {
            if (!XREALCameraDevice.isAvailable())
            {
                isAvailable = false;
                yield break;
            }
            yield return new WaitUntil(() => XREALCameraDevice.isDeviceSupported().OnSome);
            isAvailable = XREALCameraDevice.isDeviceSupported();
        }

        internal protected override void OnSessionStart(ARSession session)
        {
            isSessionStarting = true;
            if (delayedOpen.OnNone) { delayedOpen = true; }
            if (enabled) { OnEnable(); }
            isSessionStarting = false;
        }

        internal protected override void OnSessionStop()
        {
            Close();
            delayedOpen = Optional<bool>.Empty;
        }

        internal override string DumpLite()
        {
            var data = base.DumpLite();
            if (data.EndsWith(Environment.NewLine))
            {
                data = data.Substring(0, data.LastIndexOf(Environment.NewLine));
            }
            return data + " " + inspector.Dump();
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

        private void UpdateMotion()
        {
            if (device == null) { return; }

            var motionData = device.getMotionInputData();
            if (motionData.OnNone) { return; }
            using (var frame = motionData.Value)
            {
                motionUpdate?.Invoke(frame);
            }
        }
    }
}
