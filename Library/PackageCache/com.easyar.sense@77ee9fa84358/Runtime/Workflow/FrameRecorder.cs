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
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="InputFrameRecorder"/> and <see cref="VideoInputFrameRecorder"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">It will be automatically assembled into <see cref="ARSession.Assembly"/>.</para>
    /// <para xml:lang="en"><see cref="Behaviour.enabled"/> can be used to control record start/stop.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="InputFrameRecorder"/>和<see cref="VideoInputFrameRecorder"/> 的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">它会被自动组装进<see cref="ARSession.Assembly"/>。</para>
    /// <para xml:lang="zh"><see cref="Behaviour.enabled"/>可以控制录制开始和结束。</para>
    /// </summary>
    [DisallowMultipleComponent]
    public class FrameRecorder : MonoBehaviour
    {
        /// <summary>
        /// <para xml:lang="en">Start record automatically after session start.</para>
        /// <para xml:lang="zh">Session启动后自动启动录制。</para>
        /// </summary>
        public bool AutoStart;
        /// <summary>
        /// <para xml:lang="en">Recording configuration. Set before recording start (OnEnable or <see cref="ARSession.StartSession"/>).</para>
        /// <para xml:lang="zh">录制配置。可以在录制前设置（OnEnable或<see cref="ARSession.StartSession"/>之前）。</para>
        /// </summary>
        public RecordingConfiguration Configuration = new RecordingConfiguration();

        /// <summary>
        /// <para xml:lang="en">Event when the recorder can start.</para>
        /// <para xml:lang="zh">可以开始录制的事件。</para>
        /// </summary>
        public UnityEvent OnReady = new UnityEvent();
        /// <summary>
        /// <para xml:lang="en">Recorder start event.</para>
        /// <para xml:lang="zh">录制启动的事件。</para>
        /// </summary>
        public RecordStartEvent OnRecording = new RecordStartEvent();
        /// <summary>
        /// <para xml:lang="en">Recorder finish event.</para>
        /// <para xml:lang="zh">录制结束的事件。</para>
        /// </summary>
        public RecordFinishEvent OnFinish = new RecordFinishEvent();

        private VideoInputFrameRecorder recorder;
        private InputFrameRecorder recorderObsolete;
        private ARSession arSession;
        private Thread thread;
        private FrameSupplementRecorder supplementRecorder;
        private IFrameSupplementFilter supplementFilter;
        private FrameSupplementSensor supplementSensor;
        private Optional<RecordingConfiguration> workingConfig;

        /// <summary>
        /// <para xml:lang="en">Recording status.</para>
        /// <para xml:lang="zh">录制状态。</para>
        /// </summary>
        public RecorderStatus Status { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Absolute path of the file being recorded.</para>
        /// <para xml:lang="zh">当前在录制的文件的绝对路径。</para>
        /// </summary>
        public string RecordingFile { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Format of the file being recorded.</para>
        /// <para xml:lang="zh">当前在录制的文件的格式。</para>
        /// </summary>
        public Optional<InternalFormat> RecordingFormat => workingConfig.OnSome ? workingConfig.Value.Format : Optional<InternalFormat>.Empty;

        /// <summary>
        /// <para xml:lang="en">Available formats. Vary on different platforms.</para>
        /// <para xml:lang="zh">所有可用格式，根据平台有差异。</para>
        /// </summary>
        public List<InternalFormat> AvailableFormats
        {
            get
            {
                List<InternalFormat> availableFormats = new List<InternalFormat>();
                if (VideoInputFrameRecorder.isAvailable())
                {
                    availableFormats.Add(InternalFormat.H264);
                }
                availableFormats.Add(InternalFormat.Obsolete);
                return availableFormats;
            }
        }

        internal bool IsAvailable => true;

        internal int BufferRequirement { get => (recorder?.bufferRequirement() ?? 0) + (recorderObsolete?.bufferRequirement() ?? 0); }

        /// <summary>
        /// <para xml:lang="en">Internal data format.</para>
        /// <para xml:lang="zh">内部数据格式。</para>
        /// </summary>
        public enum InternalFormat
        {
            /// <summary>
            /// <para xml:lang="en">Automatcially select available format.</para>
            /// <para xml:lang="zh">自动选择可用的格式。</para>
            /// </summary>
            Auto,
            /// <summary>
            /// <para xml:lang="en">H264. Supported on MacOS, iOS and Android.</para>
            /// <para xml:lang="zh">H264。MacOS、iOS、Android上支持。</para>
            /// </summary>
            H264,
            /// <summary>
            /// <para xml:lang="en">Obsolete format. The only support format on Windows.</para>
            /// <para xml:lang="zh">过时的格式，在Windows上只支持该格式。</para>
            /// </summary>
            Obsolete,
        }

        /// <summary>
        /// <para xml:lang="en">Recording status.</para>
        /// <para xml:lang="zh">录制状态。</para>
        /// </summary>
        public enum RecorderStatus
        {
            /// <summary>
            /// <para xml:lang="en">Unknow.</para>
            /// <para xml:lang="zh">未知。</para>
            /// </summary>
            Unknown,
            /// <summary>
            /// <para xml:lang="en">Ready for recording.</para>
            /// <para xml:lang="zh">可以录制。</para>
            /// </summary>
            Ready,
            /// <summary>
            /// <para xml:lang="en">Record starting.</para>
            /// <para xml:lang="zh">录制启动中。</para>
            /// </summary>
            Starting,
            /// <summary>
            /// <para xml:lang="en">Recording.</para>
            /// <para xml:lang="zh">录制中。</para>
            /// </summary>
            Recording,
            /// <summary>
            /// <para xml:lang="en">Recording without supplement.</para>
            /// <para xml:lang="zh">录制中（无补充数据）。</para>
            /// </summary>
            RecordingWithoutSupplement,
            /// <summary>
            /// <para xml:lang="en">Error.</para>
            /// <para xml:lang="zh">出错。</para>
            /// </summary>
            Error,
        }

        private void Awake()
        {
            enabled = AutoStart;
        }

        /// <summary>
        /// <para xml:lang="en">Start/Stop recording when <see cref="ARSession"/> is running. Recording will start only when <see cref="MonoBehaviour"/>.enabled is true after session started. <see cref="MonoBehaviour"/>.enabled is default false and will be set to <see cref="AutoStart"/> in <see cref="ARSession"/>.Awake.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止录制。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始录制。<see cref="MonoBehaviour"/>.enabled默认为false，且会在<see cref="ARSession"/>.Awake中设置为<see cref="AutoStart"/>。</para>
        /// </summary>
        private void OnEnable()
        {
            if (Status < RecorderStatus.Ready) { return; }
            StartRecord();
        }

        private void OnDisable()
        {
            StopRecord();
        }

        private void OnDestroy()
        {
            OnSessionStop();
        }

        internal void OnSessionStart(ARSession session)
        {
            arSession = session;

            recorderObsolete = InputFrameRecorder.create();
            if (VideoInputFrameRecorder.isAvailable())
            {
                recorder = VideoInputFrameRecorder.create();
                recorder.setAutomaticCompletionCallback(EasyARController.Scheduler, (Action<VideoInputFrameRecorderCompletionReason>)((reason) =>
                {
                    Debug.LogWarning($"record completed automatically: {RecordingFile}");
                    if (!this) { return; }
                    Status = RecorderStatus.Error;
                    OnRecordingUpdate(false, null);
                }));

                using (var output = recorderObsolete.output())
                using (var input = recorder.input())
                {
                    output.connect(input);
                }
            }

            Status = RecorderStatus.Ready;
            OnReady?.Invoke();
            if (enabled)
            {
                OnEnable();
            }
        }

        internal void OnSessionStop()
        {
            OnDisable();
            recorder?.Dispose();
            recorder = null;
            recorderObsolete?.Dispose();
            recorderObsolete = null;
            arSession = null;
            Status = RecorderStatus.Unknown;
        }

        internal string DumpLite()
        {
            var data = string.Empty;
            data += $"FrameRecorder: {Status}{(workingConfig.OnSome ? $" {workingConfig.Value.Format}" : string.Empty)}{(Status == RecorderStatus.Recording ? $" {Path.GetFileName(RecordingFile)}" : string.Empty)}" + Environment.NewLine;
            return data;
        }

        internal InputFrameSource Output() => recorder?.output() ?? recorderObsolete?.output() ?? null;

        internal InputFrameSink Input() => recorderObsolete?.input() ?? null;

        private void StartRecord()
        {
            var config = Configuration.Clone();
            workingConfig = config;
            if (config.Format == InternalFormat.Auto)
            {
                config.Format = VideoInputFrameRecorder.isAvailable() ? InternalFormat.H264 : InternalFormat.Obsolete;
            }

            if (config.AutoFilePath)
            {
                config.FilePath.Type = WritablePathType.PersistentDataPath;
                config.FilePath.FolderPath = string.Empty;
                config.FilePath.FileName = ARSessionFactory.DefaultName(arSession.Assembly.FrameSource.GetType()).Replace(" ", "") + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss.fff");
            }
            var path = Path.Combine(config.FilePath.FolderPath, config.FilePath.FileName + (config.Format == InternalFormat.Obsolete ? ".eif" : ".mkveif"));
            if (config.FilePath.Type == WritablePathType.PersistentDataPath)
            {
                path = Path.Combine(Application.persistentDataPath, path);
            }
            RecordingFile = path;
            if (!AvailableFormats.Contains(config.Format))
            {
                Status = RecorderStatus.Error;
                OnRecordingUpdate(false, typeof(FrameRecorder) + $" fail to start with format: {config.Format}");
                return;
            }

            Status = RecorderStatus.Starting;

            supplementFilter = arSession.Assembly.FrameFilters.Where(f => f is IFrameSupplementFilter && f.enabled).Select(f => f as IFrameSupplementFilter).FirstOrDefault();
            supplementSensor = new FrameSupplementSensor(gameObject, arSession.Diagnostics);
            supplementSensor.Start();

            if (config.Format != InternalFormat.Obsolete)
            {
                if (supplementFilter != null)
                {
                    supplementFilter.ProximityLocationProvider += OnProximityLocationUpdate;
                }
                if (supplementSensor.LocationManager)
                {
                    StartCoroutine(UpdateLocationData(supplementSensor.LocationManager));
                }
                if (supplementSensor.Accelerometer != null)
                {
                    using (var output = supplementSensor.Accelerometer.output())
                    {
                        output.connect(recorder.accelerometerResultSink());
                    }
                }
                if (supplementSensor.Attitude != null)
                {
                    using (var output = supplementSensor.Attitude.output())
                    {
                        output.connect(recorder.attitudeSensorResultSink());
                    }
                }
                if (supplementSensor.Gyroscope != null)
                {
                    using (var output = supplementSensor.Gyroscope.output())
                    {
                        output.connect(recorder.gyroscopeResultSink());
                    }
                }
                if (supplementSensor.Magnetometer != null)
                {
                    using (var output = supplementSensor.Magnetometer.output())
                    {
                        output.connect(recorder.magnetometerResultSink());
                    }
                }

                StartCoroutine(StartRecord(path));
            }
            else
            {
                var status = recorderObsolete.start(path, arSession.Assembly.Display.Rotation);
                if (status)
                {
                    try
                    {
                        supplementRecorder = new FrameSupplementRecorder(path + ".json", arSession.Assembly.FrameSource);
                        StartCoroutine(RecordFrameSupplementObsolete());
                    }
                    catch (Exception ex)
                    {
                        arSession.Diagnostics.EnqueueError(typeof(FrameRecorder) + $" fail to write file: {path}.json" + Environment.NewLine + ex);
                    }
                }
                Status = status ? (supplementRecorder != null ? RecorderStatus.Recording : RecorderStatus.RecordingWithoutSupplement) : RecorderStatus.Error;
                OnRecordingUpdate(status, typeof(FrameRecorder) + " fail to start with file: " + path);
            }
        }

        private IEnumerator StartRecord(string path)
        {
            yield return new WaitUntil(() => workingConfig.OnSome && arSession && arSession.Assembly != null && arSession.Assembly.FrameSource && !(arSession.Assembly.FrameSource is FramePlayer) && arSession.Assembly.FrameSource.CameraFrameStarted);

            if (arSession.Assembly.FrameSource.DeviceCameras == null || arSession.Assembly.FrameSource.DeviceCameras.Count < 1)
            {
                Status = RecorderStatus.Error;
                OnRecordingUpdate(false, $"{typeof(FrameRecorder)} will not start due to {arSession.Assembly.FrameSource.GetType()} implementation error, please contact the plugin developer.");
                yield break;
            }
            var deviceCamera = arSession.Assembly.FrameSource.DeviceCameras[0];
            var size = deviceCamera.FrameSize;
            var cameraDeviceType = deviceCamera.CameraType;
            var cameraOrientation = deviceCamera.CameraOrientation;
            var fps = Mathf.Max(deviceCamera.FrameRateRange.x, deviceCamera.FrameRateRange.y);
            if (size.x <= 0 || size.y <= 0 || fps <= 0)
            {
                Status = RecorderStatus.Error;
                OnRecordingUpdate(false, $"{typeof(FrameRecorder)} will not start due to {arSession.Assembly.FrameSource.GetType()} implementation error, please contact the plugin developer. size = {size}, fps = {deviceCamera.FrameRateRange}");
                yield break;
            }
            var initialScreenRotation = arSession.Assembly.Display.Rotation;
            var format = VideoInputFrameRecorderVideoCodec.H264;
            var status = false;
            var finish = false;
            var metadata = JsonUtility.ToJson(FrameSupplement.Create(arSession.Assembly.FrameSource));

            thread = new Thread(() =>
            {
                status = recorder.start(path, cameraDeviceType, cameraOrientation, initialScreenRotation, format, size.x, size.y, fps, metadata);
                finish = true;
            });
            thread.Start();
            yield return new WaitUntil(() => finish);

            if (thread != null && thread.IsAlive)
            {
                thread.Join();
                thread = null;
            }
            Status = status ? RecorderStatus.Recording : RecorderStatus.Error;
            OnRecordingUpdate(status, typeof(FrameRecorder) + " fail to start with file: " + path);
        }

        private void OnRecordingUpdate(bool started, string message)
        {
            if (started)
            {
                OnRecording?.Invoke(RecordingFile);
            }
            else
            {
                if (!string.IsNullOrEmpty(message))
                {
                    arSession.Diagnostics.EnqueueError(message);
                }
                enabled = false;
            }
        }

        private void StopRecord()
        {
            StopAllCoroutines();
            if (thread != null && thread.IsAlive)
            {
                thread.Join();
                thread = null;
            }
            recorderObsolete?.stop();
            recorder?.stop();
            if (supplementFilter != null)
            {
                supplementFilter.ProximityLocationProvider -= OnProximityLocationUpdate;
            }
            supplementFilter = null;
            supplementSensor?.Stop();
            supplementSensor?.Dispose();
            supplementSensor = null;

            supplementRecorder?.Dispose();
            supplementRecorder = null;
            RecordingFile = string.Empty;
            workingConfig = null;
            var isFinish = Status >= RecorderStatus.Recording;
            var hasError = Status == RecorderStatus.Error;
            if (Status > RecorderStatus.Ready)
            {
                Status = RecorderStatus.Ready;
            }
            if (isFinish)
            {
                OnFinish?.Invoke(!hasError);
            }
        }

        private IEnumerator UpdateLocationData(LocationManager locationManager)
        {
            while (true)
            {
                yield return null;
                if (!locationManager) { continue; }

                var loc = locationManager.CurrentResult;
                if (recorder != null && loc.HasValue)
                {
                    using (var sink = recorder.locationResultSink())
                    {
                        sink.handle(new LocationResult(loc.Value.latitude, loc.Value.longitude, loc.Value.altitude, loc.Value.horizontalAccuracy, loc.Value.verticalAccuracy, true, true, true));
                    }
                }
            }
        }

        private void OnProximityLocationUpdate(ProximityLocationResult data)
        {
            if (recorder == null) { return; }
            using (var sink = recorder.proximityLocationResultSink())
            {
                sink.handle(data);
            }
        }

        private IEnumerator RecordFrameSupplementObsolete()
        {
            int index = -1;
            while (true)
            {
                yield return null;

                if (Status != RecorderStatus.Recording) { continue; }
                if (!arSession || arSession.Assembly == null) { continue; }

                var asyncFrame = arSession.AsyncCameraFrame;
                if (asyncFrame.OnNone) { continue; }

                using (var outputFrame = asyncFrame.Value)
                using (var inputFrame = outputFrame.inputFrame())
                {
                    if (inputFrame.index() == index) { continue; }
                    index = inputFrame.index();

                    var acc = Optional<AccelerometerResult>.Empty;
                    var location = Optional<LocationResult>.Empty;

                    if (supplementSensor?.Accelerometer != null)
                    {
                        var value = supplementSensor.Accelerometer.getCurrentResult();
                        if (value.OnSome)
                        {
                            acc = value.Value;
                        }
                    }
                    if (supplementSensor?.LocationManager != null)
                    {
                        var loc = supplementSensor.LocationManager.CurrentResult;
                        if (loc.HasValue)
                        {
                            location = new LocationResult(loc.Value.latitude, loc.Value.longitude, loc.Value.altitude, loc.Value.horizontalAccuracy, loc.Value.verticalAccuracy, true, true, true);
                        }
                    }
                    supplementRecorder?.Record(inputFrame.timestamp(), acc, location);
                }
            }
        }

        private class FrameSupplementSensor : IDisposable
        {
            public LocationManager LocationManager => locationManager;
            public Accelerometer Accelerometer => accelerometer;
            public AttitudeSensor Attitude => attitude;
            public Gyroscope Gyroscope => gyroscope;
            public Magnetometer Magnetometer => magnetometer;

            private AttitudeSensor attitude;
            private Gyroscope gyroscope;
            private Magnetometer magnetometer;
            private Accelerometer accelerometer;
            private LocationManager locationManager;
            private DiagnosticsController diagnostics;

            public FrameSupplementSensor(GameObject gameObject, DiagnosticsController diagnostics)
            {
                this.diagnostics = diagnostics;
                locationManager = gameObject.GetComponent<LocationManager>();
                if (!locationManager)
                {
                    locationManager = gameObject.AddComponent<LocationManager>();
                    locationManager.enabled = false;
                }

                accelerometer = new Accelerometer();
                if (!accelerometer.isAvailable())
                {
                    accelerometer.Dispose();
                    accelerometer = null;
                }
                attitude = new AttitudeSensor();
                if (!attitude.isAvailable())
                {
                    attitude.Dispose();
                    attitude = null;
                }
                gyroscope = new Gyroscope();
                if (!gyroscope.isAvailable())
                {
                    gyroscope.Dispose();
                    gyroscope = null;
                }
                magnetometer = new Magnetometer();
                if (!magnetometer.isAvailable())
                {
                    magnetometer.Dispose();
                    magnetometer = null;
                }
            }

            ~FrameSupplementSensor()
            {
                accelerometer?.Dispose();
                attitude?.Dispose();
                gyroscope?.Dispose();
                magnetometer?.Dispose();
            }

            public void Dispose()
            {
                Destroy(locationManager);
                accelerometer?.Dispose();
                attitude?.Dispose();
                gyroscope?.Dispose();
                magnetometer?.Dispose();
                GC.SuppressFinalize(this);
            }

            public void Start()
            {
                locationManager.RequestLocationPermission((result) =>
                {
                    if (!result)
                    {
                        var error = "Location permission not granted";
                        if (diagnostics)
                        {
                            diagnostics.EnqueueError(error);
                        }
                        else
                        {
                            Debug.LogError(error);
                        }
                    }
                });
                locationManager.enabled = true;
                accelerometer?.openWithSamplingPeriod(5);
                attitude?.openWithSamplingPeriod(5);
                gyroscope?.openWithSamplingPeriod(5);
                magnetometer?.openWithSamplingPeriod(5);
            }

            public void Stop()
            {
                locationManager.enabled = false;
                accelerometer?.close();
                attitude?.close();
                gyroscope?.close();
                magnetometer?.close();
            }
        }

        /// <summary>
        /// <para xml:lang="en">Recording configuration..</para>
        /// <para xml:lang="zh">录制配置。。</para>
        /// </summary>
        [Serializable]
        public class RecordingConfiguration
        {
            /// <summary>
            /// <para xml:lang="en">Generate file path automatically. The file will be stored into <see cref="Application.persistentDataPath"/>, and you need to make sure the file can be get out in your own way.</para>
            /// <para xml:lang="zh">自动生成文件路径。文件将被存储在<see cref="Application.persistentDataPath"/>，你需要自己确保能通过一些方法获取到这里的文件。</para>
            /// </summary>
            public bool AutoFilePath = true;
            /// <summary>
            /// <para xml:lang="en">File path infomation.</para>
            /// <para xml:lang="zh">文件路径信息。</para>
            /// </summary>
            public FilePathInfo FilePath;
            /// <summary>
            /// <para xml:lang="en">Format to be recorded. <see cref="InternalFormat.H264"/> may not work on some devices.</para>
            /// <para xml:lang="zh">录制的格式。使用<see cref="InternalFormat.H264"/>可能无法在一些设备上工作。</para>
            /// </summary>
            public InternalFormat Format;

            internal RecordingConfiguration Clone()
            {
                return new RecordingConfiguration
                {
                    AutoFilePath = AutoFilePath,
                    FilePath = new FilePathInfo
                    {
                        Type = FilePath.Type,
                        FolderPath = FilePath.FolderPath,
                        FileName = FilePath.FileName,
                    },
                    Format = Format,
                };
            }

            /// <summary>
            /// <para xml:lang="en">File path infomation. The recording file path with be Path.Combine(Application.persistentDataPath, FolderPath, Name + extension) when <see cref="Type"/> is <see cref="WritablePathType.PersistentDataPath"/> or Path.Combine(FolderPath, Name + extension) when <see cref="Type"/> is <see cref="WritablePathType.Absolute"/>, where extension is determined by <see cref="Format"/>.</para>
            /// <para xml:lang="zh">文件路径信息。录制的文件路径在<see cref="Type"/>为<see cref="WritablePathType.PersistentDataPath"/>时是 Path.Combine(Application.persistentDataPath, FolderPath, Name + extension)，<see cref="Type"/>为<see cref="WritablePathType.Absolute"/>时是 Path.Combine(FolderPath, Name + extension)，其中extension由<see cref="Format"/>决定。</para>
            /// </summary>
            [Serializable]
            public class FilePathInfo
            {
                /// <summary>
                /// <para xml:lang="en">File path type.</para>
                /// <para xml:lang="zh">路径类型。</para>
                /// </summary>
                public WritablePathType Type = WritablePathType.PersistentDataPath;
                /// <summary>
                /// <para xml:lang="en">Folder path.</para>
                /// <para xml:lang="zh">文件夹路径。</para>
                /// </summary>
                public string FolderPath = string.Empty;
                /// <summary>
                /// <para xml:lang="en">File name without extension.</para>
                /// <para xml:lang="zh">文件名（不含扩展名）。</para>
                /// </summary>
                public string FileName = string.Empty;
            }
        }

        /// <summary>
        /// <para xml:lang="en">File name without extension. Callback parameter represents file path in recording.</para>
        /// <para xml:lang="zh">录制开始事件。回调参数是录制的文件名。</para>
        /// </summary>
        [Serializable]
        public class RecordStartEvent : UnityEvent<string> { }

        /// <summary>
        /// <para xml:lang="en">Recorder finish event. Callback parameter will be false when recording has error(s).</para>
        /// <para xml:lang="zh">录制结束的事件。录制出错时回调参数为false。</para>
        /// </summary>
        [Serializable]
        public class RecordFinishEvent : UnityEvent<bool> { }
    }
}
