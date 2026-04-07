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
using System.Text.RegularExpressions;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">ARSession Factory.</para>
    /// <para xml:lang="zh">ARSession工厂。</para>
    /// </summary>
    public class ARSessionFactory
    {
        /// <summary>
        /// <para xml:lang="en">ARSession preset.</para>
        /// <para xml:lang="zh">ARSession预设。</para>
        /// </summary>
        public enum ARSessionPreset
        {
            /// <summary>
            /// <para xml:lang="en">Image tracking.</para>
            /// <para xml:lang="zh">图像跟踪。</para>
            /// </summary>
            ImageTracking,
            /// <summary>
            /// <para xml:lang="en">Object tracking.</para>
            /// <para xml:lang="zh">物体跟踪。</para>
            /// </summary>
            ObjectTracking,
            /// <summary>
            /// <para xml:lang="en">Cloud recognition for image.</para>
            /// <para xml:lang="zh">图像云识别。</para>
            /// </summary>
            CloudRecognition,
            /// <summary>
            /// <para xml:lang="en">Image tracking with motion fusion.</para>
            /// <para xml:lang="zh">图像跟踪，启用运动融合。</para>
            /// </summary>
            ImageTrackingMotionFusion,
            /// <summary>
            /// <para xml:lang="en">Object tracking with motion fusion.</para>
            /// <para xml:lang="zh">物体跟踪，启用运动融合。</para>
            /// </summary>
            ObjectTrackingMotionFusion,
            /// <summary>
            /// <para xml:lang="en">Surface tracking.</para>
            /// <para xml:lang="zh">表面跟踪。</para>
            /// </summary>
            SurfaceTracking,
            /// <summary>
            /// <para xml:lang="en">Motion tracking.</para>
            /// <para xml:lang="zh">运动跟踪。</para>
            /// </summary>
            MotionTracking,
            /// <summary>
            /// <para xml:lang="en">Sparse spatial map building.</para>
            /// <para xml:lang="zh">稀疏空间地图建图。</para>
            /// </summary>
            SparseSpatialMapBuilder,
            /// <summary>
            /// <para xml:lang="en">Sparse spatial map tracking.</para>
            /// <para xml:lang="zh">稀疏空间地图跟踪。</para>
            /// </summary>
            SparseSpatialMapTracker,
            /// <summary>
            /// <para xml:lang="en">Dense spatial map building.</para>
            /// <para xml:lang="zh">稠密空间地图建图。</para>
            /// </summary>
            DenseSpatialMapBuilder,
            /// <summary>
            /// <para xml:lang="en">Mega Block based on motion tracking.</para>
            /// <para xml:lang="zh">基于运动跟踪的Mega Block。</para>
            /// </summary>
            MegaBlock_MotionTracking,
            /// <summary>
            /// <para xml:lang="en">Mega Block, degenerate to available tracking mode if motion tracking not available accroding to the order: MotionTracking > Inertial.</para>
            /// <para xml:lang="zh">Mega Block，如运动跟踪不可用则根据如下顺序选择可用的跟踪模式退化：MotionTracking > Inertial。</para>
            /// </summary>
            MegaBlock_MotionTracking_Inertial,
            /// <summary>
            /// <para xml:lang="en">Mega Block, degenerate to available tracking mode if motion tracking not available accroding to the order: MotionTracking > Inertial > 3DOF.</para>
            /// <para xml:lang="zh">Mega Block，如运动跟踪不可用则根据如下顺序选择可用的跟踪模式退化：MotionTracking > Inertial > 3DOF。</para>
            /// </summary>
            MegaBlock_MotionTracking_Inertial_3DOF,
            /// <summary>
            /// <para xml:lang="en">Mega Block, degenerate to available tracking mode if motion tracking not available accroding to the order: MotionTracking > Inertial > 3DOF > 0DOF.</para>
            /// <para xml:lang="zh">Mega Block，如运动跟踪不可用则根据如下顺序选择可用的跟踪模式退化：MotionTracking > Inertial > 3DOF > 0DOF。</para>
            /// </summary>
            MegaBlock_MotionTracking_Inertial_3DOF_0DOF,
            /// <summary>
            /// <para xml:lang="en">Mega Landmark based on motion tracking.</para>
            /// <para xml:lang="zh">基于运动跟踪的Mega Landmark。</para>
            /// </summary>
            MegaLandmark_MotionTracking,
            /// <summary>
            /// <para xml:lang="en">Mega Landmark, degenerate to available tracking mode if motion tracking not available accroding to the order: MotionTracking > Inertial.</para>
            /// <para xml:lang="zh">Mega Landmark，如运动跟踪不可用则根据如下顺序选择可用的跟踪模式退化：MotionTracking > Inertial。</para>
            /// </summary>
            MegaLandmark_MotionTracking_Inertial,
            /// <summary>
            /// <para xml:lang="en">Mega Landmark, degenerate to available tracking mode if motion tracking not available accroding to the order: MotionTracking > Inertial > 3DOF.</para>
            /// <para xml:lang="zh">Mega Landmark，如运动跟踪不可用则根据如下顺序选择可用的跟踪模式退化：MotionTracking > Inertial > 3DOF。</para>
            /// </summary>
            MegaLandmark_MotionTracking_Inertial_3DOF,
            /// <summary>
            /// <para xml:lang="en">Mega Landmark, degenerate to available tracking mode if motion tracking not available accroding to the order: MotionTracking > Inertial > 3DOF > 0DOF.</para>
            /// <para xml:lang="zh">Mega Landmark，如运动跟踪不可用则根据如下顺序选择可用的跟踪模式退化：MotionTracking > Inertial > 3DOF > 0DOF。</para>
            /// </summary>
            MegaLandmark_MotionTracking_Inertial_3DOF_0DOF,
        }

        /// <summary>
        /// <para xml:lang="en">Create empty AR Session.</para>
        /// <para xml:lang="zh">创建空的ARSession。</para>
        /// </summary>
        public static GameObject CreateSession()
        {
            var name = "AR Session (EasyAR)";
            var session = CreateObject<ARSession>(name);
            session.GetComponent<FrameRecorder>().enabled = false;
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(session, $"Create {name}");
#endif
            return session;
        }

        /// <summary>
        /// <para xml:lang="en">Create AR Session. Resources are required when dense spatial map is included.</para>
        /// <para xml:lang="zh">创建ARSession。在包含稠密空间地图时需传入对应资源。</para>
        /// </summary>
        public static GameObject CreateSession(ARSessionPreset preset, Resources resources = null) => CreateSession("AR Session (EasyAR)", () => CreateFrameSources(preset), () => CreateFrameFilters(preset, resources));

        /// <summary>
        /// <para xml:lang="en">Add frame filter to AR Session. Resources are required adding dense spatial map.</para>
        /// <para xml:lang="zh">添加frame filter到ARSession。添加稠密空间地图时需传入对应资源。</para>
        /// </summary>
        public static GameObject AddFrameFilter<Filter>(GameObject sessionObj, Resources resources = null) where Filter : FrameFilter
        {
            if (!IsSession(sessionObj))
            {
                throw new InvalidOperationException($"{sessionObj} is not {nameof(ARSession)}");
            }

            GameObject go;
            if (typeof(Filter) == typeof(DenseSpatialMapBuilderFrameFilter))
            {
                go = CreateDenseSpatialMapBuilder(resources);
            }
            else if (typeof(Filter) == typeof(SparseSpatialMapBuilderFrameFilter))
            {
                go = CreateSparseSpatialMapBuilder(resources);
            }
            else
            {
                go = new GameObject(DefaultName<Filter>(), typeof(Filter));
            }
            go.transform.SetParent(sessionObj.transform, false);
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
#endif
            return go;
        }

        /// <summary>
        /// <para xml:lang="en">Add frame source to AR Session.</para>
        /// <para xml:lang="zh">添加frame source到ARSession。</para>
        /// </summary>
        public static GameObject AddFrameSource<Source>(GameObject sessionObj, bool addToFirst = false) where Source : FrameSource
        {
            if (!IsSession(sessionObj))
            {
                throw new InvalidOperationException($"{sessionObj} is not {nameof(ARSession)}");
            }
            var go = new GameObject(DefaultName<Source>(), typeof(Source));
            var parent = sessionObj.transform;
            foreach (Transform t in sessionObj.transform)
            {
                if (t.name == "Frame Source Group")
                {
                    parent = t;
                    break;
                }
            }
            go.transform.SetParent(parent, false);
            if (addToFirst)
            {
                go.transform.SetSiblingIndex(0);
            }
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
#endif
            return go;
        }

        /// <summary>
        /// <para xml:lang="en">Create ARSession origin (when Unity XR Framework like AR Foundation is not used).</para>
        /// <para xml:lang="zh">创建ARSession原点（未使用Unity XR框架比如AR Foundation时）。</para>
        /// </summary>
        public static GameObject CreateOrigin()
        {
            return CreateOrigin(null);
        }

        /// <summary>
        /// <para xml:lang="en">Add <see cref="XROriginChildController"/> ARSession origin.</para>
        /// <para xml:lang="zh">添加<see cref="XROriginChildController"/>到ARSession原点。</para>
        /// </summary>
        public static GameObject AddOriginChild(GameObject origin)
        {
            var go = CreateObject<XROriginChildController>("XR Origin Child");
            go.transform.SetParent(origin.transform, false);
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
#endif
            return go;
        }

        /// <summary>
        /// <para xml:lang="en">Create video recorder.</para>
        /// <para xml:lang="zh">创建video recorder。</para>
        /// </summary>
        public static GameObject CreateVideoRecorder()
        {
            var go = new GameObject(DefaultName<VideoRecorder>(), typeof(VideoRecorder));
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
#endif
            return go;
        }

        /// <summary>
        /// <para xml:lang="en">Create controller. Resources are required when creating sparse spatial map.</para>
        /// <para xml:lang="zh">创建控制器。创建稀疏空间地图时需传入对应资源。</para>
        /// </summary>
        public static GameObject CreateController<Controller>(Resources resources = null)
        {
            var type = typeof(Controller);
            if (type != typeof(ImageTargetController)
                && type != typeof(ObjectTargetController)
                && type != typeof(SurfaceTargetController)
                && type != typeof(SparseSpatialMapController)
                )
            {
                throw new InvalidOperationException($"{typeof(Controller)} is not a valid Controller");
            }
            GameObject go;
            if (type == typeof(SparseSpatialMapController))
            {
                go = CreateSparseSpatialMap(resources);
            }
            else
            {
                go = CreateObject<Controller>();
            }
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
#endif
            return go;
        }

        /// <summary>
        /// <para xml:lang="en">Setup frame filters to meet preset requirements.</para>
        /// <para xml:lang="zh">配置frame filter以符合预设需求。</para>
        /// </summary>
        public static void SetupFrameFilters(List<GameObject> filters, ARSessionPreset preset)
        {
            if (filters == null) { return; }

            foreach (var filter in filters)
            {
                SetupMegaTracker(filter, preset);
                SetupImageTracker(filter, preset);
                SetupObjectTracker(filter, preset);
            }
        }

        /// <summary>
        /// <para xml:lang="en">Setup image tracker to meet preset requirements.</para>
        /// <para xml:lang="zh">配置image tracker以符合预设需求。</para>
        /// </summary>
        public static void SetupImageTracker(GameObject filter, ARSessionPreset preset)
        {
            var enableMotionFusion = Optional<bool>.Empty;
            switch (preset)
            {
                case ARSessionPreset.ImageTracking:
                case ARSessionPreset.CloudRecognition:
                    enableMotionFusion = false;
                    break;
                case ARSessionPreset.ImageTrackingMotionFusion:
                    enableMotionFusion = true;
                    break;
                default:
                    break;
            }
            if (enableMotionFusion.OnSome)
            {
                var tracker = filter.GetComponent<ImageTrackerFrameFilter>();
                if (tracker)
                {
                    tracker.EnableMotionFusion = enableMotionFusion.Value;
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Setup object tracker to meet preset requirements.</para>
        /// <para xml:lang="zh">配置object tracker以符合预设需求。</para>
        /// </summary>
        public static void SetupObjectTracker(GameObject filter, ARSessionPreset preset)
        {
            var enableMotionFusion = Optional<bool>.Empty;
            switch (preset)
            {
                case ARSessionPreset.ObjectTracking:
                    enableMotionFusion = false;
                    break;
                case ARSessionPreset.ObjectTrackingMotionFusion:
                    enableMotionFusion = true;
                    break;
                default:
                    break;
            }
            if (enableMotionFusion.OnSome)
            {
                var tracker = filter.GetComponent<ObjectTrackerFrameFilter>();
                if (tracker)
                {
                    tracker.EnableMotionFusion = enableMotionFusion.Value;
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Setup Mega tracker to meet preset requirements.</para>
        /// <para xml:lang="zh">配置Mega tracker以符合预设需求。</para>
        /// </summary>
        public static void SetupMegaTracker(GameObject filter, ARSessionPreset preset)
        {
            var minLevel = Optional<MegaInputFrameLevel>.Empty;
            switch (preset)
            {
                case ARSessionPreset.MegaBlock_MotionTracking:
                case ARSessionPreset.MegaLandmark_MotionTracking:
                    minLevel = MegaInputFrameLevel.SixDof;
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial:
                    minLevel = MegaInputFrameLevel.FiveDof;
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF:
                    minLevel = MegaInputFrameLevel.ThreeDof;
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF_0DOF:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF_0DOF:
                    minLevel = MegaInputFrameLevel.ZeroDof;
                    break;
                default:
                    break;
            }
            var apiType = Optional<MegaApiType>.Empty;
            switch (preset)
            {
                case ARSessionPreset.MegaBlock_MotionTracking:
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial:
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF:
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF_0DOF:
                    apiType = MegaApiType.Block;
                    break;
                case ARSessionPreset.MegaLandmark_MotionTracking:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF_0DOF:
                    apiType = MegaApiType.Landmark;
                    break;
                default:
                    break;
            }

            if (minLevel.OnSome)
            {
                var megaTracker = filter.GetComponent<MegaTrackerFrameFilter>();
                if (megaTracker)
                {
                    megaTracker.ServiceType = apiType.Value;
                    megaTracker.MinInputFrameLevel = minLevel.Value;
                }
                var localizer = filter.GetComponent<CloudLocalizerFrameFilter>();
                if (localizer)
                {
                    localizer.ServiceType = apiType.Value;
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Sort frame source under ARSession object.</para>
        /// <para xml:lang="zh">对ARSession物体下的frame source进行排序。</para>
        /// </summary>
        public static void SortFrameSource(GameObject sessionObj, FrameSourceSortMethod method)
        {
            if (!IsSession(sessionObj))
            {
                throw new InvalidOperationException($"{sessionObj} is not {nameof(ARSession)}");
            }
            var parent = sessionObj.transform;
            foreach (Transform t in sessionObj.transform)
            {
                if (t.name == "Frame Source Group")
                {
                    parent = t;
                    break;
                }
            }
            var sources = sessionObj.GetComponentsInChildren<FrameSource>(true).Where(s => s.transform != sessionObj.transform && s.transform != parent).ToList();
            foreach (var source in sources)
            {
                if (source.transform.parent != parent)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        UnityEditor.Undo.SetTransformParent(source.transform, parent, false, $"Sort frame sources under {parent}");
                        continue;
                    }
#endif
                    source.transform.SetParent(parent, false);
                }
            }
            if (method.ARCore.OnSome)
            {
                var list = sources.Select(source => (source, source.transform.GetSiblingIndex())).OrderBy(i => i.Item2).ToList();
                if (method.ARCore == FrameSourceSortMethod.ARCoreSortMethod.PreferEasyAR)
                {
                    SortFrameSource(list, new List<Type> { typeof(ARCoreFrameSource) }, new List<Type> { typeof(ARCoreARFoundationFrameSource) });
                }
                else if (method.ARCore == FrameSourceSortMethod.ARCoreSortMethod.PreferARFoundation)
                {
                    SortFrameSource(list, new List<Type> { typeof(ARCoreARFoundationFrameSource) }, new List<Type> { typeof(ARCoreFrameSource) });
                }
            }
            if (method.ARKit.OnSome)
            {
                var list = sources.Select(source => (source, source.transform.GetSiblingIndex())).OrderBy(i => i.Item2).ToList();
                if (method.ARKit == FrameSourceSortMethod.ARKitSortMethod.PreferEasyAR)
                {
                    SortFrameSource(list, new List<Type> { typeof(ARKitFrameSource) }, new List<Type> { typeof(ARKitARFoundationFrameSource) });
                }
                else if (method.ARKit == FrameSourceSortMethod.ARKitSortMethod.PreferARFoundation)
                {
                    SortFrameSource(list, new List<Type> { typeof(ARKitARFoundationFrameSource) }, new List<Type> { typeof(ARKitFrameSource) });
                }
            }
            if (method.MotionTracker.OnSome)
            {
                var list = sources.Select(source => (source, source.transform.GetSiblingIndex())).OrderBy(i => i.Item2).ToList();
                if (method.MotionTracker == FrameSourceSortMethod.MotionTrackerSortMethod.PreferEasyAR)
                {
                    SortFrameSource(list, new List<Type> { typeof(MotionTrackerFrameSource) }, new List<Type> { typeof(ARCoreFrameSource), typeof(ARCoreARFoundationFrameSource), typeof(ARKitFrameSource), typeof(ARKitARFoundationFrameSource), typeof(AREngineFrameSource) });
                }
                else if (method.MotionTracker == FrameSourceSortMethod.MotionTrackerSortMethod.PreferSystem)
                {
                    SortFrameSource(list, new List<Type> { typeof(ARCoreFrameSource), typeof(ARCoreARFoundationFrameSource), typeof(ARKitFrameSource), typeof(ARKitARFoundationFrameSource), typeof(AREngineFrameSource) }, new List<Type> { typeof(MotionTrackerFrameSource) });
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Default name of component.</para>
        /// <para xml:lang="zh">组件的默认名称。</para>
        /// </summary>
        public static string DefaultName<Component>()
        {
            return DefaultName(typeof(Component));
        }

        /// <summary>
        /// <para xml:lang="en">Default name of component.</para>
        /// <para xml:lang="zh">组件的默认名称。</para>
        /// </summary>
        public static string DefaultName(Type type)
        {
            return string.Join(" ", Regex.Split(type.Name.Replace("FrameSource", "").Replace("FrameFilter", "").Replace("Controller", "").Replace("VisionOS", "Visionos_"), @"(?<!^)(?<![A-Z])(?=[A-Z])")).Replace("Visionos_", "VisionOS");
        }

        /// <summary>
        /// <para xml:lang="en">If the object is AR Session.</para>
        /// <para xml:lang="zh">是否是ARSession。</para>
        /// </summary>
        public static bool IsSession(GameObject sessionObj) => sessionObj && sessionObj.GetComponent<ARSession>();

        internal static GameObject CreateOrigin(XROriginChildController child)
        {
            var go = new GameObject("XR Origin (EasyAR)");
            if (child)
            {
                child.transform.SetParent(go.transform, false);
            }
            else
            {
                AddOriginChild(go);
            }
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
#endif
            return go;
        }

        internal static GameObject CreateSparseSpatialMapBuildTarget(Resources resources)
        {
            var go = CreateObject<SparseSpatialMapBuildTargetController>();
            var controller = go.GetComponent<SparseSpatialMapBuildTargetController>();
            var pgo = CreateSparseSpatialMapParticleSystem(resources, true);
            pgo.transform.SetParent(go.transform, false);
            controller.GetComponent<SparseSpatialMapPointCloudRenderer>().ParticleSystem = pgo.GetComponent<ParticleSystem>();
            return go;
        }

        static GameObject CreateSession(string name, Func<List<GameObject>> createFrameSources, Func<List<GameObject>> createFrameFilters)
        {
            var session = CreateObject<ARSession>(name);
            session.GetComponent<FrameRecorder>().enabled = false;
            void parentSession(GameObject go) => go.transform.SetParent(session.transform, false);

            var sources = createFrameSources();
            if (sources != null)
            {
                if (sources.Count == 1)
                {
                    parentSession(sources[0]);
                }
                else
                {
                    var group = new GameObject("Frame Source Group");
                    foreach (var source in sources)
                    {
                        source.transform.SetParent(group.transform, false);
                    }
                    parentSession(group);
                }
            }

            var filters = createFrameFilters();
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    parentSession(filter);
                }
            }

#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(session, $"Create {name}");
#endif
            return session;
        }

        static List<GameObject> CreateFrameFilters(ARSessionPreset preset, Resources resources = null)
        {
            var filters = new List<GameObject>();
            switch (preset)
            {
                case ARSessionPreset.SparseSpatialMapBuilder:
                    filters.Add(CreateSparseSpatialMapBuilder(resources));
                    break;
                case ARSessionPreset.SparseSpatialMapTracker:
                    filters.Add(CreateObject<SparseSpatialMapTrackerFrameFilter>());
                    break;
                case ARSessionPreset.DenseSpatialMapBuilder:
                    filters.Add(CreateDenseSpatialMapBuilder(resources));
                    break;
                case ARSessionPreset.ImageTracking:
                    filters.Add(CreateObject<ImageTrackerFrameFilter>());
                    break;
                case ARSessionPreset.CloudRecognition:
                    filters.Add(CreateObject<ImageTrackerFrameFilter>());
                    filters.Add(CreateObject<CloudRecognizerFrameFilter>());
                    break;
                case ARSessionPreset.ObjectTracking:
                    filters.Add(CreateObject<ObjectTrackerFrameFilter>());
                    break;
                case ARSessionPreset.SurfaceTracking:
                    filters.Add(CreateObject<SurfaceTrackerFrameFilter>());
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking:
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial:
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF:
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF_0DOF:
                case ARSessionPreset.MegaLandmark_MotionTracking:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF_0DOF:
                    filters.Add(CreateObject<MegaTrackerFrameFilter>());
                    break;
                case ARSessionPreset.ImageTrackingMotionFusion:
                    filters.Add(CreateObject<ImageTrackerFrameFilter>());
                    break;
                case ARSessionPreset.ObjectTrackingMotionFusion:
                    filters.Add(CreateObject<ObjectTrackerFrameFilter>());
                    break;
                default:
                    break;
            }
            SetupFrameFilters(filters, preset);
            return filters;
        }

        static List<GameObject> CreateFrameSources(ARSessionPreset preset)
        {
            var sources = new List<GameObject>();
            switch (preset)
            {
                case ARSessionPreset.ImageTracking:
                case ARSessionPreset.CloudRecognition:
                case ARSessionPreset.ObjectTracking:
                case ARSessionPreset.SurfaceTracking:
                    sources.Add(CreateObject<CameraDeviceFrameSource>());
                    break;
                case ARSessionPreset.MotionTracking:
                case ARSessionPreset.SparseSpatialMapBuilder:
                case ARSessionPreset.SparseSpatialMapTracker:
                case ARSessionPreset.DenseSpatialMapBuilder:
                    sources.Add(CreateObject<XREALFrameSource>());
                    sources.Add(CreateObject<AREngineFrameSource>());
                    sources.Add(CreateObject<ARCoreFrameSource>());
                    sources.Add(CreateObject<ARCoreARFoundationFrameSource>());
                    sources.Add(CreateObject<ARKitFrameSource>());
                    sources.Add(CreateObject<ARKitARFoundationFrameSource>());
                    sources.Add(CreateObject<VisionOSARKitFrameSource>());
                    sources.Add(CreateObject<MotionTrackerFrameSource>());
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking:
                case ARSessionPreset.MegaLandmark_MotionTracking:
                    sources.Add(CreateObject<XREALFrameSource>());
                    sources.Add(CreateObject<AREngineFrameSource>());
                    sources.Add(CreateObject<ARCoreFrameSource>());
                    sources.Add(CreateObject<ARCoreARFoundationFrameSource>());
                    sources.Add(CreateObject<ARKitFrameSource>());
                    sources.Add(CreateObject<ARKitARFoundationFrameSource>());
                    sources.Add(CreateObject<VisionOSARKitFrameSource>());
                    sources.Add(CreateObject<MotionTrackerFrameSource>());
                    sources.Add(CreateObject<EditorCameraDeviceFrameSource>());
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial:
                    sources.Add(CreateObject<XREALFrameSource>());
                    sources.Add(CreateObject<AREngineFrameSource>());
                    sources.Add(CreateObject<ARCoreFrameSource>());
                    sources.Add(CreateObject<ARCoreARFoundationFrameSource>());
                    sources.Add(CreateObject<ARKitFrameSource>());
                    sources.Add(CreateObject<ARKitARFoundationFrameSource>());
                    sources.Add(CreateObject<VisionOSARKitFrameSource>());
                    sources.Add(CreateObject<MotionTrackerFrameSource>());
                    sources.Add(CreateObject<InertialCameraDeviceFrameSource>());
                    sources.Add(CreateObject<EditorCameraDeviceFrameSource>());
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF:
                    sources.Add(CreateObject<XREALFrameSource>());
                    sources.Add(CreateObject<AREngineFrameSource>());
                    sources.Add(CreateObject<ARCoreFrameSource>());
                    sources.Add(CreateObject<ARCoreARFoundationFrameSource>());
                    sources.Add(CreateObject<ARKitFrameSource>());
                    sources.Add(CreateObject<ARKitARFoundationFrameSource>());
                    sources.Add(CreateObject<VisionOSARKitFrameSource>());
                    sources.Add(CreateObject<MotionTrackerFrameSource>());
                    sources.Add(CreateObject<InertialCameraDeviceFrameSource>());
                    sources.Add(CreateObject<ThreeDofCameraDeviceFrameSource>());
                    sources.Add(CreateObject<EditorCameraDeviceFrameSource>());
                    break;
                case ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF_0DOF:
                case ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF_0DOF:
                    sources.Add(CreateObject<XREALFrameSource>());
                    sources.Add(CreateObject<AREngineFrameSource>());
                    sources.Add(CreateObject<ARCoreFrameSource>());
                    sources.Add(CreateObject<ARCoreARFoundationFrameSource>());
                    sources.Add(CreateObject<ARKitFrameSource>());
                    sources.Add(CreateObject<ARKitARFoundationFrameSource>());
                    sources.Add(CreateObject<VisionOSARKitFrameSource>());
                    sources.Add(CreateObject<MotionTrackerFrameSource>());
                    sources.Add(CreateObject<InertialCameraDeviceFrameSource>());
                    sources.Add(CreateObject<ThreeDofCameraDeviceFrameSource>());
                    sources.Add(CreateObject<CameraDeviceFrameSource>());
                    break;
                case ARSessionPreset.ImageTrackingMotionFusion:
                case ARSessionPreset.ObjectTrackingMotionFusion:
                    sources.Add(CreateObject<XREALFrameSource>());
                    sources.Add(CreateObject<AREngineFrameSource>());
                    sources.Add(CreateObject<ARCoreFrameSource>());
                    sources.Add(CreateObject<ARCoreARFoundationFrameSource>());
                    sources.Add(CreateObject<ARKitFrameSource>());
                    sources.Add(CreateObject<ARKitARFoundationFrameSource>());
                    sources.Add(CreateObject<VisionOSARKitFrameSource>());
                    sources.Add(CreateObject<MotionTrackerFrameSource>());
                    sources.Add(CreateObject<CameraDeviceFrameSource>());
                    break;
                default:
                    break;
            }
            return sources;
        }

        static GameObject CreateSparseSpatialMap(Resources resources)
        {
            var go = CreateObject<SparseSpatialMapController>();
            var controller = go.GetComponent<SparseSpatialMapController>();
            var pgo = CreateSparseSpatialMapParticleSystem(resources, false);
            pgo.transform.SetParent(go.transform, false);
            controller.GetComponent<SparseSpatialMapPointCloudRenderer>().ParticleSystem = pgo.GetComponent<ParticleSystem>();
            return go;
        }

        static GameObject CreateSparseSpatialMapParticleSystem(Resources resources, bool allowEmpty)
        {
            var pgo = new GameObject("Point Cloud Particle System", typeof(ParticleSystem));

            var particle = pgo.GetComponent<ParticleSystem>();
            var main = particle.main;
            main.loop = false;
            main.startSize = 0.015f;
            main.startColor = new Color(11f / 255f, 205f / 255f, 255f / 255f, 1);
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.playOnAwake = false;
            var emission = particle.emission;
            emission.enabled = false;
            var shape = particle.shape;
            shape.enabled = false;
            var renderer = particle.GetComponent<Renderer>();
            if (!allowEmpty && (resources == null || !resources.SparseSpatialMapPointCloudMaterial))
            {
                throw new InvalidOperationException($"{nameof(resources.SparseSpatialMapPointCloudMaterial)} does not exist");
            }
            renderer.material = resources.SparseSpatialMapPointCloudMaterial;
            return pgo;
        }

        static GameObject CreateSparseSpatialMapBuilder(Resources resources)
        {
            if (resources == null || !resources.SparseSpatialMapPointCloudMaterial)
            {
                throw new InvalidOperationException($"{nameof(resources.SparseSpatialMapPointCloudMaterial)} does not exist");
            }
            var go = CreateObject<SparseSpatialMapBuilderFrameFilter>();
            var filter = go.GetComponent<SparseSpatialMapBuilderFrameFilter>();
            filter.PointCloudMaterial = resources.SparseSpatialMapPointCloudMaterial;
            return go;
        }

        static GameObject CreateDenseSpatialMapBuilder(Resources resources)
        {
            var go = CreateObject<DenseSpatialMapBuilderFrameFilter>();
            var filter = go.GetComponent<DenseSpatialMapBuilderFrameFilter>();
            if (resources == null || !resources.DenseSpatialMapMeshMaterial)
            {
                throw new InvalidOperationException($"{nameof(resources.DenseSpatialMapMeshMaterial)} does not exist");
            }
            filter.MapMeshMaterial = resources.DenseSpatialMapMeshMaterial;
            return go;
        }

        static GameObject CreateObject<Component>(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = DefaultName<Component>();
            }
            return new GameObject(name, typeof(Component));
        }

        static void SortFrameSource(List<(FrameSource, int)> list, List<Type> firstTypes, List<Type> secondTypes)
        {
            var firstList = list.Where(i => firstTypes.Contains(i.Item1.GetType())).ToList();
            var secondList = list.Where(i => secondTypes.Contains(i.Item1.GetType())).ToList();
            if (!firstList.Any() || !secondList.Any()) { return; }

            var secondMin = secondList.First().Item2;
            if (firstList.Last().Item2 <= secondMin) { return; }

#if UNITY_EDITOR
            var parent = firstList.First().Item1.transform.parent;
            UnityEditor.Undo.RegisterChildrenOrderUndo(parent, $"Sort frame sources under {parent}");
#endif
            var index = secondMin;
            foreach (var first in firstList.Where(i => i.Item2 > secondMin))
            {
                first.Item1.transform.SetSiblingIndex(index);
                index++;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Resources required to create components.</para>
        /// <para xml:lang="zh">创建对应组件所需资源。</para>
        /// </summary>
        public class Resources
        {
            /// <summary>
            /// <para xml:lang="en">Resources required to SparseSpatialMapController.</para>
            /// <para xml:lang="zh">创建SparseSpatialMapController所需资源。</para>
            /// </summary>
            public Material SparseSpatialMapPointCloudMaterial;
            /// <summary>
            /// <para xml:lang="en">Resources required to DenseSpatialMapBuilderFrameFilter.</para>
            /// <para xml:lang="zh">创建DenseSpatialMapBuilderFrameFilter所需资源。</para>
            /// </summary>
            public Material DenseSpatialMapMeshMaterial;

            /// <summary>
            /// <para xml:lang="en">Default resources loadable in editor.</para>
            /// <para xml:lang="zh">编辑器可加载的默认资源。</para>
            /// </summary>
            public static Resources EditorDefault()
            {
#if UNITY_EDITOR
                return new Resources
                {
                    SparseSpatialMapPointCloudMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>($"Packages/{UnityPackage.Name}/Assets/Materials/PointCloudParticle.mat"),
                    DenseSpatialMapMeshMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>($"Packages/{UnityPackage.Name}/Assets/Materials/DenseSpatialMapMesh.mat"),
                };
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// <para xml:lang="en">Frame source sort method .</para>
        /// <para xml:lang="zh">Frame source 的排序方法。</para>
        /// </summary>
        public struct FrameSourceSortMethod
        {
            /// <summary>
            /// <para xml:lang="en">ARCore/ARCoreARFoundation frame source sort method. Leave empty to keep unchanged.</para>
            /// <para xml:lang="zh">ARCore/ARCoreARFoundation frame source 的排序方法。不设置则保持不变。</para>
            /// </summary>
            public Optional<ARCoreSortMethod> ARCore;
            /// <summary>
            /// <para xml:lang="en">ARKit/ARKitARFoundation frame source sort method. Leave empty to keep unchanged.</para>
            /// <para xml:lang="zh">ARKit/ARKitARFoundation frame source 的排序方法。不设置则保持不变。</para>
            /// </summary>
            public Optional<ARKitSortMethod> ARKit;
            /// <summary>
            /// <para xml:lang="en">EasyAR motion tracker/System SLAM (ARCore, ARKit, AREngine) frame source sort method. Leave empty to keep unchanged.</para>
            /// <para xml:lang="zh">EasyAR motion tracker/System SLAM (ARCore, ARKit, AREngine) frame source 的排序方法。不设置则保持不变。</para>
            /// </summary>
            public Optional<MotionTrackerSortMethod> MotionTracker;

            /// <summary>
            /// <para xml:lang="en">ARCore/ARCoreARFoundation frame source sort method.</para>
            /// <para xml:lang="zh">ARCore/ARCoreARFoundation frame source 的排序方法。</para>
            /// </summary>
            public enum ARCoreSortMethod
            {
                /// <summary>
                /// <para xml:lang="en">Prefer EasyAR ARCore wrapper.</para>
                /// <para xml:lang="zh">优先EasyAR ARCore 封装。</para>
                /// </summary>
                PreferEasyAR,
                /// <summary>
                /// <para xml:lang="en">Prefer ARFoundation ARCore wrapper.</para>
                /// <para xml:lang="zh">优先ARFoundation ARCore 封装。</para>
                /// </summary>
                PreferARFoundation,
            }
            /// <summary>
            /// <para xml:lang="en">ARKit/ARKitARFoundation frame source sort method.</para>
            /// <para xml:lang="zh">ARKit/ARKitARFoundation frame source 的排序方法。</para>
            /// </summary>
            public enum ARKitSortMethod
            {
                /// <summary>
                /// <para xml:lang="en">Prefer EasyAR ARKit wrapper.</para>
                /// <para xml:lang="zh">优先EasyAR ARKit 封装。</para>
                /// </summary>
                PreferEasyAR,
                /// <summary>
                /// <para xml:lang="en">Prefer ARFoundation ARKit wrapper.</para>
                /// <para xml:lang="zh">优先ARFoundation ARKit 封装。</para>
                /// </summary>
                PreferARFoundation,
            }
            /// <summary>
            /// <para xml:lang="en">EasyAR motion tracker/System SLAM (ARCore, ARKit, AREngine) frame source sort method.</para>
            /// <para xml:lang="zh">EasyAR motion tracker/System SLAM (ARCore, ARKit, AREngine) frame source 的排序方法。</para>
            /// </summary>
            public enum MotionTrackerSortMethod
            {
                /// <summary>
                /// <para xml:lang="en">Prefer system SLAM (ARCore, ARKit, AREngine).</para>
                /// <para xml:lang="zh">优先系统SLAM（ARCore, ARKit, AREngine）。</para>
                /// </summary>
                PreferSystem,
                /// <summary>
                /// <para xml:lang="en">Prefer EasyAR motion tracker.</para>
                /// <para xml:lang="zh">优先EasyAR motion tracker。</para>
                /// </summary>
                PreferEasyAR,
            }
        }
    }
}
