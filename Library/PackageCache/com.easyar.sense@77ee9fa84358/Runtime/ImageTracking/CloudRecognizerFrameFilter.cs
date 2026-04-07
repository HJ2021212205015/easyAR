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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="CloudRecognizer"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="CloudRecognizer"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
    public class CloudRecognizerFrameFilter : FrameFilter, FrameFilter.IHFlipHandler
    {
        private CloudRecognizer cloudRecognizer;

        [HideInInspector, SerializeField]
        private ServiceAccessSourceType serviceAccessSource;
        [HideInInspector, SerializeField]
        private APIKeyAccessData apiKeyAccessData = new();
        [HideInInspector, SerializeField]
        private AppSecretAccessData appSecretAccessData = new();
        [HideInInspector, SerializeField]
        private TokenAccessData tokenAccessData = new();
        private bool failToCreate;
        private bool horizontalFlip;
        private ARSession session;
        private Tuple<float, ResponseCache> curResponse;

        /// <summary>
        /// <para xml:lang="en">Service access source type.</para>
        /// <para xml:lang="zh">服务访问数据源类型。</para>
        /// </summary>
        public enum ServiceAccessSourceType
        {
            /// <summary>
            /// <para xml:lang="en">Use global service config <see cref="EasyARSettings.GlobalCloudRecognizerServiceConfig"/>. The global service config can be changed on the inspector after click Unity menu EasyAR -> Sense -> Configuration.</para>
            /// <para xml:lang="zh">使用全局服务器配置<see cref="EasyARSettings.GlobalCloudRecognizerServiceConfig"/>。全局配置可以点击Unity菜单EasyAR -> Sense -> Configuration后在属性面板里面进行填写。</para>
            /// </summary>
            GlobalConfig,
            /// <summary>
            /// <para xml:lang="en">Use <see cref="APIKeyAccessData"/> type of access data.</para>
            /// <para xml:lang="zh">使用<see cref="APIKeyAccessData"/>类型的访问数据。</para>
            /// </summary>
            APIKey,
            /// <summary>
            /// <para xml:lang="en">Use <see cref="AppSecretAccessData"/> type of access data. Usually for private cloud service.</para>
            /// <para xml:lang="zh">使用<see cref="AppSecretAccessData"/>类型的访问数据。通常用于私有云。</para>
            /// </summary>
            AppSecret,
            /// <summary>
            /// <para xml:lang="en">Use <see cref="TokenAccessData"/> type of access data.</para>
            /// <para xml:lang="zh">使用<see cref="TokenAccessData"/>类型的访问数据。</para>
            /// </summary>
            Token,
        }

        /// <summary>
        /// <para xml:lang="en">Service access source type. Only effective if modified before the session starts.</para>
        /// <para xml:lang="zh">服务访问数据源类型。在session启动前修改才有效。</para>
        /// </summary>
        public ServiceAccessSourceType ServiceAccessSource
        {
            get => serviceAccessSource;
            set
            {
                if (session) { return; }
                serviceAccessSource = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Service access data. Only effective if modified before the session starts. Do not set if <see cref="ServiceAccessSourceType.GlobalConfig"/> is in use.</para>
        /// <para xml:lang="zh">服务访问数据。在session启动前修改才有效。使用<see cref="ServiceAccessSourceType.GlobalConfig"/>无需设置。</para>
        /// </summary>
        public ExplicitAddressAccessData ServiceAccessData
        {
            get
            {
                return ServiceAccessSource switch
                {
                    ServiceAccessSourceType.GlobalConfig => EasyARSettings.Instance ? EasyARSettings.Instance.GlobalCloudRecognizerServiceConfig : null,
                    ServiceAccessSourceType.APIKey => apiKeyAccessData,
                    ServiceAccessSourceType.AppSecret => appSecretAccessData,
                    ServiceAccessSourceType.Token => tokenAccessData,
                    _ => throw new InvalidOperationException(),
                };
            }
            set
            {
                if (session) { return; }
                switch (ServiceAccessSource)
                {
                    case ServiceAccessSourceType.GlobalConfig:
                        throw new ArgumentException($"{nameof(ServiceAccessData)} can not be set when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                    case ServiceAccessSourceType.APIKey:
                        if (value is not APIKeyAccessData apiKeyData)
                        {
                            throw new ArgumentException($"{nameof(ServiceAccessData)} must be {nameof(APIKeyAccessData)} when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                        }
                        apiKeyAccessData = apiKeyData;
                        break;
                    case ServiceAccessSourceType.AppSecret:
                        if (value is not AppSecretAccessData appSecretData)
                        {
                            throw new ArgumentException($"{nameof(ServiceAccessData)} must be {nameof(AppSecretAccessData)} when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                        }
                        appSecretAccessData = appSecretData;
                        break;
                    case ServiceAccessSourceType.Token:
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

        internal override bool IsAvailable => CloudRecognizer.isAvailable();
        internal override int BufferRequirement => 0;

        private void OnDestroy()
        {
            OnSessionStop();
        }

        /// <summary>
        /// <para xml:lang="en">Send recognition request. The lowest available request interval is 300ms. <paramref name="timeoutMilliseconds"/> is the optional timeout in milliseconds when communicating with server, default to 3000 if empty.</para>
        /// <para xml:lang="zh">发送云识别请求。最低可用请求间隔是300ms。<paramref name="timeoutMilliseconds"/>是与服务器通信的超时时间（毫秒），可选，默认3000。</para>
        /// </summary>
        public void Resolve(Optional<int> timeoutMilliseconds, Action<CloudRecognizationResponse> callback)
        {
            var startTime = Time.realtimeSinceStartup;
            Action<CloudRecognizationResponse> onResponse = (response) =>
            {
                response.TotalResponseDuration = Time.realtimeSinceStartup - startTime;
                curResponse = Tuple.Create(Time.realtimeSinceStartup, new ResponseCache(response));
                callback?.Invoke(response);
            };

            if (!enabled)
            {
                onResponse?.Invoke(new CloudRecognizationResponse("Disabled"));
                return;
            }
            if (!session || cloudRecognizer == null)
            {
                if (failToCreate)
                {
                    onResponse?.Invoke(new CloudRecognizationResponse("FailToCreate"));
                }
                else
                {
                    onResponse?.Invoke(new CloudRecognizationResponse("Unavailable"));
                }
                return;
            }

            var asyncFrame = session.AsyncCameraFrame;
            if (asyncFrame.OnNone)
            {
                onResponse?.Invoke(new CloudRecognizationResponse("NoFrame"));
                return;
            }

            using (var outputFrame = asyncFrame.Value)
            using (var inputFrame = outputFrame.inputFrame())
            {
                var timestamp = inputFrame.hasTemporalInformation() ? inputFrame.timestamp() : Time.realtimeSinceStartup;
                void onResult(CloudRecognizationResult result)
                {
                    var targetO = result.getTarget();
                    using (var target = targetO.ValueOrDefault(null))
                    {
                        onResponse?.Invoke(new CloudRecognizationResponse
                        {
                            Timestamp = timestamp,
                            Target = target,
                            Status = result.getStatus(),
                            ErrorMessage = result.getUnknownErrorMessage(),
                        });
                    }
                }
                var timeout = timeoutMilliseconds.ValueOrDefault(3000);
                {
                    cloudRecognizer.resolve(inputFrame, timeout, EasyARController.Scheduler, onResult);
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Updates API Token.</para>
        /// <para xml:lang="zh">更新API Token。</para>
        /// </summary>
        public void UpdateToken(string token)
        {
            cloudRecognizer?.updateToken(token);
        }


        internal override void OnSessionStart(ARSession session)
        {
            this.session = session;
            var access = ServiceAccessData?.TrimClone() as ExplicitAddressAccessData;

            if (access == null
                || string.IsNullOrEmpty(access.AppID)
                || string.IsNullOrEmpty(access.ServerAddress)
                || (access is APIKeyAccessData apiKeyTrimed && (string.IsNullOrEmpty(apiKeyTrimed.APIKey) || string.IsNullOrEmpty(apiKeyTrimed.APISecret)))
                || (access is AppSecretAccessData appSecretTrimed && (string.IsNullOrEmpty(appSecretTrimed.AppSecret)))
                || (access is TokenAccessData tokenTrimed && (string.IsNullOrEmpty(tokenTrimed.Token)))
                )
            {
                throw new DiagnosticsMessageException(
                    "Service config (for authentication) NOT set, please set" + Environment.NewLine +
                    "globally in EasyAR Settings (Project Settings > EasyAR) or" + Environment.NewLine +
                    $"locally on <{GetType()}> Component." + Environment.NewLine +
                    "Get from EasyAR Develop Center (www.easyar.com) -> CRS -> Database Details.");
            }

            try
            {
                if (access is APIKeyAccessData apiKeyAccess)
                {
                    cloudRecognizer = CloudRecognizer.create(apiKeyAccess.ServerAddress, apiKeyAccess.APIKey, apiKeyAccess.APISecret, apiKeyAccess.AppID);
                }
                else if (access is AppSecretAccessData secretAccess)
                {
                    cloudRecognizer = CloudRecognizer.createByCloudSecret(secretAccess.ServerAddress, secretAccess.AppSecret, secretAccess.AppID);
                }
                else if (access is TokenAccessData tokenAccess)
                {
                    cloudRecognizer = CloudRecognizer.createWithToken(tokenAccess.ServerAddress, tokenAccess.Token, tokenAccess.AppID);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported access data type: {access.GetType().Name}");
                }
                failToCreate = false;
            }
            catch (ArgumentNullException)
            {
                failToCreate = true;
                throw new DiagnosticsMessageException($"Fail to create {nameof(CloudRecognizer)}, check logs for detials.");
            }
        }

        internal override void OnSessionStop()
        {
            cloudRecognizer?.Dispose();
            cloudRecognizer = null;
            failToCreate = false;
            curResponse = null;
        }

        internal override string DumpLite()
        {
            if (cloudRecognizer == null) { return null; }

            var data = $"{ARSessionFactory.DefaultName(GetType())}: {enabled}" + Environment.NewLine;
            data += $"- Server: {ServiceAccessData?.ServerAddress}, {ServiceAccessData?.AppID}" + Environment.NewLine;
            if (curResponse != null)
            {
                var response = curResponse.Item2;
                data += $"- {response.Timestamp:F3}, {response.Status} ({response.TotalResponseDuration:F3}), {response.TargetName} ({response.TargetID})" + Environment.NewLine;
                if (response.ErrorMessage.OnSome)
                {
                    data += "- Error: " + response.ErrorMessage + Environment.NewLine;
                }
                if (Time.realtimeSinceStartup - curResponse.Item1 > 5)
                {
                    curResponse = null;
                }
            }
            return data;
        }

        void IHFlipHandler.SetHFlip(bool flip) => horizontalFlip = flip;

        private class ResponseCache
        {
            public double Timestamp;
            public float TotalResponseDuration;
            public CloudRecognizationStatus Status;
            public Optional<string> TargetName;
            public Optional<string> TargetID;
            public Optional<string> ErrorMessage;

            internal ResponseCache(CloudRecognizationResponse r)
            {
                Timestamp = r.Timestamp;
                TotalResponseDuration = r.TotalResponseDuration;
                Status = r.Status;
                ErrorMessage = r.ErrorMessage;
                if (r.Target.OnSome)
                {
                    TargetName = r.Target.Value.name();
                    TargetID = r.Target.Value.uid();
                }
            }
        }
    }


    /// <summary>
    /// <para xml:lang="en">The response of recognition request.</para>
    /// <para xml:lang="zh">识别请求的响应。</para>
    /// </summary>
    public class CloudRecognizationResponse
    {
        /// <summary>
        /// <para xml:lang="en"><see cref="InputFrame"/> timestamp when sending the request.</para>
        /// <para xml:lang="zh">发送请求时的<see cref="InputFrame"/>时间戳。</para>
        /// </summary>
        public double Timestamp;
        /// <summary>
        /// <para xml:lang="en">The duration in seconds for response including rendering loop delay.</para>
        /// <para xml:lang="zh">响应耗时（秒），包含渲染循环延时。</para>
        /// </summary>
        public float TotalResponseDuration;
        /// <summary>
        /// <para xml:lang="en">Recognition status.</para>
        /// <para xml:lang="zh">识别状态。</para>
        /// </summary>
        public CloudRecognizationStatus Status;
        /// <summary>
        /// <para xml:lang="en">The recognized target when status is <see cref="CloudRecognizationStatus.FoundTarget"/>.</para>
        /// <para xml:lang="zh">在识别状态为<see cref="CloudRecognizationStatus.FoundTarget"/>时识别到的target。</para>
        /// </summary>
        public Optional<ImageTarget> Target;
        /// <summary>
        /// <para xml:lang="en">Error message.</para>
        /// <para xml:lang="zh">错误信息。</para>
        /// </summary>
        public Optional<string> ErrorMessage;

        internal CloudRecognizationResponse() { }

        internal CloudRecognizationResponse(string error)
        {
            Timestamp = Time.time;
            Status = CloudRecognizationStatus.UnknownError;
            ErrorMessage = $"Skip underlying request: {error}";
        }
    }
}
