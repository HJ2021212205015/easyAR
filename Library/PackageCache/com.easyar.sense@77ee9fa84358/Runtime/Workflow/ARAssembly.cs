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
    ///<remarks>
    /// ```
    ///                                  +-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .-- .--+
    ///                                  |                                                                                                                  .
    ///                                  .                                 +---------------------------------------------------------------+                |
    ///                                  |                                 |                                                               |                .
    ///                                  .                                 |                       + -> ObjectTracker - - - - +            |                |
    ///                                  |                                 v                       '                          '            |                .
    ///                                  .                        +--> i2FAdapter --> fbFrameFork - - > ImageTracker - - - +  '            |                |
    ///                                  |                        |                                                        '  '            |                .
    ///                                  v                        |                                                        v  v            |                |
    ///  FrameSource         .--> iFrameThrottler --> iFrameFork --> i2OAdapter ------------------------------------> oFrameJoin --> oFrameFork --> oFrameBuffer ~~> o
    ///      |               |                                    '                                                     ^  ^  ^
    ///      v               |                                    '                                                     '  '  '
    ///  InputFrameRecorder  |                                    + - - - - - - - - - - - - - - - - - > Mega*Tracker- - +  '  '
    ///      |               |                                    '                                                        '  '
    ///      v               |                                    + - - - - - - - - - - - - - - - - - > SparseSpatialMap - +  '
    ///  (VideoInputFrameRecorder)                                '                                                           '
    ///                                                           + - - - - - - - - - - - - - - - - - > SurfaceTracker - - - -+
    ///                                                           '
    ///                                                           + - - - - - - - - - - - - - - - - - > DenseSpatialMap ~ ~ > o
    ///                                                           '
    ///                                                           + - - - - - - - - - - - - - - - - - > CloudRecognizer ~ ~ > o
    /// ```
    ///</remarks>

    /// <summary>
    /// <para xml:lang="en">Assembly of AR components. It implements one typical assemble strategy for all EasyAR Sense components.</para>
    /// <para xml:lang="zh">AR组件的组装体。它实现了一种对所有EasyAR Sense组件的典型组装。</para>
    /// </summary>
    public class ARAssembly : IDisposable
    {
        private InputFrameThrottler iFrameThrottler;
        private InputFrameFork iFrameFork;
        private InputFrameToOutputFrameAdapter i2OAdapter;
        private InputFrameToFeedbackFrameAdapter i2FAdapter;
        private FeedbackFrameFork fbFrameFork;
        private OutputFrameJoin oFrameJoin;
        private OutputFrameFork oFrameFork;
        private InputFrameSink input;
        private int extraBufferCapacity;
        private int oFrameBufferBufferRequirement;

        internal ARAssembly(AssembleWorkbench workbench)
        {
            FrameSource = workbench.PickedFrameSource;
            FrameRecorder = workbench.PickedFrameRecorder;
            FrameFilters = workbench.PickedFrameFilters;
            CameraController = workbench.PickedCameraController;
            CameraImageRenderer = workbench.PickedCameraImageRenderer;
            AvailableCenterMode = workbench.PickedCenterMode;
            Camera = workbench.PickedCamera;
            Origin = workbench.PickedOrigin;
            DefaultOriginChild = workbench.PickedOriginChild;
            Display = workbench.PickedDisplay;
            IsASync = workbench.IsAsync.Value;
        }

        ~ARAssembly()
        {
            DisposeAll();
        }

        /// <summary>
        /// <para xml:lang="en">Frame source.</para>
        /// <para xml:lang="zh">Frame数据源。</para>
        /// </summary>
        public FrameSource FrameSource { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Frame recorder. The value depends on your configuration on the recorder.</para>
        /// <para xml:lang="zh">输入帧录制器。取值将取决于你在recorder上的配置。</para>
        /// </summary>
        public Optional<FrameRecorder> FrameRecorder { get; private set; }

        /// <summary>
        /// <para xml:lang="en"><see cref="FrameFilter"/> list.</para>
        /// <para xml:lang="zh"><see cref="FrameFilter"/>的列表。</para>
        /// </summary>
        public List<FrameFilter> FrameFilters { get; private set; } = new List<FrameFilter>();

        /// <summary>
        /// <para xml:lang="en">On desktop or mobile phones, it represents the <see cref="UnityEngine.Camera"/> in the virtual world in reflection of real world camera device, its projection matrix and transform will be set to reflect the real world camera, and it is controlled by EasyAR. On head mounted displays, it is only used to display some diagnostics message in front of your eyes, it is not used for camera image rendering, it is not controled by EasyAR.</para>
        /// <para xml:lang="zh">在桌面设备或手机上，该相机代表现实环境中相机设备在虚拟世界中对应的<see cref="UnityEngine.Camera"/>，其投影矩阵和位置都将与真实相机对应，受EasyAR控制。在头显上，该相机仅用于将一些诊断文字展示在眼前，不用于画面渲染，相机也不受EasyAR控制。</para>
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Camera image renderer. If has no value if EasyAR does not control image rendering, when using AR Foundation or HMD.</para>
        /// <para xml:lang="zh">相机图像渲染器。在使用AR Foundation或头显时，EasyAR不控制图像渲染，它将无值。</para>
        /// </summary>
        public Optional<CameraImageRenderer> CameraImageRenderer { get; private set; }
        /// <summary>
        /// <para xml:lang="en">Origin of session when one type of motion tracking is running.</para>
        /// <para xml:lang="zh">在任一运动跟踪功能运行时的session原点。</para>
        /// </summary>
        public Optional<GameObject> Origin { get; private set; }
        /// <summary>
        /// <para xml:lang="en">Available center mode. Vary when using different frame source.</para>
        /// <para xml:lang="zh">可用的中心模式。在使用不同frame source时会有不同。</para>
        /// </summary>
        public IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Extra device buffer capacity. When you hold a OutputFrame/InputFrame or image from InputFrame for more than one render frame, you should increase this value by one.</para>
        /// <para xml:lang="zh">额外需要的设备缓冲容量。如果需要保留OutputFrame/InputFrame或InputFrame中的image超过渲染的一帧，需要增加1。</para>
        /// </summary>
        public int ExtraBufferCapacity
        {
            get
            {
                return extraBufferCapacity;
            }
            set
            {
                extraBufferCapacity = value;
                ResetBufferCapacity();
            }
        }

        /// <summary>
        /// <para xml:lang="en">Display information used by the assembly.</para>
        /// <para xml:lang="zh">Assembly使用的显示设备信息。</para>
        /// </summary>
        public IDisplay Display { get; private set; }

        internal bool IsASync { get; private set; }

        internal Optional<RenderCameraController> CameraController { get; private set; }

        internal Optional<XROriginChildController> DefaultOriginChild { get; private set; }

        void IDisposable.Dispose()
        {
            DisposeAll();
            GC.SuppressFinalize(this);
        }

        internal void Break()
        {
            oFrameFork?.output(0).disconnect();
            if (FrameSource)
            {
                FrameSource.Disconnect();
            }
            input?.Dispose();
            input = null;
        }

        internal void ResetBufferCapacity()
        {
            if (FrameSource != null)
            {
                FrameSource.BufferCapacity = GetBufferRequirement();
            }
        }

        internal void OnFrameInput(bool hasSpatial)
        {
            if (FrameSource is FramePlayer framePlayer)
            {
                if (framePlayer.UpdateInputFrameSpatial(hasSpatial))
                {
                    AvailableCenterMode = framePlayer.AvailableCenterMode;
                }
            }
        }

        internal void Assemble(OutputFrameBuffer oFrameBuffer)
        {
            oFrameBufferBufferRequirement = oFrameBuffer.bufferRequirement();

            // throttler
            iFrameThrottler = InputFrameThrottler.create();

            // fork input
            iFrameFork = InputFrameFork.create(2 + FrameFilters.Where(f => f is FrameFilter.IInputFrameSink).Count());
            iFrameThrottler.output().connect(iFrameFork.input());
            var iFrameForkIndex = 0;
            i2OAdapter = InputFrameToOutputFrameAdapter.create();
            iFrameFork.output(iFrameForkIndex).connect(i2OAdapter.input());
            iFrameForkIndex++;
            i2FAdapter = InputFrameToFeedbackFrameAdapter.create();
            iFrameFork.output(iFrameForkIndex).connect(i2FAdapter.input());
            iFrameForkIndex++;
            foreach (var filter in FrameFilters)
            {
                if (filter is FrameFilter.IInputFrameSink)
                {
                    FrameFilter.IInputFrameSink unit = filter as FrameFilter.IInputFrameSink;
                    var sink = unit.InputFrameSink();
                    if (sink != null)
                    {
                        iFrameFork.output(iFrameForkIndex).connect(unit.InputFrameSink());
                    }
                    iFrameForkIndex++;
                }
            }

            // feedback
            fbFrameFork = FeedbackFrameFork.create(FrameFilters.Where(f => f is FrameFilter.IFeedbackFrameSink).Count());
            i2FAdapter.output().connect(fbFrameFork.input());
            var fbFrameForkIndex = 0;
            foreach (var filter in FrameFilters)
            {
                if (filter is FrameFilter.IFeedbackFrameSink)
                {
                    FrameFilter.IFeedbackFrameSink unit = filter as FrameFilter.IFeedbackFrameSink;
                    fbFrameFork.output(fbFrameForkIndex).connect(unit.FeedbackFrameSink());
                    fbFrameForkIndex++;
                }
            }

            // join
            oFrameJoin = OutputFrameJoin.create(1 + FrameFilters.Where(f => f is FrameFilter.IOutputFrameSource).Count());
            var joinIndex = 0;
            foreach (var filter in FrameFilters)
            {
                if (filter is FrameFilter.IOutputFrameSource)
                {
                    FrameFilter.IOutputFrameSource unit = filter as FrameFilter.IOutputFrameSource;
                    unit.OutputFrameSource().connect(oFrameJoin.input(joinIndex));
                    joinIndex++;
                }
            }
            i2OAdapter.output().connect(oFrameJoin.input(joinIndex));

            // fork output for feedback
            oFrameFork = OutputFrameFork.create(2);
            oFrameJoin.output().connect(oFrameFork.input());
            oFrameFork.output(0).connect(oFrameBuffer.input());
            oFrameFork.output(1).connect(i2FAdapter.sideInput());

            // signal throttler
            oFrameBuffer.signalOutput().connect(iFrameThrottler.signalInput());
            var inputFrameSink = iFrameThrottler.input();

            // connect recorder
            if (FrameRecorder.OnSome && FrameRecorder.Value)
            {
                FrameRecorder.Value.Output().connect(inputFrameSink);
                inputFrameSink = FrameRecorder.Value.Input();
            }

            // connect source
            if (FrameSource)
            {
                FrameSource.Connect(inputFrameSink);
            }

            // set BufferCapacity
            ResetBufferCapacity();

            input = inputFrameSink;
        }

        private int GetBufferRequirement()
        {
            int count = 1; // for OutputFrameBuffer.peek
            if (FrameSource) { count += 1; }
            if (FrameRecorder.OnSome && FrameRecorder.Value) { count += FrameRecorder.Value.BufferRequirement; }
            if (iFrameThrottler != null) { count += iFrameThrottler.bufferRequirement(); }
            if (i2FAdapter != null) { count += i2FAdapter.bufferRequirement(); }
            count += oFrameBufferBufferRequirement;
            foreach (var filter in FrameFilters)
            {
                if (filter != null) { count += filter.BufferRequirement; }
            }
            count += extraBufferCapacity;
            return count;
        }

        private void DisposeAll()
        {
            iFrameThrottler?.Dispose();
            iFrameFork?.Dispose();
            i2OAdapter?.Dispose();
            i2FAdapter?.Dispose();
            fbFrameFork?.Dispose();
            oFrameJoin?.Dispose();
            oFrameFork?.Dispose();
        }
    }
}
