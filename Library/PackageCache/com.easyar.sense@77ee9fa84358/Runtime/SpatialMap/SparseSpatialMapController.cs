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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls the map tracked by <see cref="SparseSpatialMap"/> in the scene.</para>
    /// <para xml:lang="zh">在场景中由<see cref="SparseSpatialMap"/>跟踪的地图的<see cref="MonoBehaviour"/>。</para>
    /// </summary>
    [RequireComponent(typeof(SparseSpatialMapPointCloudRenderer))]
    public class SparseSpatialMapController : TargetController
    {
        // for serialization backward compatibility, do not change field name including letter case
        [HideInInspector, SerializeField]
        private ParticleSystem PointCloudParticleSystem;
        [HideInInspector, SerializeField]
        private int SourceType;

        [HideInInspector, SerializeField]
        private DataSource sourceType;
        [HideInInspector, SerializeField]
        private MapManagerSourceData mapManagerSource = new();
        [HideInInspector, SerializeField]
        private bool trackerHasSet;
        [HideInInspector, SerializeField]
        private SparseSpatialMapTrackerFrameFilter tracker;
        private SparseSpatialMapTrackerFrameFilter holder;
        private bool hasEverDirectlyTracked;
        private SparseSpatialMapPointCloudRenderer pointCloudRenderer;

        internal enum DataSource
        {
            MapManager,
        }

        /// <summary>
        /// <para xml:lang="en">Renderer of point cloud.</para>
        /// <para xml:lang="zh">点云渲染器。</para>
        /// </summary>
        public SparseSpatialMapPointCloudRenderer PointCloudRenderer => pointCloudRenderer;

        /// <summary>
        /// <para xml:lang="en">Map info. Only usable after <see cref="MonoBehaviour"/>.Start.</para>
        /// <para xml:lang="zh">地图信息，仅在<see cref="MonoBehaviour"/>.Start之后可用。</para>
        /// </summary>
        public SparseSpatialMapInfo Info { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Point cloud data. Only usable after <see cref="TargetController.TargetFound"/>.</para>
        /// <para xml:lang="zh">点云数据。仅在<see cref="TargetController.TargetFound"/>之后可用。</para>
        /// </summary>
        public List<Vector3> PointCloud { get; private set; } = new();

        /// <summary>
        /// <para xml:lang="en">Data source for map creation. Only effective if modified before <see cref="MonoBehaviour"/>.Start.</para>
        /// <para xml:lang="zh">创建map的数据来源。在<see cref="MonoBehaviour"/>.Start前修改才有效。</para>
        /// </summary>
        public SourceData Source
        {
            get => sourceType switch
            {
                DataSource.MapManager => mapManagerSource,
                _ => throw new ArgumentOutOfRangeException(),
            };
            set
            {
                if (value is MapManagerSourceData mapManagerSource)
                {
                    sourceType = DataSource.MapManager;
                    this.mapManagerSource = mapManagerSource;
                }
                else
                {
                    throw new InvalidOperationException("Invalid source data type: " + value?.GetType());
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">The <see cref="SparseSpatialMapTrackerFrameFilter"/> which loads the map. When set to null, the map will be unloaded from MapWorker previously set. Modify at any time, the load will happen only when the session is running.</para>
        /// <para xml:lang="zh">加载target的<see cref="SparseSpatialMapTrackerFrameFilter"/>。如果设为null，map将会被从之前设置的MapWorker中卸载。可随时修改，加载只会在session运行时发生。</para>
        /// </summary>
        public SparseSpatialMapTrackerFrameFilter Tracker
        {
            get => tracker;
            set
            {
                tracker = value;
                if (Application.isPlaying)
                {
                    UpdateHolder();
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Is target being tracked directly.</para>
        /// <para xml:lang="zh">目标是否被直接跟踪。</para>
        /// </summary>
        public bool IsDirectlyTracked { get; private set; }

        internal Optional<SourceData> LoadingSource { get; private set; }
        internal bool IsLoaded { get; private set; } // NOTE: EasyAR Sense multi-thread callback is dispatched to game thread by DelayedCallbackScheduler, this flag is used to keep load-track order.

        internal Material ObsoleteBuildTargetMaterial => (SourceType == 0 && PointCloudParticleSystem && PointCloudParticleSystem.GetComponent<Renderer>() && PointCloudParticleSystem.GetComponent<Renderer>().material) ? PointCloudParticleSystem.GetComponent<Renderer>().material : null;

        private protected override void Awake()
        {
            base.Awake();
            pointCloudRenderer = GetComponent<SparseSpatialMapPointCloudRenderer>();
            if (!pointCloudRenderer)
            {
                // for backward compatibility
                pointCloudRenderer = gameObject.AddComponent<SparseSpatialMapPointCloudRenderer>();
            }
        }

        private protected override void Start()
        {
            base.Start();

            LoadingSource = Source.TrimClone();
            if (LoadingSource.Value is MapManagerSourceData mapManagerSource)
            {
                Info = new SparseSpatialMapInfo() { ID = mapManagerSource.ID, Name = mapManagerSource.Name };
            }
            UpdateHolder();
        }

        private protected override void OnDestroy()
        {
            base.OnDestroy();
            if (tracker)
            {
                tracker = null;
                UpdateHolder();
            }
            Info = null;
        }

        /// <summary>
        /// <para xml:lang="en">Perform hit test against the point cloud. The results are returned sorted by their distance to the camera in ascending order. <paramref name="pointInView"/> should be normalized to [0, 1]^2.</para>
        /// <para xml:lang="en">Only usable when <see cref="IsDirectlyTracked"/> is true.</para>
        /// <para xml:lang="zh">在当前点云中进行Hit Test，得到距离相机从近到远一条射线上的n（n>=0）个位置坐标。<paramref name="pointInView"/> 需要被归一化到[0, 1]^2。</para>
        /// <para xml:lang="zh">仅在<see cref="IsDirectlyTracked"/>为true时可用。</para>
        /// </summary>
        public List<Vector3> HitTest(Vector2 pointInView) => IsDirectlyTracked && tracker != null ? tracker.HitTestAgainstPointCloud(pointInView) : new();

        internal void OnLoad()
        {
            IsLoaded = true;
        }

        internal void OnUnload()
        {
            UpdatePointCloud(null);
            OnDirectTracking(false, false);
            IsLoaded = false;
            hasEverDirectlyTracked = false;
        }

        internal bool OnDirectTracking(bool status, bool enableIndirect)
        {
            IsDirectlyTracked = status;
            if (!hasEverDirectlyTracked && status) { hasEverDirectlyTracked = true; }
            return OnTracking(status || (hasEverDirectlyTracked && enableIndirect));
        }

        internal void UpdatePointCloud(List<Vector3> points)
        {
            PointCloud = points;
            pointCloudRenderer.PointCloud = PointCloud;
        }

        private void UpdateHolder()
        {
            if (holder == tracker) { return; }
            if (holder) { holder.Unhold(this); }
            holder = tracker;
            if (holder) { holder.Hold(this); }
        }

        /// <summary>
        /// <para xml:lang="en">Sparse map information.</para>
        /// <para xml:lang="zh">稀疏地图信息。</para>
        /// </summary>
        public class SparseSpatialMapInfo
        {
            /// <summary>
            /// <para xml:lang="en">Sparse map ID.</para>
            /// <para xml:lang="zh">稀疏地图的ID。</para>
            /// </summary>
            public string ID = string.Empty;
            /// <summary>
            /// <para xml:lang="en">Sparse map name.</para>
            /// <para xml:lang="zh">稀疏地图的名字。</para>
            /// </summary>
            public string Name = string.Empty;
        }

        /// <summary>
        /// <para xml:lang="en">Data for target creation.</para>
        /// <para xml:lang="zh">创建target的数据。</para>
        /// </summary>
        [Serializable]
        public abstract class SourceData
        {
            internal abstract SourceData TrimClone();
        }

        /// <summary>
        /// <para xml:lang="en">MapManager source for map creation.</para>
        /// <para xml:lang="zh">创建map的MapManager来源。</para>
        /// </summary>
        [Serializable]
        public class MapManagerSourceData : SourceData
        {
            /// <summary>
            /// <para xml:lang="en">Sparse map ID.</para>
            /// <para xml:lang="zh">稀疏地图的ID。</para>
            /// </summary>
            public string ID = string.Empty;
            /// <summary>
            /// <para xml:lang="en">Sparse map name.</para>
            /// <para xml:lang="zh">稀疏地图的名字。</para>
            /// </summary>
            public string Name = string.Empty;

            internal override SourceData TrimClone() => new MapManagerSourceData
            {
                ID = ID?.Trim() ?? string.Empty,
                Name = Name,
            };
        }

    }
}
