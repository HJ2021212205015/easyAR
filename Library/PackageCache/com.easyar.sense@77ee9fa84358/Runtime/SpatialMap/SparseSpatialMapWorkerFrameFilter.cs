//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="SparseSpatialMap"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="SparseSpatialMap"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
    public abstract class SparseSpatialMapWorkerFrameFilter : FrameFilter, FrameFilter.IInputFrameSink, FrameFilter.IOutputFrameSource
    {
        private protected SparseSpatialMap Sparse;
        private protected SparseSpatialMapManager Manager;
        private protected ARSession Session;

        [HideInInspector, SerializeField]
        private ServiceAccessSourceType serviceAccessSource;
        [HideInInspector, SerializeField]
        private FixedAddressAPIKeyAccessData fixedAddressAPIKeyAccessData = new();

        /// <summary>
        /// <para xml:lang="en">Service access source type.</para>
        /// <para xml:lang="zh">服务访问数据源类型。</para>
        /// </summary>
        public enum ServiceAccessSourceType
        {
            /// <summary>
            /// <para xml:lang="en">Use global service config <see cref="EasyARSettings.GlobalSpatialMapServiceConfig"/>. The global service config can be changed on the inspector after click Unity menu EasyAR -> Sense -> Configuration.</para>
            /// <para xml:lang="zh">使用全局服务器配置<see cref="EasyARSettings.GlobalSpatialMapServiceConfig"/>。全局配置可以点击Unity菜单EasyAR -> Sense -> Configuration后在属性面板里面进行填写。</para>
            /// </summary>
            GlobalConfig,
            /// <summary>
            /// <para xml:lang="en">Use <see cref="FixedAddressAPIKeyAccessData"/> type of access data.</para>
            /// <para xml:lang="zh">使用<see cref="FixedAddressAPIKeyAccessData"/>类型的访问数据。</para>
            /// </summary>
            APIKey,
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
                if (Session) { return; }
                serviceAccessSource = value;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Service access data. Only effective if modified before the session starts. Do not set if <see cref="ServiceAccessSourceType.GlobalConfig"/> is in use.</para>
        /// <para xml:lang="zh">服务访问数据。在session启动前修改才有效。使用<see cref="ServiceAccessSourceType.GlobalConfig"/>无需设置。</para>
        /// </summary>
        public FixedAddressAPIKeyAccessData ServiceAccessData
        {
            get
            {
                return ServiceAccessSource switch
                {
                    ServiceAccessSourceType.GlobalConfig => EasyARSettings.Instance ? EasyARSettings.Instance.GlobalSpatialMapServiceConfig : null,
                    ServiceAccessSourceType.APIKey => fixedAddressAPIKeyAccessData,
                    _ => throw new InvalidOperationException(),
                };
            }
            set
            {
                if (Session) { return; }
                switch (ServiceAccessSource)
                {
                    case ServiceAccessSourceType.GlobalConfig:
                        throw new ArgumentException($"{nameof(ServiceAccessData)} can not be set when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                    case ServiceAccessSourceType.APIKey:
                        if (value is not FixedAddressAPIKeyAccessData apiKeyData)
                        {
                            throw new ArgumentException($"{nameof(ServiceAccessData)} must be {nameof(FixedAddressAPIKeyAccessData)} when {nameof(ServiceAccessSource)} is {ServiceAccessSource}.");
                        }
                        fixedAddressAPIKeyAccessData = apiKeyData;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        internal override bool IsAvailable => SparseSpatialMap.isAvailable() && SparseSpatialMapManager.isAvailable();
        internal override int BufferRequirement => Sparse.bufferRequirement();

        private protected virtual void OnEnable()
        {
            Sparse?.start();
        }

        private void OnDisable()
        {
            Sparse?.stop();
        }

        private void OnDestroy()
        {
            OnSessionStop();
        }


        /// <summary>
        /// <para xml:lang="en">Clears allocated cache storage space.</para>
        /// <para xml:lang="zh">清除已占用的缓存数据存储空间。</para>
        /// </summary>
        public static void ClearCache()
        {
            if (SparseSpatialMapManager.isAvailable())
            {
                using (var manager = SparseSpatialMapManager.create())
                {
                    manager.clear();
                }
            }
        }

        internal override void OnSessionStart(ARSession session)
        {
            this.Session = session;

            Sparse = SparseSpatialMap.create();
            Manager = SparseSpatialMapManager.create();

            if (!Sparse.setResultAsyncMode(session.Assembly.IsASync))
            {
                EasyARController.LicenseValidation.OnResultSyncModeFail(session.Assembly.FrameSource.GetType());
            }
        }

        internal override void OnSessionStop()
        {
            Session = null;
            Manager?.Dispose();
            Manager = null;
            Sparse?.Dispose();
            Sparse = null;
        }

        internal List<Vector3> HitTestAgainstPointCloud(Vector2 pointInView)
        {
            var points = new List<Vector3>();
            if (!Session || Sparse == null) { return points; }

            var coord = Session.ImageCoordinatesFromScreenCoordinates(pointInView);
            if (coord.OnNone) { return points; }

            var hitPoints = Sparse.hitTestAgainstPointCloud(coord.Value.ToEasyARVector());
            return hitPoints.Select(p => new Vector3(p.data_0, p.data_1, -p.data_2)).ToList();
        }

        InputFrameSink IInputFrameSink.InputFrameSink() => Sparse?.inputFrameSink();
        OutputFrameSource IOutputFrameSource.OutputFrameSource() => Sparse?.outputFrameSource();

        void IOutputFrameSource.OnResult(Optional<FrameFilterResult> frameFilterResult, Optional<MotionInputData> motion)
        {
            OnResult(frameFilterResult.OnSome ? frameFilterResult.Value as SparseSpatialMapResult : null, motion);
        }

        void IOutputFrameSource.RetrieveSyncResults(Optional<MotionInputData> motion)
        {
            using (var result = EasyARController.IsReady && motion.OnSome ? Sparse?.getSyncResult(motion.Value).ValueOrDefault(null) : null)
            {
                OnResult(result, motion);
            }
        }

        private protected abstract void OnResult(SparseSpatialMapResult mapResult, Optional<MotionInputData> motion);

        private protected List<Vector3> RetrievePoints(Buffer buffer)
        {
            if (buffer == null)
            {
                return new List<Vector3>();
            }

            var bufferFloat = new float[buffer.size() / 4];

            if (buffer.size() > 0)
            {
                Marshal.Copy(buffer.data(), bufferFloat, 0, bufferFloat.Length);
            }
            return Enumerable.Range(0, bufferFloat.Length / 3).Select(k =>
            {
                return new Vector3(bufferFloat[k * 3], bufferFloat[k * 3 + 1], -bufferFloat[k * 3 + 2]);
            }).ToList();
        }

        private protected string NotifyEmptyConfig(FixedAddressAPIKeyAccessData config)
        {
            var error = string.Empty;
            if (config == null ||
                string.IsNullOrEmpty(config.APIKey) ||
                string.IsNullOrEmpty(config.APISecret) ||
                string.IsNullOrEmpty(config.AppID))
            {
                error = "Service config (for authentication) NOT set, please set" + Environment.NewLine +
                    "globally in EasyAR Settings (Project Settings > EasyAR) or" + Environment.NewLine +
                    $"locally on <{GetType()}> Component." + Environment.NewLine +
                    "Get from EasyAR Develop Center (www.easyar.com) -> SpatialMap -> Database Details.";
                Session.Diagnostics.EnqueueError(error);
            }
            return error;
        }
    }
}
