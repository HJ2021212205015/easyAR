//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

#if EASYAR_ENABLE_MEGA
using EasyAR.Mega.Scene;
#endif
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Cloud based visual positioning localization filtering for Mega Landmark.</para>
    /// <para xml:lang="zh">Mega Landmark VPS云定位过滤功能。</para>
    /// </summary>
    public class MegaLandmarkFilterWrapper
    {
        private MegaLandmarkFilter filter;
        private Func<Optional<LocationResult>> locationGetter;
        private Func<int> timeoutGetter;
        private Action<MegaLandmarkFilterResponse> onResponse;

        internal MegaLandmarkFilterWrapper(ExplicitAddressAccessData access, Func<Optional<LocationResult>> locationGetter, Func<int> timeoutGetter, Action<MegaLandmarkFilterResponse> onResponse)
        {
            try
            {
                if (access is APIKeyAccessData apiKeyAccess)
                {
                    filter = MegaLandmarkFilter.create(apiKeyAccess.ServerAddress, apiKeyAccess.APIKey, apiKeyAccess.APISecret, apiKeyAccess.AppID);
                }
                else if (access is TokenAccessData tokenAccess)
                {
                    filter = MegaLandmarkFilter.createWithToken(tokenAccess.ServerAddress, tokenAccess.Token, tokenAccess.AppID);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported access data type: {access?.GetType().Name}");
                }
            }
            catch (ArgumentNullException)
            {
                throw new DiagnosticsMessageException($"Fail to create {nameof(MegaLandmarkFilter)}, check logs for detials.");
            }
            this.locationGetter = locationGetter;
            this.timeoutGetter = timeoutGetter;
            this.onResponse = onResponse;
        }

        /// <summary>
        /// <para xml:lang="en">Location filtering. Filter with GPS data.</para>
        /// <para xml:lang="zh">位置筛选。使用GPS信息进行筛选。</para>
        /// </summary>
        public void FilterByLocation(Action<MegaLandmarkFilterResponse> callback)
        {
            var location = locationGetter();
            if (location.OnNone)
            {
                Debug.LogError("no location data");
                var response = new MegaLandmarkFilterResponse("no location data");
                onResponse.Invoke(response);
                callback?.Invoke(response);
                return;
            }

            filter.filterByLocation(location.Value, timeoutGetter(), EasyARController.Scheduler, (result) =>
            {
                var response = new MegaLandmarkFilterResponse
                {
                    Timestamp = Time.realtimeSinceStartupAsDouble,
                    Status = result.status(),
                    SpotVersionId = result.spotVersionId(),
                    ErrorMessage = string.IsNullOrEmpty(result.exceptionInfo()) ? Optional<string>.Empty : result.exceptionInfo(),
                };
                onResponse.Invoke(response);
                callback?.Invoke(response);
            });
        }

        /// <summary>
        /// <para xml:lang="en">Filtering by SpotId.</para>
        /// <para xml:lang="zh">通过SpotId筛选。</para>
        /// </summary>
        public void FilterBySpotId(string spotId, Action<MegaLandmarkFilterResponse> callback)
        {
            filter.filterBySpotId(spotId, timeoutGetter(), EasyARController.Scheduler, (result) =>
            {
                var response = new MegaLandmarkFilterResponse
                {
                    Timestamp = Time.realtimeSinceStartupAsDouble,
                    Status = result.status(),
                    SpotVersionId = result.spotVersionId(),
                    ErrorMessage = string.IsNullOrEmpty(result.exceptionInfo()) ? Optional<string>.Empty : result.exceptionInfo(),
                };
                onResponse.Invoke(response);
                callback?.Invoke(response);
            });
        }

        internal void SwitchEndPoint(ExplicitAddressAccessData access)
        {
            try
            {
                if (access is APIKeyAccessData apiKeyAccess)
                {
                    filter = MegaLandmarkFilter.create(apiKeyAccess.ServerAddress, apiKeyAccess.APIKey, apiKeyAccess.APISecret, access.AppID);
                }
                else if (access is TokenAccessData tokenAccess)
                {
                    filter = MegaLandmarkFilter.createWithToken(tokenAccess.ServerAddress, tokenAccess.Token, tokenAccess.AppID);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported access data type: {access.GetType().Name}");
                }
            }
            catch (ArgumentNullException)
            {
                throw new DiagnosticsMessageException($"Fail to switch {nameof(MegaLandmarkFilter)} remote end point, check logs for detials.");
            }
        }

        internal void UpdateToken(string token)
        {
            filter?.updateToken(token);
        }
    }

    internal class FrameSupplementInput
    {
        private readonly object lockObject = new();
        private Optional<LocationResult> location;
        private Optional<AccelerometerResult> accelerometer;
        private Optional<ProximityLocationResult> proximityLocation;

        public Optional<LocationResult> Location
        {
            get
            {
                lock (lockObject) { return location; }
            }
            set
            {
                lock (lockObject) { location = value; }
            }
        }
        public Optional<AccelerometerResult> Accelerometer
        {
            get
            {
                lock (lockObject) { return accelerometer; }
            }
            set
            {
                lock (lockObject) { accelerometer = value; }
            }
        }
        public Optional<ProximityLocationResult> ProximityLocation
        {
            get
            {
                lock (lockObject) { return proximityLocation; }
            }
            set
            {
                lock (lockObject) { proximityLocation = value; }
            }
        }
    }

    internal static class DeviceAuxiliaryInfoFactory
    {
        public static DeviceAuxiliaryInfo Create(Accelerometer accelerometer, LocationManager locationManager, Optional<ProximityLocationResult> proximityLocation)
        {
            var info = DeviceAuxiliaryInfo.create();
            if (accelerometer != null)
            {
                var accResult = accelerometer.getCurrentResult();
                if (accResult.OnSome)
                {
                    info.setAcceleration(accResult.Value);
                }
            }
            if (locationManager)
            {
                var loc = locationManager.CurrentResult;
                if (loc.HasValue)
                {
                    info.setGPSLocation(new LocationResult(loc.Value.latitude, loc.Value.longitude, loc.Value.altitude, loc.Value.horizontalAccuracy, loc.Value.verticalAccuracy, true, true, true));
                }
            }
            if (proximityLocation.OnSome)
            {
                info.setProximityLocation(proximityLocation.Value);
            }
            return info;
        }

        public static DeviceAuxiliaryInfo Create(Accelerometer accelerometer, Optional<LocationResult> simulatorLocation, Optional<ProximityLocationResult> proximityLocation)
        {
            var info = DeviceAuxiliaryInfo.create();
            if (accelerometer != null)
            {
                var accResult = accelerometer.getCurrentResult();
                if (accResult.OnSome)
                {
                    info.setAcceleration(accResult.Value);
                }
            }
            if (simulatorLocation.OnSome)
            {
                info.setGPSLocation(simulatorLocation.Value);
            }
            if (proximityLocation.OnSome)
            {
                info.setProximityLocation(proximityLocation.Value);
            }
            return info;
        }

        public static DeviceAuxiliaryInfo Create(FrameSupplementInput frameSupplementInput)
        {
            var info = DeviceAuxiliaryInfo.create();
            if (frameSupplementInput.Accelerometer.OnSome)
            {
                info.setAcceleration(frameSupplementInput.Accelerometer.Value);
            }
            if (frameSupplementInput.Location.OnSome)
            {
                info.setGPSLocation(frameSupplementInput.Location.Value);
            }
            if (frameSupplementInput.ProximityLocation.OnSome)
            {
                info.setProximityLocation(frameSupplementInput.ProximityLocation.Value);
            }
            return info;
        }
    }

    internal class MegaWarningHelper
    {
        DiagnosticsController diagnostics;
        bool hasFilterWarning;
        bool hasLocationInputWarning;

        public MegaWarningHelper(DiagnosticsController diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public void DisplayWarningMessages(Optional<MegaLocationInputMode> LocationInputMode) => DisplayWarningMessages(false, Optional<MegaInputFrameLevel>.Empty, Optional<CameraTransformType>.Empty, LocationInputMode);

        public void DisplayWarningMessages(bool isEditorSimulation, Optional<MegaInputFrameLevel> minTransformType, Optional<CameraTransformType> curTransformType, Optional<MegaLocationInputMode> LocationInputMode)
        {
#if EASYAR_ENABLE_MEGA
            if (isEditorSimulation && !hasFilterWarning)
            {
                if (curTransformType.OnSome && minTransformType.OnSome && curTransformType.Value.ToMegaInputFrameLevel() < minTransformType.Value)
                {
                    diagnostics.EnqueueCaution($"Mega is running in Simulator mode in Unity Editor without device motion tracking ability: current input frame level ({curTransformType.Value.ToMegaInputFrameLevel()}) < minimum acceptable input frame level ({minTransformType}).");
                    hasFilterWarning = true;
                }
            }
            if (LocationInputMode == MegaLocationInputMode.Simulator && !hasLocationInputWarning)
            {
                diagnostics.EnqueueCaution($"Mega is running in {MegaLocationInputMode.Simulator} mode with simulated or no location input." + Environment.NewLine +
                $"For real Mega experience, please set LocationInputMode to {MegaLocationInputMode.Onsite} and run on mobile devices.");
                hasLocationInputWarning = true;
            }
#endif
        }
    }

    internal class MegaDumpHelper
    {
        MegaLocalizationResponse response;
        MegaLandmarkFilterResponse filterResponse;
        float time;
        float timeFilter;
#if EASYAR_ENABLE_MEGA
        BlockController recentBlock;
#endif

        public void UpdateResponse(MegaLocalizationResponse response)
        {
            this.response = response;
            time = Time.realtimeSinceStartup;
        }

        public void UpdateFilterResponse(MegaLandmarkFilterResponse filterResponse)
        {
            this.filterResponse = filterResponse;
            timeFilter = Time.realtimeSinceStartup;
        }

#if EASYAR_ENABLE_MEGA
        public string DumpResponse(Camera camera, BlockRootController blockRoot)
        {
            string data = string.Empty;
            if (filterResponse != null)
            {
                data += $"- Filter: {filterResponse.Timestamp:F3}, {filterResponse.Status}, {filterResponse.SpotVersionId}" + Environment.NewLine;
                if (filterResponse.ErrorMessage.OnSome)
                {
                    data += "- Filter Error: " + filterResponse.ErrorMessage + Environment.NewLine;
                }
            }
            if (response != null)
            {
                data += $"- {response.Timestamp:F3}, {response.Status}, ({(response.ServerResponseDuration.OnSome ? response.ServerResponseDuration.Value.ToString("F3") : response.ServerResponseDuration.ToString())}, {(response.ServerCalculationDuration.OnSome ? response.ServerCalculationDuration.Value.ToString("F3") : response.ServerCalculationDuration.ToString())})" + Environment.NewLine;
                foreach (var block in response.Blocks)
                {
                    recentBlock = block;
                    data += $"- Block: {block.Info.Name} ({block.Info.ID})" + Environment.NewLine;
                }
                if (response.SpotVersionId.OnSome)
                {
                    data += $"- Spot Version: {response.SpotVersionId}" + Environment.NewLine;
                }
                if (response.ErrorMessage.OnSome)
                {
                    data += "- Error: " + response.ErrorMessage + Environment.NewLine;
                }
            }
            if (camera && blockRoot && recentBlock)
            {
                var cameraPose = new Pose(camera.transform.position, camera.transform.rotation);
                var pose = cameraPose.GetTransformedBy(new Pose(blockRoot.transform.position, blockRoot.transform.rotation).Inverse());
                var poseBlock = cameraPose.GetTransformedBy(new Pose(recentBlock.transform.position, recentBlock.transform.rotation).Inverse());
                data += $"- Device Pos: root {pose.position}, block {poseBlock.position}" + Environment.NewLine;
            }

            if (Time.realtimeSinceStartup - time > 5)
            {
                response = null;
                time = Time.realtimeSinceStartup;
            }
            if (Time.realtimeSinceStartup - timeFilter > 60)
            {
                filterResponse = null;
                timeFilter = Time.realtimeSinceStartup;
            }
            return data;
        }
#endif

        public static string ServerSuffix(string address)
        {
            if (string.IsNullOrEmpty(address)) { return string.Empty; }
            var r = new Regex(@"^https://(?<API>(cls|landmark[v]?)[0-9]*-api)\.easyar\.com", RegexOptions.ExplicitCapture);
            var match = r.Match(address);
            if (match.Success)
            {
                var api = match.Result("${API}");
                return $"<{api}>" + r.Replace(address, m => "");
            }
            return address;
        }
    }

#if EASYAR_ENABLE_MEGA
    internal class BlockHolderDiagnosticsWrapper
    {
        private BlockHolder blockHolder;

        public BlockHolderDiagnosticsWrapper(BlockHolder blockHolder)
        {
            this.blockHolder = blockHolder;
        }

        public BlockController OnLocalize(BlockController.BlockInfo blockInfo)
        {
            try
            {
                return blockHolder.OnLocalize(blockInfo);
            }
            catch (BlockHolder.ConfigException e)
            {
                throw new DiagnosticsMessageException(e.Message);
            }
        }

        public void OnTrack(List<Tuple<BlockController.BlockInfo, BlockHolder.PoseSet>> blocks)
        {
            try
            {
                blockHolder.OnTrack(blocks);
            }
            catch (BlockHolder.ConfigException e)
            {
                throw new DiagnosticsMessageException(e.Message);
            }
        }
    }
#endif

    internal static class MegaInputFrameLevelConverter
    {
        public static MegaInputFrameLevel ToMegaInputFrameLevel(this CameraTransformType transformType)
        {
            switch (transformType)
            {
                case CameraTransformType.ZeroDof:
                    return MegaInputFrameLevel.ZeroDof;
                case CameraTransformType.ThreeDofRotOnly:
                    return MegaInputFrameLevel.ThreeDof;
                case CameraTransformType.SixDof:
                    return MegaInputFrameLevel.SixDof;
                case CameraTransformType.FiveDofRotXZ:
                    return MegaInputFrameLevel.FiveDof;
                default:
                    throw new NotSupportedException($"{nameof(CameraTransformType)}: {transformType}");
            }
        }
    }
}
