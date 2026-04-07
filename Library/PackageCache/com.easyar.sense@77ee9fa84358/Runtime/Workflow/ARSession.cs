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
using System.Linq;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls AR session in the scene. One session contains a set of components assembled as <see cref="ARAssembly"/> and controls data flow in the whole life cycle. This component is the entrance of AR. Multiple active instances are not allowed in the same time.</para>
    /// <para xml:lang="en">All EasyAR components can only work after <see cref="StartSession"/>.</para>
    /// <para xml:lang="en">Relative transform between <see cref="Camera"/> and a few AR components are controlled by the session, one of those objects is called <see cref="CenterObject"/>, it stays still in the scene, while other objects move relatively to <see cref="CenterObject"/>. This object is selected according to the value of <see cref="CenterMode"/>. See description of <see cref="ARCenterMode"/> for more details.</para>
    /// <para xml:lang="zh">在场景中控制AR会话的<see cref="MonoBehaviour"/>。一个会话包含一组组装成<see cref="ARAssembly"/>的组件，并控制整个生命周期的数据流。这个组件是AR的入口。同一时刻不允许出现多个active的实例。</para>
    /// <para xml:lang="zh">EasyAR组件的所有功能必须在<see cref="StartSession"/>之后才能使用。</para>
    /// <para xml:lang="zh"><see cref="Camera"/>和一部分AR组件之间的相对transform是受session控制的，其中的一个物体被称为<see cref="CenterObject"/>，它在场景中不动，其它物体相对这个<see cref="CenterObject"/>运动。这个物体是根据<see cref="CenterMode"/>的数值进行选择的。更详细的说明可以查看<see cref="ARCenterMode"/>的描述。</para>
    /// </summary>
    [DefaultExecutionOrder(int.MinValue)] // NOTE: Run first so that AR Foundation components (except ARSession) can be disabled automatically during app startup.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EasyARController), typeof(DiagnosticsController))]
    [RequireComponent(typeof(FrameRecorder), typeof(FramePlayer), typeof(CameraImageRenderer))]
    public class ARSession : MonoBehaviour
    {
        /// <summary>
        /// <para xml:lang="en">Start session automatically during <see cref="MonoBehaviour"/>.Start. You need to manually call <see cref="StartSession"/> to start the session if set to false.</para>
        /// <para xml:lang="zh"><see cref="MonoBehaviour"/>.Start时自动启动session。如果设置为false，你需要手动调用<see cref="StartSession"/>来启动session。</para>
        /// </summary>
        [SerializeField, HideInInspector]
        public bool AutoStart = true;
        /// <summary>
        /// <para xml:lang="en">AR center mode. Modify at any time and takes effect immediately. If the specified mode is not available in a session, it will be change to one of the available mode automatically.</para>
        /// <para xml:lang="zh">AR中心模式。可随时修改，立即生效。如果指定的模式不可用，它将会被自动修改为可用的模式。</para>
        /// </summary>
        [SerializeField, HideInInspector]
        public ARCenterMode CenterMode;
        /// <summary>
        /// <para xml:lang="en">Horizontal flip rendering modes. Modify at any time and takes effect immediately. Only available when using image or object tracking.</para>
        /// <para xml:lang="zh">水平镜像渲染模式。可随时修改，立即生效。仅在使用图像或物体跟踪时可用。</para>
        /// </summary>
        [SerializeField, HideInInspector]
        public FlipOptions HorizontalFlip = new FlipOptions();

        private OutputFrameBuffer oFrameBuffer;

        // delegate or delegated
        [SerializeField, HideInInspector]
        private GameObject specificTargetCenter;
        [SerializeField, HideInInspector]
        private AssembleOptions assembleOptions = new AssembleOptions();
        private SessionState state;
        private DiagnosticsController diagnostics;
        private UnityXRSwitcher xrSwitcher;

        // session status
        private bool hasData;
        private int frameIndex = -1;
        private MotionInputData frameMotion;
        private ARHorizontalFlipMode frameFlip;
        private CameraParameters frameCameraParameters;

        // session cache
        private Optional<Tuple<string, float>> assmbleDumpMessage;
        private List<MonoBehaviour> startedComponets = new List<MonoBehaviour>();
        private bool hasFramePlayerError;
        private float preAspect;
        private CameraImageRenderer lastCameraImageRenderer;
        private Optional<int> lastFrameIndex;
        private int initializeCount;
        private int errorIndex;

        /// <summary>
        /// <para xml:lang="en">Session <see cref="State"/> change event.</para>
        /// <para xml:lang="zh">session <see cref="State"/>改变的事件。</para>
        /// </summary>
        public event Action<SessionState> StateChanged;

        /// <summary>
        /// <para xml:lang="en">Session <see cref="Assemble"/> update event. This event will be triggered 1-2 times during one <see cref="Assemble"/> calling. <see cref="StartSession"/> will call assemble if it was not called before. The first time of the event happens when assemble finish. It will be the only one if device list finished download when trigger, otherwise another event will be triggered when the download finish.</para>
        /// <para xml:lang="zh">session <see cref="Assemble"/>更新事件。在一次<see cref="Assemble"/>（如果之前没有调用，<see cref="StartSession"/>会隐式调用）中这个事件会被触发1-2次。其中第一次是Assemble完成时。如果Assemble完成时设备列表更新已经完成将不会有第二次调用，否则第二次调用将在设备列表更新完成时发生。</para>
        /// </summary>
        public event Action<SessionReport.AvailabilityReport> AssembleUpdate;

        /// <summary>
        /// <para xml:lang="en">Input frame update event. It is triggered only when the <see cref="InputFrame"/> used by the session was changed. You will not receive this event on HMD and pass-through image rendering is done by device if it exists.</para>
        /// <para xml:lang="zh">输入帧更新事件，仅在session显示的<see cref="InputFrame"/>产生变化时触发。头显上不会收到此事件，且如果存在透视图像，其渲染由设备完成。</para>
        /// </summary>
        public event Action<InputFrame> InputFrameUpdate;

        /// <summary>
        /// <para xml:lang="en">Post session update event. It has the same frequency as <see cref="MonoBehaviour"/>.Update (The extension writter should ensure the correct implementation on HMD). All transform changes controlled by EasyAR would be ended when this event triggers.</para>
        /// <para xml:lang="zh">Session更新完成事件，该事件频率和<see cref="MonoBehaviour"/>.Update频率相同（在头显上需要扩展作者正确实现）。在该事件触发时，该帧内所有受EasyAR控制的transform变化已经完成。</para>
        /// </summary>
        public event Action PostSessionUpdate;

        internal event Action SessionUpdate;

        /// <summary>
        /// <para xml:lang="en">AR center mode.</para>
        /// <para xml:lang="en">*NOTE: In EasyAR Sense Unity Plugin, there are four different types of center modes. Similar concept may not exist in some other AR frameworks like AR Foundation, and the behavior of object relationships is usually equal to* <see cref="SessionOrigin"/> *mode here.*</para>
        /// <para xml:lang="en">Relative transform between <see cref="ARAssembly.Camera"/> and a few AR components are controlled by the session, one of those objects is called <see cref="CenterObject"/>, it stays still in the scene, while other objects move relatively to <see cref="CenterObject"/>. This object is selected according to the value of <see cref="CenterMode"/>.</para>
        /// <para xml:lang="en"><see cref="CenterObject"/> represents an object or parent of object that do not move in Unity space. It can be <see cref="Origin"/>, <see cref="ARAssembly.Camera"/> or some `target`. A `target` could be object containing component <see cref="TargetController"/> or <see cref="EasyAR.Mega.Scene.BlockRootController"/>. While in the context of sparse spatial map or EasyAR Mega, the exact center <see cref="GameObject"/> is the localized map or block object under the root, and <see cref="CenterObject"/> is parent of this object.</para>
        /// <para xml:lang="en"><see cref="CenterObject"/> may change to other `target` in <see cref="FirstTarget"/> or <see cref="SpecificTarget"/> mode when `target` is not found or lost in a frame. If no `target` is found in this frame, the <see cref="CenterObject"/> will fallback to the center of first available mode in the order of <see cref="SessionOrigin"/> and <see cref="Camera"/>.</para>
        /// <para xml:lang="en">The relative transform between `target` and <see cref="ARAssembly.Camera"/> is controlled by this session every frame. The relative transform between <see cref="Origin"/> and <see cref="ARAssembly.Camera"/> is also controlled by this session every frame when <see cref="FrameSource.IsCameraUnderControl"/> is true. When <see cref="FrameSource.IsCameraUnderControl"/> is false, the relative transform between <see cref="Origin"/> and <see cref="ARAssembly.Camera"/> is not controlled by this session and is usually controlled by other AR Frameworks like AR Foundation.</para>
        /// <para xml:lang="zh">AR中心模式。</para>
        /// <para xml:lang="zh">*注意：在EasyAR Sense Unity Plugin中总共有四种中心模式。在其它AR框架比如AR Foundation中可能并不存在类似的概念，通常它们里面的物体间相对关系的行为与这里的*<see cref="SessionOrigin"/>*模式一致。*</para>
        /// <para xml:lang="zh"><see cref="ARAssembly.Camera"/>和一部分AR组件之间的相对transform是受session控制的，其中的一个物体被称为<see cref="CenterObject"/>，它在场景中不动，其它物体相对这个<see cref="CenterObject"/>运动。这个物体是根据<see cref="CenterMode"/>的数值进行选择的。</para>
        /// <para xml:lang="zh"><see cref="CenterObject"/> 表示在Unity空间中不运动的物体或这个物体的父节点。它可能是 <see cref="Origin"/>，<see cref="ARAssembly.Camera"/> 或某个 `target` 。 `Target` 可以是包含<see cref="TargetController"/>或<see cref="EasyAR.Mega.Scene.BlockRootController"/>组件的物体。在使用稀疏空间地图和EasyAR Mega的时候，实际的中心<see cref="GameObject"/>是root节点下具体定位到的map或block物体，<see cref="CenterObject"/> 是这个物体的父节点。</para>
        /// <para xml:lang="zh">在<see cref="FirstTarget"/> 或 <see cref="SpecificTarget"/>模式下，当 `target` 在某一帧中未被识别到或丢失的时候，<see cref="CenterObject"/> 可能会变成其它 `target` ，而如果在帧内找不到 `target` ，<see cref="CenterObject"/>会按先后顺序退变为<see cref="SessionOrigin"/>和<see cref="Camera"/>里面第一个可用的模式的中心。</para>
        /// <para xml:lang="zh">`Target` 和<see cref="ARAssembly.Camera"/>的相对位置关系由当前session控制。<see cref="Origin"/> 和<see cref="ARAssembly.Camera"/>的相对位置关系，在<see cref="FrameSource.IsCameraUnderControl"/> 为true的时候，也由当前session控制，而当<see cref="FrameSource.IsCameraUnderControl"/> 为false的时候，它是不受当前session控制的，通常由其它AR框架比如AR Foundation控制。</para>
        /// </summary>
        public enum ARCenterMode
        {
            /// <summary>
            /// <para xml:lang="en">The session will use the first tracked `target` as center.</para>
            /// <para xml:lang="en">You can move or rotate the `target` and <see cref="ARAssembly.Camera"/> will follow. You cannot manually change the transform of <see cref="ARAssembly.Camera"/> in this mode. <see cref="Origin"/> will also follow if any type of motion tracking is running, and its transform cannot be manually changed.</para>
            /// <para xml:lang="en">When the `target` is lost, the center object will be recalculated. While in the context of sparse spatial map or EasyAR Mega, the exact center <see cref="GameObject"/> is the localized map or block object under the root. Start localizing another map is treated as lost from localizing the previous one, and the center object will be recalculated.</para>
            /// <para xml:lang="zh">当前session是以第一个跟踪到的 `target` 为中心的。</para>
            /// <para xml:lang="zh">你可以移动或旋转 `target` ，<see cref="ARAssembly.Camera"/>会跟着动。在这个模式下你将无法手动控制<see cref="ARAssembly.Camera"/>的transform。如果任意一种运动跟踪在运行，<see cref="Origin"/>也会跟着动，它的transform也是不能手动控制的。</para>
            /// <para xml:lang="zh">当 `target` 丢失之后，中心物体会重新计算。在使用稀疏空间地图和EasyAR Mega的时候，实际的中心<see cref="GameObject"/>是root节点下具体定位到的map或block物体。并且，定位一张新的地图将会被认作从前一张地图定位过程中的丢失，中心物体会重新计算。</para>
            /// </summary>
            FirstTarget,

            /// <summary>
            /// <para xml:lang="en">The session is <see cref="ARAssembly.Camera"/> centered.</para>
            /// <para xml:lang="en">You can move or rotate the <see cref="ARAssembly.Camera"/> and `target` will follow. You cannot manually change the transform of `target`. <see cref="Origin"/> will also follow if any type of motion tracking is running, and its transform cannot be manually changed.</para>
            /// <para xml:lang="zh">当前session是以<see cref="ARAssembly.Camera"/>为中心的。</para>
            /// <para xml:lang="zh">你可以移动或旋转<see cref="ARAssembly.Camera"/>，`target` 会跟着动。在这个模式下你将无法手动控制 `target` 的transform。如果任意一种运动跟踪在运行，<see cref="Origin"/>也会跟着动，它的transform也是不能手动控制的。</para>
            /// </summary>
            Camera,

            /// <summary>
            /// <para xml:lang="en">The session will use <see cref="SpecificTargetCenter"/> as center.</para>
            /// <para xml:lang="en">You can move or rotate the `target` and <see cref="ARAssembly.Camera"/> will follow. You cannot manually change the transform of <see cref="ARAssembly.Camera"/> in this mode. <see cref="Origin"/> will also follow if any type of motion tracking is running, and its transform cannot be manually changed.</para>
            /// <para xml:lang="zh">当前session是以<see cref="SpecificTargetCenter"/>为中心的。</para>
            /// <para xml:lang="zh">你可以移动或旋转 `target` ，<see cref="ARAssembly.Camera"/>会跟着动。在这个模式下你将无法手动控制<see cref="ARAssembly.Camera"/>的transform。如果任意一种运动跟踪在运行，<see cref="Origin"/>也会跟着动，它的transform也是不能手动控制的。</para>
            /// </summary>
            SpecificTarget,

            /// <summary>
            /// <para xml:lang="en">The session will use <see cref="Origin"/> as center.</para>
            /// <para xml:lang="en">You can move or rotate the <see cref="Origin"/> and the <see cref="ARAssembly.Camera"/> will follow. You cannot manually change the <see cref="ARAssembly.Camera"/>'s transform in this mode. If there are any `target` being tracked, it will also follow, and its transform cannot be manually changed.</para>
            /// <para xml:lang="zh">当前session是以<see cref="Origin"/>为中心的。</para>
            /// <para xml:lang="zh">你可以移动或旋转<see cref="Origin"/>，<see cref="ARAssembly.Camera"/>会跟着动。在这个模式下你将无法手动控制<see cref="ARAssembly.Camera"/>的transform。如果有任何 `target` 正在被跟踪，它也会跟着动，并且它的transform也是不能手动控制的。</para>
            /// </summary>
            SessionOrigin,
        }

        /// <summary>
        /// <para xml:lang="en">Horizontal flip rendering mode.</para>
        /// <para xml:lang="en">In a flip rendering mode, the camera image will be mirrored. And to display to tracked objects in the right way, it will affect the 3D object rendering as well, so there are two different ways of doing horizontal flip. Horizontal flip can only work in object sensing like image or object tracking algorithms.</para>
        /// <para xml:lang="zh">水平镜像渲染模式。</para>
        /// <para xml:lang="zh">在水平翻转状态下，相机图像将镜像显示，为确保物体跟踪正常，它同时会影响3D物体的渲染，因此提供两种不同的方式。水平翻转只能在物体感知（比如图像跟踪或物体跟踪）算法下工作。</para>
        /// </summary>
        public enum ARHorizontalFlipMode
        {
            /// <summary>
            /// <para xml:lang="en">No flip.</para>
            /// <para xml:lang="zh">不翻转。</para>
            /// </summary>
            None,
            /// <summary>
            /// <para xml:lang="en">Render with horizontal flip, the camera image will be flipped in rendering, the camera projection matrix will be changed to do flip rendering. Target scale will not change.</para>
            /// <para xml:lang="zh">水平镜像渲染，camera图像会镜像显示，camera投影矩阵会变化进行镜像渲染，target scale不会改变。</para>
            /// </summary>
            World,
            /// <summary>
            /// <para xml:lang="en">Render with horizontal flip, the camera image will be flipped in rendering, the target scale will be changed to do flip rendering. Camera projection matrix will not change.</para>
            /// <para xml:lang="zh">水平镜像渲染，camera图像会镜像显示，target scale会改变进行镜像渲染，camera投影矩阵不会改变。</para>
            /// </summary>
            Target,
        }

        /// <summary>
        /// <para xml:lang="en">The state of session.</para>
        /// <para xml:lang="zh">Session的状态。</para>
        /// </summary>
        public enum SessionState
        {
            /// <summary>
            /// <para xml:lang="en">The initial state when session not started or assembled.</para>
            /// <para xml:lang="zh">初始状态，session未启动或组装。</para>
            /// </summary>
            None,
            /// <summary>
            /// <para xml:lang="en">Session broken caused by <see cref="ARAssembly"/> fail to assemble or other reasons.</para>
            /// <para xml:lang="zh"><see cref="ARAssembly"/>组装失败等原因session被破坏。</para>
            /// </summary>
            Broken,
            /// <summary>
            /// <para xml:lang="en">In the process of assembling.</para>
            /// <para xml:lang="zh">在组装过程中。</para>
            /// </summary>
            Assembling,
            /// <summary>
            /// <para xml:lang="en">Assemble finished successfully.</para>
            /// <para xml:lang="zh">成功完成组装。</para>
            /// </summary>
            Assembled,
            /// <summary>
            /// <para xml:lang="en">Session is ready to run after a success <see cref="StartSession"/>.</para>
            /// <para xml:lang="zh">Session在成功<see cref="StartSession"/>之后已经准备好运行。</para>
            /// </summary>
            Ready,
            /// <summary>
            /// <para xml:lang="en">Session is running.</para>
            /// <para xml:lang="zh">Session在运行中。</para>
            /// </summary>
            Running,
            /// <summary>
            /// <para xml:lang="en">Session is paused.</para>
            /// <para xml:lang="en">Session will be paused when <see cref="FrameSource"/> generate empty frames, usually when device stop or application pause.</para>
            /// <para xml:lang="zh">Session暂停运行。</para>
            /// <para xml:lang="zh">Session会在<see cref="FrameSource"/>生成空帧数据的时候暂停，通常会发生在设备停止或应用暂停的时候。</para>
            /// </summary>
            Paused,
        }

        /// <summary>
        /// <para xml:lang="en">Specified AR center object. <see cref="CenterObject"/> will be set to this object when <see cref="CenterMode"/> == <see cref="ARCenterMode.SpecificTarget"/>. Modify at any time and takes effect immediately.</para>
        /// <para xml:lang="en">The object must contain component <see cref="TargetController"/> or <see cref="EasyAR.Mega.Scene.BlockRootController"/>.</para>
        /// <para xml:lang="zh">手动指定的中心物体。<see cref="CenterMode"/> == <see cref="ARCenterMode.SpecificTarget"/>时<see cref="CenterObject"/>将被设成这个物体。可随时修改，立即生效。</para>
        /// <para xml:lang="zh">该物体必须包含<see cref="TargetController"/>或<see cref="EasyAR.Mega.Scene.BlockRootController"/>组件。</para>
        /// </summary>
        public GameObject SpecificTargetCenter
        {
            get => specificTargetCenter;
            set
            {
                if (value
                    && !value.GetComponent<TargetController>()
#if EASYAR_ENABLE_MEGA
                    && !value.GetComponent<EasyAR.Mega.Scene.BlockRootController>()
#endif
                    )
                {
                    Debug.LogWarning($"Ignore set SpecificTargetCenter: Cannot find target component from {value}");
                    return;
                }
                specificTargetCenter = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Center object this session is using in current frame.</para>
        /// <para xml:lang="en">This object represents an object or parent of object that do not move in Unity space. It can be <see cref="Origin"/>, <see cref="ARAssembly.Camera"/> or some `target`. A `target` could be object containing component <see cref="TargetController"/> or <see cref="EasyAR.Mega.Scene.BlockRootController"/>. While in the context of sparse spatial map or EasyAR Mega, the exact center <see cref="GameObject"/> is the localized map or block object under the root, and <see cref="CenterObject"/> is parent of this object. See description of <see cref="ARCenterMode"/> for more details.</para>
        /// <para xml:lang="zh">这个session在当前帧使用的中心物体。</para>
        /// <para xml:lang="zh">这个物体表示在Unity空间中不运动的物体或这个物体的父节点。它可能是 <see cref="Origin"/>，<see cref="ARAssembly.Camera"/> 或某个 `target` 。 `Target` 可以是包含<see cref="TargetController"/>或<see cref="EasyAR.Mega.Scene.BlockRootController"/>组件的物体。在使用稀疏空间地图和EasyAR Mega的时候，实际的中心<see cref="GameObject"/>是root节点下具体定位到的map或block物体，<see cref="CenterObject"/> 是这个物体的父节点。更详细的说明可以查看<see cref="ARCenterMode"/>的描述。</para>
        /// </summary>
        public GameObject CenterObject { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Assembly of AR components.</para>
        /// <para xml:lang="zh">AR组件的组装体。</para>
        /// </summary>
        public ARAssembly Assembly { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Session diagnostics component.</para>
        /// <para xml:lang="zh">Session诊断组件。</para>
        /// </summary>
        public DiagnosticsController Diagnostics => diagnostics ?? GetComponent<DiagnosticsController>();

        /// <summary>
        /// <para xml:lang="en">Available center mode in the session.</para>
        /// <para xml:lang="zh">当前session可用的中心模式。</para>
        /// </summary>
        public IReadOnlyList<ARCenterMode> AvailableCenterMode
        {
            get => (Assembly != null) ? Assembly.AvailableCenterMode : new List<ARCenterMode>();
        }

        /// <summary>
        /// <para xml:lang="en">Origin of session when one type of motion tracking is running.</para>
        /// <para xml:lang="en">It will be selected from the scene or created by EasyAR or 3rdparty frame source extension automatically. XROrigin from Unity XR framework will be selected if built-in frame source is used. Only XR Origin in default tree layout will be selected if AR Foundation package not exist.</para>
        /// <para xml:lang="zh">在任一运动跟踪功能运行时的session原点。</para>
        /// <para xml:lang="zh">它将会从场景中被自动选择，如果不存在则会由EasyAR或第三方frame source扩展创建。使用内置frame source时，如果Unity XR框架的XROrigin存在，它会被选择。如果AR Foundation的包不存在，保持默认树结构的XR Origin才会被选择。</para>
        /// </summary>
        public GameObject Origin
        {
            get => (Assembly != null && Assembly.Origin.OnSome) ? Assembly.Origin.Value : null;
        }

        /// <summary>
        /// <para xml:lang="en">Tracking status when one type of motion tracking is running.</para>
        /// <para xml:lang="zh">在任一运动跟踪功能运行时的运动跟踪状态。</para>
        /// </summary>
        public Optional<MotionTrackingStatus> TrackingStatus => frameMotion?.trackingStatus() ?? Optional<MotionTrackingStatus>.Empty;

        /// <summary>
        /// <para xml:lang="en">Session assemble options. Need to set before <see cref="Assemble"/> calling. <see cref="StartSession"/> will call assemble if it was not called before.</para>
        /// <para xml:lang="zh">Session 的组装选项，需要在<see cref="Assemble"/>（如果之前没有调用，<see cref="StartSession"/>会隐式调用）前设置。</para>
        /// </summary>
        public AssembleOptions AssembleOptions => assembleOptions;

        /// <summary>
        /// <para xml:lang="en">Session report. It can be accessed after assemble finish, and will be updated when session state change. It can be used to check session broken details or component availablility details.</para>
        /// <para xml:lang="zh">Session 报告。它在assemble完成后可用查看，且会在session状态改变时更新。它用于查看session损坏的详细信息或组件可用性等详细信息。</para>
        /// </summary>
        public SessionReport Report { get; private set; } = new SessionReport();

        /// <summary>
        /// <para xml:lang="en">The state of current session.</para>
        /// <para xml:lang="zh">当前session的状态。</para>
        /// </summary>
        public SessionState State
        {
            get => state;
            private set
            {
                if (state == value) { return; }

                state = value;
                if (state != SessionState.Broken) { Report.OnSessionRecover(); }
                StateChanged?.Invoke(state);
            }
        }

        internal static ARSession Current { get; private set; }

        internal Optional<CameraTransformType> CameraTransformType => frameMotion?.transformType() ?? Optional<CameraTransformType>.Empty;

        // not synced with image background nor head motion
        internal Optional<OutputFrame> AsyncCameraFrame => oFrameBuffer?.peek() ?? null;

        private void Awake()
        {
            // for backward compatibility
            if (!GetComponent<DiagnosticsController>()) { gameObject.AddComponent<DiagnosticsController>(); }
            if (!GetComponent<FrameRecorder>()) { gameObject.AddComponent<FrameRecorder>(); }
            if (!GetComponent<FramePlayer>()) { gameObject.AddComponent<FramePlayer>(); }
            if (!GetComponent<CameraImageRenderer>()) { gameObject.AddComponent<CameraImageRenderer>(); }

            diagnostics = GetComponent<DiagnosticsController>();
            if (EasyARSettings.Instance && (EasyARSettings.Instance.UnityXR?.UnityXRAutoSwitch?.Player?.Enable ?? true))
            {
                xrSwitcher = new UnityXRSwitcher();
            }
        }

        /// <summary>
        /// <para xml:lang="en">Enable/disable session output after when session is running. Camera image (if controlled by EasyAR) and all session object transform will not update when disabled.</para>
        /// <para xml:lang="zh">session运行时启用/禁用输出。在禁用状态下，session不会输出，相机图像（如果由EasyAR控制）和所有session物体的transform都不会更新。</para>
        /// </summary>
        private void OnEnable()
        {
            Current = this;
            oFrameBuffer?.resume();
        }

        private void Start()
        {
            if (AutoStart) { StartSession(); }
        }

        private void Update()
        {
            if (!CanSessionUpdate()) { return; }
            EasyARController.RunScheduler();
            if (!Assembly.IsASync) { return; }

            UpdateSessionAsync();
        }

        private void OnDisable()
        {
            if (Current == this) { Current = null; }
            if (oFrameBuffer != null)
            {
                oFrameBuffer?.pause();
                lastFrameIndex = Optional<int>.Empty;
                ClearSession();
            }
            StopLastCameraImageRenderer(null);

            if (State >= SessionState.Assembling && !gameObject.activeInHierarchy)
            {
                StopSession(false);
            }
        }

        private void OnDestroy()
        {
            StopSession(false);
            oFrameBuffer?.Dispose();
            oFrameBuffer = null;
        }

        /// <summary>
        /// <para xml:lang="en">Start session. It will be called automatically from <see cref="MonoBehaviour"/>.Start if <see cref="AutoStart"/> is true.</para>
        /// <para xml:lang="zh">启动session。如果<see cref="AutoStart"/>是true，它会在<see cref="MonoBehaviour"/>.Start中自动调用。</para>
        /// </summary>
        public void StartSession()
        {
            Diagnostics.RecoverFromFatalError();
            EasyARController.RunScheduler();
            var error = EasyARController.LicenseValidation.Error;
            if (error.OnSome)
            {
                errorIndex = EasyARController.LicenseValidation.ErrorIndex;
                var message = $"Unable to start {this} before EasyAR Sense is ready" + (EasyARController.LicenseValidation.IsLicenseInvalid ? $" (license invalid)" : $" (uninitialized)") + Environment.NewLine + error;
                Diagnostics.EnqueueSenseError(message);
                throw new Exception(message);
            }

            if (!gameObject.activeInHierarchy)
            {
                throw new Exception($"Unable to start {this} when the game object disabled");
            }
            if (State >= SessionState.Ready)
            {
                Debug.LogWarning($"{this} in state {State} already started");
                return;
            }
            Diagnostics.ClearCaution();
            if (State == SessionState.Assembling)
            {
                StopAllCoroutines();
            }
            if (State == SessionState.Assembled)
            {
                StartAssembledSession();
            }
            else
            {
                StartSession(false);
            }
        }

        /// <summary>
        /// <para xml:lang="en">Assemble session using <see cref="AssembleOptions"/>. It will be called automatically from <see cref="StartSession"/> not called before.</para>
        /// <para xml:lang="zh">使用<see cref="AssembleOptions"/>组装session。如果调用过，它会在<see cref="StartSession"/>中自动调用。</para>
        /// </summary>
        public IEnumerator Assemble() => Assemble(null, null);

        /// <summary>
        /// <para xml:lang="en">Stop session. All transform update and image rendering update will stop. You can choose to keep the last image frame when stop, but it only works when EasyAR controls image rendering (not using AR Foundation and HMD, etc.).</para>
        /// <para xml:lang="zh">停止启动session。所有transform更新和图像渲染更新将会停止。你可用旋转在停止时保留最后一帧图像，但只能在EasyAR控制图像渲染时有效（AR Foundation和头显等无效）。</para>
        /// </summary>
        public void StopSession(bool keepLastFrame)
        {
            StopLastCameraImageRenderer(null);
            StopAllCoroutines();
            if (State >= SessionState.Ready)
            {
                StopAssembledSession(keepLastFrame);
            }
            if (Assembly != null)
            {
                (Assembly as IDisposable).Dispose();
                Assembly = null;
            }
            if (!keepLastFrame && enabled)
            {
                oFrameBuffer?.pause();
                oFrameBuffer?.resume();
                lastFrameIndex = Optional<int>.Empty;
            }
            frameCameraParameters?.Dispose();
            frameCameraParameters = null;
            frameMotion?.Dispose();
            frameMotion = null;
            frameIndex = -1;
            frameFlip = ARHorizontalFlipMode.None;
            State = SessionState.None;
        }

        /// <summary>
        /// <para xml:lang="en">Transforms points from screen coordinate system ([0, 1]^2) to image coordinate system ([0, 1]^2). <paramref name="pointInView"/> should be normalized to [0, 1]^2. Not available on HMD</para>
        /// <para xml:lang="zh">从屏幕坐标系（[0, 1]^2）变换到图像坐标系（[0, 1]^2）。<paramref name="pointInView"/> 需要被归一化到[0, 1]^2。头显上不可用。</para>
        /// </summary>
        public Optional<Vector2> ImageCoordinatesFromScreenCoordinates(Vector2 pointInView)
        {
            if (frameCameraParameters == null || Assembly == null || !Assembly.Camera)
            {
                return Optional<Vector2>.Empty;
            }
            return frameCameraParameters.imageCoordinatesFromScreenCoordinates(Assembly.Camera.aspect, Assembly.Display.Rotation, true, false, new Vec2F(pointInView.x, 1 - pointInView.y)).ToUnityVector();
        }

        internal string DumpLite()
        {
            var data = string.Empty;
            data += $"Session: {State}{(State >= SessionState.Ready && TrackingStatus.OnNone ? " (No Motion)" : "")}, {CameraTransformType}, {CenterMode} ({(CenterObject ? CenterObject.name : "-")}), {frameFlip}" + Environment.NewLine;
            if (Assembly != null && State >= SessionState.Ready && TrackingStatus.OnSome)
            {
                data += $"- {(Assembly.IsASync ? "async" : "sync")}, {TrackingStatus}, {(Origin ? Origin.name : "-")}" + Environment.NewLine;
            }
            if (assmbleDumpMessage.OnSome)
            {
                data += assmbleDumpMessage.Value.Item1 + Environment.NewLine;
                if (Time.realtimeSinceStartup - assmbleDumpMessage.Value.Item2 > 5)
                {
                    assmbleDumpMessage = null;
                }
            }
            return data;
        }

        private void DumpAvailabilityReport(SessionReport.AvailabilityReport report, bool isUpdate)
        {
            var message = $"Availability:{(isUpdate ? " Updated (restart ARSession/App to take effect)" : string.Empty)}" + Environment.NewLine;
            if (report.DeviceList.Count > 0)
            {
                message += $"- Device list update status:" + Environment.NewLine;
                foreach (var item in report.DeviceList)
                {
                    message += $"  - {item.Type}: {item.Status} {item.Error}" + Environment.NewLine;
                }
            }
            if (report.FrameSources.Count > 0)
            {
                message += $"- Frame Source:" + Environment.NewLine;
                foreach (var item in report.FrameSources)
                {
                    message += $"  - {ARSessionFactory.DefaultName(item.Component.GetType())}: {item.Availability}" + Environment.NewLine;
                }
            }
            if (report.FrameFilters.Count > 0)
            {
                message += $"- Frame Filter:" + Environment.NewLine;
                foreach (var item in report.FrameFilters)
                {
                    message += $"  - {ARSessionFactory.DefaultName(item.Component.GetType())}: {item.Availability}" + Environment.NewLine;
                }
            }
            assmbleDumpMessage = Tuple.Create(message, Time.realtimeSinceStartup);
        }

        private void StartSession(bool isRetry)
        {
            StartCoroutine(Assemble((report) =>
            {
                if (Report.BrokenReason.OnSome)
                {
                    if (!isRetry && Report.BrokenReason == SessionReport.SessionBrokenReason.NoAvailabileFrameSource && report.PendingDeviceList.Count > 0)
                    {
                        Diagnostics.EnqueueWarning(Report.Exception.ToString() + Environment.NewLine + "Device list updating...");
                    }
                    else
                    {
                        Diagnostics.EnqueueSessionError(Report.Exception.ToString());
                    }
                }
                if (State == SessionState.Assembled)
                {
                    StartAssembledSession();
                }
            }, (report) =>
            {
                if (isRetry) { return; }

                if (Report.BrokenReason == SessionReport.SessionBrokenReason.NoAvailabileFrameSource)
                {
                    if (report.FrameSources.Where(f => f.Availability == SessionReport.AvailabilityReport.AvailabilityStatus.Available).Any())
                    {
                        StartSession(true);
                    }
                    else
                    {
                        Diagnostics.EnqueueSessionError(Report.Exception.ToString());
                    }
                }
            }));
        }

        private IEnumerator Assemble(Action<SessionReport.AvailabilityReport> assemblefinish, Action<SessionReport.AvailabilityReport> availabilityUpdate)
        {
            if (State >= SessionState.Assembling)
            {
                throw new Exception($"Unable to assemble {this} in state {State}");
            }
            State = SessionState.Assembling;

            var newOptions = AssembleOptions.Clone();
            if (Application.isEditor && Diagnostics && Diagnostics.IsValidateFrame)
            {
                newOptions.FrameSource = AssembleOptions.FrameSourceSelection.FramePlayer;
            }

            yield return ARAssembler.AssembleSession(gameObject, newOptions, new System.Diagnostics.StackTrace(true), (assembly, report) =>
            {
                Assembly = assembly;
                Report = report;
                if (report.BrokenReason.OnSome)
                {
                    BreakSession(report.Exception, true);
                }
                else
                {
                    State = SessionState.Assembled;
                }
                DumpAvailabilityReport(report.Availability, false);
                assemblefinish?.Invoke(report.Availability);
                AssembleUpdate?.Invoke(report.Availability);
            }, (report) =>
            {
                DumpAvailabilityReport(report, true);
                availabilityUpdate?.Invoke(report);
                AssembleUpdate?.Invoke(report);
            });
        }

        private void StartAssembledSession()
        {
            try
            {
                if (State != SessionState.Assembled)
                {
                    throw new Exception($"Session not assembled");
                }

                if (Application.isEditor && Diagnostics.IsValidateFrame && Assembly.FrameSource is FramePlayer)
                {
                    Assembly.FrameSource.enabled = false;
                }
                if (Assembly.FrameSource is FrameSource.ISyncMotionSource frameSource)
                {
                    frameSource.MotionUpdate += UpdateSessionWithMotion;
                }

                StopLastCameraImageRenderer(Assembly.CameraImageRenderer);

                if (!Assembly.FrameSource) { throw new Exception($"component destroyed: {Assembly.FrameSource?.GetType() ?? typeof(FrameSource)}"); }
                startedComponets.Add(Assembly.FrameSource);
                Assembly.FrameSource.OnSessionStart(this);
                if (Assembly.CameraController.OnSome)
                {
                    if (!Assembly.CameraController.Value) { throw new Exception($"component destroyed: {Assembly.CameraController.Value?.GetType() ?? typeof(RenderCameraController)}"); }
                    startedComponets.Add(Assembly.CameraController.Value);
                    Assembly.CameraController.Value.OnSessionStart(this);
                }
                if (Assembly.CameraImageRenderer.OnSome)
                {
                    if (!Assembly.CameraImageRenderer.Value) { throw new Exception($"component destroyed: {Assembly.CameraImageRenderer.Value?.GetType() ?? typeof(CameraImageRenderer)}"); }
                    startedComponets.Add(Assembly.CameraImageRenderer.Value);
                    Assembly.CameraImageRenderer.Value.OnSessionStart(this);
                }
                if (Assembly.FrameRecorder.OnSome)
                {
                    if (!Assembly.FrameRecorder.Value) { throw new Exception($"component destroyed: {Assembly.FrameRecorder.Value?.GetType() ?? typeof(FrameRecorder)}"); }
                    startedComponets.Add(Assembly.FrameRecorder.Value);
                    Assembly.FrameRecorder.Value.OnSessionStart(this);
                }
                foreach (var filter in Assembly.FrameFilters)
                {
                    if (!filter) { throw new Exception($"component destroyed: {filter?.GetType() ?? typeof(FrameFilter)}"); }
                    startedComponets.Add(filter);
                    filter.OnSessionStart(this);
                }

                if (oFrameBuffer == null)
                {
                    oFrameBuffer = OutputFrameBuffer.create();
                }
                Assembly.Assemble(oFrameBuffer);
                if (!AvailableCenterMode.Contains(CenterMode))
                {
                    Debug.LogWarning($"Center mode {CenterMode} is unavailable in this session, reset to {AvailableCenterMode[0]}.");
                    CenterMode = AvailableCenterMode[0];
                }
                // NOTE: device sdk will usually provide XRLoader for HMD frame source using XROrigin, then swich will be default off. If it comes to switch in some case, we respect the scene user setup (all objects in the scene should work on device).
                var isXROriginSource = Assembly.FrameSource is ExternalDeviceFrameSource ext && ext.OriginType == ExternalDeviceFrameSource.DeviceOriginType.XROrigin;
                xrSwitcher?.Switch(Assembly.FrameSource is ARFoundationFrameSource || isXROriginSource, Assembly.FrameSource is ARFoundationFrameSource || isXROriginSource, diagnostics);
                State = SessionState.Ready;
                StartCoroutine(CheckComponents());
            }
            catch (Exception e)
            {
                // rollback
                StopAssembledSession(false);
                BreakSession(new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.StartFailed, string.Empty, null, e));
            }
        }

        private void StopAssembledSession(bool keepLastFrame)
        {
            try
            {
                xrSwitcher?.Switch(false, false, null);
                oFrameBuffer?.signalOutput().disconnect();
                Assembly?.Break();
            }
            catch (Exception ex) { Debug.LogError(ex); }

            if (Assembly != null)
            {
                if (Assembly.Origin.OnSome)
                {
                    try
                    {
                        if (Assembly.DefaultOriginChild.Value)
                        {
                            Assembly.DefaultOriginChild.Value.OnSessionStop();
                        }
                        if (Assembly.Origin.Value)
                        {
                            foreach (var c in Assembly.Origin.Value.GetComponentsInChildren<XROriginChildController>(true).Where(c => c != Assembly.DefaultOriginChild))
                            {
                                c.OnSessionStop();
                            }
                        }
                    }
                    catch (Exception ex) { Debug.LogError(ex); }
                }
                foreach (var filter in Assembly.FrameFilters)
                {
                    try
                    {
                        if (filter && startedComponets.Contains(filter))
                        {
                            filter.OnSessionStop();
                        }
                    }
                    catch (Exception ex) { Debug.LogError(ex); }
                }
                if (Assembly.FrameRecorder.OnSome)
                {
                    try
                    {
                        if (Assembly.FrameRecorder.Value && startedComponets.Contains(Assembly.FrameRecorder.Value))
                        {
                            Assembly.FrameRecorder.Value.OnSessionStop();
                        }
                    }
                    catch (Exception ex) { Debug.LogError(ex); }
                }
                if (Assembly.CameraImageRenderer.OnSome)
                {
                    try
                    {
                        if (Assembly.CameraImageRenderer.Value && startedComponets.Contains(Assembly.CameraImageRenderer.Value))
                        {
                            if (keepLastFrame)
                            {
                                lastCameraImageRenderer = Assembly.CameraImageRenderer.Value;
                            }
                            Assembly.CameraImageRenderer.Value.OnSessionStop(keepLastFrame);
                        }
                    }
                    catch (Exception ex) { Debug.LogError(ex); }
                }
                if (Assembly.CameraController.OnSome)
                {
                    try
                    {
                        if (Assembly.CameraController.Value && startedComponets.Contains(Assembly.CameraController.Value))
                        {
                            Assembly.CameraController.Value.OnSessionStop();
                        }
                    }
                    catch (Exception ex) { Debug.LogError(ex); }
                }
                try
                {
                    if (Assembly.FrameSource && startedComponets.Contains(Assembly.FrameSource))
                    {
                        Assembly.FrameSource.OnSessionStop();
                    }
                }
                catch (Exception ex) { Debug.LogError(ex); }

                if (Assembly.FrameSource is FrameSource.ISyncMotionSource frameSource)
                {
                    frameSource.MotionUpdate -= UpdateSessionWithMotion;
                }
            }

            try
            {
                Diagnostics?.ClearCaution();
            }
            catch (Exception ex) { Debug.LogError(ex); }
            StopAllCoroutines();
            startedComponets.Clear();
        }

        private void StopLastCameraImageRenderer(Optional<CameraImageRenderer> current)
        {
            if (lastCameraImageRenderer && lastCameraImageRenderer != current)
            {
                lastCameraImageRenderer.OnSessionStop(false);
            }
            lastCameraImageRenderer = null;
            if (frameIndex != -1)
            {
                lastFrameIndex = frameIndex;
            }
            if (Assembly != null && !Assembly.IsASync)
            {
                lastFrameIndex = Optional<int>.Empty;
            }
        }

        private IEnumerator CheckComponents()
        {
            var failMessage = string.Empty;
            while (true)
            {
                yield return null;
                if (State < SessionState.Ready || Assembly == null) { break; }
                if (Assembly.FrameSource)
                {
                    var source = Assembly.FrameSource;
                    if (!source || !source.gameObject.activeInHierarchy)
                    {
                        failMessage = !source ? $"component destroyed: {source?.GetType() ?? typeof(FrameSource)}" : $"gameObject disabled: {source.gameObject}";
                        break;
                    }
                }
                foreach (var filter in Assembly.FrameFilters)
                {
                    if (!filter || !filter.gameObject.activeInHierarchy)
                    {
                        failMessage = !filter ? $"component destroyed: {filter?.GetType() ?? typeof(FrameFilter)}" : $"gameObject disabled: {filter.gameObject}";
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(failMessage)) { break; }
            }
            if (!string.IsNullOrEmpty(failMessage))
            {
                StopSession(false);
                BreakSession(new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.RunningFailed, failMessage, null));
            }
        }

        private bool CanSessionUpdate()
        {
            if (Current != this)
            {
                Diagnostics.EnqueueError("Multiple EasyAR session NOT allowed!");
                return false;
            }

            if (EasyARController.LicenseValidation.InitializationState == ValidationState.Pending && !EasyARController.LicenseValidation.IsAssembliesReloaded) { return false; }

            if (EasyARController.LicenseValidation.InitializeCount != initializeCount)
            {
                initializeCount = EasyARController.LicenseValidation.InitializeCount;
                if (initializeCount > 1)
                {
                    Diagnostics.RecoverFromSenseError();
                }
            }
            var error = EasyARController.LicenseValidation.Error;
            if (error.OnSome)
            {
                if (errorIndex != EasyARController.LicenseValidation.ErrorIndex)
                {
                    errorIndex = EasyARController.LicenseValidation.ErrorIndex;
                    if (State >= SessionState.Ready)
                    {
                        BreakSession(new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.RunningFailed, error.Value, null), true);
                        ClearSession();
                    }
                    Diagnostics.EnqueueSenseError(error.Value);
                }
                return false;
            }

            if (State < SessionState.Ready)
            {
                ClearSession();
                return false;
            }
            return true;
        }

        private void UpdateSessionWithMotion(MotionInputData motion)
        {
            if (!CanSessionUpdate()) { return; }
            if (Assembly.IsASync) { return; }
            if (motion == null) { return; }
            if (!enabled) { return; }

            UpdateSessionSync(motion);
        }

        private void UpdateSessionAsync()
        {
            EasyARController.RunScheduler();
            var oFrame = oFrameBuffer.peek();
            if (oFrame.OnNone)
            {
                ClearSession();
                return;
            }

            State = SessionState.Running;
            using (var outputFrame = oFrame.Value)
            using (var iFrame = outputFrame.inputFrame())
            {
                if (!CheckFramePlayer(iFrame)) { return; }

                if (lastFrameIndex.OnSome)
                {
                    if (lastFrameIndex == iFrame.index()) { return; }
                    lastFrameIndex = Optional<int>.Empty;
                }

                bool isFrontCamera = false;
                if (iFrame.hasCameraParameters())
                {
                    var cameraParameters = iFrame.cameraParameters();
                    frameCameraParameters?.Dispose();
                    frameCameraParameters = cameraParameters;
                    isFrontCamera = cameraParameters.cameraDeviceType() == CameraDeviceType.Front;
                    if (Application.isEditor)
                    {
                        var imageOrientation = cameraParameters.imageOrientation(Assembly.Display.Rotation);
                        CheckDisplay(cameraParameters.size(), imageOrientation);
                    }
                }
                var results = outputFrame.results();
                var hFlip = isFrontCamera ? HorizontalFlip.FrontCamera : HorizontalFlip.BackCamera;
                var displayCompensation = frameCameraParameters != null ? Quaternion.Euler(0, 0, -frameCameraParameters.imageOrientation(Assembly.Display.Rotation)) : Quaternion.identity;
                var cameraFrame = frameIndex != iFrame.index() ? Tuple.Create(outputFrame, displayCompensation) : Optional<Tuple<OutputFrame, Quaternion>>.Empty;
                frameIndex = iFrame.index();
                frameMotion?.Dispose();
                frameMotion = iFrame.motion().ValueOrDefault(null);

                try
                {
                    CheckSession(frameMotion);
                    UpdateCenterMode(frameMotion);
                    hasData = true;
                    DispatchResults(results, frameMotion);
                    UpdateFlip(hFlip);
                    UpdateTransform(frameMotion, displayCompensation, hFlip);
                    UpdateCamera(cameraFrame);
                    SessionUpdate?.Invoke();
                    if (cameraFrame.OnSome)
                    {
                        InputFrameUpdate?.Invoke(iFrame);
                    }
                }
                catch (SessionReport.SessionBrokenException e)
                {
                    BreakSession(e);
                }
                catch (DiagnosticsMessageException e)
                {
                    BreakSession(new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.RunningFailed, e.Message, null));
                }
                finally
                {
                    foreach (var result in results.Where(r => r.OnSome))
                    {
                        result.Value.Dispose();
                    }
                }
                PostSessionUpdate?.Invoke();
            }
        }

        private void UpdateSessionSync(MotionInputData motion)
        {
            EasyARController.RunScheduler();
            State = SessionState.Running;
            frameMotion?.Dispose();
            frameMotion = motion.Clone();

            try
            {
                CheckSession(frameMotion);
                UpdateCenterMode(frameMotion);
                hasData = true;
                RetrieveSyncResults(frameMotion);
                UpdateTransform(frameMotion, Quaternion.identity, ARHorizontalFlipMode.None);
                SessionUpdate?.Invoke();
            }
            catch (SessionReport.SessionBrokenException e)
            {
                BreakSession(e);
            }
            catch (DiagnosticsMessageException e)
            {
                BreakSession(new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.RunningFailed, e.Message, null));
            }
            PostSessionUpdate?.Invoke();
        }

        private bool CheckFramePlayer(InputFrame iFrame)
        {
            if (Assembly.FrameSource is FramePlayer player)
            {
                if (!iFrame.hasTemporalInformation())
                {
                    if (!hasFramePlayerError)
                    {
                        player.Stop();
                        Diagnostics.EnqueueError($"Invalid input frame from {nameof(FramePlayer)}.\n The file may be corrupted or not recorded by EasyAR Sense.");
                        hasFramePlayerError = true;
                    }
                    return false;
                }
                hasFramePlayerError = false;
            }
            return true;
        }

        private void CheckSession(MotionInputData motion)
        {
            // check assembly
            var destroyedComponet = string.Empty;
            if (!Assembly.Camera) // 3rdparty may destroy camera at anytime (e.g., xreal destroy camera when device disconnect)
            {
                destroyedComponet = $"{Assembly.Camera?.GetType() ?? typeof(Camera)}";
            }
            if (Assembly.Origin.OnSome && !Assembly.Origin.Value)
            {
                destroyedComponet = $"{Assembly.Origin.Value?.GetType() ?? typeof(GameObject)} (Origin)";
            }
            if (Assembly.DefaultOriginChild.OnSome && !Assembly.DefaultOriginChild.Value)
            {
                destroyedComponet = $"{Assembly.DefaultOriginChild.Value?.GetType() ?? typeof(XROriginChildController)}";
            }
            if (!Assembly.FrameSource)
            {
                destroyedComponet = $"{Assembly.FrameSource?.GetType() ?? typeof(FrameSource)}";
            }
            foreach (var filter in Assembly.FrameFilters)
            {
                if (!filter)
                {
                    destroyedComponet = $"{filter?.GetType() ?? typeof(FrameFilter)}";
                    break;
                }
            }
            if (!string.IsNullOrEmpty(destroyedComponet))
            {
                StopSession(false);
                throw new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.RunningFailed, $"component destroyed: {destroyedComponet}", null);
            }
            // check session origin
            if (motion != null && Assembly.FrameSource is FrameSource.IMotionTrackingDevice mtd && !Origin)
            {
                StopSession(false);
                throw new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.RunningFailed, $"component destroyed: Origin", null);
            }
            // check URP
            if (Assembly.CameraImageRenderer.OnSome && Diagnostics.IsRenderPipelineProperlySetup.OnSome && !Diagnostics.IsRenderPipelineProperlySetup.Value)
            {
                StopSession(false);
                throw new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.RunningFailed, Diagnostics.RenderPipelineSetupError, null);
            }
        }

        private void UpdateCenterMode(MotionInputData motion)
        {
            // update Assembly center mode
            Assembly.OnFrameInput(motion != null);
            // check center mode
            if (!AvailableCenterMode.Contains(CenterMode))
            {
                Debug.LogWarning($"Center mode {CenterMode} is unavailable in this session, reset to {AvailableCenterMode[0]}.");
                CenterMode = AvailableCenterMode[0];
            }
        }

        private void DispatchResults(List<Optional<FrameFilterResult>> results, MotionInputData motion)
        {
            var joinIndex = 0;
            foreach (var filter in Assembly.FrameFilters.Where(f => f is FrameFilter.IOutputFrameSource))
            {
                (filter as FrameFilter.IOutputFrameSource).OnResult(results[joinIndex], motion);
                joinIndex++;
            }
        }

        private void RetrieveSyncResults(MotionInputData motion)
        {
            foreach (var filter in Assembly.FrameFilters.Where(f => f is FrameFilter.IOutputFrameSource))
            {
                (filter as FrameFilter.IOutputFrameSource).RetrieveSyncResults(motion);
            }
        }

        private void UpdateFlip(ARHorizontalFlipMode hflip)
        {
            if (Assembly.CameraController.OnSome && Assembly.CameraController.Value)
            {
                Assembly.CameraController.Value.SetProjectHFlip(hflip == ARSession.ARHorizontalFlipMode.World);
            }
            if (Assembly.CameraImageRenderer.OnSome && Assembly.CameraImageRenderer.Value)
            {
                Assembly.CameraImageRenderer.Value.SetProjectHFlip(hflip == ARSession.ARHorizontalFlipMode.World);
                Assembly.CameraImageRenderer.Value.SetRenderImageHFilp(hflip != ARSession.ARHorizontalFlipMode.None);
            }
            foreach (var filter in Assembly.FrameFilters)
            {
                if (!(filter is FrameFilter.IHFlipHandler hFlipHandler)) { continue; }
                hFlipHandler.SetHFlip(hflip == ARHorizontalFlipMode.Target);
            }
            frameFlip = hflip;
        }

        private void UpdateTransform(MotionInputData motion, Quaternion displayCompensation, ARHorizontalFlipMode hflip)
        {
            // get session origin
            var sessionOrigin = Optional<Tuple<GameObject, Pose>>.CreateNone();
            if (motion != null)
            {
                sessionOrigin = Tuple.Create(Origin, motion.transform().ToUnityPose().Inverse());
                Assembly.DefaultOriginChild.Value.OnTracking(Origin, motion.trackingStatus());
                foreach (var c in Origin.GetComponentsInChildren<XROriginChildController>(true).Where(c => c != Assembly.DefaultOriginChild))
                {
                    c.OnTracking(Origin, motion.trackingStatus());
                }
            }

            // get center
            var center = Optional<Tuple<GameObject, Pose>>.CreateNone();
            if (CenterMode == ARCenterMode.FirstTarget)
            {
                for (var i = 0; i < 2; ++i)
                {
                    foreach (var filter in Assembly.FrameFilters)
                    {
                        if (!(filter is FrameFilter.ICenterTransform centerTransform)) { continue; }
                        center = centerTransform.TryGetCenter(CenterObject);
                        if (center.OnSome) { break; }
                    }
                    if (center.OnSome || !CenterObject)
                    {
                        break;
                    }
                    CenterObject = null;
                }
            }
            else if (CenterMode == ARCenterMode.SpecificTarget && SpecificTargetCenter)
            {
                foreach (var filter in Assembly.FrameFilters)
                {
                    if (!(filter is FrameFilter.ICenterTransform centerTransform)) { continue; }
                    center = centerTransform.TryGetCenter(SpecificTargetCenter);
                    if (center.OnSome) { break; }
                }
            }
            else if (CenterMode == ARCenterMode.SessionOrigin && sessionOrigin.OnSome)
            {
                center = sessionOrigin;
            }
            else if (CenterMode == ARCenterMode.Camera)
            {
                center = Tuple.Create(Assembly.Camera.gameObject, new Pose(Vector3.zero, displayCompensation).Inverse());
            }

            if (center.OnNone && AvailableCenterMode.Contains(ARCenterMode.SessionOrigin) && sessionOrigin.OnSome)
            {
                center = sessionOrigin;
            }
            if (center.OnNone && AvailableCenterMode.Contains(ARCenterMode.Camera))
            {
                center = Tuple.Create(Assembly.Camera.gameObject, new Pose(Vector3.zero, displayCompensation).Inverse());
            }

            if (center.OnNone)
            {
                return;
            }
            CenterObject = center.Value.Item1;

            var targetHFlip = hflip == ARHorizontalFlipMode.Target;

            if (center.Value.Item1 != Assembly.Camera.gameObject
                && !(sessionOrigin.OnSome && center.Value.Item1 == sessionOrigin.Value.Item1)
                && (center.Value.Item1.transform.IsChildOf(Assembly.Camera.transform) || (sessionOrigin.OnSome && center.Value.Item1.transform.IsChildOf(sessionOrigin.Value.Item1.transform)))
                )
            {
                Debug.LogWarning($"Detected session center {center.Value.Item1} is a child of camera or origin, which may cause unexpected behavior. The parent will be cleared.");
                center.Value.Item1.transform.SetParent(null, true);
            }

            // set session origin transform before camera
            if (sessionOrigin.OnSome && center.Value.Item1 != sessionOrigin.Value.Item1)
            {
                var originToCamera = sessionOrigin.Value.Item2;
                var cameraToWorld = center.Value.Item2.Inverse()
                    .FlipX(targetHFlip)
                    .GetTransformedBy(new Pose(center.Value.Item1.transform.position, center.Value.Item1.transform.rotation));
                var p = originToCamera
                    .FlipX(targetHFlip)
                    .GetTransformedBy(cameraToWorld);

                sessionOrigin.Value.Item1.transform.position = p.position;
                sessionOrigin.Value.Item1.transform.rotation = p.rotation;
            }

            // set camera transform
            if (center.Value.Item1 != Assembly.Camera.gameObject && Assembly.FrameSource.IsCameraUnderControl)
            {
                var cameraToCenter = center.Value.Item2.Inverse();
                var p = new Pose(Vector3.zero, displayCompensation).Inverse()
                    .GetTransformedBy(cameraToCenter)
                    .FlipX(targetHFlip)
                    .GetTransformedBy(new Pose(center.Value.Item1.transform.position, center.Value.Item1.transform.rotation));

                Assembly.Camera.transform.position = p.position;
                Assembly.Camera.transform.rotation = p.rotation;
            }

            // set tracked object transform
            foreach (var filter in Assembly.FrameFilters)
            {
                if (!(filter is FrameFilter.ICenterTransform centerTransform)) { continue; }
                centerTransform.UpdateTransform(center.Value.Item1, center.Value.Item2);
            }
        }

        private void UpdateCamera(Optional<Tuple<OutputFrame, Quaternion>> frame)
        {
            if (Assembly.CameraController.OnSome && Assembly.CameraController.Value)
            {
                Assembly.CameraController.Value.UpdateCamera(frame);
            }
            if (Assembly.CameraImageRenderer.OnSome && Assembly.CameraImageRenderer.Value)
            {
                Assembly.CameraImageRenderer.Value.UpdateCamera(frame);
            }
        }

        private void ClearSession()
        {
            if (State >= SessionState.Ready)
            {
                State = SessionState.Paused;
            }
            if (hasData)
            {
                if (Assembly != null)
                {
                    if (Assembly.DefaultOriginChild.OnSome && Assembly.DefaultOriginChild.Value)
                    {
                        Assembly.DefaultOriginChild.Value.OnEmptyFrame();
                    }
                    if (Assembly.Origin.OnSome && Assembly.Origin.Value)
                    {
                        foreach (var c in Assembly.Origin.Value.GetComponentsInChildren<XROriginChildController>(true).Where(c => c != Assembly.DefaultOriginChild))
                        {
                            c.OnEmptyFrame();
                        }
                    }
                    if (Assembly.CameraImageRenderer.OnSome && Assembly.CameraImageRenderer.Value)
                    {
                        Assembly.CameraImageRenderer.Value.ClearCamera();
                    }
                    foreach (var filter in Assembly.FrameFilters.Where(f => f is FrameFilter.IOutputFrameSource))
                    {
                        if (!filter) { continue; }
                        var outputFrameSource = filter as FrameFilter.IOutputFrameSource;
                        if (Assembly.IsASync)
                        {
                            outputFrameSource.OnResult(null, null);
                        }
                        else
                        {
                            outputFrameSource.RetrieveSyncResults(null);
                        }
                    }
                    foreach (var filter in Assembly.FrameFilters)
                    {
                        if (!filter) { continue; }
                        if (!(filter is FrameFilter.ICenterTransform centerTransform)) { continue; }
                        if (!Assembly.Camera || !Assembly.Camera.gameObject) { continue; }
                        centerTransform.UpdateTransform(Assembly.Camera.gameObject, Pose.identity);
                    }
                }

                hasData = false;
                frameMotion?.Dispose();
                frameMotion = null;
            }
            frameCameraParameters?.Dispose();
            frameCameraParameters = null;
        }

        private void BreakSession(SessionReport.SessionBrokenException exception, bool silent = false)
        {
            Report.OnSessionBroken(exception.Reason, exception);
            if (!silent)
            {
                Diagnostics.EnqueueSessionError(Report.Exception.ToString());
            }
            State = SessionState.Broken;
        }

        private void CheckDisplay(Vec2I camSize, int imageRotation)
        {
            var deviceAspect = (float)camSize.data_0 / camSize.data_1;
            if (imageRotation == 90 || imageRotation == 270)
            {
                deviceAspect = 1 / deviceAspect;
            }
            var screenAspect = (float)Screen.width / Screen.height;
            var aspect = deviceAspect / screenAspect;
            if (aspect == preAspect) { return; }
            if (aspect > 1.5 || aspect < 0.667)
            {
                Debug.LogWarning($"aspect ratio not match (screen={screenAspect} device={deviceAspect}), you are losing too much of view. Change 'Game' view aspect to {deviceAspect} to see full image.");
            }
            preAspect = aspect;
        }

        /// <summary>
        /// <para xml:lang="en">Flip rendering options.</para>
        /// <para xml:lang="zh">镜像渲染选项。</para>
        /// </summary>
        [Serializable]
        public class FlipOptions
        {
            /// <summary>
            /// <para xml:lang="en">Horizontal flip rendering mode for back camera.</para>
            /// <para xml:lang="zh">后置相机的水平镜像渲染模式。</para>
            /// </summary>
            public ARHorizontalFlipMode BackCamera;

            /// <summary>
            /// <para xml:lang="en">Horizontal flip rendering mode for front camera.</para>
            /// <para xml:lang="zh">前置相机的水平镜像渲染模式。。</para>
            /// </summary>
            public ARHorizontalFlipMode FrontCamera = ARHorizontalFlipMode.World;
        }
    }
}
