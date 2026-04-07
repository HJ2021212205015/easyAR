//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Linq;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="SparseSpatialMap"/> in the scene, providing map building functions of <see cref="SparseSpatialMap"/>.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="SparseSpatialMap"/>的<see cref="MonoBehaviour"/>，提供<see cref="SparseSpatialMap"/>的建图功能。</para>
    /// </summary>
    public class SparseSpatialMapBuilderFrameFilter : SparseSpatialMapWorkerFrameFilter
    {
        /// <summary>
        /// <para xml:lang="en"><see cref="Material"/> for point cloud render. Only effective if modified before the session starts.</para>
        /// <para xml:lang="zh">用于渲染点云的<see cref="Material"/>。在session启动前修改才有效。</para>
        /// </summary>
        public Material PointCloudMaterial;

        private GameObject mapRoot;

        /// <summary>
        /// <para xml:lang="en">Target controller to display map building process. Can only be used after session start.</para>
        /// <para xml:lang="zh">用于显示建图过程的目标controller。在session启动后才能使用。</para>
        /// </summary>
        public SparseSpatialMapBuildTargetController Target { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Start/Stop building when <see cref="ARSession"/> is running. Building will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止建图。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始建图。</para>
        /// </summary>
        private protected override void OnEnable() => base.OnEnable();

        /// <summary>
        /// <para xml:lang="en">Create and upload map.</para>
        /// <para xml:lang="en">Build map. <paramref name="name"/> is the map name. <paramref name="preview"/> is the optional preview image。<paramref name="timeoutMilliseconds"/> is the optional timeout in milliseconds when communicating with server, default to 60000 if empty.</para>
        /// <para xml:lang="zh">创建并上传地图。</para>
        /// <para xml:lang="zh">构建Map。<paramref name="name"/>为地图的名字。<paramref name="preview"/>是预览图，可选。<paramref name="timeoutMilliseconds"/>是与服务器通信的超时时间（毫秒），可选，默认60000。</para>
        /// </summary>
        public void Host(string name, Optional<Image> preview, Optional<int> timeoutMilliseconds, Action<Optional<SparseSpatialMapController.SparseSpatialMapInfo>, Optional<string>> callback)
        {
            if (Sparse == null)
            {
                callback?.Invoke(null, "Unhostable");
                return;
            }

            var access = ServiceAccessData?.TrimClone() as FixedAddressAPIKeyAccessData;
            var configError = NotifyEmptyConfig(access);
            if (!string.IsNullOrEmpty(configError))
            {
                callback?.Invoke(null, configError);
                return;
            }

            var timeout = timeoutMilliseconds.ValueOrDefault(60000);
            Manager.host(Sparse, access.APIKey, access.APISecret, access.AppID, name, preview, timeoutMilliseconds, EasyARController.Scheduler, (status, id, error) =>
            {
                if (!status)
                {
                    Debug.LogError(error);
                    callback?.Invoke(null, error);
                    return;
                }
                callback?.Invoke(new SparseSpatialMapController.SparseSpatialMapInfo() { Name = name, ID = id }, null);
            });
        }


        internal override void OnSessionStart(ARSession session)
        {
            base.OnSessionStart(session);
            using (var config = new SparseSpatialMapConfig())
            {
                config.setLocalizationMode(LocalizationMode.KeepUpdate);
                Sparse.setConfig(config);
            }

            if (!PointCloudMaterial)
            {
                PointCloudMaterial = ObjectUtil.FindObjectsByType<SparseSpatialMapController>(true).Select(c => c.ObsoleteBuildTargetMaterial).Where(m => m).FirstOrDefault();
                Debug.LogWarning($"{nameof(PointCloudMaterial)} not set, {this} is working in compatible mode trying to grab material from any old version {nameof(SparseSpatialMapController)} in the scene: {(PointCloudMaterial ? "success" : "fail")}");
            }
            mapRoot = ARSessionFactory.CreateSparseSpatialMapBuildTarget(new ARSessionFactory.Resources { SparseSpatialMapPointCloudMaterial = PointCloudMaterial });
            Target = mapRoot.GetComponent<SparseSpatialMapBuildTargetController>();
            Target.Builder = this;
            if (session.Assembly.DefaultOriginChild.OnSome && session.Assembly.DefaultOriginChild.Value)
            {
                mapRoot.transform.SetParent(session.Assembly.DefaultOriginChild.Value.transform, false);
            }

            if (enabled) { OnEnable(); }
        }

        internal override void OnSessionStop()
        {
            if (Target) { Target.Builder = null; }
            if (mapRoot) { Destroy(mapRoot); }
            mapRoot = null;
            Target = null;
            base.OnSessionStop();
        }

        internal override string DumpLite()
        {
            if (Sparse == null) { return null; }

            var data = $"{ARSessionFactory.DefaultName(GetType())}: {enabled}, {(Target ? Target.PointCloud.Count : "-")}" + Environment.NewLine;
            if (!enabled) { return null; }

            data += $"- Server: {ServiceAccessData.AppID}" + Environment.NewLine;
            return data;
        }

        private protected override void OnResult(SparseSpatialMapResult mapResult, Optional<MotionInputData> motion)
        {
            if (!Target || mapResult == null) { return; }
            using (var cloudBuffer = Sparse.getPointCloudBuffer())
            {
                Target.UpdatePointCloud(RetrievePoints(cloudBuffer));
            }
        }
    }
}
