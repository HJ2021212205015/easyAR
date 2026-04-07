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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="SparseSpatialMap"/> in the scene, providing map localizing and tracking functions of <see cref="SparseSpatialMap"/>.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="SparseSpatialMap"/>的<see cref="MonoBehaviour"/>，提供<see cref="SparseSpatialMap"/>的定位跟踪功能。</para>
    /// </summary>
    public class SparseSpatialMapTrackerFrameFilter : SparseSpatialMapWorkerFrameFilter, FrameFilter.ICenterTransform
    {
        /// <summary>
        /// <para xml:lang="en">Timeout in milliseconds when communicating with server. Modify at any time and takes effect immediately.</para>
        /// <para xml:lang="zh">与服务器通信的超时时间（毫秒）。可随时修改，立即生效。</para>
        /// </summary>
        public int RequestTimeout = 60000;

        /// <summary>
        /// <para xml:lang="en">Localization mode. Only effective if modified before the session starts or OnEnable.</para>
        /// <para xml:lang="en">Will be forced to <see cref="LocalizationMode.KeepUpdate"/> if <see cref="EnableStabilization"/> is true.</para>
        /// <para xml:lang="zh">定位模式。在session启动前或OnEnable前修改才有效。</para>
        /// <para xml:lang="zh">如果<see cref="EnableStabilization"/>是true，会强制为<see cref="LocalizationMode.KeepUpdate"/>。</para>
        /// </summary>
        public LocalizationMode LocalizationMode = LocalizationMode.KeepUpdate;

        /// <summary>
        /// <para xml:lang="en">Result pose setting. Only effective if modified before the session starts or OnEnable.</para>
        /// <para xml:lang="zh">结果姿态设置。在session启动前或OnEnable前修改才有效。</para>
        /// </summary>
        public bool EnableStabilization = true;

        /// <summary>
        /// <para xml:lang="en">Enable indirect tracking which keeps tracking through motion tracking ability from frame source while direct stopped. Only effective if modified before the session starts.</para>
        /// <para xml:lang="zh">启用非直接跟踪（直接跟踪停止后，通过frame source的运动跟踪能力继续跟踪）。在session启动前修改才有效。</para>
        /// </summary>
        public bool EnableIndirectTracking = true;

        private List<SparseSpatialMapController> holdingTargets = new();
        private string previousTargetID = string.Empty;
        private Dictionary<SparseSpatialMapController, Coroutine> loadingTargets = new();
        private Dictionary<string, SparseSpatialMapController> maps = new();
        private TargetCenterTransformHelper targetCenterTransformHelper = new();
        private bool enableIndirect;
        private Queue<(float, string)> dumpMessages = new();

        /// <summary>
        /// <para xml:lang="en">Map load finish event. The bool value indicates the load success or not. The string value is the error message when fail.</para>
        /// <para xml:lang="zh">Map加载完成的事件。bool值表示加载是否成功。string值表示出错时的错误信息。</para>
        /// </summary>
        public event Action<SparseSpatialMapController, bool, string> TargetLoad;
        /// <summary>
        /// <para xml:lang="en">Map unload finish event. The bool value indicates the unload success or not. The string value is the error message when fail.</para>
        /// <para xml:lang="zh">Map卸载完成的事件。bool值表示卸载是否成功。string值表示出错时的错误信息。</para>
        /// </summary>
        public event Action<SparseSpatialMapController, bool, string> TargetUnload;

        /// <summary>
        /// <para xml:lang="en"><see cref="SparseSpatialMapController"/> that has been loaded.</para>
        /// <para xml:lang="zh">已加载的<see cref="SparseSpatialMapController"/>。</para>
        /// </summary>
        public List<SparseSpatialMapController> Targets => maps.Select(m => m.Value).ToList();

        /// <summary>
        /// <para xml:lang="en">Start/Stop tracking when <see cref="ARSession"/> is running. Tracking will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止跟踪。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始跟踪。</para>
        /// </summary>
        private protected override void OnEnable()
        {
            if (Sparse != null)
            {
                using (var config = new SparseSpatialMapConfig())
                {
                    if (EnableStabilization)
                    {
                        LocalizationMode = LocalizationMode.KeepUpdate;
                    }
                    config.setLocalizationMode(LocalizationMode);
                    Sparse.setConfig(config);
                    Sparse.setResultPoseType(EnableStabilization);
                }
            }
            base.OnEnable();
        }

        internal override void OnSessionStart(ARSession session)
        {
            base.OnSessionStart(session);
            enableIndirect = EnableIndirectTracking;
            Sparse.startLocalization();
            foreach (var controller in holdingTargets)
            {
                LoadSparseSpatialMap(controller);
            }

            if (enabled) { OnEnable(); }
        }

        internal override void OnSessionStop()
        {
            foreach (var controller in holdingTargets)
            {
                UnloadSparseSpatialMap(controller);
            }
            loadingTargets.Clear();
            maps.Clear();
            previousTargetID = string.Empty;
            targetCenterTransformHelper.Results = null;
            base.OnSessionStop();
        }

        internal override string DumpLite()
        {
            if (Sparse == null) { return null; }

            var data = $"{ARSessionFactory.DefaultName(GetType())}: {enabled}, ({RequestTimeout}), ({LocalizationMode}, {EnableStabilization})" + Environment.NewLine;
            if (!enabled) { return null; }

            data += $"- Server: {ServiceAccessData.AppID}" + Environment.NewLine;
            if (!string.IsNullOrEmpty(previousTargetID))
            {
                data += $"- Map: {previousTargetID}" + Environment.NewLine;
            }
            while (dumpMessages.Count > 0 && Time.realtimeSinceStartup - dumpMessages.Peek().Item1 > 2)
            {
                dumpMessages.Dequeue();
            }
            foreach (var message in dumpMessages)
            {
                data += $"- {message.Item2}" + Environment.NewLine;
            }
            return data;
        }

        private protected override void OnResult(SparseSpatialMapResult mapResult, Optional<MotionInputData> motion)
        {
            var resultControllers = new List<Tuple<MonoBehaviour, Pose>>();

            string mapID = string.Empty;
            if (mapResult != null)
            {
                if (mapResult.getLocalizationStatus())
                {
                    mapID = mapResult.getLocalizationMapID();

                    var controller = TryGetMapController(mapID);
                    if (controller && controller.IsLoaded)
                    {
                        var vioPose = mapResult.getVioPose();
                        var pose = mapResult.getMapPose().Value.ToUnityPose();
                        if (controller.PointCloud.Count == 0)
                        {
                            using (var cloudBuffer = Sparse.getPointCloudBuffer())
                            {
                                controller.UpdatePointCloud(RetrievePoints(cloudBuffer));
                            }
                        }
                        if (Session.Origin && controller.gameObject.transform.IsChildOf(Session.Origin.transform))
                        {
                            controller.gameObject.transform.SetParent(null, true);
                        }
                        if (controller.OnDirectTracking(true, enableIndirect))
                        {
                            dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Found: {controller.Info.Name} ({controller.Info.ID})"));
                        }
                        resultControllers.Add(Tuple.Create((MonoBehaviour)controller, pose));
                    }
                }
            }

            if (previousTargetID != mapID && !string.IsNullOrEmpty(previousTargetID))
            {
                var controller = TryGetMapController(previousTargetID);
                if (controller)
                {
                    if (enableIndirect && Session.Assembly.DefaultOriginChild.OnSome && Session.Assembly.DefaultOriginChild.Value)
                    {
                        controller.gameObject.transform.SetParent(Session.Assembly.DefaultOriginChild.Value.transform);
                    }
                    if (controller.OnDirectTracking(false, enableIndirect))
                    {
                        dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Lost: {controller.Info.Name} ({controller.Info.ID})"));
                    }
                }
            }
            previousTargetID = mapID;
            targetCenterTransformHelper.Results = resultControllers;
        }

        Optional<Tuple<GameObject, Pose>> ICenterTransform.TryGetCenter(GameObject center) => targetCenterTransformHelper.TryGetCenter(center);

        void ICenterTransform.UpdateTransform(GameObject center, Pose centerPose) => targetCenterTransformHelper.UpdateTransform(center, centerPose);

        internal void Hold(SparseSpatialMapController controller)
        {
            if (holdingTargets.Contains(controller)) { return; }
            holdingTargets.Add(controller);
            if (Sparse != null)
            {
                LoadSparseSpatialMap(controller);
            }
        }

        internal void Unhold(SparseSpatialMapController controller)
        {
            if (!holdingTargets.Contains(controller)) { return; }
            holdingTargets.Remove(controller);
            if (Sparse != null) { UnloadSparseSpatialMap(controller); }
        }

        private void LoadSparseSpatialMap(SparseSpatialMapController controller)
        {
            if (!controller) { return; }

            var coroutine = StartCoroutine(LoadDataAndLoadSparseSpatialMap(controller));
            if (coroutine != null)
            {
                loadingTargets[controller] = coroutine;
            }
        }

        private IEnumerator LoadDataAndLoadSparseSpatialMap(SparseSpatialMapController controller)
        {
            if (!controller) { yield break; }

            yield return new WaitUntil(() => controller && controller.LoadingSource.OnSome);

            if (controller && controller.Info != null)
            {
                maps[controller.Info.ID] = controller;

                var info = controller.Info;
                if (controller.LoadingSource.Value is SparseSpatialMapController.MapManagerSourceData mapManagerSource)
                {
                    var access = ServiceAccessData?.TrimClone() as FixedAddressAPIKeyAccessData;
                    var configError = NotifyEmptyConfig(access);
                    if (!string.IsNullOrEmpty(configError))
                    {
                        dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Load: {info.Name} ({info.ID}) => false (empty config)"));
                        TargetLoad?.Invoke(controller, false, configError);
                        yield break;
                    }

                    Manager.load(Sparse, mapManagerSource.ID, access.APIKey, access.APISecret, access.AppID, RequestTimeout, EasyARController.Scheduler, (status, error) =>
                    {
                        if (controller && TryGetMapController(mapManagerSource.ID) && status)
                        {
                            controller.OnLoad();
                        }
                        dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Load: {info.Name} ({info.ID}) => {status} {(string.IsNullOrEmpty(error) ? "" : "(" + error + ")")}"));
                        TargetLoad?.Invoke(controller, status, error);
                    });
                }
            }
            loadingTargets.Remove(controller);
        }

        private void UnloadSparseSpatialMap(SparseSpatialMapController controller)
        {
            if (controller.Info != null)
            {
                if (maps.Remove(controller.Info.ID))
                {
                    controller.OnUnload();
                    var info = controller.Info;
                    Sparse.unloadMap(controller.Info.ID, EasyARController.Scheduler, (Action<bool>)((status) =>
                    {
                        dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Unload: {info.Name} ({info.ID}) => {status}"));
                        TargetUnload?.Invoke(controller, status, string.Empty);
                    }));
                }
            }

            if (loadingTargets.TryGetValue(controller, out var coroutine))
            {
                StopCoroutine(coroutine);
                loadingTargets.Remove(controller);
            }
        }

        private SparseSpatialMapController TryGetMapController(string id) => maps.TryGetValue(id, out var controller) ? controller : null;
    }
}
