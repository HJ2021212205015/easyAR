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
    internal class DeviceListDownloader : IDisposable
    {
        private ImmediateCallbackScheduler scheduler;
        private CalibrationDownloader motionTracker;
        private ARCoreDeviceListDownloader arCore;

#if UNITY_ANDROID && !UNITY_EDITOR
        private arengineinterop.ImmediateCallbackScheduler schedulerAREngine;
        private arengineinterop.AREngineDeviceListDownloader arEngine;
#endif

        private readonly object resultAccess = new object();
        bool motionTrackerRunning;
        Optional<Tuple<CalibrationDownloadStatus, Optional<string>, DateTime>> motionTrackerResult;
        bool arCoreRunning;
        Optional<Tuple<ARCoreDeviceListDownloadStatus, Optional<string>, DateTime>> arCoreResult;
#if UNITY_ANDROID && !UNITY_EDITOR
        bool arEngineRunning;
        Optional<Tuple<arengineinterop.AREngineDeviceListDownloadStatus, Optional<string>, DateTime>> arEngineResult;
#endif

        ~DeviceListDownloader()
        {
            DisposeAll();
        }

        public void Download(int timeout, bool ignoreDeviceListCache, bool requireMotionTracker, bool requireARCore, bool requireAREngine)
        {
            var now = DateTime.Now;
            var interval = new TimeSpan(1, 0, 0, 0);

            lock (resultAccess)
            {
                if (requireMotionTracker)
                {
                    if (motionTrackerRunning)
                    {
                        requireMotionTracker = false;
                    }
                    if (!ignoreDeviceListCache
                        && motionTrackerResult.OnSome
                        && (motionTrackerResult.Value.Item1 == CalibrationDownloadStatus.Successful || motionTrackerResult.Value.Item1 == CalibrationDownloadStatus.NotModified)
                        && now - motionTrackerResult.Value.Item3 < interval)
                    {
                        requireMotionTracker = false;
                    }
                    if (requireMotionTracker)
                    {
                        motionTrackerRunning = true;
                        motionTrackerResult = null;
                    }
                }
                if (requireARCore)
                {
                    if (arCoreRunning)
                    {
                        requireARCore = false;
                    }
                    if (!ignoreDeviceListCache
                        && arCoreResult.OnSome
                        && (arCoreResult.Value.Item1 == ARCoreDeviceListDownloadStatus.Successful || arCoreResult.Value.Item1 == ARCoreDeviceListDownloadStatus.NotModified)
                        && now - arCoreResult.Value.Item3 < interval)
                    {
                        requireARCore = false;
                    }
                    if (requireARCore)
                    {
                        arCoreRunning = true;
                        arCoreResult = null;
                    }
                }
#if UNITY_ANDROID && !UNITY_EDITOR
                if (IsAREngineEnabled() && requireAREngine)
                {
                    if (arEngineRunning)
                    {
                        requireAREngine = false;
                    }
                    if (!ignoreDeviceListCache
                        && arEngineResult.OnSome
                        && (arEngineResult.Value.Item1 == arengineinterop.AREngineDeviceListDownloadStatus.Successful || arEngineResult.Value.Item1 == arengineinterop.AREngineDeviceListDownloadStatus.NotModified)
                        && now - arEngineResult.Value.Item3 < interval)
                    {
                        requireAREngine = false;
                    }
                    if (requireAREngine)
                    {
                        arEngineRunning = true;
                        arEngineResult = null;
                    }
                }
#endif
            }
            if (requireMotionTracker)
            {
                if (motionTracker == null)
                {
                    motionTracker = new CalibrationDownloader();
                }
                if (scheduler == null)
                {
                    scheduler = ImmediateCallbackScheduler.getDefault();
                }
                motionTracker.download(timeout, scheduler, (status, error) =>
                {
                    Debug.Log($"{nameof(CalibrationDownloader)}: {status}{(error.OnSome ? ", " + error : string.Empty)}");
                    lock (resultAccess)
                    {
                        motionTrackerResult = Tuple.Create(status, error, DateTime.Now);
                        motionTrackerRunning = false;
                    }
                });
            }
            if (requireARCore)
            {
                if (arCore == null)
                {
                    arCore = new ARCoreDeviceListDownloader();
                }
                if (scheduler == null)
                {
                    scheduler = ImmediateCallbackScheduler.getDefault();
                }
                arCore.download(timeout, scheduler, (status, error) =>
                {
                    Debug.Log($"{nameof(ARCoreDeviceListDownloader)}: {status}{(error.OnSome ? ", " + error : string.Empty)}");
                    lock (resultAccess)
                    {
                        arCoreResult = Tuple.Create(status, error, DateTime.Now);
                        arCoreRunning = false;
                    }
                });
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            if (IsAREngineEnabled() && requireAREngine)
            {
                if (arEngine == null)
                {
                    arEngine = new arengineinterop.AREngineDeviceListDownloader();
                }
                if (schedulerAREngine == null)
                {
                    schedulerAREngine = arengineinterop.ImmediateCallbackScheduler.getDefault();
                }
                arEngine.download(timeout, schedulerAREngine, (status, error) =>
                {
                    Debug.Log($"{nameof(arengineinterop.AREngineDeviceListDownloader)}: {status}{(error.OnSome ? ", " + error : string.Empty)}");
                    lock (resultAccess)
                    {
                        arEngineResult = Tuple.Create(status, error, DateTime.Now);
                        arEngineRunning = false;
                    }
                });
            }
#endif
        }

        public bool IsDownloading(bool requireMotionTracker, bool requireARCore, bool requireAREngine)
        {
            lock (resultAccess)
            {
                if (requireMotionTracker && motionTrackerRunning) { return true; }
                if (requireARCore && arCoreRunning) { return true; }
#if UNITY_ANDROID && !UNITY_EDITOR
                if (requireAREngine && arEngineRunning) { return true; }
#endif
            }
            return false;
        }

        public (List<SessionReport.AvailabilityReport.DeviceListDownloadType> downloads, List<SessionReport.AvailabilityReport.DeviceListDownloadResult> results) GetResults(bool requireMotionTracker, bool requireARCore, bool requireAREngine)
        {
            var downloads = new List<SessionReport.AvailabilityReport.DeviceListDownloadType>();
            var results = new List<SessionReport.AvailabilityReport.DeviceListDownloadResult>();

            lock (resultAccess)
            {
                if (requireMotionTracker)
                {
                    if (motionTrackerRunning)
                    {
                        downloads.Add(SessionReport.AvailabilityReport.DeviceListDownloadType.MotionTracker);
                    }
                    else if (motionTrackerResult.OnSome)
                    {
                        results.Add(new SessionReport.AvailabilityReport.DeviceListDownloadResult
                        {
                            Type = SessionReport.AvailabilityReport.DeviceListDownloadType.MotionTracker,
                            Status = (SessionReport.AvailabilityReport.DeviceListDownloadStatus)motionTrackerResult.Value.Item1,
                            Error = motionTrackerResult.Value.Item2.OnSome ? motionTrackerResult.Value.Item2.Value : null,
                        });
                    }
                }

                if (requireARCore)
                {
                    if (arCoreRunning)
                    {
                        downloads.Add(SessionReport.AvailabilityReport.DeviceListDownloadType.ARCore);
                    }
                    else if (arCoreResult.OnSome)
                    {
                        results.Add(new SessionReport.AvailabilityReport.DeviceListDownloadResult
                        {
                            Type = SessionReport.AvailabilityReport.DeviceListDownloadType.ARCore,
                            Status = (SessionReport.AvailabilityReport.DeviceListDownloadStatus)arCoreResult.Value.Item1,
                            Error = arCoreResult.Value.Item2.OnSome ? arCoreResult.Value.Item2.Value : null,
                        });
                    }
                }

#if UNITY_ANDROID && !UNITY_EDITOR
                if (IsAREngineEnabled() && requireAREngine)
                {
                    if (arEngineRunning)
                    {
                        downloads.Add(SessionReport.AvailabilityReport.DeviceListDownloadType.AREngine);
                    }
                    else if (arEngineResult.OnSome)
                    {
                        results.Add(new SessionReport.AvailabilityReport.DeviceListDownloadResult
                        {
                            Type = SessionReport.AvailabilityReport.DeviceListDownloadType.AREngine,
                            Status = (SessionReport.AvailabilityReport.DeviceListDownloadStatus)arEngineResult.Value.Item1,
                            Error = arEngineResult.Value.Item2.OnSome ? arEngineResult.Value.Item2.Value : null,
                        });
                    }
                }
#endif
            }

            return (downloads, results);
        }

        public void Dispose()
        {
            DisposeAll();
            GC.SuppressFinalize(this);
        }

        private void DisposeAll()
        {
            motionTracker?.Dispose();
            arCore?.Dispose();
            scheduler?.Dispose();
#if UNITY_ANDROID && !UNITY_EDITOR
            arEngine?.Dispose();
            schedulerAREngine?.Dispose();
#endif
        }

        private bool IsAREngineEnabled()
        {
            if (EasyARSettings.Instance)
            {
                return EasyARSettings.Instance.AREngineSDK != EasyARSettings.AREngineType.Disabled;
            }
            return true;
        }
    }
}
