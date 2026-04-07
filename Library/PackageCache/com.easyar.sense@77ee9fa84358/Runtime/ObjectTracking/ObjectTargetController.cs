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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="ObjectTarget"/> in the scene, providing a few extensions in the Unity environment. Use <see cref="Target"/> to access target data after load success.</para>
    /// <para xml:lang="en">Target data will be loaded separately. It will start after session start. The full data loading will happen only once in the life time, if the session stops before load finish, it will be restarted in the next session start.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="ObjectTarget"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。加载成功后可以使用<see cref="Target"/>访问target数据。</para>
    /// <para xml:lang="zh">target的数据会单独加载，加载会在session成功启动后发生，生命周期中只会完整加载一次，如加载到一半session停止，将在下次session启动后再次加载。</para>
    /// </summary>
    public class ObjectTargetController : TargetController
    {
        internal bool HorizontalFlip;

        // for serialization backward compatibility, do not change field name including letter case
        [HideInInspector, SerializeField]
        private DataSource SourceType = DataSource.ObjFile;
        [HideInInspector, SerializeField]
        private ObjFileSourceData ObjFileSource = new();
        private TargetSourceData TargetSource;

        [HideInInspector, SerializeField]
        private bool trackerHasSet;
        [HideInInspector, SerializeField]
        private ObjectTrackerFrameFilter tracker;
        private ObjectTrackerFrameFilter holder;
        private float scale = 1;
        private float scaleX = 1;
        private bool preHFlip;

        /// <summary>
        /// <para xml:lang="en">Target data finish load (not load into a tracker).</para>
        /// <para xml:lang="zh">Target数据加载完成（不是加载到tracker中）。</para>
        /// </summary>
        public event Action<bool> TargetDataLoad;

        internal enum DataSource
        {
            ObjFile,
            Target,
        }

        /// <summary>
        /// <para xml:lang="en">Target data. Only usable after successful <see cref="TargetDataLoad"/>.</para>
        /// <para xml:lang="zh">Target数据，仅在<see cref="TargetDataLoad"/>成功之后可用。</para>
        /// </summary>
        public ObjectTarget Target { get; private set; }

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
                DataSource.ObjFile => ObjFileSource,
                DataSource.Target => TargetSource,
                _ => throw new ArgumentOutOfRangeException(),
            };
            set
            {
                if (value is ObjFileSourceData objFileSource)
                {
                    SourceType = DataSource.ObjFile;
                    ObjFileSource = objFileSource;
                }
                else if (value is TargetSourceData targetSource)
                {
                    SourceType = DataSource.Target;
                    TargetSource = targetSource;
                }
                else
                {
                    throw new InvalidOperationException("Invalid source data type: " + value?.GetType());
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">The <see cref="ObjectTrackerFrameFilter"/> which loads the target. When set to null, the target will be unloaded from tracker previously set. Modify at any time, the load will happen only when the session is running.</para>
        /// <para xml:lang="zh">加载target的<see cref="ObjectTrackerFrameFilter"/>。如果设为null，target将会被从之前设置的tracker中卸载。可随时修改，加载只会在session运行时发生。</para>
        /// </summary>
        public ObjectTrackerFrameFilter Tracker
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
        /// <para xml:lang="en">Bounding box of the target. Only usable after successful <see cref="TargetDataLoad"/>.</para>
        /// <para xml:lang="zh">Target的包围盒。仅在<see cref="TargetDataLoad"/>成功之后可用。</para>
        /// </summary>
        public List<Vector3> BoundingBox => Target.boundingBox().Select(box => box.ToUnityVector()).ToList();

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

        internal void OnTargetDataLoad(Optional<ObjectTarget> target)
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
        /// <para xml:lang="en">Obj file data for target creation.</para>
        /// <para xml:lang="zh">创建target的obj文件数据。</para>
        /// </summary>
        [Serializable]
        public class ObjFileSourceData : SourceData
        {
            /// <summary>
            /// <para xml:lang="en">File path type.</para>
            /// <para xml:lang="zh">文件路径类型。</para>
            /// </summary>
            public PathType PathType = PathType.StreamingAssets;
            /// <summary>
            /// <para xml:lang="en">Obj file path.</para>
            /// <para xml:lang="zh">Obj文件路径。</para>
            /// </summary>
            public string ObjPath = string.Empty;
            /// <summary>
            /// <para xml:lang="en">extra file paths referenced in obj file and other files, like .mtl, jpg, .png. These files are usually multiple textures and mtl files.</para>
            /// <para xml:lang="zh">Obj文件及其它文件中引用的额外文件路径，如：.mtl, .jpg, .png等。这些文件一般由多个贴图文件，和mtl组成。</para>
            /// </summary>
            public List<string> ExtraFilePaths = new();
            /// <summary>
            /// <para xml:lang="en">Target name.</para>
            /// <para xml:lang="zh">Target名字。</para>
            /// </summary>
            public string Name = string.Empty;
            /// <summary>
            /// <para xml:lang="en">Target scale in meter. Reference <see cref="ObjectTarget.scale"/>.</para>
            /// <para xml:lang="zh">Target的缩放比例，单位为米。参考<see cref="ObjectTarget.scale"/>。</para>
            /// </summary>
            public float Scale = 1;

            internal override SourceData TrimClone() => new ObjFileSourceData
            {
                PathType = PathType,
                ObjPath = ObjPath?.Trim(),
                ExtraFilePaths = ExtraFilePaths?.Select(p => p.Trim()).ToList() ?? new(),
                Name = Name,
                Scale = Scale,
            };
        }

        /// <summary>
        /// <para xml:lang="en"><see cref="ObjectTarget"/> data for target creation.</para>
        /// <para xml:lang="zh">创建target的<see cref="ObjectTarget"/>数据。</para>
        /// </summary>
        public class TargetSourceData : SourceData
        {
            /// <summary>
            /// <para xml:lang="en"><see cref="ObjectTarget"/> object.</para>
            /// <para xml:lang="zh"><see cref="ObjectTarget"/>对象。</para>
            /// </summary>
            public ObjectTarget Target;

            internal override SourceData TrimClone() => new TargetSourceData
            {
                Target = Target,
            };
        }
    }


    internal static class ObjectTargetDataLoader
    {
        public static IEnumerator Load(ObjectTargetController.SourceData data, Action<ObjectTarget> onSuccess, Action<string> onFail)
        {
            if (data is ObjectTargetController.ObjFileSourceData objFileSource)
            {
                yield return LoadObjFile(objFileSource, onSuccess, onFail);
            }
            else if (data is ObjectTargetController.TargetSourceData targetSource)
            {
                ObjectTarget target = null;
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
            else
            {
                throw new InvalidOperationException("Invalid source data type: " + data?.GetType());
            }
        }

        private static IEnumerator LoadObjFile(ObjectTargetController.ObjFileSourceData source, Action<ObjectTarget> onSuccess, Action<string> onFail)
        {
            using (var objBufferDic = new BufferDictionary())
            {
                Buffer buffer = null;
                string error = string.Empty;
                yield return FileUtil.LoadFile(source.ObjPath, source.PathType, (Buffer b) => buffer = b.Clone(), (e) => error = e);
                if (buffer == null)
                {
                    onFail.Invoke(error);
                    yield break;
                }
                objBufferDic.set(FileUtil.PathToUrl(source.ObjPath), buffer);

                foreach (var filePath in source.ExtraFilePaths)
                {
                    Buffer bufferExtra = null;
                    string errorExtra = string.Empty;
                    yield return FileUtil.LoadFile(filePath, source.PathType, (Buffer b) => bufferExtra = b.Clone(), (e) => error = e);
                    if (bufferExtra == null)
                    {
                        onFail.Invoke(errorExtra);
                        yield break;
                    }
                    objBufferDic.set(FileUtil.PathToUrl(filePath), bufferExtra);
                }

                using (var param = new ObjectTargetParameters())
                {
                    param.setBufferDictionary(objBufferDic);
                    param.setObjPath(FileUtil.PathToUrl(source.ObjPath));
                    param.setName(source.Name);
                    param.setScale(source.Scale);
                    param.setUid(Guid.NewGuid().ToString());
                    param.setMeta(string.Empty);

                    var targetOptional = ObjectTarget.createFromParameters(param);
                    if (targetOptional.OnNone)
                    {
                        onFail?.Invoke("invalid parameter");
                        yield break;
                    }
                    onSuccess?.Invoke(targetOptional.Value);
                }
            }
        }
    }
}
