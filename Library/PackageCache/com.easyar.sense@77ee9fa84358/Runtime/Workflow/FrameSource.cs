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
    /// <para xml:lang="en">A frame source represents an input frame data provider, it will provide image data or together with motion data.</para>
    /// <para xml:lang="en">You can inherent a few sub types of <see cref="FrameSource"/> to implenment custom camera, but you cannot inherent <see cref="FrameSource"/> directly. Custom camera represnets a new device or data input method usaully.</para>
    /// <para xml:lang="en">EasyAR Sense will stop responding after a fixed and limited time per run if trial product (personal license, trial XR license, or trial Mega services, etc.) is being used with custom camera or HMD.</para>
    /// <para xml:lang="zh">一个frame source代表frame输入数据的数据源，它提供图像数据或同时提供运动数据。</para>
    /// <para xml:lang="zh">你可以通过继承<see cref="FrameSource"/>的一些子类型来实现自定义相机，但你不能直接继承<see cref="FrameSource"/>。自定义相机通常表达一个新的设备或新的数据输入方式。</para>
    /// <para xml:lang="zh">在自定义相机或头显上使用试用产品（个人版license、试用版XR license或试用版Mega服务等）时，EasyAR Sense每次启动后会在固定的有限时间内停止响应。</para>
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class FrameSource : MonoBehaviour
    {
        private protected static IReadOnlyList<ARSession.ARCenterMode> AllCenterModes = Enum.GetValues(typeof(ARSession.ARCenterMode)).Cast<ARSession.ARCenterMode>().ToList();
        private InputFrameSink sink;
        private int bufferCapacity;

        internal abstract bool IsManuallyDisabled { get; }
        internal virtual int BufferCapacity { get => bufferCapacity; set => bufferCapacity = value; }

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="en">On desktop or mobile phones, it represents the <see cref="UnityEngine.Camera"/> in the virtual world in reflection of real world camera device, its projection matrix and transform will be set to reflect the real world camera, and it is controlled by EasyAR. On head mounted displays, it is only used to display some diagnostics message in front of your eyes, it is not used for camera image rendering, it is not controled by EasyAR.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.Assemble"/>过程中被访问。</para>
        /// <para xml:lang="zh">在桌面设备或手机上，该相机代表现实环境中相机设备在虚拟世界中对应的<see cref="UnityEngine.Camera"/>，其投影矩阵和位置都将与真实相机对应，受EasyAR控制。在头显上，该相机仅用于将一些诊断文字展示在眼前，不用于画面渲染，相机也不受EasyAR控制。</para>
        /// </summary>
        internal protected abstract Camera Camera { get; }

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="en">When the value is ture, the session will update camera transform depending on the center mode and render camera images.</para>
        /// <para xml:lang="en">When creating a HMD extension, it should be false. You should have full control of the 3D camera in the scene. You should handle camera rendering, especially in VST.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.Assemble"/>过程中被访问。</para>
        /// <para xml:lang="zh">当值为ture时，session 会更新相机的transform，且会渲染相机图像.</para>
        /// <para xml:lang="zh">在创建头显扩展时，它应为false。你应该完全控制场景中的3D相机。你应该处理相机渲染，尤其是在VST模式下。</para>
        /// </summary>
        internal protected abstract bool IsCameraUnderControl { get; }

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="en">If the frame source represent head mounted displays. If true, diagnostics will be displayed on a 3D board in from of camera.</para>
        /// <para xml:lang="en">Some frame filter may work different on devices.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.Assemble"/>过程中被访问。</para>
        /// <para xml:lang="zh">当前frame source是否是头显。如果是头显，诊断信息将显示在相机前的3D板子上。</para>
        /// <para xml:lang="zh">部分frame filter在设备上运行会有不同。</para>
        /// </summary>
        internal protected abstract bool IsHMD { get; }

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="en">Provide display system information. You can use <see cref="Display.DefaultSystemDisplay"/> or <see cref="Display.DefaultHMDDisplay"/> for default display.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.Assemble"/>过程中被访问。</para>
        /// <para xml:lang="zh">提供显示系统信息。你可以使用<see cref="Display.DefaultSystemDisplay"/>或<see cref="Display.DefaultHMDDisplay"/>来获取默认的显示信息。</para>
        /// </summary>
        internal protected abstract IDisplay Display { get; }

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="en">If the frame source is available.</para>
        /// <para xml:lang="en">If the value equals null, <see cref="CheckAvailability"/> will be called and the value will be accessed after <see cref="Coroutine"/> finish.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.Assemble"/>过程中被访问。</para>
        /// <para xml:lang="zh">当前frame source是否可用。</para>
        /// <para xml:lang="zh">如果数值等于null，<see cref="CheckAvailability"/>会被调用，数值将在<see cref="Coroutine"/>结束后获取。</para>
        /// </summary>
        internal protected abstract Optional<bool> IsAvailable { get; }
        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="en">All available center modes.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.Assemble"/>过程中被访问。</para>
        /// <para xml:lang="zh">所有可以使用的中心模式。</para>
        /// </summary>
        internal protected virtual IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode { get => AllCenterModes; }

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will be accessed during the whole life time of <see cref="ARSession"/>.</para>
        /// <para xml:lang="en">If the camera frame has started to input.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它会在<see cref="ARSession"/>的整个生命周期内被访问。</para>
        /// <para xml:lang="zh">相机帧是否开始输入。</para>
        /// </summary>
        internal protected abstract bool CameraFrameStarted { get; }

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will be accessed when <see cref="CameraFrameStarted"/> is true.</para>
        /// <para xml:lang="en">Device camera to provide camera frame data. The list must contain all cameras if camera frames are provided by multiple cameras.</para>
        /// <para xml:lang="en">Make sure the value is correct when <see cref="CameraFrameStarted"/> is true.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它会在<see cref="CameraFrameStarted"/>为true时被访问。</para>
        /// <para xml:lang="zh">提供相机帧数据的设备相机。如果相机帧数据由多个相机提供，列表中需要包含所有相机。</para>
        /// <para xml:lang="zh">确保在<see cref="CameraFrameStarted"/>为true时数值正确。</para>
        /// </summary>
        internal protected abstract List<FrameSourceCamera> DeviceCameras { get; }

        internal static bool IsCustomCamera(FrameSource f) => f is ISenseExternalFrameSource || f is ISyncMotionSource;

        internal virtual string DumpLite()
        {
            var data = string.Empty;
            data += $"Source: {ARSessionFactory.DefaultName(GetType())} ({enabled})" + Environment.NewLine;
            return data;
        }

        internal virtual void Connect(InputFrameSink val) => sink = val;
        internal virtual void Disconnect() => sink = null;

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.StartSession"/>.</para>
        /// <para xml:lang="en">Handle session start if the frame source was assembled into <see cref="ARSession.Assembly"/>. It is designed for lazy initialization and you can do AR specific initialization work in the method.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.StartSession"/>过程中被访问。</para>
        /// <para xml:lang="zh">处理session启动，如果这个frame source已经组装进<see cref="ARSession.Assembly"/>。这个方法设计上是用来做延迟初始化的，你可以在这个方法中做AR独有的初始化工作。</para>
        /// </summary>
        internal protected abstract void OnSessionStart(ARSession session);
        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will be accessed during <see cref="ARSession.StopSession"/> or other session stop/break procedure.</para>
        /// <para xml:lang="en">Handle session stop if the frame source was assembled into <see cref="ARSession.Assembly"/>. You can use this method to destroy and resources created in <see cref="ARSession.StartSession"/> and during the session running and resotre inner state. It is ensured to be called before session destroy. If the frame source was destroyed before session, it will not be called and session will break.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它会在<see cref="ARSession.StopSession"/>或其它session停止/损坏过程中被访问。</para>
        /// <para xml:lang="zh">处理session停止，如果这个frame source已经组装进<see cref="ARSession.Assembly"/>。你可以使用这个方法销毁<see cref="ARSession.StartSession"/>以及session运行中创建的资源并恢复内部状态。在session销毁之前这个方法会被保证调用。如果frame source在sessino之前销毁，它将不会被调用，且session将损坏。</para>
        /// </summary>
        internal protected abstract void OnSessionStop();

        /// <summary>
        /// <para xml:lang="en">Provide only when creating a new frame source. It will only be accessed during <see cref="ARSession.Assemble"/>.</para>
        /// <para xml:lang="en"><see cref="Coroutine"/> to check frame source availability when <see cref="IsAvailable"/> equals null.</para>
        /// <para xml:lang="zh">仅当创建一个新的frame source时提供。它仅会在<see cref="ARSession.Assemble"/>过程中被访问。</para>
        /// <para xml:lang="zh"><see cref="IsAvailable"/>等于null时用于检查frame source是否可用的<see cref="Coroutine"/>。</para>
        /// </summary>
        internal protected virtual IEnumerator CheckAvailability() => null;

        internal interface IMotionTrackingDevice
        {
            GameObject Origin { get; }
        }

        internal interface ISyncMotionSource
        {
            event Action<MotionInputData> MotionUpdate;
        }

        internal interface ISenseBuiltinFrameSource
        {
            void ConnectFrom(InputFrameSource source)
            {
                if (this is not FrameSource frameSource) { return; }
                if (frameSource.sink == null) { return; }
                source.connect(frameSource.sink);
            }
        }

        internal interface ISenseExternalFrameSource
        {
            bool HandleCameraFrameData(InputFrame frame)
            {
                if (this is not FrameSource frameSource) { return false; }
                if (frameSource.sink == null) { return false; }
                frameSource.sink.handle(frame);
                return true;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">Camera to provide camera frame data in frame source.</para>
    /// <para xml:lang="zh">Frame source中提供相机帧数据的相机。</para>
    /// </summary>
    public class FrameSourceCamera : IDisposable
    {
        /// <summary>
        /// <para xml:lang="en">Set value according to the description of each property.</para>
        /// <para xml:lang="zh">根据每个属性的描述设置数值。</para>
        /// </summary>
        public FrameSourceCamera(CameraDeviceType cameraType, int cameraOrientation, Vector2Int frameSize, Vector2 frameRateRange)
        {
            CameraType = cameraType;
            CameraOrientation = cameraOrientation;
            FrameSize = frameSize;
            FrameRateRange = frameRateRange;
        }

        /// <summary>
        /// <para xml:lang="en">Camera device type.</para>
        /// <para xml:lang="zh">相机设备类型。</para>
        /// </summary>
        public CameraDeviceType CameraType { get; }
        /// <summary>
        /// <para xml:lang="en">Angles rotation required to rotate clockwise and display camera image on device with natural orientation. The range is [0, 360).</para>
        /// <para xml:lang="zh">camera图像在设备的自然方向上显示时需要顺时针旋转的角度。范围为[0, 360)。</para>
        /// </summary>
        public int CameraOrientation { get; }
        /// <summary>
        /// <para xml:lang="en">The size of image.</para>
        /// <para xml:lang="zh">图像的大小。</para>
        /// </summary>
        public Vector2Int FrameSize { get; }
        /// <summary>
        /// <para xml:lang="en">The frame rate range. Define x is lower bound and y is upper bound of the frame rate range.</para>
        /// <para xml:lang="zh">帧率范围。定义x为帧率范围下界y为帧率范围上界。</para>
        /// </summary>
        public Vector2 FrameRateRange { get; }

        // NOTICE: EasyAR Sense API may change in the near future, make sure to call Dispose as a usual IDisposable.
        public void Dispose()
        {
        }
    }

    /// <summary>
    /// <para xml:lang="en">Device camera to provide camera frame data in frame source.</para>
    /// <para xml:lang="zh">Frame source中提供相机帧数据的设备相机。</para>
    /// </summary>
    public class DeviceFrameSourceCamera : FrameSourceCamera
    {
        /// <summary>
        /// <para xml:lang="en">Set value according to the description of each property.</para>
        /// <para xml:lang="zh">根据每个属性的描述设置数值。</para>
        /// </summary>
        public DeviceFrameSourceCamera(CameraDeviceType cameraType, int cameraOrientation, Vector2Int frameSize, Vector2 frameRateRange, CameraExtrinsics extrinsics, AxisSystemType axisSystem) : base(cameraType, cameraOrientation, frameSize, frameRateRange)
        {
            Extrinsics = extrinsics;
            AxisSystem = axisSystem;
        }

        /// <summary>
        /// <para xml:lang="en">Camera extrinsics, it is usually a calibrated matrix. The axes should match with <see cref="AxisSystem"/> definition. If extrinsics axis definition is different with your pose axis definition or they do not match with <see cref="AxisSystem"/>, you must do axis conversion first before you set this value.</para>
        /// <para xml:lang="zh">相机外参，一般是标定的矩阵。其坐标轴应符合<see cref="AxisSystem"/>定义。如果外参的坐标轴定义与你的pose的坐标轴定义不同或它们不符合<see cref="AxisSystem"/>的定义，你需要在设置这个数值之前进行坐标轴变换。</para>
        /// </summary>
        public CameraExtrinsics Extrinsics { get; }
        /// <summary>
        /// <para xml:lang="en">Axis system used by head/camera pose and camera extrinsics. The same axis system should be used by all matrixes. Axis conversion should be taken over before you send data into EasyAR if your data does not match known axis system types.</para>
        /// <para xml:lang="zh">头/相机pose以及相机外参使用的坐标轴系统。所有矩阵必须使用相同的坐标轴系统。如果你的数据定义不符合已知的系统，你需要在传给EasyAR之前进行坐标轴变换。</para>
        /// </summary>
        public AxisSystemType AxisSystem { get; }

        /// <summary>
        /// <para xml:lang="en">Camera extrinsics, it is a matrix usually calibrated to explain physical offset of camera from device/head pose origin.</para>
        /// <para xml:lang="zh">相机外参，一般是标定的矩阵，表达在相机相对设备/头的pose原点的物理偏移。</para>
        /// </summary>
        public class CameraExtrinsics
        {
            private Pose pose = Pose.identity;
            private bool inverse;

            /// <summary>
            /// <para xml:lang="en">Pose represents calibrated extrinsics matrix Tcw (a.k.a. matrix from world (head) to camera) when inverse == false, Twc when inverse == true.</para>
            /// <para xml:lang="zh">Pose代表标定的外参矩阵，inverse == false时是Tcw（即from world (head) to camera的矩阵），inverse == true时是Twc。</para>
            /// </summary>
            public CameraExtrinsics(Pose pose, bool inverse)
            {
                this.pose = pose;
                this.inverse = inverse;
            }

            /// <summary>
            /// <para xml:lang="en">Camera extrinsics matrix Tcw.</para>
            /// <para xml:lang="zh">相机外参矩阵Tcw。</para>
            /// </summary>
            public Pose Value => inverse ? pose.Inverse() : pose;

            /// <summary>
            /// <para xml:lang="en">Inverse of camera extrinsics matrix Twc.</para>
            /// <para xml:lang="zh">相机外参矩阵的逆Twc。</para>
            /// </summary>
            public Pose Inverse => inverse ? pose : pose.Inverse();
        }
    }

    internal class FrameSourceInspector
    {
        private Tuple<int, float> frameCheck = Tuple.Create(-1, 0f);
        private Coroutine checkFrameCoroutine;
        private MonoBehaviour component;
        private bool started;

        public int ReceivedFrameCount { get; set; }

        public FrameSourceInspector(MonoBehaviour component)
        {
            this.component = component;
        }

        public void Start()
        {
            Stop();
            started = true;
            checkFrameCoroutine = component.StartCoroutine(CheckFrame());
        }

        public void Stop()
        {
            started = false;
            if (checkFrameCoroutine == null) { return; }
            component.StopCoroutine(checkFrameCoroutine);
            checkFrameCoroutine = null;
        }

        public void Reset()
        {
            frameCheck = Tuple.Create(ReceivedFrameCount, Time.realtimeSinceStartup);
        }

        public string Dump()
        {
            if (!started) { return Environment.NewLine; }

            var received = ReceivedFrameCount == 0 ? "-" : $"{ReceivedFrameCount / 100 * 100}+";
            if (ReceivedFrameCount == 0)
            {
                received += " (device initializing or not respond)";
            }
            var data = $"received {received}" + Environment.NewLine;
            var interval = Time.realtimeSinceStartup - frameCheck.Item2;
            if (ReceivedFrameCount > 0 && interval > 5)
            {
                data += $"- !! WARNING: device input has stuck for {(int)interval} seconds, please check your device !!" + Environment.NewLine;
            }
            return data;
        }

        private IEnumerator CheckFrame()
        {
            Reset();
            var diagnostics = DiagnosticsController.TryGetDiagnosticsController(component.gameObject);
            yield return new WaitForSeconds(5);

            while (true)
            {
                if (!diagnostics)
                {
                    diagnostics = DiagnosticsController.TryGetDiagnosticsController(component.gameObject);
                    if (!diagnostics)
                    {
                        yield return null;
                        continue;
                    }
                }
                if (frameCheck.Item1 != ReceivedFrameCount)
                {
                    frameCheck = Tuple.Create(ReceivedFrameCount, Time.realtimeSinceStartup);
                }
                var interval = Time.realtimeSinceStartup - frameCheck.Item2;

                if (frameCheck.Item1 == 0)
                {
                    diagnostics.EnqueueWarning($"0 frame received since start (for {(int)interval} seconds), device initializing or not respond", 1.5f);
                    yield return new WaitForSeconds(2);
                    continue;
                }
                if (interval > 5)
                {
                    diagnostics.EnqueueWarning($"Device input has stuck for {(int)interval} seconds, please check your device", 1.5f);
                    yield return new WaitForSeconds(2);
                    continue;
                }
                yield return null;
            }
        }
    }
}
