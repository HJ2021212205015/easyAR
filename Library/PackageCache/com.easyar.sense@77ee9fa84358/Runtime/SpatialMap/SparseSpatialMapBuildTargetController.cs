//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls the map generated from <see cref="SparseSpatialMap"/> in the scene.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="SparseSpatialMap"/>生成的地图的<see cref="MonoBehaviour"/>。</para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SparseSpatialMapPointCloudRenderer))]
    public class SparseSpatialMapBuildTargetController : MonoBehaviour
    {
        internal SparseSpatialMapBuilderFrameFilter Builder;
        private SparseSpatialMapPointCloudRenderer pointCloudRenderer;

        /// <summary>
        /// <para xml:lang="en">Renderer of point cloud.</para>
        /// <para xml:lang="zh">点云渲染器。</para>
        /// </summary>
        public SparseSpatialMapPointCloudRenderer PointCloudRenderer => pointCloudRenderer;

        /// <summary>
        /// <para xml:lang="en">Current point cloud data.</para>
        /// <para xml:lang="zh">当前点云数据。</para>
        /// </summary>
        public List<Vector3> PointCloud { get; private set; } = new();

        private void Awake()
        {
            pointCloudRenderer = GetComponent<SparseSpatialMapPointCloudRenderer>();
        }

        /// <summary>
        /// <para xml:lang="en">Perform hit test against the point cloud. The results are returned sorted by their distance to the camera in ascending order. <paramref name="pointInView"/> should be normalized to [0, 1]^2.</para>
        /// <para xml:lang="zh">在当前点云中进行Hit Test，得到距离相机从近到远一条射线上的n（n>=0）个位置坐标。<paramref name="pointInView"/> 需要被归一化到[0, 1]^2。</para>
        /// </summary>
        public List<Vector3> HitTest(Vector2 pointInView) => Builder ? Builder.HitTestAgainstPointCloud(pointInView) : new();

        internal void UpdatePointCloud(List<Vector3> points)
        {
            PointCloud = points;
            pointCloudRenderer.PointCloud = PointCloud;
        }
    }
}
