//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;
using UnityEngine.Rendering;
#if EASYAR_URP_ENABLE
using UnityEngine.Rendering.Universal;
#if EASYAR_URP_17_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
#endif
#else
using ScriptableRendererFeature = UnityEngine.ScriptableObject;
#endif

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">A render feature for rendering the camera image for AR devies when URP in used. Add this render feature to the renderer feature list in forward renderer asset.</para>
    /// <para xml:lang="zh">使用URP时用来渲染AR设备相机图像的render feature。需要在forward renderer asset的renderer feature 列表中添加这个render feature。</para>
    /// </summary>
    public class EasyARCameraImageRendererFeature : ScriptableRendererFeature
    {
        internal static bool IsActive { get; private set; }

#if EASYAR_URP_ENABLE
        CameraImageRenderPass renderPass;
        CameraImageRenderPass renderPassUser;
#if EASYAR_URP_13_1_OR_NEWER
        Optional<RTHandleSystem> rtHandleSystem;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (rtHandleSystem.OnSome)
                {
                    rtHandleSystem.Value.Dispose();
                    rtHandleSystem = null;
                }
            }
        }
#endif

        public override void Create()
        {
            IsActive = true;
            renderPass = new CameraImageRenderPass();
            renderPassUser = new CameraImageRenderPass(true);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            Camera camera = renderingData.cameraData.camera;
            if (!camera) { return; }

            var imageRenderer = CameraImageRenderer.TryGetRenderer(camera);
            if (!imageRenderer || !imageRenderer.Material) { return; }

            if (imageRenderer.enabled)
            {
#if EASYAR_URP_17_OR_NEWER
                renderPass.Setup(imageRenderer);
#else
                renderPass.Setup(imageRenderer.Material, imageRenderer.InvertCulling);
#endif
                renderer.EnqueuePass(renderPass);
            }

            if (imageRenderer.UserTexture.OnSome)
            {
#if EASYAR_URP_13_1_OR_NEWER
                if (rtHandleSystem.OnNone)
                {
                    rtHandleSystem = new RTHandleSystem();
                    int w = renderingData.cameraData.cameraTargetDescriptor.width;
                    int h = renderingData.cameraData.cameraTargetDescriptor.height;
                    rtHandleSystem.Value.Initialize(w, h);
                }
                renderPassUser.SetupRTHandleSystem(rtHandleSystem.Value);
#endif
#if EASYAR_URP_17_OR_NEWER
                renderPassUser.Setup(imageRenderer);
#else
                renderPassUser.Setup(imageRenderer.Material, imageRenderer.InvertCulling);
                renderPassUser.SetupTarget(imageRenderer.UserTexture.Value);
#endif
                renderer.EnqueuePass(renderPassUser);
            }
        }

#if EASYAR_URP_17_OR_NEWER
        class CameraImageRenderPass : ScriptableRenderPass
        {
            static readonly Matrix4x4 projection = Matrix4x4.Ortho(0f, 1f, 0f, 1f, -0.1f, 9.9f);
            const string kRenderGraphDisablePassName = "EasyAR Camera Image Render Pass (Render Graph Disabled)";
            const string kRenderGraphEnablePassName = "EasyAR Camera Image Render Pass (Render Graph Enabled)";
            const string kRenderGraphEnableTexturePassName = "EasyAR Camera Image Texture Render Pass (Render Graph Enabled)";

            readonly Mesh mesh;
            bool textureTargetFlag = false;
            Optional<RTHandleSystem> rtHandleSystem;
            PassData renderPassData = new PassData();
            CameraImageRenderer imageRenderer;

            public CameraImageRenderPass(bool toTextureTarget = false)
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
#if !UNITY_6000_2_OR_NEWER
                bool workaround = !EasyARSettings.Instance || EasyARSettings.Instance.WorkaroundForUnity.URP17RG_DX11_RuinedScene;
                if (workaround && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
                {
                    renderPassEvent = RenderPassEvent.BeforeRendering;
                }
#endif
                mesh = new Mesh
                {
                    vertices = new Vector3[]
                    {
                        new Vector3(0f, 0f, 0.1f),
                        new Vector3(0f, 1f, 0.1f),
                        new Vector3(1f, 1f, 0.1f),
                        new Vector3(1f, 0f, 0.1f),
                    },
                    uv = new Vector2[]
                    {
                        new Vector2(0f, 0f),
                        new Vector2(0f, 1f),
                        new Vector2(1f, 1f),
                        new Vector2(1f, 0f),
                    },
                    triangles = new int[] { 0, 1, 2, 0, 2, 3 }
                };
                textureTargetFlag = toTextureTarget;
            }

            public void Setup(CameraImageRenderer renderer) => imageRenderer = renderer;

            public void SetupRTHandleSystem(RTHandleSystem system) => rtHandleSystem = system;

            static void ExecuteRenderPass(RasterCommandBuffer rasterCommandBuffer, PassData passData)
            {
                rasterCommandBuffer.SetInvertCulling(passData.invertCulling);
                rasterCommandBuffer.SetViewProjectionMatrices(Matrix4x4.identity, projection);
                rasterCommandBuffer.DrawMesh(passData.mesh, Matrix4x4.identity, passData.material);
                rasterCommandBuffer.SetViewProjectionMatrices(passData.worldToCameraMatrix, passData.projectionMatrix);
            }

            static void ExecuteRasterRenderGraphPass(PassData passData, RasterGraphContext rasterContext) => ExecuteRenderPass(rasterContext.cmd, passData);

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                string renderGraphName = textureTargetFlag ? kRenderGraphEnableTexturePassName : kRenderGraphEnablePassName;
                using (var builder = renderGraph.AddRasterRenderPass(renderGraphName, out renderPassData, profilingSampler))
                {
                    UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

                    var cameraDesc = cameraData.cameraTargetDescriptor;

                    renderPassData.worldToCameraMatrix = cameraData.camera.worldToCameraMatrix;
                    renderPassData.projectionMatrix = cameraData.camera.projectionMatrix;
                    renderPassData.invertCulling = imageRenderer.InvertCulling;
                    renderPassData.mesh = mesh;
                    renderPassData.material = imageRenderer.Material;

                    builder.AllowGlobalStateModification(true);
                    builder.AllowPassCulling(false);

                    if (textureTargetFlag)
                    {
                        if (imageRenderer.UserTexture.OnSome)
                        {
                            RTHandle destinationRtHandle = rtHandleSystem.Value.Alloc(imageRenderer.UserTexture.Value);
                            TextureHandle destinationTextureHandle = renderGraph.ImportTexture(destinationRtHandle);
                            builder.SetRenderAttachment(destinationTextureHandle, 0);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
#if !UNITY_6000_2_OR_NEWER
                        bool workaround = !EasyARSettings.Instance || EasyARSettings.Instance.WorkaroundForUnity.URP17RG_DX11_RuinedScene;
                        if (!(workaround && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11))
                        {
                            builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture, 0);
                        }
#endif
                        builder.SetRenderAttachment(resourceData.activeColorTexture, 0);
                    }
                    builder.SetRenderFunc<PassData>(ExecuteRasterRenderGraphPass);
                }
            }

            // For Compability Mode Only
#pragma warning disable 618, 672
            public override void Configure(CommandBuffer commandBuffer, RenderTextureDescriptor renderTextureDescriptor)
            {
                if (imageRenderer.UserTexture.OnSome && rtHandleSystem.OnSome)
                {
                    ConfigureTarget(rtHandleSystem.Value.Alloc(imageRenderer.UserTexture.Value));
                }
                ConfigureClear(ClearFlag.Depth, Color.clear);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (rtHandleSystem.OnSome)
                {
                    rtHandleSystem.Value.SetReferenceSize(
                        renderingData.cameraData.cameraTargetDescriptor.width,
                        renderingData.cameraData.cameraTargetDescriptor.height
                    );
                }
                renderPassData.worldToCameraMatrix = renderingData.cameraData.camera.worldToCameraMatrix;
                renderPassData.projectionMatrix = renderingData.cameraData.camera.projectionMatrix;
                renderPassData.invertCulling = imageRenderer.InvertCulling;
                renderPassData.mesh = mesh;
                renderPassData.material = imageRenderer.Material;
                var cmd = CommandBufferPool.Get(kRenderGraphDisablePassName);
                ExecuteRenderPass(CommandBufferHelpers.GetRasterCommandBuffer(cmd), renderPassData);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
#pragma warning restore 618, 672

            class PassData
            {
                internal Matrix4x4 worldToCameraMatrix;
                internal Matrix4x4 projectionMatrix;
                internal bool invertCulling;
                internal Mesh mesh;
                internal Material material;
            }
        }
#else
        class CameraImageRenderPass : ScriptableRenderPass
        {
            static readonly Matrix4x4 projection = Matrix4x4.Ortho(0f, 1f, 0f, 1f, -0.1f, 9.9f);
            readonly Mesh mesh;
            Material material;
            bool invertCulling;
#if EASYAR_URP_13_1_OR_NEWER
            Optional<RTHandle> colorTarget;
            Optional<RTHandleSystem> rtHandleSystem;
#else
            Optional<RenderTargetIdentifier> colorTarget;
#endif

            public CameraImageRenderPass(bool _ = false)
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
                mesh = new Mesh
                {
                    vertices = new Vector3[]
                    {
                        new Vector3(0f, 0f, 0.1f),
                        new Vector3(0f, 1f, 0.1f),
                        new Vector3(1f, 1f, 0.1f),
                        new Vector3(1f, 0f, 0.1f),
                    },
                    uv = new Vector2[]
                    {
                        new Vector2(0f, 0f),
                        new Vector2(0f, 1f),
                        new Vector2(1f, 1f),
                        new Vector2(1f, 0f),
                    },
                    triangles = new int[] { 0, 1, 2, 0, 2, 3 }
                };
            }

            public void Setup(Material mat, bool iCulling)
            {
                material = mat;
                invertCulling = iCulling;
            }

#if EASYAR_URP_13_1_OR_NEWER
            public void SetupRTHandleSystem(RTHandleSystem system)=> rtHandleSystem = system;
            public void SetupTarget(RenderTexture color) => colorTarget = rtHandleSystem.Value.Alloc(color);
#else
            public void SetupTarget(RenderTexture color) => colorTarget = (RenderTargetIdentifier)color;
#endif

            public override void Configure(CommandBuffer commandBuffer, RenderTextureDescriptor renderTextureDescriptor)
            {
                if (colorTarget.OnSome)
                {
                    ConfigureTarget(colorTarget.Value);
                }
                ConfigureClear(ClearFlag.Depth, Color.clear);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
#if EASYAR_URP_13_1_OR_NEWER
                if (rtHandleSystem.OnSome)
                {
                    rtHandleSystem.Value.SetReferenceSize(
                        renderingData.cameraData.cameraTargetDescriptor.width,
                        renderingData.cameraData.cameraTargetDescriptor.height
                    );
                }
#endif
                var cmd = CommandBufferPool.Get();
                cmd.SetInvertCulling(invertCulling);
                cmd.SetViewProjectionMatrices(Matrix4x4.identity, projection);
                cmd.DrawMesh(mesh, Matrix4x4.identity, material);
                cmd.SetViewProjectionMatrices(renderingData.cameraData.camera.worldToCameraMatrix, renderingData.cameraData.camera.projectionMatrix);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }
#endif

#endif
    }
}
