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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="ObjectTracker"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="ObjectTracker"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
    public class ObjectTrackerFrameFilter : FrameFilter, FrameFilter.IFeedbackFrameSink, FrameFilter.IOutputFrameSource, FrameFilter.ICenterTransform, FrameFilter.IHFlipHandler
    {
        private ObjectTracker tracker;
        [HideInInspector, SerializeField]
        private int simultaneousNum = 1;
        [HideInInspector, SerializeField]
        private bool enableMotionFusion;
        private List<int> previousTargetIDs = new();
        private Dictionary<int, ObjectTargetController> allTargetController = new();
        private bool horizontalFlip;
        private TargetCenterTransformHelper targetCenterTransformHelper = new();
        private List<ObjectTargetController> holdingTargets = new();
        private Dictionary<ObjectTargetController, Coroutine> loadingTargets = new();
        private ARSession session;
        private Queue<(float, string)> dumpMessages = new();

        /// <summary>
        /// <para xml:lang="en">Target load finish event. The bool value indicates the load success or not.</para>
        /// <para xml:lang="zh">Target加载完成的事件。bool值表示加载是否成功。</para>
        /// </summary>
        public event Action<ObjectTargetController, bool> TargetLoad;
        /// <summary>
        /// <para xml:lang="en">Target unload finish event. The bool value indicates the unload success or not.</para>
        /// <para xml:lang="zh">Target卸载完成的事件。bool值表示卸载是否成功。</para>
        /// </summary>
        public event Action<ObjectTargetController, bool> TargetUnload;

        /// <summary>
        /// <para xml:lang="en"><see cref="ObjectTargetController"/> that has been loaded.</para>
        /// <para xml:lang="zh">已加载的<see cref="ObjectTargetController"/>。</para>
        /// </summary>
        public List<ObjectTargetController> Targets => allTargetController.Select(c => c.Value).ToList();

        /// <summary>
        /// <para xml:lang="en">The max number of targets which will be the simultaneously tracked by the tracker. Modify at any time and takes effect immediately.</para>
        /// <para xml:lang="zh">最大可被tracker跟踪的目标个数。可随时修改，立即生效。</para>
        /// </summary>
        public int SimultaneousNum
        {
            get
            {
                return tracker?.simultaneousNum() ?? simultaneousNum;
            }
            set
            {
                simultaneousNum = value;
                tracker?.setSimultaneousNum(simultaneousNum);
                if (session) { session.Assembly?.ResetBufferCapacity(); }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Enable motion fusion. Overwrites <see cref="SetResultPostProcessing"/>.</para>
        /// <para xml:lang="zh">启用运动跟踪。会覆盖<see cref="SetResultPostProcessing"/>。</para>
        /// </summary>
        public bool EnableMotionFusion
        {
            get => enableMotionFusion;
            set
            {
                enableMotionFusion = value;
                tracker?.setResultPostProcessing(value, value);
            }
        }

        internal override bool IsAvailable => ObjectTracker.isAvailable();
        internal override int BufferRequirement => tracker.bufferRequirement();

        /// <summary>
        /// <para xml:lang="en">Start/Stop tracking when <see cref="ARSession"/> is running. Tracking will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止跟踪。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始跟踪。</para>
        /// </summary>
        private void OnEnable()
        {
            tracker?.start();
        }

        private void OnDisable()
        {
            tracker?.stop();
        }

        private void OnDestroy()
        {
            OnSessionStop();
        }

        /// <summary>
        /// <para xml:lang="en">Sets result post-processing. Overwrites <see cref="EnableMotionFusion"/>. Only effective if modified before the session starts.</para>
        /// <para xml:lang="zh">设置结果后处理。会覆盖<see cref="EnableMotionFusion"/>。在session启动前修改才有效。</para>
        /// </summary>
        public void SetResultPostProcessing(bool enablePersistentTargetInstance) => tracker?.setResultPostProcessing(enablePersistentTargetInstance, EnableMotionFusion);

        internal override void OnSessionStart(ARSession session)
        {
            this.session = session;
            tracker = ObjectTracker.create();
            tracker.setSimultaneousNum(simultaneousNum);
            tracker.setResultPostProcessing(EnableMotionFusion, EnableMotionFusion);

            if (!tracker.setResultAsyncMode(session.Assembly.IsASync))
            {
                EasyARController.LicenseValidation.OnResultSyncModeFail(session.Assembly.FrameSource.GetType());
            }

            foreach (var controller in holdingTargets)
            {
                LoadTarget(controller);
            }
            if (enabled)
            {
                OnEnable();
            }
        }

        internal override void OnSessionStop()
        {
            session = null;
            foreach (var controller in holdingTargets)
            {
                UnloadTarget(controller);
            }
            loadingTargets.Clear();
            allTargetController.Clear();
            targetCenterTransformHelper.Results = null;
            tracker?.Dispose();
            tracker = null;
        }

        FeedbackFrameSink IFeedbackFrameSink.FeedbackFrameSink() => tracker?.feedbackFrameSink();
        OutputFrameSource IOutputFrameSource.OutputFrameSource() => tracker?.outputFrameSource();

        void IOutputFrameSource.OnResult(Optional<FrameFilterResult> frameFilterResult, Optional<MotionInputData> motion)
        {
            OnResult(frameFilterResult.OnSome ? frameFilterResult.Value as TargetTrackerResult : null);
        }

        void IOutputFrameSource.RetrieveSyncResults(Optional<MotionInputData> motion)
        {
            using (var result = EasyARController.IsReady && motion.OnSome ? tracker?.getSyncResult(motion.Value).ValueOrDefault(null) : null)
            {
                OnResult(result);
            }
        }

        private void OnResult(TargetTrackerResult targetTrackerResult)
        {
            var resultControllers = new List<Tuple<MonoBehaviour, Pose>>();
            var targetIDs = new List<int>();
            var lostIDs = new List<int>();

            if (targetTrackerResult != null)
            {
                foreach (var targetInstance in targetTrackerResult.targetInstances())
                {
                    using (targetInstance)
                    using (var target = targetInstance.target())
                    {
                        var controller = TryGetTargetController(target.runtimeID());
                        if (controller && controller.IsLoaded)
                        {
                            targetIDs.Add(target.runtimeID());
                            var pose = targetInstance.pose().ToUnityPose();
                            resultControllers.Add(Tuple.Create((MonoBehaviour)controller, pose));
                        }
                    }
                }
            }

            foreach (var id in previousTargetIDs)
            {
                lostIDs.Add(id);
            }
            foreach (var id in targetIDs)
            {
                if (lostIDs.Contains(id))
                {
                    lostIDs.Remove(id);
                }
                var controller = TryGetTargetController(id);
                if (controller)
                {
                    if (controller.OnTracking(true))
                    {
                        dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Found: {controller.Target?.name()} ({id})"));
                    }
                }
            }
            foreach (var id in lostIDs)
            {
                var controller = TryGetTargetController(id);
                if (controller)
                {
                    if (controller.OnTracking(false))
                    {
                        dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Lost: {controller.Target?.name()} ({id})"));
                    }
                }
            }
            previousTargetIDs = targetIDs;
            targetCenterTransformHelper.Results = resultControllers;
        }

        Optional<Tuple<GameObject, Pose>> ICenterTransform.TryGetCenter(GameObject center) => targetCenterTransformHelper.TryGetCenter(center);

        void ICenterTransform.UpdateTransform(GameObject center, Pose centerPose) => targetCenterTransformHelper.UpdateTransform(center, centerPose);

        void IHFlipHandler.SetHFlip(bool flip)
        {
            if (horizontalFlip != flip)
            {
                horizontalFlip = flip;
                targetCenterTransformHelper.HorizontalFlip = flip;
                foreach (var value in allTargetController.Values)
                {
                    value.HorizontalFlip = flip;
                }
            }
        }

        internal override string DumpLite()
        {
            if (tracker == null) { return null; }

            var data = $"{ARSessionFactory.DefaultName(GetType())}: {enabled}, ({EnableMotionFusion})" + Environment.NewLine;
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

        internal void Hold(ObjectTargetController controller)
        {
            if (holdingTargets.Contains(controller)) { return; }
            holdingTargets.Add(controller);
            if (tracker != null)
            {
                LoadTarget(controller);
            }
        }

        internal void Unhold(ObjectTargetController controller)
        {
            if (!holdingTargets.Contains(controller)) { return; }
            holdingTargets.Remove(controller);
            if (tracker != null) { UnloadTarget(controller); }
        }

        private void LoadTarget(ObjectTargetController controller)
        {
            if (!controller) { return; }

            var coroutine = StartCoroutine(LoadDataAndLoadTarget(controller));
            if (coroutine != null)
            {
                loadingTargets[controller] = coroutine;
            }
        }

        private IEnumerator LoadDataAndLoadTarget(ObjectTargetController controller)
        {
            if (!controller) { yield break; }

            if (controller.LoadingResult.OnNone)
            {
                if (controller.LoadingSource.OnNone)
                {
                    yield return new WaitUntil(() => controller && controller.LoadingSource.OnSome);
                }
                yield return ObjectTargetDataLoader.Load(controller.LoadingSource.Value, (target) =>
                {
                    if (!controller) { return; }
                    controller.OnTargetDataLoad(target);
                }, (error) =>
                {
                    if (!controller) { return; }

                    error = $"fail to load target data from {controller.LoadingSource.Value.GetType()}: {error}";
                    if (session)
                    {
                        session.Diagnostics.EnqueueError(error);
                    }
                    else
                    {
                        Debug.LogError(error);
                    }
                    controller.OnTargetDataLoad(null);
                });
            }

            if (controller && controller.Target != null)
            {
                allTargetController[controller.Target.runtimeID()] = controller;
                tracker.loadTarget(controller.Target, EasyARController.Scheduler, (target, status) =>
                {
                    if (controller && allTargetController.ContainsValue(controller) && status)
                    {
                        controller.OnLoad();
                    }
                    dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Load: {target.name()} ({target.runtimeID()}) => {status}"));
                    TargetLoad?.Invoke(controller, status);
                });
            }
            loadingTargets.Remove(controller);
        }

        private void UnloadTarget(ObjectTargetController controller)
        {
            if (controller.Target != null)
            {
                if (allTargetController.Remove(controller.Target.runtimeID()))
                {
                    controller.OnUnload();
                    tracker.unloadTarget(controller.Target, EasyARController.Scheduler, (target, status) =>
                    {
                        dumpMessages.Enqueue((Time.realtimeSinceStartup, $"Unload: {target.name()} ({target.runtimeID()}) => {status}"));
                        TargetUnload?.Invoke(controller, status);
                    });
                }
            }

            if (loadingTargets.TryGetValue(controller, out var coroutine))
            {
                StopCoroutine(coroutine);
                loadingTargets.Remove(controller);
            }
        }

        private ObjectTargetController TryGetTargetController(int runtimeID)
        {
            if (allTargetController.TryGetValue(runtimeID, out var controller))
            {
                return controller;
            }
            return null;
        }
    }
}
