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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="MegaTracker"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="MegaTracker"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
#if EASYAR_ENABLE_MEGA
    [RequireComponent(typeof(BlockHolder))]
#endif
    public class MegaTrackerFrameFilter : FrameFilter, FrameFilter.IInputFrameSink, FrameFilter.IOutputFrameSource, FrameFilter.ICenterTransform, IFrameSupplementFilter
    {
        private MegaTracker tracker = default;
        private LocationResultSink locationResultSink;
        private AccelerometerResultSink accelerometerResultSink;
        private ProximityLocationResultSink proximityLocationResultSink;

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
        MegaInputFrameLevel minInputFrameLevel = MegaInputFrameLevel.SixDof;
        [HideInInspector, SerializeField]
        private MegaRequestTimeParameters requestTimeParameters = new MegaRequestTimeParameters();
        [HideInInspector, SerializeField]
        private MegaBlockPrior blockPrior = new();
        [HideInInspector, SerializeField]
        private bool hasBlockPrior;
        private bool enableLocalization = true;
        private bool enableStatelessLocalization;
        private (Optional<float> x, Optional<float> y, Optional<float> z) cameraPositionOverride = new();
        private string requestMessage;
        private Optional<ProximityLocationResult> proximityLocation;

        private ARSession arSession;
        private ExplicitAddressAccessData currentEndPoint;
        private Accelerometer accelerometer;
        private LocationManager locationManager;

        private MegaDumpHelper dumpHelper = new MegaDumpHelper();
        private MegaWarningHelper warningHelper;
        private readonly Queue<MegaLocalizationResponse> pendingResponses = new Queue<MegaLocalizationResponse>();
        private bool isEditorSimulation;
        private MegaLocationInputMode userLocationInputMode;
        private Action<ProximityLocationResult> proximityLocationProvider;
        private object _framePlayerLocationLock = new();
        private Optional<LocationResult> _framePlayerLocation;

        /// <summary>
        /// <para xml:lang="en">CloudLocalization service callback event. This event is usually used for debug, objects transform and status in the scene does not change with event data in the same time the event is triggered.</para>
        /// <para xml:lang="zh">服务定位返回事件。该事件通常用作debug，事件发生时场景中物体的位置和状态与事件中的数据无对应关系。</para>
        /// </summary>
        public event Action<MegaLocalizationResponse> LocalizationRespond;

        event Action<ProximityLocationResult> IFrameSupplementFilter.ProximityLocationProvider
        {
            add => proximityLocationProvider += value;
            remove => proximityLocationProvider -= value;
        }

#if EASYAR_ENABLE_MEGA
        private BlockHolderDiagnosticsWrapper blockHolderWrapper;
        private Optional<Location> simulatorLocation;
        private Queue<Exception> unhandledException = new Queue<Exception>();

        /// <summary>
        /// <para xml:lang="en">Location data to be used when <see cref="LocationInputMode"/> == <see cref="MegaLocationInputMode.Simulator"/>.</para>
        /// <para xml:lang="zh"><see cref="LocationInputMode"/> == <see cref="MegaLocationInputMode.Simulator"/>时使用的位置数据。</para>
        /// </summary>
        public Optional<Location> SimulatorLocation
        {
            get => simulatorLocation;
            set
            {
                if (tracker != null && value.OnNone)
                {
                    Debug.LogError($"{value} unacceptable after session start");
                    return;
                }
                if (LocationInputMode == MegaLocationInputMode.Simulator)
                {
                    locationResultSink?.handle(new LocationResult
                    {
                        latitude = value.Value.latitude,
                        longitude = value.Value.longitude,
                    });
                }
                simulatorLocation = value;
            }
        }

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
        /// <para xml:lang="en">Request time parameters.</para>
        /// <para xml:lang="zh">请求时间参数。</para>
        /// </summary>
        public MegaRequestTimeParameters RequestTimeParameters
        {
            get => requestTimeParameters;
            set
            {
                requestTimeParameters = value;
                tracker?.setRequestTimeParameters(value.Timeout, value.RequestInterval);
            }
        }

        /// <summary>
        /// <para xml:lang="en">Whether to enable localzation. If disabled, localization result from server response will not be in the output. Defaults to true.</para>
        /// <para xml:lang="en">It may affect the tracking effect. Do not use it unless you consult with EasyAR and fully understand the implications.</para>
        /// <para xml:lang="zh">是否启用定位。不启用定位时，服务器返回的定位结果不会输出。默认为true。</para>
        /// <para xml:lang="zh">会影响跟踪效果。除非向EasyAR咨询并明确理解影响，否则不要使用。</para>
        /// </summary>
        public bool EnableLocalization
        {
            get => enableLocalization;
            set
            {
                enableLocalization = value;
                tracker?.setEnableLocalization(value);
            }
        }

        /// <summary>
        /// <para xml:lang="en">Whether to enable stateless localzation. If enabled, previous result will not be used in localization. Defaults to false.</para>
        /// <para xml:lang="en">It may affect the tracking effect. Do not use it unless you consult with EasyAR and fully understand the implications.</para>
        /// <para xml:lang="zh">是否启用无状态定位。启用无状态定位时，定位时不会使用上一次定位结果。默认为false。</para>
        /// <para xml:lang="zh">会影响跟踪效果。除非向EasyAR咨询并明确理解影响，否则不要使用。</para>
        /// </summary>
        public bool EnableStatelessLocalization
        {
            get => enableStatelessLocalization;
            set
            {
                enableStatelessLocalization = value;
                tracker?.setEnableStatelessLocalization(value);
            }
        }

        /// <summary>
        /// <para xml:lang="en">The override value for camera position from map block. When set, MegaTracker will output override value instead of the position in input frame. This option can be utilized to improve output pose precision when there is known map info but limited sensor capability.</para>
        /// <para xml:lang="zh">相机位置相对于地图block的覆盖值。设置之后，MegaTracker会输出覆盖值，而不使用输入帧数据中的对应坐标。可用于在已知地图信息且传感器有限的情况下提高输出位姿的精度。</para>
        /// </summary>
        public (Optional<float> x, Optional<float> y, Optional<float> z) CameraPositionOverride
        {
            get => cameraPositionOverride;
            set
            {
                cameraPositionOverride = value;
                tracker?.setCameraPositionOverride(value.x, value.y, value.z);
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
                if (proximityLocation.OnSome)
                {
                    HandleProximityLocation(proximityLocation.Value);
                }
            }
        }

        public string RequestMessage
        {
            get => requestMessage;
            set
            {
                requestMessage = value;
                tracker?.setRequestMessage(value ?? string.Empty);
            }
        }

        /// <summary>
        /// <para xml:lang="en">Minimum <see cref="MegaInputFrameLevel"/> allowed for input frame. Session will fail to start if frame source can give only input frame with lower dimension <see cref="CameraTransformType"/>. Set before session start.</para>
        /// <para xml:lang="zh">输入帧最小允许的<see cref="MegaInputFrameLevel"/>。如果frame source只能给出维度更低<see cref="CameraTransformType"/>的数据，Session会启动失败。需要在Session启动前设置。</para>
        /// </summary>
        public MegaInputFrameLevel MinInputFrameLevel
        {
            get => minInputFrameLevel;
            set
            {
                if (tracker != null)
                {
                    Debug.LogError($"MinInputFrameLevel can not be set after session start");
                    return;
                }
                minInputFrameLevel = value;
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
                if (tracker != null)
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
                BindBlocks();
            }
        }

        internal override bool IsAvailable =>
#if EASYAR_ENABLE_MEGA
            MegaTracker.isAvailable();
#else
            throw new Exception($"Package com.easyar.mega is required to use {nameof(MegaTrackerFrameFilter)}");
#endif
        internal override int BufferRequirement => tracker.bufferRequirement();

        private Optional<LocationResult> FramePlayerLocation
        {
            get
            {
                lock (_framePlayerLocationLock)
                {
                    return _framePlayerLocation;
                }
            }
            set
            {
                lock (_framePlayerLocationLock)
                {
                    _framePlayerLocation = value;
                }
            }
        }

        private void Awake()
        {
#if EASYAR_ENABLE_MEGA
            BlockHolder = gameObject.GetComponent<BlockHolder>();
            if (!BlockHolder)
            {
                BlockHolder = gameObject.AddComponent<BlockHolder>();
            }
#endif
            locationManager = gameObject.AddComponent<LocationManager>();
            locationManager.enabled = false;
        }

        /// <summary>
        /// <para xml:lang="en">Start/Stop tracking when <see cref="ARSession"/> is running. Tracking will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止跟踪。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始跟踪。</para>
        /// </summary>
        private void OnEnable()
        {
            if (tracker != null && LocationInputMode == MegaLocationInputMode.Onsite)
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
            tracker?.start();
        }

        private void OnDisable()
        {
            locationManager.enabled = false;
            if (arSession) { arSession.SessionUpdate -= SendResponse; }
            SendResponse();
            tracker?.stop();
            accelerometer?.close();
        }

        private void OnDestroy()
        {
            OnSessionStop();
        }

#if EASYAR_ENABLE_MEGA
        /// <summary>
        /// <para xml:lang="en">Switches remote end point.</para>
        /// <para xml:lang="zh">切换远端端点。</para>
        /// </summary>
        public void SwitchEndPoint(ExplicitAddressAccessData config, BlockRootController root)
        {
            if (tracker == null) { return; }
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
                if (currentEndPoint is APIKeyAccessData apiKeyAccess)
                {
                    tracker.switchEndPoint(apiKeyAccess.ServerAddress, apiKeyAccess.APIKey, apiKeyAccess.APISecret, apiKeyAccess.AppID);
                }
                else if (currentEndPoint is TokenAccessData tokenAccess)
                {
                    tracker.switchEndPointWithToken(tokenAccess.ServerAddress, tokenAccess.Token, tokenAccess.AppID);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported access data type: {currentEndPoint.GetType().Name}");
                }
                BlockHolder.BlockRoot = root;
            }
            catch (ArgumentNullException)
            {
                throw new Exception($"Fail to switch remote end point, check logs for detials.");
            }
            if (LandmarkFilter.OnSome)
            {
                LandmarkFilter.Value.SwitchEndPoint(currentEndPoint);
            }
        }
#endif

        /// <summary>
        /// <para xml:lang="en">Reset tracker.</para>
        /// <para xml:lang="zh">重置tracker。</para>
        /// </summary>
        public void ResetTracker()
        {
            tracker?.reset();
        }

        /// <summary>
        /// <para xml:lang="en">Updates API Token.</para>
        /// <para xml:lang="zh">更新API Token。</para>
        /// </summary>
        public void UpdateToken(string token)
        {
            tracker?.updateToken(token);
            LandmarkFilter.ValueOrDefault(null)?.UpdateToken(token);
        }

        internal override void OnSessionStart(ARSession session)
        {
            arSession = session;
            isEditorSimulation = Application.isEditor && !(arSession.Assembly.FrameSource is FramePlayer);
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
                    "locally on <MegaTrackerFrameFilter> Component." + Environment.NewLine +
                    "Get from EasyAR Develop Center.");
            }
            currentEndPoint = accessTrimed;

            try
            {
                if (currentEndPoint is APIKeyAccessData apiKeyAccess)
                {
                    tracker = MegaTracker.create(apiKeyAccess.ServerAddress, apiKeyAccess.APIKey, apiKeyAccess.APISecret, apiKeyAccess.AppID);
                }
                else if (currentEndPoint is TokenAccessData tokenAccess)
                {
                    tracker = MegaTracker.createWithToken(tokenAccess.ServerAddress, tokenAccess.Token, tokenAccess.AppID);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported access data type: {currentEndPoint.GetType().Name}");
                }
            }
            catch (ArgumentNullException)
            {
                throw new DiagnosticsMessageException($"Fail to create {nameof(MegaTracker)}, check logs for detials.");
            }
            if (ServiceType == MegaApiType.Landmark)
            {
                LandmarkFilter = new MegaLandmarkFilterWrapper(currentEndPoint, () =>
                {
                    if (locationInputMode == MegaLocationInputMode.FramePlayer)
                    {
                        return FramePlayerLocation;
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
                }, () => RequestTimeParameters.Timeout,
                response =>
                {
                    if (response.Status == MegaLandmarkFilterStatus.Found)
                    {
                        tracker?.setSpotVersionId(response.SpotVersionId);
                    }
                    dumpHelper.UpdateFilterResponse(response);
                }
                );
            }

            tracker.setMegaApiType(ServiceType);
            if (BlockPrior.OnSome) { BindBlocks(); }
            locationResultSink = tracker.locationResultSink();
            accelerometerResultSink = tracker.accelerometerResultSink();
            if (ProximityLocation.OnSome)
            {
                HandleProximityLocation(ProximityLocation.Value);
            }
            if (!tracker.setResultAsyncMode(arSession.Assembly.IsASync))
            {
                EasyARController.LicenseValidation.OnResultSyncModeFail(arSession.Assembly.FrameSource.GetType());
            }
            tracker.setRequestTimeParameters(RequestTimeParameters.Timeout, RequestTimeParameters.RequestInterval);
            tracker.setEnableLocalization(enableLocalization);
            tracker.setEnableStatelessLocalization(enableStatelessLocalization);
            tracker.setCameraPositionOverride(cameraPositionOverride.x, cameraPositionOverride.y, cameraPositionOverride.z);
            tracker.setRequestMessage(RequestMessage ?? string.Empty);
            tracker.setLocalizationCallback(EasyARController.Scheduler, (Action<MegaTrackerLocalizationResponse>)((response) =>
            {
                var status = response.status();
                if (status == MegaTrackerLocalizationStatus.UnknownError)
                {
                    Debug.LogError(status + Environment.NewLine + response.errorMessage());
                }
                else if (status != MegaTrackerLocalizationStatus.Found && status != MegaTrackerLocalizationStatus.NotFound && status != MegaTrackerLocalizationStatus.MissingSpotVersionId)
                {
                    Debug.LogWarning(status);
                }
                var timestamp = 0.0;
                using (var iFrame = response.inputFrame())
                {
                    if (iFrame.hasTemporalInformation())
                    {
                        timestamp = iFrame.timestamp();
                    }
                }
                var responseLite = new MegaLocalizationResponse
                {
                    Timestamp = timestamp,
                    Status = response.status(),
                    SpotVersionId = response.spotVersionId(),
                    ServerCalculationDuration = response.serverCalculationDuration(),
                    ServerResponseDuration = response.serverResponseDuration(),
                    ErrorMessage = response.errorMessage(),
                    ExtraInfo = response.extraInfo(),
                };

#if EASYAR_ENABLE_MEGA
                try
                {
                    var blocks = new List<BlockController>();
                    foreach (var instance in response.instances())
                    {
                        using (instance)
                        {
                            if (instance.appId() != currentEndPoint.AppID) { continue; }

                            var block = blockHolderWrapper.OnLocalize(new BlockController.BlockInfo
                            {
                                ID = instance.blockId(),
                                Name = instance.name(),
                            });
                            if (block)
                            {
                                blocks.Add(block);
                            }
                        }
                    }
                    responseLite.Blocks = blocks;
                }
                catch (DiagnosticsMessageException e)
                {
                    unhandledException.Enqueue(e);
                    throw e;
                }
#endif
                dumpHelper.UpdateResponse(responseLite);
                pendingResponses.Enqueue(responseLite);
            }));

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
                    accelerometer?.output().connect(accelerometerResultSink);
                }
                if (LocationInputMode == MegaLocationInputMode.Onsite)
                {
                    StartCoroutine(locationManager.HandleLocation(locationResultSink));
                }
                else
                {
#if EASYAR_ENABLE_MEGA
                    if (SimulatorLocation.OnSome)
                    {
                        locationResultSink?.handle(new LocationResult
                        {
                            latitude = SimulatorLocation.Value.latitude,
                            longitude = SimulatorLocation.Value.longitude,
                        });
                    }
#endif
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

            accelerometerResultSink?.Dispose();
            accelerometerResultSink = null;
            locationResultSink?.Dispose();
            locationResultSink = null;
            proximityLocationResultSink?.Dispose();
            proximityLocationResultSink = null;
            tracker?.close();
            tracker?.Dispose();
            tracker = null;
            accelerometer?.Dispose();
            accelerometer = null;
            arSession = null;
            locationInputMode = userLocationInputMode;
            LandmarkFilter = null;
        }

        InputFrameSink IInputFrameSink.InputFrameSink() => tracker?.inputFrameSink();
        OutputFrameSource IOutputFrameSource.OutputFrameSource() => tracker?.outputFrameSource();

        void IOutputFrameSource.OnResult(Optional<FrameFilterResult> frameFilterResult, Optional<MotionInputData> motion)
        {
#if EASYAR_ENABLE_MEGA
            if (!CheckSession()) { return; }

            if (frameFilterResult.OnNone)
            {
                blockHolderWrapper.OnTrack(null);
                return;
            }
            OnResult(frameFilterResult.Value as MegaTrackerResult, motion);
#endif
        }

        void IOutputFrameSource.RetrieveSyncResults(Optional<MotionInputData> motion)
        {
#if EASYAR_ENABLE_MEGA
            if (!CheckSession()) { return; }

            if (tracker == null || motion.OnNone || !EasyARController.IsReady)
            {
                blockHolderWrapper.OnTrack(null);
                return;
            }
            var resultSync = tracker.getSyncResult(motion.Value);
            if (resultSync.OnNone)
            {
                blockHolderWrapper.OnTrack(null);
                return;
            }

            using (var result = resultSync.Value)
            {
                OnResult(result, motion);
            }
#endif
        }

        private bool CheckSession()
        {
#if EASYAR_ENABLE_MEGA
            if (unhandledException.Count > 0)
            {
                var e = unhandledException.Peek();
                unhandledException.Dequeue();
                throw e;
            }
            if (enabled && arSession)
            {
                warningHelper.DisplayWarningMessages(isEditorSimulation, MinInputFrameLevel, arSession.CameraTransformType, LocationInputMode);
                if (!isEditorSimulation && arSession.CameraTransformType.OnSome && arSession.CameraTransformType.Value.ToMegaInputFrameLevel() < MinInputFrameLevel)
                {
                    blockHolderWrapper.OnTrack(null);
                    arSession.Diagnostics.EnqueueError($"Mega running fail: current input frame level ({arSession.CameraTransformType}) < minimum input frame level set by the app developer ({MinInputFrameLevel}).");
                    enabled = false;
                    return false;
                }
            }
#endif
            return true;
        }

        private void OnResult(MegaTrackerResult result, Optional<MotionInputData> motion)
        {
#if EASYAR_ENABLE_MEGA
            var blocks = new List<Tuple<BlockController.BlockInfo, BlockHolder.PoseSet>>();
            foreach (var instance in result.instances())
            {
                using (instance)
                {
                    if (instance.appId() != currentEndPoint.AppID) { continue; }

                    blocks.Add(Tuple.Create(new BlockController.BlockInfo
                    {
                        ID = instance.blockId(),
                        Name = instance.name(),
                    }, new BlockHolder.PoseSet
                    {
                        BlockToCamera = instance.pose().ToUnityPose(),
                        CameraToVIOOrigin = motion.OnSome ? motion.Value.transform().ToUnityPose() : default(Pose?)
                    }));
                }
            }
            blockHolderWrapper.OnTrack(blocks);
#endif
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
            if (tracker == null) { return; }
            if (!isHMD)
            {
                using (var souce = player.accelerometerResultSource())
                using (var sink = tracker.accelerometerResultSink())
                {
                    souce.connect(sink);
                }
            }
            using (var souce = player.proximityLocationResultSource())
            using (var sink = tracker.proximityLocationResultSink())
            {
                souce.connect(sink);
            }
            using (var souce = player.locationResultSource())
            {
                souce.setHandler((Action<LocationResult>)((data) => OnFramePlayerLocationResult(data)));
            }
        }

        void IFrameSupplementFilter.Disconnect(VideoInputFramePlayer player)
        {
            FramePlayerLocation = Optional<LocationResult>.Empty;
        }

        void IFrameSupplementFilter.Connect(FrameSupplementPlayer player, bool isHMD)
        {
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
            FramePlayerLocation = Optional<LocationResult>.Empty;
        }

#if EASYAR_ENABLE_MEGA
        internal override string DumpLite()
        {
            if (tracker == null) { return null; }

            var isFail = arSession && arSession.CameraTransformType.OnSome && arSession.CameraTransformType.Value.ToMegaInputFrameLevel() < MinInputFrameLevel;
            var failMessage = isFail ? (isEditorSimulation ? " (Editor Simulation)" : " (Running Failed)") : string.Empty;
            var data = $"Mega {ServiceType}: min={MinInputFrameLevel}{failMessage}, {LocationInputMode}" +
                (LocationInputMode == MegaLocationInputMode.Simulator ? $" {SimulatorLocation}" : string.Empty) +
                $", ({RequestTimeParameters.Timeout}, {RequestTimeParameters.RequestInterval}), ({enableLocalization}, {enableStatelessLocalization}), {cameraPositionOverride}";

            if (!isEditorSimulation && isFail) { return data; }
            if (!enabled) { return null; }

            data += Environment.NewLine;
            data += $"- Block Holder: {BlockHolder.BlockRootSource}, {BlockHolder.MultiBlock}" + Environment.NewLine;
            data += $"- Server: {MegaDumpHelper.ServerSuffix(currentEndPoint.ServerAddress)}, {currentEndPoint.AppID}" + Environment.NewLine;
            return data + dumpHelper.DumpResponse(arSession && arSession.Assembly != null ? arSession.Assembly.Camera : null, BlockHolder.BlockRoot);
        }
#endif

        private void OnFramePlayerAccelerometerResult(FrameSupplement.DeviceInput.AccelerometerResult data)
        {
            accelerometerResultSink?.handle(new AccelerometerResult(data.x, data.y, data.z, data.timestamp));
        }

        private void OnFramePlayerLocationResult(FrameSupplement.DeviceInput.LocationResult data)
        {
            OnFramePlayerLocationResult(new LocationResult(data.latitude, data.longitude, data.altitude, data.horizontalAccuracy, data.verticalAccuracy, true, true, true));
        }

        private void OnFramePlayerLocationResult(LocationResult data)
        {
            if (locationResultSink == null) { return; }

            FramePlayerLocation = data;
            locationResultSink?.handle(data);
        }

        private void SendResponse()
        {
            // response is received from Scheduler, which is triggered at the beginning of update loop
            while (pendingResponses.Count > 0)
            {
                var response = pendingResponses.Dequeue();
                if (!isEditorSimulation && arSession.CameraTransformType.OnSome && arSession.CameraTransformType.Value.ToMegaInputFrameLevel() < MinInputFrameLevel)
                {
                    LocalizationRespond?.Invoke(new MegaLocalizationResponse
                    {
                        Timestamp = response.Timestamp,
                        Status = MegaTrackerLocalizationStatus.UnknownError,
                        ErrorMessage = $"Mega running fail: current input frame level ({arSession.CameraTransformType.Value}) < minimum input frame level set by the app developer ({MinInputFrameLevel}).",
                    });
                    continue;
                }
                LocalizationRespond?.Invoke(response);
            }
        }

        private void HandleProximityLocation(ProximityLocationResult pLocation)
        {
            proximityLocationProvider?.Invoke(pLocation);
            if (tracker == null) { return; }
            if (proximityLocationResultSink == null)
            {
                proximityLocationResultSink = tracker.proximityLocationResultSink();
            }
            proximityLocationResultSink.handle(pLocation);
        }

        private void BindBlocks()
        {
            if (tracker == null) { return; }
            if (BlockPrior.OnSome)
            {
                tracker?.bindBlockPrior(BlockPrior.Value.ToBlockPriorResult());
            }
            else
            {
                tracker?.unbindBlockPrior();
            }
        }
    }
}
