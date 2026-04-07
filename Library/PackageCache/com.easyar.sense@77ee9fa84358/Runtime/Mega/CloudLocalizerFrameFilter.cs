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
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="CloudLocalizer"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="CloudLocalizer"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
#if EASYAR_ENABLE_MEGA
    [RequireComponent(typeof(BlockHolder))]
#endif
    public class CloudLocalizerFrameFilter : FrameFilter, FrameFilter.ICenterTransform, IFrameSupplementFilter
    {
        /// <summary>
        /// <para xml:lang="en">Timeout in milliseconds when communicating with server.</para>
        /// <para xml:lang="zh">与服务器通信的超时时间（毫秒）。</para>
        /// </summary>
        public int RequestTimeout = 6000;

        private CloudLocalizer localizer;
        private Accelerometer accelerometer;
        private LocationManager locationManager;

        [HideInInspector, SerializeField]
        private MegaApiType serviceType;
        [HideInInspector, SerializeField]
        private MegaServiceAccessSourceType serviceAccessSource;
        [HideInInspector, SerializeField]
        private APIKeyAccessData apiKeyAccessData = new();
        [HideInInspector, SerializeField]
        private TokenAccessData tokenAccessData = new();
        [HideInInspector, SerializeField]
        private MegaLocationInputMode locationInputMode = MegaLocationInputMode.Simulator;
        [HideInInspector, SerializeField]
        private MegaBlockPrior blockPrior = new();
        [HideInInspector, SerializeField]
        private bool hasBlockPrior;
        private string requestMessage;
        private Optional<ProximityLocationResult> proximityLocation;

        private ARSession arSession;
        private ExplicitAddressAccessData currentEndPoint;

        private ResolveRequest request = new ResolveRequest();
        private MegaDumpHelper dumpHelper = new MegaDumpHelper();
        private MegaWarningHelper warningHelper;
        private bool failToCreate;
        private readonly Queue<Action> pendingResponses = new Queue<Action>();
        private MegaLocationInputMode userLocationInputMode;
        private Queue<Exception> unhandledException = new Queue<Exception>();
        private Optional<FrameSupplementInput> frameSupplementInput;
        private Action<ProximityLocationResult> proximityLocationProvider;
        private Optional<string> spotVersionId;

        event Action<ProximityLocationResult> IFrameSupplementFilter.ProximityLocationProvider
        {
            add => proximityLocationProvider += value;
            remove => proximityLocationProvider -= value;
        }

#if EASYAR_ENABLE_MEGA
        private BlockHolderDiagnosticsWrapper blockHolderWrapper;

        /// <summary>
        /// <para xml:lang="en">Location data to be used when <see cref="LocationInputMode"/> == <see cref="MegaLocationInputMode.Simulator"/>.</para>
        /// <para xml:lang="zh"><see cref="LocationInputMode"/> == <see cref="MegaLocationInputMode.Simulator"/>时使用的位置数据。</para>
        /// </summary>
        public Optional<Location> SimulatorLocation { get; set; }

        /// <summary>
        /// <para xml:lang="en">The block holder which holds and manages blocks in the scene.</para>
        /// <para xml:lang="zh">持有Block的组件，在场景中持有并管理Block。</para>
        /// </summary>
        public BlockHolder BlockHolder { get; private set; }
#endif

        /// <summary>
        /// <para xml:lang="en">The Mega Landmark filter if <see cref="ServiceType"/> is <see cref="MegaApiType.Landmark"/>. You should manually call <see cref="MegaLandmarkFilterWrapper.FilterBySpotId"/> first. The tracker can only track after filter is found in this case.</para>
        /// <para xml:lang="zh"><see cref="ServiceType"/>为<see cref="MegaApiType.Landmark"/>时的Mega Landmark 过滤功能。开始时你需要手动调用<see cref="MegaLandmarkFilterWrapper.FilterBySpotId"/>。在Filter返回Found之前Tracker将不会开始跟踪。</para>
        /// </summary>
        public Optional<MegaLandmarkFilterWrapper> LandmarkFilter { get; private set; }

        /// <summary>
        /// <para xml:lang="en">EasyAR Mega service type. Set before session start.</para>
        /// <para xml:lang="zh">EasyAR Mega 服务类型。需要在Session启动前设置。</para>
        /// </summary>
        public MegaApiType ServiceType
        {
            get => serviceType;
            set
            {
                if (arSession) { return; }
                serviceType = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Service access source type. Set before session start.</para>
        /// <para xml:lang="zh">服务访问数据源类型。需要在Session启动前设置。</para>
        /// </summary>
        public MegaServiceAccessSourceType ServiceAccessSource
        {
            get => serviceAccessSource;
            set
            {
                if (arSession) { return; }
                serviceAccessSource = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Service access data. Set before session start. Do not set if <see cref="MegaServiceAccessSourceType.GlobalConfig"/> is in use.</para>
        /// <para xml:lang="zh">服务访问数据。需要在Session启动前设置。使用<see cref="MegaServiceAccessSourceType.GlobalConfig"/>无需设置。</para>
        /// </summary>
        public ExplicitAddressAccessData ServiceAccessData
        {
            get
            {
                switch (ServiceAccessSource)
                {
                    case MegaServiceAccessSourceType.GlobalConfig:
                        if (ServiceType == MegaApiType.Block)
                        {
                            return EasyARSettings.Instance ? EasyARSettings.Instance.GlobalMegaBlockLocalizationServiceConfig : null;
                        }
                        else if (ServiceType == MegaApiType.Landmark)
                        {
                            return EasyARSettings.Instance ? EasyARSettings.Instance.GlobalMegaLandmarkLocalizationServiceConfig : null;
                        }
                        throw new InvalidOperationException();
                    case MegaServiceAccessSourceType.APIKey:
                        return apiKeyAccessData;
                    case MegaServiceAccessSourceType.Token:
                        return tokenAccessData;
                    default:
                        throw new InvalidOperationException();
                }
            }
            set
            {
                if (arSession) { return; }
                switch (ServiceAccessSource)
                {
                    case MegaServiceAccessSourceType.GlobalConfig:
                        throw new ArgumentException($"{nameof(ServiceAccessData)} can not be set when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                    case MegaServiceAccessSourceType.APIKey:
                        if (value is not APIKeyAccessData apiKeyData)
                        {
                            throw new ArgumentException($"{nameof(ServiceAccessData)} must be {nameof(APIKeyAccessData)} when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                        }
                        apiKeyAccessData = apiKeyData;
                        break;
                    case MegaServiceAccessSourceType.Token:
                        if (value is not TokenAccessData tokenData)
                        {
                            throw new ArgumentException($"{nameof(ServiceAccessData)} must be {nameof(TokenAccessData)} when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                        }
                        tokenAccessData = tokenData;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Proximity location result.</para>
        /// <para xml:lang="zh">邻近位置结果。</para>
        /// </summary>
        public Optional<ProximityLocationResult> ProximityLocation
        {
            private get => proximityLocation;
            set
            {
                proximityLocation = value;
            }
        }

        public string RequestMessage
        {
            get => requestMessage;
            set
            {
                requestMessage = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Location input mode. Set before session start. Must set to <see cref="MegaLocationInputMode.Simulator"/> when debug remotely or running on PC otherwize Mega will fail to work. Set to <see cref="MegaLocationInputMode.Onsite"/> when running on site to reach the best performance.</para>
        /// <para xml:lang="zh">位置输入模式。需要在Session启动前设置。远程调试或电脑上运行必须设置成<see cref="MegaLocationInputMode.Simulator"/>，否则将无法使用。现场使用要设置成<see cref="MegaLocationInputMode.Onsite"/>以达到最佳效果。</para>
        /// </summary>
        public MegaLocationInputMode LocationInputMode
        {
            get => locationInputMode;
            set
            {
                if (localizer != null)
                {
                    Debug.LogError($"LocationInputMode can not be set after session start");
                    return;
                }
                if (value == MegaLocationInputMode.FramePlayer)
                {
                    Debug.LogError($"LocationInputMode can not be set to {value}");
                    return;
                }
                if (value == MegaLocationInputMode.Onsite && Application.isPlaying && Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && !SystemUtil.IsVisionOS())
                {
                    Debug.LogWarning($"{value} mode is not supported under {Application.platform}, reset to {MegaLocationInputMode.Simulator}.");
                    locationInputMode = MegaLocationInputMode.Simulator;
                    return;
                }
                locationInputMode = value;
            }
        }

        public Optional<MegaBlockPrior> BlockPrior
        {
            get => hasBlockPrior ? blockPrior : Optional<MegaBlockPrior>.Empty;
            set
            {
                hasBlockPrior = value.OnSome;
                blockPrior = value.ValueOrDefault(new());
            }
        }

        internal override bool IsAvailable =>
#if EASYAR_ENABLE_MEGA
            CloudLocalizer.isAvailable();
#else
            throw new Exception($"Package com.easyar.mega is required to use {nameof(CloudLocalizerFrameFilter)}");
#endif
        internal override int BufferRequirement => 0;

        private void Awake()
        {
#if EASYAR_ENABLE_MEGA
            BlockHolder = gameObject.GetComponent<BlockHolder>();
#endif
            locationManager = gameObject.AddComponent<LocationManager>();
            locationManager.enabled = false;
        }

        /// <summary>
        /// <para xml:lang="en">Enable/disable localizing when <see cref="ARSession"/> is running.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时启用/禁用定位。</para>
        /// </summary>
        private void OnEnable()
        {
            if (localizer != null && LocationInputMode == MegaLocationInputMode.Onsite)
            {
                locationManager.RequestLocationPermission((result) =>
                {
                    if (!result)
                    {
                        var error = "Location permission not granted";
                        if (arSession && arSession.Diagnostics)
                        {
                            arSession.Diagnostics.EnqueueError(error);
                        }
                        else
                        {
                            Debug.LogError(error);
                        }
                    }
                });
                locationManager.enabled = true;
            }
            accelerometer?.openWithSamplingPeriod(100);
            if (arSession) { arSession.SessionUpdate += SendResponse; }
        }

        private void OnDisable()
        {
            locationManager.enabled = false;
            if (arSession) { arSession.SessionUpdate -= SendResponse; }
            SendResponse();
            accelerometer?.close();
        }

        private void OnDestroy()
        {
            OnSessionStop();
        }

        /// <summary>
        /// <para xml:lang="en">Send localization request.</para>
        /// <para xml:lang="zh">发送定位请求。</para>
        /// </summary>
        public void Resolve(Action<MegaLocalizationResponse> callback)
        {
            Action<MegaLocalizationResponse> onResponse = (response) =>
            {
                dumpHelper.UpdateResponse(response);
                callback?.Invoke(response);
            };

            if (!enabled)
            {
                onResponse?.Invoke(new MegaLocalizationResponse("localizer disabled"));
                return;
            }
            if (!arSession || localizer == null)
            {
                onResponse?.Invoke(failToCreate ? new MegaLocalizationResponse("localizer fail to create") : new MegaLocalizationResponse("localizer not exist"));
                return;
            }

            if (proximityLocation.OnSome)
            {
                proximityLocationProvider?.Invoke(proximityLocation.Value);
            }

            var asyncFrame = arSession.AsyncCameraFrame;
            if (asyncFrame.OnNone)
            {
                onResponse?.Invoke(new MegaLocalizationResponse("NoFrame"));
                return;
            }

            using (var outputFrame = asyncFrame.Value)
            using (var iFrame = outputFrame.inputFrame())
            {
                if (iFrame.hasSpatialInformation() && iFrame.trackingStatus() == MotionTrackingStatus.NotTracking)
                {
                    onResponse?.Invoke(new MegaLocalizationResponse("not tracking"));
                    return;
                }

                warningHelper.DisplayWarningMessages(LocationInputMode);
                request.RequestTimeParameters = new MegaRequestTimeParameters
                {
                    RequestInterval = -1,
                    Timeout = RequestTimeout,
                };
                request.RequestMessage = RequestMessage;
                DeviceAuxiliaryInfo deviceAuxiliary = null;
                if (LocationInputMode == MegaLocationInputMode.FramePlayer)
                {
                    if (frameSupplementInput.OnSome)
                    {
                        deviceAuxiliary = DeviceAuxiliaryInfoFactory.Create(frameSupplementInput.Value);
                    }
                    else
                    {
                        deviceAuxiliary = DeviceAuxiliaryInfo.create();
                    }
                }
                else if (LocationInputMode == MegaLocationInputMode.Simulator)
                {
                    var sLocation = Optional<LocationResult>.Empty;
#if EASYAR_ENABLE_MEGA
                    if (SimulatorLocation.OnSome)
                    {
                        sLocation = new LocationResult
                        {
                            latitude = SimulatorLocation.Value.latitude,
                            longitude = SimulatorLocation.Value.longitude,
                        };
                    }
#endif
                    deviceAuxiliary = DeviceAuxiliaryInfoFactory.Create(accelerometer, sLocation, proximityLocation);
                }
                else
                {
                    deviceAuxiliary = DeviceAuxiliaryInfoFactory.Create(accelerometer, locationManager, proximityLocation);
                }
                using (deviceAuxiliary)
                {
                    if (BlockPrior.OnSome)
                    {
                        deviceAuxiliary.setBlockPrior(BlockPrior.Value.ToBlockPriorResult());
                    }
                    var success = Resolve(iFrame, deviceAuxiliary, request, (response) =>
                    {
                        pendingResponses.Enqueue(() => onResponse?.Invoke(response));
                    });
                    if (!success)
                    {
                        onResponse?.Invoke(new MegaLocalizationResponse("onging request exist"));
                    }
                }
            }
        }

#if EASYAR_ENABLE_MEGA
        /// <summary>
        /// <para xml:lang="en">Switches remote end point.</para>
        /// <para xml:lang="zh">切换远端端点。</para>
        /// </summary>
        public void SwitchEndPoint(ExplicitAddressAccessData config, BlockRootController root)
        {
            if (localizer == null) { return; }
            if (config == null) { return; }
            if ((config is not APIKeyAccessData) && (config is not TokenAccessData))
            {
                throw new InvalidOperationException($"Unsupported access data type: {config.GetType().Name}");
            }
            if (!root)
            {
                throw new Exception($"Fail to switch remote end point: block root is null!");
            }
            if (BlockHolder.BlockRoot == root)
            {
                throw new Exception($"Fail to switch remote end point: block root not changed!");
            }

            BlockHolder.OnSwitchEndPoint();
            currentEndPoint = config is APIKeyAccessData apiKey ? new APIKeyAccessData
            {
                AppID = apiKey.AppID.Trim(),
                ServerAddress = apiKey.ServerAddress.Trim(),
                APIKey = apiKey.APIKey.Trim(),
                APISecret = apiKey.APISecret.Trim(),
            } as ExplicitAddressAccessData : (config is TokenAccessData token ? new TokenAccessData
            {
                AppID = token.AppID.Trim(),
                ServerAddress = token.ServerAddress.Trim(),
                Token = token.Token.Trim(),
            } as ExplicitAddressAccessData : null);

            try
            {
                localizer.Dispose();
                localizer = null;
                if (currentEndPoint is APIKeyAccessData apiKeyAccess)
                {
                    localizer = CloudLocalizer.create(apiKeyAccess.ServerAddress, apiKeyAccess.APIKey, apiKeyAccess.APISecret, apiKeyAccess.AppID);
                }
                else if (currentEndPoint is TokenAccessData tokenAccess)
                {
                    localizer = CloudLocalizer.createWithToken(tokenAccess.ServerAddress, tokenAccess.Token, tokenAccess.AppID);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported access data type: {currentEndPoint.GetType().Name}");
                }
                BlockHolder.BlockRoot = root;
                failToCreate = false;
            }
            catch (ArgumentNullException)
            {
                failToCreate = true;
                throw new Exception($"Fail to switch remote end point, check logs for detials.");
            }
            if (LandmarkFilter.OnSome)
            {
                LandmarkFilter.Value.SwitchEndPoint(currentEndPoint);
            }
        }
#endif

        /// <summary>
        /// <para xml:lang="en">Updates API Token.</para>
        /// <para xml:lang="zh">更新API Token。</para>
        /// </summary>
        public void UpdateToken(string token)
        {
            localizer?.updateToken(token);
            LandmarkFilter.ValueOrDefault(null)?.UpdateToken(token);
        }

        internal override void OnSessionStart(ARSession session)
        {
            arSession = session;
#if EASYAR_ENABLE_MEGA
            blockHolderWrapper = new BlockHolderDiagnosticsWrapper(BlockHolder);
#endif

            userLocationInputMode = LocationInputMode;

            var access = ServiceAccessData;
            var accessTrimed = access is APIKeyAccessData apiKey ? new APIKeyAccessData
            {
                AppID = apiKey.AppID.Trim(),
                ServerAddress = apiKey.ServerAddress.Trim(),
                APIKey = apiKey.APIKey.Trim(),
                APISecret = apiKey.APISecret.Trim(),
            } as ExplicitAddressAccessData : (access is TokenAccessData token ? new TokenAccessData
            {
                AppID = token.AppID.Trim(),
                ServerAddress = token.ServerAddress.Trim(),
                Token = token.Token.Trim(),
            } as ExplicitAddressAccessData : null);

            if (accessTrimed == null
                || string.IsNullOrEmpty(accessTrimed.AppID)
                || string.IsNullOrEmpty(accessTrimed.ServerAddress)
                || (accessTrimed is APIKeyAccessData apiKeyTrimed && (string.IsNullOrEmpty(apiKeyTrimed.APIKey) || string.IsNullOrEmpty(apiKeyTrimed.APISecret)))
                || (accessTrimed is TokenAccessData tokenTrimed && (string.IsNullOrEmpty(tokenTrimed.Token)))
                )
            {
                throw new DiagnosticsMessageException(
                    "Service config (for authentication) NOT set, please set" + Environment.NewLine +
                    "globally in EasyAR Settings (Project Settings > EasyAR) or" + Environment.NewLine +
                    "locally on <CloudLocalizerFrameFilter> Component." + Environment.NewLine +
                    "Get from EasyAR Develop Center.");
            }
            currentEndPoint = accessTrimed;

            try
            {
                if (currentEndPoint is APIKeyAccessData apiKeyAccess)
                {
                    localizer = CloudLocalizer.create(apiKeyAccess.ServerAddress, apiKeyAccess.APIKey, apiKeyAccess.APISecret, apiKeyAccess.AppID);
                }
                else if (currentEndPoint is TokenAccessData tokenAccess)
                {
                    localizer = CloudLocalizer.createWithToken(tokenAccess.ServerAddress, tokenAccess.Token, tokenAccess.AppID);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported access data type: {currentEndPoint.GetType().Name}");
                }
                failToCreate = false;
            }
            catch (ArgumentNullException)
            {
                failToCreate = true;
                throw new DiagnosticsMessageException($"Fail to create {nameof(CloudLocalizer)}, check logs for detials.");
            }
            if (ServiceType == MegaApiType.Landmark)
            {
                LandmarkFilter = new MegaLandmarkFilterWrapper(currentEndPoint, () =>
                {
                    if (locationInputMode == MegaLocationInputMode.FramePlayer)
                    {
                        if (frameSupplementInput.OnNone) { return Optional<LocationResult>.Empty; }
                        return frameSupplementInput.Value.Location;
                    }
#if EASYAR_ENABLE_MEGA
                    else if (LocationInputMode == MegaLocationInputMode.Simulator)
                    {
                        if (SimulatorLocation.OnNone) { return Optional<LocationResult>.Empty; }
                        return new LocationResult
                        {
                            latitude = SimulatorLocation.Value.latitude,
                            longitude = SimulatorLocation.Value.longitude,
                        };
                    }
#endif
                    else
                    {
                        if (!locationManager || !locationManager.CurrentResult.HasValue) { return Optional<LocationResult>.Empty; }
                        var loc = locationManager.CurrentResult.Value;
                        return new LocationResult(loc.latitude, loc.longitude, loc.altitude, loc.horizontalAccuracy, loc.verticalAccuracy, true, true, true);
                    }
                }, () => RequestTimeout,
                response =>
                {
                    if (response.Status == MegaLandmarkFilterStatus.Found)
                    {
                        spotVersionId = response.SpotVersionId;
                    }
                    dumpHelper.UpdateFilterResponse(response);
                }
                );
            }

            localizer.setMegaApiType(ServiceType);

            warningHelper = new MegaWarningHelper(arSession.Diagnostics);

            if (arSession.Assembly.FrameSource is FramePlayer player)
            {
                locationInputMode = MegaLocationInputMode.FramePlayer; // set to FramePlayer
            }
            else
            {
                if (LocationInputMode == MegaLocationInputMode.Onsite && Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && !SystemUtil.IsVisionOS())
                {
                    Debug.LogWarning($"{LocationInputMode} mode is not supported under {Application.platform}, reset to {MegaLocationInputMode.Simulator}.");
                    locationInputMode = MegaLocationInputMode.Simulator;
                }

                if (!arSession.Assembly.FrameSource.IsHMD)
                {
                    accelerometer = new Accelerometer();
                    if (!accelerometer.isAvailable())
                    {
                        accelerometer.Dispose();
                        accelerometer = null;
                    }
                }
            }

            if (enabled)
            {
                OnEnable();
            }
        }

        internal override void OnSessionStop()
        {
            OnDisable();
            StopAllCoroutines();

            localizer?.Dispose();
            localizer = null;
            accelerometer?.Dispose();
            accelerometer = null;
            arSession = null;
            locationInputMode = userLocationInputMode;
            LandmarkFilter = null;
        }

        Optional<Tuple<GameObject, Pose>> ICenterTransform.TryGetCenter(GameObject center) =>
#if EASYAR_ENABLE_MEGA
            BlockHolder.TryGetCenter(center);
#else
            null;
#endif

        void ICenterTransform.UpdateTransform(GameObject center, Pose centerPose)
#if EASYAR_ENABLE_MEGA
            => BlockHolder.UpdateTransform(center, centerPose);
#else
            { }
#endif

        void IFrameSupplementFilter.Connect(VideoInputFramePlayer player, bool isHMD)
        {
            frameSupplementInput = new FrameSupplementInput();
            if (!isHMD)
            {
                using (var souce = player.accelerometerResultSource())
                {
                    souce.setHandler((Action<AccelerometerResult>)((data) =>
                    {
                        if (frameSupplementInput.OnSome) { frameSupplementInput.Value.Accelerometer = data; }
                    }));
                }
            }
            using (var souce = player.locationResultSource())
            {
                souce.setHandler((Action<LocationResult>)((data) =>
                {
                    if (frameSupplementInput.OnSome) { frameSupplementInput.Value.Location = data; }
                }));
            }
            using (var souce = player.proximityLocationResultSource())
            {
                souce.setHandler((Action<ProximityLocationResult>)((data) =>
                {
                    if (frameSupplementInput.OnSome) { frameSupplementInput.Value.ProximityLocation = data; }
                }));
            }
        }

        void IFrameSupplementFilter.Disconnect(VideoInputFramePlayer player)
        {
            frameSupplementInput = null;
        }

        void IFrameSupplementFilter.Connect(FrameSupplementPlayer player, bool isHMD)
        {
            frameSupplementInput = new FrameSupplementInput();
            if (!isHMD)
            {
                player.AccelerometerOutput += OnFramePlayerAccelerometerResult;
            }
            player.LocationOutput += OnFramePlayerLocationResult;
        }

        void IFrameSupplementFilter.Disconnect(FrameSupplementPlayer player)
        {
            player.AccelerometerOutput -= OnFramePlayerAccelerometerResult;
            player.LocationOutput -= OnFramePlayerLocationResult;
            frameSupplementInput = null;
        }

#if EASYAR_ENABLE_MEGA
        internal override string DumpLite()
        {
            if (localizer == null || !enabled) { return null; }
            var data = $"Mega {ServiceType}: {LocationInputMode}" +
                (LocationInputMode == MegaLocationInputMode.Simulator ? $" {SimulatorLocation}" : string.Empty) +
                $", ({RequestTimeout})" + Environment.NewLine;
            data += $"- Block Holder: {BlockHolder.BlockRootSource}, {BlockHolder.MultiBlock}" + Environment.NewLine;
            data += $"- Server: {MegaDumpHelper.ServerSuffix(currentEndPoint.ServerAddress)}, {currentEndPoint.AppID}" + Environment.NewLine;
            return data + dumpHelper.DumpResponse(arSession && arSession.Assembly != null ? arSession.Assembly.Camera : null, BlockHolder.BlockRoot);
        }
#endif

        private bool Resolve(InputFrame iFrame, DeviceAuxiliaryInfo deviceAuxiliaryInfo, ResolveRequest request, Action<MegaLocalizationResponse> callback)
        {
            if (localizer == null) { return false; }
            if (request.IsInResolve) { return false; }
            if ((Time.realtimeSinceStartup - request.StartTime) * 1000 < request.RequestTimeParameters.RequestInterval) { return false; }

            request.IsInResolve = true;
            request.StartTime = Time.realtimeSinceStartup;

            var timestamp = iFrame.hasTemporalInformation() ? iFrame.timestamp() : Time.realtimeSinceStartup;
            if (ServiceType == MegaApiType.Landmark)
            {
                if (spotVersionId.OnNone)
                {
                    request.IsInResolve = false;
                    callback?.Invoke(new MegaLocalizationResponse
                    {
                        Timestamp = timestamp,
                        Status = MegaTrackerLocalizationStatus.MissingSpotVersionId,
                    });
                    return true;
                }
            }
            Action<CloudLocalizerResult> resolveCallback = (response) =>
            {
                request.IsInResolve = false;

                MegaTrackerLocalizationStatus status;
                switch (response.localizeStatus())
                {
                    case CloudLocalizerStatus.UnknownError:
                        status = MegaTrackerLocalizationStatus.UnknownError;
                        break;
                    case CloudLocalizerStatus.Found:
                        status = MegaTrackerLocalizationStatus.Found;
                        break;
                    case CloudLocalizerStatus.NotFound:
                        status = MegaTrackerLocalizationStatus.NotFound;
                        break;
                    case CloudLocalizerStatus.RequestIntervalTooLow:
                        status = MegaTrackerLocalizationStatus.RequestIntervalTooLow;
                        break;
                    case CloudLocalizerStatus.RequestTimeout:
                        status = MegaTrackerLocalizationStatus.RequestTimeout;
                        break;
                    case CloudLocalizerStatus.QpsLimitExceeded:
                        status = MegaTrackerLocalizationStatus.QpsLimitExceeded;
                        break;
                    case CloudLocalizerStatus.WakingUp:
                        status = MegaTrackerLocalizationStatus.WakingUp;
                        break;
                    case CloudLocalizerStatus.MissingSpotVersionId:
                        status = MegaTrackerLocalizationStatus.MissingSpotVersionId;
                        break;
                    default:
                        throw new Exception("Unknown status: " + response.localizeStatus());
                }
                if (status == MegaTrackerLocalizationStatus.UnknownError)
                {
                    Debug.LogError(status + Environment.NewLine + response.exceptionInfo());
                }
                else if (status != MegaTrackerLocalizationStatus.Found && status != MegaTrackerLocalizationStatus.NotFound && status != MegaTrackerLocalizationStatus.MissingSpotVersionId)
                {
                    Debug.LogWarning(status);
                }
                var responseLite = new MegaLocalizationResponse
                {
                    Timestamp = timestamp,
                    Status = status,
                    SpotVersionId = response.spotVersionId(),
                    ServerCalculationDuration = response.serverCalculationDuration(),
                    ServerResponseDuration = response.serverResponseDuration(),
                    ErrorMessage = string.IsNullOrEmpty(response.exceptionInfo()) ? null : response.exceptionInfo(),
                    ExtraInfo = response.extraInfo(),
                };

#if EASYAR_ENABLE_MEGA
                try
                {
                    var blocks = new List<BlockController>();
                    var blocksInfo = new List<Tuple<BlockController.BlockInfo, BlockHolder.PoseSet>>();
                    foreach (var instance in response.blockInstances())
                    {
                        using (instance)
                        {
                            var blockInfo = new BlockController.BlockInfo
                            {
                                ID = instance.blockId(),
                                Name = instance.name(),
                            };
                            var block = blockHolderWrapper.OnLocalize(blockInfo);
                            if (block)
                            {
                                blocks.Add(block);
                                blocksInfo.Add(Tuple.Create(blockInfo, new BlockHolder.PoseSet
                                {
                                    BlockToCamera = instance.pose().ToUnityPose(),
                                    CameraToVIOOrigin = default(Pose?)
                                }));
                            }
                        }
                    }
                    blockHolderWrapper.OnTrack(blocksInfo);
                    responseLite.Blocks = blocks;
                }
                catch (DiagnosticsMessageException e)
                {
                    unhandledException.Enqueue(e);
                    throw e;
                }
#endif
                callback?.Invoke(responseLite);
            };
            if (ServiceType == MegaApiType.Block)
            {
                localizer.resolve(iFrame, request.RequestMessage ?? string.Empty, deviceAuxiliaryInfo, request.RequestTimeParameters.Timeout, EasyARController.Scheduler, resolveCallback);
            }
            else if (ServiceType == MegaApiType.Landmark)
            {
                localizer.resolveLandmark(iFrame, request.RequestMessage ?? string.Empty, deviceAuxiliaryInfo, spotVersionId.Value, request.RequestTimeParameters.Timeout, EasyARController.Scheduler, resolveCallback);
            }
            return true;
        }

        private void OnFramePlayerAccelerometerResult(FrameSupplement.DeviceInput.AccelerometerResult data)
        {
            if (frameSupplementInput.OnNone) { return; }
            frameSupplementInput.Value.Accelerometer = new AccelerometerResult(data.x, data.y, data.z, data.timestamp);
        }

        private void OnFramePlayerLocationResult(FrameSupplement.DeviceInput.LocationResult data)
        {
            if (frameSupplementInput.OnNone) { return; }
            frameSupplementInput.Value.Location = new LocationResult(data.latitude, data.longitude, data.altitude, data.horizontalAccuracy, data.verticalAccuracy, true, true, true);
        }

        private void SendResponse()
        {
            if (unhandledException.Count > 0)
            {
                var e = unhandledException.Peek();
                unhandledException.Dequeue();
                throw e;
            }

            // response is received from Scheduler, which is triggered at the beginning of update loop
            while (pendingResponses.Count > 0)
            {
                pendingResponses.Dequeue()?.Invoke();
            }
        }

        private class ResolveRequest
        {
            public bool IsInResolve;
            public float StartTime;
            public MegaRequestTimeParameters RequestTimeParameters;
            public string RequestMessage;
        }
    }
}
