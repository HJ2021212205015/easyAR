//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="InputFramePlayer"/> and <see cref="VideoInputFramePlayer"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">It will be used when <see cref="AssembleOptions.FrameSource"/> is <see cref="AssembleOptions.FrameSourceSelection.FramePlayer"/> or when 'Frame Player' is selected in the 'Session Validation Tool' from <see cref="DiagnosticsController"/> inspector (editor only).</para>
    /// <para xml:lang="zh">在场景中控制<see cref="InputFramePlayer"/>和<see cref="VideoInputFramePlayer"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">它将会在<see cref="AssembleOptions.FrameSource"/>是<see cref="AssembleOptions.FrameSourceSelection.FramePlayer"/>或者编辑器上在<see cref="DiagnosticsController"/> inspector的'Session Validation Tool'中选择了'Frame Player'<see cref="DiagnosticsController"/>时被使用。</para>
    /// </summary>
    public class FramePlayer : FrameSource, FrameSource.ISenseBuiltinFrameSource, FrameSource.IMotionTrackingDevice
    {
        /// <summary>
        /// <para xml:lang="en">File path type. Set before <see cref="Play"/>.</para>
        /// <para xml:lang="zh">路径类型。可以在<see cref="Play"/>之前设置。</para>
        /// </summary>
        public WritablePathType FilePathType;

        /// <summary>
        /// <para xml:lang="en">File path. Set before <see cref="Play"/>.</para>
        /// <para xml:lang="zh">文件路径。可以在<see cref="Play"/>之前设置。</para>
        /// </summary>
        public string FilePath = string.Empty;

        private static IReadOnlyList<ARSession.ARCenterMode> noSpatialCenterModes = new List<ARSession.ARCenterMode> { ARSession.ARCenterMode.FirstTarget, ARSession.ARCenterMode.Camera, ARSession.ARCenterMode.SpecificTarget };

        private VideoInputFramePlayer player;
        private InputFramePlayer playerObsolete;

        private FrameSupplementPlayer supplementPlayer;
        private ARSession arSession;
        private DisplayEmulator display = new DisplayEmulator();
        private Optional<bool> delayedOpen;
        [SerializeField, HideInInspector]
        private Camera cameraCandidate;
        private bool spatialDisabled;
        private Optional<bool> isSupplementRequired;
        private readonly byte[] eif1Head = { 0x45, 0x49, 0x46, 0x31 };
        private readonly byte[] eif2Head = { 0x1A, 0x45, 0xDF, 0xA3 };
        private readonly byte[] eif1CHead = { 0x45, 0x49, 0x46, 0x43 };

        /// <summary>
        /// <para xml:lang="en"> Whether the playback is started.</para>
        /// <para xml:lang="zh"> 是否已启动播放。</para>
        /// </summary>
        public bool IsStarted => player != null || playerObsolete != null;
        /// <summary>
        /// <para xml:lang="en"> Whether the playback is completed.</para>
        /// <para xml:lang="zh"> 是否已完成播放。</para>
        /// </summary>
        public bool IsCompleted => player?.isCompleted() ?? playerObsolete?.isCompleted() ?? default;

        /// <summary>
        /// <para xml:lang="en"> Total expected playback time. The unit is second.</para>
        /// <para xml:lang="zh"> 预期的总播放时间。单位为秒。</para>
        /// </summary>
        public Optional<double> Length => player?.totalTime() ?? playerObsolete?.totalTime() ?? default;

        /// <summary>
        /// <para xml:lang="en"> Current time played.</para>
        /// <para xml:lang="zh"> 已经播放的时间。</para>
        /// </summary>
        public double Time => player?.currentTime() ?? playerObsolete?.currentTime() ?? default;

        /// <summary>
        /// <para xml:lang="en">Whether the current playback time point can be relocated. If recording halts improperly, index data to set playback time point may be missing.</para>
        /// <para xml:lang="zh">是否可定位当前播放时刻。录制过程非正常中断时，可能导致缺少索引数据，而无法设定当前播放时间。</para>
        /// </summary>
        public bool IsSeekable => player?.isSeekable() ?? default;

        /// <summary>
        /// <para xml:lang="en">Whether the playback speed can be changed.</para>
        /// <para xml:lang="zh">是否可修改播放速度。</para>
        /// </summary>
        public bool IsSpeedChangeable => player != null;

        /// <summary>
        /// <para xml:lang="en">Current playback speed.</para>
        /// <para xml:lang="zh">当前的播放速度。</para>
        /// </summary>
        public double Speed
        {
            get => player?.speed() ?? 1;
            set => player?.setSpeed(value);
        }

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
        internal protected override IDisplay Display => display;

        internal protected override Optional<bool> IsAvailable => true;
        internal protected override IReadOnlyList<ARSession.ARCenterMode> AvailableCenterMode => spatialDisabled ? noSpatialCenterModes : AllCenterModes;
        internal protected override bool CameraFrameStarted => IsStarted;
        internal protected override List<FrameSourceCamera> DeviceCameras => new List<FrameSourceCamera>();

        private bool useObsoleteSupplementPlayer => isSupplementRequired == true && playerObsolete != null;

        private void Awake()
        {
            supplementPlayer = new FrameSupplementPlayer();
        }

        /// <summary>
        /// <para xml:lang="en">Play/Pause eif file playback when <see cref="ARSession"/> is running. Playback will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时播放/暂停eif文件。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始播放。</para>
        /// </summary>
        private void OnEnable()
        {
            if (delayedOpen == true) { Play(); }
            if (player != null || playerObsolete != null)
            {
                if (useObsoleteSupplementPlayer) { supplementPlayer.Resume(); }
                player?.resume();
                playerObsolete?.resume();
            }
        }

        private void OnDisable()
        {
            if (player != null || playerObsolete != null)
            {
                if (useObsoleteSupplementPlayer) { supplementPlayer.Pause(); }
                player?.pause();
                playerObsolete?.pause();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (isSupplementRequired == true) { supplementPlayer.OnApplicationPause(pause); }
        }

        private void OnDestroy()
        {
            OnSessionStop();
            supplementPlayer?.Dispose();
        }

        /// <summary>
        /// <para xml:lang="en">Start eif file playback. If neither <see cref="Play"/> nor <see cref="Stop"/> is called manually, <see cref="Play"/> will be automatically invoked upon <see cref="ARSession"/> startup.</para>
        /// <para xml:lang="en">Can only be used after session start.</para>
        /// <para xml:lang="zh">播放eif文件。如果未手动调用<see cref="Play"/>和<see cref="Stop"/>，<see cref="ARSession"/>启动后会自动<see cref="Play"/>。</para>
        /// <para xml:lang="zh">在session启动后才能使用。</para>
        /// </summary>
        public bool Play()
        {
            if (!arSession)
            {
                Debug.LogError("session not started");
                return false;
            }
            Stop();

            var diagnostics = DiagnosticsController.TryGetDiagnosticsController(gameObject);
            var path = FilePath;
            if (FilePathType == WritablePathType.PersistentDataPath)
            {
                path = Application.persistentDataPath + "/" + path;
            }
            if (!File.Exists(path))
            {
                diagnostics.EnqueueError($"File not found: {path}");
                return false;
            }

            var head = GetHead(path);
            var playerType = Optional<Type>.Empty;
            if (head.SequenceEqual(eif1Head))
            {
                playerType = typeof(InputFramePlayer);
            }
            else if (head.SequenceEqual(eif2Head))
            {
                playerType = typeof(VideoInputFramePlayer);
            }
            else if (head.SequenceEqual(eif1CHead))
            {
                diagnostics.EnqueueError($"File encrypted, use it for feedback only: {path}");
                return false;
            }
            else
            {
                diagnostics.EnqueueError($"File format not supported: {path}");
                return false;
            }

            var isStarted = false;
            IFrameSupplementFilter supplementFilter = null;
            if (arSession && arSession.Assembly != null)
            {
                supplementFilter = arSession.Assembly.FrameFilters.Where(f => f is IFrameSupplementFilter).Select(f => f as IFrameSupplementFilter).FirstOrDefault();
            }
            if (playerType == typeof(VideoInputFramePlayer))
            {
                if (!VideoInputFramePlayer.isAvailable())
                {
                    diagnostics.EnqueueError($"{nameof(VideoInputFramePlayer)} not available: {path}");
                    return false;
                }

                player = VideoInputFramePlayer.create();
                isStarted = player.start(path);
                if (!isStarted)
                {
                    player?.Dispose();
                    player = null;
                }
                if (!isStarted)
                {
                    diagnostics.EnqueueError($"{nameof(FramePlayer)} fail to start with file: {path}");
                    return false;
                }
                var meta = player.metadata();
                var supplement = JsonUtility.FromJson<FrameSupplement>(meta);
                if (supplement == null || supplement.device == null || supplement.application == null)
                {
                    diagnostics.EnqueueError($"invalid metadata: {path}\n{meta}");
                    return false;
                }
                supplementFilter?.Connect(player, supplement.device.isHMD);
            }
            else if (playerType == typeof(InputFramePlayer))
            {
                playerObsolete = InputFramePlayer.create();
                isStarted = playerObsolete.start(path);
                if (!isStarted)
                {
                    playerObsolete?.Dispose();
                    playerObsolete = null;
                }
                if (!isStarted)
                {
                    diagnostics.EnqueueError($"{nameof(FramePlayer)} fail to start with file: {path}");
                    return false;
                }

                if (useObsoleteSupplementPlayer)
                {
                    var metaPath = path + ".json";
                    try
                    {
                        supplementPlayer.Start(metaPath, () => { return Time; });
                    }
                    catch (Exception ex)
                    {
                        playerObsolete?.Dispose();
                        playerObsolete = null;
                        diagnostics.EnqueueError($"File not valid: {metaPath}" + Environment.NewLine + ex);
                        return false;
                    }
                    supplementFilter?.Connect(supplementPlayer, supplementPlayer.IsHMD);
                }
            }

            display.EmulateRotation(player?.initalScreenRotation() ?? playerObsolete?.initalScreenRotation() ?? default);
            if (player != null)
            {
                using (var source = player.output())
                {
                    ((ISenseBuiltinFrameSource)this).ConnectFrom(source);
                }
            }
            else if (playerObsolete != null)
            {
                using (var source = playerObsolete.output())
                {
                    ((ISenseBuiltinFrameSource)this).ConnectFrom(source);
                }
            }
            enabled = true;
            OnEnable();
            return true;
        }

        /// <summary>
        /// <para xml:lang="en">Stop eif file playback.</para>
        /// <para xml:lang="zh">停止播放eif文件。</para>
        /// </summary>
        public void Stop()
        {
            spatialDisabled = false;
            delayedOpen = false;
            display.EmulateRotation(0);
            OnDisable();

            IFrameSupplementFilter supplementFilter = null;
            if (arSession && arSession.Assembly != null)
            {
                supplementFilter = arSession.Assembly.FrameFilters.Where(f => f is IFrameSupplementFilter).Select(f => f as IFrameSupplementFilter).FirstOrDefault();
            }
            if (useObsoleteSupplementPlayer)
            {
                supplementPlayer.Stop();
                supplementFilter?.Disconnect(supplementPlayer);
            }
            if (player != null)
            {
                using (var souce = player.accelerometerResultSource())
                {
                    souce.disconnect();
                }
                using (var souce = player.locationResultSource())
                {
                    souce.disconnect();
                }
                using (var souce = player.proximityLocationResultSource())
                {
                    souce.disconnect();
                }
                supplementFilter?.Disconnect(player);
            }

            player?.stop();
            player?.Dispose();
            player = null;
            playerObsolete?.stop();
            playerObsolete?.Dispose();
            playerObsolete = null;
        }

        /// <summary>
        /// <para xml:lang="en">Sets current playback time point. The unit is second. If index data is missing, it returns false.</para>
        /// <para xml:lang="zh">设定当前播放时刻。单位为秒。如果缺少索引数据，则返回false。</para>
        /// </summary>
        public bool Seek(double time) => player?.seek(time) ?? default;

        internal protected override void OnSessionStart(ARSession session)
        {
            arSession = session;
            isSupplementRequired = session.Assembly.FrameFilters.Where(f => f is IFrameSupplementFilter).Any();
            if (delayedOpen.OnNone) { delayedOpen = true; }
            if (enabled) { OnEnable(); }
        }

        internal protected override void OnSessionStop()
        {
            Stop();
            arSession = null;
            delayedOpen = Optional<bool>.Empty;
            isSupplementRequired = Optional<bool>.Empty;
        }

        internal override void Connect(InputFrameSink val)
        {
            base.Connect(val);
            if (player != null)
            {
                using (var output = player.output())
                {
                    output.connect(val);
                }
            }
            else if (playerObsolete != null)
            {
                using (var output = playerObsolete.output())
                {
                    output.connect(val);
                }
            }
        }

        internal override void Disconnect()
        {
            base.Disconnect();
            if (player != null)
            {
                using (var output = player.output())
                {
                    output.disconnect();
                }
            }
            else if (playerObsolete != null)
            {
                using (var output = playerObsolete.output())
                {
                    output.disconnect();
                }
            }
        }

        internal bool UpdateInputFrameSpatial(bool hasSpatial)
        {
            if (spatialDisabled != !hasSpatial)
            {
                spatialDisabled = !hasSpatial;
                return true;
            }
            return false;
        }

        private byte[] GetHead(string path)
        {
            byte[] head = new byte[4];
            try
            {
                using (var stream = File.OpenRead(path))
                {
                    for (var i = 0; i < head.Length; ++i)
                    {
                        var data = stream.ReadByte();
                        if (data == -1) { throw new EndOfStreamException(); }
                        head[i] = checked((byte)(data));
                    }
                }
            }
            catch (Exception) { }
            return head;
        }
    }
}
