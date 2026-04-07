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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="DenseSpatialMap"/> in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="DenseSpatialMap"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
    public class DenseSpatialMapBuilderFrameFilter : FrameFilter, FrameFilter.IInputFrameSink
    {
        /// <summary>
        /// <para xml:lang="en"><see cref="Material"/> for map mesh render. Only effective if modified before the session starts. Mesh transparency is not enabled in URP by now when using default material.</para>
        /// <para xml:lang="zh">用于渲染Map网格的<see cref="Material"/>。在session启动前修改才有效。在当前版本中，使用URP时默认材质的透明显示未开启。</para>
        /// </summary>
        public Material MapMeshMaterial;

        /// <summary>
        /// <para xml:lang="en">The target maximum update time per frame in milliseconds. Modify at any time and takes effect immediately.</para>
        /// <para xml:lang="en">The real time used each frame may differ from this value and a minimum amount fo data is ensured to be updated no matter what the value is. No extra time will be used if data does need to update. Decrease this value if the mesh update slows rendering.</para>
        /// <para xml:lang="zh">目标的每帧最长更新时间（毫秒）。可随时修改，立即生效。</para>
        /// <para xml:lang="zh">实际每帧使用的时间可能与这个数值有所差异，无论数值设置成多少，每帧都会至少更新一部分数据。如果数据不需要更新则不会耗费额外时间。如果网格更新使渲染变慢可以降低这个数值。</para>
        /// </summary>
        public int TargetMaxUpdateTimePerFrame = 10;

        /// <summary>
        /// <para xml:lang="en">Whether to create mesh collider on the mesh created. Only effective if modified before the session starts.</para>
        /// <para xml:lang="zh">是否在生成的mesh上创建mesh collider。在session启动前修改才有效。</para>
        /// </summary>
        public bool EnableMeshCollider = true;

        private DenseSpatialMap builder;
        private Dictionary<Vector3, DenseSpatialMapBlockController> blocksDict = new();
        private GameObject mapRoot;
        [SerializeField, HideInInspector]
        private bool renderMesh = true;
        private Material mapMaterial;
        private bool enableMeshCollider;
        private PendingData pendingData;
        private System.Diagnostics.Stopwatch updateTimer = new();

        private Camera depthRenderCamera;
        private Shader depthShader;
        private RenderTexture depthTexture;
        private bool disableTranslucent;

        /// <summary>
        /// <para xml:lang="en">Event when a new mesh block created.</para>
        /// <para xml:lang="zh">新网格块创建的事件。</para>
        /// </summary>
        public event Action<DenseSpatialMapBlockController> MeshBlockCreate;
        /// <summary>
        /// <para xml:lang="en">Event when mesh block updates.</para>
        /// <para xml:lang="zh">网格块更新的事件。</para>
        /// </summary>
        public event Action<List<DenseSpatialMapBlockController>> MeshBlockUpdate;

        /// <summary>
        /// <para xml:lang="en">Mesh render on/off.</para>
        /// <para xml:lang="zh">是否渲染网格。</para>
        /// </summary>
        public bool RenderMesh
        {
            get => renderMesh;
            set
            {
                renderMesh = value;
                foreach (var block in blocksDict)
                {
                    block.Value.GetComponent<MeshRenderer>().enabled = renderMesh;
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Mesh color. Only effective if modified after the session starts.</para>
        /// <para xml:lang="en">Alpha is ingored when using URP or running in HMD when default shader is in use.</para>
        /// <para xml:lang="zh">网格颜色。在session启动后修改才有效。</para>
        /// <para xml:lang="zh">使用默认shader时，透明通道在URP或头显上运行时会被忽略。</para>
        /// </summary>
        public Color MeshColor
        {
            get => mapMaterial ? mapMaterial.color : Color.black;
            set
            {
                if (!mapMaterial) { return; }
                var color = value;
                if (disableTranslucent && color.a != 1 && color != Color.clear)
                {
                    color.a = 1f;
                    Debug.LogWarning("translucent disabled, ignore color alpha");
                }
                mapMaterial.color = color;
            }
        }

        /// <summary>
        /// <para xml:lang="en">All mesh blocks.</para>
        /// <para xml:lang="zh">当前所有网格块。</para>
        /// </summary>
        public List<DenseSpatialMapBlockController> MeshBlocks => blocksDict.Select(item => item.Value).ToList();

        internal override bool IsAvailable => DenseSpatialMap.isAvailable();
        internal override int BufferRequirement => builder.bufferRequirement();

        private void Awake()
        {
            depthShader = Shader.Find("EasyAR/DenseSpatialMapDepth");
        }

        /// <summary>
        /// <para xml:lang="en">Start/Stop building when <see cref="ARSession"/> is running. Building will start only when <see cref="MonoBehaviour"/>.enabled is true after session started.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时开始/停止建图。在session启动后，<see cref="MonoBehaviour"/>.enabled为true时才会开始建图。</para>
        /// </summary>
        private void OnEnable()
        {
            builder?.start();
        }

        private void Update()
        {
            if (builder == null) { return; }
            if (pendingData == null)
            {
                if (builder.updateSceneMesh(false))
                {
                    var sceneMesh = builder.getMesh();
                    pendingData = new PendingData
                    {
                        Stage = PendingData.UpdateStage.UpdateInfo,
                        SceneMesh = sceneMesh,
                        PendingInfo = sceneMesh.getBlocksInfoIncremental(),
                        PendingController = new()
                    };
                }
                if (pendingData == null) { return; }
            }

            updateTimer.Restart();
            if (pendingData.Stage == PendingData.UpdateStage.UpdateInfo)
            {
                if (pendingData.PendingInfo != null && pendingData.PendingInfo.Count > 0)
                {
                    var usedInfo = new List<BlockInfo>();
                    try
                    {
                        foreach (var blockInfo in pendingData.PendingInfo)
                        {
                            if (updateTimer.ElapsedMilliseconds > TargetMaxUpdateTimePerFrame && usedInfo.Count >= 3) { break; }

                            usedInfo.Add(blockInfo);
                            blocksDict.TryGetValue(new Vector3(blockInfo.x, blockInfo.y, blockInfo.z), out var oldBlock);

                            if (blockInfo.numOfVertex == 0 || blockInfo.numOfIndex == 0)
                            {
                                if (oldBlock)
                                {
                                    blocksDict.Remove(new Vector3(blockInfo.x, blockInfo.y, blockInfo.z));
                                    Destroy(oldBlock.gameObject);
                                }
                                continue;
                            }

                            if (oldBlock == null)
                            {
                                var go = new GameObject("MeshBlock");
                                go.AddComponent<MeshCollider>();
                                go.AddComponent<MeshFilter>();
                                var renderer = go.AddComponent<MeshRenderer>();
                                renderer.material = mapMaterial;
                                renderer.enabled = RenderMesh;
                                var block = go.AddComponent<DenseSpatialMapBlockController>();
                                block.UpdateData(blockInfo, pendingData.SceneMesh);
                                go.transform.SetParent(mapRoot.transform, false);
                                blocksDict.Add(new Vector3(blockInfo.x, blockInfo.y, blockInfo.z), block);
                                pendingData.PendingController.Add(block);
                                MeshBlockCreate?.Invoke(block);
                            }
                            else if (oldBlock.Info.version != blockInfo.version)
                            {
                                oldBlock.UpdateData(blockInfo, pendingData.SceneMesh);
                                pendingData.PendingController.Add(oldBlock);
                            }
                        }
                    }
                    finally
                    {
                        foreach (var info in usedInfo)
                        {
                            pendingData.PendingInfo.Remove(info);
                        }
                    }
                }

                if (pendingData.PendingInfo != null && pendingData.PendingInfo.Count > 0)
                {
                    return;
                }
                else
                {
                    pendingData.Stage = PendingData.UpdateStage.UpdateMeshFilter;
                }
            }

            if (pendingData.Stage == PendingData.UpdateStage.UpdateMeshFilter)
            {
                if (pendingData.PendingController != null)
                {
                    foreach (var block in pendingData.PendingController)
                    {
                        block.UpdateMeshFilter();
                    }
                }

                if (enableMeshCollider)
                {
                    pendingData.Stage = PendingData.UpdateStage.UpdateMeshCollider;
                    return;
                }
                else
                {
                    if (pendingData.PendingController != null && pendingData.PendingController.Count > 0)
                    {
                        MeshBlockUpdate?.Invoke(pendingData.PendingController);
                    }
                    pendingData.Stage = PendingData.UpdateStage.Finish;
                }
            }


            if (pendingData.Stage == PendingData.UpdateStage.UpdateMeshCollider)
            {
                if (pendingData.PendingController != null && pendingData.PendingController.Count > 0)
                {
                    var usedController = new List<DenseSpatialMapBlockController>();
                    try
                    {
                        foreach (var block in pendingData.PendingController)
                        {
                            if (updateTimer.ElapsedMilliseconds > TargetMaxUpdateTimePerFrame && usedController.Count >= 3) { break; }

                            usedController.Add(block);
                            block.UpdateMeshCollider();
                        }
                    }
                    finally
                    {
                        foreach (var info in usedController)
                        {
                            pendingData.PendingController.Remove(info);
                        }

                        MeshBlockUpdate?.Invoke(usedController);
                    }
                }

                if (pendingData.PendingController != null && pendingData.PendingController.Count > 0)
                {
                    return;
                }
                else
                {
                    pendingData.Stage = PendingData.UpdateStage.Finish;
                }
            }

            if (pendingData.Stage == PendingData.UpdateStage.Finish)
            {
                pendingData.Dispose();
                pendingData = null;
            }
        }

        private void OnDisable()
        {
            builder?.stop();
        }

        private void OnDestroy()
        {
            OnSessionStop();
        }

        internal override void OnSessionStart(ARSession session)
        {
            mapRoot = new GameObject("DenseSpatialMapRoot");
            if (session.Assembly.DefaultOriginChild.OnSome && session.Assembly.DefaultOriginChild.Value)
            {
                mapRoot.transform.SetParent(session.Assembly.DefaultOriginChild.Value.transform, false);
            }
            builder = DenseSpatialMap.create();

            enableMeshCollider = EnableMeshCollider;
            (mapMaterial, disableTranslucent) = SetupMaterial(MapMeshMaterial, session.Assembly.FrameSource.IsHMD || SystemUtil.RenderPipeline == SystemUtil.RenderPipelineType.URP);
            if (!disableTranslucent)
            {
                depthRenderCamera = session.Assembly.Camera;
                Application.onBeforeRender += RenderDepth;
            }

            if (enabled) { OnEnable(); }
        }

        internal override void OnSessionStop()
        {
            if (mapRoot) { Destroy(mapRoot); }
            mapRoot = null;
            blocksDict.Clear();
            builder?.Dispose();
            builder = null;
            pendingData?.Dispose();
            pendingData = null;
            Application.onBeforeRender -= RenderDepth;
            if (mapMaterial) { Destroy(mapMaterial); }
            if (depthTexture) { Destroy(depthTexture); }
            depthRenderCamera = null;
            disableTranslucent = false;
        }

        InputFrameSink IInputFrameSink.InputFrameSink() => builder?.inputFrameSink();

        internal override string DumpLite()
        {
            if (builder == null) { return null; }

            var data = $"{ARSessionFactory.DefaultName(GetType())}: {enabled}, {TargetMaxUpdateTimePerFrame}, {RenderMesh} ({MeshColor}), {enableMeshCollider}, {MeshBlocks.Count}" + Environment.NewLine;
            return data;
        }

        private static (Material, bool) SetupMaterial(Material material, bool disableDepth)
        {
            if (!material) { return (null, false); }
            var mapMaterial = Instantiate(material);

            if (mapMaterial.shader == Shader.Find("EasyAR/DenseSpatialMapMesh"))
            {
                if (SystemUtil.RenderPipeline == SystemUtil.RenderPipelineType.Builtin)
                {
                    mapMaterial.SetShaderPassEnabled("UniversalForward", false);
                    mapMaterial.SetShaderPassEnabled("ForwardBase", true);
                }
                else if (SystemUtil.RenderPipeline == SystemUtil.RenderPipelineType.URP)
                {
                    mapMaterial.SetShaderPassEnabled("UniversalForward", true);
                    mapMaterial.SetShaderPassEnabled("ForwardBase", false);
                }

                mapMaterial.SetInt("_UseDepthTexture", disableDepth ? 0 : 1);
                if (disableDepth)
                {
                    var c = mapMaterial.color;
                    mapMaterial.color = new Color(c.r, c.g, c.b);
                    Debug.LogWarning("translucent disabled, ignore color alpha");
                }
                return (mapMaterial, disableDepth);
            }
            return (mapMaterial, false);
        }

        [BeforeRenderOrder(100)]
        private void RenderDepth()
        {
            if (!RenderMesh) { return; }
            if (!depthRenderCamera || !mapMaterial) { return; }

            var size = new Vector2Int((int)(Screen.width * depthRenderCamera.rect.width), (int)(Screen.height * depthRenderCamera.rect.height));
            if (depthTexture && (depthTexture.width != size.x || depthTexture.height != size.y))
            {
                Destroy(depthTexture);
            }
            if (!depthTexture)
            {
                depthTexture = new RenderTexture(size.x, size.y, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                mapMaterial.SetTexture("_DepthTexture", depthTexture);
            }
            depthRenderCamera.targetTexture = depthTexture;
            depthRenderCamera.RenderWithShader(depthShader, "Tag");
            depthRenderCamera.targetTexture = null;
        }

        private class PendingData : IDisposable
        {
            public UpdateStage Stage;
            public List<BlockInfo> PendingInfo;
            public List<DenseSpatialMapBlockController> PendingController;
            public SceneMesh SceneMesh;

            public enum UpdateStage
            {
                UpdateInfo,
                UpdateMeshFilter,
                UpdateMeshCollider,
                Finish,
            }

            ~PendingData()
            {
                SceneMesh?.Dispose();
            }

            public void Dispose()
            {
                SceneMesh?.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
