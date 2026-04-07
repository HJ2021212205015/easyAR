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
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Render sparse spatial map point cloud as particles.</para>
    /// <para xml:lang="zh">渲染稀疏空间地图点云成粒子。</para>
    /// </summary>
    [DisallowMultipleComponent]
    public class SparseSpatialMapPointCloudRenderer : MonoBehaviour
    {
        [HideInInspector, SerializeField]
        private bool show = true;
        [HideInInspector, SerializeField]
        private ParticleSystem pointCloudParticleSystem;
        [HideInInspector, SerializeField]
        private PointCloudParticleParameter particleParameter = new();
        private List<Vector3> pointCloud = new();

        /// <summary>
        /// <para xml:lang="en">The <see cref="UnityEngine.ParticleSystem"/> used for point cloud rendering. Modify at any time and takes effect immediately.</para>
        /// <para xml:lang="zh">渲染点云的<see cref="UnityEngine.ParticleSystem"/>。可随时修改，立即生效。</para>
        /// </summary>
        public ParticleSystem ParticleSystem
        {
            get => pointCloudParticleSystem;
            set
            {
                pointCloudParticleSystem = value;
                UpdateParticleSystem();
            }
        }

        /// <summary>
        /// <para xml:lang="en">Parameters for point cloud particles rendering. Modify at any time and takes effect immediately.</para>
        /// <para xml:lang="zh">渲染点云粒子的参数。可随时修改，立即生效。</para>
        /// </summary>
        public PointCloudParticleParameter ParticleParameter
        {
            get => particleParameter;
            set
            {
                particleParameter = value;
                UpdateParticleSystem();
            }
        }
        /// <summary>
        /// <para xml:lang="en">Show or hide point cloud. Modify at any time and takes effect immediately.</para>
        /// <para xml:lang="zh">显示或隐藏点云。可随时修改，立即生效。</para>
        /// </summary>
        public bool Show
        {
            get => show;
            set
            {
                show = value;
                UpdateParticleSystem();
            }
        }

        internal List<Vector3> PointCloud
        {
            set
            {
                pointCloud = value;
                UpdateParticleSystem();
            }
        }

        private void UpdateParticleSystem()
        {
            if (!pointCloudParticleSystem) { return; }

            if (!show || pointCloud == null)
            {
                pointCloudParticleSystem.Clear();
                return;
            }

            var particles = pointCloud.Select(p =>
            new ParticleSystem.Particle
            {
                position = p,
                startLifetime = particleParameter.StartLifetime,
                remainingLifetime = particleParameter.RemainingLifetime,
                startSize = particleParameter.StartSize,
                startColor = particleParameter.StartColor
            }).ToArray();
            pointCloudParticleSystem.SetParticles(particles, particles.Length);
        }

        /// <summary>
        /// <para xml:lang="en">Parameters for point cloud particles rendering.</para>
        /// <para xml:lang="zh">渲染点云粒子的参数。</para>
        /// </summary>
        [Serializable]
        public class PointCloudParticleParameter
        {
            /// <summary>
            /// <para xml:lang="en">Particles start color.</para>
            /// <para xml:lang="zh">粒子初始颜色。</para>
            /// </summary>
            public Color32 StartColor = new Color32(11, 205, 255, 255);
            /// <summary>
            /// <para xml:lang="en">Particles start size.</para>
            /// <para xml:lang="zh">粒子初始大小。</para>
            /// </summary>
            public float StartSize = 0.015f;
            /// <summary>
            /// <para xml:lang="en">Particles start life time.</para>
            /// <para xml:lang="zh">粒子初始生存时间。</para>
            /// </summary>
            public float StartLifetime = float.MaxValue;
            /// <summary>
            /// <para xml:lang="en">Particles remaining life time.</para>
            /// <para xml:lang="zh">粒子剩余生存时间。</para>
            /// </summary>
            public float RemainingLifetime = float.MaxValue;
        }
    }
}
