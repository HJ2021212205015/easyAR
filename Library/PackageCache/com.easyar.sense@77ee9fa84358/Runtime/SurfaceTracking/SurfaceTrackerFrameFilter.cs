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
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="SurfaceTracker"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="SurfaceTracker"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
    public class SurfaceTrackerFrameFilter : FrameFilter, FrameFilter.IInputFrameSink, FrameFilter.IOutputFrameSource, FrameFilter.ICenterTransform
    {
        private SurfaceTracker tracker;
        [HideInInspector, SerializeField]
        private SurfaceTargetController target;
        private TargetCenterTransformHelper targetCenterTransformHelper = new();
        private ARSession session;

        /// <summary>
        /// <para xml:lang="en">The object Camera move against.</para>
        /// <para xml:lang="en">If it is not set, a new object will be generated automatically in session start.</para>
        /// <para xml:lang="zh">相机运动的相对物体。</para>
        /// <para xml:lang="zh">如未设置，一个新的object会在session启动时自动生成。</para>
        /// </summary>
        public SurfaceTargetController Target
        {
            get => target;
            set
            {
                if (tracker != null) { return; }
                target = value;
            }
        }

        internal override bool IsAvailable => SurfaceTracker.isAvailable();
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

        internal override void OnSessionStart(ARSession session)
        {
            this.session = session;
            tracker = SurfaceTracker.create();
            if (target && !target.gameObject.scene.IsValid())
            {
                target = Instantiate(target);
            }
            if (!target)
            {
                // do not destroy in case user data in it, there will be no leak because it is a scene object
                var gameObject = new GameObject("SurfaceTarget", typeof(SurfaceTargetController));
                target = gameObject.GetComponent<SurfaceTargetController>();
            }
            target.Load(this);
            if (enabled)
            {
                OnEnable();
            }
        }

        internal override void OnSessionStop()
        {
            if (target)
            {
                target.UnLoad();
            }
            targetCenterTransformHelper.Results = null;
            session = null;
            tracker?.Dispose();
            tracker = null;
        }

        internal void AlignTargetToViewPoint(Vector2 pointInView)
        {
            if (!session || tracker == null) { return; }

            var coord = session.ImageCoordinatesFromScreenCoordinates(pointInView);
            if (coord.OnNone) { return; }

            tracker.alignTargetToCameraImagePoint(coord.Value.ToEasyARVector());
        }

        InputFrameSink IInputFrameSink.InputFrameSink() => tracker?.inputFrameSink();
        OutputFrameSource IOutputFrameSource.OutputFrameSource() => tracker?.outputFrameSource();

        void IOutputFrameSource.OnResult(Optional<FrameFilterResult> frameFilterResult, Optional<MotionInputData> motion)
        {
            var list = new List<Tuple<MonoBehaviour, Pose>>();
            if (target)
            {
                if (frameFilterResult.OnSome)
                {
                    var result = frameFilterResult.Value as SurfaceTrackerResult;
                    var pose = result.transform().ToUnityPose().Inverse();
                    list.Add(Tuple.Create((MonoBehaviour)target, pose));
                    target.OnTracking(true);
                }
                else
                {
                    target.OnTracking(false);
                }
            }
            targetCenterTransformHelper.Results = list;
        }

        void IOutputFrameSource.RetrieveSyncResults(Optional<MotionInputData> motion) { }

        Optional<Tuple<GameObject, Pose>> ICenterTransform.TryGetCenter(GameObject center) => targetCenterTransformHelper.TryGetCenter(center);

        void ICenterTransform.UpdateTransform(GameObject center, Pose centerPose) => targetCenterTransformHelper.UpdateTransform(center, centerPose);
    }
}

