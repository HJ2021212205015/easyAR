//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls Recorder in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="en">You have full control of what is recorded. The recorder do not record the screen or the camera output silently, the video data being recorded should be passed in continuously using <see cref="RecordFrame"/></para>
    /// <para xml:lang="en">NOTE: only available on Android GLES2/3 and multi-thread rendering off when using Non-Enterprise Sense variant.</para>
    /// <para xml:lang="zh">在场景中控制Recorder的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// <para xml:lang="zh">用户对视频录制的内容有完全控制，录屏功能不会默默地录制屏幕或是camera输出，录制的视频数据需要通过<see cref="RecordFrame"/>不断传入。</para>
    /// <para xml:lang="zh">注意: 仅在非企业版Sense中，Android GLES2/3且关闭多线程渲染时可用。</para>
    /// </summary>
    public class VideoRecorder : MonoBehaviour
    {
        /// <summary>
        /// <para xml:lang="en">Video profile. Only effective if modified before <see cref="StartRecording"/>.</para>
        /// <para xml:lang="zh">视频配置。在<see cref="StartRecording"/>前修改才有效。</para>
        /// </summary>
        public RecordProfile VideoProfile = RecordProfile.Quality_Default;
        /// <summary>
        /// <para xml:lang="en">Record profile. Used when <see cref="VideoProfile"/> == <see cref="RecordProfile.Custom"/>. Only effective if modified before <see cref="StartRecording"/>.</para>
        /// <para xml:lang="zh">自定义录屏配置。<see cref="VideoProfile"/> == <see cref="RecordProfile.Custom"/>时使用。在<see cref="StartRecording"/>前修改才有效。</para>
        /// </summary>
        public VideoProfiles CustomVideoProfile = new();
        /// <summary>
        /// <para xml:lang="en">Audio profile. Only effective if modified before <see cref="StartRecording"/>.</para>
        /// <para xml:lang="zh">音频配置。在<see cref="StartRecording"/>前修改才有效。</para>
        /// </summary>
        public AudioProfiles AudioProfile = new();
        /// <summary>
        /// <para xml:lang="en">Record video orientation. Only effective if modified before <see cref="StartRecording"/>.</para>
        /// <para xml:lang="zh">录屏视频朝向。在<see cref="StartRecording"/>前修改才有效。</para>
        /// </summary>
        public VideoOrientation Orientation = VideoOrientation.ScreenAdaptive;
        /// <summary>
        /// <para xml:lang="en">Record zoom mode. Only effective if modified before <see cref="StartRecording"/>.</para>
        /// <para xml:lang="zh">录屏缩放模式。在<see cref="StartRecording"/>前修改才有效。</para>
        /// </summary>
        public RecordZoomMode ZoomMode;
        /// <summary>
        /// <para xml:lang="en">Record output file path type. Only effective if modified before <see cref="StartRecording"/>.</para>
        /// <para xml:lang="zh">录屏文件输出路径类型。在<see cref="StartRecording"/>前修改才有效。</para>
        /// </summary>
        public WritablePathType FilePathType;
        /// <summary>
        /// <para xml:lang="en">Record output file path. Only effective if modified before <see cref="StartRecording"/>.</para>
        /// <para xml:lang="zh">录屏文件输出路径。在<see cref="StartRecording"/>前修改才有效。</para>
        /// </summary>
        public string FilePath = string.Empty;

        private Recorder recorder;
        private bool willRecord;

        /// <summary>
        /// <para xml:lang="en">Video profile.</para>
        /// <para xml:lang="zh">视频配置。</para>
        /// </summary>
        public enum RecordProfile
        {
            /// <summary>
            /// <para xml:lang="en">1080P, low quality</para>
            /// <para xml:lang="zh">1080P，低质量</para>
            /// </summary>
            Quality_1080P_Low = 0x00000001,
            /// <summary>
            /// <para xml:lang="en">1080P, middle quality</para>
            /// <para xml:lang="zh">1080P，中质量</para>
            /// </summary>
            Quality_1080P_Middle = 0x00000002,
            /// <summary>
            /// <para xml:lang="en">1080P, high quality</para>
            /// <para xml:lang="zh">1080P，高质量</para>
            /// </summary>
            Quality_1080P_High = 0x00000004,
            /// <summary>
            /// <para xml:lang="en">720P, low quality</para>
            /// <para xml:lang="zh">720P，低质量</para>
            /// </summary>
            Quality_720P_Low = 0x00000008,
            /// <summary>
            /// <para xml:lang="en">720P, middle quality</para>
            /// <para xml:lang="zh">720P，中质量</para>
            /// </summary>
            Quality_720P_Middle = 0x00000010,
            /// <summary>
            /// <para xml:lang="en">720P, high quality</para>
            /// <para xml:lang="zh">720P，高质量</para>
            /// </summary>
            Quality_720P_High = 0x00000020,
            /// <summary>
            /// <para xml:lang="en">480P, low quality</para>
            /// <para xml:lang="zh">480P，低质量</para>
            /// </summary>
            Quality_480P_Low = 0x00000040,
            /// <summary>
            /// <para xml:lang="en">480P, middle quality</para>
            /// <para xml:lang="zh">480P，中质量</para>
            /// </summary>
            Quality_480P_Middle = 0x00000080,
            /// <summary>
            /// <para xml:lang="en">480P, high quality</para>
            /// <para xml:lang="zh">480P，高质量</para>
            /// </summary>
            Quality_480P_High = 0x00000100,
            /// <summary>
            /// <para xml:lang="en">default resolution and quality, same as `Quality_720P_Middle`</para>
            /// <para xml:lang="zh">默认分辨率与质量，与 `Quality_720P_Middle` 相同</para>
            /// </summary>
            Quality_Default = 0x00000010,
            /// <summary>
            /// <para xml:lang="en">Custom quality</para>
            /// <para xml:lang="zh">自定义质量</para>
            /// </summary>
            Custom = 0x00010000,
        }

        /// <summary>
        /// <para xml:lang="en">Video zoom mode.</para>
        /// <para xml:lang="zh">视频缩放模式。</para>
        /// </summary>
        public enum RecordZoomMode
        {
            /// <summary>
            /// <para xml:lang="en">If output aspect ratio does not fit input, content will be clipped to fit output aspect ratio.</para>
            /// <para xml:lang="zh">如果输出宽高比与输入不符，内容会被剪裁到适合输出比例。</para>
            /// </summary>
            NoZoomAndClip = 0x00000000,
            /// <summary>
            /// <para xml:lang="en">If output aspect ratio does not fit input, content will not be clipped and there will be black borders in one dimension.</para>
            /// <para xml:lang="zh">如果输出宽高比与输入不符，内容将不会被剪裁，在某个维度上会有黑边。</para>
            /// </summary>
            ZoomInWithAllContent = 0x00000001,
        }

        /// <summary>
        /// <para xml:lang="en">Video size.</para>
        /// <para xml:lang="zh">视频尺寸。</para>
        /// </summary>
        public enum VideoSize
        {
            /// <summary>
            /// <para xml:lang="en">1080P</para>
            /// <para xml:lang="zh">1080P</para>
            /// </summary>
            Vid1080p = 0x00000002,
            /// <summary>
            /// <para xml:lang="en">720P</para>
            /// <para xml:lang="zh">720P</para>
            /// </summary>
            Vid720p = 0x00000010,
            /// <summary>
            /// <para xml:lang="en">480P</para>
            /// <para xml:lang="zh">480P</para>
            /// </summary>
            Vid480p = 0x00000080,
        }

        /// <summary>
        /// <para xml:lang="en">Record video orientation.</para>
        /// <para xml:lang="zh">录屏视频朝向。</para>
        /// </summary>
        public enum VideoOrientation
        {
            /// <summary>
            /// <para xml:lang="en">Video recorded is landscape.</para>
            /// <para xml:lang="zh">录制的视频是横向。</para>
            /// </summary>
            Landscape,
            /// <summary>
            /// <para xml:lang="en">Video recorded is portrait.</para>
            /// <para xml:lang="zh">录制的视频是竖向。</para>
            /// </summary>
            Portrait,
            /// <summary>
            /// <para xml:lang="en">Video orientation fit screen aspect ratio automatically at start.</para>
            /// <para xml:lang="zh">录制的视频朝向根据启动时屏幕比例自动调整。</para>
            /// </summary>
            ScreenAdaptive,
        }

        /// <summary>
        /// <para xml:lang="en">If the video recorder is available. Only true on Android GLES2/3 and multi-thread rendering off when using Non-Enterprise Sense variant.</para>
        /// <para xml:lang="zh">录屏功能是否可用（ 仅在非企业版Sense中，Android GLES2/3且关闭多线程渲染时可用）。</para>
        /// </summary>
        public static bool IsAvailable => CheckAvailability().Item1;
        /// <summary>
        /// <para xml:lang="en">Reasion when not available.</para>
        /// <para xml:lang="zh">不可用时的原因。</para>
        /// </summary>
        public static string NotAvailableReason => CheckAvailability().Item2;

        private void OnDestroy()
        {
            StopRecording();
        }

        /// <summary>
        /// <para xml:lang="en">Start recording. The video data being recorded should be passed in continuously using <see cref="RecordFrame"/>。</para>
        /// <para xml:lang="zh">开始录屏。录制的视频数据需要通过<see cref="RecordFrame"/>不断传入。</para>
        /// </summary>
        public void StartRecording(Action<bool, PermissionStatus, string> onStart, Action<string> onRecordError)
        {
            var available = CheckAvailability();
            if (!available.Item1)
            {
                onStart?.Invoke(false, PermissionStatus.Error, $"{GetType()} not available: {available.Item2}");
                return;
            }
            if (!EasyARController.IsReady)
            {
                onStart?.Invoke(false, PermissionStatus.Error, "EasyAR Sense not ready");
                return;
            }
            willRecord = true;
            Recorder.requestPermissions(EasyARController.Scheduler, (Action<PermissionStatus, string>)((status, msg) =>
            {
                if (!willRecord)
                {
                    onStart?.Invoke(false, status, "stopped");
                    return;
                }
                if (status != PermissionStatus.Granted)
                {
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, $"Recorder permission {status}, {msg}");
                    onStart?.Invoke(false, status, msg);
                    return;
                }

                StopRecording();
                try
                {
                    using var configuration = new RecorderConfiguration();
                    configuration.setOutputFile(FilePathType == WritablePathType.PersistentDataPath ? Application.persistentDataPath + "/" + FilePath : FilePath);
                    if (VideoProfile != RecordProfile.Custom)
                    {
                        configuration.setProfile((easyar.RecordProfile)VideoProfile);
                    }
                    else
                    {
                        configuration.setVideoSize((RecordVideoSize)CustomVideoProfile.Size);
                        configuration.setVideoBitrate(CustomVideoProfile.Bitrate);
                    }
                    configuration.setZoomMode((easyar.RecordZoomMode)ZoomMode);

                    configuration.setAudioBitrate(AudioProfile.Bitrate);
                    configuration.setAudioSampleRate(AudioProfile.SampleRate);
                    configuration.setChannelCount(AudioProfile.ChannelCount);

                    configuration.setVideoOrientation(Orientation switch
                    {
                        VideoOrientation.Portrait => RecordVideoOrientation.Portrait,
                        VideoOrientation.Landscape => RecordVideoOrientation.Landscape,
                        _ => Screen.width > Screen.height ? RecordVideoOrientation.Landscape : RecordVideoOrientation.Portrait,
                    });
                    recorder = Recorder.create(configuration, EasyARController.Scheduler, (Action<RecordStatus, string>)((rstatus, rmsg) =>
                    {
                        Debug.Log($"Recorder callback: {rstatus}, {rmsg})");
                        if (rstatus == RecordStatus.OnStarted)
                        {
                            onStart?.Invoke(true, status, msg);
                        }
                        else if (rstatus == RecordStatus.FailedToStart)
                        {
                            onStart?.Invoke(false, status, rmsg);
                        }
                        else if (rstatus == RecordStatus.FileFailed)
                        {
                            onRecordError?.Invoke($"{rstatus}: {rmsg}");
                        }
                        else if (rstatus == RecordStatus.LogError)
                        {
                            onRecordError?.Invoke(rmsg);
                        }
                    }));
                }
                catch (ArgumentNullException)
                {
                    var error = $"Fail to create {nameof(Recorder)}, check logs for detials.";
                    DiagnosticsController.TryShowDiagnosticsError(gameObject, error);
                    onStart?.Invoke(false, status, error);
                    return;
                }
                recorder.start();
            }));
        }

        /// <summary>
        /// <para xml:lang="en">Stop recording.</para>
        /// <para xml:lang="zh">停止录屏。</para>
        /// </summary>
        public bool StopRecording()
        {
            var success = false;
            willRecord = false;
            if (recorder != null)
            {
                success = recorder.stop();
                recorder.Dispose();
                recorder = null;
            }
            return success;
        }

        /// <summary>
        /// <para xml:lang="en">Record a frame using <paramref name="texture"/>.</para>
        /// <para xml:lang="zh">使用<paramref name="texture"/>录制一帧数据。</para>
        /// </summary>
        public void RecordFrame(RenderTexture texture)
        {
            if (recorder == null) { return; }
            using (var textureId = TextureId.fromInt(texture.GetNativeTexturePtr().ToInt32()))
            {
                recorder.updateFrame(textureId, texture.width, texture.height);
            }
        }

        private static (bool, string) CheckAvailability()
        {
            string error = string.Empty;
            if (Application.platform != RuntimePlatform.Android)
            {
                return (false, $"Platform: {Application.platform}");
            }
            else if (!SystemUtil.IsGLES())
            {
                return (false, $"Graphics Device Type: {SystemInfo.graphicsDeviceType}");
            }
            else if (SystemInfo.graphicsMultiThreaded)
            {
                return (false, $"Graphics Multi Threaded");
            }
            else if (!Recorder.isAvailable())
            {
                return (false, $"EasyAR Sense Recorder not available in Variant: {Engine.variant()}");
            }
            return (true, string.Empty);
        }

        /// <summary>
        /// <para xml:lang="en">Video profile.</para>
        /// <para xml:lang="zh">视频配置。</para>
        /// </summary>
        [Serializable]
        public class VideoProfiles
        {
            /// <summary>
            /// <para xml:lang="en">Size.</para>
            /// <para xml:lang="zh">尺寸。</para>
            /// </summary>
            public VideoSize Size = VideoSize.Vid720p;
            /// <summary>
            /// <para xml:lang="en">Bitrate.</para>
            /// <para xml:lang="zh">比特率。</para>
            /// </summary>
            public int Bitrate = 2500000;
        }

        /// <summary>
        /// <para xml:lang="en">Audio profile.</para>
        /// <para xml:lang="zh">音频配置。</para>
        /// </summary>
        [Serializable]
        public class AudioProfiles
        {
            /// <summary>
            /// <para xml:lang="en">Bitrate.</para>
            /// <para xml:lang="zh">比特率。</para>
            /// </summary>

            public int Bitrate = 96000;
            /// <summary>
            /// <para xml:lang="en">Sample rate.</para>
            /// <para xml:lang="zh">采样率。</para>
            /// </summary>
            public int SampleRate = 44100;
            /// <summary>
            /// <para xml:lang="en">Channel count.</para>
            /// <para xml:lang="zh">通道数。</para>
            /// </summary>
            public int ChannelCount = 1;
        }
    }
}
