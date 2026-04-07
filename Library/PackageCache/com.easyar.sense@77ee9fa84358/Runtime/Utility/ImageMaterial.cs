//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Material to render <see cref="Image"/>.</para>
    /// <para xml:lang="zh">用于渲染<see cref="Image"/>的材质。</para>
    /// </summary>
    public class ImageMaterial : IDisposable
    {
        private Shaders shaders;
        private PixelFormat format;
        private int imageWidth;
        private int imageHeight;
        private int pixelWidth;
        private int pixelHeight;
        private Material material;
        private Texture2D[] textures = new Texture2D[0];

        /// <summary>
        /// <para xml:lang="en">Create material to render <paramref name="image"/>.</para>
        /// <para xml:lang="zh">创建适用于渲染<paramref name="image"/>的材质。</para>
        /// </summary>
        public ImageMaterial(Image image)
        {
            shaders = new Shaders();
            format = image.format();
            imageWidth = image.width();
            imageHeight = image.height();
            pixelWidth = image.pixelWidth();
            pixelHeight = image.pixelHeight();

            switch (format)
            {
                case PixelFormat.Gray:
                    textures = new Texture2D[1];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.Alpha8, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.GRAY);
                    material.SetTexture("_grayTexture", textures[0]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMax", (imageWidth - 0.5f) / pixelWidth);
                    break;
                case PixelFormat.YUV_NV21:
                    textures = new Texture2D[2];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.Alpha8, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;
                    textures[1] = new Texture2D(pixelWidth / 2, imageHeight / 2, TextureFormat.RGBA4444, false);
                    textures[1].wrapMode = TextureWrapMode.Clamp;
                    textures[1].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.YUV_NV21);
                    material.SetTexture("_yTexture", textures[0]);
                    material.SetTexture("_uvTexture", textures[1]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMaxY", (imageWidth - 0.5f) / pixelWidth);
                    material.SetFloat("_RowMaxUV", (imageWidth / 2 - 0.5f) / (pixelWidth / 2));
                    break;
                case PixelFormat.YUV_NV12:
                    textures = new Texture2D[2];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.Alpha8, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;
                    textures[1] = new Texture2D(pixelWidth / 2, imageHeight / 2, TextureFormat.RGBA4444, false);
                    textures[1].wrapMode = TextureWrapMode.Clamp;
                    textures[1].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.YUV_NV12);
                    material.SetTexture("_yTexture", textures[0]);
                    material.SetTexture("_uvTexture", textures[1]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMaxY", (imageWidth - 0.5f) / pixelWidth);
                    material.SetFloat("_RowMaxUV", (imageWidth / 2 - 0.5f) / (pixelWidth / 2));
                    break;
                case PixelFormat.YUV_I420:
                    textures = new Texture2D[3];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.Alpha8, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;
                    textures[1] = new Texture2D(pixelWidth / 2, imageHeight / 2, TextureFormat.Alpha8, false);
                    textures[1].wrapMode = TextureWrapMode.Clamp;
                    textures[1].filterMode = FilterMode.Bilinear;
                    textures[2] = new Texture2D(pixelWidth / 2, imageHeight / 2, TextureFormat.Alpha8, false);
                    textures[2].wrapMode = TextureWrapMode.Clamp;
                    textures[2].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.YUV_I420_YV12);
                    material.SetTexture("_yTexture", textures[0]);
                    material.SetTexture("_uTexture", textures[1]);
                    material.SetTexture("_vTexture", textures[2]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMaxY", (imageWidth - 0.5f) / pixelWidth);
                    material.SetFloat("_RowMaxUV", (imageWidth / 2 - 0.5f) / (pixelWidth / 2));
                    break;
                case PixelFormat.YUV_YV12:
                    textures = new Texture2D[3];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.Alpha8, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;
                    textures[1] = new Texture2D(pixelWidth / 2, imageHeight / 2, TextureFormat.Alpha8, false);
                    textures[1].wrapMode = TextureWrapMode.Clamp;
                    textures[1].filterMode = FilterMode.Bilinear;
                    textures[2] = new Texture2D(pixelWidth / 2, imageHeight / 2, TextureFormat.Alpha8, false);
                    textures[2].wrapMode = TextureWrapMode.Clamp;
                    textures[2].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.YUV_I420_YV12);
                    material.SetTexture("_yTexture", textures[0]);
                    material.SetTexture("_uTexture", textures[1]);
                    material.SetTexture("_vTexture", textures[2]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMaxY", (imageWidth - 0.5f) / pixelWidth);
                    material.SetFloat("_RowMaxUV", (imageWidth / 2 - 0.5f) / (pixelWidth / 2));
                    break;
                case PixelFormat.RGB888:
                    textures = new Texture2D[1];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.RGB24, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.RGB);
                    material.SetTexture("_MainTex", textures[0]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMax", (imageWidth - 0.5f) / pixelWidth);
                    break;
                case PixelFormat.BGR888:
                    textures = new Texture2D[1];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.RGB24, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.BGR);
                    material.SetTexture("_MainTex", textures[0]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMax", (imageWidth - 0.5f) / pixelWidth);
                    break;
                case PixelFormat.RGBA8888:
                    textures = new Texture2D[1];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.RGBA32, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.RGB);
                    material.SetTexture("_MainTex", textures[0]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMax", (imageWidth - 0.5f) / pixelWidth);
                    break;
                case PixelFormat.BGRA8888:
                    textures = new Texture2D[1];
                    textures[0] = new Texture2D(pixelWidth, imageHeight, TextureFormat.RGBA32, false);
                    textures[0].wrapMode = TextureWrapMode.Clamp;
                    textures[0].filterMode = FilterMode.Bilinear;

                    material = new Material(shaders.BGR);
                    material.SetTexture("_MainTex", textures[0]);

                    material.SetFloat("_RowRatio", (float)imageWidth / pixelWidth);
                    material.SetFloat("_RowMax", (imageWidth - 0.5f) / pixelWidth);
                    break;
                default:
                    break;
            }

            UncheckedReplace(image);
        }

        ~ImageMaterial()
        {
            DisposeResources();
        }

        /// <summary>
        /// <para xml:lang="en">Material.</para>
        /// <para xml:lang="zh">材质。</para>
        /// </summary>
        public Material Material => material;

        /// <summary>
        /// <para xml:lang="en">Dispose resources.</para>
        /// <para xml:lang="zh">销毁资源。</para>
        /// </summary>
        public void Dispose()
        {
            DisposeResources();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para xml:lang="en">Whether image used in material can be replaced by <paramref name="image"/>.</para>
        /// <para xml:lang="zh">材质中使用的图像是否可替换成<paramref name="image"/>。</para>
        /// </summary>
        public bool CanReplace(Image image) => image.width() == imageWidth && image.height() == imageHeight && image.pixelWidth() == pixelWidth && image.pixelHeight() == pixelHeight && image.format() == format;

        /// <summary>
        /// <para xml:lang="en">Replace <see cref="Image"/> used in material. Only images with of the same format, size and pixel size are supported.</para>
        /// <para xml:lang="zh">替换材质中的<see cref="Image"/>。仅支持相同格式、大小及像素大小的图像。</para>
        /// </summary>
        public void Replace(Image image)
        {
            if (!CanReplace(image)) { throw new InvalidOperationException("image format or dimension not match"); }
            UncheckedReplace(image);
        }

        internal void UncheckedReplace(Image image)
        {
            using (var buffer = image.buffer())
            {
                var ptr = buffer.data();
                var pixelResolution = pixelWidth * pixelHeight;
                var dataResolution = pixelWidth * imageHeight;
                switch (format)
                {
                    case PixelFormat.Gray:
                        textures[0].LoadRawTextureData(ptr, dataResolution);
                        textures[0].Apply();
                        break;
                    case PixelFormat.YUV_NV21:
                        textures[0].LoadRawTextureData(ptr, dataResolution);
                        textures[0].Apply();
                        textures[1].LoadRawTextureData(new IntPtr(ptr.ToInt64() + pixelResolution), dataResolution);
                        textures[1].Apply();
                        break;
                    case PixelFormat.YUV_NV12:
                        textures[0].LoadRawTextureData(ptr, dataResolution);
                        textures[0].Apply();
                        textures[1].LoadRawTextureData(new IntPtr(ptr.ToInt64() + pixelResolution), dataResolution);
                        textures[1].Apply();
                        break;
                    case PixelFormat.YUV_I420:
                        textures[0].LoadRawTextureData(new IntPtr(ptr.ToInt64()), pixelResolution);
                        textures[0].Apply();
                        textures[1].LoadRawTextureData(new IntPtr(ptr.ToInt64() + pixelResolution), dataResolution / 4);
                        textures[1].Apply();
                        textures[2].LoadRawTextureData(new IntPtr(ptr.ToInt64() + pixelResolution + pixelResolution / 4), dataResolution / 4);
                        textures[2].Apply();
                        break;
                    case PixelFormat.YUV_YV12:
                        textures[0].LoadRawTextureData(new IntPtr(ptr.ToInt64()), pixelResolution);
                        textures[0].Apply();
                        textures[1].LoadRawTextureData(new IntPtr(ptr.ToInt64() + pixelResolution + pixelResolution / 4), dataResolution / 4);
                        textures[1].Apply();
                        textures[2].LoadRawTextureData(new IntPtr(ptr.ToInt64() + pixelResolution), dataResolution / 4);
                        textures[2].Apply();
                        break;
                    case PixelFormat.RGB888:
                        textures[0].LoadRawTextureData(new IntPtr(ptr.ToInt64()), buffer.size());
                        textures[0].Apply();
                        break;
                    case PixelFormat.BGR888:
                        textures[0].LoadRawTextureData(new IntPtr(ptr.ToInt64()), buffer.size());
                        textures[0].Apply();
                        break;
                    case PixelFormat.RGBA8888:
                        textures[0].LoadRawTextureData(new IntPtr(ptr.ToInt64()), buffer.size());
                        textures[0].Apply();
                        break;
                    case PixelFormat.BGRA8888:
                        textures[0].LoadRawTextureData(new IntPtr(ptr.ToInt64()), buffer.size());
                        textures[0].Apply();
                        break;
                    default:
                        break;
                }
            }
        }

        private void DisposeResources()
        {
            if (material)
            {
                UnityEngine.Object.Destroy(material);
            }
            foreach (var texture in textures)
            {
                UnityEngine.Object.Destroy(texture);
            }
        }

        class Shaders
        {
            public Shader RGB;
            public Shader BGR;
            public Shader GRAY;
            public Shader YUV_I420_YV12;
            public Shader YUV_NV12;
            public Shader YUV_NV21;

            public Shaders()
            {
                RGB = Shader.Find("EasyAR/CameraImage_RGB");
                BGR = Shader.Find("EasyAR/CameraImage_BGR");
                GRAY = Shader.Find("EasyAR/CameraImage_Gray");
                YUV_I420_YV12 = Shader.Find("EasyAR/CameraImage_YUV_I420_YV12");
                YUV_NV12 = Shader.Find("EasyAR/CameraImage_YUV_NV12");
                YUV_NV21 = Shader.Find("EasyAR/CameraImage_YUV_NV21");

                if (!RGB || !BGR || !GRAY || !YUV_I420_YV12 || !YUV_NV12 || !YUV_NV21)
                {
                    throw new InvalidOperationException($"Could not find EasyAR shader for video overlay.");
                }
            }
        }
    }
}
