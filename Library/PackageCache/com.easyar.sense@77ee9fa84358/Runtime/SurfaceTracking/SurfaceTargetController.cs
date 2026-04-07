//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls surface target in the scene. The surface target is a virtual node, representing the relative node when the camera moves in surface tracking.</para>
    /// <para xml:lang="zh">在场景中控制surface target的<see cref="MonoBehaviour"/>。surface target是一个虚拟的节点，它表示在表面跟踪中，camera移动的相对节点。</para>
    /// </summary>
    public class SurfaceTargetController : TargetController
    {
        private SurfaceTrackerFrameFilter tracker;

        /// <summary>
        /// <para xml:lang="en">Sets the tracking target to a point on camera image. <paramref name="pointInView"/> should be normalized to [0, 1]^2.</para>
        /// <para xml:lang="en">Only usable when it is <see cref="SurfaceTrackerFrameFilter.Target"/>.</para>
        /// <para xml:lang="zh">将跟踪目标点对准到相机图像的指定点。<paramref name="pointInView"/> 需要被归一化到[0, 1]^2。</para>
        /// <para xml:lang="zh">仅在自身是<see cref="SurfaceTrackerFrameFilter.Target"/>时可用。</para>
        /// </summary>
        public void AlignTo(Vector2 pointInView)
        {
            if (!tracker) { return; }
            tracker.AlignTargetToViewPoint(pointInView);
        }

        internal void Load(SurfaceTrackerFrameFilter tracker)
        {
            this.tracker = tracker;
        }

        internal void UnLoad()
        {
            tracker = null;
            OnTracking(false);
        }
    }
}
