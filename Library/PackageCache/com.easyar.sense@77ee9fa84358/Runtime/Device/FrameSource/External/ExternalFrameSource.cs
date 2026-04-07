//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">A external frame source represents a frame source not built-in in the EasyAR Sense. It is used for AR Engine, AR Foundation, HMD support, and customize your own frame source.</para>
    /// <para xml:lang="en">You can inherent a few sub types of <see cref="ExternalFrameSource"/> to implenment custom camera, but you cannot inherent <see cref="ExternalFrameSource"/> directly. Custom camera represnets a new device or data input method usaully.</para>
    /// <para xml:lang="en">EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.</para>
    /// <para xml:lang="zh">一个外部frame source代表非EasyAR Sense内置的frame source。它用于支持AR Engine，AR Foundation或头显，也可以用于定义你自己的frame source。</para>
    /// <para xml:lang="zh">你可以通过继承<see cref="ExternalFrameSource"/>的一些子类型来实现自定义相机，但你不能直接继承<see cref="ExternalFrameSource"/>。自定义相机通常表达一个新的设备或新的数据输入方式。</para>
    /// <para xml:lang="zh">在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。</para>
    /// </summary>
    public abstract class ExternalFrameSource : FrameSource, FrameSource.ISenseExternalFrameSource
    {
        private readonly BufferPoolWrapper bufferPool = new();
        private FrameSourceInspector inspector;
        private Coroutine inspectorControlCoroutine;

        /// <summary>
        /// <para xml:lang="en">Received frame count. Usually used for debug. There are usually hardware issues if this value stop to increase, and re-plug may help when using some devices like Nreal.</para>
        /// <para xml:lang="zh">获取到的帧计数。通常在debug中使用。如果这个数值停止增长，通常是硬件问题，某些设备（比如Nreal）重新插拔可能能解决。</para>
        /// </summary>
        public int ReceivedFrameCount => inspector?.ReceivedFrameCount ?? 0;

        internal override bool IsManuallyDisabled => false;

        internal override int BufferCapacity
        {
            get => base.BufferCapacity;
            set
            {
                base.BufferCapacity = value;
                bufferPool.BufferCapacity = value;
            }
        }
        private protected abstract CameraTransformType CameraTransformType { get; }

        protected virtual void Awake()
        {
            inspector = new FrameSourceInspector(this);
        }

        protected virtual void OnApplicationPause(bool pause)
        {
            inspector?.Reset();
        }

        protected virtual void OnDestroy()
        {
            bufferPool?.Dispose();
            OnSessionStop();
        }

        internal protected override void OnSessionStart(ARSession session)
        {
            inspectorControlCoroutine = StartCoroutine(InspectorControl());
        }

        internal protected override void OnSessionStop()
        {
            if (inspectorControlCoroutine != null)
            {
                StopCoroutine(inspectorControlCoroutine);
                inspectorControlCoroutine = null;
            }
            inspector?.Stop();
        }

        internal override string DumpLite()
        {
            var data = base.DumpLite();
            if (data.EndsWith(Environment.NewLine))
            {
                data = data.Substring(0, data.LastIndexOf(Environment.NewLine));
            }
            return data + " " + inspector?.Dump();
        }

        // NOTICE: EasyAR Sense API may change in the near future
        private protected bool HandleCameraFrameData(InputFrame frame, Optional<CameraTransformType> type)
        {
            if (type.OnNone && (CameraTransformType != frame.cameraTransformType() || (this is IMotionTrackingDevice) != (frame.hasSpatialInformation())))
            {
                DiagnosticsController.TryShowDiagnosticsError(gameObject, "camera frame data does not match frame source type");
                return false;
            }
            if (((ISenseExternalFrameSource)this).HandleCameraFrameData(frame))
            {
                inspector.ReceivedFrameCount++;
            }
            return true;
        }

        /// <summary>
        /// <para xml:lang="en">Try acquire buffer from buffer pool.</para>
        /// <para xml:lang="zh">尝试从内存池中获取内存块。</para>
        /// </summary>
        protected Optional<Buffer> TryAcquireBuffer(int size) => bufferPool.TryAcquireBuffer(size);

        private IEnumerator InspectorControl()
        {
            while (true)
            {
                yield return new WaitUntil(() => CameraFrameStarted);
                inspector?.Start();
                yield return new WaitUntil(() => !CameraFrameStarted);
                inspector?.Stop();
            }
        }
    }
}
