//================================================================================================================================
//
//  Copyright (c) 2020-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace easyar
{
    [DisallowMultipleComponent]
    internal class LocationManager : MonoBehaviour
    {
        private static List<LocationManager> startedManagers = new List<LocationManager>();
        private bool isStarted = false;
        private bool isFirstStart = true;
        private bool isPaused;
        private bool? isPermissionGranted;

        public LocationInfo? CurrentResult
        {
            get
            {
#if EASYAR_ENABLE_MEGA
                if (!Input.location.isEnabledByUser || Input.location.status != LocationServiceStatus.Running) { return null; }
                return Input.location.lastData;
#else
                return null;
#endif
            }
        }

        private void OnEnable()
        {
            if (isPaused) { return; }
            if (isStarted)
            {
                if (startedManagers.Where(f => f).Count() == 0)
                {
#if EASYAR_ENABLE_MEGA
                    Input.location.Start(10, 3);
#endif
                }
                if (!startedManagers.Contains(this))
                {
                    startedManagers.Add(this);
                }
                if (isFirstStart)
                {
                    if (Application.platform == RuntimePlatform.IPhonePlayer || SystemUtil.IsVisionOS())
                    {
                        StartCoroutine(CheckPermission(Time.realtimeSinceStartup));
                    }
                    isFirstStart = false;
                }
            }
        }

        private void OnDisable()
        {
            if (isPaused) { return; }
            startedManagers.Remove(this);
            if (isStarted)
            {
                if (startedManagers.Where(f => f).Count() == 0)
                {
#if EASYAR_ENABLE_MEGA
                    Input.location.Stop();
#endif
                }
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (enabled) { OnDisable(); }
                isPaused = true;
            }
            else
            {
                isPaused = false;
                if (enabled) { OnEnable(); }
            }
        }

        public void RequestLocationPermission(Action<bool> callback)
        {
#if EASYAR_ENABLE_MEGA
            if (Application.platform == RuntimePlatform.IPhonePlayer || SystemUtil.IsVisionOS())
            {
                SetLocationPermission(null);
                StartCoroutine(CheckLocationPermission(callback));
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                {
                    SetLocationPermission(true);
                }
                else
                {
                    var callbacks = new PermissionCallbacks();
                    callbacks.PermissionGranted += (_) => { if (this) { SetLocationPermission(true); } };
                    callbacks.PermissionDenied += (_) => { if (this) { SetLocationPermission(false); } };
                    callbacks.PermissionDeniedAndDontAskAgain += (_) => { if (this) { SetLocationPermission(false); } };
                    Permission.RequestUserPermission(Permission.FineLocation, callbacks);
                }
                StartCoroutine(CheckLocationPermission(callback));
#endif
            }
#else
            SetLocationPermission(false);
#endif
        }

        public IEnumerator HandleLocation(LocationResultSink locationResultSink)
        {
            double timestamp = 0;
            while (true)
            {
                while (locationResultSink == null) { yield return null; }
                while (!this || !CurrentResult.HasValue) { yield return null; }
                var loc = CurrentResult.Value;
                if (loc.timestamp == timestamp) { yield return null; }
                timestamp = loc.timestamp;
                locationResultSink.handle(new LocationResult(loc.latitude, loc.longitude, loc.altitude, loc.horizontalAccuracy, loc.verticalAccuracy, true, true, true));
                yield return null;
            }
        }

        private IEnumerator CheckLocationPermission(Action<bool> callback)
        {
            while (!isPermissionGranted.HasValue) { yield return null; }
            callback?.Invoke(isPermissionGranted.Value);
        }

        private void SetLocationPermission(bool? granted)
        {
            isPermissionGranted = granted;
#if EASYAR_ENABLE_MEGA
            if (Application.platform == RuntimePlatform.Android)
            {
                if (isPermissionGranted.HasValue && isPermissionGranted.Value && Input.location.isEnabledByUser)
                {
                    isStarted = true;
                    if (enabled) { OnEnable(); }
                }
                else
                {
                    StartCoroutine(CheckPermission(Time.realtimeSinceStartup));
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer || SystemUtil.IsVisionOS())
            {
                if (!Input.location.isEnabledByUser)
                {
                    isPermissionGranted = false;
                }
                else
                {
                    isStarted = true;
                    if (enabled) { OnEnable(); }
                }
            }
#endif
        }

        private IEnumerator CheckPermission(float requestTime)
        {
#if EASYAR_ENABLE_MEGA
            if (Application.platform == RuntimePlatform.Android)
            {
                while (!Input.location.isEnabledByUser)
                {
                    if (!isPermissionGranted.HasValue && Time.realtimeSinceStartup - requestTime > 5)
                    {
                        isPermissionGranted = Input.location.isEnabledByUser;
                    }
                    yield return null;
                }
                isPermissionGranted = Input.location.isEnabledByUser;
                if (isPermissionGranted.Value)
                {
                    isStarted = true;
                    if (enabled) { OnEnable(); }
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer || SystemUtil.IsVisionOS())
            {
                while (Input.location.status == LocationServiceStatus.Initializing || Input.location.status == LocationServiceStatus.Stopped)
                {
                    yield return null;
                }
                isPermissionGranted = Input.location.status == LocationServiceStatus.Running;
            }
#else
            yield return null;
#endif
        }
    }
}
