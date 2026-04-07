//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="ImageTarget"/> in the scene, providing a few extensions in the Unity environment. Use <see cref="Target"/> to access target data after load success.</para>
    /// <para xml:lang="en">Target data will be loaded separately. It will start after session start. The full data loading will happen only once in the life time, if the session stops before load finish, it will be restarted in the next session start.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="ImageTarget"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。加载成功后可以使用<see cref="Target"/>访问target数据。</para>
    /// <para xml:lang="zh">target的数据会单独加载，加载会在session成功启动后发生，生命周期中只会完整加载一次，如加载到一半session停止，将在下次session启动后再次加载。</para>
    /// </summary>
    public class ImageTargetController : TargetController
    {
        internal bool HorizontalFlip;
        internal GizmoStorage GizmoData = new();

        // for serialization backward compatibility, do not change field name including letter case
        [HideInInspector, SerializeField]
        private DataSource SourceType = DataSource.ImageFile;
        [HideInInspector, SerializeField]
        private ImageFileSourceData ImageFileSource = new();
        [HideInInspector, SerializeField]
        private TargetDataFileSourceData TargetDataFileSource = new();
        [HideInInspector, SerializeField]
        private Texture2DSourceData Texture2DSource = new();
        private TargetSourceData TargetSource = new();

        [HideInInspector, SerializeField]
        private bool trackerHasSet;
        [HideInInspector, SerializeField]
        private ImageTrackerFrameFilter tracker;
        private ImageTrackerFrameFilter holder;
        private float scale = 0.1f;
        private float scaleX = 0.1f;
        private bool preHFlip;

        /// <summary>
        /// <para xml:lang="en">Target data finish load (not load into a tracker).</para>
        /// <para xml:lang="zh">Target数据加载完成（不是加载到tracker中）。</para>
        /// </summary>
        public event Action<bool> TargetDataLoad;

        internal enum DataSource
        {
            ImageFile,
            TargetDataFile,
            Target,
            Texture2D,
        }

        /// <summary>
        /// <para xml:lang="en">Target data. Only usable after successful <see cref="TargetDataLoad"/>.</para>
        /// <para xml:lang="zh">Target数据，仅在<see cref="TargetDataLoad"/>成功之后可用。</para>
        /// </summary>
        public ImageTarget Target { get; private set; }

        /// <summary>
        /// <para xml:lang="en">Data source for target creation. Only effective if modified before <see cref="MonoBehaviour"/>.Start.</para>
        /// <para xml:lang="en">If there are any referenced resources (like textures or Targets), they can be destroyed in <see cref="TargetDataLoad"/>, destroy earlier may cause load fail.</para>
        /// <para xml:lang="zh">创建target的数据来源。在<see cref="MonoBehaviour"/>.Start前修改才有效。</para>
        /// <para xml:lang="zh">如果存在引用的资源（Texture或Target等），可以在<see cref="TargetDataLoad"/>中销毁，提前销毁将导致加载失败。</para>
        /// </summary>
        public SourceData Source
        {
            get => SourceType switch
            {
                DataSource.ImageFile => ImageFileSource,
                DataSource.TargetDataFile => TargetDataFileSource,
                DataSource.Target => TargetSource,
                DataSource.Texture2D => Texture2DSource,
                _ => throw new ArgumentOutOfRangeException(),
            };
            set
            {
                if (value is ImageFileSourceData imageFileSource)
                {
                    SourceType = DataSource.ImageFile;
                    ImageFileSource = imageFileSource;
                }
                else if (value is TargetDataFileSourceData targetDataFileSource)
                {
                    SourceType = DataSource.TargetDataFile;
                    TargetDataFileSource = targetDataFileSource;
                }
                else if (value is TargetSourceData targetSource)
                {
                    SourceType = DataSource.Target;
                    TargetSource = targetSource;
                }
                else if (value is Texture2DSourceData texture2DSource)
                {
                    SourceType = DataSource.Texture2D;
                    Texture2DSource = texture2DSource;
                }
                else
                {
                    throw new InvalidOperationException("Invalid source data type: " + value?.GetType());
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">The <see cref="ImageTrackerFrameFilter"/> which loads the target. When set to null, the target will be unloaded from tracker previously set. Modify at any time, the load will happen only when the session is running.</para>
        /// <para xml:lang="zh">加载target的<see cref="ImageTrackerFrameFilter"/>。如果设为null，target将会被从之前设置的tracker中卸载。可随时修改，加载只会在session运行时发生。</para>
        /// </summary>
        public ImageTrackerFrameFilter Tracker
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
        /// <para xml:lang="en">Physical size of <see cref="Target"/> in meter. Only usable after successful <see cref="TargetDataLoad"/>.</para>
        /// <para xml:lang="zh"><see cref="Target"/>的物理大小，单位为米。仅在<see cref="TargetDataLoad"/>成功之后可用。</para>
        /// </summary>
        public Vector2 Size => new (Target.scale(), Target.scale() / Target.aspectRatio());

        internal bool IsLoaded { get; private set; } // NOTE: EasyAR Sense multi-thread callback is dispatched to game thread by DelayedCallbackScheduler, this flag is used to keep load-track order.
        internal Optional<SourceData> LoadingSource { get; private set; }
        internal Optional<bool> LoadingResult { get; private set; }

        private protected override void Start()
        {
            base.Start();
            LoadingSource = Source.TrimClone();
            UpdateHolder();
        }

        private void Update()
        {
            CheckScale();
        }

        private protected override void OnDestroy()
        {
            base.OnDestroy();
            if (tracker)
            {
                tracker = null;
                UpdateHolder();
            }
            Target?.Dispose();
            Target = null;
        }

        internal void OnTargetDataLoad(Optional<ImageTarget> target)
        {
            LoadingResult = target.OnSome;
            if (target.OnSome)
            {
                Target = target.Value;
                UpdateScale();
            }
            TargetDataLoad?.Invoke(target.OnSome);
        }

        internal void OnLoad()
        {
            IsLoaded = true;
            CheckScale();
        }

        internal void OnUnload()
        {
            OnTracking(false);
            IsLoaded = false;
        }

        internal override bool OnTracking(bool status)
        {
            if (status) { CheckScale(); }
            return base.OnTracking(status);
        }

        private void UpdateHolder()
        {
            if (holder == tracker) { return; }
            if (holder) { holder.Unhold(this); }
            holder = tracker;
            if (holder) { holder.Hold(this); }
        }

        private void UpdateScale()
        {
            if (Target == null) { return; }
            scale = Target.scale();
            var vec3Unit = Vector3.one;
            if (HorizontalFlip)
            {
                vec3Unit.x = -vec3Unit.x;
            }
            transform.localScale = vec3Unit * scale;
            scaleX = transform.localScale.x;
            preHFlip = HorizontalFlip;
        }

        private void CheckScale()
        {
            if (Target == null) { return; }
            if (scaleX != transform.localScale.x)
            {
                Target.setScale(Math.Abs(transform.localScale.x));
                UpdateScale();
            }
            else if (scale != transform.localScale.y)
            {
                Target.setScale(Math.Abs(transform.localScale.y));
                UpdateScale();
            }
            else if (scale != transform.localScale.z)
            {
                Target.setScale(Math.Abs(transform.localScale.z));
                UpdateScale();
            }
            else if (scale != Target.scale())
            {
                UpdateScale();
            }
            else if (preHFlip != HorizontalFlip)
            {
                UpdateScale();
            }
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
        /// <para xml:lang="en">Image data for target creation.</para>
        /// <para xml:lang="zh">创建target的图像数据。</para>
        /// </summary>
        [Serializable]
        public class ImageFileSourceData : SourceData
        {
            /// <summary>
            /// <para xml:lang="en">File path type.</para>
            /// <para xml:lang="zh">文件路径类型。</para>
            /// </summary>
            public PathType PathType = PathType.StreamingAssets;
            /// <summary>
            /// <para xml:lang="en">File path.</para>
            /// <para xml:lang="zh">文件路径。</para>
            /// </summary>
            public string Path = string.Empty;
            /// <summary>
            /// <para xml:lang="en">Target name.</para>
            /// <para xml:lang="zh">Target名字。</para>
            /// </summary>
            public string Name = string.Empty;
            /// <summary>
            /// <para xml:lang="en">Target scale in meter. Reference <see cref="ImageTarget.scale"/>.</para>
            /// <para xml:lang="zh">Target图像的缩放比例，单位为米。参考<see cref="ImageTarget.scale"/>。</para>
            /// </summary>
            public float Scale = 0.1f;

            internal override SourceData TrimClone() => new ImageFileSourceData
            {
                PathType = PathType,
                Path = Path?.Trim(),
                Name = Name,
                Scale = Scale,
            };
        }

        /// <summary>
        /// <para xml:lang="en">Target data for target creation. Target scale and name are defined in the etd file.</para>
        /// <para xml:lang="zh">创建target的target data。Target名字和缩放在etd文件中定义。</para>
        /// </summary>
        [Serializable]
        public class TargetDataFileSourceData : SourceData
        {
            /// <summary>
            /// <para xml:lang="en">File path type.</para>
            /// <para xml:lang="zh">文件路径类型。</para>
            /// </summary>
            public PathType PathType = PathType.StreamingAssets;
            /// <summary>
            /// <para xml:lang="en">File path.</para>
            /// <para xml:lang="zh">文件路径。</para>
            /// </summary>
            public string Path = string.Empty;

            internal override SourceData TrimClone() => new TargetDataFileSourceData
            {
                PathType = PathType,
                Path = Path?.Trim(),
            };
        }

        /// <summary>
        /// <para xml:lang="en"><see cref="Texture2D"/> data for target creation.</para>
        /// <para xml:lang="zh">创建target的<see cref="Texture2D"/>数据。</para>
        /// </summary>
        [Serializable]
        public class Texture2DSourceData : SourceData
        {
            /// <summary>
            /// <para xml:lang="en">Readable <see cref="Texture2D"/> object.</para>
            /// <para xml:lang="zh">可读的<see cref="Texture2D"/>对象。</para>
            /// </summary>
            public Texture2D Texture = null;
            /// <summary>
            /// <para xml:lang="en">Target name.</para>
            /// <para xml:lang="zh">Target名字。</para>
            /// </summary>
            public string Name = string.Empty;
            /// <summary>
            /// <para xml:lang="en">Target scale in meter. Reference <see cref="ImageTarget.scale"/>.</para>
            /// <para xml:lang="zh">Target图像的缩放比例，单位为米。参考<see cref="ImageTarget.scale"/>。</para>
            /// </summary>
            public float Scale = 0.1f;

            /// <summary>
            /// <para xml:lang="en">Whether <see cref="Texture"/> is loadable.</para>
            /// <para xml:lang="zh"><see cref="Texture"/>是否可以被加载。</para>
            /// </summary>
            public bool IsTextureLoadable { get => IsTexture2DSourceLoadable().Item1; }
            /// <summary>
            /// <para xml:lang="en">The reason when <see cref="Texture"/> is not loadable.</para>
            /// <para xml:lang="zh"><see cref="Texture"/>不能被加载的原因。</para>
            /// </summary>
            public string TextureUnloadableReason { get => IsTexture2DSourceLoadable().Item2; }

            /// <summary>
            /// <para xml:lang="en"><see cref="PixelFormat"/> of the texture.</para>
            /// <para xml:lang="zh">Texture的<see cref="PixelFormat"/>。</para>
            /// </summary>
            public Optional<PixelFormat> TexturePixelFormat => Texture ? Texture.format switch
            {
                TextureFormat.R8 => (Optional<PixelFormat>)PixelFormat.Gray,
                TextureFormat.RGB24 => (Optional<PixelFormat>)PixelFormat.RGB888,
                TextureFormat.RGBA32 => (Optional<PixelFormat>)PixelFormat.RGBA8888,
                TextureFormat.BGRA32 => (Optional<PixelFormat>)PixelFormat.BGRA8888,
                _ => Optional<PixelFormat>.Empty,
            } : Optional<PixelFormat>.Empty;

            internal override SourceData TrimClone() => new Texture2DSourceData
            {
                Texture = Texture,
                Name = Name,
                Scale = Scale,
            };

            private Tuple<bool, string> IsTexture2DSourceLoadable()
            {
                if (!Texture)
                {
                    return Tuple.Create(false, "Texture is null");
                }
                if (!Texture.isReadable)
                {
                    return Tuple.Create(false, "Texture must be readable to be used as the source for target creation.");
                }

                var pixelFormat = TexturePixelFormat;
                if (pixelFormat.OnNone)
                {
                    return Tuple.Create(false, $"Texture format {Texture.format} is not supported for target creation.");
                }

                var bytePerPixel = (pixelFormat == PixelFormat.RGBA8888 || pixelFormat == PixelFormat.BGRA8888) ? 4 : ((pixelFormat == PixelFormat.RGB888 || pixelFormat == PixelFormat.BGR888) ? 3 : 1);
                var size = new Vector2Int(Texture.width, Texture.height);
                var data = Texture.GetRawTextureData<byte>();
                if (size.x <= 0 || size.y <= 0 || data.Length < bytePerPixel * size.x * size.y)
                {
                    return Tuple.Create(false, $"Texture size is invalid ({Texture.format}, {size.x}, {size.y}), {data.Length}.");
                }

                return Tuple.Create(true, string.Empty);
            }
        }

        /// <summary>
        /// <para xml:lang="en"><see cref="ImageTarget"/> data for target creation.</para>
        /// <para xml:lang="zh">创建target的<see cref="ImageTarget"/>数据。</para>
        /// </summary>
        public class TargetSourceData : SourceData
        {
            /// <summary>
            /// <para xml:lang="en"><see cref="ImageTarget"/> object.</para>
            /// <para xml:lang="zh"><see cref="ImageTarget"/>对象。</para>
            /// </summary>
            public ImageTarget Target;

            internal override SourceData TrimClone() => new TargetSourceData
            {
                Target = Target,
            };
        }

        internal class GizmoStorage
        {
            public string Signature;
            public Texture2D Texture;
            public Material Material;
            public float Scale = 0.1f;
            public float ScaleX = 0.1f;
            public bool HorizontalFlip;
        }
    }

    internal static class ImageTargetDataLoader
    {
        public static IEnumerator Load(ImageTargetController.SourceData data, ThreadWorker worker, Action<ImageTarget> onSuccess, Action<string> onFail)
        {
            if (data is ImageTargetController.ImageFileSourceData imageFileSource)
            {
                yield return LoadImageFile(imageFileSource, worker, onSuccess, onFail);
            }
            else if (data is ImageTargetController.TargetDataFileSourceData targetDataFileSource)
            {
                yield return LoadTargetDataFile(targetDataFileSource, worker, onSuccess, onFail);
            }
            else if (data is ImageTargetController.TargetSourceData targetSource)
            {
                ImageTarget target = null;
                try
                {
                    target = targetSource.Target?.Clone();
                }
                catch (Exception) { }
                if (target == null)
                {
                    onFail("Target is null or disposed");
                }
                else
                {
                    onSuccess?.Invoke(target);
                }
            }
            else if (data is ImageTargetController.Texture2DSourceData texture2DSource)
            {
                yield return LoadTexture2D(texture2DSource, worker, onSuccess, onFail);
            }
            else
            {
                throw new InvalidOperationException("Invalid source data type: " + data?.GetType());
            }
        }

        private static IEnumerator LoadImageFile(ImageTargetController.ImageFileSourceData source, ThreadWorker worker, Action<ImageTarget> onSuccess, Action<string> onFail)
        {
            Buffer buffer = null;
            string error = string.Empty;
            yield return FileUtil.LoadFile(source.Path, source.PathType, (Buffer b) => buffer = b.Clone(), (e) => error = e);
            if (buffer == null)
            {
                onFail.Invoke(error);
                yield break;
            }

            using (buffer)
            {
                Optional<Image> imageOptional = null;
                bool taskFinished = false;
                worker.Run(() =>
                {
                    imageOptional = ImageHelper.decode(buffer);
                    taskFinished = true;
                });

                while (!taskFinished)
                {
                    yield return 0;
                }
                if (imageOptional.OnNone)
                {
                    onFail?.Invoke("invalid buffer");
                    yield break;
                }

                using (var image = imageOptional.Value)
                using (var param = new ImageTargetParameters())
                {
                    param.setImage(image);
                    param.setName(source.Name);
                    param.setScale(source.Scale);
                    param.setUid(Guid.NewGuid().ToString());
                    param.setMeta(string.Empty);

                    var targetOptional = ImageTarget.createFromParameters(param);
                    if (targetOptional.OnNone)
                    {
                        onFail?.Invoke("invalid parameter");
                        yield break;
                    }
                    onSuccess?.Invoke(targetOptional.Value);
                }
            }
        }

        private static IEnumerator LoadTargetDataFile(ImageTargetController.TargetDataFileSourceData source, ThreadWorker worker, Action<ImageTarget> onSuccess, Action<string> onFail)
        {
            Buffer buffer = null;
            string error = string.Empty;
            yield return FileUtil.LoadFile(source.Path, source.PathType, (Buffer b) => buffer = b.Clone(), (e) => error = e);
            if (buffer == null)
            {
                onFail.Invoke(error);
                yield break;
            }

            using (buffer)
            {
                Optional<ImageTarget> targetOptional = null;
                bool taskFinished = false;
                worker.Run(() =>
                {
                    targetOptional = ImageTarget.createFromTargetData(buffer);
                    taskFinished = true;
                });

                while (!taskFinished)
                {
                    yield return 0;
                }
                if (targetOptional.OnNone)
                {
                    onFail?.Invoke("invalid buffer");
                    yield break;
                }
                onSuccess?.Invoke(targetOptional.Value);
            }
        }

        private static IEnumerator LoadTexture2D(ImageTargetController.Texture2DSourceData source, ThreadWorker worker, Action<ImageTarget> onSuccess, Action<string> onFail)
        {
            if (!source.IsTextureLoadable)
            {
                onFail?.Invoke(source.TextureUnloadableReason);
                yield break;
            }

            Optional<ImageTarget> targetOptional = null;
            bool taskFinished = false;

            var data = source.Texture.GetRawTextureData<byte>();
            var size = new Vector2Int(source.Texture.width, source.Texture.height);
            var pixelFormat = source.TexturePixelFormat.Value;

            worker.Run(() =>
            {
                targetOptional = LoadTexture2DData(data, size, pixelFormat, source.Name, source.Scale);
                taskFinished = true;
            });

            yield return new WaitUntil(() => taskFinished);
            if (targetOptional.OnNone)
            {
                onFail?.Invoke("invalid parameter");
                yield break;
            }
            onSuccess?.Invoke(targetOptional.Value);
        }

        private static unsafe Optional<ImageTarget> LoadTexture2DData(Unity.Collections.NativeArray<byte> data, Vector2Int size, PixelFormat pixelFormat, string name, float scale)
        {
            var ptr = data.GetUnsafeReadOnlyPtr();
            int oneLineLength = size.x * ((pixelFormat == PixelFormat.RGBA8888 || pixelFormat == PixelFormat.BGRA8888) ? 4 : ((pixelFormat == PixelFormat.RGB888 || pixelFormat == PixelFormat.BGR888) ? 3 : 1));
            int totalLength = oneLineLength * size.y;
            using (var buffer = Buffer.create(totalLength))
            {
                for (int i = 0; i < size.y; i++)
                {
                    buffer.tryCopyFrom(new IntPtr(ptr), oneLineLength * i, totalLength - oneLineLength * (i + 1), oneLineLength);
                }

                using (var image = Image.create(buffer, pixelFormat, size.x, size.y, size.x, size.y))
                using (var param = new ImageTargetParameters())
                {
                    param.setImage(image);
                    param.setName(name);
                    param.setScale(scale);
                    param.setUid(Guid.NewGuid().ToString());
                    param.setMeta(string.Empty);
                    return ImageTarget.createFromParameters(param);
                }
            }
        }
    }
}
