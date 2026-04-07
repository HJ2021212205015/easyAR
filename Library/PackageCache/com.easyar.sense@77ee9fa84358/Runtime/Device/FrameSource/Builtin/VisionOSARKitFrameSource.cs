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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls VisionOS ARKit camera device (<see cref="VisionOSARKitCameraDevice"/>) in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">This frame source is one type of motion tracking device, and will output motion data in a <see cref="ARSession"/>.</para>
    /// <para xml:lang="en">EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.</para>
    /// <para xml:lang="zh">在场景中控制VisionOS ARKit相机设备（<see cref="VisionOSARKitCameraDevice"/>）的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">这个frame source是一种运动跟踪设备，在<see cref="ARSession"/>中会输出运动数据。</para>
    /// <para xml:lang="zh">在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。</para>
    /// </summary>
    public class VisionOSARKitFrameSource : FrameSource, FrameSource.ISenseBuiltinFrameSource, FrameSource.IMotionTrackingDevice, FrameSource.ISyncMotionSource
    {
        private static IReadOnlyList<ARSession.ARCenterMode> centerModes = new List<ARSession.ARCenterMode> { ARSession.ARCenterMode.SessionOrigin, ARSession.ARCenterMode.FirstTarget, ARSession.ARCenterMode.SpecificTarget };
        private VisionOSARKitCameraDevice device;
        [SerializeField, HideInInspector]
        private Camera cameraCandidate;

        private Optional<bool> delayedOpen;
        private bool started;
        private bool canStart;
        private Action<MotionInputData> motionUpdate;

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
        public bool Opened => device != null && canStart;

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

        internal override bool IsManuallyDisabled => false;
        GameObject IMotionTrackingDevice.Origin => XROriginCache.XROrigin(true);
        internal protected override bool IsHMD => true;
        internal protected override Camera Camera => XROriginCache.XRCamera(true);
        internal protected override bool IsCameraUnderControl => false;
        internal protected override IDisplay Display => easyar.Display.DefaultHMDDisplay;
        internal protected override Optional<bool> IsAvailable => VisionOSARKitCameraDevice.isAvailable();
        internal protected override IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode => centerModes;
        internal protected override bool CameraFrameStarted => started;
        internal protected override List<FrameSourceCamera> DeviceCameras => Opened ? new List<FrameSourceCamera> { new(device.type(), device.cameraOrientation(), Size, FrameRateRange) } : new();

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
            if (device != null && canStart)
            {
                started = device.start();
            }
        }

        private void Update()
        {
            UpdateMotion();
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
            Close();

            device = new VisionOSARKitCameraDevice();
            if (base.BufferCapacity != 0)
            {
                device.setBufferCapacity(base.BufferCapacity);
            }
            using (var source = device.inputFrameSource())
            {
                ((ISenseBuiltinFrameSource)this).ConnectFrom(source);
            }

            device.requestPermissions(EasyARController.Scheduler, (Action<PermissionStatus, string>)((status, msg) =>
            {
                if (device == null)
                {
                    DeviceOpened?.Invoke(false, status, "closed");
                    return;
                }
                if (status != PermissionStatus.Granted)
                {
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, $"Camera permission {status}, {msg}");
                    Close();
                    DeviceOpened?.Invoke(false, status, msg);
                    return;
                }
                canStart = true;

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
            if (device != null)
            {
                OnDisable();
                device.close();
                device.Dispose();
                device = null;
                if (canStart)
                {
                    DeviceClosed?.Invoke();
                }
            }
            canStart = false;
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
