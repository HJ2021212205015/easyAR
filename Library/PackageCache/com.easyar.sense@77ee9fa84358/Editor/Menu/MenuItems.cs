//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace easyar
{
    public static class ExtensionMenuItemHelper
    {
        public const int Priority = 31;
        public const string MenuPath = "GameObject/EasyAR Sense/Extensions/";

        public static GameObject AddFrameSource<Source>() where Source : FrameSource
        {
            var go = ARSessionFactory.AddFrameSource<Source>(Selection.activeGameObject, true);
            return go;
        }

        public static GameObject KeepFrameSourceOnly<Source>() where Source : FrameSource
        {
            var sessionObj = Selection.activeGameObject;
            if (!ARSessionFactory.IsSession(sessionObj))
            {
                throw new InvalidOperationException($"{sessionObj} is not part of {nameof(ARSession)} or it is not empty");
            }

            GameObject go = null;
            foreach (var frameSource in sessionObj.GetComponentsInChildren<FrameSource>(true).Where(f => !(f is FramePlayer) && !f.GetComponent<ARSession>()))
            {
                if (frameSource is Source)
                {
                    if (!go)
                    {
                        go = frameSource.gameObject;
                    }
                    else
                    {
                        Undo.DestroyObjectImmediate(frameSource.gameObject);
                    }
                }
                else
                {
                    Undo.DestroyObjectImmediate(frameSource.gameObject);
                }
            }
            if (!go)
            {
                go = ARSessionFactory.AddFrameSource<Source>(sessionObj, true);
            }
            if (!go.activeSelf)
            {
                Undo.RecordObject(go, "active");
                go.SetActive(true);
            }
            return go;
        }

        public static bool AddFrameSourceValidate()
        {
            return ARSessionFactory.IsSession(Selection.activeGameObject);
        }
    }

    class MenuItems
    {
        const int pMega = 20;
        const int pSpatialMap = 21;
        const int pMotionTracking = 22;
        const int pSurfaceTracking = 23;
        const int pImageTracking = 24;
        const int pObjectTracking = 25;
        const int pHMD = 26;
        const int pSource = 27;
        const int pOrigin = 28;
        const int pSession = 29;
        const int pUtil = 30;
        const int pVideo = 30;
        const string mRoot = "GameObject/EasyAR Sense/";
        const string mSession = "AR Session (Preset)/";
        const string mMega = "Mega/";
        const string mSpatialMap = "SpatialMap/";
        const string mMotionTracking = "Motion Tracking/";
        const string mSurfaceTracking = "Surface Tracking/";
        const string mImageTracking = "Image Tracking/";
        const string mObjectTracking = "Object Tracking/";
        const string mHMD = "Head Mounted Display (built-in)/";
        const string mSource = "Frame Source by Transform Type/";
        const string mSourceZeroDof = mSource + "0 Dof/";
        const string mSourceThreeDofRotOnly = mSource + "3 Dof Rot-Only/";
        const string mSourceSixDof = mSource + "6 Dof/";
        const string mSourceFiveDofRotXZ = mSource + "5 Dof Rot-XZ/";
        const string mOrigin = "Origin/";
        const string mUtil = "Utility/";
        const string mVideo = "Video/";

        #region ARSession

        [MenuItem(mRoot + mSession + "AR Session (Empty)", priority = pSession)]
        static void ARSession() => ARSessionFactory.CreateSession();

        [MenuItem(mRoot + mSession + "AR Session (Mega Block Default Preset)", priority = pSession)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking_Inertial Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block Default Preset)", priority = pMega)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking_Inertial Preset)", priority = pMega)]
        static void ARSessionPresetMegaBlock_MotionTracking_Inertial() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaBlock_MotionTracking_Inertial);

        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking Preset)", priority = pMega)]
        static void ARSessionPresetMegaBlock_MotionTracking() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaBlock_MotionTracking);

        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking_Inertial_3DOF Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking_Inertial_3DOF Preset)", priority = pMega)]
        static void ARSessionPresetMegaBlock_MotionTracking_Inertial_3DOF() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF);

        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking_Inertial_3DOF_0DOF Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking_Inertial_3DOF_0DOF Preset)", priority = pMega)]
        static void ARSessionPresetMegaBlock_MotionTracking_Inertial_3DOF_0DOF() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaBlock_MotionTracking_Inertial_3DOF_0DOF);

        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark Default Preset)", priority = pSession)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking_Inertial Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark Default Preset)", priority = pMega)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking_Inertial Preset)", priority = pMega)]
        static void ARSessionPresetMegaLandmark_MotionTracking_Inertial() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaLandmark_MotionTracking_Inertial);

        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking Preset)", priority = pMega)]
        static void ARSessionPresetMegaLandmark_MotionTracking() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaLandmark_MotionTracking);

        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF Preset)", priority = pMega)]
        static void ARSessionPresetMegaLandmark_MotionTracking_Inertial_3DOF() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF);

        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF_0DOF Preset)", priority = pSession)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF_0DOF Preset)", priority = pMega)]
        static void ARSessionPresetMegaLandmark_MotionTracking_Inertial_3DOF_0DOF() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MegaLandmark_MotionTracking_Inertial_3DOF_0DOF);

        [MenuItem(mRoot + mSession + "AR Session (Sparse SpatialMap Build Preset)", priority = pSession)]
        [MenuItem(mRoot + mSpatialMap + "AR Session (Sparse SpatialMap Build Preset)", priority = pSpatialMap)]
        static void ARSessionPresetSparseSpatialMapBuild() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.SparseSpatialMapBuilder, ARSessionFactory.Resources.EditorDefault());

        [MenuItem(mRoot + mSession + "AR Session (Sparse SpatialMap Track Preset)", priority = pSession)]
        [MenuItem(mRoot + mSpatialMap + "AR Session (Sparse SpatialMap Track Preset)", priority = pSpatialMap)]
        static void ARSessionPresetSparseSpatialMapTrack() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.SparseSpatialMapTracker);

        [MenuItem(mRoot + mSession + "AR Session (Dense SpatialMap Build Preset)", priority = pSession)]
        [MenuItem(mRoot + mSpatialMap + "AR Session (Dense SpatialMap Build Preset)", priority = pSpatialMap)]
        static void ARSessionPresetDenseSpatialMap() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.DenseSpatialMapBuilder, ARSessionFactory.Resources.EditorDefault());

        [MenuItem(mRoot + mSession + "AR Session (Motion Tracking Preset)", priority = pSession)]
        [MenuItem(mRoot + mMotionTracking + "AR Session (Motion Tracking Preset)", priority = pMotionTracking)]
        static void ARSessionPresetMotionTracking() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.MotionTracking);

        [MenuItem(mRoot + mSession + "AR Session (Surface Tracking Preset)", priority = pSession)]
        [MenuItem(mRoot + mSurfaceTracking + "AR Session (Surface Tracking Preset)", priority = pSurfaceTracking)]
        static void ARSessionPresetSurfaceTracking() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.SurfaceTracking);

        [MenuItem(mRoot + mSession + "AR Session (Image Tracking Preset)", priority = pSession)]
        [MenuItem(mRoot + mImageTracking + "AR Session (Image Tracking Preset)", priority = pImageTracking)]
        static void ARSessionPresetImageTracking() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.ImageTracking);

        [MenuItem(mRoot + mSession + "AR Session (Image Tracking with Motion Fusion Preset)", priority = pSession)]
        [MenuItem(mRoot + mImageTracking + "AR Session (Image Tracking with Motion Fusion Preset)", priority = pImageTracking)]
        static void ARSessionPresetImageTrackingMotionFusion() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.ImageTrackingMotionFusion);

        [MenuItem(mRoot + mSession + "AR Session (CRS Preset)", priority = pSession)]
        [MenuItem(mRoot + mImageTracking + "AR Session (CRS Preset)", priority = pImageTracking)]
        static void ARSessionPresetCloudRecognizer() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.CloudRecognition);

        [MenuItem(mRoot + mSession + "AR Session (Object Tracking Preset)", priority = pSession)]
        [MenuItem(mRoot + mObjectTracking + "AR Session (Object Tracking Preset)", priority = pObjectTracking)]
        static void ARSessionPresetObjectTracking() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.ObjectTracking);

        [MenuItem(mRoot + mSession + "AR Session (Object Tracking with Motion Fusion Preset)", priority = pSession)]
        [MenuItem(mRoot + mObjectTracking + "AR Session (Object Tracking with Motion Fusion Preset)", priority = pObjectTracking)]
        static void ARSessionPresetObjectTrackingMotionFusion() => ARSessionFactory.CreateSession(ARSessionFactory.ARSessionPreset.ObjectTrackingMotionFusion);

        #endregion

        #region Origin

        [MenuItem(mRoot + mOrigin + "Origin : XR Origin (EasyAR)", priority = pOrigin)]
        static void XROrigin() => ARSessionFactory.CreateOrigin();

        // validate function not work correctly (perhaps root menu conflict), so do not validate and report error if object not valid
        [MenuItem(mRoot + mOrigin + "Origin : XR Origin Child", priority = pOrigin)]
        static void XROriginChild() => ARSessionFactory.AddOriginChild(Selection.activeGameObject);

        #endregion

        #region FrameSource
        [MenuItem(mRoot + mSourceSixDof + "Frame Source : Motion Tracker", priority = pSource)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : Motion Tracker", priority = pMotionTracking)]
        static void MotionTracker() => ARSessionFactory.AddFrameSource<MotionTrackerFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARCore", priority = pSource)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARCore", priority = pMotionTracking)]
        static void ARCore() => ARSessionFactory.AddFrameSource<ARCoreFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARCore ARFoundation", priority = pSource)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARCore ARFoundation", priority = pMotionTracking)]
        static void ARCoreARFoundation() => ARSessionFactory.AddFrameSource<ARCoreARFoundationFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARKit", priority = pSource)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARKit", priority = pMotionTracking)]
        static void ARKit() => ARSessionFactory.AddFrameSource<ARKitFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARKit ARFoundation", priority = pSource)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARKit ARFoundation", priority = pMotionTracking)]
        static void ARKitARFoundation() => ARSessionFactory.AddFrameSource<ARKitARFoundationFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceSixDof + "Frame Source : AREngine", priority = pSource)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : AREngine", priority = pMotionTracking)]
        static void AREngine() => ARSessionFactory.AddFrameSource<AREngineFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceFiveDofRotXZ + "Frame Source : Inertial Camera Device", priority = pSource)]
        static void InertialCameraDevice() => ARSessionFactory.AddFrameSource<InertialCameraDeviceFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceThreeDofRotOnly + "Frame Source : Three Dof Camera Device", priority = pSource)]
        static void ThreeDofCameraDevice() => ARSessionFactory.AddFrameSource<ThreeDofCameraDeviceFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceZeroDof + "Frame Source : Camera Device", priority = pSource)]
        [MenuItem(mRoot + mSurfaceTracking + "Frame Source : Camera Device", priority = pSurfaceTracking)]
        [MenuItem(mRoot + mImageTracking + "Frame Source : Camera Device", priority = pImageTracking)]
        [MenuItem(mRoot + mObjectTracking + "Frame Source : Camera Device", priority = pObjectTracking)]
        static void CameraDevice() => ARSessionFactory.AddFrameSource<CameraDeviceFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mSourceZeroDof + "Frame Source : Camera Device (Editor Only)", priority = pSource)]
        static void EditorCameraDevice() => ARSessionFactory.AddFrameSource<EditorCameraDeviceFrameSource>(Selection.activeGameObject);


        [MenuItem(mRoot + mHMD + "Frame Source : VisionOS ARKit", priority = pHMD)]
        static void VisionOSARKit() => ARSessionFactory.AddFrameSource<VisionOSARKitFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mHMD + "Frame Source : VisionOS ARKit (keep it only)", priority = pHMD)]
        static void KeepVisionOSARKitOnly() => ExtensionMenuItemHelper.KeepFrameSourceOnly<VisionOSARKitFrameSource>();

        [MenuItem(mRoot + mHMD + "Frame Source : XREAL", priority = pHMD)]
        static void XREAL() => ARSessionFactory.AddFrameSource<XREALFrameSource>(Selection.activeGameObject);

        [MenuItem(mRoot + mHMD + "Frame Source : XREAL (keep it only)", priority = pHMD)]
        static void KeepXREALOnly() => ExtensionMenuItemHelper.KeepFrameSourceOnly<XREALFrameSource>();
        #endregion

        #region FrameFilter

        [MenuItem(mRoot + mSurfaceTracking + "Frame Filter : Surface Tracker", priority = pSurfaceTracking)]
        static void SurfaceTracker() => ARSessionFactory.AddFrameFilter<SurfaceTrackerFrameFilter>(Selection.activeGameObject);

        [MenuItem(mRoot + mSurfaceTracking + "Target : Surface Target", priority = pSurfaceTracking)]
        static void SurfaceTarget() => ARSessionFactory.CreateController<SurfaceTargetController>();

        [MenuItem(mRoot + mImageTracking + "Frame Filter : Image Tracker", priority = pImageTracking)]
        static void ImageTracker() => ARSessionFactory.AddFrameFilter<ImageTrackerFrameFilter>(Selection.activeGameObject);

        [MenuItem(mRoot + mImageTracking + "Target : Image Target", priority = pImageTracking)]
        static void ImageTarget() => ARSessionFactory.CreateController<ImageTargetController>();

        [MenuItem(mRoot + mImageTracking + "Frame Filter : Cloud Recognizer", priority = pImageTracking)]
        static void CloudRecognizer() => ARSessionFactory.AddFrameFilter<CloudRecognizerFrameFilter>(Selection.activeGameObject);

        [MenuItem(mRoot + mObjectTracking + "Frame Filter : Object Tracker", priority = pObjectTracking)]
        static void ObjectTracker() => ARSessionFactory.AddFrameFilter<ObjectTrackerFrameFilter>(Selection.activeGameObject);

        [MenuItem(mRoot + mObjectTracking + "Target : Object Target", priority = pObjectTracking)]
        static void ObjectTarget() => ARSessionFactory.CreateController<ObjectTargetController>();


        [MenuItem(mRoot + mSpatialMap + "Frame Filter : Sparse SpatialMap Builder", priority = pSpatialMap)]
        static void SparseSpatialMapBuilder() => ARSessionFactory.AddFrameFilter<SparseSpatialMapBuilderFrameFilter>(Selection.activeGameObject, ARSessionFactory.Resources.EditorDefault());

        [MenuItem(mRoot + mSpatialMap + "Frame Filter : Sparse SpatialMap Tracker", priority = pSpatialMap)]
        static void SparseSpatialMapTracker() => ARSessionFactory.AddFrameFilter<SparseSpatialMapTrackerFrameFilter>(Selection.activeGameObject);

        [MenuItem(mRoot + mSpatialMap + "Frame Filter : Dense SpatialMap Builder", priority = pSpatialMap)]
        static void DenseSpatialMapBuilder() => ARSessionFactory.AddFrameFilter<DenseSpatialMapBuilderFrameFilter>(Selection.activeGameObject, ARSessionFactory.Resources.EditorDefault());

        [MenuItem(mRoot + mSpatialMap + "Target : Sparse SpatialMap", priority = pSpatialMap)]
        static void SparseSpatialMap() => ARSessionFactory.CreateController<SparseSpatialMapController>(ARSessionFactory.Resources.EditorDefault());


        [MenuItem(mRoot + mMega + "Frame Filter : Mega Tracker (for Mega Block)", priority = pMega)]
        static void MegaTracker_Block() => ARSessionFactory.SetupMegaTracker(ARSessionFactory.AddFrameFilter<MegaTrackerFrameFilter>(Selection.activeGameObject), ARSessionFactory.ARSessionPreset.MegaBlock_MotionTracking);

        [MenuItem(mRoot + mMega + "Frame Filter : Mega Localizer (for Mega Block)", priority = pMega)]
        static void MegaLocalizer_Block() => ARSessionFactory.SetupMegaTracker(ARSessionFactory.AddFrameFilter<CloudLocalizerFrameFilter>(Selection.activeGameObject), ARSessionFactory.ARSessionPreset.MegaBlock_MotionTracking);

        [MenuItem(mRoot + mMega + "Frame Filter : Mega Tracker (for Mega Landmark)", priority = pMega)]
        static void MegaTracker_Landmark() => ARSessionFactory.SetupMegaTracker(ARSessionFactory.AddFrameFilter<MegaTrackerFrameFilter>(Selection.activeGameObject), ARSessionFactory.ARSessionPreset.MegaLandmark_MotionTracking);

        [MenuItem(mRoot + mMega + "Frame Filter : Mega Localizer (for Mega Landmark)", priority = pMega)]
        static void MegaLocalizer_Landmark() => ARSessionFactory.SetupMegaTracker(ARSessionFactory.AddFrameFilter<CloudLocalizerFrameFilter>(Selection.activeGameObject), ARSessionFactory.ARSessionPreset.MegaLandmark_MotionTracking);
        #endregion

        #region Utility
        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARCore > ARCore ARFoundation", priority = pUtil)]
        static void SortARCore_EasyAR() => ARSessionFactory.SortFrameSource(Selection.activeGameObject, new ARSessionFactory.FrameSourceSortMethod { ARCore = ARSessionFactory.FrameSourceSortMethod.ARCoreSortMethod.PreferEasyAR });

        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARCore ARFoundation > ARCore", priority = pUtil)]
        static void SortARCore_ARFoundation() => ARSessionFactory.SortFrameSource(Selection.activeGameObject, new ARSessionFactory.FrameSourceSortMethod { ARCore = ARSessionFactory.FrameSourceSortMethod.ARCoreSortMethod.PreferARFoundation });

        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARKit > ARKit ARFoundation", priority = pUtil)]
        static void SortARKit_EasyAR() => ARSessionFactory.SortFrameSource(Selection.activeGameObject, new ARSessionFactory.FrameSourceSortMethod { ARKit = ARSessionFactory.FrameSourceSortMethod.ARKitSortMethod.PreferEasyAR });

        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARKit ARFoundation > ARKit", priority = pUtil)]
        static void SortARKit_ARFoundation() => ARSessionFactory.SortFrameSource(Selection.activeGameObject, new ARSessionFactory.FrameSourceSortMethod { ARKit = ARSessionFactory.FrameSourceSortMethod.ARKitSortMethod.PreferARFoundation });

        [MenuItem(mRoot + mUtil + "Sort Frame Source : System SLAM > Motion Tracker", priority = pUtil)]
        static void SortMotionTracker_System() => ARSessionFactory.SortFrameSource(Selection.activeGameObject, new ARSessionFactory.FrameSourceSortMethod { MotionTracker = ARSessionFactory.FrameSourceSortMethod.MotionTrackerSortMethod.PreferSystem });

        [MenuItem(mRoot + mUtil + "Sort Frame Source : Motion Tracker > System SLAM", priority = pUtil)]
        static void SortMotionTracker_EasyAR() => ARSessionFactory.SortFrameSource(Selection.activeGameObject, new ARSessionFactory.FrameSourceSortMethod { MotionTracker = ARSessionFactory.FrameSourceSortMethod.MotionTrackerSortMethod.PreferEasyAR });
        #endregion

        #region Extra
        [MenuItem(mRoot + mVideo + "Video Recorder", priority = pVideo)]
        static void VideoRecorder() => ARSessionFactory.CreateVideoRecorder();
        #endregion

        [MenuItem(mRoot + mSession + "AR Session (Empty)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Block Default Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block Default Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking_Inertial Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking_Inertial Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking_Inertial_3DOF Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking_Inertial_3DOF Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Block + MotionTracking_Inertial_3DOF_0DOF Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Block + MotionTracking_Inertial_3DOF_0DOF Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark Default Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark Default Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking_Inertial Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking_Inertial Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF_0DOF Preset)", true)]
        [MenuItem(mRoot + mMega + "AR Session (Mega Landmark + MotionTracking_Inertial_3DOF_0DOF Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Sparse SpatialMap Track Preset)", true)]
        [MenuItem(mRoot + mSpatialMap + "AR Session (Sparse SpatialMap Track Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Sparse SpatialMap Build Preset)", true)]
        [MenuItem(mRoot + mSpatialMap + "AR Session (Sparse SpatialMap Build Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Dense SpatialMap Build Preset)", true)]
        [MenuItem(mRoot + mSpatialMap + "AR Session (Dense SpatialMap Build Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Motion Tracking Preset)", true)]
        [MenuItem(mRoot + mMotionTracking + "AR Session (Motion Tracking Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Surface Tracking Preset)", true)]
        [MenuItem(mRoot + mSurfaceTracking + "AR Session (Surface Tracking Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Image Tracking Preset)", true)]
        [MenuItem(mRoot + mImageTracking + "AR Session (Image Tracking Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Image Tracking with Motion Fusion Preset)", true)]
        [MenuItem(mRoot + mImageTracking + "AR Session (Image Tracking with Motion Fusion Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (CRS Preset)", true)]
        [MenuItem(mRoot + mImageTracking + "AR Session (CRS Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Object Tracking Preset)", true)]
        [MenuItem(mRoot + mObjectTracking + "AR Session (Object Tracking Preset)", true)]
        [MenuItem(mRoot + mSession + "AR Session (Object Tracking with Motion Fusion Preset)", true)]
        [MenuItem(mRoot + mObjectTracking + "AR Session (Object Tracking with Motion Fusion Preset)", true)]
        [MenuItem(mRoot + mOrigin + "Origin : XR Origin (EasyAR)", true)]
        [MenuItem(mRoot + mMotionTracking + "Origin : XR Origin (EasyAR)", true)]
        [MenuItem(mRoot + mSurfaceTracking + "Target : Surface Target", true)]
        [MenuItem(mRoot + mImageTracking + "Target : Image Target", true)]
        [MenuItem(mRoot + mObjectTracking + "Target : Object Target", true)]
        [MenuItem(mRoot + mSpatialMap + "Target : Sparse SpatialMap", true)]
        [MenuItem(mRoot + mVideo + "Video Recorder", true)]
        static bool MenuValidateRootObject() => !Selection.activeGameObject;

        [MenuItem(mRoot + mSourceSixDof + "Frame Source : Motion Tracker", true)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : Motion Tracker", true)]
        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARCore", true)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARCore", true)]
        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARCore ARFoundation", true)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARCore ARFoundation", true)]
        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARKit", true)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARKit", true)]
        [MenuItem(mRoot + mSourceSixDof + "Frame Source : ARKit ARFoundation", true)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : ARKit ARFoundation", true)]
        [MenuItem(mRoot + mSourceSixDof + "Frame Source : AREngine", true)]
        [MenuItem(mRoot + mMotionTracking + "Frame Source : AREngine", true)]
        [MenuItem(mRoot + mSourceFiveDofRotXZ + "Frame Source : Inertial Camera Device", true)]
        [MenuItem(mRoot + mSourceThreeDofRotOnly + "Frame Source : Three Dof Camera Device", true)]
        [MenuItem(mRoot + mSourceZeroDof + "Frame Source : Camera Device", true)]
        [MenuItem(mRoot + mSurfaceTracking + "Frame Source : Camera Device", true)]
        [MenuItem(mRoot + mImageTracking + "Frame Source : Camera Device", true)]
        [MenuItem(mRoot + mObjectTracking + "Frame Source : Camera Device", true)]
        [MenuItem(mRoot + mSourceZeroDof + "Frame Source : Camera Device (Editor Only)", true)]
        [MenuItem(mRoot + mHMD + "Frame Source : VisionOS ARKit", true)]
        [MenuItem(mRoot + mHMD + "Frame Source : VisionOS ARKit (keep it only)", true)]
        [MenuItem(mRoot + mHMD + "Frame Source : XREAL", true)]
        [MenuItem(mRoot + mHMD + "Frame Source : XREAL (keep it only)", true)]
        [MenuItem(mRoot + mSurfaceTracking + "Frame Filter : Surface Tracker", true)]
        [MenuItem(mRoot + mImageTracking + "Frame Filter : Image Tracker", true)]
        [MenuItem(mRoot + mImageTracking + "Frame Filter : Cloud Recognizer", true)]
        [MenuItem(mRoot + mObjectTracking + "Frame Filter : Object Tracker", true)]
        [MenuItem(mRoot + mSpatialMap + "Frame Filter : Sparse SpatialMap Builder", true)]
        [MenuItem(mRoot + mSpatialMap + "Frame Filter : Sparse SpatialMap Tracker", true)]
        [MenuItem(mRoot + mSpatialMap + "Frame Filter : Dense SpatialMap Builder", true)]
        [MenuItem(mRoot + mMega + "Frame Filter : Mega Tracker (for Mega Block)", true)]
        [MenuItem(mRoot + mMega + "Frame Filter : Mega Localizer (for Mega Block)", true)]
        [MenuItem(mRoot + mMega + "Frame Filter : Mega Tracker (for Mega Landmark)", true)]
        [MenuItem(mRoot + mMega + "Frame Filter : Mega Localizer (for Mega Landmark)", true)]
        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARCore > ARCore ARFoundation", true)]
        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARCore ARFoundation > ARCore", true)]
        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARKit > ARKit ARFoundation", true)]
        [MenuItem(mRoot + mUtil + "Sort Frame Source : ARKit ARFoundation > ARKit", true)]
        [MenuItem(mRoot + mUtil + "Sort Frame Source : System SLAM > Motion Tracker", true)]
        [MenuItem(mRoot + mUtil + "Sort Frame Source : Motion Tracker > System SLAM", true)]
        static bool MenuValidateSession() => ARSessionFactory.IsSession(Selection.activeGameObject);
    }
}
