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
    /// <para xml:lang="en">Options to assemble ARSession.</para>
    /// <para xml:lang="zh">ARSession的组装选项。</para>
    /// </summary>
    [Serializable]
    public class AssembleOptions
    {
        /// <summary>
        /// <para xml:lang="en"><see cref="easyar.FrameSource"/> selection strategy.</para>
        /// <para xml:lang="zh"><see cref="easyar.FrameSource"/>的选择策略。</para>
        /// </summary>
        public FrameSourceSelection FrameSource;
        /// <summary>
        /// <para xml:lang="en"><see cref="easyar.FrameFilter"/> selection strategy.</para>
        /// <para xml:lang="zh"><see cref="easyar.FrameFilter"/>的选择策略。</para>
        /// </summary>
        public FrameFilterSelection FrameFilter;
        /// <summary>
        /// <para xml:lang="en">Frame source to select when <see cref="FrameSource"/> is <see cref="FrameSourceSelection.Manual"/>.</para>
        /// <para xml:lang="zh"><see cref="FrameSource"/>为<see cref="FrameSourceSelection.Manual"/>选择的frame source。</para>
        /// </summary>
        public FrameSource SpecifiedFrameSource;
        /// <summary>
        /// <para xml:lang="en">Frame filters to select when <see cref="FrameFilter"/> is <see cref="FrameFilterSelection.Manual"/>.</para>
        /// <para xml:lang="zh"><see cref="FrameFilter"/>为<see cref="FrameFilterSelection.Manual"/>选择的frame filters。</para>
        /// </summary>
        public List<FrameFilter> SpecifiedFrameFilters = new List<FrameFilter>();
        /// <summary>
        /// <para xml:lang="en">Enable custom camera.</para>
        /// <para xml:lang="en">All user defined frame sources are treated as custom camera. AR Engine, ARFoundation and all HMD support are also implemented as custom camera.</para>
        /// <para xml:lang="en">If you were using personal edition license or trial Mega services, you can use custom camera 100 seconds per run.</para>
        /// <para xml:lang="zh">启用自定义相机。</para>
        /// <para xml:lang="zh">所有用户定义的frame source都是自定义相机。AR Engine、ARFoundation及各种头显的支持也是通过自定义相机实现的。</para>
        /// <para xml:lang="zh">使用定义相机时，个人版或使用试用Mega云服务每次启动只能使用100秒。</para>
        /// </summary>
        public bool EnableCustomCamera = true;

        /// <summary>
        /// <para xml:lang="en">Device list download options.</para>
        /// <para xml:lang="zh">设备列表下载选项。</para>
        /// </summary>
        public DeviceListDownload DeviceList = new DeviceListDownload();

        internal AssembleOptions Clone()
        {
            return new AssembleOptions
            {
                FrameSource = FrameSource,
                FrameFilter = FrameFilter,
                SpecifiedFrameSource = SpecifiedFrameSource,
                SpecifiedFrameFilters = SpecifiedFrameFilters.ToList(),
                EnableCustomCamera = EnableCustomCamera,
                DeviceList = new DeviceListDownload
                {
                    Timeout = DeviceList.Timeout,
                    WaitTime = DeviceList.WaitTime,
                    IgnoreCache = DeviceList.IgnoreCache,
                },
            };
        }

        /// <summary>
        /// <para xml:lang="en">Selection strategy to pick <see cref="easyar.FrameSource"/>.</para>
        /// <para xml:lang="zh">挑选<see cref="easyar.FrameSource"/>的选择策略。</para>
        /// </summary>
        public enum FrameSourceSelection
        {
            /// <summary>
            /// <para xml:lang="en">Auto selections. Select first available and active child.</para>
            /// <para xml:lang="zh">自动选择，选择第一个可用且active的子节点。</para>
            /// </summary>
            Auto,
            /// <summary>
            /// <para xml:lang="en">Manually specified.</para>
            /// <para xml:lang="zh">手动指定。</para>
            /// </summary>
            Manual,
            /// <summary>
            /// <para xml:lang="en">Select frame palyer.</para>
            /// <para xml:lang="zh">选择Frame player。</para>
            /// </summary>
            FramePlayer,
        }

        /// <summary>
        /// <para xml:lang="en">Selection strategy to pick <see cref="easyar.FrameFilter"/>.</para>
        /// <para xml:lang="zh">挑选<see cref="easyar.FrameFilter"/>的选择策略。</para>
        /// </summary>
        public enum FrameFilterSelection
        {
            /// <summary>
            /// <para xml:lang="en">Auto selections. Select all active children.</para>
            /// <para xml:lang="zh">自动选择。选择所有active的子节点。</para>
            /// </summary>
            Auto,
            /// <summary>
            /// <para xml:lang="en">Auto selections. Select all active and available children.</para>
            /// <para xml:lang="zh">自动选择。选择所有active且可用的子节点。</para>
            /// </summary>
            AutoAvailable,
            /// <summary>
            /// <para xml:lang="en">Manually specified.</para>
            /// <para xml:lang="zh">手动指定。</para>
            /// </summary>
            Manual,
            /// <summary>
            /// <para xml:lang="en">Do not select.</para>
            /// <para xml:lang="zh">不选择。</para>
            /// </summary>
            None,
        }

        /// <summary>
        /// <para xml:lang="en">Device list download options.</para>
        /// <para xml:lang="zh">设备列表下载选项。</para>
        /// </summary>
        [Serializable]
        public class DeviceListDownload
        {
            /// <summary>
            /// <para xml:lang="en">Request tiemout (seconds).</para>
            /// <para xml:lang="zh">请求超时时间（s）。</para>
            /// </summary>
            public float Timeout = 10;
            /// <summary>
            /// <para xml:lang="en">Wait time (seconds). The process will continue after wait time even if the download was not ended. An event will be triggered after the download finish in this case.</para>
            /// <para xml:lang="zh">等待时间（s）。超过等待时间即使下载未完成也会继续，这时下载完成后会通过事件更新。</para>
            /// </summary>
            public float WaitTime = 1;
            /// <summary>
            /// <para xml:lang="en">Ignore cache. Download even if the process has downloaded recently.</para>
            /// <para xml:lang="zh">忽略缓存，无论当前进程近期是否下载过都强制下载。</para>
            /// </summary>
            public bool IgnoreCache;
        }
    }

    /// <summary>
    /// <para xml:lang="en">Session Report. Used for component availablity, broken reasons and other query.</para>
    /// <para xml:lang="zh">Session 报告。用于查询组件可用性或session损耗原因等。</para>
    /// </summary>
    public class SessionReport
    {
        /// <summary>
        /// <para xml:lang="en">Session broken reason.</para>
        /// <para xml:lang="zh">Session 损坏原因。</para>
        /// </summary>
        public enum SessionBrokenReason
        {
            /// <summary>
            /// <para xml:lang="en">Report in assembling, EasyAR Sense fail to initialize.</para>
            /// <para xml:lang="zh">组装过程，EasyAR Sense 未成功初始化。</para>
            /// </summary>
            Uninitialized,
            /// <summary>
            /// <para xml:lang="en">Report in assembling, EasyAR Sense license validation fail or not apply to current use.</para>
            /// <para xml:lang="zh">组装过程，EasyAR Sense license验证失败或不适用于当前使用。</para>
            /// </summary>
            LicenseInvalid,
            /// <summary>
            /// <para xml:lang="en">Report in assembling, session object incomplete.</para>
            /// <para xml:lang="zh">组装过程，Session物体不完整。</para>
            /// </summary>
            SessionObjectIncomplete,
            /// <summary>
            /// <para xml:lang="en">Report in assembling, no availabile frame source.</para>
            /// <para xml:lang="zh">组装过程，无可用的frame source。</para>
            /// </summary>
            NoAvailabileFrameSource,
            /// <summary>
            /// <para xml:lang="en">Report in assembling, frame source incomplete.</para>
            /// <para xml:lang="zh">组装过程，Frame source不完整。</para>
            /// </summary>
            FrameSourceIncomplete,
            /// <summary>
            /// <para xml:lang="en">Report in assembling, frame filter not availabile.</para>
            /// <para xml:lang="zh">组装过程，存在不可用的frame filter。</para>
            /// </summary>
            FrameFilterNotAvailabile,
            /// <summary>
            /// <para xml:lang="en">Start failed.</para>
            /// <para xml:lang="zh">启动失败。</para>
            /// </summary>
            StartFailed,
            /// <summary>
            /// <para xml:lang="en">Start failed.</para>
            /// <para xml:lang="zh">运行失败。</para>
            /// </summary>
            RunningFailed,
        }

        /// <summary>
        /// <para xml:lang="en">Session broken reason.</para>
        /// <para xml:lang="zh">Session 损坏原因。</para>
        /// </summary>
        public Optional<SessionBrokenReason> BrokenReason { get; private set; }
        /// <summary>
        /// <para xml:lang="en">Session broken exception.</para>
        /// <para xml:lang="zh">Session 损坏具体异常。</para>
        /// </summary>
        public SessionBrokenException Exception { get; private set; }
        /// <summary>
        /// <para xml:lang="en">Availability report.</para>
        /// <para xml:lang="zh">可用性报告。</para>
        /// </summary>
        public AvailabilityReport Availability { get; private set; }

        internal SessionReport() { }

        internal SessionReport(Optional<SessionBrokenReason> brokenReason, SessionBrokenException exception, AvailabilityReport availability)
        {
            BrokenReason = brokenReason;
            Exception = exception;
            Availability = availability;
        }

        internal void OnSessionBroken(SessionBrokenReason reason, SessionBrokenException exception)
        {
            BrokenReason = reason;
            Exception = exception;
        }

        internal void OnSessionRecover()
        {
            BrokenReason = Optional<SessionBrokenReason>.Empty;
            Exception = null;
        }

        /// <summary>
        /// <para xml:lang="en">Session broken exception.</para>
        /// <para xml:lang="zh">Session 损坏具体异常。</para>
        /// </summary>
        public class SessionBrokenException : Exception
        {
            private System.Diagnostics.StackTrace stackTrace;
            private SessionBrokenReason reason;
            public override string StackTrace => stackTrace?.ToString() ?? base.StackTrace;
            internal SessionBrokenReason Reason => reason;

            internal SessionBrokenException(SessionBrokenReason reason, string message, System.Diagnostics.StackTrace stackTrace) : base(message)
            {
                this.stackTrace = stackTrace;
                this.reason = reason;
            }
            internal SessionBrokenException(SessionBrokenReason reason, string message, System.Diagnostics.StackTrace stackTrace, Exception innerException) : base(message, innerException)
            {
                this.stackTrace = stackTrace;
                this.reason = reason;
            }
            public override string ToString()
            {
                if (InnerException != null) { return $"Session Broken: {reason} with inner exception:" + Environment.NewLine + InnerException.ToString(); }
                return $"Session Broken: {reason}:" + Environment.NewLine + Message;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Availability report.</para>
        /// <para xml:lang="zh">可用性报告。</para>
        /// </summary>
        [Serializable]
        public class AvailabilityReport
        {
            /// <summary>
            /// <para xml:lang="en">Frame filter checked in assembling.</para>
            /// <para xml:lang="zh">组装过程检查过的Frame filter。</para>
            /// </summary>
            public List<Item> FrameFilters = new List<Item>();
            /// <summary>
            /// <para xml:lang="en">Frame source checked in assembling.</para>
            /// <para xml:lang="zh">组装过程检查过的Frame source。</para>
            /// </summary>
            public List<Item> FrameSources = new List<Item>();
            /// <summary>
            /// <para xml:lang="en">Pending device list download tasks.</para>
            /// <para xml:lang="zh">未完成的设备列表下载任务。</para>
            /// </summary>
            public List<DeviceListDownloadType> PendingDeviceList = new List<DeviceListDownloadType>();
            /// <summary>
            /// <para xml:lang="en">Device list download results.</para>
            /// <para xml:lang="zh">设备列表下载结果。</para>
            /// </summary>
            public List<DeviceListDownloadResult> DeviceList = new List<DeviceListDownloadResult>();

            internal AvailabilityReport Clone()
            {
                return new AvailabilityReport
                {
                    FrameFilters = FrameFilters.ToList(),
                    FrameSources = FrameSources.ToList(),
                    PendingDeviceList = PendingDeviceList.ToList(),
                    DeviceList = DeviceList.ToList(),
                };
            }

            /// <summary>
            /// <para xml:lang="en">Availability item.</para>
            /// <para xml:lang="zh">可用性项目。</para>
            /// </summary>
            [Serializable]
            public class Item
            {
                /// <summary>
                /// <para xml:lang="en">Component.</para>
                /// <para xml:lang="zh">组件。</para>
                /// </summary>
                public MonoBehaviour Component;
                /// <summary>
                /// <para xml:lang="en">Availability.</para>
                /// <para xml:lang="zh">可用性。</para>
                /// </summary>
                public AvailabilityStatus Availability;
            }

            /// <summary>
            /// <para xml:lang="en">Device list download result.</para>
            /// <para xml:lang="zh">设备列表下载结果。</para>
            /// </summary>
            [Serializable]
            public class DeviceListDownloadResult
            {
                /// <summary>
                /// <para xml:lang="en">component type.</para>
                /// <para xml:lang="zh">组件类型。</para>
                /// </summary>
                public DeviceListDownloadType Type;
                /// <summary>
                /// <para xml:lang="en">Download status.</para>
                /// <para xml:lang="zh">下载状态。</para>
                /// </summary>
                public DeviceListDownloadStatus Status;
                /// <summary>
                /// <para xml:lang="en">Error messages.</para>
                /// <para xml:lang="zh">错误信息。</para>
                /// </summary>
                public string Error;
            }

            /// <summary>
            /// <para xml:lang="en">Availability status.</para>
            /// <para xml:lang="zh">可用性状态。</para>
            /// </summary>
            public enum AvailabilityStatus
            {
                /// <summary>
                /// <para xml:lang="en">Available.</para>
                /// <para xml:lang="zh">可用。</para>
                /// </summary>
                Available,
                /// <summary>
                /// <para xml:lang="en">Unavailable.</para>
                /// <para xml:lang="zh">不可用。</para>
                /// </summary>
                Unavailable,
                /// <summary>
                /// <para xml:lang="en">Disabled manually or when certain conditions were met.</para>
                /// <para xml:lang="zh">人为或满足某些条件被禁用。</para>
                /// </summary>
                Disabled,
                /// <summary>
                /// <para xml:lang="en">Not selected for availability checking.</para>
                /// <para xml:lang="zh">未选择进行检查。</para>
                /// </summary>
                NotSelected,
            }

            /// <summary>
            /// <para xml:lang="en">Download list component type.</para>
            /// <para xml:lang="zh">下载列表的组件类型。</para>
            /// </summary>
            public enum DeviceListDownloadType
            {
                /// <summary>
                /// <para xml:lang="en">MotionTracker.</para>
                /// <para xml:lang="zh">MotionTracker。</para>
                /// </summary>
                MotionTracker,
                /// <summary>
                /// <para xml:lang="en">ARCore.</para>
                /// <para xml:lang="zh">ARCore。</para>
                /// </summary>
                ARCore,
                /// <summary>
                /// <para xml:lang="en">AREngine.</para>
                /// <para xml:lang="zh">AREngine。</para>
                /// </summary>
                AREngine,
            }

            /// <summary>
            /// <para xml:lang="en">Download list status.</para>
            /// <para xml:lang="zh">下载列表的状态。</para>
            /// </summary>
            public enum DeviceListDownloadStatus
            {
                /// <summary>
                /// <para xml:lang="en">Download successful.</para>
                /// <para xml:lang="zh">下载成功</para>
                /// </summary>
                Successful,
                /// <summary>
                /// <para xml:lang="en">Data is already latest.</para>
                /// <para xml:lang="zh">数据已是最新版本</para>
                /// </summary>
                NotModified,
                /// <summary>
                /// <para xml:lang="en">Connection error</para>
                /// <para xml:lang="zh">连接错误</para>
                /// </summary>
                ConnectionError,
                /// <summary>
                /// <para xml:lang="en">Unexpected error</para>
                /// <para xml:lang="zh">其他错误</para>
                /// </summary>
                UnexpectedError,
            }
        }
    }

    internal static class ARAssembler
    {
        private static DeviceListDownloader downloader;

        internal static IEnumerator AssembleSession(GameObject session, AssembleOptions options, System.Diagnostics.StackTrace stackTrace, Action<ARAssembly, SessionReport> assembleFinish, Action<SessionReport.AvailabilityReport> deviceListUpdate)
        {
            var easyarController = session.GetComponent<EasyARController>();
            if (!easyarController || !session.GetComponent<DiagnosticsController>() || !session.GetComponent<FrameRecorder>() || !session.GetComponent<FramePlayer>() || !session.GetComponent<CameraImageRenderer>())
            {
                assembleFinish?.Invoke(null, new SessionReport(SessionReport.SessionBrokenReason.SessionObjectIncomplete, new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.SessionObjectIncomplete, "missing ARSession object component, please recreate ARSession in the Unity Editor", stackTrace), new SessionReport.AvailabilityReport()));
                yield break;
            }

            var error = EasyARController.LicenseValidation.Error;
            if (error.OnSome)
            {
                assembleFinish?.Invoke(null, new SessionReport(EasyARController.LicenseValidation.IsLicenseInvalid ? SessionReport.SessionBrokenReason.LicenseInvalid : SessionReport.SessionBrokenReason.Uninitialized, new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.Uninitialized, error.Value, stackTrace), new SessionReport.AvailabilityReport()));
                yield break;
            }

            var workbench = CreateWorkbench(session);
            yield return PickFrameSource(workbench, options);

            if (workbench.PickedFrameSource is FrameSource.ISyncMotionSource)
            {
                Engine.enableEyewearSupport();
                var timeStart = Time.realtimeSinceStartup;
                var interval = 5;
                while (EasyARController.LicenseValidation.ValidationState == ValidationState.Pending)
                {
                    if (EasyARController.LicenseValidation.ValidationState != ValidationState.Pending) { break; }
                    if (Time.realtimeSinceStartup - timeStart > interval)
                    {
                        timeStart = Time.realtimeSinceStartup;
                        interval = 3;
                        workbench.Diagnostics.EnqueueWarning("Please keep internet connection and wait EasyAR online validation finish...\n请保持网络连接并等待EasyAR完成线上验证...", 2f);
                    }
                    yield return null;
                }
                error = EasyARController.LicenseValidation.Error;
                if (error.OnSome)
                {
                    assembleFinish?.Invoke(null, new SessionReport(EasyARController.LicenseValidation.IsLicenseInvalid ? SessionReport.SessionBrokenReason.LicenseInvalid : SessionReport.SessionBrokenReason.Uninitialized, new SessionReport.SessionBrokenException(SessionReport.SessionBrokenReason.Uninitialized, error.Value, stackTrace), new SessionReport.AvailabilityReport()));
                    yield break;
                }
            }

            PickFrameRecorder(workbench, options);
            PickFrameFilters(workbench, options);

            var failReason = SessionReport.SessionBrokenReason.NoAvailabileFrameSource;
            var failMessage = string.Empty;
            try
            {
                if (!workbench.PickedFrameSource)
                {
                    var message = string.Empty;
                    if (options.FrameSource == AssembleOptions.FrameSourceSelection.Manual)
                    {
                        var result = workbench.Report.FrameSources.Where(fs => fs.Availability != SessionReport.AvailabilityReport.AvailabilityStatus.NotSelected).SingleOrDefault();
                        message = $"This device is not supported by the developer selected frame source (session = '{session}'):\n" + (result == null ? (!options.SpecifiedFrameSource ? "null" : $"{options.SpecifiedFrameSource} ({ARSessionFactory.DefaultName(options.SpecifiedFrameSource.GetType())}) is not active inside current session") : $"{ARSessionFactory.DefaultName(result.Component.GetType())} ({result.Availability})");
                    }
                    else if (options.FrameSource == AssembleOptions.FrameSourceSelection.Auto)
                    {
                        foreach (var fs in workbench.Report.FrameSources)
                        {
                            message += (string.IsNullOrEmpty(message) ? "" : ", ") + $"{ARSessionFactory.DefaultName(fs.Component.GetType())} ({fs.Availability})";
                        }

                        message = $"This device is not supported by any active frame source in session '{session}':\n{message}";
                    }
                    else if (options.FrameSource == AssembleOptions.FrameSourceSelection.FramePlayer)
                    {
                        message = $"This device is not supported by {ARSessionFactory.DefaultName<FramePlayer>()} (session = '{session}')";
                    }
                    throw new SessionReport.SessionBrokenException(failReason, message, stackTrace);
                }

                failReason = SessionReport.SessionBrokenReason.FrameSourceIncomplete;

                failMessage = $"missing Display from {workbench.PickedFrameSource}";
                if (workbench.PickedFrameSource.Display == null)
                {
                    throw new SessionReport.SessionBrokenException(failReason, failMessage, stackTrace);
                }
                workbench.OnPick(workbench.PickedFrameSource.Display);

                // get origin before camera, camera under XR Origin may be selected
                failMessage = $"missing Origin from {workbench.PickedFrameSource}";
                if (workbench.PickedFrameSource is FrameSource.IMotionTrackingDevice mtb)
                {
                    var origin = mtb.Origin;
                    if (!origin)
                    {
                        throw new SessionReport.SessionBrokenException(failReason, failMessage, stackTrace);
                    }
                    workbench.OnPick(origin, XROriginCache.CurrentOriginChild);
                }

                failMessage = $"missing {nameof(Camera)} from {workbench.PickedFrameSource}";
                var camera = workbench.PickedFrameSource.Camera;
                if (!camera)
                {
                    throw new SessionReport.SessionBrokenException(failReason, failMessage, stackTrace);
                }
                workbench.OnPick(camera);

                failMessage = $"missing {nameof(ARSession.ARCenterMode)} from {workbench.PickedFrameSource}";
                if (workbench.PickedFrameSource.AvailableCenterMode.Count <= 0)
                {
                    throw new SessionReport.SessionBrokenException(failReason, failMessage, stackTrace);
                }
                if (workbench.PickedOrigin && workbench.PickedFrameSource.AvailableCenterMode.Contains(ARSession.ARCenterMode.Camera) && workbench.PickedCamera.transform.IsChildOf(workbench.PickedOrigin.transform))
                {
                    var modes = workbench.PickedFrameSource.AvailableCenterMode.Where(m => m != ARSession.ARCenterMode.Camera).ToList();
                    Debug.Log($"Remove {ARSession.ARCenterMode.Camera} mode from AvailableCenterMode when camera is child of origin");
                    if (modes.Count <= 0)
                    {
                        throw new SessionReport.SessionBrokenException(failReason, failMessage, stackTrace);
                    }
                    workbench.OnPick(modes);
                }
                else
                {
                    workbench.OnPick(workbench.PickedFrameSource.AvailableCenterMode);
                }

                if (workbench.PickedFrameSource.IsCameraUnderControl)
                {
                    var controller = session.GetComponent<RenderCameraController>();
                    if (!controller) { controller = session.gameObject.AddComponent<RenderCameraController>(); }
                    workbench.OnPick(controller, session.GetComponent<CameraImageRenderer>());
                    if (workbench.Diagnostics.IsRenderPipelineProperlySetup.OnSome && !workbench.Diagnostics.IsRenderPipelineProperlySetup.Value)
                    {
                        failReason = SessionReport.SessionBrokenReason.SessionObjectIncomplete;
                        throw new SessionReport.SessionBrokenException(failReason, workbench.Diagnostics.RenderPipelineSetupError, stackTrace);
                    }
                }

                failReason = SessionReport.SessionBrokenReason.FrameFilterNotAvailabile;
                var notAvailableFilters = workbench.Report.FrameFilters.Where(f => f.Availability == SessionReport.AvailabilityReport.AvailabilityStatus.Unavailable || f.Availability == SessionReport.AvailabilityReport.AvailabilityStatus.Disabled);
                if (notAvailableFilters.Any() && options.FrameFilter != AssembleOptions.FrameFilterSelection.AutoAvailable)
                {
                    var message = string.Empty;
                    foreach (var ff in notAvailableFilters)
                    {
                        message += (string.IsNullOrEmpty(message) ? "" : ", ") + $"{ARSessionFactory.DefaultName(ff.Component.GetType())} ({ff.Availability})";
                    }
                    message = $"This device is not supported by the following frame filter(s) (session = '{session}'):\n{message}";
                    throw new SessionReport.SessionBrokenException(failReason, message, stackTrace);
                }
            }
            catch (SessionReport.SessionBrokenException e)
            {
                workbench.OnAssembleFail(e);
            }
            catch (Exception e)
            {
                workbench.OnAssembleFail(new SessionReport.SessionBrokenException(failReason, failMessage, stackTrace, e));
            }
            finally
            {
                workbench.OnAssembleFinish();
                assembleFinish?.Invoke(workbench.Assembly.OnSome ? workbench.Assembly.Value : null, new SessionReport(workbench.AssembleException.OnSome ? failReason : Optional<SessionReport.SessionBrokenReason>.Empty, workbench.AssembleException.OnSome ? workbench.AssembleException.Value : null, workbench.Report.Clone()));
            }

            if (workbench.Report.PendingDeviceList.Count > 0)
            {
                yield return new WaitUntil(() => !downloader.IsDownloading(workbench.HasMotionTracker, workbench.HasARCore, workbench.HasAREngine));
                (workbench.Report.PendingDeviceList, workbench.Report.DeviceList) = downloader.GetResults(workbench.HasMotionTracker, workbench.HasARCore, workbench.HasAREngine);
                foreach (var item in workbench.Report.FrameSources.Where(f => (workbench.HasMotionTracker && f.Component is MotionTrackerFrameSource) || (workbench.HasARCore && f.Component is ARCoreFrameSource) || (workbench.HasAREngine && f.Component is AREngineFrameSource)))
                {
                    var frameSource = item.Component as FrameSource;
                    var isAvailable = false;
                    try
                    {
                        isAvailable = frameSource.IsAvailable.Value;
                    }
                    catch (Exception e)
                    {
                        workbench.Diagnostics.EnqueueError(e.ToString());
                    }
                    item.Availability = isAvailable ? SessionReport.AvailabilityReport.AvailabilityStatus.Available : SessionReport.AvailabilityReport.AvailabilityStatus.Unavailable;
                }
                deviceListUpdate?.Invoke(workbench.Report.Clone());
            }
        }

        private static AssembleWorkbench CreateWorkbench(GameObject session)
        {
            return new AssembleWorkbench(session.GetComponent<DiagnosticsController>(), GetComponentsInChildrenTransformOrder<FrameSource>(session.transform).Where(fs => !(fs is FramePlayer)).ToList(), session.GetComponentsInChildren<FrameFilter>().ToList(), session.GetComponent<FramePlayer>(), session.GetComponent<FrameRecorder>());
        }

        private static void PickFrameRecorder(AssembleWorkbench workbench, AssembleOptions options)
        {
            if (workbench.FrameRecorder && workbench.FrameRecorder.IsAvailable && options.FrameSource != AssembleOptions.FrameSourceSelection.FramePlayer)
            {
                workbench.OnPick(workbench.FrameRecorder);
            }
        }

        private static IEnumerator PickFrameSource(AssembleWorkbench workbench, AssembleOptions options)
        {
            var fsCandidates = workbench.FrameSources;
            if (options.FrameSource == AssembleOptions.FrameSourceSelection.Manual)
            {
                fsCandidates = fsCandidates.Where(fs => fs == options.SpecifiedFrameSource).ToList();
            }
            else if (options.FrameSource == AssembleOptions.FrameSourceSelection.FramePlayer)
            {
                fsCandidates = new List<FrameSource> { workbench.FramePlayer };
            }

            foreach (var fs in workbench.FrameSources.Where(fs => !fsCandidates.Contains(fs)))
            {
                workbench.OnPick(fs, SessionReport.AvailabilityReport.AvailabilityStatus.NotSelected);
            }

            var motionTrackers = fsCandidates.Where(fs => fs is MotionTrackerFrameSource).ToList();
            if (motionTrackers.Any())
            {
                var hasMega = MayPickMegaFilter(workbench, options);
                foreach (var t in motionTrackers)
                {
                    (t as MotionTrackerFrameSource)?.DetermineMinQualityLevel(hasMega);
                }
            }

            var fsCandidatesDisabled = fsCandidates.Where(fs => (!options.EnableCustomCamera && FrameSource.IsCustomCamera(fs)) || fs.IsManuallyDisabled);
            foreach (var fs in fsCandidatesDisabled)
            {
                workbench.OnPick(fs, SessionReport.AvailabilityReport.AvailabilityStatus.Disabled);
            }
            fsCandidates = fsCandidates.Where(fs => !fsCandidatesDisabled.Contains(fs)).ToList();

            var hasMotionTracker = fsCandidates.Where(fs => fs is MotionTrackerFrameSource).Any();
            var hasARCore = fsCandidates.Where(fs => fs is ARCoreFrameSource).Any();
            var hasAREngine = fsCandidates.Where(fs => fs is AREngineFrameSource).Any();

            if (Application.platform == RuntimePlatform.Android && options.DeviceList.Timeout > 0 && (hasMotionTracker || hasARCore || hasAREngine))
            {
                if (downloader == null)
                {
                    downloader = new DeviceListDownloader();
                    AppDomain.CurrentDomain.DomainUnload += (_, __) =>
                    {
                        downloader?.Dispose();
                        downloader = null;
                    };
                }
                workbench.OnDownload(hasMotionTracker, hasARCore, hasAREngine);
                downloader.Download((int)(options.DeviceList.Timeout * 1000), options.DeviceList.IgnoreCache, hasMotionTracker, hasARCore, hasAREngine);
                var tStart = Time.realtimeSinceStartup;
                yield return new WaitUntil(
                    () => !downloader.IsDownloading(hasMotionTracker, hasARCore, hasAREngine)
                    || Time.realtimeSinceStartup - tStart >= options.DeviceList.WaitTime
                );

                (workbench.Report.PendingDeviceList, workbench.Report.DeviceList) = downloader.GetResults(hasMotionTracker, hasARCore, hasAREngine);
            }

            foreach (var fs in fsCandidates)
            {
                IEnumerator check = null;
                try
                {
                    check = fs.CheckAvailability();
                }
                catch (Exception e)
                {
                    workbench.Diagnostics.EnqueueError(e.ToString());
                }
                if (check != null)
                {
                    check = SafeCoroutine(check, (e) =>
                    {
                        workbench.Diagnostics.EnqueueError(e.ToString());
                    });
                    yield return check;
                }
                var isAvailable = false;
                try
                {
                    isAvailable = fs.IsAvailable.Value;
                }
                catch (Exception e)
                {
                    workbench.Diagnostics.EnqueueError(e.ToString());
                }
                workbench.OnPick(fs, isAvailable ? SessionReport.AvailabilityReport.AvailabilityStatus.Available : SessionReport.AvailabilityReport.AvailabilityStatus.Unavailable);
            }
        }

        private static void PickFrameFilters(AssembleWorkbench workbench, AssembleOptions options)
        {
            var ffCandidates = workbench.FrameFilters;
#if !EASYAR_ENABLE_MEGA
            ffCandidates = ffCandidates.Where(ff => !(ff is MegaTrackerFrameFilter || ff is CloudLocalizerFrameFilter)).ToList();
#endif
            if (options.FrameFilter == AssembleOptions.FrameFilterSelection.Manual)
            {
                ffCandidates = options.SpecifiedFrameFilters != null ? ffCandidates.Where(f => options.SpecifiedFrameFilters.Contains(f)).ToList() : new List<FrameFilter>();
            }
            else if (options.FrameFilter == AssembleOptions.FrameFilterSelection.None)
            {
                ffCandidates = new List<FrameFilter>();
            }

            foreach (var ff in workbench.FrameFilters.Where(ff => !ffCandidates.Contains(ff)))
            {
                workbench.OnPick(ff, SessionReport.AvailabilityReport.AvailabilityStatus.NotSelected);
            }

            var hasMegaWhenPlayer = false;
            foreach (var component in ffCandidates)
            {
                var isAvailable = false;
                var isDisabled = false;
                try
                {
                    isAvailable = component.IsAvailable;
                }
                catch (Exception e)
                {
                    workbench.Diagnostics.EnqueueError(e.ToString());
                }
                if (component is SurfaceTrackerFrameFilter)
                {
                    isAvailable = isAvailable && !((workbench.IsAsync.OnSome && !workbench.IsAsync.Value) || workbench.PickedFrameSource is ExternalFrameSource);
                }
                if (component is MegaTrackerFrameFilter megaTracker)
                {
                    var possibleType = MegaInputFrameLevel.SixDof;
                    if (workbench.PickedFrameSource is CameraDeviceFrameSource) { possibleType = MegaInputFrameLevel.ZeroDof; }
                    if (workbench.PickedFrameSource is InertialCameraDeviceFrameSource) { possibleType = MegaInputFrameLevel.FiveDof; }
                    if (workbench.PickedFrameSource is ThreeDofCameraDeviceFrameSource) { possibleType = MegaInputFrameLevel.ThreeDof; }
                    var minType = megaTracker.MinInputFrameLevel;
                    var isEditorSimulation = Application.isEditor && !(workbench.PickedFrameSource is FramePlayer);
                    isDisabled = !isEditorSimulation && possibleType < minType;
                }
                if (workbench.PickedFrameSource is FramePlayer && component is IFrameSupplementFilter && isAvailable && !isDisabled)
                {
                    if (hasMegaWhenPlayer)
                    {
                        Debug.LogWarning($"only one Mega frame filter is allowed when using {nameof(FramePlayer)}");
                        isDisabled = true;
                    }
                    hasMegaWhenPlayer = true;
                }
                workbench.OnPick(component, isDisabled ? SessionReport.AvailabilityReport.AvailabilityStatus.Disabled : (isAvailable ? SessionReport.AvailabilityReport.AvailabilityStatus.Available : SessionReport.AvailabilityReport.AvailabilityStatus.Unavailable));
            }
        }

        private static bool MayPickMegaFilter(AssembleWorkbench workbench, AssembleOptions options)
        {
            var ffCandidates = workbench.FrameFilters;
            if (options.FrameFilter == AssembleOptions.FrameFilterSelection.Manual)
            {
                ffCandidates = options.SpecifiedFrameFilters != null ? ffCandidates.Where(f => options.SpecifiedFrameFilters.Contains(f)).ToList() : new List<FrameFilter>();
            }
            else if (options.FrameFilter == AssembleOptions.FrameFilterSelection.None)
            {
                ffCandidates = new List<FrameFilter>();
            }
            return ffCandidates.Where(ff => ff is MegaTrackerFrameFilter || ff is CloudLocalizerFrameFilter).Any();
        }

        private static List<CType> GetComponentsInChildrenTransformOrder<CType>(Transform transform)
        {
            var list = new List<CType>();
            foreach (Transform t in transform)
            {
                GetComponentsInChildrenTransformOrder(list, t);
            }
            return list;
        }

        private static void GetComponentsInChildrenTransformOrder<CType>(List<CType> transforms, Transform transform)
        {
            if (!transform || !transform.gameObject.activeSelf) { return; }
            transforms.AddRange(transform.GetComponents<CType>());
            foreach (Transform t in transform)
            {
                GetComponentsInChildrenTransformOrder(transforms, t);
            }
        }

        private static IEnumerator SafeCoroutine(IEnumerator enumerator, Action<Exception> callback)
        {
            if (enumerator == null) { yield break; }

            while (true)
            {
                var b = false;
                try
                {
                    b = enumerator.MoveNext();
                }
                catch (Exception e)
                {
                    callback?.Invoke(e);
                    break;
                }
                if (!b) { break; }
                yield return enumerator.Current;
            }
        }
    }

    internal class AssembleWorkbench
    {
        public DiagnosticsController Diagnostics { get; private set; }
        public List<FrameSource> FrameSources { get; private set; }
        public List<FrameFilter> FrameFilters { get; private set; }
        public FramePlayer FramePlayer { get; private set; }
        public FrameRecorder FrameRecorder { get; private set; }
        public Optional<SessionReport.SessionBrokenException> AssembleException { get; private set; }
        public Optional<ARAssembly> Assembly { get; private set; }
        public SessionReport.AvailabilityReport Report { get; private set; } = new SessionReport.AvailabilityReport();

        internal List<FrameSource> FrameSourceCandidates { get; private set; }
        internal FrameSource PickedFrameSource { get; private set; }
        internal List<FrameFilter> PickedFrameFilters { get; private set; } = new List<FrameFilter>();
        internal FrameRecorder PickedFrameRecorder { get; private set; }
        internal RenderCameraController PickedCameraController { get; private set; }
        internal CameraImageRenderer PickedCameraImageRenderer { get; private set; }
        internal IReadOnlyList<ARSession.ARCenterMode> PickedCenterMode { get; private set; }
        internal GameObject PickedOrigin { get; private set; }
        internal XROriginChildController PickedOriginChild { get; private set; }
        internal Camera PickedCamera { get; private set; }
        internal IDisplay PickedDisplay { get; private set; }
        internal Optional<bool> IsAsync { get; private set; }

        internal bool HasMotionTracker { get; private set; }
        internal bool HasARCore { get; private set; }
        internal bool HasAREngine { get; private set; }

        internal AssembleWorkbench(DiagnosticsController diagnostics, List<FrameSource> frameSources, List<FrameFilter> frameFilters, FramePlayer framePlayer, FrameRecorder frameRecorder)
        {
            Diagnostics = diagnostics;
            FrameSources = frameSources;
            FrameFilters = frameFilters;
            FramePlayer = framePlayer;
            FrameRecorder = frameRecorder;
        }

        internal void OnDownload(bool requireMotionTracker, bool requireARCore, bool requireAREngine)
        {
            HasMotionTracker = requireMotionTracker;
            HasARCore = requireARCore;
            HasAREngine = requireAREngine;
        }

        internal void OnPick(FrameSource component, SessionReport.AvailabilityReport.AvailabilityStatus status)
        {
            if (status == SessionReport.AvailabilityReport.AvailabilityStatus.Available && !PickedFrameSource)
            {
                PickedFrameSource = component;
                IsAsync = !(PickedFrameSource is FrameSource.ISyncMotionSource);
            }
            Report.FrameSources.Add(new SessionReport.AvailabilityReport.Item { Component = component, Availability = status });
        }

        internal void OnPick(FrameFilter component, SessionReport.AvailabilityReport.AvailabilityStatus status)
        {
            if (status == SessionReport.AvailabilityReport.AvailabilityStatus.Available)
            {
                PickedFrameFilters.Add(component);
            }
            Report.FrameFilters.Add(new SessionReport.AvailabilityReport.Item { Component = component, Availability = status });
        }

        internal void OnPick(RenderCameraController cameraController, CameraImageRenderer cameraImageRenderer)
        {
            PickedCameraController = cameraController;
            PickedCameraImageRenderer = cameraImageRenderer;
        }
        internal void OnPick(FrameRecorder component) => PickedFrameRecorder = component;
        internal void OnPick(Camera camera) => PickedCamera = camera;
        internal void OnPick(IDisplay display) => PickedDisplay = display;
        internal void OnPick(GameObject origin, XROriginChildController child)
        {
            PickedOrigin = origin;
            PickedOriginChild = child;
        }
        internal void OnPick(IReadOnlyList<ARSession.ARCenterMode> availableCenterMode) => PickedCenterMode = availableCenterMode;

        internal void OnAssembleFail(SessionReport.SessionBrokenException e) => AssembleException = e;

        internal void OnAssembleFinish()
        {
            if (AssembleException.OnSome) { return; }
            Assembly = new ARAssembly(this);
        }
    }
}
