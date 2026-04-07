//=============================================================================================================================
//
// EasyAR Sense 4.8.0.11837-e0e3282d9
// Copyright (c) 2015-2026 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
#if ENABLE_IL2CPP
using AOT;
#endif

namespace easyar
{
    internal static partial class Detail
    {
#if (UNITY_IOS || UNITY_VISIONOS) && !UNITY_EDITOR
        public const String BindingLibraryName = "__Internal";
#else
        public const String BindingLibraryName = "EasyAR";
#endif
    }

    public abstract class RefBase : IDisposable
    {
        internal IntPtr cdata_;
        internal Action<IntPtr> deleter_;
        internal delegate void Retainer(IntPtr This, out IntPtr Return);
        internal Retainer retainer_;

        internal RefBase(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer)
        {
            cdata_ = cdata;
            deleter_ = deleter;
            retainer_ = retainer;
        }

        internal IntPtr cdata
        {
            get
            {
                if (cdata_ == IntPtr.Zero) { throw new ObjectDisposedException(GetType().FullName); }
                return cdata_;
            }
        }

        ~RefBase()
        {
            if ((cdata_ != IntPtr.Zero) && (deleter_ != null))
            {
                deleter_(cdata_);
                cdata_ = IntPtr.Zero;
                deleter_ = null;
                retainer_ = null;
            }
        }

        public void Dispose()
        {
            if ((cdata_ != IntPtr.Zero) && (deleter_ != null))
            {
                deleter_(cdata_);
                cdata_ = IntPtr.Zero;
                deleter_ = null;
                retainer_ = null;
            }
            GC.SuppressFinalize(this);
        }

        protected abstract object CloneObject();
        public RefBase Clone()
        {
            return (RefBase)(CloneObject());
        }
    }

    internal static partial class Detail
    {
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_String_from_utf8(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_String_from_utf8_begin(IntPtr begin, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_String_begin(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_String_end(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_String_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_String__dtor(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_bufferDictionary(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_setBufferDictionary(IntPtr This, IntPtr bufferDictionary);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_objPath(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_setObjPath(IntPtr This, IntPtr objPath);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_name(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_setName(IntPtr This, IntPtr name);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_uid(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_setUid(IntPtr This, IntPtr uid);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_meta(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_setMeta(IntPtr This, IntPtr meta);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_ObjectTargetParameters_scale(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters_setScale(IntPtr This, float size);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTargetParameters__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ObjectTargetParameters__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_createFromParameters(IntPtr parameters, out OptionalOfObjectTarget Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_createFromObjectFile(IntPtr path, StorageType storageType, IntPtr name, IntPtr uid, IntPtr meta, float scale, out OptionalOfObjectTarget Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_ObjectTarget_scale(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_boundingBox(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ObjectTarget_setScale(IntPtr This, float scale);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ObjectTarget_runtimeID(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_uid(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_name(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_setName(IntPtr This, IntPtr name);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_meta(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget_setMeta(IntPtr This, IntPtr data);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTarget__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ObjectTarget__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castObjectTargetToTarget(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastTargetToObjectTarget(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTrackerResult_targetInstances(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTrackerResult_setTargetInstances(IntPtr This, IntPtr instances);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTrackerResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTrackerResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ObjectTrackerResult__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castObjectTrackerResultToFrameFilterResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastFrameFilterResultToObjectTrackerResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castObjectTrackerResultToTargetTrackerResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastTargetTrackerResultToObjectTrackerResult(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ObjectTracker_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_feedbackFrameSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ObjectTracker_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_outputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_setResultPostProcessing(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enablePersistentTargetInstance, [MarshalAs(UnmanagedType.I1)] bool enableMotionFusion);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ObjectTracker_setResultAsyncMode(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enableAsync);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ObjectTracker_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_loadTarget(IntPtr This, IntPtr target, IntPtr callbackScheduler, FunctorOfVoidFromTargetAndBool callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_unloadTarget(IntPtr This, IntPtr target, IntPtr callbackScheduler, FunctorOfVoidFromTargetAndBool callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_targets(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ObjectTracker_setSimultaneousNum(IntPtr This, int num);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ObjectTracker_simultaneousNum(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker_getSyncResult(IntPtr This, IntPtr motionInputData, out OptionalOfObjectTrackerResult Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ObjectTracker__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ObjectTracker__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreDeviceListDownloader__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreDeviceListDownloader_download(IntPtr This, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString onCompleted);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreDeviceListDownloader__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreDeviceListDownloader__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ARCoreDeviceListDownloader__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CalibrationDownloader__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CalibrationDownloader_download(IntPtr This, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString onCompleted);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CalibrationDownloader__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CalibrationDownloader__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CalibrationDownloader__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerBlockInstance_blockId(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerBlockInstance_name(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_CloudLocalizerBlockInstance_pose(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerBlockInstance__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerBlockInstance__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CloudLocalizerBlockInstance__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CloudLocalizerStatus easyar_CloudLocalizerResult_localizeStatus(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerResult_spotVersionId(IntPtr This, out OptionalOfString Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerResult_blockInstances(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerResult_extraInfo(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerResult_exceptionInfo(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfDouble easyar_CloudLocalizerResult_serverResponseDuration(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfDouble easyar_CloudLocalizerResult_serverCalculationDuration(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizerResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CloudLocalizerResult__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo_setAcceleration(IntPtr This, AccelerometerResult acce);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo_setGPSLocation(IntPtr This, LocationResult gps);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo_setProximityLocation(IntPtr This, ProximityLocationResult proximity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo_setBlockPrior(IntPtr This, IntPtr blockPrior);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo_setECompass(IntPtr This, double northHeading, double headingAccuracy);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DeviceAuxiliaryInfo__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_DeviceAuxiliaryInfo__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CloudLocalizer_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer_create(IntPtr server, IntPtr apiKey, IntPtr apiSecret, IntPtr appId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer_createWithToken(IntPtr server, IntPtr apiToken, IntPtr appId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer_updateToken(IntPtr This, IntPtr apiToken);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer_setMegaApiType(IntPtr This, MegaApiType type);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer_resolve(IntPtr This, IntPtr inputFrame, IntPtr message, IntPtr deviceAuxInfo, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromCloudLocalizerResult callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer_resolveLandmark(IntPtr This, IntPtr inputFrame, IntPtr message, IntPtr deviceAuxInfo, IntPtr spotVersionId, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromCloudLocalizerResult callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudLocalizer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CloudLocalizer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern MegaLandmarkFilterStatus easyar_MegaLandmarkFilterResult_status(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilterResult_spotVersionId(IntPtr This, out OptionalOfString Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilterResult_exceptionInfo(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilterResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilterResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MegaLandmarkFilterResult__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MegaLandmarkFilter_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter_create(IntPtr server, IntPtr apiKey, IntPtr apiSecret, IntPtr appId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter_createWithToken(IntPtr server, IntPtr apiToken, IntPtr appId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter_updateToken(IntPtr This, IntPtr apiToken);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter_filterByLocation(IntPtr This, LocationResult gps, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromMegaLandmarkFilterResult callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter_filterBySpotId(IntPtr This, IntPtr spotId, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromMegaLandmarkFilterResult callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaLandmarkFilter__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MegaLandmarkFilter__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerBlockInstance_appId(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerBlockInstance_blockId(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerBlockInstance_name(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_MegaTrackerBlockInstance_pose(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerBlockInstance__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerBlockInstance__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MegaTrackerBlockInstance__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerResult_instances(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MegaTrackerResult__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castMegaTrackerResultToFrameFilterResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastFrameFilterResultToMegaTrackerResult(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse_inputFrame(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfAccelerometerResult easyar_MegaTrackerLocalizationResponse_acceleration(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfLocationResult easyar_MegaTrackerLocalizationResponse_location(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern MegaTrackerLocalizationStatus easyar_MegaTrackerLocalizationResponse_status(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse_spotVersionId(IntPtr This, out OptionalOfString Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse_instances(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse_appId(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfDouble easyar_MegaTrackerLocalizationResponse_serverResponseDuration(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfDouble easyar_MegaTrackerLocalizationResponse_serverCalculationDuration(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse_errorMessage(IntPtr This, out OptionalOfString Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse_extraInfo(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTrackerLocalizationResponse__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MegaTrackerLocalizationResponse__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MegaTracker_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_inputFrameSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_accelerometerResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_locationResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_proximityLocationResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_MegaTracker_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_outputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_create(IntPtr server, IntPtr apiKey, IntPtr apiSecret, IntPtr appId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_createWithToken(IntPtr server, IntPtr apiToken, IntPtr appId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_updateToken(IntPtr This, IntPtr apiToken);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setMegaApiType(IntPtr This, MegaApiType type);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setSpotVersionId(IntPtr This, OptionalOfString spotVersionId);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_switchEndPoint(IntPtr This, IntPtr server, IntPtr apiKey, IntPtr apiSecret, IntPtr appId);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_switchEndPointWithToken(IntPtr This, IntPtr server, IntPtr apiToken, IntPtr appId);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_bindBlockPrior(IntPtr This, IntPtr blockPrior);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_unbindBlockPrior(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setRequestTimeParameters(IntPtr This, OptionalOfInt timeoutMilliseconds, int requestIntervalMilliseconds);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setEnableLocalization(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enableLocalization);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setEnableStatelessLocalization(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enableStatelessLocalization);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MegaTracker_setResultAsyncMode(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enableAsync);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setRequestMessage(IntPtr This, IntPtr message);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setLocalizationCallback(IntPtr This, IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromMegaTrackerLocalizationResponse callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_setCameraPositionOverride(IntPtr This, OptionalOfFloat x, OptionalOfFloat y, OptionalOfFloat z);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MegaTracker_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_reset(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker_getSyncResult(IntPtr This, IntPtr motionInputData, out OptionalOfMegaTrackerResult Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MegaTracker__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MegaTracker__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CloudRecognizationStatus easyar_CloudRecognizationResult_getStatus(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizationResult_getTarget(IntPtr This, out OptionalOfImageTarget Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizationResult_getUnknownErrorMessage(IntPtr This, out OptionalOfString Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizationResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizationResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CloudRecognizationResult__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CloudRecognizer_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer_create(IntPtr cloudRecognitionServiceServerAddress, IntPtr apiKey, IntPtr apiSecret, IntPtr cloudRecognitionServiceAppId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer_createWithToken(IntPtr cloudRecognitionServiceServerAddress, IntPtr apiToken, IntPtr cloudRecognitionServiceAppId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer_createByCloudSecret(IntPtr cloudRecognitionServiceServerAddress, IntPtr cloudRecognitionServiceSecret, IntPtr cloudRecognitionServiceAppId, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer_updateToken(IntPtr This, IntPtr apiToken);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer_resolve(IntPtr This, IntPtr inputFrame, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromCloudRecognizationResult callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CloudRecognizer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CloudRecognizer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Buffer_wrap(IntPtr ptr, int size, FunctorOfVoid deleter, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Buffer_create(int size, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Buffer_data(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_Buffer_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Buffer_memoryCopy(IntPtr src, IntPtr dest, int length);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Buffer_tryCopyFrom(IntPtr This, IntPtr src, int srcIndex, int index, int length);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Buffer_tryCopyTo(IntPtr This, int index, IntPtr dest, int destIndex, int length);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Buffer_partition(IntPtr This, int index, int length, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Buffer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Buffer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Buffer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferDictionary__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_BufferDictionary_count(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_BufferDictionary_contains(IntPtr This, IntPtr path);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferDictionary_tryGet(IntPtr This, IntPtr path, out OptionalOfBuffer Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferDictionary_set(IntPtr This, IntPtr path, IntPtr buffer);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_BufferDictionary_remove(IntPtr This, IntPtr path);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferDictionary_clear(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferDictionary__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferDictionary__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_BufferDictionary__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferPool__ctor(int block_size, int capacity, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_BufferPool_block_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_BufferPool_capacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_BufferPool_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferPool_tryAcquire(IntPtr This, out OptionalOfBuffer Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferPool__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BufferPool__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_BufferPool__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Image__ctor(IntPtr buffer, PixelFormat format, int width, int height, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Image_buffer(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern PixelFormat easyar_Image_format(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_Image_width(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_Image_height(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_Image_pixelWidth(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_Image_pixelHeight(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Image_create(IntPtr buffer, PixelFormat format, int width, int height, int pixelWidth, int pixelHeight, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Image__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Image__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Image__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BlockPriorResult_blockIds(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BlockPriorResult_mode(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BlockPriorResult_create(IntPtr blockIds, BlockPriorMode mode, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BlockPriorResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_BlockPriorResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_BlockPriorResult__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_DenseSpatialMap_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DenseSpatialMap_inputFrameSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_DenseSpatialMap_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DenseSpatialMap_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_DenseSpatialMap_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DenseSpatialMap_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DenseSpatialMap_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DenseSpatialMap_getMesh(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_DenseSpatialMap_updateSceneMesh(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool updateMeshAll);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DenseSpatialMap__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DenseSpatialMap__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_DenseSpatialMap__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_SceneMesh_getNumOfVertexAll(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_SceneMesh_getNumOfIndexAll(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh_getVerticesAll(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh_getNormalsAll(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh_getIndicesAll(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_SceneMesh_getNumOfVertexIncremental(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_SceneMesh_getNumOfIndexIncremental(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh_getVerticesIncremental(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh_getNormalsIncremental(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh_getIndicesIncremental(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh_getBlocksInfoIncremental(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_SceneMesh_getBlockDimensionInMeters(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SceneMesh__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SceneMesh__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Accelerometer__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Accelerometer_isAvailable(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Accelerometer_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Accelerometer_open(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Accelerometer_openWithSamplingPeriod(IntPtr This, int samplingPeriodMilliseconds);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Accelerometer_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfAccelerometerResult easyar_Accelerometer_getCurrentResult(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Accelerometer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Accelerometer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Accelerometer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ARCoreCameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ARCoreCameraDevice_isDeviceSupported();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ARCoreCameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice_setFocusMode(IntPtr This, ARCoreCameraDeviceFocusMode focusMode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ARCoreCameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_ARCoreCameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ARCoreCameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_ARCoreCameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_ARCoreCameraDevice_frameRateRangeLower(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_ARCoreCameraDevice_frameRateRangeUpper(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARCoreCameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ARCoreCameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ARKitCameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ARKitCameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice_setFocusMode(IntPtr This, ARKitCameraDeviceFocusMode focusMode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ARKitCameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_ARKitCameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ARKitCameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_ARKitCameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_ARKitCameraDevice_frameRate(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ARKitCameraDevice_supportedSizeCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_ARKitCameraDevice_supportedSize(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ARKitCameraDevice_setSize(IntPtr This, Vec2I size);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ARKitCameraDevice_supportedFrameRateCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_ARKitCameraDevice_supportedFrameRate(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ARKitCameraDevice_setFrameRate(IntPtr This, double frameRate);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ARKitCameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ARKitCameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensor__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_AttitudeSensor_isAvailable(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensor_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_AttitudeSensor_open(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_AttitudeSensor_openWithSamplingPeriod(IntPtr This, int samplingPeriodMilliseconds);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensor_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfAttitudeSensorResult easyar_AttitudeSensor_getCurrentResult(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensor__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensor__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_AttitudeSensor__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern AndroidCameraApiType easyar_CameraDevice_androidCameraApiType(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_setAndroidCameraApiType(IntPtr This, AndroidCameraApiType type);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_setStateChangedCallback(IntPtr This, IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromCameraState stateChangedCallback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_requestPermissions(IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromPermissionStatusAndString permissionCallback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraDevice_cameraCount();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_openWithIndex(IntPtr This, int cameraIndex);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_openWithSpecificType(IntPtr This, CameraDeviceType type);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_openWithPreferredType(IntPtr This, CameraDeviceType type);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraDevice_index(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_CameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_cameraParameters(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice_setCameraParameters(IntPtr This, IntPtr cameraParameters);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_CameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraDevice_supportedSizeCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_CameraDevice_supportedSize(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_setSize(IntPtr This, Vec2I size);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraDevice_supportedFrameRateRangeCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_CameraDevice_supportedFrameRateRangeLower(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_CameraDevice_supportedFrameRateRangeUpper(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraDevice_frameRateRange(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_setFrameRateRange(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_setFlashTorchMode(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool on);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_setFocusMode(IntPtr This, CameraDeviceFocusMode focusMode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraDevice_autoFocus(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern AndroidCameraApiType easyar_CameraDeviceSelector_getAndroidCameraApiType(CameraDevicePreference preference);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraDeviceSelector_createCameraDevice(CameraDevicePreference preference, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceFocusMode easyar_CameraDeviceSelector_getFocusMode(CameraDevicePreference preference);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Gyroscope__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Gyroscope_isAvailable(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Gyroscope_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Gyroscope_open(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Gyroscope_openWithSamplingPeriod(IntPtr This, int samplingPeriodMilliseconds);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Gyroscope_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfGyroscopeResult easyar_Gyroscope_getCurrentResult(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Gyroscope__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Gyroscope__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Gyroscope__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Magnetometer__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Magnetometer_isAvailable(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Magnetometer_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Magnetometer_open(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Magnetometer_openWithSamplingPeriod(IntPtr This, int samplingPeriodMilliseconds);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Magnetometer_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfMagnetometerResult easyar_Magnetometer_getCurrentResult(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Magnetometer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Magnetometer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Magnetometer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ThreeDofCameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ThreeDofCameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice_setFocusMode(IntPtr This, ThreeDofCameraDeviceFocusMode focusMode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_ThreeDofCameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ThreeDofCameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_ThreeDofCameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ThreeDofCameraDevice_supportedSizeCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_ThreeDofCameraDevice_supportedSize(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ThreeDofCameraDevice_setSize(IntPtr This, Vec2I size);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ThreeDofCameraDevice_open(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ThreeDofCameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_ThreeDofCameraDevice_frameRateRangeLower(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_ThreeDofCameraDevice_frameRateRangeUpper(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ThreeDofCameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ThreeDofCameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VisionOSARKitCameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_VisionOSARKitCameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice_requestPermissions(IntPtr This, IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromPermissionStatusAndString permissionCallback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice_getMotionInputData(IntPtr This, out OptionalOfMotionInputData Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VisionOSARKitCameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_VisionOSARKitCameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_VisionOSARKitCameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_VisionOSARKitCameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_VisionOSARKitCameraDevice_frameRateRangeLower(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_VisionOSARKitCameraDevice_frameRateRangeUpper(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VisionOSARKitCameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_VisionOSARKitCameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_XREALCameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfBool easyar_XREALCameraDevice_isDeviceSupported();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_XREALCameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice_getMotionInputData(IntPtr This, out OptionalOfMotionInputData Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_XREALCameraDevice_receivedFrameCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_XREALCameraDevice_open(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_XREALCameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_XREALCameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_XREALCameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_XREALCameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_XREALCameraDevice_frameRate(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_XREALCameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_XREALCameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_SurfaceTrackerResult_transform(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTrackerResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTrackerResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SurfaceTrackerResult__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castSurfaceTrackerResultToFrameFilterResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastFrameFilterResultToSurfaceTrackerResult(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SurfaceTracker_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker_inputFrameSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_SurfaceTracker_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker_outputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SurfaceTracker_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker_alignTargetToCameraImagePoint(IntPtr This, Vec2F cameraImagePoint);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SurfaceTracker__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SurfaceTracker__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_EventDumpRecorder_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_EventDumpRecorder_start(IntPtr This, IntPtr filePath, int initalScreenRotation);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_EventDumpRecorder_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_EventDumpRecorder__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_EventDumpRecorder__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_EventDumpRecorder__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MotionTrackerCameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern MotionTrackerCameraDeviceQualityLevel easyar_MotionTrackerCameraDevice_getQualityLevel();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MotionTrackerCameraDevice_setFrameRateType(IntPtr This, MotionTrackerCameraDeviceFPS fps);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MotionTrackerCameraDevice_setFocusMode(IntPtr This, MotionTrackerCameraDeviceFocusMode focusMode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MotionTrackerCameraDevice_setFrameResolutionType(IntPtr This, MotionTrackerCameraDeviceResolution resolution);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MotionTrackerCameraDevice_setTrackingMode(IntPtr This, MotionTrackerCameraDeviceTrackingMode trackingMode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_MotionTrackerCameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_MotionTrackerCameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_MotionTrackerCameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_MotionTrackerCameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_MotionTrackerCameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_MotionTrackerCameraDevice_frameRateRangeLower(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_MotionTrackerCameraDevice_frameRateRangeUpper(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice_hitTestAgainstPointCloud(IntPtr This, Vec2F cameraImagePoint, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice_hitTestAgainstHorizontalPlane(IntPtr This, Vec2F cameraImagePoint, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice_getLocalPointsCloud(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionTrackerCameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MotionTrackerCameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameRecorder_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InputFrameRecorder_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameRecorder_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameRecorder_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InputFrameRecorder_start(IntPtr This, IntPtr filePath, int initialScreenRotation);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameRecorder_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameRecorder__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameRecorder__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrameRecorder__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFramePlayer_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFramePlayer_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InputFramePlayer_start(IntPtr This, IntPtr filePath);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFramePlayer_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFramePlayer_pause(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InputFramePlayer_resume(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_InputFramePlayer_totalTime(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_InputFramePlayer_currentTime(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InputFramePlayer_initalScreenRotation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InputFramePlayer_isCompleted(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFramePlayer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFramePlayer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFramePlayer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFrameRecorder_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_accelerometerResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_gyroscopeResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_attitudeSensorResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_magnetometerResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_locationResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_proximityLocationResultSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_VideoInputFrameRecorder_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFrameRecorder_start(IntPtr This, IntPtr filePath, CameraDeviceType cameraDeviceType, int cameraOrientation, int initialScreenRotation, VideoInputFrameRecorderVideoCodec codec, int width, int height, double frameRate, IntPtr metadata);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder_setAutomaticCompletionCallback(IntPtr This, IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromVideoInputFrameRecorderCompletionReason automaticCompletionCallback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFrameRecorder__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_VideoInputFrameRecorder__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFramePlayer_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_accelerometerResultSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_gyroscopeResultSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_attitudeSensorResultSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_magnetometerResultSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_locationResultSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_proximityLocationResultSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFramePlayer_start(IntPtr This, IntPtr filePath);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_pause(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFramePlayer_resume(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfDouble easyar_VideoInputFramePlayer_totalTime(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_VideoInputFramePlayer_currentTime(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFramePlayer_isSeekable(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFramePlayer_seek(IntPtr This, double time);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_VideoInputFramePlayer_speed(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_setSpeed(IntPtr This, double @value);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_VideoInputFramePlayer_initalScreenRotation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer_metadata(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoInputFramePlayer_isCompleted(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoInputFramePlayer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_VideoInputFramePlayer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageHelper_decode(IntPtr buffer, out OptionalOfImage Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InertialCameraDevice_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InertialCameraDevice_bufferCapacity(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice_setBufferCapacity(IntPtr This, int capacity);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice_inputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice_setFocusMode(IntPtr This, InertialCameraDeviceFocusMode focusMode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_InertialCameraDevice_type(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InertialCameraDevice_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_InertialCameraDevice_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InertialCameraDevice_supportedSizeCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_InertialCameraDevice_supportedSize(IntPtr This, int index);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InertialCameraDevice_setSize(IntPtr This, Vec2I size);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InertialCameraDevice_open(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InertialCameraDevice_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice_reset(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_InertialCameraDevice_frameRateRangeLower(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_InertialCameraDevice_frameRateRangeUpper(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InertialCameraDevice__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InertialCameraDevice__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CallbackScheduler__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CallbackScheduler__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CallbackScheduler__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DelayedCallbackScheduler__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_DelayedCallbackScheduler_runOne(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DelayedCallbackScheduler__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_DelayedCallbackScheduler__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_DelayedCallbackScheduler__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castDelayedCallbackSchedulerToCallbackScheduler(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastCallbackSchedulerToDelayedCallbackScheduler(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImmediateCallbackScheduler_getDefault(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImmediateCallbackScheduler__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImmediateCallbackScheduler__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ImmediateCallbackScheduler__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castImmediateCallbackSchedulerToCallbackScheduler(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastCallbackSchedulerToImmediateCallbackScheduler(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_JniUtility_wrapByteArray(IntPtr bytes, [MarshalAs(UnmanagedType.I1)] bool readOnly, FunctorOfVoid deleter, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_JniUtility_wrapBuffer(IntPtr directBuffer, FunctorOfVoid deleter, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_JniUtility_getDirectBufferAddress(IntPtr directBuffer);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Log_logMessage(LogLevel level, IntPtr message);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Log_setLogFunc(FunctorOfVoidFromLogLevelAndString func);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Log_setLogFuncWithScheduler(IntPtr scheduler, FunctorOfVoidFromLogLevelAndString func);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Log_resetLogFunc();

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Storage_setAssetDirPath(IntPtr path);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_image(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_setImage(IntPtr This, IntPtr image);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_name(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_setName(IntPtr This, IntPtr name);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_uid(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_setUid(IntPtr This, IntPtr uid);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_meta(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_setMeta(IntPtr This, IntPtr meta);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_ImageTargetParameters_scale(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters_setScale(IntPtr This, float scale);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTargetParameters__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ImageTargetParameters__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_createFromParameters(IntPtr parameters, out OptionalOfImageTarget Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_createFromTargetFile(IntPtr path, StorageType storageType, out OptionalOfImageTarget Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_createFromTargetData(IntPtr buffer, out OptionalOfImageTarget Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ImageTarget_save(IntPtr This, IntPtr path);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_createFromImageFile(IntPtr path, StorageType storageType, IntPtr name, IntPtr uid, IntPtr meta, float scale, out OptionalOfImageTarget Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_ImageTarget_scale(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_ImageTarget_aspectRatio(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ImageTarget_setScale(IntPtr This, float scale);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_images(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ImageTarget_runtimeID(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_uid(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_name(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_setName(IntPtr This, IntPtr name);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_meta(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget_setMeta(IntPtr This, IntPtr data);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTarget__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ImageTarget__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castImageTargetToTarget(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastTargetToImageTarget(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerConfig__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImageTrackerMode easyar_ImageTrackerConfig_trackerMode(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerConfig_setTrackerMode(IntPtr This, ImageTrackerMode @value);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerConfig__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerConfig__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ImageTrackerConfig__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerResult_targetInstances(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerResult_setTargetInstances(IntPtr This, IntPtr instances);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTrackerResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ImageTrackerResult__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castImageTrackerResultToFrameFilterResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastFrameFilterResultToImageTrackerResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castImageTrackerResultToTargetTrackerResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastTargetTrackerResultToImageTrackerResult(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ImageTracker_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_feedbackFrameSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ImageTracker_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_outputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_createWithMode(ImageTrackerMode trackMode, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_createWithConfig(IntPtr config, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_setResultPostProcessing(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enablePersistentTargetInstance, [MarshalAs(UnmanagedType.I1)] bool enableMotionFusion);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ImageTracker_setResultAsyncMode(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enableAsync);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ImageTracker_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_loadTarget(IntPtr This, IntPtr target, IntPtr callbackScheduler, FunctorOfVoidFromTargetAndBool callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_unloadTarget(IntPtr This, IntPtr target, IntPtr callbackScheduler, FunctorOfVoidFromTargetAndBool callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_targets(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_ImageTracker_setSimultaneousNum(IntPtr This, int num);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ImageTracker_simultaneousNum(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker_getSyncResult(IntPtr This, IntPtr motionInputData, out OptionalOfImageTrackerResult Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ImageTracker__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ImageTracker__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Recorder_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Recorder_requestPermissions(IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromPermissionStatusAndString permissionCallback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Recorder_create(IntPtr config, IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromRecordStatusAndString statusCallback, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Recorder_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Recorder_updateFrame(IntPtr This, IntPtr texture, int width, int height);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Recorder_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Recorder__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Recorder__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Recorder__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setOutputFile(IntPtr This, IntPtr path);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_RecorderConfiguration_setProfile(IntPtr This, RecordProfile profile);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setVideoSize(IntPtr This, RecordVideoSize framesize);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setVideoBitrate(IntPtr This, int bitrate);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setChannelCount(IntPtr This, int count);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setAudioSampleRate(IntPtr This, int samplerate);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setAudioBitrate(IntPtr This, int bitrate);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setVideoOrientation(IntPtr This, RecordVideoOrientation mode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration_setZoomMode(IntPtr This, RecordZoomMode mode);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_RecorderConfiguration__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_RecorderConfiguration__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern MotionTrackingStatus easyar_SparseSpatialMapResult_getMotionTrackingStatus(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfMatrix44F easyar_SparseSpatialMapResult_getVioPose(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfMatrix44F easyar_SparseSpatialMapResult_getMapPose(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SparseSpatialMapResult_getLocalizationStatus(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapResult_getLocalizationMapID(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SparseSpatialMapResult__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castSparseSpatialMapResultToFrameFilterResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastFrameFilterResultToSparseSpatialMapResult(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_PlaneData__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern PlaneType easyar_PlaneData_getType(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_PlaneData_getPose(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_PlaneData_getExtentX(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_PlaneData_getExtentZ(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_PlaneData__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_PlaneData__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_PlaneData__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapConfig__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapConfig_setLocalizationMode(IntPtr This, LocalizationMode @value);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern LocalizationMode easyar_SparseSpatialMapConfig_getLocalizationMode(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapConfig__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapConfig__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SparseSpatialMapConfig__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SparseSpatialMap_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_inputFrameSink(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_SparseSpatialMap_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_outputFrameSource(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_setResultPoseType(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enableStabilization);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SparseSpatialMap_setResultAsyncMode(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool enableAsync);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SparseSpatialMap_start(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_getPointCloudBuffer(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_getMapPlanes(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_hitTestAgainstPointCloud(IntPtr This, Vec2F cameraImagePoint, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_hitTestAgainstPlanes(IntPtr This, Vec2F cameraImagePoint, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_getMapVersion(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_unloadMap(IntPtr This, IntPtr mapID, IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromBool resultCallBack);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_setConfig(IntPtr This, IntPtr config);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_getConfig(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SparseSpatialMap_startLocalization(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_stopLocalization(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap_getSyncResult(IntPtr This, IntPtr motionInputData, out OptionalOfSparseSpatialMapResult Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMap__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SparseSpatialMap__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_SparseSpatialMapManager_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapManager_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapManager_host(IntPtr This, IntPtr mapBuilder, IntPtr apiKey, IntPtr apiSecret, IntPtr sparseSpatialMapAppId, IntPtr name, OptionalOfImage preview, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromBoolAndStringAndString onCompleted);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapManager_load(IntPtr This, IntPtr mapTracker, IntPtr serverMapId, IntPtr apiKey, IntPtr apiSecret, IntPtr sparseSpatialMapAppId, OptionalOfInt timeoutMilliseconds, IntPtr callbackScheduler, FunctorOfVoidFromBoolAndString onCompleted);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapManager_clear(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapManager__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SparseSpatialMapManager__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SparseSpatialMapManager__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_Engine_schemaHash();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Engine_initialize(IntPtr licenseKey);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Engine_onPause();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Engine_onResume();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ValidationState easyar_Engine_validationState();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Engine_errorMessage(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Engine_versionString(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Engine_name(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Engine_variant(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_Engine_isLicenseKeyMatched(IntPtr licenseKey, IntPtr packageName, IntPtr variant, EngineOperatingSystem operatingSystem);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Engine_enableEyewearSupport();

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer__ctor(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoPlayer_isAvailable();
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer_setVideoType(IntPtr This, VideoType videoType);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer_setRenderTexture(IntPtr This, IntPtr texture);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer_open(IntPtr This, IntPtr path, StorageType storageType, IntPtr callbackScheduler, OptionalOfFunctorOfVoidFromVideoStatus callback);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer_close(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoPlayer_play(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer_stop(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer_pause(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoPlayer_isRenderTextureAvailable(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer_updateFrame(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_VideoPlayer_duration(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_VideoPlayer_currentPosition(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoPlayer_seek(IntPtr This, int position);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_VideoPlayer_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_VideoPlayer_volume(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_VideoPlayer_setVolume(IntPtr This, float volume);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_VideoPlayer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_VideoPlayer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraParameters__ctor(Vec2I imageSize, Vec2F focalLength, Vec2F principalPoint, CameraDeviceType cameraDeviceType, int cameraOrientation, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2I easyar_CameraParameters_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2F easyar_CameraParameters_focalLength(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2F easyar_CameraParameters_principalPoint(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraModelType easyar_CameraParameters_cameraModelType(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraDeviceType easyar_CameraParameters_cameraDeviceType(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraParameters_cameraOrientation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraParameters_createWithDefaultIntrinsics(Vec2I imageSize, CameraDeviceType cameraDeviceType, int cameraOrientation, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraParameters_tryCreateWithCustomIntrinsics(Vec2I imageSize, IntPtr cameraParamList, CameraModelType cameraModel, CameraDeviceType cameraDeviceType, int cameraOrientation, out OptionalOfCameraParameters Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraParameters_getResized(IntPtr This, Vec2I imageSize, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_CameraParameters_imageOrientation(IntPtr This, int screenRotation);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraParameters_imageHorizontalFlip(IntPtr This, [MarshalAs(UnmanagedType.I1)] bool manualHorizontalFlip);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_CameraParameters_projection(IntPtr This, float nearPlane, float farPlane, float viewportAspectRatio, int screenRotation, [MarshalAs(UnmanagedType.I1)] bool combiningFlip, [MarshalAs(UnmanagedType.I1)] bool manualHorizontalFlip);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_CameraParameters_imageProjection(IntPtr This, float viewportAspectRatio, int screenRotation, [MarshalAs(UnmanagedType.I1)] bool combiningFlip, [MarshalAs(UnmanagedType.I1)] bool manualHorizontalFlip);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2F easyar_CameraParameters_screenCoordinatesFromImageCoordinates(IntPtr This, float viewportAspectRatio, int screenRotation, [MarshalAs(UnmanagedType.I1)] bool combiningFlip, [MarshalAs(UnmanagedType.I1)] bool manualHorizontalFlip, Vec2F imageCoordinates);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec2F easyar_CameraParameters_imageCoordinatesFromScreenCoordinates(IntPtr This, float viewportAspectRatio, int screenRotation, [MarshalAs(UnmanagedType.I1)] bool combiningFlip, [MarshalAs(UnmanagedType.I1)] bool manualHorizontalFlip, Vec2F screenCoordinates);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_CameraParameters_equalsTo(IntPtr This, IntPtr other);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraParameters__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_CameraParameters__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_CameraParameters__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSink_handle(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SignalSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSource_setHandler(IntPtr This, OptionalOfFunctorOfVoid handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_SignalSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_SignalSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSink_handle(IntPtr This, AccelerometerResult inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_AccelerometerResultSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromAccelerometerResult handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AccelerometerResultSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_AccelerometerResultSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSink_handle(IntPtr This, GyroscopeResult inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_GyroscopeResultSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromGyroscopeResult handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_GyroscopeResultSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_GyroscopeResultSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSink_handle(IntPtr This, AttitudeSensorResult inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_AttitudeSensorResultSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromAttitudeSensorResult handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_AttitudeSensorResultSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_AttitudeSensorResultSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSink_handle(IntPtr This, MagnetometerResult inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MagnetometerResultSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromMagnetometerResult handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MagnetometerResultSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MagnetometerResultSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSink_handle(IntPtr This, LocationResult inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_LocationResultSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromLocationResult handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_LocationResultSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_LocationResultSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSink_handle(IntPtr This, ProximityLocationResult inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ProximityLocationResultSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromProximityLocationResult handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ProximityLocationResultSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ProximityLocationResultSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSink_handle(IntPtr This, IntPtr inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrameSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromInputFrame handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrameSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSink_handle(IntPtr This, IntPtr inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_OutputFrameSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromOutputFrame handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_OutputFrameSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSink_handle(IntPtr This, IntPtr inputData);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSink__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSink__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_FeedbackFrameSink__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSource_setHandler(IntPtr This, OptionalOfFunctorOfVoidFromFeedbackFrame handler);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSource_connect(IntPtr This, IntPtr sink);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSource_disconnect(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSource__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameSource__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_FeedbackFrameSource__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameFork_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameFork_output(IntPtr This, int index, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InputFrameFork_outputCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameFork_create(int outputCount, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameFork__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameFork__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrameFork__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameFork_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameFork_output(IntPtr This, int index, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_OutputFrameFork_outputCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameFork_create(int outputCount, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameFork__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameFork__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_OutputFrameFork__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameJoin_input(IntPtr This, int index, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameJoin_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_OutputFrameJoin_inputCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameJoin_create(int inputCount, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameJoin_createWithJoiner(int inputCount, FunctorOfOutputFrameFromListOfOutputFrame joiner, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameJoin__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameJoin__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_OutputFrameJoin__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameFork_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameFork_output(IntPtr This, int index, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_FeedbackFrameFork_outputCount(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameFork_create(int outputCount, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameFork__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrameFork__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_FeedbackFrameFork__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameThrottler_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InputFrameThrottler_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameThrottler_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameThrottler_signalInput(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameThrottler_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameThrottler__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameThrottler__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrameThrottler__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_OutputFrameBuffer_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer_signalOutput(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer_peek(IntPtr This, out OptionalOfOutputFrame Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer_pause(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer_resume(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrameBuffer__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_OutputFrameBuffer__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToOutputFrameAdapter_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToOutputFrameAdapter_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToOutputFrameAdapter_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToOutputFrameAdapter__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToOutputFrameAdapter__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrameToOutputFrameAdapter__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToFeedbackFrameAdapter_input(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InputFrameToFeedbackFrameAdapter_bufferRequirement(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToFeedbackFrameAdapter_sideInput(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToFeedbackFrameAdapter_output(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToFeedbackFrameAdapter_create(out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToFeedbackFrameAdapter__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrameToFeedbackFrameAdapter__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrameToFeedbackFrameAdapter__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionInputData_tryCreateSixDof(double timestamp, Vec3F position, Vec4F rotation, MotionTrackingStatus tracking_status, out OptionalOfMotionInputData Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionInputData_tryCreateFiveDofRotXZ(double timestamp, Vec3F position, Vec4F rotation, out OptionalOfMotionInputData Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionInputData_tryCreateThreeDofRotOnly(double timestamp, Vec4F rotation, out OptionalOfMotionInputData Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_MotionInputData_timestamp(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraTransformType easyar_MotionInputData_transformType(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_MotionInputData_transform(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern MotionTrackingStatus easyar_MotionInputData_trackingStatus(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionInputData__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_MotionInputData__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_MotionInputData__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_InputFrame_index(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame_image(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InputFrame_hasCameraParameters(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame_cameraParameters(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InputFrame_hasTemporalInformation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double easyar_InputFrame_timestamp(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool easyar_InputFrame_hasSpatialInformation(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_InputFrame_cameraTransform(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraTransformType easyar_InputFrame_cameraTransformType(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern MotionTrackingStatus easyar_InputFrame_trackingStatus(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame_motion(IntPtr This, out OptionalOfMotionInputData Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame_tryCreate(IntPtr image, IntPtr cameraParameters, double timestamp, Matrix44F cameraTransform, CameraTransformType cameraTransformType, MotionTrackingStatus trackingStatus, out OptionalOfInputFrame Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame_createWithImageAndCameraParametersAndTemporal(IntPtr image, IntPtr cameraParameters, double timestamp, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame_createWithImageAndCameraParameters(IntPtr image, IntPtr cameraParameters, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame_createWithImage(IntPtr image, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_InputFrame__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_InputFrame__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FrameFilterResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FrameFilterResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_FrameFilterResult__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrame__ctor(IntPtr inputFrame, IntPtr results, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_OutputFrame_index(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrame_inputFrame(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrame_results(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrame__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_OutputFrame__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_OutputFrame__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrame__ctor(IntPtr inputFrame, OptionalOfOutputFrame previousOutputFrame, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrame_inputFrame(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrame_previousOutputFrame(IntPtr This, out OptionalOfOutputFrame Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrame__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_FeedbackFrame__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_FeedbackFrame__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfMatrix44F easyar_PoseUtility_createPoseWithHardwareOffset(Vec3F position, Vec4F rotation, OptionalOfVec3F offsetPosition, OptionalOfVec4F offsetRotation);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_PoseUtility__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_PoseUtility__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_PoseUtility__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_Target_runtimeID(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Target_uid(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Target_name(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Target_setName(IntPtr This, IntPtr name);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Target_meta(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Target_setMeta(IntPtr This, IntPtr data);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Target__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_Target__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_Target__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern TargetStatus easyar_TargetInstance_status(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TargetInstance_target(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix44F easyar_TargetInstance_pose(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TargetInstance__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TargetInstance__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_TargetInstance__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TargetTrackerResult_targetInstances(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TargetTrackerResult_setTargetInstances(IntPtr This, IntPtr instances);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TargetTrackerResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TargetTrackerResult__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_TargetTrackerResult__typeName(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_castTargetTrackerResultToFrameFilterResult(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_tryCastFrameFilterResultToTargetTrackerResult(IntPtr This, out IntPtr Return);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_TextureId_getInt(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_TextureId_getPointer(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TextureId_fromInt(int @value, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TextureId_fromPointer(IntPtr ptr, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TextureId__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_TextureId__retain(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_TextureId__typeName(IntPtr This);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfVec3F__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfVec3F__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfVec3F_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfVec3F_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vec3F easyar_ListOfVec3F_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfTargetInstance__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfTargetInstance__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfTargetInstance_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfTargetInstance_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfTargetInstance_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfFloat__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfFloat__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfFloat_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfFloat_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float easyar_ListOfFloat_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfOptionalOfFrameFilterResult__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfOptionalOfFrameFilterResult__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfOptionalOfFrameFilterResult_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfOptionalOfFrameFilterResult_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OptionalOfFrameFilterResult easyar_ListOfOptionalOfFrameFilterResult_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfTarget__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfTarget__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfTarget_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfTarget_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfTarget_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfCloudLocalizerBlockInstance__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfCloudLocalizerBlockInstance__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfCloudLocalizerBlockInstance_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfCloudLocalizerBlockInstance_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfCloudLocalizerBlockInstance_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfString__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfString__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfString_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfString_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfString_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfMegaTrackerBlockInstance__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfMegaTrackerBlockInstance__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfMegaTrackerBlockInstance_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfMegaTrackerBlockInstance_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfMegaTrackerBlockInstance_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfImage__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfImage__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfImage_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfImage_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfImage_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfBlockInfo__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfBlockInfo__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfBlockInfo_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfBlockInfo_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern BlockInfo easyar_ListOfBlockInfo_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfPlaneData__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfPlaneData__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfPlaneData_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfPlaneData_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfPlaneData_at(IntPtr This, int index);

        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfOutputFrame__ctor(IntPtr begin, IntPtr end, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfOutputFrame__dtor(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void easyar_ListOfOutputFrame_copy(IntPtr This, out IntPtr Return);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int easyar_ListOfOutputFrame_size(IntPtr This);
        [DllImport(BindingLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr easyar_ListOfOutputFrame_at(IntPtr This, int index);

        private static Dictionary<String, Func<IntPtr, RefBase>> TypeNameToConstructor = new Dictionary<String, Func<IntPtr, RefBase>>
        {
            { "ObjectTargetParameters", cdata => new ObjectTargetParameters(cdata, easyar_ObjectTargetParameters__dtor, easyar_ObjectTargetParameters__retain) },
            { "ObjectTarget", cdata => new ObjectTarget(cdata, easyar_ObjectTarget__dtor, easyar_ObjectTarget__retain) },
            { "ObjectTrackerResult", cdata => new ObjectTrackerResult(cdata, easyar_ObjectTrackerResult__dtor, easyar_ObjectTrackerResult__retain) },
            { "ObjectTracker", cdata => new ObjectTracker(cdata, easyar_ObjectTracker__dtor, easyar_ObjectTracker__retain) },
            { "ARCoreDeviceListDownloader", cdata => new ARCoreDeviceListDownloader(cdata, easyar_ARCoreDeviceListDownloader__dtor, easyar_ARCoreDeviceListDownloader__retain) },
            { "CalibrationDownloader", cdata => new CalibrationDownloader(cdata, easyar_CalibrationDownloader__dtor, easyar_CalibrationDownloader__retain) },
            { "CloudLocalizerBlockInstance", cdata => new CloudLocalizerBlockInstance(cdata, easyar_CloudLocalizerBlockInstance__dtor, easyar_CloudLocalizerBlockInstance__retain) },
            { "CloudLocalizerResult", cdata => new CloudLocalizerResult(cdata, easyar_CloudLocalizerResult__dtor, easyar_CloudLocalizerResult__retain) },
            { "DeviceAuxiliaryInfo", cdata => new DeviceAuxiliaryInfo(cdata, easyar_DeviceAuxiliaryInfo__dtor, easyar_DeviceAuxiliaryInfo__retain) },
            { "CloudLocalizer", cdata => new CloudLocalizer(cdata, easyar_CloudLocalizer__dtor, easyar_CloudLocalizer__retain) },
            { "MegaLandmarkFilterResult", cdata => new MegaLandmarkFilterResult(cdata, easyar_MegaLandmarkFilterResult__dtor, easyar_MegaLandmarkFilterResult__retain) },
            { "MegaLandmarkFilter", cdata => new MegaLandmarkFilter(cdata, easyar_MegaLandmarkFilter__dtor, easyar_MegaLandmarkFilter__retain) },
            { "MegaTrackerBlockInstance", cdata => new MegaTrackerBlockInstance(cdata, easyar_MegaTrackerBlockInstance__dtor, easyar_MegaTrackerBlockInstance__retain) },
            { "MegaTrackerResult", cdata => new MegaTrackerResult(cdata, easyar_MegaTrackerResult__dtor, easyar_MegaTrackerResult__retain) },
            { "MegaTrackerLocalizationResponse", cdata => new MegaTrackerLocalizationResponse(cdata, easyar_MegaTrackerLocalizationResponse__dtor, easyar_MegaTrackerLocalizationResponse__retain) },
            { "MegaTracker", cdata => new MegaTracker(cdata, easyar_MegaTracker__dtor, easyar_MegaTracker__retain) },
            { "CloudRecognizationResult", cdata => new CloudRecognizationResult(cdata, easyar_CloudRecognizationResult__dtor, easyar_CloudRecognizationResult__retain) },
            { "CloudRecognizer", cdata => new CloudRecognizer(cdata, easyar_CloudRecognizer__dtor, easyar_CloudRecognizer__retain) },
            { "Buffer", cdata => new Buffer(cdata, easyar_Buffer__dtor, easyar_Buffer__retain) },
            { "BufferDictionary", cdata => new BufferDictionary(cdata, easyar_BufferDictionary__dtor, easyar_BufferDictionary__retain) },
            { "BufferPool", cdata => new BufferPool(cdata, easyar_BufferPool__dtor, easyar_BufferPool__retain) },
            { "Image", cdata => new Image(cdata, easyar_Image__dtor, easyar_Image__retain) },
            { "BlockPriorResult", cdata => new BlockPriorResult(cdata, easyar_BlockPriorResult__dtor, easyar_BlockPriorResult__retain) },
            { "DenseSpatialMap", cdata => new DenseSpatialMap(cdata, easyar_DenseSpatialMap__dtor, easyar_DenseSpatialMap__retain) },
            { "SceneMesh", cdata => new SceneMesh(cdata, easyar_SceneMesh__dtor, easyar_SceneMesh__retain) },
            { "Accelerometer", cdata => new Accelerometer(cdata, easyar_Accelerometer__dtor, easyar_Accelerometer__retain) },
            { "ARCoreCameraDevice", cdata => new ARCoreCameraDevice(cdata, easyar_ARCoreCameraDevice__dtor, easyar_ARCoreCameraDevice__retain) },
            { "ARKitCameraDevice", cdata => new ARKitCameraDevice(cdata, easyar_ARKitCameraDevice__dtor, easyar_ARKitCameraDevice__retain) },
            { "AttitudeSensor", cdata => new AttitudeSensor(cdata, easyar_AttitudeSensor__dtor, easyar_AttitudeSensor__retain) },
            { "CameraDevice", cdata => new CameraDevice(cdata, easyar_CameraDevice__dtor, easyar_CameraDevice__retain) },
            { "Gyroscope", cdata => new Gyroscope(cdata, easyar_Gyroscope__dtor, easyar_Gyroscope__retain) },
            { "Magnetometer", cdata => new Magnetometer(cdata, easyar_Magnetometer__dtor, easyar_Magnetometer__retain) },
            { "ThreeDofCameraDevice", cdata => new ThreeDofCameraDevice(cdata, easyar_ThreeDofCameraDevice__dtor, easyar_ThreeDofCameraDevice__retain) },
            { "VisionOSARKitCameraDevice", cdata => new VisionOSARKitCameraDevice(cdata, easyar_VisionOSARKitCameraDevice__dtor, easyar_VisionOSARKitCameraDevice__retain) },
            { "XREALCameraDevice", cdata => new XREALCameraDevice(cdata, easyar_XREALCameraDevice__dtor, easyar_XREALCameraDevice__retain) },
            { "SurfaceTrackerResult", cdata => new SurfaceTrackerResult(cdata, easyar_SurfaceTrackerResult__dtor, easyar_SurfaceTrackerResult__retain) },
            { "SurfaceTracker", cdata => new SurfaceTracker(cdata, easyar_SurfaceTracker__dtor, easyar_SurfaceTracker__retain) },
            { "EventDumpRecorder", cdata => new EventDumpRecorder(cdata, easyar_EventDumpRecorder__dtor, easyar_EventDumpRecorder__retain) },
            { "MotionTrackerCameraDevice", cdata => new MotionTrackerCameraDevice(cdata, easyar_MotionTrackerCameraDevice__dtor, easyar_MotionTrackerCameraDevice__retain) },
            { "InputFrameRecorder", cdata => new InputFrameRecorder(cdata, easyar_InputFrameRecorder__dtor, easyar_InputFrameRecorder__retain) },
            { "InputFramePlayer", cdata => new InputFramePlayer(cdata, easyar_InputFramePlayer__dtor, easyar_InputFramePlayer__retain) },
            { "VideoInputFrameRecorder", cdata => new VideoInputFrameRecorder(cdata, easyar_VideoInputFrameRecorder__dtor, easyar_VideoInputFrameRecorder__retain) },
            { "VideoInputFramePlayer", cdata => new VideoInputFramePlayer(cdata, easyar_VideoInputFramePlayer__dtor, easyar_VideoInputFramePlayer__retain) },
            { "InertialCameraDevice", cdata => new InertialCameraDevice(cdata, easyar_InertialCameraDevice__dtor, easyar_InertialCameraDevice__retain) },
            { "CallbackScheduler", cdata => new CallbackScheduler(cdata, easyar_CallbackScheduler__dtor, easyar_CallbackScheduler__retain) },
            { "DelayedCallbackScheduler", cdata => new DelayedCallbackScheduler(cdata, easyar_DelayedCallbackScheduler__dtor, easyar_DelayedCallbackScheduler__retain) },
            { "ImmediateCallbackScheduler", cdata => new ImmediateCallbackScheduler(cdata, easyar_ImmediateCallbackScheduler__dtor, easyar_ImmediateCallbackScheduler__retain) },
            { "ImageTargetParameters", cdata => new ImageTargetParameters(cdata, easyar_ImageTargetParameters__dtor, easyar_ImageTargetParameters__retain) },
            { "ImageTarget", cdata => new ImageTarget(cdata, easyar_ImageTarget__dtor, easyar_ImageTarget__retain) },
            { "ImageTrackerConfig", cdata => new ImageTrackerConfig(cdata, easyar_ImageTrackerConfig__dtor, easyar_ImageTrackerConfig__retain) },
            { "ImageTrackerResult", cdata => new ImageTrackerResult(cdata, easyar_ImageTrackerResult__dtor, easyar_ImageTrackerResult__retain) },
            { "ImageTracker", cdata => new ImageTracker(cdata, easyar_ImageTracker__dtor, easyar_ImageTracker__retain) },
            { "Recorder", cdata => new Recorder(cdata, easyar_Recorder__dtor, easyar_Recorder__retain) },
            { "RecorderConfiguration", cdata => new RecorderConfiguration(cdata, easyar_RecorderConfiguration__dtor, easyar_RecorderConfiguration__retain) },
            { "SparseSpatialMapResult", cdata => new SparseSpatialMapResult(cdata, easyar_SparseSpatialMapResult__dtor, easyar_SparseSpatialMapResult__retain) },
            { "PlaneData", cdata => new PlaneData(cdata, easyar_PlaneData__dtor, easyar_PlaneData__retain) },
            { "SparseSpatialMapConfig", cdata => new SparseSpatialMapConfig(cdata, easyar_SparseSpatialMapConfig__dtor, easyar_SparseSpatialMapConfig__retain) },
            { "SparseSpatialMap", cdata => new SparseSpatialMap(cdata, easyar_SparseSpatialMap__dtor, easyar_SparseSpatialMap__retain) },
            { "SparseSpatialMapManager", cdata => new SparseSpatialMapManager(cdata, easyar_SparseSpatialMapManager__dtor, easyar_SparseSpatialMapManager__retain) },
            { "VideoPlayer", cdata => new VideoPlayer(cdata, easyar_VideoPlayer__dtor, easyar_VideoPlayer__retain) },
            { "CameraParameters", cdata => new CameraParameters(cdata, easyar_CameraParameters__dtor, easyar_CameraParameters__retain) },
            { "SignalSink", cdata => new SignalSink(cdata, easyar_SignalSink__dtor, easyar_SignalSink__retain) },
            { "SignalSource", cdata => new SignalSource(cdata, easyar_SignalSource__dtor, easyar_SignalSource__retain) },
            { "AccelerometerResultSink", cdata => new AccelerometerResultSink(cdata, easyar_AccelerometerResultSink__dtor, easyar_AccelerometerResultSink__retain) },
            { "AccelerometerResultSource", cdata => new AccelerometerResultSource(cdata, easyar_AccelerometerResultSource__dtor, easyar_AccelerometerResultSource__retain) },
            { "GyroscopeResultSink", cdata => new GyroscopeResultSink(cdata, easyar_GyroscopeResultSink__dtor, easyar_GyroscopeResultSink__retain) },
            { "GyroscopeResultSource", cdata => new GyroscopeResultSource(cdata, easyar_GyroscopeResultSource__dtor, easyar_GyroscopeResultSource__retain) },
            { "AttitudeSensorResultSink", cdata => new AttitudeSensorResultSink(cdata, easyar_AttitudeSensorResultSink__dtor, easyar_AttitudeSensorResultSink__retain) },
            { "AttitudeSensorResultSource", cdata => new AttitudeSensorResultSource(cdata, easyar_AttitudeSensorResultSource__dtor, easyar_AttitudeSensorResultSource__retain) },
            { "MagnetometerResultSink", cdata => new MagnetometerResultSink(cdata, easyar_MagnetometerResultSink__dtor, easyar_MagnetometerResultSink__retain) },
            { "MagnetometerResultSource", cdata => new MagnetometerResultSource(cdata, easyar_MagnetometerResultSource__dtor, easyar_MagnetometerResultSource__retain) },
            { "LocationResultSink", cdata => new LocationResultSink(cdata, easyar_LocationResultSink__dtor, easyar_LocationResultSink__retain) },
            { "LocationResultSource", cdata => new LocationResultSource(cdata, easyar_LocationResultSource__dtor, easyar_LocationResultSource__retain) },
            { "ProximityLocationResultSink", cdata => new ProximityLocationResultSink(cdata, easyar_ProximityLocationResultSink__dtor, easyar_ProximityLocationResultSink__retain) },
            { "ProximityLocationResultSource", cdata => new ProximityLocationResultSource(cdata, easyar_ProximityLocationResultSource__dtor, easyar_ProximityLocationResultSource__retain) },
            { "InputFrameSink", cdata => new InputFrameSink(cdata, easyar_InputFrameSink__dtor, easyar_InputFrameSink__retain) },
            { "InputFrameSource", cdata => new InputFrameSource(cdata, easyar_InputFrameSource__dtor, easyar_InputFrameSource__retain) },
            { "OutputFrameSink", cdata => new OutputFrameSink(cdata, easyar_OutputFrameSink__dtor, easyar_OutputFrameSink__retain) },
            { "OutputFrameSource", cdata => new OutputFrameSource(cdata, easyar_OutputFrameSource__dtor, easyar_OutputFrameSource__retain) },
            { "FeedbackFrameSink", cdata => new FeedbackFrameSink(cdata, easyar_FeedbackFrameSink__dtor, easyar_FeedbackFrameSink__retain) },
            { "FeedbackFrameSource", cdata => new FeedbackFrameSource(cdata, easyar_FeedbackFrameSource__dtor, easyar_FeedbackFrameSource__retain) },
            { "InputFrameFork", cdata => new InputFrameFork(cdata, easyar_InputFrameFork__dtor, easyar_InputFrameFork__retain) },
            { "OutputFrameFork", cdata => new OutputFrameFork(cdata, easyar_OutputFrameFork__dtor, easyar_OutputFrameFork__retain) },
            { "OutputFrameJoin", cdata => new OutputFrameJoin(cdata, easyar_OutputFrameJoin__dtor, easyar_OutputFrameJoin__retain) },
            { "FeedbackFrameFork", cdata => new FeedbackFrameFork(cdata, easyar_FeedbackFrameFork__dtor, easyar_FeedbackFrameFork__retain) },
            { "InputFrameThrottler", cdata => new InputFrameThrottler(cdata, easyar_InputFrameThrottler__dtor, easyar_InputFrameThrottler__retain) },
            { "OutputFrameBuffer", cdata => new OutputFrameBuffer(cdata, easyar_OutputFrameBuffer__dtor, easyar_OutputFrameBuffer__retain) },
            { "InputFrameToOutputFrameAdapter", cdata => new InputFrameToOutputFrameAdapter(cdata, easyar_InputFrameToOutputFrameAdapter__dtor, easyar_InputFrameToOutputFrameAdapter__retain) },
            { "InputFrameToFeedbackFrameAdapter", cdata => new InputFrameToFeedbackFrameAdapter(cdata, easyar_InputFrameToFeedbackFrameAdapter__dtor, easyar_InputFrameToFeedbackFrameAdapter__retain) },
            { "MotionInputData", cdata => new MotionInputData(cdata, easyar_MotionInputData__dtor, easyar_MotionInputData__retain) },
            { "InputFrame", cdata => new InputFrame(cdata, easyar_InputFrame__dtor, easyar_InputFrame__retain) },
            { "FrameFilterResult", cdata => new FrameFilterResult(cdata, easyar_FrameFilterResult__dtor, easyar_FrameFilterResult__retain) },
            { "OutputFrame", cdata => new OutputFrame(cdata, easyar_OutputFrame__dtor, easyar_OutputFrame__retain) },
            { "FeedbackFrame", cdata => new FeedbackFrame(cdata, easyar_FeedbackFrame__dtor, easyar_FeedbackFrame__retain) },
            { "PoseUtility", cdata => new PoseUtility(cdata, easyar_PoseUtility__dtor, easyar_PoseUtility__retain) },
            { "Target", cdata => new Target(cdata, easyar_Target__dtor, easyar_Target__retain) },
            { "TargetInstance", cdata => new TargetInstance(cdata, easyar_TargetInstance__dtor, easyar_TargetInstance__retain) },
            { "TargetTrackerResult", cdata => new TargetTrackerResult(cdata, easyar_TargetTrackerResult__dtor, easyar_TargetTrackerResult__retain) },
            { "TextureId", cdata => new TextureId(cdata, easyar_TextureId__dtor, easyar_TextureId__retain) },
        };

        public class AutoRelease : IDisposable
        {
            private List<Action> actions;

            public void Add(Action deleter)
            {
                if (actions == null) { actions = new List<Action>(); }
                actions.Add(deleter);
            }
            public T Add<T>(T p, Action<T> deleter)
            {
                if (p.Equals(default(T))) { return p; }
                if (actions == null) { actions = new List<Action>(); }
                actions.Add(() => deleter(p));
                return p;
            }

            public void Dispose()
            {
                if (actions != null)
                {
                    foreach (var a in actions)
                    {
                        a();
                    }
                    actions = null;
                }
            }
        }

        public static IntPtr String_to_c(AutoRelease ar, string s)
        {
            if (s == null) { throw new ArgumentNullException(); }
            var bytes = System.Text.Encoding.UTF8.GetBytes(s);
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + bytes.Length);
                var returnValue = IntPtr.Zero;
                easyar_String_from_utf8(beginPtr, endPtr, out returnValue);
                return ar.Add(returnValue, easyar_String__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static IntPtr String_to_c_inner(string s)
        {
            if (s == null) { throw new ArgumentNullException(); }
            var bytes = System.Text.Encoding.UTF8.GetBytes(s);
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + bytes.Length);
                var returnValue = IntPtr.Zero;
                easyar_String_from_utf8(beginPtr, endPtr, out returnValue);
                return returnValue;
            }
            finally
            {
                handle.Free();
            }
        }
        public static String String_from_c(AutoRelease ar, IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(ptr, easyar_String__dtor);
            IntPtr beginPtr = easyar_String_begin(ptr);
            IntPtr endPtr = easyar_String_end(ptr);
            var length = (int)(endPtr.ToInt64() - beginPtr.ToInt64());
            var bytes = new byte[length];
            Marshal.Copy(beginPtr, bytes, 0, length);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        public static String String_from_cstring(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) { throw new ArgumentNullException(); }
            var length = 0;
            while (true)
            {
                var b = Marshal.ReadByte(ptr, length);
                if (b == 0) { break; }
                length += 1;
            }
            var bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static T Object_from_c<T>(IntPtr ptr, Func<IntPtr, IntPtr> typeNameGetter)
        {
            if (ptr == IntPtr.Zero) { throw new ArgumentNullException(); }
            var typeName = String_from_cstring(typeNameGetter(ptr));
            if (!TypeNameToConstructor.ContainsKey(typeName)) { throw new InvalidOperationException("ConstructorNotExistForType"); }
            var ctor = TypeNameToConstructor[typeName];
            var o = ctor(ptr);
            return (T)(Object)(o);
        }
        public static TValue map<TKey, TValue>(this TKey v, Func<TKey, TValue> f)
        {
            return f(v);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfBuffer
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoid
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoid.FunctionDelegate))]
#endif
        public static void FunctorOfVoid_func(IntPtr state, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var f = (Action)((GCHandle)(state)).Target;
                    f();
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoid.DestroyDelegate))]
#endif
        public static void FunctorOfVoid_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoid FunctorOfVoid_to_c(Action f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoid { _state = (IntPtr)(s), _func = FunctorOfVoid_func, _destroy = FunctorOfVoid_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfObjectTarget
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        private static int ListOfVec3F_ElementSize = Marshal.SizeOf(typeof(Vec3F));
        public static IntPtr ListOfVec3F_to_c(AutoRelease ar, List<Vec3F> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfVec3F_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfVec3F__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfVec3F__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<Vec3F> ListOfVec3F_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfVec3F__dtor);
            var size = easyar_ListOfVec3F_size(l);
            var values = new List<Vec3F>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfVec3F_at(l, k);
                values.Add(v);
            }
            return values;
        }

        private static int ListOfTargetInstance_ElementSize = IntPtr.Size;
        public static IntPtr ListOfTargetInstance_to_c(AutoRelease ar, List<TargetInstance> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.cdata).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfTargetInstance_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfTargetInstance__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfTargetInstance__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<TargetInstance> ListOfTargetInstance_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfTargetInstance__dtor);
            var size = easyar_ListOfTargetInstance_size(l);
            var values = new List<TargetInstance>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfTargetInstance_at(l, k);
                easyar_TargetInstance__retain(v, out v);
                values.Add(Object_from_c<TargetInstance>(v, easyar_TargetInstance__typeName));
            }
            return values;
        }

        private static int ListOfFloat_ElementSize = Marshal.SizeOf(typeof(float));
        public static IntPtr ListOfFloat_to_c(AutoRelease ar, List<float> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfFloat_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfFloat__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfFloat__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<float> ListOfFloat_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfFloat__dtor);
            var size = easyar_ListOfFloat_size(l);
            var values = new List<float>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfFloat_at(l, k);
                values.Add(v);
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfCameraParameters
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfMotionInputData
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfInputFrame
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfOutputFrame
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        private static int ListOfOptionalOfFrameFilterResult_ElementSize = IntPtr.Size;
        public static IntPtr ListOfOptionalOfFrameFilterResult_to_c(AutoRelease ar, List<Optional<FrameFilterResult>> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.map(p => p.OnSome ? new OptionalOfFrameFilterResult { has_value = true, value = p.Value.cdata } : new OptionalOfFrameFilterResult { has_value = false, value = default(IntPtr) })).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfOptionalOfFrameFilterResult_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfOptionalOfFrameFilterResult__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfOptionalOfFrameFilterResult__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<Optional<FrameFilterResult>> ListOfOptionalOfFrameFilterResult_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfOptionalOfFrameFilterResult__dtor);
            var size = easyar_ListOfOptionalOfFrameFilterResult_size(l);
            var values = new List<Optional<FrameFilterResult>>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfOptionalOfFrameFilterResult_at(l, k);
                if (v.has_value) { easyar_FrameFilterResult__retain(v.value, out v.value); }
                values.Add(v.map(p => p.has_value ? Object_from_c<FrameFilterResult>(p.value, easyar_FrameFilterResult__typeName) : Optional<FrameFilterResult>.Empty));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFrameFilterResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromOutputFrame
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromOutputFrame value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromOutputFrame
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromOutputFrame.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromOutputFrame_func(IntPtr state, IntPtr arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_OutputFrame__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<OutputFrame>(varg0, easyar_OutputFrame__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var f = (Action<OutputFrame>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromOutputFrame.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromOutputFrame_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromOutputFrame FunctorOfVoidFromOutputFrame_to_c(Action<OutputFrame> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromOutputFrame { _state = (IntPtr)(s), _func = FunctorOfVoidFromOutputFrame_func, _destroy = FunctorOfVoidFromOutputFrame_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromTargetAndBool
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, [MarshalAs(UnmanagedType.I1)] bool arg1, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromTargetAndBool.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromTargetAndBool_func(IntPtr state, IntPtr arg0, [MarshalAs(UnmanagedType.I1)] bool arg1, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_Target__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<Target>(varg0, easyar_Target__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var sarg1 = arg1;
                    var f = (Action<Target, bool>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromTargetAndBool.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromTargetAndBool_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromTargetAndBool FunctorOfVoidFromTargetAndBool_to_c(Action<Target, bool> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromTargetAndBool { _state = (IntPtr)(s), _func = FunctorOfVoidFromTargetAndBool_func, _destroy = FunctorOfVoidFromTargetAndBool_destroy };
        }

        private static int ListOfTarget_ElementSize = IntPtr.Size;
        public static IntPtr ListOfTarget_to_c(AutoRelease ar, List<Target> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.cdata).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfTarget_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfTarget__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfTarget__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<Target> ListOfTarget_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfTarget__dtor);
            var size = easyar_ListOfTarget_size(l);
            var values = new List<Target>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfTarget_at(l, k);
                easyar_Target__retain(v, out v);
                values.Add(Object_from_c<Target>(v, easyar_Target__typeName));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfObjectTrackerResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfInt
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public int value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, ARCoreDeviceListDownloadStatus arg0, OptionalOfString arg1, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString_func(IntPtr state, ARCoreDeviceListDownloadStatus arg0, OptionalOfString arg1, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var varg1 = arg1;
                    if (varg1.has_value) { easyar_String_copy(varg1.value, out varg1.value); }
                    var sarg1 = varg1.map(p => p.has_value ? String_from_c(ar, p.value) : Optional<string>.Empty);
                    var f = (Action<ARCoreDeviceListDownloadStatus, Optional<string>>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString_to_c(Action<ARCoreDeviceListDownloadStatus, Optional<string>> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString { _state = (IntPtr)(s), _func = FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString_func, _destroy = FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfString
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, CalibrationDownloadStatus arg0, OptionalOfString arg1, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString_func(IntPtr state, CalibrationDownloadStatus arg0, OptionalOfString arg1, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var varg1 = arg1;
                    if (varg1.has_value) { easyar_String_copy(varg1.value, out varg1.value); }
                    var sarg1 = varg1.map(p => p.has_value ? String_from_c(ar, p.value) : Optional<string>.Empty);
                    var f = (Action<CalibrationDownloadStatus, Optional<string>>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString_to_c(Action<CalibrationDownloadStatus, Optional<string>> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString { _state = (IntPtr)(s), _func = FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString_func, _destroy = FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString_destroy };
        }

        private static int ListOfCloudLocalizerBlockInstance_ElementSize = IntPtr.Size;
        public static IntPtr ListOfCloudLocalizerBlockInstance_to_c(AutoRelease ar, List<CloudLocalizerBlockInstance> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.cdata).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfCloudLocalizerBlockInstance_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfCloudLocalizerBlockInstance__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfCloudLocalizerBlockInstance__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<CloudLocalizerBlockInstance> ListOfCloudLocalizerBlockInstance_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfCloudLocalizerBlockInstance__dtor);
            var size = easyar_ListOfCloudLocalizerBlockInstance_size(l);
            var values = new List<CloudLocalizerBlockInstance>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfCloudLocalizerBlockInstance_at(l, k);
                easyar_CloudLocalizerBlockInstance__retain(v, out v);
                values.Add(Object_from_c<CloudLocalizerBlockInstance>(v, easyar_CloudLocalizerBlockInstance__typeName));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfDouble
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public double value;
        }

        private static int ListOfString_ElementSize = IntPtr.Size;
        public static IntPtr ListOfString_to_c(AutoRelease ar, List<string> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => String_to_c(ar, e)).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfString_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfString__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfString__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<string> ListOfString_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfString__dtor);
            var size = easyar_ListOfString_size(l);
            var values = new List<string>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfString_at(l, k);
                easyar_String_copy(v, out v);
                values.Add(String_from_c(ar, v));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromCloudLocalizerResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCloudLocalizerResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromCloudLocalizerResult_func(IntPtr state, IntPtr arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_CloudLocalizerResult__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<CloudLocalizerResult>(varg0, easyar_CloudLocalizerResult__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var f = (Action<CloudLocalizerResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCloudLocalizerResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromCloudLocalizerResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromCloudLocalizerResult FunctorOfVoidFromCloudLocalizerResult_to_c(Action<CloudLocalizerResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromCloudLocalizerResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromCloudLocalizerResult_func, _destroy = FunctorOfVoidFromCloudLocalizerResult_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromMegaLandmarkFilterResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromMegaLandmarkFilterResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromMegaLandmarkFilterResult_func(IntPtr state, IntPtr arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_MegaLandmarkFilterResult__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<MegaLandmarkFilterResult>(varg0, easyar_MegaLandmarkFilterResult__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var f = (Action<MegaLandmarkFilterResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromMegaLandmarkFilterResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromMegaLandmarkFilterResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromMegaLandmarkFilterResult FunctorOfVoidFromMegaLandmarkFilterResult_to_c(Action<MegaLandmarkFilterResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromMegaLandmarkFilterResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromMegaLandmarkFilterResult_func, _destroy = FunctorOfVoidFromMegaLandmarkFilterResult_destroy };
        }

        private static int ListOfMegaTrackerBlockInstance_ElementSize = IntPtr.Size;
        public static IntPtr ListOfMegaTrackerBlockInstance_to_c(AutoRelease ar, List<MegaTrackerBlockInstance> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.cdata).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfMegaTrackerBlockInstance_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfMegaTrackerBlockInstance__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfMegaTrackerBlockInstance__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<MegaTrackerBlockInstance> ListOfMegaTrackerBlockInstance_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfMegaTrackerBlockInstance__dtor);
            var size = easyar_ListOfMegaTrackerBlockInstance_size(l);
            var values = new List<MegaTrackerBlockInstance>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfMegaTrackerBlockInstance_at(l, k);
                easyar_MegaTrackerBlockInstance__retain(v, out v);
                values.Add(Object_from_c<MegaTrackerBlockInstance>(v, easyar_MegaTrackerBlockInstance__typeName));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfAccelerometerResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public AccelerometerResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfLocationResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public LocationResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromMegaTrackerLocalizationResponse
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromMegaTrackerLocalizationResponse value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromMegaTrackerLocalizationResponse
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromMegaTrackerLocalizationResponse.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromMegaTrackerLocalizationResponse_func(IntPtr state, IntPtr arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_MegaTrackerLocalizationResponse__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<MegaTrackerLocalizationResponse>(varg0, easyar_MegaTrackerLocalizationResponse__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var f = (Action<MegaTrackerLocalizationResponse>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromMegaTrackerLocalizationResponse.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromMegaTrackerLocalizationResponse_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromMegaTrackerLocalizationResponse FunctorOfVoidFromMegaTrackerLocalizationResponse_to_c(Action<MegaTrackerLocalizationResponse> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromMegaTrackerLocalizationResponse { _state = (IntPtr)(s), _func = FunctorOfVoidFromMegaTrackerLocalizationResponse_func, _destroy = FunctorOfVoidFromMegaTrackerLocalizationResponse_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFloat
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public float value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfMegaTrackerResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfImageTarget
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        private static int ListOfImage_ElementSize = IntPtr.Size;
        public static IntPtr ListOfImage_to_c(AutoRelease ar, List<Image> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.cdata).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfImage_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfImage__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfImage__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<Image> ListOfImage_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfImage__dtor);
            var size = easyar_ListOfImage_size(l);
            var values = new List<Image>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfImage_at(l, k);
                easyar_Image__retain(v, out v);
                values.Add(Object_from_c<Image>(v, easyar_Image__typeName));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromCloudRecognizationResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCloudRecognizationResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromCloudRecognizationResult_func(IntPtr state, IntPtr arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_CloudRecognizationResult__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<CloudRecognizationResult>(varg0, easyar_CloudRecognizationResult__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var f = (Action<CloudRecognizationResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCloudRecognizationResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromCloudRecognizationResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromCloudRecognizationResult FunctorOfVoidFromCloudRecognizationResult_to_c(Action<CloudRecognizationResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromCloudRecognizationResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromCloudRecognizationResult_func, _destroy = FunctorOfVoidFromCloudRecognizationResult_destroy };
        }

        private static int ListOfBlockInfo_ElementSize = Marshal.SizeOf(typeof(BlockInfo));
        public static IntPtr ListOfBlockInfo_to_c(AutoRelease ar, List<BlockInfo> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfBlockInfo_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfBlockInfo__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfBlockInfo__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<BlockInfo> ListOfBlockInfo_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfBlockInfo__dtor);
            var size = easyar_ListOfBlockInfo_size(l);
            var values = new List<BlockInfo>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfBlockInfo_at(l, k);
                values.Add(v);
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromAccelerometerResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromAccelerometerResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromAccelerometerResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, AccelerometerResult arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromAccelerometerResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromAccelerometerResult_func(IntPtr state, AccelerometerResult arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<AccelerometerResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromAccelerometerResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromAccelerometerResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromAccelerometerResult FunctorOfVoidFromAccelerometerResult_to_c(Action<AccelerometerResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromAccelerometerResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromAccelerometerResult_func, _destroy = FunctorOfVoidFromAccelerometerResult_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromInputFrame
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromInputFrame value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromInputFrame
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromInputFrame.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromInputFrame_func(IntPtr state, IntPtr arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_InputFrame__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<InputFrame>(varg0, easyar_InputFrame__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var f = (Action<InputFrame>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromInputFrame.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromInputFrame_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromInputFrame FunctorOfVoidFromInputFrame_to_c(Action<InputFrame> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromInputFrame { _state = (IntPtr)(s), _func = FunctorOfVoidFromInputFrame_func, _destroy = FunctorOfVoidFromInputFrame_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromAttitudeSensorResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromAttitudeSensorResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromAttitudeSensorResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, AttitudeSensorResult arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromAttitudeSensorResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromAttitudeSensorResult_func(IntPtr state, AttitudeSensorResult arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<AttitudeSensorResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromAttitudeSensorResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromAttitudeSensorResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromAttitudeSensorResult FunctorOfVoidFromAttitudeSensorResult_to_c(Action<AttitudeSensorResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromAttitudeSensorResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromAttitudeSensorResult_func, _destroy = FunctorOfVoidFromAttitudeSensorResult_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfAttitudeSensorResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public AttitudeSensorResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromCameraState
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromCameraState value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromCameraState
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, CameraState arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCameraState.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromCameraState_func(IntPtr state, CameraState arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<CameraState>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromCameraState.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromCameraState_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromCameraState FunctorOfVoidFromCameraState_to_c(Action<CameraState> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromCameraState { _state = (IntPtr)(s), _func = FunctorOfVoidFromCameraState_func, _destroy = FunctorOfVoidFromCameraState_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromPermissionStatusAndString
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromPermissionStatusAndString value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromPermissionStatusAndString
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, PermissionStatus arg0, IntPtr arg1, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromPermissionStatusAndString.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromPermissionStatusAndString_func(IntPtr state, PermissionStatus arg0, IntPtr arg1, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var varg1 = arg1;
                    easyar_String_copy(varg1, out varg1);
                    var sarg1 = String_from_c(ar, varg1);
                    var f = (Action<PermissionStatus, string>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromPermissionStatusAndString.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromPermissionStatusAndString_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromPermissionStatusAndString FunctorOfVoidFromPermissionStatusAndString_to_c(Action<PermissionStatus, string> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromPermissionStatusAndString { _state = (IntPtr)(s), _func = FunctorOfVoidFromPermissionStatusAndString_func, _destroy = FunctorOfVoidFromPermissionStatusAndString_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromGyroscopeResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromGyroscopeResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromGyroscopeResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, GyroscopeResult arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromGyroscopeResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromGyroscopeResult_func(IntPtr state, GyroscopeResult arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<GyroscopeResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromGyroscopeResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromGyroscopeResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromGyroscopeResult FunctorOfVoidFromGyroscopeResult_to_c(Action<GyroscopeResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromGyroscopeResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromGyroscopeResult_func, _destroy = FunctorOfVoidFromGyroscopeResult_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfGyroscopeResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public GyroscopeResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromMagnetometerResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromMagnetometerResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromMagnetometerResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, MagnetometerResult arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromMagnetometerResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromMagnetometerResult_func(IntPtr state, MagnetometerResult arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<MagnetometerResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromMagnetometerResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromMagnetometerResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromMagnetometerResult FunctorOfVoidFromMagnetometerResult_to_c(Action<MagnetometerResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromMagnetometerResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromMagnetometerResult_func, _destroy = FunctorOfVoidFromMagnetometerResult_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfMagnetometerResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public MagnetometerResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfBool
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            private Byte value_;
            public bool value { get { return value_ != 0; } set { value_ = (Byte)(value ? 1 : 0); } }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromVideoInputFrameRecorderCompletionReason
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromVideoInputFrameRecorderCompletionReason value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromVideoInputFrameRecorderCompletionReason
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, VideoInputFrameRecorderCompletionReason arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromVideoInputFrameRecorderCompletionReason.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromVideoInputFrameRecorderCompletionReason_func(IntPtr state, VideoInputFrameRecorderCompletionReason arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<VideoInputFrameRecorderCompletionReason>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromVideoInputFrameRecorderCompletionReason.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromVideoInputFrameRecorderCompletionReason_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromVideoInputFrameRecorderCompletionReason FunctorOfVoidFromVideoInputFrameRecorderCompletionReason_to_c(Action<VideoInputFrameRecorderCompletionReason> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromVideoInputFrameRecorderCompletionReason { _state = (IntPtr)(s), _func = FunctorOfVoidFromVideoInputFrameRecorderCompletionReason_func, _destroy = FunctorOfVoidFromVideoInputFrameRecorderCompletionReason_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromLocationResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromLocationResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromLocationResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, LocationResult arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromLocationResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromLocationResult_func(IntPtr state, LocationResult arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<LocationResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromLocationResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromLocationResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromLocationResult FunctorOfVoidFromLocationResult_to_c(Action<LocationResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromLocationResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromLocationResult_func, _destroy = FunctorOfVoidFromLocationResult_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromProximityLocationResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromProximityLocationResult value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromProximityLocationResult
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, ProximityLocationResult arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromProximityLocationResult.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromProximityLocationResult_func(IntPtr state, ProximityLocationResult arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<ProximityLocationResult>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromProximityLocationResult.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromProximityLocationResult_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromProximityLocationResult FunctorOfVoidFromProximityLocationResult_to_c(Action<ProximityLocationResult> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromProximityLocationResult { _state = (IntPtr)(s), _func = FunctorOfVoidFromProximityLocationResult_func, _destroy = FunctorOfVoidFromProximityLocationResult_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfImage
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromLogLevelAndString
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, LogLevel arg0, IntPtr arg1, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromLogLevelAndString.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromLogLevelAndString_func(IntPtr state, LogLevel arg0, IntPtr arg1, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var varg1 = arg1;
                    easyar_String_copy(varg1, out varg1);
                    var sarg1 = String_from_c(ar, varg1);
                    var f = (Action<LogLevel, string>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromLogLevelAndString.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromLogLevelAndString_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromLogLevelAndString FunctorOfVoidFromLogLevelAndString_to_c(Action<LogLevel, string> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromLogLevelAndString { _state = (IntPtr)(s), _func = FunctorOfVoidFromLogLevelAndString_func, _destroy = FunctorOfVoidFromLogLevelAndString_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfImageTrackerResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromRecordStatusAndString
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromRecordStatusAndString value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromRecordStatusAndString
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, RecordStatus arg0, IntPtr arg1, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromRecordStatusAndString.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromRecordStatusAndString_func(IntPtr state, RecordStatus arg0, IntPtr arg1, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var varg1 = arg1;
                    easyar_String_copy(varg1, out varg1);
                    var sarg1 = String_from_c(ar, varg1);
                    var f = (Action<RecordStatus, string>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromRecordStatusAndString.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromRecordStatusAndString_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromRecordStatusAndString FunctorOfVoidFromRecordStatusAndString_to_c(Action<RecordStatus, string> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromRecordStatusAndString { _state = (IntPtr)(s), _func = FunctorOfVoidFromRecordStatusAndString_func, _destroy = FunctorOfVoidFromRecordStatusAndString_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfMatrix44F
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public Matrix44F value;
        }

        private static int ListOfPlaneData_ElementSize = IntPtr.Size;
        public static IntPtr ListOfPlaneData_to_c(AutoRelease ar, List<PlaneData> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.cdata).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfPlaneData_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfPlaneData__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfPlaneData__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<PlaneData> ListOfPlaneData_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfPlaneData__dtor);
            var size = easyar_ListOfPlaneData_size(l);
            var values = new List<PlaneData>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfPlaneData_at(l, k);
                easyar_PlaneData__retain(v, out v);
                values.Add(Object_from_c<PlaneData>(v, easyar_PlaneData__typeName));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromBool
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromBool value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromBool
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, [MarshalAs(UnmanagedType.I1)] bool arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromBool.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromBool_func(IntPtr state, [MarshalAs(UnmanagedType.I1)] bool arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<bool>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromBool.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromBool_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromBool FunctorOfVoidFromBool_to_c(Action<bool> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromBool { _state = (IntPtr)(s), _func = FunctorOfVoidFromBool_func, _destroy = FunctorOfVoidFromBool_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfSparseSpatialMapResult
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public IntPtr value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromBoolAndStringAndString
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, [MarshalAs(UnmanagedType.I1)] bool arg0, IntPtr arg1, IntPtr arg2, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromBoolAndStringAndString.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromBoolAndStringAndString_func(IntPtr state, [MarshalAs(UnmanagedType.I1)] bool arg0, IntPtr arg1, IntPtr arg2, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var varg1 = arg1;
                    easyar_String_copy(varg1, out varg1);
                    var sarg1 = String_from_c(ar, varg1);
                    var varg2 = arg2;
                    easyar_String_copy(varg2, out varg2);
                    var sarg2 = String_from_c(ar, varg2);
                    var f = (Action<bool, string, string>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1, sarg2);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromBoolAndStringAndString.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromBoolAndStringAndString_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromBoolAndStringAndString FunctorOfVoidFromBoolAndStringAndString_to_c(Action<bool, string, string> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromBoolAndStringAndString { _state = (IntPtr)(s), _func = FunctorOfVoidFromBoolAndStringAndString_func, _destroy = FunctorOfVoidFromBoolAndStringAndString_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromBoolAndString
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, [MarshalAs(UnmanagedType.I1)] bool arg0, IntPtr arg1, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromBoolAndString.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromBoolAndString_func(IntPtr state, [MarshalAs(UnmanagedType.I1)] bool arg0, IntPtr arg1, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var varg1 = arg1;
                    easyar_String_copy(varg1, out varg1);
                    var sarg1 = String_from_c(ar, varg1);
                    var f = (Action<bool, string>)((GCHandle)(state)).Target;
                    f(sarg0, sarg1);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromBoolAndString.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromBoolAndString_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromBoolAndString FunctorOfVoidFromBoolAndString_to_c(Action<bool, string> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromBoolAndString { _state = (IntPtr)(s), _func = FunctorOfVoidFromBoolAndString_func, _destroy = FunctorOfVoidFromBoolAndString_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromVideoStatus
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromVideoStatus value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromVideoStatus
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, VideoStatus arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromVideoStatus.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromVideoStatus_func(IntPtr state, VideoStatus arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var sarg0 = arg0;
                    var f = (Action<VideoStatus>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromVideoStatus.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromVideoStatus_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromVideoStatus FunctorOfVoidFromVideoStatus_to_c(Action<VideoStatus> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromVideoStatus { _state = (IntPtr)(s), _func = FunctorOfVoidFromVideoStatus_func, _destroy = FunctorOfVoidFromVideoStatus_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoid
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoid value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfFunctorOfVoidFromFeedbackFrame
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public FunctorOfVoidFromFeedbackFrame value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfVoidFromFeedbackFrame
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromFeedbackFrame.FunctionDelegate))]
#endif
        public static void FunctorOfVoidFromFeedbackFrame_func(IntPtr state, IntPtr arg0, out IntPtr exception)
        {
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_FeedbackFrame__retain(varg0, out varg0);
                    var sarg0 = Object_from_c<FeedbackFrame>(varg0, easyar_FeedbackFrame__typeName);
                    ar.Add(() => sarg0.Dispose());
                    var f = (Action<FeedbackFrame>)((GCHandle)(state)).Target;
                    f(sarg0);
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfVoidFromFeedbackFrame.DestroyDelegate))]
#endif
        public static void FunctorOfVoidFromFeedbackFrame_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfVoidFromFeedbackFrame FunctorOfVoidFromFeedbackFrame_to_c(Action<FeedbackFrame> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfVoidFromFeedbackFrame { _state = (IntPtr)(s), _func = FunctorOfVoidFromFeedbackFrame_func, _destroy = FunctorOfVoidFromFeedbackFrame_destroy };
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FunctorOfOutputFrameFromListOfOutputFrame
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void FunctionDelegate(IntPtr state, IntPtr arg0, out IntPtr Return, out IntPtr exception);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void DestroyDelegate(IntPtr _state);

            public IntPtr _state;
            public FunctionDelegate _func;
            public DestroyDelegate _destroy;
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfOutputFrameFromListOfOutputFrame.FunctionDelegate))]
#endif
        public static void FunctorOfOutputFrameFromListOfOutputFrame_func(IntPtr state, IntPtr arg0, out IntPtr Return, out IntPtr exception)
        {
            Return = default(IntPtr);
            exception = IntPtr.Zero;
            try
            {
                using (var ar = new AutoRelease())
                {
                    var varg0 = arg0;
                    easyar_ListOfOutputFrame_copy(varg0, out varg0);
                    var sarg0 = ListOfOutputFrame_from_c(ar, varg0);
                    sarg0.ForEach(_v0_ => { ar.Add(() => _v0_.Dispose()); });
                    var f = (Func<List<OutputFrame>, OutputFrame>)((GCHandle)(state)).Target;
                    var _return_value_ = f(sarg0);
                    var _return_value_c_ = _return_value_.cdata;
                    Return = _return_value_c_;
                }
            }
            catch (Exception ex)
            {
                exception = Detail.String_to_c_inner(ex.ToString());
            }
        }
#if ENABLE_IL2CPP
        [MonoPInvokeCallback(typeof(FunctorOfOutputFrameFromListOfOutputFrame.DestroyDelegate))]
#endif
        public static void FunctorOfOutputFrameFromListOfOutputFrame_destroy(IntPtr _state)
        {
            ((GCHandle)(_state)).Free();
        }
        public static FunctorOfOutputFrameFromListOfOutputFrame FunctorOfOutputFrameFromListOfOutputFrame_to_c(Func<List<OutputFrame>, OutputFrame> f)
        {
            if (f == null) { throw new ArgumentNullException(); }
            var s = GCHandle.Alloc(f, GCHandleType.Normal);
            return new FunctorOfOutputFrameFromListOfOutputFrame { _state = (IntPtr)(s), _func = FunctorOfOutputFrameFromListOfOutputFrame_func, _destroy = FunctorOfOutputFrameFromListOfOutputFrame_destroy };
        }

        private static int ListOfOutputFrame_ElementSize = IntPtr.Size;
        public static IntPtr ListOfOutputFrame_to_c(AutoRelease ar, List<OutputFrame> l)
        {
            if (l == null) { throw new ArgumentNullException(); }
            var arr = l.Select(e => e.cdata).ToArray();
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                var beginPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                var endPtr = new IntPtr(beginPtr.ToInt64() + ListOfOutputFrame_ElementSize * arr.Length);
                var ptr = IntPtr.Zero;
                easyar_ListOfOutputFrame__ctor(beginPtr, endPtr, out ptr);
                return ar.Add(ptr, easyar_ListOfOutputFrame__dtor);
            }
            finally
            {
                handle.Free();
            }
        }
        public static List<OutputFrame> ListOfOutputFrame_from_c(AutoRelease ar, IntPtr l)
        {
            if (l == IntPtr.Zero) { throw new ArgumentNullException(); }
            ar.Add(l, easyar_ListOfOutputFrame__dtor);
            var size = easyar_ListOfOutputFrame_size(l);
            var values = new List<OutputFrame>();
            values.Capacity = size;
            for (int k = 0; k < size; k += 1)
            {
                var v = easyar_ListOfOutputFrame_at(l, k);
                easyar_OutputFrame__retain(v, out v);
                values.Add(Object_from_c<OutputFrame>(v, easyar_OutputFrame__typeName));
            }
            return values;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfVec3F
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public Vec3F value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OptionalOfVec4F
        {
            private Byte has_value_;
            public bool has_value { get { return has_value_ != 0; } set { has_value_ = (Byte)(value ? 1 : 0); } }
            public Vec4F value;
        }

    }

    /// <summary>
    /// <para xml:lang="en">
    /// ObjectTargetParameters represents the parameters to create a `ObjectTarget`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// ObjectTargetParameters表示创建 `ObjectTarget`_ 所需要的参数。
    /// </para>
    /// </summary>
    public class ObjectTargetParameters : RefBase
    {
        internal ObjectTargetParameters(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ObjectTargetParameters(cdata_new, deleter_, retainer_);
        }
        public new ObjectTargetParameters Clone()
        {
            return (ObjectTargetParameters)(CloneObject());
        }
        public ObjectTargetParameters() : base(IntPtr.Zero, Detail.easyar_ObjectTargetParameters__dtor, Detail.easyar_ObjectTargetParameters__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ObjectTargetParameters__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets `Buffer`_ dictionary.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取 `Buffer`_ 字典。
        /// </para>
        /// </summary>
        public virtual BufferDictionary bufferDictionary()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTargetParameters_bufferDictionary(cdata, out _return_value_);
                return Detail.Object_from_c<BufferDictionary>(_return_value_, Detail.easyar_BufferDictionary__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `Buffer`_ dictionary. obj, mtl and jpg/png files shall be loaded into the dictionay, and be able to be located by relative or absolute paths.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `Buffer`_ 字典。需要将obj, mtl和jpg/png文件加载到这个字典中，并使得mtl和jpg/png能通过相对或绝对路径查找到。
        /// </para>
        /// </summary>
        public virtual void setBufferDictionary(BufferDictionary bufferDictionary)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTargetParameters_setBufferDictionary(cdata, bufferDictionary.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets obj file path.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取obj文件路径。
        /// </para>
        /// </summary>
        public virtual string objPath()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTargetParameters_objPath(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets obj file path.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置obj文件路径。
        /// </para>
        /// </summary>
        public virtual void setObjPath(string objPath)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTargetParameters_setObjPath(cdata, Detail.String_to_c(ar, objPath));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets target name. It can be used to distinguish targets.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target名字。名字用来区分target。
        /// </para>
        /// </summary>
        public virtual string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTargetParameters_name(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets target name.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置target名字。
        /// </para>
        /// </summary>
        public virtual void setName(string name)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTargetParameters_setName(cdata, Detail.String_to_c(ar, name));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the target uid. You can set this uid in the json config as a method to distinguish from targets.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target uid。可以在json配置中设置这个uid，在自己的代码中作为一种区分target的方法。
        /// </para>
        /// </summary>
        public virtual string uid()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTargetParameters_uid(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets target uid.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置target uid。
        /// </para>
        /// </summary>
        public virtual void setUid(string uid)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTargetParameters_setUid(cdata, Detail.String_to_c(ar, uid));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets meta data.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meta data。
        /// </para>
        /// </summary>
        public virtual string meta()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTargetParameters_meta(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets meta data。
        /// </para>
        /// <para xml:lang="zh">
        /// 设置meta data。
        /// </para>
        /// </summary>
        public virtual void setMeta(string meta)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTargetParameters_setMeta(cdata, Detail.String_to_c(ar, meta));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the scale of model. The value is the physical scale divided by model coordinate system scale. The default value is 1. (Supposing the unit of model coordinate system is 1 meter.)
        /// </para>
        /// <para xml:lang="zh">
        /// 模型的缩放比例。其值为模型在空间中的物理大小与在模型坐标系中的大小的比值，默认值为1。（假设模型坐标系中的标尺单位为米。）
        /// </para>
        /// </summary>
        public virtual float scale()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTargetParameters_scale(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the scale of model. The value is the physical scale divided by model coordinate system scale. The default value is 1. (Supposing the unit of model coordinate system is 1 meter.)
        /// It is needed to set the model scale in rendering engine separately.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置模型的缩放比例。其值为模型在空间中的物理大小与在模型坐标系中的大小的比值，默认值为1（假设模型坐标系中的标尺单位为米）。
        /// 还需要在渲染引擎中单独设置此模型缩放。
        /// </para>
        /// </summary>
        public virtual void setScale(float size)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTargetParameters_setScale(cdata, size);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ObjectTarget represents 3d object targets that can be tracked by `ObjectTracker`_ .
    /// The size of ObjectTarget is determined by the `obj` file. You can change it by changing the object `scale`, which is default to 1.
    /// A ObjectTarget can be tracked by `ObjectTracker`_ after a successful load into the `ObjectTracker`_ using `ObjectTracker.loadTarget`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// ObjectTarget表示3D object target，它可以被 `ObjectTracker`_ 所跟踪。
    /// ObjectTarget的大小由 `obj` 文件决定。可以通过修改 `scale` 达到修改size的目的。 `scale` 默认为1。
    /// ObjectTarget通过 `ObjectTracker.loadTarget`_ 成功载入 `ObjectTracker`_ 之后可以被 `ObjectTracker`_ 检测和跟踪。
    /// </para>
    /// </summary>
    public class ObjectTarget : Target
    {
        internal ObjectTarget(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ObjectTarget(cdata_new, deleter_, retainer_);
        }
        public new ObjectTarget Clone()
        {
            return (ObjectTarget)(CloneObject());
        }
        public ObjectTarget() : base(IntPtr.Zero, Detail.easyar_ObjectTarget__dtor, Detail.easyar_ObjectTarget__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ObjectTarget__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a target from parameters.
        /// </para>
        /// <para xml:lang="zh">
        /// 从参数创建。
        /// </para>
        /// </summary>
        public static Optional<ObjectTarget> createFromParameters(ObjectTargetParameters parameters)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfObjectTarget);
                Detail.easyar_ObjectTarget_createFromParameters(parameters.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ObjectTarget>(p.value, Detail.easyar_ObjectTarget__typeName) : Optional<ObjectTarget>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creats a target from obj, mtl and jpg/png files.
        /// </para>
        /// <para xml:lang="zh">
        /// 从obj, mtl和jpg/png文件创建。
        /// </para>
        /// </summary>
        public static Optional<ObjectTarget> createFromObjectFile(string path, StorageType storageType, string name, string uid, string meta, float scale)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfObjectTarget);
                Detail.easyar_ObjectTarget_createFromObjectFile(Detail.String_to_c(ar, path), storageType, Detail.String_to_c(ar, name), Detail.String_to_c(ar, uid), Detail.String_to_c(ar, meta), scale, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ObjectTarget>(p.value, Detail.easyar_ObjectTarget__typeName) : Optional<ObjectTarget>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The scale of model. The value is the physical scale divided by model coordinate system scale. The default value is 1. (Supposing the unit of model coordinate system is 1 meter.)
        /// </para>
        /// <para xml:lang="zh">
        /// 模型的缩放比例。其值为模型在空间中的物理大小与在模型坐标系中的大小的比值，默认值为1。（假设模型坐标系中的标尺单位为米）
        /// </para>
        /// </summary>
        public virtual float scale()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTarget_scale(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The bounding box of object, it contains the 8 points of the box.
        /// Vertices&#39;s indices are defined and stored following the rule:
        /// ::
        ///
        ///       4-----7
        ///      /|    /|
        ///     5-----6 |    z
        ///     | |   | |    |
        ///     | 0---|-3    o---y
        ///     |/    |/    /
        ///     1-----2    x
        /// </para>
        /// <para xml:lang="zh">
        /// 物体的包围盒，包括盒子的8个顶点。
        /// 顶点索引定义如下：
        /// ::
        ///
        ///       4-----7
        ///      /|    /|
        ///     5-----6 |    z
        ///     | |   | |    |
        ///     | 0---|-3    o---y
        ///     |/    |/    /
        ///     1-----2    x
        /// </para>
        /// </summary>
        public virtual List<Vec3F> boundingBox()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTarget_boundingBox(cdata, out _return_value_);
                return Detail.ListOfVec3F_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets model target scale, this will overwrite the value set in the json file or the default value. The value is the physical scale divided by model coordinate system scale. The default value is 1. (Supposing the unit of model coordinate system is 1 meter.)
        /// It is needed to set the model scale in rendering engine separately.
        /// It also should been done before loading ObjectTarget into  `ObjectTracker`_ using `ObjectTracker.loadTarget`_.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置模型的缩放比例。设置之后会覆盖默认值以及在json文件中设的数值。其值为模型在空间中的物理大小与在模型坐标系中的大小的比值，默认值为1。（假设模型坐标系中的标尺单位为米）
        /// 还需要在渲染引擎中单独设置此模型缩放。
        /// 注意该设置需要在通过 `ObjectTracker.loadTarget`_ 载入 `ObjectTracker`_ 之前进行。
        /// </para>
        /// </summary>
        public virtual bool setScale(float scale)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTarget_setScale(cdata, scale);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target id. A target id is a integer number generated at runtime. This id is non-zero and increasing globally.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target id。target id是运行时创建的整型数据，只有在成功的配置之后才是有效（非0）的。这个id是非0且全局递增的。
        /// </para>
        /// </summary>
        public override int runtimeID()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTarget_runtimeID(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target uid. A target uid is useful in cloud based algorithms. If no cloud is used, you can set this uid in the json config as a alternative method to distinguish from targets.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target uid。ImageTarget的uid在云识别算法中使用。在没有接入云识别的时候，你可以在json配置中设置这个uid，在自己的代码中作为另一种区分target的方法。
        /// </para>
        /// </summary>
        public override string uid()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTarget_uid(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target name. Name is used to distinguish targets in a json file.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target名字。名字用来在json文件中区分target。
        /// </para>
        /// </summary>
        public override string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTarget_name(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set name. It will erase previously set data or data from cloud.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置target名字。这个操作会覆盖上一次的设置或是服务器返回的数据。
        /// </para>
        /// </summary>
        public override void setName(string name)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTarget_setName(cdata, Detail.String_to_c(ar, name));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the meta data set by setMetaData. Or, in a cloud returned target, returns the meta data set in the cloud server.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取setMetaData所设置的meta data。或者在云识别返回的target中，获得服务器所设置的meta data。
        /// </para>
        /// </summary>
        public override string meta()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTarget_meta(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set meta data. It will erase previously set data or data from cloud.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置meta data。这个操作会覆盖上一次的设置或是服务器返回的数据。
        /// </para>
        /// </summary>
        public override void setMeta(string data)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTarget_setMeta(cdata, Detail.String_to_c(ar, data));
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Result of `ObjectTracker`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// `ObjectTracker`_ 的结果。
    /// </para>
    /// </summary>
    internal class ObjectTrackerResult : TargetTrackerResult
    {
        internal ObjectTrackerResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ObjectTrackerResult(cdata_new, deleter_, retainer_);
        }
        public new ObjectTrackerResult Clone()
        {
            return (ObjectTrackerResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the list of `TargetInstance`_ contained in the result.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前结果中包含的 `TargetInstance`_ 列表。
        /// </para>
        /// </summary>
        public override List<TargetInstance> targetInstances()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTrackerResult_targetInstances(cdata, out _return_value_);
                return Detail.ListOfTargetInstance_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the list of `TargetInstance`_ contained in the result.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前结果中包含的 `TargetInstance`_ 列表。
        /// </para>
        /// </summary>
        public override void setTargetInstances(List<TargetInstance> instances)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTrackerResult_setTargetInstances(cdata, Detail.ListOfTargetInstance_to_c(ar, instances));
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ObjectTracker implements 3D object target detection and tracking.
    /// ObjectTracker occupies (1 + SimultaneousNum) buffers of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components.
    /// After creation, you can call start/stop to enable/disable the track process. start and stop are very lightweight calls.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// ObjectTracker inputs `FeedbackFrame`_ from feedbackFrameSink. `FeedbackFrameSource`_ shall be connected to feedbackFrameSink for use.
    /// Before a `Target`_ can be tracked by ObjectTracker, you have to load it using loadTarget/unloadTarget. You can get load/unload results from callbacks passed into the interfaces.
    /// </para>
    /// <para xml:lang="zh">
    /// ObjectTracker实现了3D object target的检测和跟踪。
    /// ObjectTracker占用(1 + SimultaneousNum)个camera的buffer。应使用camera的setBufferCapacity设置不少于所有组件占用的camera的buffer数量。
    /// 创建之后，可以调用start/stop来开始和停止运行，start/stop是非常轻量的调用。
    /// 当不再需要该组件时，可以调用close对其进行关闭。close之后不应继续使用。
    /// ObjectTracker通过feedbackFrameSink输入 `FeedbackFrame`_ ，应将 `FeedbackFrameSource`_ 连接到feedbackFrameSink上进行使用。
    /// 在 `Target`_ 可以被ObjectTracker跟踪之前，你需要通过loadTarget/unloadTarget将它载入。可以通过传入接口的回调来获取load/unload的结果。
    /// </para>
    /// </summary>
    internal class ObjectTracker : RefBase
    {
        internal ObjectTracker(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ObjectTracker(cdata_new, deleter_, retainer_);
        }
        public new ObjectTracker Clone()
        {
            return (ObjectTracker)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTracker_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `FeedbackFrame`_ input port. The InputFrame member of FeedbackFrame must have raw image, timestamp, and camera parameters. Both pinhole and fisheye camera are supported.
        /// </para>
        /// <para xml:lang="zh">
        /// `FeedbackFrame`_ 输入端口。FeedbackFrame中的InputFrame成员要求必须拥有图像、时间戳和camera参数，camera支持针孔和鱼眼相机。
        /// </para>
        /// </summary>
        public virtual FeedbackFrameSink feedbackFrameSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTracker_feedbackFrameSink(cdata, out _return_value_);
                return Detail.Object_from_c<FeedbackFrameSink>(_return_value_, Detail.easyar_FeedbackFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTracker_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `OutputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `OutputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource outputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTracker_outputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static ObjectTracker create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTracker_create(out _return_value_);
                return Detail.Object_from_c<ObjectTracker>(_return_value_, Detail.easyar_ObjectTracker__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets result post-processing.
        /// enablePersistentTargetInstance defaults to false. When it is enabled and `InputFrame`_ contains spatial information, targetInstances in `ImageTrackerResult`_ will contain all recognized instances (with not tracking target instances).
        /// enableMotionFusion defaults to false. When it is enabled and `InputFrame`_ contains temporal information and spatial information, pose of targetInstances in `ImageTrackerResult`_ will utilize RealTimeCoordinateTransform .
        /// </para>
        /// <para xml:lang="zh">
        /// 设置结果后处理。
        /// enablePersistentTargetInstance默认为false，开启时，如果 `InputFrame`_ 数据中包含空间信息，则 `ImageTrackerResult`_ 中的targetInstances会包含所有识别到的instance(包括当前未跟踪的)。
        /// enableMotionFusion默认为false，开启时，如果 `InputFrame`_ 数据中包含时间信息和空间信息，则 `ImageTrackerResult`_ 中的 targetInstances 的 pose 会进行平滑处理使用 RealTimeCoordinateTransform 进行处理。
        /// </para>
        /// </summary>
        public virtual void setResultPostProcessing(bool enablePersistentTargetInstance, bool enableMotionFusion)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTracker_setResultPostProcessing(cdata, enablePersistentTargetInstance, enableMotionFusion);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets result async mode. Set true If results are to be outputted via outputFrameSource, false if results are to be fetched via getSyncResult. enableAsync defaults to true. Sync mode is only available with input frames including spatial information and with a XR license.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置结果异步模式。如果结果通过outputFrameSource输出，则应设置位true，如果结果通过getSyncResult获取，则应设置为false。默认enableAsync为true。同步模式仅在输入帧包含空间信息且使用XR License时可生效。
        /// </para>
        /// </summary>
        public virtual bool setResultAsyncMode(bool enableAsync)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTracker_setResultAsyncMode(cdata, enableAsync);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts the track algorithm.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始跟踪算法。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTracker_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops the track algorithm. Call start to start the track again.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停跟踪算法。调用start来重新启动跟踪。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTracker_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTracker_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Load a `Target`_ into the tracker. A Target can only be tracked by tracker after a successful load.
        /// This method is an asynchronous method. A load operation may take some time to finish and detection of a new/lost target may take more time during the load. The track time after detection will not be affected. If you want to know the load result, you have to handle the callback data. The callback will be called from the thread specified by `CallbackScheduler`_ . It will not block the track thread or any other operations except other load/unload.
        /// </para>
        /// <para xml:lang="zh">
        /// 加载一个 `Target`_ 进入tracker。 `Target`_ 只有在成功加载进入tracker之后才能被识别和跟踪。
        /// 这个方法是异步方法。加载过程可能会需要一些时间来完成，这段时间内新的和丢失的target的检测可能会花比平时更多的时间，但是检测到之后的跟踪不受影响。如果你希望知道加载的结果，需要处理callback数据。callback将会在 `CallbackScheduler`_ 所指定的线程上被调用。跟踪线程和除了其它加载/卸载之外的操作都不会被阻塞。
        /// </para>
        /// </summary>
        public virtual void loadTarget(Target target, CallbackScheduler callbackScheduler, Action<Target, bool> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTracker_loadTarget(cdata, target.cdata, callbackScheduler.cdata, Detail.FunctorOfVoidFromTargetAndBool_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Unload a `Target`_ from the tracker.
        /// This method is an asynchronous method. An unload operation may take some time to finish and detection of a new/lost target may take more time during the unload. If you want to know the unload result, you have to handle the callback data. The callback will be called from the thread specified by `CallbackScheduler`_ . It will not block the track thread or any other operations except other load/unload.
        /// </para>
        /// <para xml:lang="zh">
        /// 从tracker中卸载 `Target`_ 。
        /// 这个方法是异步方法。卸载过程可能会需要一些时间来完成，这段时间内新的和丢失的target的检测可能会花比平时更多的时间，但是检测到之后的跟踪不受影响。如果你希望知道卸载的结果，需要处理callback数据。callback将会在 `CallbackScheduler`_ 所指定的线程上被调用。跟踪线程和除了其它加载/卸载之外的操作都不会被阻塞。
        /// </para>
        /// </summary>
        public virtual void unloadTarget(Target target, CallbackScheduler callbackScheduler, Action<Target, bool> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ObjectTracker_unloadTarget(cdata, target.cdata, callbackScheduler.cdata, Detail.FunctorOfVoidFromTargetAndBool_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns current loaded targets in the tracker. If an asynchronous load/unload is in progress, the returned value will not reflect the result until all load/unload finish.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前已经被加载进入tracker的target。如果异步的加载/卸载正在执行，在加载/卸载完成之前的返回值将不会反映这些加载/卸载的结果。
        /// </para>
        /// </summary>
        public virtual List<Target> targets()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ObjectTracker_targets(cdata, out _return_value_);
                return Detail.ListOfTarget_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the max number of targets which will be the simultaneously tracked by the tracker. The default value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置最大可被tracker跟踪的目标个数。默认值为1。
        /// </para>
        /// </summary>
        public virtual bool setSimultaneousNum(int num)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTracker_setSimultaneousNum(cdata, num);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the max number of targets which will be the simultaneously tracked by the tracker. The default value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取最大可被tracker跟踪的目标个数。默认值为1。
        /// </para>
        /// </summary>
        public virtual int simultaneousNum()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ObjectTracker_simultaneousNum(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get synchronized result. If ObjectTracker is paused, or the result async mode is not set to false through setResultAsyncMode, the return value is empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得同步输出结果。如果ObjectTracker已暂停，或者未通过setResultAsyncMode将结果异步模式设为false，则返回值为空。
        /// </para>
        /// </summary>
        public virtual Optional<ObjectTrackerResult> getSyncResult(MotionInputData motionInputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfObjectTrackerResult);
                Detail.easyar_ObjectTracker_getSyncResult(cdata, motionInputData.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ObjectTrackerResult>(p.value, Detail.easyar_ObjectTrackerResult__typeName) : Optional<ObjectTrackerResult>.Empty);
            }
        }
    }

    internal enum ARCoreDeviceListDownloadStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Download successful.
        /// </para>
        /// <para xml:lang="zh">
        /// 下载成功
        /// </para>
        /// </summary>
        Successful = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Data is already latest.
        /// </para>
        /// <para xml:lang="zh">
        /// 数据已是最新版本
        /// </para>
        /// </summary>
        NotModified = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Connection error
        /// </para>
        /// <para xml:lang="zh">
        /// 连接错误
        /// </para>
        /// </summary>
        ConnectionError = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Unexpected error
        /// </para>
        /// <para xml:lang="zh">
        /// 其他错误
        /// </para>
        /// </summary>
        UnexpectedError = 3,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ARCoreDeviceListDownloader is used for download and update of device list data in ARCoreCameraDevice.
    /// </para>
    /// <para xml:lang="zh">
    /// ARCoreDeviceListDownloader用于下载更新ARCoreCameraDevice中使用的设备列表标定参数。
    /// </para>
    /// </summary>
    internal class ARCoreDeviceListDownloader : RefBase
    {
        internal ARCoreDeviceListDownloader(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ARCoreDeviceListDownloader(cdata_new, deleter_, retainer_);
        }
        public new ARCoreDeviceListDownloader Clone()
        {
            return (ARCoreDeviceListDownloader)(CloneObject());
        }
        public ARCoreDeviceListDownloader() : base(IntPtr.Zero, Detail.easyar_ARCoreDeviceListDownloader__dtor, Detail.easyar_ARCoreDeviceListDownloader__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ARCoreDeviceListDownloader__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        public virtual void download(Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<ARCoreDeviceListDownloadStatus, Optional<string>> onCompleted)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARCoreDeviceListDownloader_download(cdata, timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromARCoreDeviceListDownloadStatusAndOptionalOfString_to_c(onCompleted));
            }
        }
    }

    internal enum CalibrationDownloadStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Download successful.
        /// </para>
        /// <para xml:lang="zh">
        /// 下载成功
        /// </para>
        /// </summary>
        Successful = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Data is already latest.
        /// </para>
        /// <para xml:lang="zh">
        /// 数据已是最新版本
        /// </para>
        /// </summary>
        NotModified = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Connection error
        /// </para>
        /// <para xml:lang="zh">
        /// 连接错误
        /// </para>
        /// </summary>
        ConnectionError = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Unexpected error
        /// </para>
        /// <para xml:lang="zh">
        /// 其他错误
        /// </para>
        /// </summary>
        UnexpectedError = 3,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// CalibrationDownloader is used for download and update of calibration data in MotionTracker. The calibration data will only take effect after reallocation of MotionTracker.
    /// </para>
    /// <para xml:lang="zh">
    /// CalibrationDownloader用于下载更新MotionTracker中使用的标定参数。下载后需要重新创建MotionTracker才会起作用。
    /// </para>
    /// </summary>
    internal class CalibrationDownloader : RefBase
    {
        internal CalibrationDownloader(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CalibrationDownloader(cdata_new, deleter_, retainer_);
        }
        public new CalibrationDownloader Clone()
        {
            return (CalibrationDownloader)(CloneObject());
        }
        public CalibrationDownloader() : base(IntPtr.Zero, Detail.easyar_CalibrationDownloader__dtor, Detail.easyar_CalibrationDownloader__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_CalibrationDownloader__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        public virtual void download(Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<CalibrationDownloadStatus, Optional<string>> onCompleted)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CalibrationDownloader_download(cdata, timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromCalibrationDownloadStatusAndOptionalOfString_to_c(onCompleted));
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// MEGA API Type.
    /// </para>
    /// <para xml:lang="zh">
    /// MEGA API类型。
    /// </para>
    /// </summary>
    public enum MegaApiType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Mega Block (CLS).
        /// </para>
        /// <para xml:lang="zh">
        /// Mega Block（CLS）。
        /// </para>
        /// </summary>
        Block = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Mega Landmark.
        /// </para>
        /// <para xml:lang="zh">
        /// Mega Landmark。
        /// </para>
        /// </summary>
        Landmark = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// CloudLocalizer status.
    /// </para>
    /// <para xml:lang="zh">
    /// 云定位状态。
    /// </para>
    /// </summary>
    internal enum CloudLocalizerStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Unknown error
        /// </para>
        /// <para xml:lang="zh">
        /// 未知错误
        /// </para>
        /// </summary>
        UnknownError = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// A block is localized.
        /// </para>
        /// <para xml:lang="zh">
        /// 定位到block
        /// </para>
        /// </summary>
        Found = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// No blocks are localized.
        /// </para>
        /// <para xml:lang="zh">
        /// 没有定位到block
        /// </para>
        /// </summary>
        NotFound = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Request time out (more than 1 minute)
        /// </para>
        /// <para xml:lang="zh">
        /// 请求超时(超过1分钟)
        /// </para>
        /// </summary>
        RequestTimeout = 3,
        /// <summary>
        /// <para xml:lang="en">
        /// Request time interval is too low
        /// </para>
        /// <para xml:lang="zh">
        /// 请求间隔过短
        /// </para>
        /// </summary>
        RequestIntervalTooLow = 4,
        /// <summary>
        /// <para xml:lang="en">
        /// QPS limit exceeded
        /// </para>
        /// <para xml:lang="zh">
        /// QPS超过限制
        /// </para>
        /// </summary>
        QpsLimitExceeded = 5,
        /// <summary>
        /// <para xml:lang="en">
        /// Service is waking up
        /// </para>
        /// <para xml:lang="zh">
        /// 服务正在唤醒中
        /// </para>
        /// </summary>
        WakingUp = 6,
        /// <summary>
        /// <para xml:lang="en">
        /// SpotVersionId is missing. It is possible that the value is not set.
        /// </para>
        /// <para xml:lang="zh">
        /// 缺少SpotVersionId。可能是没有设置。
        /// </para>
        /// </summary>
        MissingSpotVersionId = 7,
        /// <summary>
        /// <para xml:lang="en">
        /// API Token expired.
        /// </para>
        /// <para xml:lang="zh">
        /// API Token过期。
        /// </para>
        /// </summary>
        ApiTokenExpired = 8,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// The block instance localized by MegaTracker.
    /// </para>
    /// <para xml:lang="zh">
    /// CloudLocalizer定位到的block的实例。
    /// </para>
    /// </summary>
    internal class CloudLocalizerBlockInstance : RefBase
    {
        internal CloudLocalizerBlockInstance(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CloudLocalizerBlockInstance(cdata_new, deleter_, retainer_);
        }
        public new CloudLocalizerBlockInstance Clone()
        {
            return (CloudLocalizerBlockInstance)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The ID of the block.
        /// </para>
        /// <para xml:lang="zh">
        /// 地图block的ID。
        /// </para>
        /// </summary>
        public virtual string blockId()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudLocalizerBlockInstance_blockId(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The name of the block.
        /// </para>
        /// <para xml:lang="zh">
        /// 地图block的名称。
        /// </para>
        /// </summary>
        public virtual string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudLocalizerBlockInstance_name(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The map pose in the camera coordinates.
        /// </para>
        /// <para xml:lang="zh">
        /// 地图在相机坐标系中的姿态。
        /// </para>
        /// </summary>
        public virtual Matrix44F pose()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CloudLocalizerBlockInstance_pose(cdata);
                return _return_value_;
            }
        }
    }

    internal class CloudLocalizerResult : RefBase
    {
        internal CloudLocalizerResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CloudLocalizerResult(cdata_new, deleter_, retainer_);
        }
        public new CloudLocalizerResult Clone()
        {
            return (CloudLocalizerResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Localization status.
        /// </para>
        /// <para xml:lang="zh">
        /// 定位状态。
        /// </para>
        /// </summary>
        public virtual CloudLocalizerStatus localizeStatus()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CloudLocalizerResult_localizeStatus(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The spot version ID. Only available with `MegaApiType`_.Landmark .
        /// </para>
        /// <para xml:lang="zh">
        /// 地点版本ID。仅当开启 `MegaApiType`_.Landmark 时可用。
        /// </para>
        /// </summary>
        public virtual Optional<string> spotVersionId()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfString);
                Detail.easyar_CloudLocalizerResult_spotVersionId(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.String_from_c(ar, p.value) : Optional<string>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current localized block instances.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前定位到的block的实例。
        /// </para>
        /// </summary>
        public virtual List<CloudLocalizerBlockInstance> blockInstances()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudLocalizerResult_blockInstances(cdata, out _return_value_);
                return Detail.ListOfCloudLocalizerBlockInstance_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Extra informations of the localization.
        /// </para>
        /// <para xml:lang="zh">
        /// 额外信息。
        /// </para>
        /// </summary>
        public virtual string extraInfo()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudLocalizerResult_extraInfo(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Detailed exception message.
        /// </para>
        /// <para xml:lang="zh">
        /// 异常信息。
        /// </para>
        /// </summary>
        public virtual string exceptionInfo()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudLocalizerResult_exceptionInfo(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The duration in seconds for server response.
        /// </para>
        /// <para xml:lang="zh">
        /// 服务器响应耗时（秒）。
        /// </para>
        /// </summary>
        public virtual Optional<double> serverResponseDuration()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CloudLocalizerResult_serverResponseDuration(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<double>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The duration in seconds for server internal calculation.
        /// </para>
        /// <para xml:lang="zh">
        /// 服务器内部计算耗时（秒）。
        /// </para>
        /// </summary>
        public virtual Optional<double> serverCalculationDuration()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CloudLocalizerResult_serverCalculationDuration(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<double>.Empty);
            }
        }
    }

    internal class DeviceAuxiliaryInfo : RefBase
    {
        internal DeviceAuxiliaryInfo(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new DeviceAuxiliaryInfo(cdata_new, deleter_, retainer_);
        }
        public new DeviceAuxiliaryInfo Clone()
        {
            return (DeviceAuxiliaryInfo)(CloneObject());
        }
        public static DeviceAuxiliaryInfo create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_DeviceAuxiliaryInfo_create(out _return_value_);
                return Detail.Object_from_c<DeviceAuxiliaryInfo>(_return_value_, Detail.easyar_DeviceAuxiliaryInfo__typeName);
            }
        }
        public virtual void setAcceleration(AccelerometerResult acce)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_DeviceAuxiliaryInfo_setAcceleration(cdata, acce);
            }
        }
        public virtual void setGPSLocation(LocationResult gps)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_DeviceAuxiliaryInfo_setGPSLocation(cdata, gps);
            }
        }
        public virtual void setProximityLocation(ProximityLocationResult proximity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_DeviceAuxiliaryInfo_setProximityLocation(cdata, proximity);
            }
        }
        public virtual void setBlockPrior(BlockPriorResult blockPrior)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_DeviceAuxiliaryInfo_setBlockPrior(cdata, blockPrior.cdata);
            }
        }
        public virtual void setECompass(double northHeading, double headingAccuracy)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_DeviceAuxiliaryInfo_setECompass(cdata, northHeading, headingAccuracy);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// CloudLocalizer implements cloud based localization.
    /// </para>
    /// <para xml:lang="zh">
    /// CloudLocalizer实现了云定位功能。
    /// </para>
    /// </summary>
    internal class CloudLocalizer : RefBase
    {
        internal CloudLocalizer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CloudLocalizer(cdata_new, deleter_, retainer_);
        }
        public new CloudLocalizer Clone()
        {
            return (CloudLocalizer)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CloudLocalizer_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance and connects to the server.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建并连接服务器。
        /// </para>
        /// </summary>
        public static CloudLocalizer create(string server, string apiKey, string apiSecret, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudLocalizer_create(Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiKey), Detail.String_to_c(ar, apiSecret), Detail.String_to_c(ar, appId), out _return_value_);
                return Detail.Object_from_c<CloudLocalizer>(_return_value_, Detail.easyar_CloudLocalizer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with API Token and connects to the server.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用API Token创建并连接服务器。
        /// </para>
        /// </summary>
        public static CloudLocalizer createWithToken(string server, string apiToken, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudLocalizer_createWithToken(Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiToken), Detail.String_to_c(ar, appId), out _return_value_);
                return Detail.Object_from_c<CloudLocalizer>(_return_value_, Detail.easyar_CloudLocalizer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Updates API Token.
        /// </para>
        /// <para xml:lang="zh">
        /// 更新API Token。
        /// </para>
        /// </summary>
        public virtual void updateToken(string apiToken)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudLocalizer_updateToken(cdata, Detail.String_to_c(ar, apiToken));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets MEGA API Type. The default is Block.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置MEGA API类型。默认为Block
        /// </para>
        /// </summary>
        public virtual void setMegaApiType(MegaApiType type)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudLocalizer_setMegaApiType(cdata, type);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Send localization request.
        /// Send `InputFrame`_ to resolve a cloud localization. `InputFrame`_ should have at least image data and camera parameters.
        /// message input is a json string.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求定位。
        /// 输入`InputFrame`_进行云定位。`InputFrame`_必须包含图像以及相机参数。
        /// message为json格式字串。
        /// </para>
        /// </summary>
        public virtual void resolve(InputFrame inputFrame, string message, DeviceAuxiliaryInfo deviceAuxInfo, Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<CloudLocalizerResult> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudLocalizer_resolve(cdata, inputFrame.cdata, Detail.String_to_c(ar, message), deviceAuxInfo.cdata, timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromCloudLocalizerResult_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Send localization request.
        /// Send `InputFrame`_ to resolve a cloud localization. `InputFrame`_ should have at least image data and camera parameters.
        /// message input is a json string.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求定位。
        /// 输入`InputFrame`_进行云定位。`InputFrame`_必须包含图像以及相机参数。
        /// message为json格式字串。
        /// </para>
        /// </summary>
        public virtual void resolveLandmark(InputFrame inputFrame, string message, DeviceAuxiliaryInfo deviceAuxInfo, string spotVersionId, Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<CloudLocalizerResult> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudLocalizer_resolveLandmark(cdata, inputFrame.cdata, Detail.String_to_c(ar, message), deviceAuxInfo.cdata, Detail.String_to_c(ar, spotVersionId), timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromCloudLocalizerResult_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops the localization and closes connection. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止定位并关闭连接。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudLocalizer_close(cdata);
            }
        }
    }

    public enum MegaLandmarkFilterStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Unknown error
        /// </para>
        /// <para xml:lang="zh">
        /// 未知错误
        /// </para>
        /// </summary>
        UnknownError = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Localized.
        /// </para>
        /// <para xml:lang="zh">
        /// 定位到
        /// </para>
        /// </summary>
        Found = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Not localized.
        /// </para>
        /// <para xml:lang="zh">
        /// 没有定位到
        /// </para>
        /// </summary>
        NotFound = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Request time out (more than 1 minute)
        /// </para>
        /// <para xml:lang="zh">
        /// 请求超时(超过1分钟)
        /// </para>
        /// </summary>
        RequestTimeout = 3,
        /// <summary>
        /// <para xml:lang="en">
        /// Request time interval is too low
        /// </para>
        /// <para xml:lang="zh">
        /// 请求间隔过短
        /// </para>
        /// </summary>
        RequestIntervalTooLow = 4,
        /// <summary>
        /// <para xml:lang="en">
        /// QPS limit exceeded
        /// </para>
        /// <para xml:lang="zh">
        /// QPS超过限制
        /// </para>
        /// </summary>
        QpsLimitExceeded = 5,
        /// <summary>
        /// <para xml:lang="en">
        /// Service is waking up
        /// </para>
        /// <para xml:lang="zh">
        /// 服务正在唤醒中
        /// </para>
        /// </summary>
        WakingUp = 6,
        /// <summary>
        /// <para xml:lang="en">
        /// API Token expired.
        /// </para>
        /// <para xml:lang="zh">
        /// API Token过期。
        /// </para>
        /// </summary>
        ApiTokenExpired = 8,
    }

    internal class MegaLandmarkFilterResult : RefBase
    {
        internal MegaLandmarkFilterResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MegaLandmarkFilterResult(cdata_new, deleter_, retainer_);
        }
        public new MegaLandmarkFilterResult Clone()
        {
            return (MegaLandmarkFilterResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Request status.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求状态。
        /// </para>
        /// </summary>
        public virtual MegaLandmarkFilterStatus status()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaLandmarkFilterResult_status(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The spot version ID.
        /// </para>
        /// <para xml:lang="zh">
        /// 地点版本ID。
        /// </para>
        /// </summary>
        public virtual Optional<string> spotVersionId()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfString);
                Detail.easyar_MegaLandmarkFilterResult_spotVersionId(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.String_from_c(ar, p.value) : Optional<string>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Detailed exception message.
        /// </para>
        /// <para xml:lang="zh">
        /// 异常信息。
        /// </para>
        /// </summary>
        public virtual string exceptionInfo()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaLandmarkFilterResult_exceptionInfo(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// MegaLandmarkFilter implements cloud based visual positioning localization filtering.
    /// </para>
    /// <para xml:lang="zh">
    /// MegaLandmarkFilter实现了VPS云定位过滤功能。
    /// </para>
    /// </summary>
    internal class MegaLandmarkFilter : RefBase
    {
        internal MegaLandmarkFilter(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MegaLandmarkFilter(cdata_new, deleter_, retainer_);
        }
        public new MegaLandmarkFilter Clone()
        {
            return (MegaLandmarkFilter)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaLandmarkFilter_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance and connects to the server.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建并连接服务器。
        /// </para>
        /// </summary>
        public static MegaLandmarkFilter create(string server, string apiKey, string apiSecret, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaLandmarkFilter_create(Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiKey), Detail.String_to_c(ar, apiSecret), Detail.String_to_c(ar, appId), out _return_value_);
                return Detail.Object_from_c<MegaLandmarkFilter>(_return_value_, Detail.easyar_MegaLandmarkFilter__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with API Token.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用API Token创建。
        /// </para>
        /// </summary>
        public static MegaLandmarkFilter createWithToken(string server, string apiToken, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaLandmarkFilter_createWithToken(Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiToken), Detail.String_to_c(ar, appId), out _return_value_);
                return Detail.Object_from_c<MegaLandmarkFilter>(_return_value_, Detail.easyar_MegaLandmarkFilter__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Updates API Token.
        /// </para>
        /// <para xml:lang="zh">
        /// 更新API Token。
        /// </para>
        /// </summary>
        public virtual void updateToken(string apiToken)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaLandmarkFilter_updateToken(cdata, Detail.String_to_c(ar, apiToken));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Location filtering.
        /// Send GPS information to get SpotVersionId.
        /// </para>
        /// <para xml:lang="zh">
        /// 位置筛选。
        /// 输入GPS信息，返回SpotVersionId。
        /// </para>
        /// </summary>
        public virtual void filterByLocation(LocationResult gps, Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<MegaLandmarkFilterResult> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaLandmarkFilter_filterByLocation(cdata, gps, timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromMegaLandmarkFilterResult_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Filtering by SpotId.
        /// Returns SpotVersionId.
        /// </para>
        /// <para xml:lang="zh">
        /// 通过SpotId筛选。
        /// 返回SpotVersionId。
        /// </para>
        /// </summary>
        public virtual void filterBySpotId(string spotId, Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<MegaLandmarkFilterResult> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaLandmarkFilter_filterBySpotId(cdata, Detail.String_to_c(ar, spotId), timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromMegaLandmarkFilterResult_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Closes connection. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭连接。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaLandmarkFilter_close(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// The block instance localized by MegaTracker.
    /// </para>
    /// <para xml:lang="zh">
    /// MegaTracker定位到的block的实例。
    /// </para>
    /// </summary>
    internal class MegaTrackerBlockInstance : RefBase
    {
        internal MegaTrackerBlockInstance(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MegaTrackerBlockInstance(cdata_new, deleter_, retainer_);
        }
        public new MegaTrackerBlockInstance Clone()
        {
            return (MegaTrackerBlockInstance)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The appId of service.
        /// </para>
        /// <para xml:lang="zh">
        /// 服务appId。
        /// </para>
        /// </summary>
        public virtual string appId()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerBlockInstance_appId(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The ID of the block.
        /// </para>
        /// <para xml:lang="zh">
        /// 地图block的ID。
        /// </para>
        /// </summary>
        public virtual string blockId()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerBlockInstance_blockId(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The name of the block.
        /// </para>
        /// <para xml:lang="zh">
        /// 地图block的名称。
        /// </para>
        /// </summary>
        public virtual string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerBlockInstance_name(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The map block pose in the camera coordinates.
        /// </para>
        /// <para xml:lang="zh">
        /// 地图block在相机坐标系中的姿态。
        /// </para>
        /// </summary>
        public virtual Matrix44F pose()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTrackerBlockInstance_pose(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// The result of MegaTracker. Updated at the same frame rate with OutputFrame.
    /// </para>
    /// <para xml:lang="zh">
    /// MegaTracker的输出，会以OutputFrame的频率更新。
    /// </para>
    /// </summary>
    internal class MegaTrackerResult : FrameFilterResult
    {
        internal MegaTrackerResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MegaTrackerResult(cdata_new, deleter_, retainer_);
        }
        public new MegaTrackerResult Clone()
        {
            return (MegaTrackerResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current localized block instances. An existing instance will be preserved until another block is localized, or when MegaTracker is stopped or paused.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前定位到的block的实例。已有的实例会一直保持直到定位到另一个block，或者MegaTracker被stop或pause。
        /// </para>
        /// </summary>
        public virtual List<MegaTrackerBlockInstance> instances()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerResult_instances(cdata, out _return_value_);
                return Detail.ListOfMegaTrackerBlockInstance_from_c(ar, _return_value_);
            }
        }
    }

    public enum MegaTrackerLocalizationStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Unknown error
        /// </para>
        /// <para xml:lang="zh">
        /// 未知错误
        /// </para>
        /// </summary>
        UnknownError = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// A block is localized.
        /// </para>
        /// <para xml:lang="zh">
        /// 定位到block
        /// </para>
        /// </summary>
        Found = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// No blocks are localized.
        /// </para>
        /// <para xml:lang="zh">
        /// 没有定位到block
        /// </para>
        /// </summary>
        NotFound = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Request time out (more than 1 minute)
        /// </para>
        /// <para xml:lang="zh">
        /// 请求超时(超过1分钟)
        /// </para>
        /// </summary>
        RequestTimeout = 3,
        /// <summary>
        /// <para xml:lang="en">
        /// Request time interval is too low
        /// </para>
        /// <para xml:lang="zh">
        /// 请求间隔过短
        /// </para>
        /// </summary>
        RequestIntervalTooLow = 4,
        /// <summary>
        /// <para xml:lang="en">
        /// QPS limit exceeded
        /// </para>
        /// <para xml:lang="zh">
        /// QPS超过限制
        /// </para>
        /// </summary>
        QpsLimitExceeded = 5,
        /// <summary>
        /// <para xml:lang="en">
        /// Service is waking up
        /// </para>
        /// <para xml:lang="zh">
        /// 服务正在唤醒中
        /// </para>
        /// </summary>
        WakingUp = 6,
        /// <summary>
        /// <para xml:lang="en">
        /// SpotVersionId is missing. It is possible that the value is not set.
        /// </para>
        /// <para xml:lang="zh">
        /// 缺少SpotVersionId。可能是没有设置。
        /// </para>
        /// </summary>
        MissingSpotVersionId = 7,
        /// <summary>
        /// <para xml:lang="en">
        /// API Token expired.
        /// </para>
        /// <para xml:lang="zh">
        /// API Token过期。
        /// </para>
        /// </summary>
        ApiTokenExpired = 8,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// The response of MegaTracker localization request.
    /// </para>
    /// <para xml:lang="zh">
    /// MegaTracker定位请求的响应。
    /// </para>
    /// </summary>
    internal class MegaTrackerLocalizationResponse : RefBase
    {
        internal MegaTrackerLocalizationResponse(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MegaTrackerLocalizationResponse(cdata_new, deleter_, retainer_);
        }
        public new MegaTrackerLocalizationResponse Clone()
        {
            return (MegaTrackerLocalizationResponse)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The input frame on request.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求时使用的输入帧。
        /// </para>
        /// </summary>
        public virtual InputFrame inputFrame()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerLocalizationResponse_inputFrame(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrame>(_return_value_, Detail.easyar_InputFrame__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The accelerometer reading on request.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求时使用的加速度计读数。
        /// </para>
        /// </summary>
        public virtual Optional<AccelerometerResult> acceleration()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTrackerLocalizationResponse_acceleration(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<AccelerometerResult>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The location reading on request.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求时使用的位置读数。
        /// </para>
        /// </summary>
        public virtual Optional<LocationResult> location()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTrackerLocalizationResponse_location(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<LocationResult>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Localization status.
        /// </para>
        /// <para xml:lang="zh">
        /// 定位状态。
        /// </para>
        /// </summary>
        public virtual MegaTrackerLocalizationStatus status()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTrackerLocalizationResponse_status(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The spot version ID.
        /// </para>
        /// <para xml:lang="zh">
        /// 地点版本ID。
        /// </para>
        /// </summary>
        public virtual Optional<string> spotVersionId()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfString);
                Detail.easyar_MegaTrackerLocalizationResponse_spotVersionId(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.String_from_c(ar, p.value) : Optional<string>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current localized block instances. An existing instance will be preserved until another block is localized, or when MegaTracker is stopped or paused.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前定位到的block的实例。已有的实例会一直保持直到定位到另一个block，或者MegaTracker被stop或pause。
        /// </para>
        /// </summary>
        public virtual List<MegaTrackerBlockInstance> instances()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerLocalizationResponse_instances(cdata, out _return_value_);
                return Detail.ListOfMegaTrackerBlockInstance_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The appId of service.
        /// </para>
        /// <para xml:lang="zh">
        /// 服务appId。
        /// </para>
        /// </summary>
        public virtual string appId()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerLocalizationResponse_appId(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The duration in seconds for server response.
        /// </para>
        /// <para xml:lang="zh">
        /// 服务器响应耗时（秒）。
        /// </para>
        /// </summary>
        public virtual Optional<double> serverResponseDuration()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTrackerLocalizationResponse_serverResponseDuration(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<double>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The duration in seconds for server internal calculation.
        /// </para>
        /// <para xml:lang="zh">
        /// 服务器内部计算耗时（秒）。
        /// </para>
        /// </summary>
        public virtual Optional<double> serverCalculationDuration()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTrackerLocalizationResponse_serverCalculationDuration(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<double>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Error message. It is filled when status is UnknownError.
        /// </para>
        /// <para xml:lang="zh">
        /// 错误信息。当status为UnknownError时有值。
        /// </para>
        /// </summary>
        public virtual Optional<string> errorMessage()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfString);
                Detail.easyar_MegaTrackerLocalizationResponse_errorMessage(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.String_from_c(ar, p.value) : Optional<string>.Empty);
            }
        }
        public virtual string extraInfo()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTrackerLocalizationResponse_extraInfo(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Provides cloud based localization.
    /// MegaTracker occupies 1 buffers of camera.
    /// </para>
    /// <para xml:lang="zh">
    /// 提供云定位功能。
    /// MegaTracker占用1个camera的buffer。
    /// </para>
    /// </summary>
    internal class MegaTracker : RefBase
    {
        internal MegaTracker(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MegaTracker(cdata_new, deleter_, retainer_);
        }
        public new MegaTracker Clone()
        {
            return (MegaTracker)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Check whether SparseSpatialMap is is available, always return true.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查MegaTracker是否可用。总是返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTracker_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port for input frame. For MegaTracker to work, the inputFrame must include camera parameters, timestamp and spatial information. See also `InputFrameSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 输入帧输入端口。MegaTracker输入帧必须包含camera参数、时间戳信息和空间信息（cameraTransform和trackingStatus）。参考 `InputFrameSink`_ 。
        /// </para>
        /// </summary>
        public virtual InputFrameSink inputFrameSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTracker_inputFrameSink(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port for accelerometer result. See also `AccelerometerResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 加速度计结果输入端口。参考 `AccelerometerResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual AccelerometerResultSink accelerometerResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTracker_accelerometerResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<AccelerometerResultSink>(_return_value_, Detail.easyar_AccelerometerResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for location result. See also `LocationResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。位置结果输入端口。参考 `LocationResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual LocationResultSink locationResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTracker_locationResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<LocationResultSink>(_return_value_, Detail.easyar_LocationResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for proximity location result. See also `ProximityLocationResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。邻近位置结果输入端口。参考 `ProximityLocationResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual ProximityLocationResultSink proximityLocationResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTracker_proximityLocationResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<ProximityLocationResultSink>(_return_value_, Detail.easyar_ProximityLocationResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTracker_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for output frame. See also `OutputFrameSource`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 输出帧输出端口。参考 `OutputFrameSource`_ 。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource outputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTracker_outputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static MegaTracker create(string server, string apiKey, string apiSecret, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTracker_create(Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiKey), Detail.String_to_c(ar, apiSecret), Detail.String_to_c(ar, appId), out _return_value_);
                return Detail.Object_from_c<MegaTracker>(_return_value_, Detail.easyar_MegaTracker__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with API Token.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用API Token创建。
        /// </para>
        /// </summary>
        public static MegaTracker createWithToken(string server, string apiToken, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MegaTracker_createWithToken(Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiToken), Detail.String_to_c(ar, appId), out _return_value_);
                return Detail.Object_from_c<MegaTracker>(_return_value_, Detail.easyar_MegaTracker__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Updates API Token.
        /// </para>
        /// <para xml:lang="zh">
        /// 更新API Token。
        /// </para>
        /// </summary>
        public virtual void updateToken(string apiToken)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_updateToken(cdata, Detail.String_to_c(ar, apiToken));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets MEGA API Type. The default is Block.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置MEGA API类型。默认为Block
        /// </para>
        /// </summary>
        public virtual void setMegaApiType(MegaApiType type)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setMegaApiType(cdata, type);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the spot version ID. Only available with `MegaApiType`_.Landmark .
        /// </para>
        /// <para xml:lang="zh">
        /// 设置地点版本ID。仅当开启 `MegaApiType`_.Landmark 时可用。
        /// </para>
        /// </summary>
        public virtual void setSpotVersionId(Optional<string> spotVersionId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setSpotVersionId(cdata, spotVersionId.map(p => p.OnSome ? new Detail.OptionalOfString { has_value = true, value = Detail.String_to_c(ar, p.Value) } : new Detail.OptionalOfString { has_value = false, value = default(IntPtr) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Switches remote end point.
        /// </para>
        /// <para xml:lang="zh">
        /// 切换远端端点。
        /// </para>
        /// </summary>
        public virtual void switchEndPoint(string server, string apiKey, string apiSecret, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_switchEndPoint(cdata, Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiKey), Detail.String_to_c(ar, apiSecret), Detail.String_to_c(ar, appId));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Switches remote end point with API Token.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用API Token切换远端端点。
        /// </para>
        /// </summary>
        public virtual void switchEndPointWithToken(string server, string apiToken, string appId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_switchEndPointWithToken(cdata, Detail.String_to_c(ar, server), Detail.String_to_c(ar, apiToken), Detail.String_to_c(ar, appId));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Binds to a specific block prior. An automatic unbind will be performed during reset or end point switching.
        /// </para>
        /// <para xml:lang="zh">
        /// 绑定到特定的地图block。重置或切换远端端点时会自动解绑。
        /// </para>
        /// </summary>
        public virtual void bindBlockPrior(BlockPriorResult blockPrior)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_bindBlockPrior(cdata, blockPrior.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Release bind to any block.
        /// </para>
        /// <para xml:lang="zh">
        /// 解除地图block绑定。
        /// </para>
        /// </summary>
        public virtual void unbindBlockPrior()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_unbindBlockPrior(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets request time parameters. timeoutMilliseconds is connection timeout. requestIntervalMilliseconds is the expected request interval, and the default value is 1000 and the minimum value is 300, with a longer value results a larger overall error.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置请求时间参数。timeoutMilliseconds为连接超时时间。requestIntervalMilliseconds为期望的请求间隔时间，默认为1000，最小为300，值越大整体误差越大。
        /// </para>
        /// </summary>
        public virtual void setRequestTimeParameters(Optional<int> timeoutMilliseconds, int requestIntervalMilliseconds)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setRequestTimeParameters(cdata, timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), requestIntervalMilliseconds);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets whether to enable localzation. If disabled, localization result from server response will not be in the output. Defaults to true.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置是否启用定位。不启用定位时，服务器返回的定位结果不会输出。默认为true。
        /// </para>
        /// </summary>
        public virtual void setEnableLocalization(bool enableLocalization)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setEnableLocalization(cdata, enableLocalization);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets whether to enable stateless localzation. If enabled, previous result will not be used in localization. Defaults to false.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置是否启用无状态定位。启用无状态定位时，定位时不会使用上一次定位结果。默认为false。
        /// </para>
        /// </summary>
        public virtual void setEnableStatelessLocalization(bool enableStatelessLocalization)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setEnableStatelessLocalization(cdata, enableStatelessLocalization);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets result async mode. Set true If results are to be outputted via outputFrameSource, false if results are to be fetched via getSyncResult. enableAsync defaults to true. Sync mode is only available with a XR license.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置结果异步模式。如果结果通过outputFrameSource输出，则应设置位true，如果结果通过getSyncResult获取，则应设置为false。默认enableAsync为true。同步模式仅使用XR License时可生效。
        /// </para>
        /// </summary>
        public virtual bool setResultAsyncMode(bool enableAsync)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTracker_setResultAsyncMode(cdata, enableAsync);
                return _return_value_;
            }
        }
        public virtual void setRequestMessage(string message)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setRequestMessage(cdata, Detail.String_to_c(ar, message));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets localization callback. On every localization, the callback will be triggered.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置定位回调。每次定位时，会触发此回调。
        /// </para>
        /// </summary>
        public virtual void setLocalizationCallback(CallbackScheduler callbackScheduler, Optional<Action<MegaTrackerLocalizationResponse>> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setLocalizationCallback(cdata, callbackScheduler.cdata, callback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromMegaTrackerLocalizationResponse { has_value = true, value = Detail.FunctorOfVoidFromMegaTrackerLocalizationResponse_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromMegaTrackerLocalizationResponse { has_value = false, value = default(Detail.FunctorOfVoidFromMegaTrackerLocalizationResponse) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the override value for camera position from map block. When set, MegaTracker will output override value instead of the position in input frame. This option can be utilized to improve output pose precision when there is known map info but limited sensor capability.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置相机位置相对于地图block的覆盖值。设置之后，MegaTracker会输出覆盖值，而不使用输入帧数据中的对应坐标。可用于在已知地图信息且传感器有限的情况下提高输出位姿的精度。
        /// </para>
        /// </summary>
        public virtual void setCameraPositionOverride(Optional<float> x, Optional<float> y, Optional<float> z)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_setCameraPositionOverride(cdata, x.map(p => p.OnSome ? new Detail.OptionalOfFloat { has_value = true, value = p.Value } : new Detail.OptionalOfFloat { has_value = false, value = default(float) }), y.map(p => p.OnSome ? new Detail.OptionalOfFloat { has_value = true, value = p.Value } : new Detail.OptionalOfFloat { has_value = false, value = default(float) }), z.map(p => p.OnSome ? new Detail.OptionalOfFloat { has_value = true, value = p.Value } : new Detail.OptionalOfFloat { has_value = false, value = default(float) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Start MegaTracker.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始MegaTracker。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MegaTracker_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stop MegaTracker. Call start() to resume running.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止MegaTracker。调用start重新运行。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Reset MegaTracker and clear all internal states.
        /// </para>
        /// <para xml:lang="zh">
        /// 重置MegaTracker，清除所有状态。
        /// </para>
        /// </summary>
        public virtual void reset()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_reset(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close MegaTracker. MegaTracker can no longer be used.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭MegaTracker。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MegaTracker_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get synchronized result. If MegaTracker is paused, or the result async mode is not set to false through setResultAsyncMode, the return value is empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得同步输出结果。如果MegaTracker已暂停，或者未通过setResultAsyncMode将结果异步模式设为false，则返回值为空。
        /// </para>
        /// </summary>
        public virtual Optional<MegaTrackerResult> getSyncResult(MotionInputData motionInputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfMegaTrackerResult);
                Detail.easyar_MegaTracker_getSyncResult(cdata, motionInputData.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<MegaTrackerResult>(p.value, Detail.easyar_MegaTrackerResult__typeName) : Optional<MegaTrackerResult>.Empty);
            }
        }
    }

    public enum CloudRecognizationStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Unknown error
        /// </para>
        /// <para xml:lang="zh">
        /// 未知错误
        /// </para>
        /// </summary>
        UnknownError = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// A target is recognized.
        /// </para>
        /// <para xml:lang="zh">
        /// 识别到target
        /// </para>
        /// </summary>
        FoundTarget = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// No target is recognized.
        /// </para>
        /// <para xml:lang="zh">
        /// 未识别到target
        /// </para>
        /// </summary>
        TargetNotFound = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Reached the access limit
        /// </para>
        /// <para xml:lang="zh">
        /// 到达访问额度
        /// </para>
        /// </summary>
        ReachedAccessLimit = 3,
        /// <summary>
        /// <para xml:lang="en">
        /// Request interval too low
        /// </para>
        /// <para xml:lang="zh">
        /// 请求间隔过低
        /// </para>
        /// </summary>
        RequestIntervalTooLow = 4,
        /// <summary>
        /// <para xml:lang="en">
        /// API Token expired.
        /// </para>
        /// <para xml:lang="zh">
        /// API Token过期。
        /// </para>
        /// </summary>
        ApiTokenExpired = 5,
    }

    internal class CloudRecognizationResult : RefBase
    {
        internal CloudRecognizationResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CloudRecognizationResult(cdata_new, deleter_, retainer_);
        }
        public new CloudRecognizationResult Clone()
        {
            return (CloudRecognizationResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns recognition status.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得识别状态。
        /// </para>
        /// </summary>
        public virtual CloudRecognizationStatus getStatus()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CloudRecognizationResult_getStatus(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the recognized target when status is FoundTarget.
        /// </para>
        /// <para xml:lang="zh">
        /// 在识别状态为FoundTarget时，获得识别到的target。
        /// </para>
        /// </summary>
        public virtual Optional<ImageTarget> getTarget()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfImageTarget);
                Detail.easyar_CloudRecognizationResult_getTarget(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ImageTarget>(p.value, Detail.easyar_ImageTarget__typeName) : Optional<ImageTarget>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the error message when status is UnknownError.
        /// </para>
        /// <para xml:lang="zh">
        /// 在识别状态为UnknownError时，获得错误信息。
        /// </para>
        /// </summary>
        public virtual Optional<string> getUnknownErrorMessage()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfString);
                Detail.easyar_CloudRecognizationResult_getUnknownErrorMessage(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.String_from_c(ar, p.value) : Optional<string>.Empty);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// CloudRecognizer implements cloud recognition. It can only be used after created a recognition image library on the cloud. Please refer to EasyAR CRS documentation.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// Before using a CloudRecognizer, an `ImageTracker`_ must be setup and prepared. Any target returned from cloud should be manually put into the `ImageTracker`_ using `ImageTracker.loadTarget`_ if it need to be tracked. Then the target can be used as same as a local target after loaded into the tracker. When a target is recognized, you can get it from callback, and you should use target uid to distinguish different targets. The target runtimeID is dynamically created and cannot be used as unique identifier in the cloud situation.
    /// </para>
    /// <para xml:lang="zh">
    /// CloudRecognizer实现了云识别功能。云识别功能需要在云端创建云识别图库才能使用，请参考EasyAR CRS文档。
    /// 当不再需要该组件时，可以调用close对其进行关闭。close之后不应继续使用。
    /// 在使用CloudRecognizer之前，需要设置并准备好一个 `ImageTracker`_ 。任何返回的target在被track之前都应使用 `ImageTracker.loadTarget`_ 手动加载进入 `ImageTracker`_ 。加载之后，target的识别和跟踪即和本地target的使用相同。在一个target被识别到之后，你可以从回调中获取到，然后你应该使用target uid来区分不同的target。target runtimeID是动态生成的，不适用于作为云识别情况下的target的唯一区分。
    /// </para>
    /// </summary>
    internal class CloudRecognizer : RefBase
    {
        internal CloudRecognizer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CloudRecognizer(cdata_new, deleter_, retainer_);
        }
        public new CloudRecognizer Clone()
        {
            return (CloudRecognizer)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CloudRecognizer_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance and connects to the server.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建并连接服务器。
        /// </para>
        /// </summary>
        public static CloudRecognizer create(string cloudRecognitionServiceServerAddress, string apiKey, string apiSecret, string cloudRecognitionServiceAppId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudRecognizer_create(Detail.String_to_c(ar, cloudRecognitionServiceServerAddress), Detail.String_to_c(ar, apiKey), Detail.String_to_c(ar, apiSecret), Detail.String_to_c(ar, cloudRecognitionServiceAppId), out _return_value_);
                return Detail.Object_from_c<CloudRecognizer>(_return_value_, Detail.easyar_CloudRecognizer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with API Token and connects to the server.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用API Token创建并连接服务器。
        /// </para>
        /// </summary>
        public static CloudRecognizer createWithToken(string cloudRecognitionServiceServerAddress, string apiToken, string cloudRecognitionServiceAppId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudRecognizer_createWithToken(Detail.String_to_c(ar, cloudRecognitionServiceServerAddress), Detail.String_to_c(ar, apiToken), Detail.String_to_c(ar, cloudRecognitionServiceAppId), out _return_value_);
                return Detail.Object_from_c<CloudRecognizer>(_return_value_, Detail.easyar_CloudRecognizer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance and connects to the server with Cloud Secret.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用Cloud Secret创建并连接服务器。
        /// </para>
        /// </summary>
        public static CloudRecognizer createByCloudSecret(string cloudRecognitionServiceServerAddress, string cloudRecognitionServiceSecret, string cloudRecognitionServiceAppId)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CloudRecognizer_createByCloudSecret(Detail.String_to_c(ar, cloudRecognitionServiceServerAddress), Detail.String_to_c(ar, cloudRecognitionServiceSecret), Detail.String_to_c(ar, cloudRecognitionServiceAppId), out _return_value_);
                return Detail.Object_from_c<CloudRecognizer>(_return_value_, Detail.easyar_CloudRecognizer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Updates API Token.
        /// </para>
        /// <para xml:lang="zh">
        /// 更新API Token。
        /// </para>
        /// </summary>
        public virtual void updateToken(string apiToken)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudRecognizer_updateToken(cdata, Detail.String_to_c(ar, apiToken));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Send recognition request. The lowest available request interval is 300ms.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求识别。可用最低请求间隔为300ms。
        /// </para>
        /// </summary>
        public virtual void resolve(InputFrame inputFrame, Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<CloudRecognizationResult> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudRecognizer_resolve(cdata, inputFrame.cdata, timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromCloudRecognizationResult_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops the recognition and closes connection. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止识别并关闭连接。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CloudRecognizer_close(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Buffer stores a raw byte array, which can be used to access image data.
    /// To access image data in Java API, get buffer from `Image`_ and copy to a Java byte array.
    /// You can always access image data since the first version of EasyAR Sense. Refer to `Image`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// Buffer 存储了原始字节数组，可以用来访问图像数据。
    /// 在Java API中可以从 `Image`_ 中获取buffer然后copy数据到Java字节数组。
    /// 在EasyAR Sense的所有版本中，你都可以访问图像数据。参考 `Image`_ 。
    /// </para>
    /// </summary>
    public class Buffer : RefBase
    {
        internal Buffer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new Buffer(cdata_new, deleter_, retainer_);
        }
        public new Buffer Clone()
        {
            return (Buffer)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Wraps a raw memory block. When Buffer is released by all holders, deleter callback will be invoked to execute user-defined memory destruction. deleter must be thread-safe.
        /// </para>
        /// <para xml:lang="zh">
        /// 包装一个指定长度的原始内存块。在Buffer被完全释放的时候，会调用deleter回调，执行用户自定义内存销毁行为。deleter必须是线程安全的。
        /// </para>
        /// </summary>
        public static Buffer wrap(IntPtr ptr, int size, Action deleter)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Buffer_wrap(ptr, size, Detail.FunctorOfVoid_to_c(deleter), out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a Buffer of specified byte size.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建一个指定字节长度的Buffer。
        /// </para>
        /// </summary>
        public static Buffer create(int size)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Buffer_create(size, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns raw data address.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回原始内存地址。
        /// </para>
        /// </summary>
        public virtual IntPtr data()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Buffer_data(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Byte size of raw data.
        /// </para>
        /// <para xml:lang="zh">
        /// Buffer的字节长度。
        /// </para>
        /// </summary>
        public virtual int size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Buffer_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Copies raw memory. It can be used in languages or platforms without complete support for memory operations.
        /// </para>
        /// <para xml:lang="zh">
        /// 复制原始内存。主要用于内存操作不完善的语言或环境。
        /// </para>
        /// </summary>
        public static void memoryCopy(IntPtr src, IntPtr dest, int length)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Buffer_memoryCopy(src, dest, length);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Tries to copy data from a raw memory address into Buffer. If copy succeeds, it returns true, or else it returns false. Possible failure causes includes: source or destination data range overflow.
        /// </para>
        /// <para xml:lang="zh">
        /// 尝试从原始内存地址复制数据到Buffer中。如果复制成功，则返回true，否则返回false。失败的原因有：源数据范围或目标数据范围超出可用范围。
        /// </para>
        /// </summary>
        public virtual bool tryCopyFrom(IntPtr src, int srcIndex, int index, int length)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Buffer_tryCopyFrom(cdata, src, srcIndex, index, length);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Tries to copy data from Buffer to user array. If copy succeeds, it returns true, or else it returns false. Possible failure causes includes: source or destination data range overflow.
        /// </para>
        /// <para xml:lang="zh">
        /// 尝试从Buffer复制数据到原始内存地址中。如果复制成功，则返回true，否则返回false。失败的原因有：源数据范围或目标数据范围超出可用范围。
        /// </para>
        /// </summary>
        public virtual bool tryCopyTo(int index, IntPtr dest, int destIndex, int length)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Buffer_tryCopyTo(cdata, index, dest, destIndex, length);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a sub-buffer with a reference to the original Buffer. A Buffer will only be released after all its sub-buffers are released.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建一个子Buffer，并引用原Buffer。一个Buffer在所有子Buffer释放后才会释放。
        /// </para>
        /// </summary>
        public virtual Buffer partition(int index, int length)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Buffer_partition(cdata, index, length, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        public static Buffer wrapByteArray(byte[] bytes)
        {
            var Length = bytes.Length;
            var h = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            return Buffer.wrap(h.AddrOfPinnedObject(), Length, () => h.Free());
        }
        public static Buffer wrapByteArray(byte[] bytes, int index, int length)
        {
            return wrapByteArray(bytes, index, length, () => { });
        }
        public static Buffer wrapByteArray(byte[] bytes, int index, int length, Action deleter)
        {
            if ((length < 0) || (index < 0) || (index > bytes.Length) || (index + length > bytes.Length))
            {
                throw new ArgumentException("BufferRangeOverflow");
            }
            var h = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var ptr = new IntPtr(h.AddrOfPinnedObject().ToInt64() + index);
            return Buffer.wrap(ptr, length, () =>
            {
                h.Free();
                if (deleter != null)
                {
                    deleter();
                }
            });
        }
        public void copyFromByteArray(byte[] src)
        {
            copyFromByteArray(src, 0, 0, src.Length);
        }
        public void copyFromByteArray(byte[] src, int srcIndex, int index, int length)
        {
            var srcSize = src.Length;
            var destSize = size();
            if ((length < 0) || (srcIndex < 0) || (srcIndex > srcSize) || (srcIndex + length > srcSize) || (index < 0) || (index > destSize) || (index + length > destSize))
            {
                throw new ArgumentException("BufferRangeOverflow");
            }
            Marshal.Copy(src, srcIndex, data(), length);
        }
        public void copyToByteArray(byte[] dest)
        {
            copyToByteArray(0, dest, 0, size());
        }
        public void copyToByteArray(int index, byte[] dest, int destIndex, int length)
        {
            var srcSize = size();
            var destSize = dest.Length;
            if ((length < 0) || (index < 0) || (index > srcSize) || (index + length > srcSize) || (destIndex < 0) || (destIndex > destSize) || (destIndex + length > destSize))
            {
                throw new ArgumentException("BufferRangeOverflow");
            }
            var ptr = new IntPtr(data().ToInt64() + index);
            Marshal.Copy(ptr, dest, destIndex, length);
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// A mapping from file path to `Buffer`_ . It can be used to represent multiple files in the memory.
    /// </para>
    /// <para xml:lang="zh">
    /// 一个从文件路径到 `Buffer`_ 的映射。用于表示多个放在内存中的文件。
    /// </para>
    /// </summary>
    public class BufferDictionary : RefBase
    {
        internal BufferDictionary(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new BufferDictionary(cdata_new, deleter_, retainer_);
        }
        public new BufferDictionary Clone()
        {
            return (BufferDictionary)(CloneObject());
        }
        public BufferDictionary() : base(IntPtr.Zero, Detail.easyar_BufferDictionary__dtor, Detail.easyar_BufferDictionary__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_BufferDictionary__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current file count.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前文件数量。
        /// </para>
        /// </summary>
        public virtual int count()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_BufferDictionary_count(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if a specified path is in the dictionary.
        /// </para>
        /// <para xml:lang="zh">
        /// 确定指定路径是否在字典中。
        /// </para>
        /// </summary>
        public virtual bool contains(string path)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_BufferDictionary_contains(cdata, Detail.String_to_c(ar, path));
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Tries to get the corresponding `Buffer`_ for a specified path.
        /// </para>
        /// <para xml:lang="zh">
        /// 尝试获得指定路径对应的 `Buffer`_ 。
        /// </para>
        /// </summary>
        public virtual Optional<Buffer> tryGet(string path)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfBuffer);
                Detail.easyar_BufferDictionary_tryGet(cdata, Detail.String_to_c(ar, path), out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<Buffer>(p.value, Detail.easyar_Buffer__typeName) : Optional<Buffer>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `Buffer`_ for a specified path.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置指定路径对应的 `Buffer`_ 。
        /// </para>
        /// </summary>
        public virtual void @set(string path, Buffer buffer)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_BufferDictionary_set(cdata, Detail.String_to_c(ar, path), buffer.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Removes a specified path.
        /// </para>
        /// <para xml:lang="zh">
        /// 移除指定的路径。
        /// </para>
        /// </summary>
        public virtual bool remove(string path)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_BufferDictionary_remove(cdata, Detail.String_to_c(ar, path));
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Clears the dictionary.
        /// </para>
        /// <para xml:lang="zh">
        /// 清空字典。
        /// </para>
        /// </summary>
        public virtual void clear()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_BufferDictionary_clear(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// BufferPool is a memory pool to reduce memory allocation time consumption for functionality like custom camera interoperability, which needs to allocate memory buffers of a fixed size repeatedly.
    /// </para>
    /// <para xml:lang="zh">
    /// BufferPool 实现了一个内存池，可用于自定义摄像头接入等需要反复分配相同大小内存的功能，降低内存分配耗时。
    /// </para>
    /// </summary>
    public class BufferPool : RefBase
    {
        internal BufferPool(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new BufferPool(cdata_new, deleter_, retainer_);
        }
        public new BufferPool Clone()
        {
            return (BufferPool)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// block_size is the byte size of each `Buffer`_ .
        /// capacity is the maximum count of `Buffer`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// block_size为每个 `Buffer`_ 的字节大小。
        /// capacity为最大 `Buffer`_ 数量。
        /// </para>
        /// </summary>
        public BufferPool(int block_size, int capacity) : base(IntPtr.Zero, Detail.easyar_BufferPool__dtor, Detail.easyar_BufferPool__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_BufferPool__ctor(block_size, capacity, out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The byte size of each `Buffer`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 每个 `Buffer`_ 的字节大小。
        /// </para>
        /// </summary>
        public virtual int block_size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_BufferPool_block_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The maximum count of `Buffer`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 最大 `Buffer`_ 数量。
        /// </para>
        /// </summary>
        public virtual int capacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_BufferPool_capacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current acquired count of `Buffer`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 当前外部获得的 `Buffer`_ 数量。
        /// </para>
        /// </summary>
        public virtual int size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_BufferPool_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Tries to acquire a memory block. If current acquired count of `Buffer`_ does not reach maximum, a new `Buffer`_ is fetched or allocated, or else null is returned.
        /// </para>
        /// <para xml:lang="zh">
        /// 尝试获得内存块。如果当前外部获得的 `Buffer`_ 数量没有达到最大 `Buffer`_ 数量，则取出或分配一个新的 `Buffer`_ ，否则返回空。
        /// </para>
        /// </summary>
        public virtual Optional<Buffer> tryAcquire()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfBuffer);
                Detail.easyar_BufferPool_tryAcquire(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<Buffer>(p.value, Detail.easyar_Buffer__typeName) : Optional<Buffer>.Empty);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// PixelFormat represents the format of image pixel data. All formats follow the pixel direction from left to right and from top to bottom.
    /// </para>
    /// <para xml:lang="zh">
    /// PixelFormat表示图像像素格式。所有格式的像素方向均为从左到右，从上到下的。
    /// </para>
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Unknown
        /// </para>
        /// <para xml:lang="zh">
        /// 未知
        /// </para>
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// 256 shades grayscale
        /// </para>
        /// <para xml:lang="zh">
        /// 256阶灰度
        /// </para>
        /// </summary>
        Gray = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// YUV_NV21
        /// </para>
        /// <para xml:lang="zh">
        /// YUV_NV21
        /// </para>
        /// </summary>
        YUV_NV21 = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// YUV_NV12
        /// </para>
        /// <para xml:lang="zh">
        /// YUV_NV12
        /// </para>
        /// </summary>
        YUV_NV12 = 3,
        /// <summary>
        /// <para xml:lang="en">
        /// YUV_I420
        /// </para>
        /// <para xml:lang="zh">
        /// YUV_I420
        /// </para>
        /// </summary>
        YUV_I420 = 4,
        /// <summary>
        /// <para xml:lang="en">
        /// YUV_YV12
        /// </para>
        /// <para xml:lang="zh">
        /// YUV_YV12
        /// </para>
        /// </summary>
        YUV_YV12 = 5,
        /// <summary>
        /// <para xml:lang="en">
        /// RGB888
        /// </para>
        /// <para xml:lang="zh">
        /// RGB888
        /// </para>
        /// </summary>
        RGB888 = 6,
        /// <summary>
        /// <para xml:lang="en">
        /// BGR888
        /// </para>
        /// <para xml:lang="zh">
        /// BGR888
        /// </para>
        /// </summary>
        BGR888 = 7,
        /// <summary>
        /// <para xml:lang="en">
        /// RGBA8888
        /// </para>
        /// <para xml:lang="zh">
        /// RGBA8888
        /// </para>
        /// </summary>
        RGBA8888 = 8,
        /// <summary>
        /// <para xml:lang="en">
        /// BGRA8888
        /// </para>
        /// <para xml:lang="zh">
        /// BGRA8888
        /// </para>
        /// </summary>
        BGRA8888 = 9,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Image stores an image data and represents an image in memory.
    /// Image raw data can be accessed as byte array. The width/height/etc information are also accessible.
    /// You can always access image data since the first version of EasyAR Sense.
    ///
    /// You can do this in iOS
    /// ::
    ///
    ///     #import &lt;easyar/buffer.oc.h&gt;
    ///     #import &lt;easyar/image.oc.h&gt;
    ///
    ///     easyar_OutputFrame * outputFrame = [outputFrameBuffer peek];
    ///     if (outputFrame != nil) {
    ///         easyar_Image * i = [[outputFrame inputFrame] image];
    ///         easyar_Buffer * b = [i buffer];
    ///         char * bytes = calloc([b size], 1);
    ///         memcpy(bytes, [b data], [b size]);
    ///         // use bytes here
    ///         free(bytes);
    ///     }
    ///
    /// Or in Android
    /// ::
    ///
    ///     import cn.easyar.*;
    ///
    ///     OutputFrame outputFrame = outputFrameBuffer.peek();
    ///     if (outputFrame != null) {
    ///         InputFrame inputFrame = outputFrame.inputFrame();
    ///         Image i = inputFrame.image();
    ///         Buffer b = i.buffer();
    ///         byte[] bytes = new byte[b.size()];
    ///         b.copyToByteArray(0, bytes, 0, bytes.length);
    ///         // use bytes here
    ///         b.dispose();
    ///         i.dispose();
    ///         inputFrame.dispose();
    ///         outputFrame.dispose();
    ///     }
    /// </para>
    /// <para xml:lang="zh">
    /// Image存储了图像数据，用来表示内存中的图像。
    /// Image以字节数组的方式提供了对原始数据的访问，同时也提供了访问width/height等信息的接口。
    /// 在EasyAR Sense的所有版本中，你都可以访问图像数据。
    ///
    /// 在iOS中可以这样访问
    /// ::
    ///
    ///     #import &lt;easyar/buffer.oc.h&gt;
    ///     #import &lt;easyar/image.oc.h&gt;
    ///
    ///     easyar_OutputFrame * outputFrame = [outputFrameBuffer peek];
    ///     if (outputFrame != nil) {
    ///         easyar_Image * i = [[outputFrame inputFrame] image];
    ///         easyar_Buffer * b = [i buffer];
    ///         char * bytes = calloc([b size], 1);
    ///         memcpy(bytes, [b data], [b size]);
    ///         // use bytes here
    ///         free(bytes);
    ///     }
    ///
    /// 在Android里面，
    /// ::
    ///
    ///     import cn.easyar.*;
    ///
    ///     OutputFrame outputFrame = outputFrameBuffer.peek();
    ///     if (outputFrame != null) {
    ///         InputFrame inputFrame = outputFrame.inputFrame();
    ///         Image i = inputFrame.image();
    ///         Buffer b = i.buffer();
    ///         byte[] bytes = new byte[b.size()];
    ///         b.copyToByteArray(0, bytes, 0, bytes.length);
    ///         // use bytes here
    ///         b.dispose();
    ///         i.dispose();
    ///         inputFrame.dispose();
    ///         outputFrame.dispose();
    ///     }
    /// </para>
    /// </summary>
    public class Image : RefBase
    {
        internal Image(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new Image(cdata_new, deleter_, retainer_);
        }
        public new Image Clone()
        {
            return (Image)(CloneObject());
        }
        public Image(Buffer buffer, PixelFormat format, int width, int height) : base(IntPtr.Zero, Detail.easyar_Image__dtor, Detail.easyar_Image__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_Image__ctor(buffer.cdata, format, width, height, out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns buffer inside image. It can be used to access internal data of image. The content of `Buffer`_ shall not be modified, as they may be accessed from other threads.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回图像中的数据buffer。可以使用 `Buffer`_ API访问内部数据。不应对获得的数据 `Buffer`_ 的内容进行修改，因为这些内容可能在其他线程被使用。
        /// </para>
        /// </summary>
        public virtual Buffer buffer()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Image_buffer(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns image format.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回图像格式。
        /// </para>
        /// </summary>
        public virtual PixelFormat format()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Image_format(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns image width. There is a padding of (pixelWidth - width) pixels at the right side of the image.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回图像宽度。图像数据的右方会有 pixelWidth - width 像素的padding。
        /// </para>
        /// </summary>
        public virtual int width()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Image_width(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns image height. There is a padding of (pixelHeight - height) pixels at the bottom of the image.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回图像高度。图像数据的下方会有 pixelHeight - height 像素的padding。
        /// </para>
        /// </summary>
        public virtual int height()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Image_height(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns image pixel width for encoding.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回图像编码时使用的像素宽度。
        /// </para>
        /// </summary>
        public virtual int pixelWidth()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Image_pixelWidth(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns image pixel height for encoding.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回图像编码时使用的像素高度。
        /// </para>
        /// </summary>
        public virtual int pixelHeight()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Image_pixelHeight(cdata);
                return _return_value_;
            }
        }
        public static Image create(Buffer buffer, PixelFormat format, int width, int height, int pixelWidth, int pixelHeight)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Image_create(buffer.cdata, format, width, height, pixelWidth, pixelHeight, out _return_value_);
                return Detail.Object_from_c<Image>(_return_value_, Detail.easyar_Image__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Square matrix of 4. The data arrangement is row-major.
    /// </para>
    /// <para xml:lang="zh">
    /// 四阶方阵。数据的排列方式为row-major。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix44F
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of matrix.
        /// </para>
        /// <para xml:lang="zh">
        /// 矩阵的原始数据数组。
        /// </para>
        /// </summary>
        public float[] data
        {
            get
            {
                return new float[] { data_0, data_1, data_2, data_3, data_4, data_5, data_6, data_7, data_8, data_9, data_10, data_11, data_12, data_13, data_14, data_15 };
            }
            set
            {
                if (value.Length != 16) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
                this.data_2 = value[2];
                this.data_3 = value[3];
                this.data_4 = value[4];
                this.data_5 = value[5];
                this.data_6 = value[6];
                this.data_7 = value[7];
                this.data_8 = value[8];
                this.data_9 = value[9];
                this.data_10 = value[10];
                this.data_11 = value[11];
                this.data_12 = value[12];
                this.data_13 = value[13];
                this.data_14 = value[14];
                this.data_15 = value[15];
            }
        }
        public float data_0;
        public float data_1;
        public float data_2;
        public float data_3;
        public float data_4;
        public float data_5;
        public float data_6;
        public float data_7;
        public float data_8;
        public float data_9;
        public float data_10;
        public float data_11;
        public float data_12;
        public float data_13;
        public float data_14;
        public float data_15;

        public Matrix44F(float data_0, float data_1, float data_2, float data_3, float data_4, float data_5, float data_6, float data_7, float data_8, float data_9, float data_10, float data_11, float data_12, float data_13, float data_14, float data_15)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
            this.data_2 = data_2;
            this.data_3 = data_3;
            this.data_4 = data_4;
            this.data_5 = data_5;
            this.data_6 = data_6;
            this.data_7 = data_7;
            this.data_8 = data_8;
            this.data_9 = data_9;
            this.data_10 = data_10;
            this.data_11 = data_11;
            this.data_12 = data_12;
            this.data_13 = data_13;
            this.data_14 = data_14;
            this.data_15 = data_15;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Square matrix of 3. The data arrangement is row-major.
    /// </para>
    /// <para xml:lang="zh">
    /// 三阶方阵。数据的排列方式为row-major。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix33F
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of matrix.
        /// </para>
        /// <para xml:lang="zh">
        /// 矩阵的原始数据数组。
        /// </para>
        /// </summary>
        public float[] data
        {
            get
            {
                return new float[] { data_0, data_1, data_2, data_3, data_4, data_5, data_6, data_7, data_8 };
            }
            set
            {
                if (value.Length != 9) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
                this.data_2 = value[2];
                this.data_3 = value[3];
                this.data_4 = value[4];
                this.data_5 = value[5];
                this.data_6 = value[6];
                this.data_7 = value[7];
                this.data_8 = value[8];
            }
        }
        public float data_0;
        public float data_1;
        public float data_2;
        public float data_3;
        public float data_4;
        public float data_5;
        public float data_6;
        public float data_7;
        public float data_8;

        public Matrix33F(float data_0, float data_1, float data_2, float data_3, float data_4, float data_5, float data_6, float data_7, float data_8)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
            this.data_2 = data_2;
            this.data_3 = data_3;
            this.data_4 = data_4;
            this.data_5 = data_5;
            this.data_6 = data_6;
            this.data_7 = data_7;
            this.data_8 = data_8;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Accelerometer reading.
    ///
    /// The positive direction of x-axis is from the device center to its right side of the screen.
    /// The positive direction of y-axis is from the device center to its top side of the screen.
    /// The positive direction of z-axis is from the device center perpendicular to the screen outward.
    ///
    /// The unit of x, y, z is m/s^2.
    /// The unit of timestamp is second.
    /// </para>
    /// <para xml:lang="zh">
    /// 加速度计读数。
    ///
    /// x轴正方向从设备中心指向屏幕右部。
    /// y轴正方向从设备中心指向屏幕上部。
    /// z轴正方向从设备中心垂直屏幕向外。
    ///
    /// x, y, z单位为m/s^2。
    /// timestamp单位为秒。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AccelerometerResult
    {
        public float x;
        public float y;
        public float z;
        public double timestamp;

        public AccelerometerResult(float x, float y, float z, double timestamp)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.timestamp = timestamp;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Gyroscope reading.
    ///
    /// The positive direction of x-axis is from the device center to its right side of the screen.
    /// The positive direction of y-axis is from the device center to its top side of the screen.
    /// The positive direction of z-axis is from the device center perpendicular to the screen outward.
    ///
    /// The x, y, z values represent the angular speed around the corresponding axis in radians/second.
    /// The rotation is positive in counter-clockwise, looking from the positive location on the axis at the device.
    /// The unit of timestamp is second.
    /// </para>
    /// <para xml:lang="zh">
    /// 陀螺仪读数。
    ///
    /// x轴正方向从设备中心指向屏幕右部。
    /// y轴正方向从设备中心指向屏幕上部。
    /// z轴正方向从设备中心垂直屏幕向外。
    ///
    /// x, y, z表示绕对应的轴旋转的角速度，单位为弧度/秒。
    /// 从坐标轴正方向上某处看向设备时的逆时针方向为旋转正方向。
    /// timestamp单位为秒。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct GyroscopeResult
    {
        public float x;
        public float y;
        public float z;
        public double timestamp;

        public GyroscopeResult(float x, float y, float z, double timestamp)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.timestamp = timestamp;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Attitude sensor reading.
    ///
    /// The positive direction of x-axis is from the device center to its right side.
    /// The positive direction of y-axis is from the device center to its top side.
    /// The positive direction of z-axis is from the device center to its front.
    ///
    /// The device is rotated trough an angle θ around an axis (x, y, z).
    /// A unit quaternion can by obtained by (cos(θ/2), x*sin(θ/2), y*sin(θ/2), z*sin(θ/2)).
    /// (v0, v1, v2, v3) = (cos(θ/2), x*sin(θ/2), y*sin(θ/2), z*sin(θ/2)).
    ///
    /// The reference coordinate system is defined as a direct orthonormal basis, where:
    ///     Z points towards the sky and is perpendicular to the ground.
    ///     (X, Y, Z) is right-handed.
    ///
    /// The unit of timestamp is second.
    /// </para>
    /// <para xml:lang="zh">
    /// 姿态传感器读数。
    ///
    /// x轴正方向从设备中心指向屏幕右部。
    /// y轴正方向从设备中心指向屏幕上部。
    /// z轴正方向从设备中心垂直屏幕向外。
    ///
    /// 设备绕轴(x, y, z)旋转了一个角度θ。
    /// 单位旋转四元数为(cos(θ/2), x\*sin(θ/2), y\*sin(θ/2), z\*sin(θ/2))。
    /// (v0, v1, v2, v3) = (cos(θ/2), x\*sin(θ/2), y\*sin(θ/2), z\*sin(θ/2))。
    ///
    /// 参考坐标系被定义为一组有向的标准正交基，其中
    ///     Z指向天空，并与地面平行。
    ///     (X, Y, Z)构成右手系。
    ///
    /// timestamp单位为秒。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AttitudeSensorResult
    {
        public float v0;
        public float v1;
        public float v2;
        public float v3;
        public double timestamp;

        public AttitudeSensorResult(float v0, float v1, float v2, float v3, double timestamp)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.timestamp = timestamp;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Magnetometer reading.
    ///
    /// The positive direction of x-axis is from the device center to its right side of the screen.
    /// The positive direction of y-axis is from the device center to its top side of the screen.
    /// The positive direction of z-axis is from the device center perpendicular to the screen outward.
    ///
    /// The unit of x, y, z is uT(micro-Tesla).
    /// The unit of timestamp is second.
    /// </para>
    /// <para xml:lang="zh">
    /// 磁力计读数。
    ///
    /// x轴正方向从设备中心指向屏幕右部。
    /// y轴正方向从设备中心指向屏幕上部。
    /// z轴正方向从设备中心垂直屏幕向外。
    ///
    /// x, y, z单位为uT(微特斯拉)。
    /// timestamp单位为秒。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MagnetometerResult
    {
        public float x;
        public float y;
        public float z;
        public double timestamp;

        public MagnetometerResult(float x, float y, float z, double timestamp)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.timestamp = timestamp;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Location reading.
    ///
    /// The unit of latitude, longitude is meter.
    /// The unit of altitude is meter.
    /// The unit of horizontalAccuracy is meter.
    /// verticalAccuracy is the accuracy in the direction of gravity of earth, and its unit is meter.
    /// </para>
    /// <para xml:lang="zh">
    /// 位置读数。
    ///
    /// latitude, longitude单位为度。
    /// altitude单位为米。
    /// horizontalAccuracy为水平方向精度，单位为米。
    /// verticalAccuracy为重力方向精度，单位为米。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct LocationResult
    {
        public double latitude;
        public double longitude;
        public double altitude;
        public double horizontalAccuracy;
        public double verticalAccuracy;
        public bool hasAltitude { get { return hasAltitude_ != 0; } set { hasAltitude_ = (Byte)(value ? 1 : 0); } }
        private Byte hasAltitude_;
        public bool hasHorizontalAccuracy { get { return hasHorizontalAccuracy_ != 0; } set { hasHorizontalAccuracy_ = (Byte)(value ? 1 : 0); } }
        private Byte hasHorizontalAccuracy_;
        public bool hasVerticalAccuracy { get { return hasVerticalAccuracy_ != 0; } set { hasVerticalAccuracy_ = (Byte)(value ? 1 : 0); } }
        private Byte hasVerticalAccuracy_;

        public LocationResult(double latitude, double longitude, double altitude, double horizontalAccuracy, double verticalAccuracy, bool hasAltitude, bool hasHorizontalAccuracy, bool hasVerticalAccuracy)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
            this.horizontalAccuracy = horizontalAccuracy;
            this.verticalAccuracy = verticalAccuracy;
            this.hasAltitude_ = (Byte)(hasAltitude ? 1 : 0);
            this.hasHorizontalAccuracy_ = (Byte)(hasHorizontalAccuracy ? 1 : 0);
            this.hasVerticalAccuracy_ = (Byte)(hasVerticalAccuracy ? 1 : 0);
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Proximity location reading.
    ///
    /// The unit of x, y, z is meter. Origin is at map block origin. y is up.
    /// The unit of accuracy is meter.
    /// The unit of timestamp and validTime is second.
    /// is2d is whether y is disabled.
    /// </para>
    /// <para xml:lang="zh">
    /// 邻近位置读数。
    ///
    /// x, y, z单位为米。原点为地图块原点。y向上。
    /// accuracy单位为米。
    /// timestamp, validTime单位为秒。
    /// is2d表示是否不使用y。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ProximityLocationResult
    {
        public float x;
        public float y;
        public float z;
        public float accuracy;
        public double timestamp;
        public bool is2d { get { return is2d_ != 0; } set { is2d_ = (Byte)(value ? 1 : 0); } }
        private Byte is2d_;
        public double validTime;

        public ProximityLocationResult(float x, float y, float z, float accuracy, double timestamp, bool is2d, double validTime)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.accuracy = accuracy;
            this.timestamp = timestamp;
            this.is2d_ = (Byte)(is2d ? 1 : 0);
            this.validTime = validTime;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// working mode in which way the system will utilize the block maps prior.
    /// </para>
    /// <para xml:lang="zh">
    /// 先验地图信息工作模式
    /// </para>
    /// </summary>
    public enum BlockPriorMode
    {
        Soft = 0,
        Hard = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Prior infos about the block maps.
    ///
    /// blockIds are a list of the map id of block maps.
    /// mode is to indicate in which way the system will utilize it, soft or hard. Refer to `BlockPriorMode`_.
    /// </para>
    /// <para xml:lang="zh">
    /// 先验地图信息。
    ///
    /// blockIds为地图id。
    /// mode为先验信息被系统使用的模式，可以为soft或hard。参考`BlockPriorMode`_。
    /// </para>
    /// </summary>
    internal class BlockPriorResult : RefBase
    {
        internal BlockPriorResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new BlockPriorResult(cdata_new, deleter_, retainer_);
        }
        public new BlockPriorResult Clone()
        {
            return (BlockPriorResult)(CloneObject());
        }
        public virtual List<string> blockIds()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_BlockPriorResult_blockIds(cdata, out _return_value_);
                return Detail.ListOfString_from_c(ar, _return_value_);
            }
        }
        public virtual string mode()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_BlockPriorResult_mode(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        public static BlockPriorResult create(List<string> blockIds, BlockPriorMode mode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_BlockPriorResult_create(Detail.ListOfString_to_c(ar, blockIds), mode, out _return_value_);
                return Detail.Object_from_c<BlockPriorResult>(_return_value_, Detail.easyar_BlockPriorResult__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// 3 dimensional vector of double.
    /// </para>
    /// <para xml:lang="zh">
    /// 三维double向量。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vec3D
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of vector.
        /// </para>
        /// <para xml:lang="zh">
        /// 向量的原始数据数组。
        /// </para>
        /// </summary>
        public double[] data
        {
            get
            {
                return new double[] { data_0, data_1, data_2 };
            }
            set
            {
                if (value.Length != 3) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
                this.data_2 = value[2];
            }
        }
        public double data_0;
        public double data_1;
        public double data_2;

        public Vec3D(double data_0, double data_1, double data_2)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
            this.data_2 = data_2;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// 4 dimensional vector of float.
    /// </para>
    /// <para xml:lang="zh">
    /// 四维float向量。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec4F
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of vector.
        /// </para>
        /// <para xml:lang="zh">
        /// 向量的原始数据数组。
        /// </para>
        /// </summary>
        public float[] data
        {
            get
            {
                return new float[] { data_0, data_1, data_2, data_3 };
            }
            set
            {
                if (value.Length != 4) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
                this.data_2 = value[2];
                this.data_3 = value[3];
            }
        }
        public float data_0;
        public float data_1;
        public float data_2;
        public float data_3;

        public Vec4F(float data_0, float data_1, float data_2, float data_3)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
            this.data_2 = data_2;
            this.data_3 = data_3;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// 3 dimensional vector of float.
    /// </para>
    /// <para xml:lang="zh">
    /// 三维float向量。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec3F
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of vector.
        /// </para>
        /// <para xml:lang="zh">
        /// 向量的原始数据数组。
        /// </para>
        /// </summary>
        public float[] data
        {
            get
            {
                return new float[] { data_0, data_1, data_2 };
            }
            set
            {
                if (value.Length != 3) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
                this.data_2 = value[2];
            }
        }
        public float data_0;
        public float data_1;
        public float data_2;

        public Vec3F(float data_0, float data_1, float data_2)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
            this.data_2 = data_2;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// 2 dimensional vector of float.
    /// </para>
    /// <para xml:lang="zh">
    /// 二维float向量。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec2F
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of vector.
        /// </para>
        /// <para xml:lang="zh">
        /// 向量的原始数据数组。
        /// </para>
        /// </summary>
        public float[] data
        {
            get
            {
                return new float[] { data_0, data_1 };
            }
            set
            {
                if (value.Length != 2) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
            }
        }
        public float data_0;
        public float data_1;

        public Vec2F(float data_0, float data_1)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// 4 dimensional vector of int.
    /// </para>
    /// <para xml:lang="zh">
    /// 四维int向量。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vec4I
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of vector.
        /// </para>
        /// <para xml:lang="zh">
        /// 向量的原始数据数组。
        /// </para>
        /// </summary>
        public int[] data
        {
            get
            {
                return new int[] { data_0, data_1, data_2, data_3 };
            }
            set
            {
                if (value.Length != 4) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
                this.data_2 = value[2];
                this.data_3 = value[3];
            }
        }
        public int data_0;
        public int data_1;
        public int data_2;
        public int data_3;

        public Vec4I(int data_0, int data_1, int data_2, int data_3)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
            this.data_2 = data_2;
            this.data_3 = data_3;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// 2 dimensional vector of int.
    /// </para>
    /// <para xml:lang="zh">
    /// 二维int向量。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec2I
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The raw data of vector.
        /// </para>
        /// <para xml:lang="zh">
        /// 向量的原始数据数组。
        /// </para>
        /// </summary>
        public int[] data
        {
            get
            {
                return new int[] { data_0, data_1 };
            }
            set
            {
                if (value.Length != 2) { throw new ArgumentException(); }
                this.data_0 = value[0];
                this.data_1 = value[1];
            }
        }
        public int data_0;
        public int data_1;

        public Vec2I(int data_0, int data_1)
        {
            this.data_0 = data_0;
            this.data_1 = data_1;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// DenseSpatialMap is used to reconstruct the environment accurately and densely. The reconstructed model is represented by `triangle mesh`, which is denoted simply by `mesh`.
    /// DenseSpatialMap occupies 1 buffers of camera.
    /// </para>
    /// <para xml:lang="zh">
    /// DenseSpatialMap用来对环境进行精确的三维稠密重建，其重建的模型用三角网格表示，称为mesh。
    /// DenseSpatialMap占用1个camera的buffer。
    /// </para>
    /// </summary>
    internal class DenseSpatialMap : RefBase
    {
        internal DenseSpatialMap(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new DenseSpatialMap(cdata_new, deleter_, retainer_);
        }
        public new DenseSpatialMap Clone()
        {
            return (DenseSpatialMap)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns True when the device supports dense reconstruction, otherwise returns False.
        /// </para>
        /// <para xml:lang="zh">
        /// 当设备支持稠密重建功能时返回True，否则返回False。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_DenseSpatialMap_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port for input frame. For DenseSpatialMap to work, the inputFrame must include image and it&#39;s camera parameters and spatial information (cameraTransform and trackingStatus). See also `InputFrameSink`_ .
        /// Note: Both pinhole and fisheye camera are supported. But performance may decrease while using a fisheye camera.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入帧输入端口。DenseSpatialMap输入帧必须包含图像和对应的camera参数、空间信息（cameraTransform和trackingStatus）。参考 `InputFrameSink`_ 。
        /// 注意：camera支持针孔和鱼眼相机模型，使用鱼眼相机性能会受影响。
        /// </para>
        /// </summary>
        public virtual InputFrameSink inputFrameSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_DenseSpatialMap_inputFrameSink(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_DenseSpatialMap_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Create `DenseSpatialMap`_ object.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建`DenseSpatialMap`_对象。
        /// </para>
        /// </summary>
        public static DenseSpatialMap create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_DenseSpatialMap_create(out _return_value_);
                return Detail.Object_from_c<DenseSpatialMap>(_return_value_, Detail.easyar_DenseSpatialMap__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Start or continue runninng `DenseSpatialMap`_ algorithm.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始重建或从暂停中恢复，继续重建。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_DenseSpatialMap_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Pause the reconstruction algorithm. Call `start` to resume reconstruction.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停重建过程。调用start来继续重建过程。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_DenseSpatialMap_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close `DenseSpatialMap`_ algorithm.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭重建过程。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_DenseSpatialMap_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the mesh management object of type `SceneMesh`_ . The contents will automatically update after calling the `DenseSpatialMap.updateSceneMesh`_ function.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取 `SceneMesh`_ 类型的mesh管理对象。其中的内容会在调用`DenseSpatialMap.updateSceneMesh`_ 函数之后自动更新。
        /// </para>
        /// </summary>
        public virtual SceneMesh getMesh()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_DenseSpatialMap_getMesh(cdata, out _return_value_);
                return Detail.Object_from_c<SceneMesh>(_return_value_, Detail.easyar_SceneMesh__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the lastest updated mesh and save it to the `SceneMesh`_ object obtained by `DenseSpatialMap.getMesh`_ .
        /// The parameter `updateMeshAll` indicates whether to perform a `full update` or an `incremental update`. When `updateMeshAll` is True, `full update` is performed. All meshes are saved to `SceneMesh`_ . When `updateMeshAll` is False, `incremental update` is performed, and only the most recently updated mesh is saved to `SceneMesh`_ .
        /// `Full update` will take extra time and memory space, causing performance degradation.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取最近更新的mesh，保存到由`DenseSpatialMap.getMesh`_ 得到的 `SceneMesh`_ 对象中。
        /// 参数updateMeshAll指明是进行full update还是incremental update。当updateMeshAll为True时进行full update，所有的mesh都会保存到`SceneMesh`_ 中；当updateMeshAll为False时进行incremental update，只保存最近更新的mesh到`SceneMesh`_ 中。
        /// 进行full update将占用额外的时间和内存空间，导致性能下降。
        /// </para>
        /// </summary>
        public virtual bool updateSceneMesh(bool updateMeshAll)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_DenseSpatialMap_updateSceneMesh(cdata, updateMeshAll);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// The dense reconstructed model is represented by triangle mesh, or simply denoted as mesh. Because mesh updates frequently, in order to ensure efficiency, the mesh of the whole reconstruction model is divided into many mesh blocks. A mesh block is composed of a cube about 1 meter long, with attributes such as vertices and indices.
    ///
    /// BlockInfo is used to describe the content of a mesh block. (x, y, z) is the index of mesh block, the coordinates of a mesh block&#39;s origin in world coordinate system can be obtained by  multiplying (x, y, z) by the physical size of mesh block. You may filter the part you want to display in advance by the mesh block&#39;s world coordinates for the sake of saving rendering time.
    /// </para>
    /// <para xml:lang="zh">
    /// 稠密重建得到的模型使用三角网格表示，称为mesh。由于mesh会进行频繁的更新，为了保证效率，整个重建模型的mesh被分割成了非常多的mesh block。一个mesh block由一个边长大概1米的立方体组成，其中有vertex和index等元素。
    ///
    /// BlockInfo用来描述一个mesh block的内容。其中(x,y,z)是mesh block的索引，将(x,y,z)乘上每个mesh block的物理尺寸可以获得这个mesh block的原点在世界坐标系中的坐标。可以通过mesh block在世界中的位置对需要显示的部分进行提前过滤，以节省渲染需要的时间。
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BlockInfo
    {
        /// <summary>
        /// <para xml:lang="en">
        /// x in index (x, y, z) of mesh block.
        /// </para>
        /// <para xml:lang="zh">
        /// mesh block的索引(x,y,z)中的x。
        /// </para>
        /// </summary>
        public int x;
        /// <summary>
        /// <para xml:lang="en">
        /// y in index (x, y, z) of mesh block.
        /// </para>
        /// <para xml:lang="zh">
        /// mesh block的索引(x,y,z)中的y。
        /// </para>
        /// </summary>
        public int y;
        /// <summary>
        /// <para xml:lang="en">
        /// z in index (x, y, z) of mesh block.
        /// </para>
        /// <para xml:lang="zh">
        /// mesh block的索引(x,y,z)中的z。
        /// </para>
        /// </summary>
        public int z;
        /// <summary>
        /// <para xml:lang="en">
        /// Number of vertices in a mesh block.
        /// </para>
        /// <para xml:lang="zh">
        /// 一个mesh block中所拥有的顶点的数目。
        /// </para>
        /// </summary>
        public int numOfVertex;
        /// <summary>
        /// <para xml:lang="en">
        /// startPointOfVertex is the starting position of the vertex data stored in the vertex buffer, indicating from where the stored vertices belong to current mesh block. It is not equal to the number of bytes of the offset from the beginning of vertex buffer. The offset is startPointOfVertex*3*4 bytes.
        /// </para>
        /// <para xml:lang="zh">
        /// 顶点数据在vertex buffer中存放的起始位置，表示从第几个顶点开始是属于当前这个mesh block的。不等于偏移量的字节数，起始位置的偏移为startPointOfVertex*3*4个字节。
        /// </para>
        /// </summary>
        public int startPointOfVertex;
        /// <summary>
        /// <para xml:lang="en">
        /// The number of indices in a mesh block. Each of three consecutive vertices form a triangle.
        /// </para>
        /// <para xml:lang="zh">
        /// 一个mesh block中所拥有的索引的数目，每连续3个顶点构成一个三角面。
        /// </para>
        /// </summary>
        public int numOfIndex;
        /// <summary>
        /// <para xml:lang="en">
        /// Similar to startPointOfVertex. startPointOfIndex is the starting position of the index data stored in the index buffer, indicating from where the stored indices belong to current mesh block. It is not equal to the number of bytes of the offset from the beginning of index buffer. The offset is startPointOfIndex*3*4 bytes.
        /// </para>
        /// <para xml:lang="zh">
        /// 与startPointOfVertex类似。索引数据在index buffer中存放的起始位置，表示从第几个索引开始是属于当前这个mesh block的。不等于偏移量的字节数，起始位置的偏移为startPointOfIndex*3*4个字节。
        /// </para>
        /// </summary>
        public int startPointOfIndex;
        /// <summary>
        /// <para xml:lang="en">
        /// Version represents how many times the mesh block has updated. The larger the version, the newer the block. If the version of a mesh block increases after calling `DenseSpatialMap.updateSceneMesh`_ , it indicates that the mash block has changed.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前mesh block更新的次数，version越大表示更新的次数更多。如果调用`DenseSpatialMap.updateSceneMesh`_ 后一个mesh block的version变大了，说明其中的内容发生了变化。
        /// </para>
        /// </summary>
        public int version;

        public BlockInfo(int x, int y, int z, int numOfVertex, int startPointOfVertex, int numOfIndex, int startPointOfIndex, int version)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.numOfVertex = numOfVertex;
            this.startPointOfVertex = startPointOfVertex;
            this.numOfIndex = numOfIndex;
            this.startPointOfIndex = startPointOfIndex;
            this.version = version;
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// SceneMesh is used to manage and preserve the results of `DenseSpatialMap`_.
    /// There are two kinds of meshes saved in SceneMesh, one is the mesh of the whole reconstructed scene, hereinafter referred to as `meshAll`, the other is the recently updated mesh, hereinafter referred to as `meshUpdated`. `meshAll` is a whole mesh, including all vertex data and index data, etc. `meshUpdated` is composed of several `mesh block` s, each `mesh block` is a cube, which contains the mesh formed by the object surface in the corresponding cube space.
    /// `meshAll` is available only when the `DenseSpatialMap.updateSceneMesh`_ method is called specifying that all meshes need to be updated. If `meshAll` has been updated previously and not updated in recent times, the data in `meshAll` is remain the same.
    /// </para>
    /// </summary>
    internal class SceneMesh : RefBase
    {
        internal SceneMesh(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SceneMesh(cdata_new, deleter_, retainer_);
        }
        public new SceneMesh Clone()
        {
            return (SceneMesh)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the number of vertices in `meshAll`.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshAll中顶点的数目。
        /// </para>
        /// </summary>
        public virtual int getNumOfVertexAll()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SceneMesh_getNumOfVertexAll(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the number of indices in `meshAll`. Since every 3 indices form a triangle, the returned value should be a multiple of 3.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshAll中索引的数目。由于每3个索引构成一个三角面，返回的数值应该是3的整数倍。
        /// </para>
        /// </summary>
        public virtual int getNumOfIndexAll()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SceneMesh_getNumOfIndexAll(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the position component of the vertices in `meshAll` (in the world coordinate system). The position of a vertex is described by three coordinates (x, y, z) in meters. The position data are stored tightly in `Buffer`_ by `x1, y1, z1, x2, y2, z2, ...` Each component is of `float` type.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshAll中的顶点的位置数据（世界坐标系下）。一个顶点的位置由(x,y,z)三个坐标描述，单位是米。位置数据在Buffer中是按照x1,y1,z1,x2,y2,z2,...紧密排列的。每个分量都是float类型。
        /// </para>
        /// </summary>
        public virtual Buffer getVerticesAll()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SceneMesh_getVerticesAll(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the normal component of vertices in `meshAll`. The normal of a vertex is described by three components (nx, ny, nz). The normal is normalized, that is, the length is 1. Normal data are stored tightly in `Buffer`_ by `nx1, ny1, nz1, nx2, ny2, nz2,....` Each component is of `float` type.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshAll中的顶点的法向量数据。一个顶点的法向量由(nx,ny,nz)三个分量描述，该法向量是归一化后的结果，即模长为1。法向量数据在Buffer中是按照nx1,ny1,nz1,nx2,ny2,nz2,...紧密排列的。每个分量都是float类型。
        /// </para>
        /// </summary>
        public virtual Buffer getNormalsAll()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SceneMesh_getNormalsAll(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the index data in `meshAll`. Each triangle is composed of three indices (ix, iy, iz). Indices are stored tightly in `Buffer`_ by `ix1, iy1, iz1, ix2, iy2, iz2,...` Each component is of `int32` type.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshAll中的索引数据。每一个三角面由(ix,iy,iz)三个索引构成。索引数据在Buffer中是按照ix1,iy1,iz1,ix2,iy2,iz2,...紧密排列的。每个分量都是int32类型。
        /// </para>
        /// </summary>
        public virtual Buffer getIndicesAll()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SceneMesh_getIndicesAll(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the number of vertices in `meshUpdated`.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshUpdated中顶点的数目。
        /// </para>
        /// </summary>
        public virtual int getNumOfVertexIncremental()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SceneMesh_getNumOfVertexIncremental(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the number of indices in `meshUpdated`. Since every 3 indices form a triangle, the returned value should be a multiple of 3.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshUpdated中索引的数目。
        /// </para>
        /// </summary>
        public virtual int getNumOfIndexIncremental()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SceneMesh_getNumOfIndexIncremental(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the position component of the vertices in `meshUpdated` (in the world coordinate system). The position of a vertex is described by three coordinates (x, y, z) in meters. The position data are stored tightly in `Buffer`_ by `x1, y1, z1, x2, y2, z2, ...` Each component is of `float` type.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshUpdated中的顶点的位置数据（世界坐标系下）。一个顶点的位置由(x,y,z)三个坐标描述，单位是米。顶点数据在Buffer中是按照x1,y1,z1,x2,y2,z2,...紧密排列的。每个分量都是float类型。
        /// </para>
        /// </summary>
        public virtual Buffer getVerticesIncremental()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SceneMesh_getVerticesIncremental(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the normal component of vertices in `meshUpdated`. The normal of a vertex is described by three components (nx, ny, nz). The normal is normalized, that is, the length is 1. Normal data are stored tightly in `Buffer`_ by `nx1, ny1, nz1, nx2, ny2, nz2,....` Each component is of `float` type.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshUpdated中的顶点的法向量数据（世界坐标系下）。一个顶点的法向量由(nx,ny,nz)三个分量描述，该法向量是归一化后的结果，即模长为1。法向量数据在Buffer中是按照nx1,ny1,nz1,nx2,ny2,nz2,...紧密排列的。每个分量都是float类型。
        /// </para>
        /// </summary>
        public virtual Buffer getNormalsIncremental()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SceneMesh_getNormalsIncremental(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the index data in `meshUpdated`. Each triangle is composed of three indices (ix, iy, iz). Indices are stored tightly in `Buffer`_ by `ix1, iy1, iz1, ix2, iy2, iz2,...` Each component is of `int32` type.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshUpdated中的所有三角面的索引。每一个三角面由(ix,iy,iz)三个索引构成。索引数据在Buffer中是按照ix1,iy1,iz1,ix2,iy2,iz2,...紧密排列的。每个分量都是int32类型。
        /// </para>
        /// </summary>
        public virtual Buffer getIndicesIncremental()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SceneMesh_getIndicesIncremental(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the description object of `mesh block` in `meshUpdate`. The return value is an array of `BlockInfo`_ elements, each of which is a detailed description of a `mesh block`.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meshUpdated中的mesh block的描述对象。返回值是一个由 BlockInfo 构成的数组，其中每一个元素是对一个mesh block的信息的详细描述。
        /// </para>
        /// </summary>
        public virtual List<BlockInfo> getBlocksInfoIncremental()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SceneMesh_getBlocksInfoIncremental(cdata, out _return_value_);
                return Detail.ListOfBlockInfo_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the edge length of a `mesh block` in meters.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取一个mesh block的边长，单位是米。
        /// </para>
        /// </summary>
        public virtual float getBlockDimensionInMeters()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SceneMesh_getBlockDimensionInMeters(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Accelerometer calls the accelerometer provided by the operating system, and outputs `AccelerometerResult`_ .
    /// When it is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// It is not recommended to open multiple times simultaneously, which may cause failure on open or cause precision downgrade.
    /// </para>
    /// <para xml:lang="zh">
    /// Accelerometer调用了系统提供的加速度计，输出 `AccelerometerResult`_ 。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// 不建议同时打开多次，可能会无法使用或精度下降。
    /// </para>
    /// </summary>
    internal class Accelerometer : RefBase
    {
        internal Accelerometer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new Accelerometer(cdata_new, deleter_, retainer_);
        }
        public new Accelerometer Clone()
        {
            return (Accelerometer)(CloneObject());
        }
        public Accelerometer() : base(IntPtr.Zero, Detail.easyar_Accelerometer__dtor, Detail.easyar_Accelerometer__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_Accelerometer__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Android or iOS with supported hardware. On other operating systems, it is not supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Android或iOS上且硬件支持时返回true。其他操作系统暂不支持。
        /// </para>
        /// </summary>
        public virtual bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Accelerometer_isAvailable(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual AccelerometerResultSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Accelerometer_output(cdata, out _return_value_);
                return Detail.Object_from_c<AccelerometerResultSource>(_return_value_, Detail.easyar_AccelerometerResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device. Sampling period is defined by implementation. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 打开设备。采样周期由内部实现所确定。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool open()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Accelerometer_open(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device with a specific sampling period. Sampling period is limited by hardware and may not reach the specified value. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 以特定采样周期打开设备。采样周期会受到硬件限制而可能无法达到指定值。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool openWithSamplingPeriod(int samplingPeriodMilliseconds)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Accelerometer_openWithSamplingPeriod(cdata, samplingPeriodMilliseconds);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Closes. It shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭设备。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Accelerometer_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the most recent result. If there is no result, it returns empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前的最新结果。如果结果尚未获取到，返回空。
        /// </para>
        /// </summary>
        public virtual Optional<AccelerometerResult> getCurrentResult()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Accelerometer_getCurrentResult(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<AccelerometerResult>.Empty);
            }
        }
    }

    public enum ARCoreCameraDeviceFocusMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Auto focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 自动对焦模式
        /// </para>
        /// </summary>
        Auto = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Fixed focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 固定对焦模式
        /// </para>
        /// </summary>
        Fixed = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ARCoreCameraDevice implements a camera device based on ARCore, which outputs `InputFrame`_  (including image, camera parameters, timestamp, 6DOF location, and tracking status).
    /// Loading of libarcore_sdk_c.so with java.lang.System.loadLibrary is required.
    /// After creation, start/stop can be invoked to start or stop video stream capture.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// ARCoreCameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for use.
    /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is more than this number, the device will not output new `InputFrame`_ , until previous `InputFrame`_ have been released. This may cause screen stuck.
    /// Caution: Currently, ARCore(v1.13.0) has memory leaks on creating and destroying sessions. Repeated creations and destructions will cause an increasing and non-reclaimable memory footprint.
    /// </para>
    /// <para xml:lang="zh">
    /// ARCoreCameraDevice实现了一个基于ARCore的camera设备，输出 `InputFrame`_ （包含图像、摄像机参数、时间戳、6DOF位置信息和跟踪状态）。
    /// 使用时需要先使用java.lang.System.loadLibrary加载libarcore_sdk_c.so。
    /// 创建之后，可以调用start/stop来开始和停止采集视频流数据。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// ARCoreCameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
    /// 注意：当前ARCore(v1.13.0)的实现在创建和销毁session时存在内存泄漏，多次创建和销毁会导致内存占用不断增长且销毁后也不释放。
    /// </para>
    /// </summary>
    internal class ARCoreCameraDevice : RefBase
    {
        internal ARCoreCameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ARCoreCameraDevice(cdata_new, deleter_, retainer_);
        }
        public new ARCoreCameraDevice Clone()
        {
            return (ARCoreCameraDevice)(CloneObject());
        }
        public ARCoreCameraDevice() : base(IntPtr.Zero, Detail.easyar_ARCoreCameraDevice__dtor, Detail.easyar_ARCoreCameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ARCoreCameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Android when ARCore is installed.
        /// If called with libarcore_sdk_c.so not loaded, it returns false.
        /// Notice: If ARCore is not supported on the device but ARCore apk is installed via side-loading, it will return true, but ARCore will not function properly.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Android系统上并安装了ARCore时返回true。
        /// 在没有加载libarcore_sdk_c.so时调用会返回false。
        /// 注意：如果设备不支持ARCore，但却通过旁加载方式安装了ARCore的apk，则该函数会返回true，但ARCore不能正常使用。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the current device is supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查当前设备是否支持。
        /// </para>
        /// </summary>
        public static bool isDeviceSupported()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_isDeviceSupported();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `InputFrame`_ buffer capacity.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARCoreCameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ARCoreCameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets focus mode to focusMode.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置对焦模式为focusMode。
        /// </para>
        /// </summary>
        public virtual void setFocusMode(ARCoreCameraDeviceFocusMode focusMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARCoreCameraDevice_setFocusMode(cdata, focusMode);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始采集视频流数据。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止采集视频流数据。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARCoreCameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARCoreCameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets lower bound of current frame rate range.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围下界。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeLower()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_frameRateRangeLower(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets upper bound of current frame rate range.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围上界。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeUpper()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARCoreCameraDevice_frameRateRangeUpper(cdata);
                return _return_value_;
            }
        }
    }

    public enum ARKitCameraDeviceFocusMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Auto focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 自动对焦模式
        /// </para>
        /// </summary>
        Auto = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Fixed focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 固定对焦模式
        /// </para>
        /// </summary>
        Fixed = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ARKitCameraDevice implements a camera device based on ARKit, which outputs `InputFrame`_ (including image, camera parameters, timestamp, 6DOF location, and tracking status).
    /// After creation, start/stop can be invoked to start or stop data collection.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// ARKitCameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for use.
    /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is more than this number, the device will not output new `InputFrame`_ , until previous `InputFrame`_ have been released. This may cause screen stuck.
    /// </para>
    /// <para xml:lang="zh">
    /// ARKitCameraDevice实现了一个基于ARKit的camera设备，输出 `InputFrame`_ （包含图像、摄像机参数、时间戳、6DOF位置信息和跟踪状态）。
    /// 创建之后，可以调用start/stop来开始和停止采集视频流数据。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// ARKitCameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
    /// </para>
    /// </summary>
    internal class ARKitCameraDevice : RefBase
    {
        internal ARKitCameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ARKitCameraDevice(cdata_new, deleter_, retainer_);
        }
        public new ARKitCameraDevice Clone()
        {
            return (ARKitCameraDevice)(CloneObject());
        }
        public ARKitCameraDevice() : base(IntPtr.Zero, Detail.easyar_ARKitCameraDevice__dtor, Detail.easyar_ARKitCameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ARKitCameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on iOS 11 or later when ARKit is supported by hardware.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在iOS 11或更高版本的系统上且在支持ARKit的硬件上时返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `InputFrame`_ buffer capacity.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARKitCameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ARKitCameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets focus mode to focusMode. Call before start. Valid since iOS 11.3.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置对焦模式为focusMode。在start之前调用。iOS 11.3及之后有效。
        /// </para>
        /// </summary>
        public virtual void setFocusMode(ARKitCameraDeviceFocusMode focusMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARKitCameraDevice_setFocusMode(cdata, focusMode);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始采集视频流数据。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止采集视频流数据。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARKitCameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ARKitCameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current frame rate.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率。
        /// </para>
        /// </summary>
        public virtual double frameRate()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_frameRate(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the number of supported preview sizes.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的个数。
        /// </para>
        /// </summary>
        public virtual int supportedSizeCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_supportedSizeCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the index-th supported preview size. It returns {0, 0} if index is out of range.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的第 index 个. 如果 index 超出范围则返回{0, 0}。
        /// </para>
        /// </summary>
        public virtual Vec2I supportedSize(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_supportedSize(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the preview size. The available nearest value will be selected. Call size to get the actual size. frameRateRange may change after calling setSize.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前图像大小。最接近设置值的可选值将被使用。可以使用size来获取实际的大小。设置size后frameRateRange可能会发生变化。
        /// </para>
        /// </summary>
        public virtual bool setSize(Vec2I size)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_setSize(cdata, size);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the number of supported frame rates.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的帧率的个数。
        /// </para>
        /// </summary>
        public virtual int supportedFrameRateCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_supportedFrameRateCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the index-th supported frame rates
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的第 index 个帧率。
        /// </para>
        /// </summary>
        public virtual double supportedFrameRate(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_supportedFrameRate(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets frame rate.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前设备的当前帧率。
        /// </para>
        /// </summary>
        public virtual bool setFrameRate(double frameRate)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ARKitCameraDevice_setFrameRate(cdata, frameRate);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// AttitudeSensor calls the attitude sensor provided by the operating system, and outputs `AttitudeSensorResult`_ .
    /// When it is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// It is not recommended to open multiple times simultaneously, which may cause failure on open or cause precision downgrade.
    /// </para>
    /// <para xml:lang="zh">
    /// AttitudeSensor调用了系统提供的姿态传感器，输出 `AttitudeSensorResult`_ 。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// 不建议同时打开多次，可能会无法使用或精度下降。
    /// </para>
    /// </summary>
    internal class AttitudeSensor : RefBase
    {
        internal AttitudeSensor(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new AttitudeSensor(cdata_new, deleter_, retainer_);
        }
        public new AttitudeSensor Clone()
        {
            return (AttitudeSensor)(CloneObject());
        }
        public AttitudeSensor() : base(IntPtr.Zero, Detail.easyar_AttitudeSensor__dtor, Detail.easyar_AttitudeSensor__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_AttitudeSensor__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Android or iOS with supported hardware. On other operating systems, it is not supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Android或iOS上且硬件支持时返回true。其他操作系统暂不支持。
        /// </para>
        /// </summary>
        public virtual bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_AttitudeSensor_isAvailable(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual AttitudeSensorResultSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_AttitudeSensor_output(cdata, out _return_value_);
                return Detail.Object_from_c<AttitudeSensorResultSource>(_return_value_, Detail.easyar_AttitudeSensorResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device. Sampling period is defined by implementation. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 打开设备。采样周期由内部实现所确定。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool open()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_AttitudeSensor_open(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device with a specific sampling period. Sampling period is limited by hardware and may not reach the specified value. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 以特定采样周期打开设备。采样周期会受到硬件限制而可能无法达到指定值。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool openWithSamplingPeriod(int samplingPeriodMilliseconds)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_AttitudeSensor_openWithSamplingPeriod(cdata, samplingPeriodMilliseconds);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Closes. It shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭设备。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AttitudeSensor_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the most recent result. If there is no result, it returns empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前的最新结果。如果结果尚未获取到，返回空。
        /// </para>
        /// </summary>
        public virtual Optional<AttitudeSensorResult> getCurrentResult()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_AttitudeSensor_getCurrentResult(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<AttitudeSensorResult>.Empty);
            }
        }
    }

    public enum CameraDeviceFocusMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Normal auto focus mode. You should call autoFocus to start the focus in this mode.
        /// </para>
        /// <para xml:lang="zh">
        /// 常规对焦模式，在这个模式下需要调用autoFocus来触发对焦
        /// </para>
        /// </summary>
        Normal = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Continuous auto focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 连续自动对焦模式
        /// </para>
        /// </summary>
        Continousauto = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Infinity focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 无穷远对焦模式
        /// </para>
        /// </summary>
        Infinity = 3,
        /// <summary>
        /// <para xml:lang="en">
        /// Macro (close-up) focus mode. You should call autoFocus to start the focus in this mode.
        /// </para>
        /// <para xml:lang="zh">
        /// 微距对焦模式。在这个模式下需要调用autoFocus来触发对焦
        /// </para>
        /// </summary>
        Macro = 4,
        /// <summary>
        /// <para xml:lang="en">
        /// Medium distance focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 中等距离对焦模式
        /// </para>
        /// </summary>
        Medium = 5,
    }

    public enum AndroidCameraApiType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Android Camera1
        /// </para>
        /// <para xml:lang="zh">
        /// Android Camera1
        /// </para>
        /// </summary>
        Camera1 = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Android Camera2
        /// </para>
        /// <para xml:lang="zh">
        /// Android Camera2
        /// </para>
        /// </summary>
        Camera2 = 1,
    }

    internal enum CameraDevicePresetProfile
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The same as AVCaptureSessionPresetPhoto.
        /// </para>
        /// <para xml:lang="zh">
        /// 即AVCaptureSessionPresetPhoto
        /// </para>
        /// </summary>
        Photo = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// The same as AVCaptureSessionPresetHigh.
        /// </para>
        /// <para xml:lang="zh">
        /// 即AVCaptureSessionPresetHigh
        /// </para>
        /// </summary>
        High = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// The same as AVCaptureSessionPresetMedium.
        /// </para>
        /// <para xml:lang="zh">
        /// 即AVCaptureSessionPresetMedium
        /// </para>
        /// </summary>
        Medium = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// The same as AVCaptureSessionPresetLow.
        /// </para>
        /// <para xml:lang="zh">
        /// 即AVCaptureSessionPresetLow
        /// </para>
        /// </summary>
        Low = 3,
    }

    public enum CameraState
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Unknown
        /// </para>
        /// <para xml:lang="zh">
        /// 未知
        /// </para>
        /// </summary>
        Unknown = 0x00000000,
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnected
        /// </para>
        /// <para xml:lang="zh">
        /// 断开
        /// </para>
        /// </summary>
        Disconnected = 0x00000001,
        /// <summary>
        /// <para xml:lang="en">
        /// Preempted by another application.
        /// </para>
        /// <para xml:lang="zh">
        /// 被其他程序抢占
        /// </para>
        /// </summary>
        Preempted = 0x00000002,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// CameraDevice implements a camera device, which outputs `InputFrame`_ (including image, camera paramters, and timestamp). It is available on Windows, Mac, Android and iOS.
    /// After open, start/stop can be invoked to start or stop data collection. start/stop will not change previous set camera parameters.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// CameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for use.
    /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is more than this number, the device will not output new `InputFrame`_ , until previous `InputFrame`_ have been released. This may cause screen stuck.
    /// On Android, it is required to add android.permission.CAMERA to AndroidManifest.xml for use.
    /// On iOS, it is required to add NSCameraUsageDescription to Info.plist for use.
    /// </para>
    /// <para xml:lang="zh">
    /// CameraDevice实现了一个camera设备，输出 `InputFrame`_ （包含图像、摄像机参数和时间戳）。在Windows、Mac、Android和iOS上可用。
    /// 打开之后，可以调用start/stop来开始和停止采集数据。start/stop不会影响之前所设置的camera参数。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// CameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
    /// Android使用时需要在AndroidManifest.xml中加入android.permission.CAMERA权限声明。
    /// iOS使用时需要在Info.plist中加入NSCameraUsageDescription权限声明。
    /// </para>
    /// </summary>
    internal class CameraDevice : RefBase
    {
        internal CameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CameraDevice(cdata_new, deleter_, retainer_);
        }
        public new CameraDevice Clone()
        {
            return (CameraDevice)(CloneObject());
        }
        public CameraDevice() : base(IntPtr.Zero, Detail.easyar_CameraDevice__dtor, Detail.easyar_CameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_CameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Windows, Mac, Android or iOS.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Windows、Mac、Android和iOS上返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets current camera API (camera1 or camera2) on Android. camera1 is better for compatibility, but lacks some necessary information such as timestamp. camera2 has compatibility issues on some devices.
        /// </para>
        /// <para xml:lang="zh">
        /// 在Android上，可用于获得使用的Camera API（camera1或camera2）。camera1兼容性较好，但缺乏一些必要的信息，如时间戳。camera2在部分设备上存在兼容性问题。
        /// </para>
        /// </summary>
        public virtual AndroidCameraApiType androidCameraApiType()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_androidCameraApiType(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets current camera API (camera1 or camera2) on Android. It must be called before calling openWithIndex, openWithSpecificType or openWithPreferredType, or it will not take effect.
        /// It is recommended to use `CameraDeviceSelector`_ to create camera with camera API set to recommended based on primary algorithm to run.
        /// </para>
        /// <para xml:lang="zh">
        /// 在Android上，可用于设置使用的Camera API（Camera 1或Camera 2）。必须在调用openWithIndex、openWithSpecificType或openWithPreferredType之前进行设置，否则不会生效。
        /// 推荐使用 `CameraDeviceSelector`_ 以根据使用的主要算法来创建设好推荐Camera API的CameraDevice。
        /// </para>
        /// </summary>
        public virtual void setAndroidCameraApiType(AndroidCameraApiType type)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CameraDevice_setAndroidCameraApiType(cdata, type);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `InputFrame`_ buffer capacity.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets callback on state change to notify state of camera disconnection or preemption. It is only available on Windows.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置状态变化回调以通知摄像机断开或被抢占的状态。只在Windows平台上有作用。
        /// </para>
        /// </summary>
        public virtual void setStateChangedCallback(CallbackScheduler callbackScheduler, Optional<Action<CameraState>> stateChangedCallback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CameraDevice_setStateChangedCallback(cdata, callbackScheduler.cdata, stateChangedCallback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromCameraState { has_value = true, value = Detail.FunctorOfVoidFromCameraState_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromCameraState { has_value = false, value = default(Detail.FunctorOfVoidFromCameraState) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Requests camera permission from operating system. You can call this function or request permission directly from operating system. It is only available on Android and iOS. On other platforms, it will call the callback directly with status being granted. This function need to be called from the UI thread.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求camera系统权限。你可以选择使用这个函数或自己申请权限。只在Android和iOS平台上有效，其他平台上的行为为直接调用回调通知权限已授权。应在UI线程调用该函数。
        /// </para>
        /// </summary>
        public static void requestPermissions(CallbackScheduler callbackScheduler, Optional<Action<PermissionStatus, string>> permissionCallback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CameraDevice_requestPermissions(callbackScheduler.cdata, permissionCallback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromPermissionStatusAndString { has_value = true, value = Detail.FunctorOfVoidFromPermissionStatusAndString_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromPermissionStatusAndString { has_value = false, value = default(Detail.FunctorOfVoidFromPermissionStatusAndString) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets count of cameras recognized by the operating system.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得操作系统识别到的camera数量。
        /// </para>
        /// </summary>
        public static int cameraCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_cameraCount();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens a camera by index.
        /// </para>
        /// <para xml:lang="zh">
        /// 按照camera索引打开camera设备。
        /// </para>
        /// </summary>
        public virtual bool openWithIndex(int cameraIndex)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_openWithIndex(cdata, cameraIndex);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens a camera by specific camera device type. If no camera is matched, false will be returned. On Mac, camera device types can not be distinguished.
        /// </para>
        /// <para xml:lang="zh">
        /// 按照精确的camera设备类型打开camera设备，如果没有匹配的类型则会返回false。在Mac上，camera类型无法判别。
        /// </para>
        /// </summary>
        public virtual bool openWithSpecificType(CameraDeviceType type)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_openWithSpecificType(cdata, type);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens a camera by camera device type. If no camera is matched, the first camera will be used.
        /// </para>
        /// <para xml:lang="zh">
        /// 按照camera设备类型打开camera设备，如果没有匹配的类型则会尝试打开第一个camera设备。
        /// </para>
        /// </summary>
        public virtual bool openWithPreferredType(CameraDeviceType type)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_openWithPreferredType(cdata, type);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始采集数据。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops video stream capture. It will only stop capture and will not change previous set camera parameters and connection.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止采集数据。这个方法只会停止捕获图像，所有参数和连接将不会受到影响。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭camera。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera index. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// cameras索引。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int index()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_index(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera parameters, including image size, focal length, principal point, camera type and camera rotation against natural orientation. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera参数，包括图像大小、焦距、主点、camera类型和camera相对设备自然方向的旋转角度。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual CameraParameters cameraParameters()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CameraDevice_cameraParameters(cdata, out _return_value_);
                return Detail.Object_from_c<CameraParameters>(_return_value_, Detail.easyar_CameraParameters__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets camera parameters. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置camera参数。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual void setCameraParameters(CameraParameters cameraParameters)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_CameraDevice_setCameraParameters(cdata, cameraParameters.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the number of supported preview sizes. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的个数。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int supportedSizeCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_supportedSizeCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the index-th supported preview size. It returns {0, 0} if index is out of range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的第 index 个. 如果 index 超出范围则返回{0, 0}。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I supportedSize(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_supportedSize(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the preview size. The available nearest value will be selected. Call size to get the actual size. Call after a successful open. frameRateRange may change after calling setSize.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前图像大小。最接近设置值的可选值将被使用。可以使用size来获取实际的大小。在成功的open之后调用。设置size后frameRateRange可能会发生变化。
        /// </para>
        /// </summary>
        public virtual bool setSize(Vec2I size)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_setSize(cdata, size);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the number of supported frame rate ranges. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有帧率范围的个数。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int supportedFrameRateRangeCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_supportedFrameRateRangeCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets range lower bound of the index-th supported frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有帧率范围的第 index 个的下界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual float supportedFrameRateRangeLower(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_supportedFrameRateRangeLower(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets range upper bound of the index-th supported frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有帧率范围的第 index 个的上界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual float supportedFrameRateRangeUpper(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_supportedFrameRateRangeUpper(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets current index of frame rate range. Call after a successful open. On iOS/macOS, this can return -1, meaning variable frame rate, which can be used to get lower and upper bounds with supportedFrameRateRangeLower and supportedFrameRateRangeUpper.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备的当前帧率范围的索引。在成功的open之后调用。在iOS/macOS上，可能会返回-1，表示不固定帧率，可以使用supportedFrameRateRangeLower和supportedFrameRateRangeUpper获得最小值和最大值。
        /// </para>
        /// </summary>
        public virtual int frameRateRange()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_frameRateRange(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets current index of frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前设备的当前帧率范围的索引。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual bool setFrameRateRange(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_setFrameRateRange(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets flash torch mode to on. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置flash torch模式为on。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual bool setFlashTorchMode(bool on)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_setFlashTorchMode(cdata, on);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets focus mode to focusMode. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置对焦模式为focusMode。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual bool setFocusMode(CameraDeviceFocusMode focusMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_setFocusMode(cdata, focusMode);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Does auto focus once. Call after start. It is only available when FocusMode is Normal or Macro.
        /// </para>
        /// <para xml:lang="zh">
        /// 调用一次自动对焦。在start之后使用。仅在FocusMode为Normal或Macro时才能使用。
        /// </para>
        /// </summary>
        public virtual bool autoFocus()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDevice_autoFocus(cdata);
                return _return_value_;
            }
        }
    }

    public enum CameraDevicePreference
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Optimized for `ImageTracker`_ , `ObjectTracker`_ and `CloudRecognizer`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 对 `ImageTracker`_ , `ObjectTracker`_ 和 `CloudRecognizer`_ 进行优化
        /// </para>
        /// </summary>
        PreferObjectSensing = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Optimized for `SurfaceTracker`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 对 `SurfaceTracker`_ 进行优化
        /// </para>
        /// </summary>
        PreferSurfaceTracking = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Optimized for Motion Tracking . But to use Motion Tracking, it is preferred to use `MotionTrackerCameraDevice`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 对 Motion Tracking 进行优化。但使用 Motion Tracking 时，应优先使用 `MotionTrackerCameraDevice`_ 。
        /// </para>
        /// </summary>
        PreferMotionTracking = 2,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// It is used for selecting camera API (camera1 or camera2) on Android. camera1 is better for compatibility, but lacks some necessary information such as timestamp. camera2 has compatibility issues on some devices.
    /// Different preferences will choose camera1 or camera2 based on usage.
    /// </para>
    /// <para xml:lang="zh">
    /// 用于在Android上选择Camera API（camera1或camera2）。camera1兼容性较好，但缺乏一些必要的信息，如时间戳。camera2在部分设备上存在兼容性问题。
    /// 不同选项会根据用途选择camera1或camera2。
    /// </para>
    /// </summary>
    internal class CameraDeviceSelector
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Gets recommended Android Camera API type by a specified preference.
        /// </para>
        /// <para xml:lang="zh">
        /// 以指定选项获取推荐的Android Camera API类型。
        /// </para>
        /// </summary>
        public static AndroidCameraApiType getAndroidCameraApiType(CameraDevicePreference preference)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDeviceSelector_getAndroidCameraApiType(preference);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates `CameraDevice`_ by a specified preference.
        /// </para>
        /// <para xml:lang="zh">
        /// 以指定选项创建 `CameraDevice`_ 。
        /// </para>
        /// </summary>
        public static CameraDevice createCameraDevice(CameraDevicePreference preference)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CameraDeviceSelector_createCameraDevice(preference, out _return_value_);
                return Detail.Object_from_c<CameraDevice>(_return_value_, Detail.easyar_CameraDevice__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets recommended Camera FocusMode type by a specified preference.
        /// </para>
        /// <para xml:lang="zh">
        /// 以指定选项获取推荐的Camera FocusMode类型。
        /// </para>
        /// </summary>
        public static CameraDeviceFocusMode getFocusMode(CameraDevicePreference preference)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraDeviceSelector_getFocusMode(preference);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Gyroscope calls the gyroscope provided by the operating system, and outputs `GyroscopeResult`_ .
    /// When it is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// It is not recommended to open multiple times simultaneously, which may cause failure on open or cause precision downgrade.
    /// </para>
    /// <para xml:lang="zh">
    /// Gyroscope调用了系统提供的陀螺仪，输出 `GyroscopeResult`_ 。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// 不建议同时打开多次，可能会无法使用或精度下降。
    /// </para>
    /// </summary>
    internal class Gyroscope : RefBase
    {
        internal Gyroscope(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new Gyroscope(cdata_new, deleter_, retainer_);
        }
        public new Gyroscope Clone()
        {
            return (Gyroscope)(CloneObject());
        }
        public Gyroscope() : base(IntPtr.Zero, Detail.easyar_Gyroscope__dtor, Detail.easyar_Gyroscope__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_Gyroscope__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Android or iOS with supported hardware. On other operating systems, it is not supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Android或iOS上且硬件支持时返回true。其他操作系统暂不支持。
        /// </para>
        /// </summary>
        public virtual bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Gyroscope_isAvailable(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual GyroscopeResultSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Gyroscope_output(cdata, out _return_value_);
                return Detail.Object_from_c<GyroscopeResultSource>(_return_value_, Detail.easyar_GyroscopeResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device. Sampling period is defined by implementation. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 打开设备。采样周期由内部实现所确定。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool open()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Gyroscope_open(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device with a specific sampling period. Sampling period is limited by hardware and may not reach the specified value. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 以特定采样周期打开设备。采样周期会受到硬件限制而可能无法达到指定值。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool openWithSamplingPeriod(int samplingPeriodMilliseconds)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Gyroscope_openWithSamplingPeriod(cdata, samplingPeriodMilliseconds);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Closes. It shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭设备。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Gyroscope_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the most recent result. If there is no result, it returns empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前的最新结果。如果结果尚未获取到，返回空。
        /// </para>
        /// </summary>
        public virtual Optional<GyroscopeResult> getCurrentResult()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Gyroscope_getCurrentResult(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<GyroscopeResult>.Empty);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Magnetometer calls the magnetometer provided by the operating system, and outputs `MagnetometerResult`_ .
    /// When it is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// It is not recommended to open multiple times simultaneously, which may cause failure on open or cause precision downgrade.
    /// </para>
    /// <para xml:lang="zh">
    /// Magnetometer调用了系统提供的磁力计，输出 `MagnetometerResult`_ 。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// 不建议同时打开多次，可能会无法使用或精度下降。
    /// </para>
    /// </summary>
    internal class Magnetometer : RefBase
    {
        internal Magnetometer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new Magnetometer(cdata_new, deleter_, retainer_);
        }
        public new Magnetometer Clone()
        {
            return (Magnetometer)(CloneObject());
        }
        public Magnetometer() : base(IntPtr.Zero, Detail.easyar_Magnetometer__dtor, Detail.easyar_Magnetometer__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_Magnetometer__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Android or iOS with supported hardware. On other operating systems, it is not supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Android或iOS上且硬件支持时返回true。其他操作系统暂不支持。
        /// </para>
        /// </summary>
        public virtual bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Magnetometer_isAvailable(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual MagnetometerResultSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Magnetometer_output(cdata, out _return_value_);
                return Detail.Object_from_c<MagnetometerResultSource>(_return_value_, Detail.easyar_MagnetometerResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device. Sampling period is defined by implementation. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 打开设备。采样周期由内部实现所确定。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool open()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Magnetometer_open(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens the device with a specific sampling period. Sampling period is limited by hardware and may not reach the specified value. If failed, it will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 以特定采样周期打开设备。采样周期会受到硬件限制而可能无法达到指定值。如果打开失败，返回false。
        /// </para>
        /// </summary>
        public virtual bool openWithSamplingPeriod(int samplingPeriodMilliseconds)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Magnetometer_openWithSamplingPeriod(cdata, samplingPeriodMilliseconds);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Closes. It shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭设备。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Magnetometer_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the most recent result. If there is no result, it returns empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前的最新结果。如果结果尚未获取到，返回空。
        /// </para>
        /// </summary>
        public virtual Optional<MagnetometerResult> getCurrentResult()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Magnetometer_getCurrentResult(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<MagnetometerResult>.Empty);
            }
        }
    }

    public enum ThreeDofCameraDeviceFocusMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Auto focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 自动对焦模式
        /// </para>
        /// </summary>
        Auto = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Fixed focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 固定对焦模式
        /// </para>
        /// </summary>
        Fixed = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ThreeDofCameraDevice implements a three dof camera device, which outputs `InputFrame`_ which `CameraTransformType`_ is ThreeDofRotOnly (including image, camera parameters, timestamp, camera transform matrix and tracking status).
    /// After creation, start/stop can be invoked to start or stop video stream capture.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// ThreeDofCameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for use.
    /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is more than this number, the device will not output new `InputFrame`_ , until previous `InputFrame`_ have been released. This may cause screen stuck.
    /// </para>
    /// <para xml:lang="zh">
    /// ThreeDofCameraDevice实现了一个three dof的camera设备，输出 `CameraTransformType`_ 为ThreeDofRotOnly的 `InputFrame`_ （包含图像、摄像机参数、时间戳、位姿转换矩阵和跟踪状态）。
    /// 创建之后，可以调用start/stop来开始和停止采集视频流数据。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// ThreeDofCameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
    /// </para>
    /// </summary>
    internal class ThreeDofCameraDevice : RefBase
    {
        internal ThreeDofCameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ThreeDofCameraDevice(cdata_new, deleter_, retainer_);
        }
        public new ThreeDofCameraDevice Clone()
        {
            return (ThreeDofCameraDevice)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Default create method.
        /// </para>
        /// <para xml:lang="zh">
        /// 默认创建方法。
        /// </para>
        /// </summary>
        public ThreeDofCameraDevice() : base(IntPtr.Zero, Detail.easyar_ThreeDofCameraDevice__dtor, Detail.easyar_ThreeDofCameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ThreeDofCameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Android or IOS when the gyroscope is available.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Android或iOS上且陀螺仪可用时返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `InputFrame`_ buffer capacity.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ThreeDofCameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ThreeDofCameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets focus mode to focusMode. Call before start.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置对焦模式为focusMode。在start之前调用。
        /// </para>
        /// </summary>
        public virtual void setFocusMode(ThreeDofCameraDeviceFocusMode focusMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ThreeDofCameraDevice_setFocusMode(cdata, focusMode);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the number of supported preview sizes. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的个数。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int supportedSizeCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_supportedSizeCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the index-th supported preview size. It returns {0, 0} if index is out of range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的第 index 个. 如果 index 超出范围则返回{0, 0}。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I supportedSize(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_supportedSize(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the preview size. The available nearest value will be selected. Call size to get the actual size. Call after a successful open. frameRateRange may change after calling setSize.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前图像大小。最接近设置值的可选值将被使用。可以使用size来获取实际的大小。在成功的open之后调用。设置size后frameRateRange可能会发生变化。
        /// </para>
        /// </summary>
        public virtual bool setSize(Vec2I size)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_setSize(cdata, size);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens back camera of device. If the opening fails will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 打开设备的后置camera，如果打开失败会返回false。
        /// </para>
        /// </summary>
        public virtual bool open()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_open(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始采集视频流数据。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止采集视频流数据。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ThreeDofCameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ThreeDofCameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets lower bound of current frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围下界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeLower()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_frameRateRangeLower(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets upper bound of current frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围上界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeUpper()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ThreeDofCameraDevice_frameRateRangeUpper(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// VisionOSARKitCameraDevice implements a camera device based on VisionOS ARKit, which outputs `InputFrame`_ (including image, camera parameters, timestamp, 6DOF location, and tracking status).
    /// After creation, start/stop can be invoked to start or stop data collection.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// VisionOSARKitCameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for use.
    /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is more than this number, the device will not output new `InputFrame`_ , until previous `InputFrame`_ have been released. This may cause screen stuck.
    /// </para>
    /// <para xml:lang="zh">
    /// VisionOSARKitCameraDevice实现了一个基于VisionOS ARKit的camera设备，输出 `InputFrame`_ （包含图像、摄像机参数、时间戳、6DOF位置信息和跟踪状态）。
    /// 创建之后，可以调用start/stop来开始和停止采集视频流数据。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// VisionOSARKitCameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
    /// </para>
    /// </summary>
    internal class VisionOSARKitCameraDevice : RefBase
    {
        internal VisionOSARKitCameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new VisionOSARKitCameraDevice(cdata_new, deleter_, retainer_);
        }
        public new VisionOSARKitCameraDevice Clone()
        {
            return (VisionOSARKitCameraDevice)(CloneObject());
        }
        public VisionOSARKitCameraDevice() : base(IntPtr.Zero, Detail.easyar_VisionOSARKitCameraDevice__dtor, Detail.easyar_VisionOSARKitCameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_VisionOSARKitCameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `InputFrame`_ buffer capacity.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VisionOSARKitCameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Requests necessary permissions from VisionOS including [World Sensing] and [Camera Access]
        /// </para>
        /// <para xml:lang="zh">
        /// 请求VisionOS上必要的系统权限, 包括[World Sensing]和[Camera Access]
        /// </para>
        /// </summary>
        public virtual void requestPermissions(CallbackScheduler callbackScheduler, Optional<Action<PermissionStatus, string>> permissionCallback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VisionOSARKitCameraDevice_requestPermissions(cdata, callbackScheduler.cdata, permissionCallback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromPermissionStatusAndString { has_value = true, value = Detail.FunctorOfVoidFromPermissionStatusAndString_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromPermissionStatusAndString { has_value = false, value = default(Detail.FunctorOfVoidFromPermissionStatusAndString) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VisionOSARKitCameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get synchoronous motion input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取同步的运动输入数据。
        /// </para>
        /// </summary>
        public virtual Optional<MotionInputData> getMotionInputData()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfMotionInputData);
                Detail.easyar_VisionOSARKitCameraDevice_getMotionInputData(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<MotionInputData>(p.value, Detail.easyar_MotionInputData__typeName) : Optional<MotionInputData>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始采集视频流数据。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止采集视频流数据。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VisionOSARKitCameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VisionOSARKitCameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets lower bound of current frame rate range.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围下界。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeLower()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_frameRateRangeLower(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets upper bound of current frame rate range.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围上界。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeUpper()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VisionOSARKitCameraDevice_frameRateRangeUpper(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// XREALCameraDevice implements a camera device based on XREAL Enterprise Native SDK Plugin, which outputs `InputFrame`_ (image, camera parameters, timestamp, 6DOF location, and tracking status cannot be accessed).
    /// After creation, start/stop can be invoked to start or stop data collection. Start must not be called before isDeviceSupported returns true.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// XREALCameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for use.
    /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is more than this number, the device will not output new `InputFrame`_ , until previous `InputFrame`_ have been released. This may cause screen stuck.
    /// </para>
    /// <para xml:lang="zh">
    /// XREALCameraDevice实现了一个基于XREAL Enterprise Native SDK Plugin的camera设备，输出 `InputFrame`_ （不可获取图像、摄像机参数、时间戳、6DOF位置信息和跟踪状态）。
    /// 创建之后，可以调用start/stop来开始和停止采集视频流数据。需要确保在isDeviceSupported返回true之后才能调用start。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// XREALCameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
    /// </para>
    /// </summary>
    internal class XREALCameraDevice : RefBase
    {
        internal XREALCameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new XREALCameraDevice(cdata_new, deleter_, retainer_);
        }
        public new XREALCameraDevice Clone()
        {
            return (XREALCameraDevice)(CloneObject());
        }
        public XREALCameraDevice() : base(IntPtr.Zero, Detail.easyar_XREALCameraDevice__dtor, Detail.easyar_XREALCameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_XREALCameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only when XREAL eyewear is connected on the device supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在支持XREAL眼镜的设备上且成功连接XREAL眼镜之后返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the eyewear is supported. Has value only after XREAL finishes initialization on devices where isAvailable is ture.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查眼镜是否被支持。在可用的设备上，只有XREAL完成初始化之后有值。
        /// </para>
        /// </summary>
        public static Optional<bool> isDeviceSupported()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_isDeviceSupported();
                return _return_value_.map(p => p.has_value ? p.value : Optional<bool>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `InputFrame`_ buffer capacity.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_XREALCameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_XREALCameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get synchoronous motion input data. Call after isDeviceSupported return true.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取同步的运动输入数据。在isDeviceSupported返回true之后使用。
        /// </para>
        /// </summary>
        public virtual Optional<MotionInputData> getMotionInputData()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfMotionInputData);
                Detail.easyar_XREALCameraDevice_getMotionInputData(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<MotionInputData>(p.value, Detail.easyar_MotionInputData__typeName) : Optional<MotionInputData>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Received frame count from camera. It should continously increase after start, otherwise the device connection may not be stable or XREAL service/SDK not working correctly, and you need to get help from XREAL directly.
        /// </para>
        /// <para xml:lang="zh">
        /// 从相机获取到的帧数。在start之后它应该持续增加，否则可能设备连接不稳定或XREAL服务/SDK工作不正常，这时需要直接从XREAL官方获取帮助。
        /// </para>
        /// </summary>
        public virtual int receivedFrameCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_receivedFrameCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens camera. Call after isDeviceSupported return true. XREAL license must be validated through XREAL official method on Air2 Ultra and some other devices before open.
        /// </para>
        /// <para xml:lang="zh">
        /// 打开camera设备。在isDeviceSupported返回true之后使用。在Air2 Ultra等设备上，XREAL 许可证必须先通过XREAL官方方法完成校验。
        /// </para>
        /// </summary>
        public virtual bool open()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_open(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始采集视频流数据。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止采集视频流数据。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_XREALCameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_XREALCameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current frame rate. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual double frameRate()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_XREALCameraDevice_frameRate(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Result of `SurfaceTracker`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// `SurfaceTracker`_ 的结果。
    /// </para>
    /// </summary>
    internal class SurfaceTrackerResult : FrameFilterResult
    {
        internal SurfaceTrackerResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SurfaceTrackerResult(cdata_new, deleter_, retainer_);
        }
        public new SurfaceTrackerResult Clone()
        {
            return (SurfaceTrackerResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera transform against world coordinate system. Camera coordinate system and world coordinate system are all right-handed. For the camera coordinate system, the origin is the optical center, x-right, y-up, and z in the direction of light going into camera. (The right and up, on mobile devices, is the right and up when the device is in the natural orientation.) For the world coordinate system, y is up (to the opposite of gravity). The data arrangement is row-major, not like OpenGL&#39;s column-major.
        /// </para>
        /// <para xml:lang="zh">
        /// Camera相对于世界坐标的变换。其中Camera坐标系与世界坐标系均为右手坐标系。Camera坐标系的原点为相机光心，x轴正方向为右，y轴正方向为上，z轴正方向为光线进入相机的方向。（其中的右和上，在移动设备上指设备自然方向的右和上。）世界坐标系的y轴向上（重力方向相反）。数据的排列方式为row-major，与OpenGL的column-major相反。
        /// </para>
        /// </summary>
        public virtual Matrix44F transform()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SurfaceTrackerResult_transform(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// SurfaceTracker implements tracking with environmental surfaces.
    /// SurfaceTracker occupies one buffer of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components.
    /// After creation, you can call start/stop to enable/disable the track process. start and stop are very lightweight calls.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// SurfaceTracker inputs `InputFrame`_ from inputFrameSink. `InputFrameSource`_ shall be connected to inputFrameSink for use.
    /// </para>
    /// <para xml:lang="zh">
    /// SurfaceTracker实现了对环境表面的跟踪。
    /// SurfaceTracker占用1个camera的buffer。应使用camera的setBufferCapacity设置不少于所有组件占用的camera的buffer数量。
    /// 创建之后，可以调用start/stop来开始和停止运行，start/stop是非常轻量的调用。
    /// 当不再需要该组件时，可以调用close对其进行关闭。close之后不应继续使用。
    /// SurfaceTracker通过inputFrameSink输入 `InputFrame`_ ，应将 `InputFrameSource`_ 连接到inputFrameSink上进行使用。
    /// </para>
    /// </summary>
    internal class SurfaceTracker : RefBase
    {
        internal SurfaceTracker(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SurfaceTracker(cdata_new, deleter_, retainer_);
        }
        public new SurfaceTracker Clone()
        {
            return (SurfaceTracker)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true only on Android or iOS when accelerometer and gyroscope are available.
        /// </para>
        /// <para xml:lang="zh">
        /// 只在Android、iOS系统上且加速度计、陀螺仪可用时返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SurfaceTracker_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ input port. InputFrame must have raw image, timestamp, and camera parameters.
        /// Note: fisheye camera model is NOT supported.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输入端口。InputFrame要求必须拥有图像、时间戳和camera参数。
        /// 注意：camera不支持鱼眼相机模型。
        /// </para>
        /// </summary>
        public virtual InputFrameSink inputFrameSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SurfaceTracker_inputFrameSink(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SurfaceTracker_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `OutputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `OutputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource outputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SurfaceTracker_outputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static SurfaceTracker create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SurfaceTracker_create(out _return_value_);
                return Detail.Object_from_c<SurfaceTracker>(_return_value_, Detail.easyar_SurfaceTracker__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts the track algorithm.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始跟踪算法。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SurfaceTracker_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops the track algorithm. Call start to start the track again.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停跟踪算法。调用start来重新启动跟踪。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SurfaceTracker_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SurfaceTracker_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the tracking target to a point on camera image. For the camera image coordinate system ([0, 1]^2), x-right, y-down, and origin is at left-top corner. `CameraParameters.imageCoordinatesFromScreenCoordinates`_ can be used to convert points from screen coordinate system to camera image coordinate system.
        /// </para>
        /// <para xml:lang="zh">
        /// 将跟踪目标点对准到相机图像的指定点。图像坐标系（[0, 1]^2）的x朝右、y朝下，原点在左上角。可以使用 `CameraParameters.imageCoordinatesFromScreenCoordinates`_ 来从屏幕坐标转换为图像坐标。
        /// </para>
        /// </summary>
        public virtual void alignTargetToCameraImagePoint(Vec2F cameraImagePoint)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SurfaceTracker_alignTargetToCameraImagePoint(cdata, cameraImagePoint);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Event dump recorder.
    /// It can be used to record some crucial diagnostic information into an EED file.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 事件转储录制器。
    /// 用于将一些关键诊断信息保存到EED文件中。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    public class EventDumpRecorder : RefBase
    {
        internal EventDumpRecorder(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new EventDumpRecorder(cdata_new, deleter_, retainer_);
        }
        public new EventDumpRecorder Clone()
        {
            return (EventDumpRecorder)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static EventDumpRecorder create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_EventDumpRecorder_create(out _return_value_);
                return Detail.Object_from_c<EventDumpRecorder>(_return_value_, Detail.easyar_EventDumpRecorder__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts recording.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始录制。
        /// </para>
        /// </summary>
        public virtual bool start(string filePath, int initalScreenRotation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_EventDumpRecorder_start(cdata, Detail.String_to_c(ar, filePath), initalScreenRotation);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops recording.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止录制。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_EventDumpRecorder_stop(cdata);
            }
        }
    }

    public enum MotionTrackerCameraDeviceQualityLevel
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The device does not support motion tracking. It has failed in calibration or is to be calibrated.
        /// </para>
        /// <para xml:lang="zh">
        /// 设备不支持运动跟踪，可能是适配不达标或者尚未适配
        /// </para>
        /// </summary>
        NotSupported = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// The device has not passed all our verification. Real time scale is unstable. It can be used in AR showcase or other desktop-scale applications.
        /// </para>
        /// <para xml:lang="zh">
        /// 设备不完全达标，尺度不稳定，可用于桌面尺度内的小场景，类似AR渲染等
        /// </para>
        /// </summary>
        Bad = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// The device has not passed all our verification. Real time scale is almost accurate. It can be used in AR showcase, AR games, AR navigation or other room-scale applications.
        /// </para>
        /// <para xml:lang="zh">
        /// 设备不完全达标，尺度接近准确，可用于房间尺度内的中等场景，类似AR游戏、AR渲染、AR导航等
        /// </para>
        /// </summary>
        Limited = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// The device has passed all our verification. Real time scale is accurate. It can be used in AR showcase, AR games, AR navigation, 3D reconstruction or other building-scale applications.
        /// </para>
        /// <para xml:lang="zh">
        /// 设备达标，尺度准确，可用于建筑物尺度的大型场景，类似AR游戏、AR渲染、AR导航、三维重建等
        /// </para>
        /// </summary>
        Good = 3,
    }

    public enum MotionTrackerCameraDeviceFPS
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Frame rate is 30 fps, the actual fps will vary with device capabilities. It is the default setting.
        /// </para>
        /// <para xml:lang="zh">
        /// 设备图像帧率是30fps，实际帧率取决于设备能力，它是默认设置
        /// </para>
        /// </summary>
        Camera_FPS_30 = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Frame rate is 60 fps or 30 fps, the actual fps will vary with device capabilities.
        /// </para>
        /// <para xml:lang="zh">
        /// 设备图像帧率是60fps或者30fps，实际帧率取决于设备能力
        /// </para>
        /// </summary>
        Camera_FPS_60 = 1,
    }

    public enum MotionTrackerCameraDeviceFocusMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Continuous auto focus mode, high image definition and good tracking performance. The actual focus mode will vary with device capabilities. It is the default setting.
        /// </para>
        /// <para xml:lang="zh">
        /// 连续自动对焦模式，图像清晰度高，跟踪效果一般。实际对焦效果取决于设备能力，它是默认设置
        /// </para>
        /// </summary>
        Continousauto = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Focus is fixed on Medium distance, medium image definition and better tracking performance. The actual focus mode will vary with device capabilities.
        /// </para>
        /// <para xml:lang="zh">
        /// 中等距离定焦模式，图像清晰度一般，跟踪效果较好，实际对焦效果取决于设备能力
        /// </para>
        /// </summary>
        Medium = 1,
    }

    public enum MotionTrackerCameraDeviceResolution
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Resolution is typically 1280 x 960 or 1280 x 720, the actual resolution will vary with device capabilities. It is the default setting.
        /// </para>
        /// <para xml:lang="zh">
        /// 标准分辨率是1280 x 960 或者 1280 x 720，实际分辨率取决于设备能力，它是默认设置
        /// </para>
        /// </summary>
        Resolution_1280 = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Resolution is typically 640 x 480 or 640 x 360, the actual resolution will vary with device capabilities.
        /// </para>
        /// <para xml:lang="zh">
        /// 标准分辨率是640 x 480 或者 640 x 360，实际分辨率取决于设备能力
        /// </para>
        /// </summary>
        Resolution_640 = 1,
    }

    public enum MotionTrackerCameraDeviceTrackingMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Tracking and hitTestAgainstPointCloud is supported, but plane detection, relocation and anchor are not. It has low CPU and memory usage.
        /// </para>
        /// <para xml:lang="zh">
        /// 只有跟踪和点击碰撞点云，CPU和内存占用少，但是不支持平面检测、重定位和锚点。
        /// </para>
        /// </summary>
        VIO = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// SLAM(Simultaneous tracking and mapping) supports relocation, plane detection and hitTestAgainstPointCloud. CPU and memory usage are higher. Real time pose correction of anchor is not supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 同时跟踪和建图，支持丢失后的重定位、点击碰撞点云和平面检测，但是没有锚点，不支持实时校正位姿，且CPU和内存占用稍高。
        /// </para>
        /// </summary>
        SLAM = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Anchor is SLAM(Simultaneous tracking and mapping) with real time pose correction. CPU and memory usage are highest. Anchor supports relocation, plane detection, hitTestAgainstPointCloud and pose correction. Anchor is automatically saved when hitTestAgainstPointCloud is called.
        /// </para>
        /// <para xml:lang="zh">
        /// 同时跟踪和建图，支持丢失后的重定位、点击碰撞点云、平面检测和锚点，但是CPU和内存占用最高。调用hitTestAgainstPointCloud时会自动保存锚点，通过锚点实时校正位姿。
        /// </para>
        /// </summary>
        Anchor = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// LargeScale is SLAM(Simultaneous tracking and mapping) with real time pose correction in large scenes.Tracking is more stable at a large depth of field. LargeScale supports relocation, plane detection, hitTestAgainstPointCloud and pose correction. Anchor is automatically saved when hitTestAgainstPointCloud is called.
        /// </para>
        /// <para xml:lang="zh">
        /// 适用于大场景下，同时跟踪和建图，支持丢失后的重定位、点击碰撞点云、平面检测和锚点。大景深下跟踪更稳定，调用hitTestAgainstPointCloud时会自动保存锚点，通过锚点实时校正位姿。
        /// </para>
        /// </summary>
        LargeScale = 3,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// MotionTrackerCameraDevice implements a camera device with metric-scale six degree-of-freedom motion tracking, which outputs `InputFrame`_  (including image, camera parameters, timestamp, 6DOF pose and tracking status).
    /// After creation, start/stop can be invoked to start or stop data flow.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// MotionTrackerCameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for further use.
    /// </para>
    /// <para xml:lang="zh">
    /// MotionTrackerCameraDevice实现了一个真实尺度6DOF运动追踪的camera设备，输出 `InputFrame`_ （包含图像、摄像机参数、时间戳、6DOF位置信息和跟踪状态）。
    /// 创建之后，可以调用start/stop来开始和停止数据流。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// MotionTrackerCameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// </para>
    /// </summary>
    internal class MotionTrackerCameraDevice : RefBase
    {
        internal MotionTrackerCameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MotionTrackerCameraDevice(cdata_new, deleter_, retainer_);
        }
        public new MotionTrackerCameraDevice Clone()
        {
            return (MotionTrackerCameraDevice)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Create MotionTrackerCameraDevice object.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建MotionTrackerCameraDevice对象。
        /// </para>
        /// </summary>
        public MotionTrackerCameraDevice() : base(IntPtr.Zero, Detail.easyar_MotionTrackerCameraDevice__dtor, Detail.easyar_MotionTrackerCameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_MotionTrackerCameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Check if the devices supports motion tracking. It returns True if the device supports Motion Tracking, otherwise it returns False.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查设备是否支持Motion Tracking. 当设备支持运动追踪功能时返回True，否则返回False。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the quality of Motion Tracking on the device. You can decide whether to start Motion Tracking.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取设备上Motion Tracking的质量，结合应用场景，可以通过此值判断是否启动Motion Tracking。
        /// </para>
        /// </summary>
        public static MotionTrackerCameraDeviceQualityLevel getQualityLevel()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_getQualityLevel();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets current frame rate. Call before start. If this function is not called, the default is 30 fps.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前设备的当前帧率。在start之前调用。如果该功能不被调用，默认是30fps
        /// </para>
        /// </summary>
        public virtual bool setFrameRateType(MotionTrackerCameraDeviceFPS fps)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_setFrameRateType(cdata, fps);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets focus mode. Call before start. If this function is not called, the default is Continousauto.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置对焦模式为focusMode。在start之前调用。如果该功能不被调用，默认是连续自动对焦
        /// </para>
        /// </summary>
        public virtual bool setFocusMode(MotionTrackerCameraDeviceFocusMode focusMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_setFocusMode(cdata, focusMode);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets frame resolution. Call before start. If this function is not called, the default is 1280 x 960 or 1280 x 720.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置帧分辨率。在start之前调用。如果该功能不被调用，默认是1280x960或者1280x720
        /// </para>
        /// </summary>
        public virtual bool setFrameResolutionType(MotionTrackerCameraDeviceResolution resolution)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_setFrameResolutionType(cdata, resolution);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets tracking mode. Call before start. If this function is not called, the default is Anchor.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置跟踪模式。在start之前调用。如果该功能不被调用，默认是锚点模式
        /// </para>
        /// </summary>
        public virtual bool setTrackingMode(MotionTrackerCameraDeviceTrackingMode trackingMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_setTrackingMode(cdata, trackingMode);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set `InputFrame`_ buffer capacity.
        /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is higher than this number, the device will not output new `InputFrame`_ until previous `InputFrame`_ has been released. This may cause screen stuck.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MotionTrackerCameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取`InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MotionTrackerCameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Start motion tracking or resume motion tracking after pause.
        /// Notice: Calling start after pausing will trigger device relocalization. Tracking will resume when the relocalization process succeeds.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始运动追踪，或者从暂停中触发重定位,成功后继续追踪。
        /// 注意：如果设备是调用stop暂停后再调用start追踪，会触发重定位，当重定位成功以后才会继续追踪。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Pause motion tracking. Call `start` to trigger relocation, resume motion tracking if the relocation succeeds.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停运动追踪。调用start触发重定位，重定位成功后继续运动追踪。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MotionTrackerCameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close motion tracking. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭运动追踪过程。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MotionTrackerCameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type. Call after a successful start.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。在成功的start之后调用。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation. Call after a successful start.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。在成功的start之后调用。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size. Call after a successful start.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。在成功的start之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets lower bound of current frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围下界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeLower()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_frameRateRangeLower(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets upper bound of current frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围上界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeUpper()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionTrackerCameraDevice_frameRateRangeUpper(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Perform hit test against the point cloud and return the nearest 3D point. The 3D point is represented by three consecutive values, representing X, Y, Z position coordinates in the world coordinate space.
        /// For the camera image coordinate system ([0, 1]^2), x-right, y-down, and origin is at left-top corner. `CameraParameters.imageCoordinatesFromScreenCoordinates`_ can be used to convert points from screen coordinate system to camera image coordinate system.
        /// </para>
        /// <para xml:lang="zh">
        /// 在当前点云中进行Hit Test,得到距离相机从近到远一条射线上的最近的一个3D点位置坐标。该点由三个连续的值表示，分别代表X，Y，Z轴上的坐标值。
        /// 输入图像坐标系（[0, 1]^2）的x朝右、y朝下，原点在左上角。可以使用 `CameraParameters.imageCoordinatesFromScreenCoordinates`_ 来从屏幕坐标转换为图像坐标。
        /// </para>
        /// </summary>
        public virtual List<Vec3F> hitTestAgainstPointCloud(Vec2F cameraImagePoint)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MotionTrackerCameraDevice_hitTestAgainstPointCloud(cdata, cameraImagePoint, out _return_value_);
                return Detail.ListOfVec3F_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Performs ray cast from the user&#39;s device in the direction of given screen point.
        /// Intersections with horizontal plane is detected in real time in the current field of view,and return the 3D point nearest to ray on horizontal plane.
        /// For the camera image coordinate system ([0, 1]^2), x-right, y-down, and origin is at left-top corner. `CameraParameters.imageCoordinatesFromScreenCoordinates`_ can be used to convert points from screen coordinate system to camera image coordinate system.
        /// The output point cloud coordinate on Horizontal plane is in the world coordinate system. The 3D point is represented by three consecutive values, representing X, Y, Z position coordinates in the world coordinate space.
        /// </para>
        /// <para xml:lang="zh">
        /// 在当前视野内实时检测到的水平面上进行Hit Test,点击到某个水平面后返回该平面上距离Hit Test射线最近的3D点的位置坐标。
        /// 输入图像坐标系（[0, 1]^2）的x朝右、y朝下，原点在左上角。可以使用 `CameraParameters.imageCoordinatesFromScreenCoordinates`_ 来从屏幕坐标转换为图像坐标。
        /// 输出为平面上的点云在世界坐标系中的坐标。每一个点由三个连续的值表示，分别代表X，Y，Z轴上的坐标值。
        /// </para>
        /// </summary>
        public virtual List<Vec3F> hitTestAgainstHorizontalPlane(Vec2F cameraImagePoint)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MotionTrackerCameraDevice_hitTestAgainstHorizontalPlane(cdata, cameraImagePoint, out _return_value_);
                return Detail.ListOfVec3F_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the vector of point cloud coordinate. Each 3D point is represented by three consecutive values, representing X, Y, Z position coordinates in the world coordinate space.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前点云数据位置信息。其中点云位置为世界坐标系中的位置，每一个点由三个连续的值表示，分别代表X，Y，Z轴上的坐标值。
        /// </para>
        /// </summary>
        public virtual List<Vec3F> getLocalPointsCloud()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_MotionTrackerCameraDevice_getLocalPointsCloud(cdata, out _return_value_);
                return Detail.ListOfVec3F_from_c(ar, _return_value_);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame recorder.
    /// There is an input frame input port and an input frame output port. It can be used to record input frames into an EIF file.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧录制器。
    /// 有一个输入帧输入端口和一个输入帧输出端口，用于将经过的输入帧保存到EIF文件中。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class InputFrameRecorder : RefBase
    {
        internal InputFrameRecorder(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrameRecorder(cdata_new, deleter_, retainer_);
        }
        public new InputFrameRecorder Clone()
        {
            return (InputFrameRecorder)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameRecorder_input(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrameRecorder_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameRecorder_output(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static InputFrameRecorder create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameRecorder_create(out _return_value_);
                return Detail.Object_from_c<InputFrameRecorder>(_return_value_, Detail.easyar_InputFrameRecorder__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts frame recording. initialScreenRotation is the initial screen rotation when recording data, and it will be used in rendering for playback. The definition of screen rotation is the same as in `CameraParameters`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 开始录制数据。initialScreenRotation为录制数据时的初始屏幕旋转方向，播放时以此方向为基准进行渲染。旋转方向的定义与 `CameraParameters`_ 中的一致。
        /// </para>
        /// </summary>
        public virtual bool start(string filePath, int initialScreenRotation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrameRecorder_start(cdata, Detail.String_to_c(ar, filePath), initialScreenRotation);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops frame recording. It will only stop recording and will not affect connection.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止录制数据。这个方法只会停止录制，连接将不会受到影响。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InputFrameRecorder_stop(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame player.
    /// There is an input frame output port. It can be used to get input frame from an EIF file.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧播放器。
    /// 有一个输入帧输出端口，用于从EIF文件将输入帧取出。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class InputFramePlayer : RefBase
    {
        internal InputFramePlayer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFramePlayer(cdata_new, deleter_, retainer_);
        }
        public new InputFramePlayer Clone()
        {
            return (InputFramePlayer)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFramePlayer_output(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static InputFramePlayer create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFramePlayer_create(out _return_value_);
                return Detail.Object_from_c<InputFramePlayer>(_return_value_, Detail.easyar_InputFramePlayer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始播放。
        /// </para>
        /// </summary>
        public virtual bool start(string filePath)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFramePlayer_start(cdata, Detail.String_to_c(ar, filePath));
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止播放。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InputFramePlayer_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Pauses frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停播放。
        /// </para>
        /// </summary>
        public virtual void pause()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InputFramePlayer_pause(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Resumes frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 继续播放。
        /// </para>
        /// </summary>
        public virtual bool resume()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFramePlayer_resume(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Total expected playback time span. The unit is second.
        /// </para>
        /// <para xml:lang="zh">
        /// 预期的总播放时间。单位为秒。
        /// </para>
        /// </summary>
        public virtual double totalTime()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFramePlayer_totalTime(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current playback time point. The unit is second. The initial value is 0.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前的播放时刻。单位为秒。初始为0。
        /// </para>
        /// </summary>
        public virtual double currentTime()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFramePlayer_currentTime(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The initial screen rotation when recording data.
        /// </para>
        /// <para xml:lang="zh">
        /// 录制数据时的初始屏幕旋转方向。
        /// </para>
        /// </summary>
        public virtual int initalScreenRotation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFramePlayer_initalScreenRotation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Whether the playback is completed.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取是否已完成。
        /// </para>
        /// </summary>
        public virtual bool isCompleted()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFramePlayer_isCompleted(cdata);
                return _return_value_;
            }
        }
    }

    internal enum VideoInputFrameRecorderVideoCodec
    {
        H264 = 0,
    }

    internal enum VideoInputFrameRecorderCompletionReason
    {
        DiskFull = 0,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame recorder.
    /// There is an input frame input port and an input frame output port. It can be used to record input frames into an EIF MKV file.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧录制器。
    /// 有一个输入帧输入端口和一个输入帧输出端口，用于将经过的输入帧保存到EIF MKV文件中。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class VideoInputFrameRecorder : RefBase
    {
        internal VideoInputFrameRecorder(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new VideoInputFrameRecorder(cdata_new, deleter_, retainer_);
        }
        public new VideoInputFrameRecorder Clone()
        {
            return (VideoInputFrameRecorder)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on MacOS, iOS and Android.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在MacOS、iOS、Android上返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFrameRecorder_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_input(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for accelerometer result. See also `AccelerometerResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。加速度计结果输入端口。参考 `AccelerometerResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual AccelerometerResultSink accelerometerResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_accelerometerResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<AccelerometerResultSink>(_return_value_, Detail.easyar_AccelerometerResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for gyroscope result. See also `GyroscopeResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。陀螺仪结果输入端口。参考 `GyroscopeResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual GyroscopeResultSink gyroscopeResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_gyroscopeResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<GyroscopeResultSink>(_return_value_, Detail.easyar_GyroscopeResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for attitude sensor. See also `AttitudeSensorResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。姿态传感器结果输入端口。参考 `AttitudeSensorResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual AttitudeSensorResultSink attitudeSensorResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_attitudeSensorResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<AttitudeSensorResultSink>(_return_value_, Detail.easyar_AttitudeSensorResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for magnetometer. See also `MagnetometerResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。磁力计结果输入端口。参考 `MagnetometerResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual MagnetometerResultSink magnetometerResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_magnetometerResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<MagnetometerResultSink>(_return_value_, Detail.easyar_MagnetometerResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for location result. See also `LocationResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。位置结果输入端口。参考 `LocationResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual LocationResultSink locationResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_locationResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<LocationResultSink>(_return_value_, Detail.easyar_LocationResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optional. Input port for proximity location result. See also `ProximityLocationResultSink`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 可选。邻近位置结果输入端口。参考 `ProximityLocationResultSink`_ 。
        /// </para>
        /// </summary>
        public virtual ProximityLocationResultSink proximityLocationResultSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_proximityLocationResultSink(cdata, out _return_value_);
                return Detail.Object_from_c<ProximityLocationResultSink>(_return_value_, Detail.easyar_ProximityLocationResultSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFrameRecorder_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_output(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static VideoInputFrameRecorder create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFrameRecorder_create(out _return_value_);
                return Detail.Object_from_c<VideoInputFrameRecorder>(_return_value_, Detail.easyar_VideoInputFrameRecorder__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts frame recording. The definitions of cameraDeviceType, cameraOrientation and cameraOrientation are the same as in `CameraParameters`_ . initialScreenRotation is the initial screen rotation when recording data, and it will be used in rendering for playback.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始录制数据。cameraDeviceType、cameraOrientation和initialScreenRotation的定义与 `CameraParameters`_ 中的一致。initialScreenRotation为录制数据时的初始屏幕旋转方向，播放时以此方向为基准进行渲染。
        /// </para>
        /// </summary>
        public virtual bool start(string filePath, CameraDeviceType cameraDeviceType, int cameraOrientation, int initialScreenRotation, VideoInputFrameRecorderVideoCodec codec, int width, int height, double frameRate, string metadata)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFrameRecorder_start(cdata, Detail.String_to_c(ar, filePath), cameraDeviceType, cameraOrientation, initialScreenRotation, codec, width, height, frameRate, Detail.String_to_c(ar, metadata));
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops frame recording. It will only stop recording and will not affect connection.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止录制数据。这个方法只会停止录制，连接将不会受到影响。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoInputFrameRecorder_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets callback for automatic completion of recording.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置自动完成录制回调。
        /// </para>
        /// </summary>
        public virtual void setAutomaticCompletionCallback(CallbackScheduler callbackScheduler, Optional<Action<VideoInputFrameRecorderCompletionReason>> automaticCompletionCallback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoInputFrameRecorder_setAutomaticCompletionCallback(cdata, callbackScheduler.cdata, automaticCompletionCallback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromVideoInputFrameRecorderCompletionReason { has_value = true, value = Detail.FunctorOfVoidFromVideoInputFrameRecorderCompletionReason_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromVideoInputFrameRecorderCompletionReason { has_value = false, value = default(Detail.FunctorOfVoidFromVideoInputFrameRecorderCompletionReason) }));
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame player.
    /// There is an input frame output port. It can be used to get input frame from an EIF MKV file.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧播放器。
    /// 有一个输入帧输出端口，用于从EIF MKV文件将输入帧取出。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class VideoInputFramePlayer : RefBase
    {
        internal VideoInputFramePlayer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new VideoInputFramePlayer(cdata_new, deleter_, retainer_);
        }
        public new VideoInputFramePlayer Clone()
        {
            return (VideoInputFramePlayer)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Windows.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Windows上返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_output(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for accelerometer result. See also `AccelerometerResultSource`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 加速度计结果输出端口。参考 `AccelerometerResultSource`_ 。
        /// </para>
        /// </summary>
        public virtual AccelerometerResultSource accelerometerResultSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_accelerometerResultSource(cdata, out _return_value_);
                return Detail.Object_from_c<AccelerometerResultSource>(_return_value_, Detail.easyar_AccelerometerResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for gyroscope result. See also `GyroscopeResultSource`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 陀螺仪结果输出端口。参考 `GyroscopeResultSource`_ 。
        /// </para>
        /// </summary>
        public virtual GyroscopeResultSource gyroscopeResultSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_gyroscopeResultSource(cdata, out _return_value_);
                return Detail.Object_from_c<GyroscopeResultSource>(_return_value_, Detail.easyar_GyroscopeResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for attitude sensor. See also `AttitudeSensorResultSource`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 姿态传感器结果输出端口。参考 `AttitudeSensorResultSource`_ 。
        /// </para>
        /// </summary>
        public virtual AttitudeSensorResultSource attitudeSensorResultSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_attitudeSensorResultSource(cdata, out _return_value_);
                return Detail.Object_from_c<AttitudeSensorResultSource>(_return_value_, Detail.easyar_AttitudeSensorResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for magnetometer. See also `MagnetometerResultSource`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 磁力计结果输出端口。参考 `MagnetometerResultSource`_ 。
        /// </para>
        /// </summary>
        public virtual MagnetometerResultSource magnetometerResultSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_magnetometerResultSource(cdata, out _return_value_);
                return Detail.Object_from_c<MagnetometerResultSource>(_return_value_, Detail.easyar_MagnetometerResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for location result. See also `LocationResultSource`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 位置结果输出端口。参考 `LocationResultSource`_ 。
        /// </para>
        /// </summary>
        public virtual LocationResultSource locationResultSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_locationResultSource(cdata, out _return_value_);
                return Detail.Object_from_c<LocationResultSource>(_return_value_, Detail.easyar_LocationResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for proximity location result. See also `ProximityLocationResultSource`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 邻近位置结果输出端口。参考 `ProximityLocationResultSource`_ 。
        /// </para>
        /// </summary>
        public virtual ProximityLocationResultSource proximityLocationResultSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_proximityLocationResultSource(cdata, out _return_value_);
                return Detail.Object_from_c<ProximityLocationResultSource>(_return_value_, Detail.easyar_ProximityLocationResultSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static VideoInputFramePlayer create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_create(out _return_value_);
                return Detail.Object_from_c<VideoInputFramePlayer>(_return_value_, Detail.easyar_VideoInputFramePlayer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始播放。
        /// </para>
        /// </summary>
        public virtual bool start(string filePath)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_start(cdata, Detail.String_to_c(ar, filePath));
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止播放。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoInputFramePlayer_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Pauses frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停播放。
        /// </para>
        /// </summary>
        public virtual void pause()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoInputFramePlayer_pause(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Resumes frame play.
        /// </para>
        /// <para xml:lang="zh">
        /// 继续播放。
        /// </para>
        /// </summary>
        public virtual bool resume()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_resume(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Total expected playback time span. The unit is second. If recording halts improperly, it may be missing.
        /// </para>
        /// <para xml:lang="zh">
        /// 预期的总播放时间。单位为秒。录制过程非正常中断时，可能缺少。
        /// </para>
        /// </summary>
        public virtual Optional<double> totalTime()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_totalTime(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<double>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current playback time point. The unit is second. The initial value is 0.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前的播放时刻。单位为秒。初始为0。
        /// </para>
        /// </summary>
        public virtual double currentTime()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_currentTime(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Whether the current playback time point can be relocated. If recording halts improperly, index data to set playback time point may be missing.
        /// </para>
        /// <para xml:lang="zh">
        /// 是否可定位当前播放时刻。录制过程非正常中断时，可能导致缺少索引数据，而无法设定当前播放时间。
        /// </para>
        /// </summary>
        public virtual bool isSeekable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_isSeekable(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets current playback time point. The unit is second. If index data is missing, it returns false.
        /// </para>
        /// <para xml:lang="zh">
        /// 设定当前播放时刻。单位为秒。如果缺少索引数据，则返回false。
        /// </para>
        /// </summary>
        public virtual bool seek(double time)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_seek(cdata, time);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Current playback speed. The initial value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前的播放速度。初始为1。
        /// </para>
        /// </summary>
        public virtual double speed()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_speed(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets current playback speed.
        /// </para>
        /// <para xml:lang="zh">
        /// 设定当前播放速度。
        /// </para>
        /// </summary>
        public virtual void setSpeed(double @value)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoInputFramePlayer_setSpeed(cdata, @value);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The initial screen rotation when recording data.
        /// </para>
        /// <para xml:lang="zh">
        /// 录制数据时的初始屏幕旋转方向。
        /// </para>
        /// </summary>
        public virtual int initalScreenRotation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_initalScreenRotation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The metadata when recording data.
        /// </para>
        /// <para xml:lang="zh">
        /// 录制数据时的元数据。
        /// </para>
        /// </summary>
        public virtual string metadata()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_VideoInputFramePlayer_metadata(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Whether the playback is completed.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取是否已完成。
        /// </para>
        /// </summary>
        public virtual bool isCompleted()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoInputFramePlayer_isCompleted(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Image helper class.
    /// </para>
    /// <para xml:lang="zh">
    /// 图像帮助类。
    /// </para>
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Decodes a JPEG or PNG file.
        /// </para>
        /// <para xml:lang="zh">
        /// 解码一个JPEG或PNG文件。
        /// </para>
        /// </summary>
        public static Optional<Image> decode(Buffer buffer)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfImage);
                Detail.easyar_ImageHelper_decode(buffer.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<Image>(p.value, Detail.easyar_Image__typeName) : Optional<Image>.Empty);
            }
        }
    }

    public enum InertialCameraDeviceFocusMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Auto focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 自动对焦模式
        /// </para>
        /// </summary>
        Auto = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Fixed focus mode
        /// </para>
        /// <para xml:lang="zh">
        /// 固定对焦模式
        /// </para>
        /// </summary>
        Fixed = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// InertialCameraDevice implements an inertial camera device, which outputs `InputFrame`_ which `CameraTransformType`_ is FiveDofRotXZ (including image, camera parameters, timestamp, camera transform matrix and tracking status).
    /// After creation, start/stop can be invoked to start or stop video stream capture.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// InertialCameraDevice outputs `InputFrame`_ from inputFrameSource. inputFrameSource shall be connected to `InputFrameSink`_ for use.
    /// bufferCapacity is the capacity of `InputFrame`_ buffer. If the count of `InputFrame`_ which has been output from the device and have not been released is more than this number, the device will not output new `InputFrame`_ , until previous `InputFrame`_ have been released. This may cause screen stuck.
    /// </para>
    /// <para xml:lang="zh">
    /// InertialCameraDevice实现了一个基于运动惯性的camera设备，输出 `CameraTransformType`_ 为FiveDofRotXZ的 `InputFrame`_ （包含图像、摄像机参数、时间戳、位姿转换矩阵和跟踪状态）。
    /// 创建之后，可以调用start/stop来开始和停止采集视频流数据。
    /// 当不再需要该设备时，可以调用close对其进行关闭。close之后不应继续使用。
    /// InertialCameraDevice通过inputFrameSource输出 `InputFrame`_ ，应将inputFrameSource连接到 `InputFrameSink`_ 上进行使用。
    /// bufferCapacity表示 `InputFrame`_ 缓冲的容量，如果有超过此数量的 `InputFrame`_ 从该设备中输出并且没有被释放，该设备将不再输出新的 `InputFrame`_ ，直到之前的 `InputFrame`_ 被释放。这可能造成画面卡住等问题。
    /// </para>
    /// </summary>
    internal class InertialCameraDevice : RefBase
    {
        internal InertialCameraDevice(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InertialCameraDevice(cdata_new, deleter_, retainer_);
        }
        public new InertialCameraDevice Clone()
        {
            return (InertialCameraDevice)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Default create method.
        /// </para>
        /// <para xml:lang="zh">
        /// 默认创建方法。
        /// </para>
        /// </summary>
        public InertialCameraDevice() : base(IntPtr.Zero, Detail.easyar_InertialCameraDevice__dtor, Detail.easyar_InertialCameraDevice__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_InertialCameraDevice__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Android 7.0 (API Level 24+) when accelerometer, gyroscope and attitude sensor are all available.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Android 7.0及上(API Level 24+)且加速度计、陀螺仪、姿态传感器可用时返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ buffer capacity. The default is 8.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 缓冲的容量，默认值为8。
        /// </para>
        /// </summary>
        public virtual int bufferCapacity()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_bufferCapacity(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets `InputFrame`_ buffer capacity.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置 `InputFrame`_ 缓冲的容量。
        /// </para>
        /// </summary>
        public virtual void setBufferCapacity(int capacity)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InertialCameraDevice_setBufferCapacity(cdata, capacity);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `InputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `InputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource inputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InertialCameraDevice_inputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets focus mode to focusMode. Call before start.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置对焦模式为focusMode。在start之前调用。
        /// </para>
        /// </summary>
        public virtual void setFocusMode(InertialCameraDeviceFocusMode focusMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InertialCameraDevice_setFocusMode(cdata, focusMode);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera type. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera类型。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType type()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_type(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the current preview size. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前图像大小。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the number of supported preview sizes. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的个数。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual int supportedSizeCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_supportedSizeCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the index-th supported preview size. It returns {0, 0} if index is out of range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前设备支持的所有图像大小的第 index 个. 如果 index 超出范围则返回{0, 0}。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual Vec2I supportedSize(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_supportedSize(cdata, index);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the preview size. The available nearest value will be selected. Call size to get the actual size. Call after a successful open. frameRateRange may change after calling setSize.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前图像大小。最接近设置值的可选值将被使用。可以使用size来获取实际的大小。在成功的open之后调用。设置size后frameRateRange可能会发生变化。
        /// </para>
        /// </summary>
        public virtual bool setSize(Vec2I size)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_setSize(cdata, size);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens back camera of device. If the opening fails will return false.
        /// </para>
        /// <para xml:lang="zh">
        /// 打开设备的后置camera，如果打开失败会返回false。
        /// </para>
        /// </summary>
        public virtual bool open()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_open(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始采集视频流数据。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops video stream capture.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止采集视频流数据。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InertialCameraDevice_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Resets states.
        /// </para>
        /// <para xml:lang="zh">
        /// 重置状态。
        /// </para>
        /// </summary>
        public virtual void reset()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InertialCameraDevice_reset(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InertialCameraDevice_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets lower bound of current frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围下界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeLower()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_frameRateRangeLower(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets upper bound of current frame rate range. Call after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前帧率范围上界。在成功的open之后调用。
        /// </para>
        /// </summary>
        public virtual double frameRateRangeUpper()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InertialCameraDevice_frameRateRangeUpper(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Callback scheduler.
    /// There are two subclasses: `DelayedCallbackScheduler`_ and `ImmediateCallbackScheduler`_ .
    /// `DelayedCallbackScheduler`_ is used to delay callback to be invoked manually, and it can be used in single-threaded environments (such as various UI environments).
    /// `ImmediateCallbackScheduler`_ is used to mark callback to be invoked when event is dispatched, and it can be used in multi-threaded environments (such as server or service daemon).
    /// </para>
    /// <para xml:lang="zh">
    /// 回调调度器。
    /// 有两个子类 `DelayedCallbackScheduler`_ 和 `ImmediateCallbackScheduler`_ 。
    /// 其中 `DelayedCallbackScheduler`_ 用于将回调推迟到手动调用的时候调用，可用于单线程环境下（如各种UI环境）。
    /// `ImmediateCallbackScheduler`_ 用于将回调立即执行，可用于多线程环境下（如服务器或后台服务）。
    /// </para>
    /// </summary>
    internal class CallbackScheduler : RefBase
    {
        internal CallbackScheduler(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CallbackScheduler(cdata_new, deleter_, retainer_);
        }
        public new CallbackScheduler Clone()
        {
            return (CallbackScheduler)(CloneObject());
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Delayed callback scheduler.
    /// It is used to delay callback to be invoked manually, and it can be used in single-threaded environments (such as various UI environments).
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 延时回调调度器。
    /// 用于将回调推迟到手动调用的时候调用，可用于单线程环境下（如各种UI环境）。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class DelayedCallbackScheduler : CallbackScheduler
    {
        internal DelayedCallbackScheduler(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new DelayedCallbackScheduler(cdata_new, deleter_, retainer_);
        }
        public new DelayedCallbackScheduler Clone()
        {
            return (DelayedCallbackScheduler)(CloneObject());
        }
        public DelayedCallbackScheduler() : base(IntPtr.Zero, Detail.easyar_DelayedCallbackScheduler__dtor, Detail.easyar_DelayedCallbackScheduler__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_DelayedCallbackScheduler__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Executes a callback. If there is no callback to execute, false is returned.
        /// </para>
        /// <para xml:lang="zh">
        /// 执行一个回调。如果没有回调可执行，则返回false。
        /// </para>
        /// </summary>
        public virtual bool runOne()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_DelayedCallbackScheduler_runOne(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Immediate callback scheduler.
    /// It is used to mark callback to be invoked when event is dispatched, and it can be used in multi-threaded environments (such as server or service daemon).
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 立即回调调度器。
    /// 用于将回调立即执行，可用于多线程环境下（如服务器或后台服务）。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class ImmediateCallbackScheduler : CallbackScheduler
    {
        internal ImmediateCallbackScheduler(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ImmediateCallbackScheduler(cdata_new, deleter_, retainer_);
        }
        public new ImmediateCallbackScheduler Clone()
        {
            return (ImmediateCallbackScheduler)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets a default immediate callback scheduler.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得默认的立即回调调度器。
        /// </para>
        /// </summary>
        public static ImmediateCallbackScheduler getDefault()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImmediateCallbackScheduler_getDefault(out _return_value_);
                return Detail.Object_from_c<ImmediateCallbackScheduler>(_return_value_, Detail.easyar_ImmediateCallbackScheduler__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// JNI utility class.
    /// It is used in Unity to wrap Java byte array and ByteBuffer.
    /// It is not supported on iOS.
    /// </para>
    /// <para xml:lang="zh">
    /// JNI工具类。
    /// 用于在Unity中对Java的数组和ByteBuffer进行包装。
    /// 不支持iOS平台。
    /// </para>
    /// </summary>
    internal class JniUtility
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Wraps Java&#39;s byte[]。
        /// </para>
        /// <para xml:lang="zh">
        /// 包装Java的字节数组byte[]。
        /// </para>
        /// </summary>
        public static Buffer wrapByteArray(IntPtr bytes, bool readOnly, Action deleter)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_JniUtility_wrapByteArray(bytes, readOnly, Detail.FunctorOfVoid_to_c(deleter), out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Wraps Java&#39;s java.nio.ByteBuffer, which must be a direct buffer.
        /// </para>
        /// <para xml:lang="zh">
        /// 包装Java的java.nio.ByteBuffer中的direct buffer。
        /// </para>
        /// </summary>
        public static Buffer wrapBuffer(IntPtr directBuffer, Action deleter)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_JniUtility_wrapBuffer(directBuffer, Detail.FunctorOfVoid_to_c(deleter), out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the raw address of a direct buffer of java.nio.ByteBuffer by calling JNIEnv-&gt;GetDirectBufferAddress.
        /// </para>
        /// <para xml:lang="zh">
        /// 调用JNIEnv-&gt;GetDirectBufferAddress获得java.nio.ByteBuffer中的direct buffer的原始地址。
        /// </para>
        /// </summary>
        public static IntPtr getDirectBufferAddress(IntPtr directBuffer)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_JniUtility_getDirectBufferAddress(directBuffer);
                return _return_value_;
            }
        }
    }

    public enum LogLevel
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Error
        /// </para>
        /// <para xml:lang="zh">
        /// 错误
        /// </para>
        /// </summary>
        Error = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Warning
        /// </para>
        /// <para xml:lang="zh">
        /// 警告
        /// </para>
        /// </summary>
        Warning = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Information
        /// </para>
        /// <para xml:lang="zh">
        /// 信息
        /// </para>
        /// </summary>
        Info = 2,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Log class.
    /// It is used to output log or setup a custom log output function.
    /// </para>
    /// <para xml:lang="zh">
    /// 日志类。
    /// 用于输出日志或设置自定义日志输出函数。
    /// </para>
    /// </summary>
    internal class Log
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Outputs a log.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出日志。
        /// </para>
        /// </summary>
        public static void logMessage(LogLevel level, string message)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Log_logMessage(level, Detail.String_to_c(ar, message));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets custom log output function. The log output function should be thread-safe.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置自定义日志输出函数。此日志输出函数应保证线程安全。
        /// </para>
        /// </summary>
        public static void setLogFunc(Action<LogLevel, string> func)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Log_setLogFunc(Detail.FunctorOfVoidFromLogLevelAndString_to_c(func));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets custom log output function and run it in scheduler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置自定义日志输出函数，并通过scheduler来指定运行时机。
        /// </para>
        /// </summary>
        public static void setLogFuncWithScheduler(CallbackScheduler scheduler, Action<LogLevel, string> func)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Log_setLogFuncWithScheduler(scheduler.cdata, Detail.FunctorOfVoidFromLogLevelAndString_to_c(func));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Clears custom log output function and reverts to default log output function.
        /// </para>
        /// <para xml:lang="zh">
        /// 清除自定义日志输出函数，还原成默认的日志输出函数。
        /// </para>
        /// </summary>
        public static void resetLogFunc()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Log_resetLogFunc();
            }
        }
    }

    internal class Storage
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Sets asset root path.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置asset根路径。
        /// </para>
        /// </summary>
        public static void setAssetDirPath(string path)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Storage_setAssetDirPath(Detail.String_to_c(ar, path));
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ImageTargetParameters represents the parameters to create a `ImageTarget`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// ImageTargetParameters表示创建 `ImageTarget`_ 所需要的参数。
    /// </para>
    /// </summary>
    public class ImageTargetParameters : RefBase
    {
        internal ImageTargetParameters(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ImageTargetParameters(cdata_new, deleter_, retainer_);
        }
        public new ImageTargetParameters Clone()
        {
            return (ImageTargetParameters)(CloneObject());
        }
        public ImageTargetParameters() : base(IntPtr.Zero, Detail.easyar_ImageTargetParameters__dtor, Detail.easyar_ImageTargetParameters__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ImageTargetParameters__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets image.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取图像。
        /// </para>
        /// </summary>
        public virtual Image image()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTargetParameters_image(cdata, out _return_value_);
                return Detail.Object_from_c<Image>(_return_value_, Detail.easyar_Image__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets image.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置图像。
        /// </para>
        /// </summary>
        public virtual void setImage(Image image)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTargetParameters_setImage(cdata, image.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets target name. It can be used to distinguish targets.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target名字。名字用来区分target。
        /// </para>
        /// </summary>
        public virtual string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTargetParameters_name(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets target name.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置target名字。
        /// </para>
        /// </summary>
        public virtual void setName(string name)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTargetParameters_setName(cdata, Detail.String_to_c(ar, name));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the target uid. A target uid is useful in cloud based algorithms. If no cloud is used, you can set this uid in the json config as an alternative method to distinguish from targets.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target uid。target uid在云识别算法中使用。在没有接入云识别的时候，你可以在json配置中设置这个uid，在自己的代码中作为另一种区分target的方法。
        /// </para>
        /// </summary>
        public virtual string uid()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTargetParameters_uid(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets target uid.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置target uid。
        /// </para>
        /// </summary>
        public virtual void setUid(string uid)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTargetParameters_setUid(cdata, Detail.String_to_c(ar, uid));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets meta data.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取meta data。
        /// </para>
        /// </summary>
        public virtual string meta()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTargetParameters_meta(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets meta data。
        /// </para>
        /// <para xml:lang="zh">
        /// 设置meta data。
        /// </para>
        /// </summary>
        public virtual void setMeta(string meta)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTargetParameters_setMeta(cdata, Detail.String_to_c(ar, meta));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the scale of image. The value is the physical image width divided by 1 meter. The default value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 图像的缩放比例。其值为图像宽度的物理大小与1米的比值，默认值为1。
        /// </para>
        /// </summary>
        public virtual float scale()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTargetParameters_scale(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the scale of image. The value is the physical image width divided by 1 meter. The default value is 1.
        /// It is needed to set the model scale in rendering engine separately.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置图像的缩放比例。其值为图像宽度的物理大小与1米的比值，默认值为1。
        /// 还需要在渲染引擎中单独设置此模型缩放。
        /// </para>
        /// </summary>
        public virtual void setScale(float scale)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTargetParameters_setScale(cdata, scale);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ImageTarget represents planar image targets that can be tracked by `ImageTracker`_ .
    /// The fields of ImageTarget need to be filled with the create... method before it can be read. And ImageTarget can be tracked by `ImageTracker`_ after a successful load into the `ImageTracker`_ using `ImageTracker.loadTarget`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// ImageTarget表示平面图像的target，它可以被 `ImageTracker`_ 所跟踪。
    /// ImageTarget内的数值在可以被读取之前需要首先通过create...等方法填入。然后再通过 `ImageTracker.loadTarget`_ 成功载入 `ImageTracker`_ 之后可以被 `ImageTracker`_ 检测和跟踪。
    /// </para>
    /// </summary>
    public class ImageTarget : Target
    {
        internal ImageTarget(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ImageTarget(cdata_new, deleter_, retainer_);
        }
        public new ImageTarget Clone()
        {
            return (ImageTarget)(CloneObject());
        }
        public ImageTarget() : base(IntPtr.Zero, Detail.easyar_ImageTarget__dtor, Detail.easyar_ImageTarget__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ImageTarget__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a target from parameters.
        /// </para>
        /// <para xml:lang="zh">
        /// 从参数创建。
        /// </para>
        /// </summary>
        public static Optional<ImageTarget> createFromParameters(ImageTargetParameters parameters)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfImageTarget);
                Detail.easyar_ImageTarget_createFromParameters(parameters.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ImageTarget>(p.value, Detail.easyar_ImageTarget__typeName) : Optional<ImageTarget>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a target from an etd file.
        /// </para>
        /// <para xml:lang="zh">
        /// 从etd文件创建。
        /// </para>
        /// </summary>
        public static Optional<ImageTarget> createFromTargetFile(string path, StorageType storageType)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfImageTarget);
                Detail.easyar_ImageTarget_createFromTargetFile(Detail.String_to_c(ar, path), storageType, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ImageTarget>(p.value, Detail.easyar_ImageTarget__typeName) : Optional<ImageTarget>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a target from an etd data buffer.
        /// </para>
        /// <para xml:lang="zh">
        /// 从etd数据缓存创建。
        /// </para>
        /// </summary>
        public static Optional<ImageTarget> createFromTargetData(Buffer buffer)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfImageTarget);
                Detail.easyar_ImageTarget_createFromTargetData(buffer.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ImageTarget>(p.value, Detail.easyar_ImageTarget__typeName) : Optional<ImageTarget>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Saves as an etd file.
        /// </para>
        /// <para xml:lang="zh">
        /// 保存为etd文件。
        /// </para>
        /// </summary>
        public virtual bool save(string path)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTarget_save(cdata, Detail.String_to_c(ar, path));
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a target from an image file. If not needed, name, uid, meta can be passed with empty string, and scale can be passed with default value 1. Jpeg and png files are supported.
        /// </para>
        /// <para xml:lang="zh">
        /// 从图像创建。如果不需要，name、uid、meta可以传空字符串，scale可以传默认值1。支持jpeg或png文件。
        /// </para>
        /// </summary>
        public static Optional<ImageTarget> createFromImageFile(string path, StorageType storageType, string name, string uid, string meta, float scale)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfImageTarget);
                Detail.easyar_ImageTarget_createFromImageFile(Detail.String_to_c(ar, path), storageType, Detail.String_to_c(ar, name), Detail.String_to_c(ar, uid), Detail.String_to_c(ar, meta), scale, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ImageTarget>(p.value, Detail.easyar_ImageTarget__typeName) : Optional<ImageTarget>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The scale of image. The value is the physical image width divided by 1 meter. The default value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 图像的缩放比例。其值为图像宽度的物理大小与1米的比值，默认值为1。
        /// </para>
        /// </summary>
        public virtual float scale()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTarget_scale(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// The aspect ratio of image, width divided by height.
        /// </para>
        /// <para xml:lang="zh">
        /// 图像的宽高比。
        /// </para>
        /// </summary>
        public virtual float aspectRatio()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTarget_aspectRatio(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets image target scale, this will overwrite the value set in the json file or the default value. The value is the physical image width divided by 1 meter. The default value is 1.
        /// It is needed to set the model scale in rendering engine separately.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置图像的缩放比例，设置之后会覆盖默认值以及在json文件中设的数值。其值为图像宽度的物理大小与1米的比值，默认值为1。
        /// 还需要在渲染引擎中单独设置此模型缩放。
        /// </para>
        /// </summary>
        public virtual bool setScale(float scale)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTarget_setScale(cdata, scale);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns a list of images that stored in the target. It is generally used to get image data from cloud returned target.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target中存储的图像列表。通常这个方法用来获取云端返回的target的识别图数据。
        /// </para>
        /// </summary>
        public virtual List<Image> images()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTarget_images(cdata, out _return_value_);
                return Detail.ListOfImage_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target id. A target id is a integer number generated at runtime. This id is non-zero and increasing globally.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target id。target id是运行时创建的整型数据，只有在成功的配置之后才是有效（非0）的。这个id是非0且全局递增的。
        /// </para>
        /// </summary>
        public override int runtimeID()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTarget_runtimeID(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target uid. A target uid is useful in cloud based algorithms. If no cloud is used, you can set this uid in the json config as a alternative method to distinguish from targets.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target uid。ImageTarget的uid在云识别算法中使用。在没有接入云识别的时候，你可以在json配置中设置这个uid，在自己的代码中作为另一种区分target的方法。
        /// </para>
        /// </summary>
        public override string uid()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTarget_uid(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target name. Name is used to distinguish targets in a json file.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target名字。名字用来在json文件中区分target。
        /// </para>
        /// </summary>
        public override string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTarget_name(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set name. It will erase previously set data or data from cloud.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置target名字。这个操作会覆盖上一次的设置或是服务器返回的数据。
        /// </para>
        /// </summary>
        public override void setName(string name)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTarget_setName(cdata, Detail.String_to_c(ar, name));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the meta data set by setMetaData. Or, in a cloud returned target, returns the meta data set in the cloud server.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取setMetaData所设置的meta data。或者在云识别返回的target中，获得服务器所设置的meta data。
        /// </para>
        /// </summary>
        public override string meta()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTarget_meta(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set meta data. It will erase previously set data or data from cloud.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置meta data。这个操作会覆盖上一次的设置或是服务器返回的数据。
        /// </para>
        /// </summary>
        public override void setMeta(string data)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTarget_setMeta(cdata, Detail.String_to_c(ar, data));
            }
        }
    }

    public enum ImageTrackerMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Quality is preferred.
        /// </para>
        /// <para xml:lang="zh">
        /// 优先质量
        /// </para>
        /// </summary>
        PreferQuality = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Performance is preferred.
        /// </para>
        /// <para xml:lang="zh">
        /// 优先性能
        /// </para>
        /// </summary>
        PreferPerformance = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Config for creating `ImageTracker`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// `ImageTracker`_ 创建配置。
    /// </para>
    /// </summary>
    internal class ImageTrackerConfig : RefBase
    {
        internal ImageTrackerConfig(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ImageTrackerConfig(cdata_new, deleter_, retainer_);
        }
        public new ImageTrackerConfig Clone()
        {
            return (ImageTrackerConfig)(CloneObject());
        }
        public ImageTrackerConfig() : base(IntPtr.Zero, Detail.easyar_ImageTrackerConfig__dtor, Detail.easyar_ImageTrackerConfig__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_ImageTrackerConfig__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Mode for ImageTracker. It defaults to PreferQuality.
        /// </para>
        /// <para xml:lang="zh">
        /// ImageTracker模式。默认为PreferQuality。
        /// </para>
        /// </summary>
        public virtual ImageTrackerMode trackerMode()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTrackerConfig_trackerMode(cdata);
                return _return_value_;
            }
        }
        public virtual void setTrackerMode(ImageTrackerMode @value)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTrackerConfig_setTrackerMode(cdata, @value);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Result of `ImageTracker`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// `ImageTracker`_ 的结果。
    /// </para>
    /// </summary>
    internal class ImageTrackerResult : TargetTrackerResult
    {
        internal ImageTrackerResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ImageTrackerResult(cdata_new, deleter_, retainer_);
        }
        public new ImageTrackerResult Clone()
        {
            return (ImageTrackerResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the list of `TargetInstance`_ contained in the result.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前结果中包含的 `TargetInstance`_ 列表。
        /// </para>
        /// </summary>
        public override List<TargetInstance> targetInstances()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTrackerResult_targetInstances(cdata, out _return_value_);
                return Detail.ListOfTargetInstance_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the list of `TargetInstance`_ contained in the result.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前结果中包含的 `TargetInstance`_ 列表。
        /// </para>
        /// </summary>
        public override void setTargetInstances(List<TargetInstance> instances)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTrackerResult_setTargetInstances(cdata, Detail.ListOfTargetInstance_to_c(ar, instances));
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// ImageTracker implements image target detection and tracking.
    /// ImageTracker occupies (1 + SimultaneousNum) buffers of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components.
    /// After creation, you can call start/stop to enable/disable the track process. start and stop are very lightweight calls.
    /// When the component is not needed anymore, call close function to close it. It shall not be used after calling close.
    /// ImageTracker inputs `FeedbackFrame`_ from feedbackFrameSink. `FeedbackFrameSource`_ shall be connected to feedbackFrameSink for use.
    /// Before a `Target`_ can be tracked by ImageTracker, you have to load it using loadTarget/unloadTarget. You can get load/unload results from callbacks passed into the interfaces.
    /// </para>
    /// <para xml:lang="zh">
    /// ImageTracker实现了平面卡片的检测和跟踪。
    /// ImageTracker占用(1 + SimultaneousNum)个camera的buffer。应使用camera的setBufferCapacity设置不少于所有组件占用的camera的buffer数量。
    /// 创建之后，可以调用start/stop来开始和停止运行，start/stop是非常轻量的调用。
    /// 当不再需要该组件时，可以调用close对其进行关闭。close之后不应继续使用。
    /// ImageTracker通过feedbackFrameSink输入 `FeedbackFrame`_ ，应将 `FeedbackFrameSource`_ 连接到feedbackFrameSink上进行使用。
    /// 在 `Target`_ 可以被ImageTracker跟踪之前，你需要通过loadTarget/unloadTarget将它载入。可以通过传入接口的回调来获取load/unload的结果。
    /// </para>
    /// </summary>
    internal class ImageTracker : RefBase
    {
        internal ImageTracker(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ImageTracker(cdata_new, deleter_, retainer_);
        }
        public new ImageTracker Clone()
        {
            return (ImageTracker)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTracker_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `FeedbackFrame`_ input port. The InputFrame member of FeedbackFrame must have raw image, timestamp, and camera parameters. Both pinhole and fisheye camera are supported.
        /// </para>
        /// <para xml:lang="zh">
        /// `FeedbackFrame`_ 输入端口。FeedbackFrame中的InputFrame成员要求必须拥有图像、时间戳和camera参数，camera支持针孔和鱼眼相机。
        /// </para>
        /// </summary>
        public virtual FeedbackFrameSink feedbackFrameSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTracker_feedbackFrameSink(cdata, out _return_value_);
                return Detail.Object_from_c<FeedbackFrameSink>(_return_value_, Detail.easyar_FeedbackFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTracker_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// `OutputFrame`_ output port.
        /// </para>
        /// <para xml:lang="zh">
        /// `OutputFrame`_ 输出端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource outputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTracker_outputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance. The default track mode is `ImageTrackerMode`_.PreferQuality .
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。默认的跟踪模式是 `ImageTrackerMode`_.PreferQuality 。
        /// </para>
        /// </summary>
        public static ImageTracker create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTracker_create(out _return_value_);
                return Detail.Object_from_c<ImageTracker>(_return_value_, Detail.easyar_ImageTracker__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with a specified track mode. On lower-end phones, `ImageTrackerMode`_.PreferPerformance can be used to keep a better performance with a little quality loss.
        /// </para>
        /// <para xml:lang="zh">
        /// 以特定跟踪模式创建。在低端手机上，可以使用 `ImageTrackerMode`_.PreferPerformance 来获得更好的性能，但是跟踪效果会有些许损失。
        /// </para>
        /// </summary>
        public static ImageTracker createWithMode(ImageTrackerMode trackMode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTracker_createWithMode(trackMode, out _return_value_);
                return Detail.Object_from_c<ImageTracker>(_return_value_, Detail.easyar_ImageTracker__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with a specified config.
        /// </para>
        /// <para xml:lang="zh">
        /// 以特定配置创建。
        /// </para>
        /// </summary>
        public static ImageTracker createWithConfig(ImageTrackerConfig config)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTracker_createWithConfig(config.cdata, out _return_value_);
                return Detail.Object_from_c<ImageTracker>(_return_value_, Detail.easyar_ImageTracker__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets result post-processing.
        /// enablePersistentTargetInstance defaults to false. When it is enabled and `InputFrame`_ contains spatial information, targetInstances in `ImageTrackerResult`_ will contain all recognized instances (with not tracking target instances).
        /// enableMotionFusion defaults to false. When it is enabled and `InputFrame`_ contains temporal information and spatial information, pose of targetInstances in `ImageTrackerResult`_ will utilize RealTimeCoordinateTransform .
        /// </para>
        /// <para xml:lang="zh">
        /// 设置结果后处理。
        /// enablePersistentTargetInstance默认为false，开启时，如果 `InputFrame`_ 数据中包含空间信息，则 `ImageTrackerResult`_ 中的targetInstances会包含所有识别到的instance(包括当前未跟踪的)。
        /// enableMotionFusion默认为false，开启时，如果 `InputFrame`_ 数据中包含时间信息和空间信息，则 `ImageTrackerResult`_ 中的 targetInstances 的 pose 会使用 RealTimeCoordinateTransform 进行处理。
        /// </para>
        /// </summary>
        public virtual void setResultPostProcessing(bool enablePersistentTargetInstance, bool enableMotionFusion)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTracker_setResultPostProcessing(cdata, enablePersistentTargetInstance, enableMotionFusion);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets result async mode. Set true If results are to be outputted via outputFrameSource, false if results are to be fetched via getSyncResult. enableAsync defaults to true. Sync mode is only available with input frames including spatial information and with a XR license.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置结果异步模式。如果结果通过outputFrameSource输出，则应设置位true，如果结果通过getSyncResult获取，则应设置为false。默认enableAsync为true。同步模式仅在输入帧包含空间信息且使用XR License时可生效。
        /// </para>
        /// </summary>
        public virtual bool setResultAsyncMode(bool enableAsync)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTracker_setResultAsyncMode(cdata, enableAsync);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts the track algorithm.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始跟踪算法。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTracker_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops the track algorithm. Call start to start the track again.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停跟踪算法。调用start来重新启动跟踪。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTracker_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close. The component shall not be used after calling close.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTracker_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Load a `Target`_ into the tracker. A Target can only be tracked by tracker after a successful load.
        /// This method is an asynchronous method. A load operation may take some time to finish and detection of a new/lost target may take more time during the load. The track time after detection will not be affected. If you want to know the load result, you have to handle the callback data. The callback will be called from the thread specified by `CallbackScheduler`_ . It will not block the track thread or any other operations except other load/unload.
        /// </para>
        /// <para xml:lang="zh">
        /// 加载一个 `Target`_ 进入tracker。 `Target`_ 只有在成功加载进入tracker之后才能被识别和跟踪。
        /// 这个方法是异步方法。加载过程可能会需要一些时间来完成，这段时间内新的和丢失的target的检测可能会花比平时更多的时间，但是检测到之后的跟踪不受影响。如果你希望知道加载的结果，需要处理callback数据。callback将会在 `CallbackScheduler`_ 所指定的线程上被调用。跟踪线程和除了其它加载/卸载之外的操作都不会被阻塞。
        /// </para>
        /// </summary>
        public virtual void loadTarget(Target target, CallbackScheduler callbackScheduler, Action<Target, bool> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTracker_loadTarget(cdata, target.cdata, callbackScheduler.cdata, Detail.FunctorOfVoidFromTargetAndBool_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Unload a `Target`_ from the tracker.
        /// This method is an asynchronous method. An unload operation may take some time to finish and detection of a new/lost target may take more time during the unload. If you want to know the unload result, you have to handle the callback data. The callback will be called from the thread specified by `CallbackScheduler`_ . It will not block the track thread or any other operations except other load/unload.
        /// </para>
        /// <para xml:lang="zh">
        /// 从tracker中卸载 `Target`_ 。
        /// 这个方法是异步方法。卸载过程可能会需要一些时间来完成，这段时间内新的和丢失的target的检测可能会花比平时更多的时间，但是检测到之后的跟踪不受影响。如果你希望知道卸载的结果，需要处理callback数据。callback将会在 `CallbackScheduler`_ 所指定的线程上被调用。跟踪线程和除了其它加载/卸载之外的操作都不会被阻塞。
        /// </para>
        /// </summary>
        public virtual void unloadTarget(Target target, CallbackScheduler callbackScheduler, Action<Target, bool> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ImageTracker_unloadTarget(cdata, target.cdata, callbackScheduler.cdata, Detail.FunctorOfVoidFromTargetAndBool_to_c(callback));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns current loaded targets in the tracker. If an asynchronous load/unload is in progress, the returned value will not reflect the result until all load/unload finish.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前已经被加载进入tracker的target。如果异步的加载/卸载正在执行，在加载/卸载完成之前的返回值将不会反映这些加载/卸载的结果。
        /// </para>
        /// </summary>
        public virtual List<Target> targets()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_ImageTracker_targets(cdata, out _return_value_);
                return Detail.ListOfTarget_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the max number of targets which will be the simultaneously tracked by the tracker. The default value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置最大可被tracker跟踪的目标个数。默认值为1。
        /// </para>
        /// </summary>
        public virtual bool setSimultaneousNum(int num)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTracker_setSimultaneousNum(cdata, num);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the max number of targets which will be the simultaneously tracked by the tracker. The default value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取最大可被tracker跟踪的目标个数。默认值为1。
        /// </para>
        /// </summary>
        public virtual int simultaneousNum()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_ImageTracker_simultaneousNum(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get synchronized result. If ImageTracker is paused, or the result async mode is not set to false through setResultAsyncMode, the return value is empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得同步输出结果。如果ImageTracker已暂停，或者未通过setResultAsyncMode将结果异步模式设为false，则返回值为空。
        /// </para>
        /// </summary>
        public virtual Optional<ImageTrackerResult> getSyncResult(MotionInputData motionInputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfImageTrackerResult);
                Detail.easyar_ImageTracker_getSyncResult(cdata, motionInputData.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<ImageTrackerResult>(p.value, Detail.easyar_ImageTrackerResult__typeName) : Optional<ImageTrackerResult>.Empty);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Recorder implements recording for current rendering screen.
    /// Currently Recorder only works on Android (4.3 or later) and iOS with OpenGL ES 3.0 context.
    /// Due to the dependency to OpenGLES, every method in this class (except requestPermissions, including the destructor) has to be called in a single thread containing an OpenGLES context.
    /// **Unity Only** If in Unity, Multi-threaded rendering is enabled, scripting thread and rendering thread will be two separate threads, which makes it impossible to call updateFrame in the rendering thread. For this reason, to use Recorder, Multi-threaded rendering option shall be disabled.
    /// On Android, it is required to add android.permission.RECORD_AUDIO to AndroidManifest.xml for use.
    /// On iOS, it is required to add NSMicrophoneUsageDescription to Info.plist for use.
    /// </para>
    /// <para xml:lang="zh">
    /// Recorder 实现了对当前渲染环境的屏幕录制功能。
    /// 当前Recorder 只在 Android（4.3 或更新）和 iOS的OpenGL ES 3.0 环境下工作。
    /// 由于依赖于OpenGLES，本类的所有函数(除requestPermissions以外，包括析构函数)都必须在单个包含OpenGLES上下文的线程中调用。
    /// **Unity Only** Unity中如果使用Multi-threaded rendering功能，则脚本线程将与渲染线程分离，无法在渲染线程上调用updateFrame。因此，如果需要使用屏幕录制功能，应禁用Multi-threaded rendering功能。
    /// Android使用时需要在AndroidManifest.xml中加入android.permission.RECORD_AUDIO权限声明。
    /// iOS使用时需要在Info.plist中加入NSMicrophoneUsageDescription权限声明。
    /// </para>
    /// </summary>
    internal class Recorder : RefBase
    {
        internal Recorder(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new Recorder(cdata_new, deleter_, retainer_);
        }
        public new Recorder Clone()
        {
            return (Recorder)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true only on Android 4.3 or later, or on iOS.
        /// </para>
        /// <para xml:lang="zh">
        /// 只在Android 4.3以上或iOS平台下返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Recorder_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Requests recording permissions from operating system. You can call this function or request permission directly from operating system. It is only available on Android and iOS. On other platforms, it will call the callback directly with status being granted. This function need to be called from the UI thread.
        /// </para>
        /// <para xml:lang="zh">
        /// 请求录屏所需的系统权限。你可以选择使用这个函数或自己调用系统函数申请权限。只在Android和iOS平台上有效，其他平台上的行为为直接调用回调通知权限已授权。应在UI线程调用该函数。
        /// </para>
        /// </summary>
        public static void requestPermissions(CallbackScheduler callbackScheduler, Optional<Action<PermissionStatus, string>> permissionCallback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Recorder_requestPermissions(callbackScheduler.cdata, permissionCallback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromPermissionStatusAndString { has_value = true, value = Detail.FunctorOfVoidFromPermissionStatusAndString_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromPermissionStatusAndString { has_value = false, value = default(Detail.FunctorOfVoidFromPermissionStatusAndString) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance and initialize recording. statusCallback will dispatch event of status change and corresponding log.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建并初始化录屏功能。statusCallback回调中会通知一些状态变化和对应的日志。
        /// </para>
        /// </summary>
        public static Recorder create(RecorderConfiguration config, CallbackScheduler callbackScheduler, Optional<Action<RecordStatus, string>> statusCallback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Recorder_create(config.cdata, callbackScheduler.cdata, statusCallback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromRecordStatusAndString { has_value = true, value = Detail.FunctorOfVoidFromRecordStatusAndString_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromRecordStatusAndString { has_value = false, value = default(Detail.FunctorOfVoidFromRecordStatusAndString) }), out _return_value_);
                return Detail.Object_from_c<Recorder>(_return_value_, Detail.easyar_Recorder__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Start recording.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始录屏。
        /// </para>
        /// </summary>
        public virtual void start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Recorder_start(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Update and record a frame using texture data.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用texture录制一帧数据。
        /// </para>
        /// </summary>
        public virtual void updateFrame(TextureId texture, int width, int height)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Recorder_updateFrame(cdata, texture.cdata, width, height);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stop recording. When calling stop, it will wait for file write to end and returns whether recording is successful.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止录屏。在调用stop之后，会等待文件写入结束并返回录制是否成功的结果。
        /// </para>
        /// </summary>
        public virtual bool stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Recorder_stop(cdata);
                return _return_value_;
            }
        }
    }

    internal enum RecordProfile
    {
        /// <summary>
        /// <para xml:lang="en">
        /// 1080P, low quality
        /// </para>
        /// <para xml:lang="zh">
        /// 1080P，低质量
        /// </para>
        /// </summary>
        Quality_1080P_Low = 0x00000001,
        /// <summary>
        /// <para xml:lang="en">
        /// 1080P, middle quality
        /// </para>
        /// <para xml:lang="zh">
        /// 1080P，中质量
        /// </para>
        /// </summary>
        Quality_1080P_Middle = 0x00000002,
        /// <summary>
        /// <para xml:lang="en">
        /// 1080P, high quality
        /// </para>
        /// <para xml:lang="zh">
        /// 1080P，高质量
        /// </para>
        /// </summary>
        Quality_1080P_High = 0x00000004,
        /// <summary>
        /// <para xml:lang="en">
        /// 720P, low quality
        /// </para>
        /// <para xml:lang="zh">
        /// 720P，低质量
        /// </para>
        /// </summary>
        Quality_720P_Low = 0x00000008,
        /// <summary>
        /// <para xml:lang="en">
        /// 720P, middle quality
        /// </para>
        /// <para xml:lang="zh">
        /// 720P，中质量
        /// </para>
        /// </summary>
        Quality_720P_Middle = 0x00000010,
        /// <summary>
        /// <para xml:lang="en">
        /// 720P, high quality
        /// </para>
        /// <para xml:lang="zh">
        /// 720P，高质量
        /// </para>
        /// </summary>
        Quality_720P_High = 0x00000020,
        /// <summary>
        /// <para xml:lang="en">
        /// 480P, low quality
        /// </para>
        /// <para xml:lang="zh">
        /// 480P，低质量
        /// </para>
        /// </summary>
        Quality_480P_Low = 0x00000040,
        /// <summary>
        /// <para xml:lang="en">
        /// 480P, middle quality
        /// </para>
        /// <para xml:lang="zh">
        /// 480P，中质量
        /// </para>
        /// </summary>
        Quality_480P_Middle = 0x00000080,
        /// <summary>
        /// <para xml:lang="en">
        /// 480P, high quality
        /// </para>
        /// <para xml:lang="zh">
        /// 480P，高质量
        /// </para>
        /// </summary>
        Quality_480P_High = 0x00000100,
        /// <summary>
        /// <para xml:lang="en">
        /// default resolution and quality, same as `Quality_720P_Middle`
        /// </para>
        /// <para xml:lang="zh">
        /// 默认分辨率与质量，与 `Quality_720P_Middle` 相同
        /// </para>
        /// </summary>
        Quality_Default = 0x00000010,
    }

    internal enum RecordVideoSize
    {
        /// <summary>
        /// <para xml:lang="en">
        /// 1080P
        /// </para>
        /// <para xml:lang="zh">
        /// 1080P
        /// </para>
        /// </summary>
        Vid1080p = 0x00000002,
        /// <summary>
        /// <para xml:lang="en">
        /// 720P
        /// </para>
        /// <para xml:lang="zh">
        /// 720P
        /// </para>
        /// </summary>
        Vid720p = 0x00000010,
        /// <summary>
        /// <para xml:lang="en">
        /// 480P
        /// </para>
        /// <para xml:lang="zh">
        /// 480P
        /// </para>
        /// </summary>
        Vid480p = 0x00000080,
    }

    internal enum RecordZoomMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// If output aspect ratio does not fit input, content will be clipped to fit output aspect ratio.
        /// </para>
        /// <para xml:lang="zh">
        /// 如果输出宽高比与输入不符，内容会被剪裁到适合输出比例。
        /// </para>
        /// </summary>
        NoZoomAndClip = 0x00000000,
        /// <summary>
        /// <para xml:lang="en">
        /// If output aspect ratio does not fit input, content will not be clipped and there will be black borders in one dimension.
        /// </para>
        /// <para xml:lang="zh">
        /// 如果输出宽高比与输入不符，内容将不会被剪裁，在某个维度上会有黑边。
        /// </para>
        /// </summary>
        ZoomInWithAllContent = 0x00000001,
    }

    internal enum RecordVideoOrientation
    {
        /// <summary>
        /// <para xml:lang="en">
        /// video recorded is landscape
        /// </para>
        /// <para xml:lang="zh">
        /// 录制的视频是横向
        /// </para>
        /// </summary>
        Landscape = 0x00000000,
        /// <summary>
        /// <para xml:lang="en">
        /// video recorded is portrait
        /// </para>
        /// <para xml:lang="zh">
        /// 录制的视频是竖向
        /// </para>
        /// </summary>
        Portrait = 0x00000001,
    }

    internal enum RecordStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// recording start
        /// </para>
        /// <para xml:lang="zh">
        /// 录屏开始
        /// </para>
        /// </summary>
        OnStarted = 0x00000002,
        /// <summary>
        /// <para xml:lang="en">
        /// recording stopped
        /// </para>
        /// <para xml:lang="zh">
        /// 录屏结束
        /// </para>
        /// </summary>
        OnStopped = 0x00000004,
        /// <summary>
        /// <para xml:lang="en">
        /// start fail
        /// </para>
        /// <para xml:lang="zh">
        /// 开始录屏失败
        /// </para>
        /// </summary>
        FailedToStart = 0x00000202,
        /// <summary>
        /// <para xml:lang="en">
        /// file write succeed
        /// </para>
        /// <para xml:lang="zh">
        /// 文件存储成功
        /// </para>
        /// </summary>
        FileSucceeded = 0x00000400,
        /// <summary>
        /// <para xml:lang="en">
        /// file write fail
        /// </para>
        /// <para xml:lang="zh">
        /// 文件存储失败
        /// </para>
        /// </summary>
        FileFailed = 0x00000401,
        /// <summary>
        /// <para xml:lang="en">
        /// runtime info with description
        /// </para>
        /// <para xml:lang="zh">
        /// 运行时信息，包含描述
        /// </para>
        /// </summary>
        LogInfo = 0x00000800,
        /// <summary>
        /// <para xml:lang="en">
        /// runtime error with description
        /// </para>
        /// <para xml:lang="zh">
        /// 运行时错误，包含错误描述
        /// </para>
        /// </summary>
        LogError = 0x00001000,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// RecorderConfiguration is startup configuration for `Recorder`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// RecorderConfiguration为 `Recorder`_ 的启动配置。
    /// </para>
    /// </summary>
    internal class RecorderConfiguration : RefBase
    {
        internal RecorderConfiguration(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new RecorderConfiguration(cdata_new, deleter_, retainer_);
        }
        public new RecorderConfiguration Clone()
        {
            return (RecorderConfiguration)(CloneObject());
        }
        public RecorderConfiguration() : base(IntPtr.Zero, Detail.easyar_RecorderConfiguration__dtor, Detail.easyar_RecorderConfiguration__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_RecorderConfiguration__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets absolute path for output video file.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置输出视频文件的绝对路径。
        /// </para>
        /// </summary>
        public virtual void setOutputFile(string path)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setOutputFile(cdata, Detail.String_to_c(ar, path));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording profile. Default value is Quality_720P_Middle.
        /// This is an all-in-one configuration, you can control in more advanced mode with other APIs.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏配置。默认值是 Quality_720P_Middle。
        /// 这是个整体的配置，如果需要更为细节的配置可以调用其他API。
        /// </para>
        /// </summary>
        public virtual bool setProfile(RecordProfile profile)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_RecorderConfiguration_setProfile(cdata, profile);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording video size. Default value is Vid720p.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏视频大小。默认值是 Vid720p。
        /// </para>
        /// </summary>
        public virtual void setVideoSize(RecordVideoSize framesize)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setVideoSize(cdata, framesize);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording video bit rate. Default value is 2500000.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏视频比特率。默认值是 2500000。
        /// </para>
        /// </summary>
        public virtual void setVideoBitrate(int bitrate)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setVideoBitrate(cdata, bitrate);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording audio channel count. Default value is 1.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏音频通道数。默认值是 1。
        /// </para>
        /// </summary>
        public virtual void setChannelCount(int count)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setChannelCount(cdata, count);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording audio sample rate. Default value is 44100.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏音频采样率。默认值是 44100。
        /// </para>
        /// </summary>
        public virtual void setAudioSampleRate(int samplerate)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setAudioSampleRate(cdata, samplerate);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording audio bit rate. Default value is 96000.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏音频比特率。默认值是 96000。
        /// </para>
        /// </summary>
        public virtual void setAudioBitrate(int bitrate)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setAudioBitrate(cdata, bitrate);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording video orientation. Default value is Landscape.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏视频朝向。默认值是 Landscape。
        /// </para>
        /// </summary>
        public virtual void setVideoOrientation(RecordVideoOrientation mode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setVideoOrientation(cdata, mode);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets recording zoom mode. Default value is NoZoomAndClip.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置录屏缩放模式。默认值是 NoZoomAndClip。
        /// </para>
        /// </summary>
        public virtual void setZoomMode(RecordZoomMode mode)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_RecorderConfiguration_setZoomMode(cdata, mode);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Describes the result of mapping and localization. Updated at the same frame rate with OutputFrame.
    /// </para>
    /// <para xml:lang="zh">
    /// 获取稀疏建图与定位系统的输出，会以OutputFrame的频率更新。
    /// </para>
    /// </summary>
    internal class SparseSpatialMapResult : FrameFilterResult
    {
        internal SparseSpatialMapResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SparseSpatialMapResult(cdata_new, deleter_, retainer_);
        }
        public new SparseSpatialMapResult Clone()
        {
            return (SparseSpatialMapResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Obtain motion tracking status.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前运动跟踪状态。
        /// </para>
        /// </summary>
        public virtual MotionTrackingStatus getMotionTrackingStatus()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMapResult_getMotionTrackingStatus(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns pose of the origin of VIO system in camera coordinate system.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前VIO坐标系原点在相机坐标系中的位姿。
        /// </para>
        /// </summary>
        public virtual Optional<Matrix44F> getVioPose()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMapResult_getVioPose(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<Matrix44F>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the pose of origin of the map in camera coordinate system, when localization is successful.
        /// Otherwise, returns pose of the origin of VIO system in camera coordinate system.
        /// </para>
        /// <para xml:lang="zh">
        /// 若在SparseSpatialMap中定位成功，则输出地图原点在相机坐标系中的位姿，否则，输出VIO坐标系原点在相机坐标系中的位姿。
        /// </para>
        /// </summary>
        public virtual Optional<Matrix44F> getMapPose()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMapResult_getMapPose(cdata);
                return _return_value_.map(p => p.has_value ? p.value : Optional<Matrix44F>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns true if the system can reliablly locate the pose of the device with regard to the map.
        /// Once relocalization succeeds, relative pose can be updated by motion tracking module.
        /// As long as the motion tracking module returns normal tracking status, the localization status is also true.
        /// </para>
        /// <para xml:lang="zh">
        /// 定位系统能否确定设备相对于定位地图的位姿关系。
        /// 单次定位成功后，会通过运动跟踪系统进行持续追踪，因此即使移除地图区域，但是运动跟踪一直正常工作，仍然会返回true。
        /// </para>
        /// </summary>
        public virtual bool getLocalizationStatus()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMapResult_getLocalizationStatus(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns current localized map ID.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前定位成功的地图的ID，
        /// </para>
        /// </summary>
        public virtual string getLocalizationMapID()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMapResult_getLocalizationMapID(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
    }

    internal enum PlaneType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Horizontal plane
        /// </para>
        /// <para xml:lang="zh">
        /// 水平面
        /// </para>
        /// </summary>
        Horizontal = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Vertical plane
        /// </para>
        /// <para xml:lang="zh">
        /// 竖直面
        /// </para>
        /// </summary>
        Vertical = 1,
    }

    internal class PlaneData : RefBase
    {
        internal PlaneData(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new PlaneData(cdata_new, deleter_, retainer_);
        }
        public new PlaneData Clone()
        {
            return (PlaneData)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Constructor
        /// </para>
        /// <para xml:lang="zh">
        /// Constructor
        /// </para>
        /// </summary>
        public PlaneData() : base(IntPtr.Zero, Detail.easyar_PlaneData__dtor, Detail.easyar_PlaneData__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_PlaneData__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the type of this plane.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取平面类型，当前支持水平面和竖直面。
        /// </para>
        /// </summary>
        public virtual PlaneType getType()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_PlaneData_getType(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the pose of the center of the detected plane.The pose&#39;s transformed +Y axis will be point normal out of the plane, with the +X and +Z axes orienting the extents of the bounding rectangle.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取平面中心在当前地图坐标系中的位置和姿态。Y轴正方向为平面向外方向，X轴和Z轴定义了外接矩形的范围。
        /// </para>
        /// </summary>
        public virtual Matrix44F getPose()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_PlaneData_getPose(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the length of this plane&#39;s bounding rectangle measured along the local X-axis of the coordinate space centered on the plane.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回检测到的平面的最小外接矩形的在局部坐标系X轴上的尺寸,其中最小外接矩形的中心为平面中心。
        /// </para>
        /// </summary>
        public virtual float getExtentX()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_PlaneData_getExtentX(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the length of this plane&#39;s bounding rectangle measured along the local Z-axis of the coordinate frame centered on the plane.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回检测到的平面的最小外接矩形的在局部坐标系Z轴上的尺寸,其中最小外接矩形的中心为平面中心。
        /// </para>
        /// </summary>
        public virtual float getExtentZ()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_PlaneData_getExtentZ(cdata);
                return _return_value_;
            }
        }
    }

    public enum LocalizationMode
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Attempt to perform localization in current SparseSpatialMap until success.
        /// </para>
        /// <para xml:lang="zh">
        /// 一直尝试定位，一旦定位成功，停止继续尝试
        /// </para>
        /// </summary>
        UntilSuccess = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Perform localization only once
        /// </para>
        /// <para xml:lang="zh">
        /// 尝试定位一次
        /// </para>
        /// </summary>
        Once = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Keep performing localization and adjust result on success
        /// </para>
        /// <para xml:lang="zh">
        /// 一直尝试定位，即使定位成功，依然继续尝试，并在定位再次成功时调整结果到更准确的位置和姿态
        /// </para>
        /// </summary>
        KeepUpdate = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// Keep performing localization and adjust localization result only when localization returns different map ID from previous results
        /// </para>
        /// <para xml:lang="zh">
        /// 一直尝试定位，仅在定位到新地图时调整结果到对应位置和姿态
        /// </para>
        /// </summary>
        ContinousLocalize = 3,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Configuration used to set the localization mode.
    /// </para>
    /// <para xml:lang="zh">
    /// 用于配置稀疏建图中的定位策略。
    /// </para>
    /// </summary>
    internal class SparseSpatialMapConfig : RefBase
    {
        internal SparseSpatialMapConfig(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SparseSpatialMapConfig(cdata_new, deleter_, retainer_);
        }
        public new SparseSpatialMapConfig Clone()
        {
            return (SparseSpatialMapConfig)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Constructor
        /// </para>
        /// <para xml:lang="zh">
        /// Constructor
        /// </para>
        /// </summary>
        public SparseSpatialMapConfig() : base(IntPtr.Zero, Detail.easyar_SparseSpatialMapConfig__dtor, Detail.easyar_SparseSpatialMapConfig__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_SparseSpatialMapConfig__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets localization configurations. See also `LocalizationMode`_.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置定位策略参数。参考 `LocalizationMode`_ 。
        /// </para>
        /// </summary>
        public virtual void setLocalizationMode(LocalizationMode @value)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMapConfig_setLocalizationMode(cdata, @value);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns localization configurations. See also `LocalizationMode`_.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前定位策略参数。参考 `LocalizationMode`_ 。
        /// </para>
        /// </summary>
        public virtual LocalizationMode getLocalizationMode()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMapConfig_getLocalizationMode(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Provides core components for SparseSpatialMap, can be used for sparse spatial map building as well as localization using existing map. Also provides utilities for point cloud and plane access.
    /// SparseSpatialMap occupies 2 buffers of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components.
    /// </para>
    /// <para xml:lang="zh">
    /// 提供SparseSpatialMap系统主要的功能，地图生成和存储、地图加载和定位，同时可以获取点云，平面等环境信息并进行hit Test。
    /// SparseSpatialMap占用2个camera的buffer。应使用camera的setBufferCapacity设置不少于所有组件占用的camera的buffer数量。
    /// </para>
    /// </summary>
    internal class SparseSpatialMap : RefBase
    {
        internal SparseSpatialMap(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SparseSpatialMap(cdata_new, deleter_, retainer_);
        }
        public new SparseSpatialMap Clone()
        {
            return (SparseSpatialMap)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Check whether SparseSpatialMap is is available, always return true.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查SparseSpatialMap是否可用。总是返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMap_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port for input frame. For SparseSpatialMap to work, the inputFrame must include camera parameters, timestamp and spatial information. See also `InputFrameSink`_
        /// Note: Both pinhole and fisheye camera are supported. But performance may decrease while using a fisheye camera.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入帧输入端口。SparseSpatialMap输入帧必须包含camera参数、时间戳信息和空间信息（cameraTransform和trackingStatus）。参考 `InputFrameSink`_ 。
        /// 注意：camera支持针孔和鱼眼相机模型，使用鱼眼相机性能会受影响。
        /// </para>
        /// </summary>
        public virtual InputFrameSink inputFrameSink()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_inputFrameSink(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMap_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for output frame. See also `OutputFrameSource`_
        /// </para>
        /// <para xml:lang="zh">
        /// 输出帧输出端口。参考 `OutputFrameSource`_ 。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource outputFrameSource()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_outputFrameSource(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Construct SparseSpatialMap.
        /// </para>
        /// <para xml:lang="zh">
        /// 构造SparseSpatialMap。
        /// </para>
        /// </summary>
        public static SparseSpatialMap create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_create(out _return_value_);
                return Detail.Object_from_c<SparseSpatialMap>(_return_value_, Detail.easyar_SparseSpatialMap__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets type of result pose. enableStabilization defaults to false. It only takes effect when `InputFrame`_ contains spatial information.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置结果姿态类型。默认enableStabilization为false。只有 `InputFrame`_ 中包含空间信息时才有效。
        /// </para>
        /// </summary>
        public virtual void setResultPoseType(bool enableStabilization)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMap_setResultPoseType(cdata, enableStabilization);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets result async mode. Set true If results are to be outputted via outputFrameSource, false if results are to be fetched via getSyncResult. enableAsync defaults to true. Sync mode is only available with input frames including spatial information and with a XR license.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置结果异步模式。如果结果通过outputFrameSource输出，则应设置位true，如果结果通过getSyncResult获取，则应设置为false。默认enableAsync为true。同步模式仅在输入帧包含空间信息且使用XR License时可生效。
        /// </para>
        /// </summary>
        public virtual bool setResultAsyncMode(bool enableAsync)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMap_setResultAsyncMode(cdata, enableAsync);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Start SparseSpatialMap system.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始SparseSpatialMap算法。
        /// </para>
        /// </summary>
        public virtual bool start()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMap_start(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stop SparseSpatialMap from running。Can resume running by calling start().
        /// </para>
        /// <para xml:lang="zh">
        /// 停止SparseSpatialMap算法。调用start重新运行。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMap_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Close SparseSpatialMap. SparseSpatialMap can no longer be used.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭SparseSpatialMap。close之后不应继续使用。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMap_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the buffer of point cloud coordinate. Each 3D point is represented by three consecutive values, representing X, Y, Z position coordinates in the world coordinate space, each of which takes 4 bytes.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前点云数据位置信息。其中点云位置为世界坐标系中的位置，buffer每一个点由三个连续的值表示，分别代表X，Y，Z轴上的坐标值，每一个值占用4字节。
        /// </para>
        /// </summary>
        public virtual Buffer getPointCloudBuffer()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_getPointCloudBuffer(cdata, out _return_value_);
                return Detail.Object_from_c<Buffer>(_return_value_, Detail.easyar_Buffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns detected planes in SparseSpatialMap.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取检测到的平面，类型为 `PlaneType`_ 。
        /// </para>
        /// </summary>
        public virtual List<PlaneData> getMapPlanes()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_getMapPlanes(cdata, out _return_value_);
                return Detail.ListOfPlaneData_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Perform hit test against the point cloud. The results are returned sorted by their distance to the camera in ascending order.
        /// </para>
        /// <para xml:lang="zh">
        /// 在当前点云中进行Hit Test，得到距离相机从近到远一条射线上的n（n&gt;=0）个位置坐标。
        /// </para>
        /// </summary>
        public virtual List<Vec3F> hitTestAgainstPointCloud(Vec2F cameraImagePoint)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_hitTestAgainstPointCloud(cdata, cameraImagePoint, out _return_value_);
                return Detail.ListOfVec3F_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Performs ray cast from the user&#39;s device in the direction of given screen point.
        /// Intersections with detected planes are returned. 3D positions on physical planes are sorted by distance from the device in ascending order.
        /// For the camera image coordinate system ([0, 1]^2), x-right, y-down, and origin is at left-top corner. `CameraParameters.imageCoordinatesFromScreenCoordinates`_ can be used to convert points from screen coordinate system to camera image coordinate system.
        /// The output point cloud coordinate is in the world coordinate system.
        /// </para>
        /// <para xml:lang="zh">
        /// 在当前检测到的平面上进行Hit Test，得到距离相机从近到远一条射线上的n（n&gt;=0）个位置坐标。
        /// 输入图像坐标系（[0, 1]^2）的x朝右、y朝下，原点在左上角。可以使用 `CameraParameters.imageCoordinatesFromScreenCoordinates`_ 来从屏幕坐标转换为图像坐标。
        /// 输出为点云在世界坐标系中的坐标。
        /// </para>
        /// </summary>
        public virtual List<Vec3F> hitTestAgainstPlanes(Vec2F cameraImagePoint)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_hitTestAgainstPlanes(cdata, cameraImagePoint, out _return_value_);
                return Detail.ListOfVec3F_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get the map data version of the current SparseSpatialMap.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取当前SparseSpatialMap的地图版本。
        /// </para>
        /// </summary>
        public static string getMapVersion()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_getMapVersion(out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// UnloadMap specified SparseSpatialMap data via callback function.The return value of callback indicates whether unload map succeeds (true) or fails (false).
        /// </para>
        /// <para xml:lang="zh">
        /// 通过回调，卸载指定的SparseSpatialMap地图数据。可以通过回调的返回值判断卸载是否成功,成功返回true,否则返回false。
        /// </para>
        /// </summary>
        public virtual void unloadMap(string mapID, CallbackScheduler callbackScheduler, Optional<Action<bool>> resultCallBack)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMap_unloadMap(cdata, Detail.String_to_c(ar, mapID), callbackScheduler.cdata, resultCallBack.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromBool { has_value = true, value = Detail.FunctorOfVoidFromBool_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromBool { has_value = false, value = default(Detail.FunctorOfVoidFromBool) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set configurations for SparseSpatialMap. See also `SparseSpatialMapConfig`_.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置SparseSpatialMap相关的配置。参考 `SparseSpatialMapConfig`_ 。
        /// </para>
        /// </summary>
        public virtual void setConfig(SparseSpatialMapConfig config)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMap_setConfig(cdata, config.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns configurations for SparseSpatialMap. See also `SparseSpatialMapConfig`_.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取SparseSpatialMap相关的配置。参考 `SparseSpatialMapConfig`_ 。
        /// </para>
        /// </summary>
        public virtual SparseSpatialMapConfig getConfig()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMap_getConfig(cdata, out _return_value_);
                return Detail.Object_from_c<SparseSpatialMapConfig>(_return_value_, Detail.easyar_SparseSpatialMapConfig__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Start localization in loaded maps. Should set `LocalizationMode`_ first.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始在已加载地图中尝试定位。在此之前，需要设定所需的配置参数。参考 `LocalizationMode`_。
        /// </para>
        /// </summary>
        public virtual bool startLocalization()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMap_startLocalization(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stop localization in loaded maps.
        /// </para>
        /// <para xml:lang="zh">
        /// 停当前定位过程。
        /// </para>
        /// </summary>
        public virtual void stopLocalization()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMap_stopLocalization(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get synchronized result. If SparseSpatialMap is paused, or the result async mode is not set to false through setResultAsyncMode, the return value is empty.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得同步输出结果。如果SparseSpatialMap已暂停，或者未通过setResultAsyncMode将结果异步模式设为false，则返回值为空。
        /// </para>
        /// </summary>
        public virtual Optional<SparseSpatialMapResult> getSyncResult(MotionInputData motionInputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfSparseSpatialMapResult);
                Detail.easyar_SparseSpatialMap_getSyncResult(cdata, motionInputData.cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<SparseSpatialMapResult>(p.value, Detail.easyar_SparseSpatialMapResult__typeName) : Optional<SparseSpatialMapResult>.Empty);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// SparseSpatialMap manager class, for managing sharing.
    /// </para>
    /// <para xml:lang="zh">
    /// SparseSpatialMap管理类，用于管理SparseSpatialMap的分享功能。
    /// </para>
    /// </summary>
    internal class SparseSpatialMapManager : RefBase
    {
        internal SparseSpatialMapManager(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SparseSpatialMapManager(cdata_new, deleter_, retainer_);
        }
        public new SparseSpatialMapManager Clone()
        {
            return (SparseSpatialMapManager)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Check whether SparseSpatialMapManager is is available. It returns true when the operating system is Windows, Mac, iOS or Android.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查SparseSpatialMapManager是否可用。当运行的操作系统为Windows, Mac, iOS或Android时返回true。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_SparseSpatialMapManager_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static SparseSpatialMapManager create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_SparseSpatialMapManager_create(out _return_value_);
                return Detail.Object_from_c<SparseSpatialMapManager>(_return_value_, Detail.easyar_SparseSpatialMapManager__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates a map from `SparseSpatialMap`_ and upload it to EasyAR cloud servers. After completion, a serverMapId will be returned for loading map from EasyAR cloud servers.
        /// </para>
        /// <para xml:lang="zh">
        /// 从 `SparseSpatialMap`_ 创建地图并上传到EasyAR云服务器以进行分享。创建成功时会获得用于从EasyAR云服务器加载地图的serverMapId。
        /// </para>
        /// </summary>
        public virtual void host(SparseSpatialMap mapBuilder, string apiKey, string apiSecret, string sparseSpatialMapAppId, string name, Optional<Image> preview, Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<bool, string, string> onCompleted)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMapManager_host(cdata, mapBuilder.cdata, Detail.String_to_c(ar, apiKey), Detail.String_to_c(ar, apiSecret), Detail.String_to_c(ar, sparseSpatialMapAppId), Detail.String_to_c(ar, name), preview.map(p => p.OnSome ? new Detail.OptionalOfImage { has_value = true, value = p.Value.cdata } : new Detail.OptionalOfImage { has_value = false, value = default(IntPtr) }), timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromBoolAndStringAndString_to_c(onCompleted));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Loads a map from EasyAR cloud servers by serverMapId. To unload the map, call `SparseSpatialMap.unloadMap`_ with serverMapId.
        /// </para>
        /// <para xml:lang="zh">
        /// 使用serverMapId从EasyAR云服务器加载地图到 `SparseSpatialMap`_ 中。可以调用 `SparseSpatialMap.unloadMap`_ 并传入serverMapId以卸载地图。
        /// </para>
        /// </summary>
        public virtual void load(SparseSpatialMap mapTracker, string serverMapId, string apiKey, string apiSecret, string sparseSpatialMapAppId, Optional<int> timeoutMilliseconds, CallbackScheduler callbackScheduler, Action<bool, string> onCompleted)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMapManager_load(cdata, mapTracker.cdata, Detail.String_to_c(ar, serverMapId), Detail.String_to_c(ar, apiKey), Detail.String_to_c(ar, apiSecret), Detail.String_to_c(ar, sparseSpatialMapAppId), timeoutMilliseconds.map(p => p.OnSome ? new Detail.OptionalOfInt { has_value = true, value = p.Value } : new Detail.OptionalOfInt { has_value = false, value = default(int) }), callbackScheduler.cdata, Detail.FunctorOfVoidFromBoolAndString_to_c(onCompleted));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Clears allocated cache space.
        /// </para>
        /// <para xml:lang="zh">
        /// 清除已占用的缓存数据空间。
        /// </para>
        /// </summary>
        public virtual void clear()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SparseSpatialMapManager_clear(cdata);
            }
        }
    }

    internal enum EngineOperatingSystem
    {
        Windows = 0,
        Linux = 1,
        MacOS = 2,
        iOS = 3,
        Android = 4,
        WinRT = 5,
        VisionOS = 6,
    }

    internal enum ValidationState
    {
        Successful = 0,
        Pending = 1,
        Failed = 2,
    }

    internal class Engine
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the version schema hash, which can be used to ensure type declarations consistent with runtime library.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得版本散列值，用于确保各个语言的类型定义与运行库的类型定义版本一致。
        /// </para>
        /// </summary>
        public static int schemaHash()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Engine_schemaHash();
                return _return_value_;
            }
        }
        public static bool initialize(string licenseKey)
        {
            if (Detail.easyar_Engine_schemaHash() != -1782020803)
            {
                throw new InvalidOperationException("SchemaHashNotMatched");
            }
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Engine_initialize(Detail.String_to_c(ar, licenseKey));
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Handles the app onPause, pauses internal tasks.
        /// </para>
        /// <para xml:lang="zh">
        /// 处理应用onPause，暂停内部任务。
        /// </para>
        /// </summary>
        public static void onPause()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Engine_onPause();
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Handles the app onResume, resumes internal tasks.
        /// </para>
        /// <para xml:lang="zh">
        /// 处理应用onResume，重启内部任务。
        /// </para>
        /// </summary>
        public static void onResume()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Engine_onResume();
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets license validation state.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取license验证状态。
        /// </para>
        /// </summary>
        public static ValidationState validationState()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Engine_validationState();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets error message on initialization failure.
        /// </para>
        /// <para xml:lang="zh">
        /// 用于初始化失败时获得错误信息。
        /// </para>
        /// </summary>
        public static string errorMessage()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Engine_errorMessage(out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the version number of EasyARSense.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得EasyARSense的版本号。
        /// </para>
        /// </summary>
        public static string versionString()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Engine_versionString(out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the product name of EasyARSense. (Including variant, operating system and CPU architecture.)
        /// </para>
        /// <para xml:lang="zh">
        /// 获得EasyARSense的产品名称。（包括版本变种、操作系统和CPU架构）
        /// </para>
        /// </summary>
        public static string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Engine_name(out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the release variant of EasyARSense.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得EasyARSense的发布版本。
        /// </para>
        /// </summary>
        public static string variant()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Engine_variant(out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks whether the license key matches the provided release variant, package name(or bundle id) and operating system.
        /// </para>
        /// <para xml:lang="zh">
        /// 验证许可证密钥是否匹配发布版本、包名和操作系统
        /// </para>
        /// </summary>
        public static bool isLicenseKeyMatched(string licenseKey, string packageName, string variant, EngineOperatingSystem operatingSystem)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Engine_isLicenseKeyMatched(Detail.String_to_c(ar, licenseKey), Detail.String_to_c(ar, packageName), Detail.String_to_c(ar, variant), operatingSystem);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Enable eyewear support.
        /// </para>
        /// <para xml:lang="zh">
        /// 启用眼镜支持。
        /// </para>
        /// </summary>
        public static void enableEyewearSupport()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Engine_enableEyewearSupport();
            }
        }
    }

    internal enum VideoStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Status to indicate something wrong happen in video open or play.
        /// </para>
        /// <para xml:lang="zh">
        /// 视频打开或播放过程中发生错误
        /// </para>
        /// </summary>
        Error = -1,
        /// <summary>
        /// <para xml:lang="en">
        /// Status to show video finished open and is ready for play.
        /// </para>
        /// <para xml:lang="zh">
        /// 视频成功打开，可以开始播放
        /// </para>
        /// </summary>
        Ready = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Status to indicate video finished play and reached the end.
        /// </para>
        /// <para xml:lang="zh">
        /// 视频播放完成
        /// </para>
        /// </summary>
        Completed = 1,
    }

    internal enum VideoType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Normal video.
        /// </para>
        /// <para xml:lang="zh">
        /// 普通视频
        /// </para>
        /// </summary>
        Normal = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Transparent video, left half is the RGB channel and right half is alpha channel.
        /// </para>
        /// <para xml:lang="zh">
        /// 透明视频，左半边是RGB通道，右半边是alpha通道
        /// </para>
        /// </summary>
        TransparentSideBySide = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Transparent video, top half is the RGB channel and bottom half is alpha channel.
        /// </para>
        /// <para xml:lang="zh">
        /// 透明视频，上半边是RGB通道，下半边是alpha通道
        /// </para>
        /// </summary>
        TransparentTopAndBottom = 2,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// VideoPlayer is the class for video playback.
    /// EasyAR supports normal videos, transparent videos and streaming videos. The video content will be rendered into a texture passed into the player through setRenderTexture.
    /// This class only supports OpenGLES 3.0 texture.
    /// Due to the dependency to OpenGLES, every method in this class (including the destructor) has to be called in a single thread containing an OpenGLES context.
    /// Current version requires width and height being mutiples of 16.
    ///
    /// Supported video file formats
    /// Windows: Media Foundation-compatible formats, more can be supported via extra codecs. Please refer to `Supported Media Formats in Media Foundation &lt;https://docs.microsoft.com/en-us/windows/win32/medfound/supported-media-formats-in-media-foundation&gt;`__ . DirectShow is not supported.
    /// Mac: Not supported.
    /// Android: System supported formats. Please refer to `Supported media formats &lt;https://developer.android.com/guide/topics/media/media-formats&gt;`__ .
    /// iOS: System supported formats. There is no reference in effect currently.
    /// </para>
    /// <para xml:lang="zh">
    /// VideoPlayer是视频播放类。
    /// EasyAR支持普通的视频、透明视频和流媒体播放。视频内容会被渲染到传入setRenderTexture的texture上。
    /// 该类只支持OpenGLES 3.0的texture。
    /// 由于依赖于OpenGLES，本类的所有函数(包括析构函数)都必须在单个包含OpenGLES上下文的线程中调用。
    /// 当前版本要求宽高均为16的倍数。
    ///
    /// 支持的视频文件格式
    /// Windows: Media Foundation兼容格式，安装额外的解码器可以支持更多格式，请参考 `Supported Media Formats in Media Foundation &lt;https://docs.microsoft.com/en-us/windows/win32/medfound/supported-media-formats-in-media-foundation&gt;`__ ，不支持DirectShow
    /// Mac: 不支持
    /// Android: 系统支持的格式，请参考 `Supported media formats &lt;https://developer.android.com/guide/topics/media/media-formats&gt;`__ 。
    /// iOS: 系统支持的格式，当前没有有效的参考文档
    /// </para>
    /// </summary>
    internal class VideoPlayer : RefBase
    {
        internal VideoPlayer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new VideoPlayer(cdata_new, deleter_, retainer_);
        }
        public new VideoPlayer Clone()
        {
            return (VideoPlayer)(CloneObject());
        }
        public VideoPlayer() : base(IntPtr.Zero, Detail.easyar_VideoPlayer__dtor, Detail.easyar_VideoPlayer__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_VideoPlayer__ctor(out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if the component is available. It returns true only on Windows, Android or iOS. It&#39;s not available on Mac.
        /// </para>
        /// <para xml:lang="zh">
        /// 检查是否可用。只在Windows、Android和iOS上返回true，Mac上不可用。
        /// </para>
        /// </summary>
        public static bool isAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_isAvailable();
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the video type. The type will default to normal video if not set manually. It should be called before open.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置视频类型。如果没有手动设置，将默认为普通类型。这个方法需要在open之前调用。
        /// </para>
        /// </summary>
        public virtual void setVideoType(VideoType videoType)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoPlayer_setVideoType(cdata, videoType);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Passes the texture to display video into player. It should be set before open.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入用来显示视频的texture到播放器。这个方法需要在open之前调用。
        /// </para>
        /// </summary>
        public virtual void setRenderTexture(TextureId texture)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoPlayer_setRenderTexture(cdata, texture.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Opens a video from path.
        /// path can be a local video file (path/to/video.mp4) or url (http://www.../.../video.mp4). storageType indicates the type of path. See `StorageType`_ for more description.
        /// This method is an asynchronous method. Open may take some time to finish. If you want to know the open result or the play status while playing, you have to handle callback. The callback will be called from a different thread. You can check if the open finished successfully and start play after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 从 path 打开视频。
        /// path 可以是本地视频文件（path/to/video.mp4）或url（http://www.../.../video.mp4）。storageType 表示path的类型。详细描述参见 `StorageType`_ 。
        /// 这个方法是异步的方法。open可能会花一些时间才能完成。如果你希望知道视频打开的结果或播放中的状态，需要处理callback数据。callback会在callbackScheduler对应的线程中被调用。你可以在回调中检查打开是否成功结束并在成功打开之后开始播放。
        /// </para>
        /// </summary>
        public virtual void open(string path, StorageType storageType, CallbackScheduler callbackScheduler, Optional<Action<VideoStatus>> callback)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoPlayer_open(cdata, Detail.String_to_c(ar, path), storageType, callbackScheduler.cdata, callback.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromVideoStatus { has_value = true, value = Detail.FunctorOfVoidFromVideoStatus_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromVideoStatus { has_value = false, value = default(Detail.FunctorOfVoidFromVideoStatus) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Closes the video.
        /// </para>
        /// <para xml:lang="zh">
        /// 关闭视频。
        /// </para>
        /// </summary>
        public virtual void close()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoPlayer_close(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Starts or continues to play video.
        /// </para>
        /// <para xml:lang="zh">
        /// 开始或继续播放视频。
        /// </para>
        /// </summary>
        public virtual bool play()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_play(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Stops the video playback.
        /// </para>
        /// <para xml:lang="zh">
        /// 停止视频播放。
        /// </para>
        /// </summary>
        public virtual void stop()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoPlayer_stop(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Pauses the video playback.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停视频播放。
        /// </para>
        /// </summary>
        public virtual void pause()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoPlayer_pause(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks whether video texture is ready for render. Use this to check if texture passed into the player has been touched.
        /// </para>
        /// <para xml:lang="zh">
        /// 视频texture是否可以用于渲染。可以用于检查传入player的texture是否被碰过。
        /// </para>
        /// </summary>
        public virtual bool isRenderTextureAvailable()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_isRenderTextureAvailable(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Updates texture data. This should be called in the renderer thread when isRenderTextureAvailable returns true.
        /// </para>
        /// <para xml:lang="zh">
        /// 更新texture数据。这个方法需要在isRenderTextureAvailable返回true的时候在渲染线程上调用。
        /// </para>
        /// </summary>
        public virtual void updateFrame()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_VideoPlayer_updateFrame(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the video duration. Use after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回视频长度。在成功的open之后使用。
        /// </para>
        /// </summary>
        public virtual int duration()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_duration(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the current position of video. Use after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前播放到的视频位置。在成功的open之后使用。
        /// </para>
        /// </summary>
        public virtual int currentPosition()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_currentPosition(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Seeks to play to position . Use after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 将播放位置调整到 position 。在成功的open之后使用。
        /// </para>
        /// </summary>
        public virtual bool seek(int position)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_seek(cdata, position);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the video size. Use after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回视频长宽。在成功的open之后使用。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns current volume. Use after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回视频音量。在成功的open之后使用。
        /// </para>
        /// </summary>
        public virtual float volume()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_volume(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets volume of the video. Use after a successful open.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置视频音量。在成功的open之后使用。
        /// </para>
        /// </summary>
        public virtual bool setVolume(float volume)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_VideoPlayer_setVolume(cdata, volume);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Camera device type.
    /// </para>
    /// <para xml:lang="zh">
    /// 相机设备类型。
    /// </para>
    /// </summary>
    public enum CameraDeviceType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Unknown location
        /// </para>
        /// <para xml:lang="zh">
        /// 未知位置
        /// </para>
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Rear camera
        /// </para>
        /// <para xml:lang="zh">
        /// 后置camera
        /// </para>
        /// </summary>
        Back = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Front camera
        /// </para>
        /// <para xml:lang="zh">
        /// 前置camera
        /// </para>
        /// </summary>
        Front = 2,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Camera optics model type.
    /// </para>
    /// <para xml:lang="zh">
    /// 相机模型类型。
    /// </para>
    /// </summary>
    public enum CameraModelType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Pinhole camera model
        /// Parameter list is expected in the following order:
        /// fx, fy, cx, cy
        /// </para>
        /// <para xml:lang="zh">
        /// 针孔相机模型
        /// 期望的相机参数为:
        /// fx, fy, cx, cy
        /// </para>
        /// </summary>
        Pinhole = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// OpenCV camera model
        /// Parameter list is expected in the following order:
        /// fx, fy, cx, cy, k1, k2, p1, p2 or
        /// fx, fy, cx, cy, k1, k2, p1, p2, k3 or
        /// fx, fy, cx, cy, k1, k2, p1, p2, k3, k4, k5, k6
        /// </para>
        /// <para xml:lang="zh">
        /// OpenCV相机模型
        /// 期望的相机参数为:
        /// fx, fy, cx, cy, k1, k2, p1, p2 或
        /// fx, fy, cx, cy, k1, k2, p1, p2, k3 或
        /// fx, fy, cx, cy, k1, k2, p1, p2, k3, k4, k5, k6
        /// </para>
        /// </summary>
        OpenCV = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// OpenCV Fisheye camera model
        /// Parameter list is expected in the following order:
        /// fx, fy, cx, cy, k1 or
        /// fx, fy, cx, cy, k1, k2, k3, k4
        /// </para>
        /// <para xml:lang="zh">
        /// OpenCV鱼眼相机模型
        /// 期望的相机参数为:
        /// fx, fy, cx, cy, k1 或
        /// fx, fy, cx, cy, k1, k2, k3, k4
        /// </para>
        /// </summary>
        OpenCV_Fisheye = 2,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Camera transform type.
    /// </para>
    /// <para xml:lang="zh">
    /// 相机变换类型。
    /// </para>
    /// </summary>
    public enum CameraTransformType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// 0DOF
        /// camera transform without rotation or translation
        /// </para>
        /// <para xml:lang="zh">
        /// 0DOF
        /// 没有任何相机外参的旋转、平移参数
        /// </para>
        /// </summary>
        ZeroDof = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// ThreeDofRotOnly
        /// camera transform with 3DOF rotation only
        /// </para>
        /// <para xml:lang="zh">
        /// ThreeDofRotOnly
        /// 提供部分相机外参，仅包含3自由度的旋转
        /// </para>
        /// </summary>
        ThreeDofRotOnly = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// 6DOF
        /// camera transform with fully 6DOF of both rotation and translation
        /// </para>
        /// <para xml:lang="zh">
        /// 6DOF
        /// 提供完整的相机外参，包括旋转和平移
        /// </para>
        /// </summary>
        SixDof = 2,
        /// <summary>
        /// <para xml:lang="en">
        /// 5DOF
        /// camera transform with rotation and 2D translation (without translation in the gravity direction)
        /// </para>
        /// <para xml:lang="zh">
        /// 5DOF
        /// 提供相机外参，包括旋转和2D平移(不包括重力方向平移)
        /// </para>
        /// </summary>
        FiveDofRotXZ = 3,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// MotionTrackingStatus describes the quality of device motion tracking.
    /// </para>
    /// <para xml:lang="zh">
    /// 描述设备运动跟踪的质量。
    /// </para>
    /// </summary>
    public enum MotionTrackingStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Result is not available and should not to be used to render virtual objects or do 3D reconstruction. This value occurs temporarily after initializing, tracking lost or relocalizing.
        /// </para>
        /// <para xml:lang="zh">
        /// 结果不可用，原因可能是正在初始化，跟踪丢失或者正在重定位。该状态不可以渲染物体或者做三维重建。
        /// </para>
        /// </summary>
        NotTracking = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// Tracking is available, but the quality of the result is not good enough. This value occurs temporarily due to weak texture or excessive movement. The result can be used to render virtual objects, but should generally not be used to do 3D reconstruction.
        /// </para>
        /// <para xml:lang="zh">
        /// 跟踪是有效的，但是结果不太好，原因可能是当前区域纹理太弱或运动过快。建议用来渲染物体，但不建议做三维重建。
        /// </para>
        /// </summary>
        Limited = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// Tracking with a good quality. The result can be used to render virtual objects or do 3D reconstruction.
        /// </para>
        /// <para xml:lang="zh">
        /// 跟踪质量好，可以用来渲染物体或者做三维重建。
        /// </para>
        /// </summary>
        Tracking = 2,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Camera parameters, including image size, focal length, principal point, camera type and camera rotation against natural orientation.
    /// </para>
    /// <para xml:lang="zh">
    /// camera参数，包括图像大小、焦距、主点、camera类型和camera相对设备自然方向的旋转角度。
    /// </para>
    /// </summary>
    public class CameraParameters : RefBase
    {
        internal CameraParameters(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new CameraParameters(cdata_new, deleter_, retainer_);
        }
        public new CameraParameters Clone()
        {
            return (CameraParameters)(CloneObject());
        }
        public CameraParameters(Vec2I imageSize, Vec2F focalLength, Vec2F principalPoint, CameraDeviceType cameraDeviceType, int cameraOrientation) : base(IntPtr.Zero, Detail.easyar_CameraParameters__dtor, Detail.easyar_CameraParameters__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_CameraParameters__ctor(imageSize, focalLength, principalPoint, cameraDeviceType, cameraOrientation, out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Image size.
        /// </para>
        /// <para xml:lang="zh">
        /// 图像大小。
        /// </para>
        /// </summary>
        public virtual Vec2I size()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_size(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Focal length, the distance from effective optical center to CCD plane, divided by unit pixel density in width and height directions. The unit is pixel.
        /// </para>
        /// <para xml:lang="zh">
        /// 焦距。相机的等效光心到CCD平面的距离除以宽高两个方向的单位像素密度。单位为像素。
        /// </para>
        /// </summary>
        public virtual Vec2F focalLength()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_focalLength(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Principal point, coordinates of the intersection point of principal axis on CCD plane against the left-top corner of the image. The unit is pixel.
        /// </para>
        /// <para xml:lang="zh">
        /// 主点。相机的主光轴在CCD平面上的交点到图像左上角的像素坐标。单位为像素。
        /// </para>
        /// </summary>
        public virtual Vec2F principalPoint()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_principalPoint(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Optics model camera used.
        /// </para>
        /// <para xml:lang="zh">
        /// 相机模型。
        /// </para>
        /// </summary>
        public virtual CameraModelType cameraModelType()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_cameraModelType(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera device type. Default, back or front camera. On desktop devices, there are only default cameras. On mobile devices, there is a differentiation between back and front cameras.
        /// </para>
        /// <para xml:lang="zh">
        /// 相机设备类型。默认camera、后置camera或前置camera。桌面设备均为默认camera，移动设备区分后置camera和前置camera。
        /// </para>
        /// </summary>
        public virtual CameraDeviceType cameraDeviceType()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_cameraDeviceType(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Angles rotation required to rotate clockwise and display camera image on device with natural orientation. The range is [0, 360).
        /// For Android phones and some Android tablets, this value is 90 degrees.
        /// For Android eye-wear and some Android tablets, this value is 0 degrees.
        /// For all current iOS devices, this value is 90 degrees.
        /// </para>
        /// <para xml:lang="zh">
        /// camera图像在设备的自然方向上显示时需要顺时针旋转的角度。范围为[0, 360)。
        /// Android手机和部分Android平板为90度。
        /// Android眼镜和部分Android平板为0度。
        /// 现有iOS设备均为90度。
        /// </para>
        /// </summary>
        public virtual int cameraOrientation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_cameraOrientation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates CameraParameters with default camera intrinsics. Default intrinsics are calculated by image size, which is not very precise.
        /// </para>
        /// <para xml:lang="zh">
        /// 以默认相机内参创建CameraParameters。默认相机内参（焦距、主点）根据图像大小自动计算，但并不是特别准确。
        /// </para>
        /// </summary>
        public static CameraParameters createWithDefaultIntrinsics(Vec2I imageSize, CameraDeviceType cameraDeviceType, int cameraOrientation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CameraParameters_createWithDefaultIntrinsics(imageSize, cameraDeviceType, cameraOrientation, out _return_value_);
                return Detail.Object_from_c<CameraParameters>(_return_value_, Detail.easyar_CameraParameters__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates CameraParameters with custom camera intrinsics. Refer to CameraModelType to see the support of different camera models.
        /// Note: Use this interface with caution. Incorrect input data may cause a fail, resulting in a null return.&quot;
        /// </para>
        /// <para xml:lang="zh">
        /// 使用自定义相机内参创建CameraParameters。需要指定相机内参（焦距、主点、畸变系数）以及相机模型。相机模型支持参考CameraModelType。
        /// 注意: 谨慎使用该接口，输入数据错误可能导致创建对象失败，返回为空。
        /// </para>
        /// </summary>
        public static Optional<CameraParameters> tryCreateWithCustomIntrinsics(Vec2I imageSize, List<float> cameraParamList, CameraModelType cameraModel, CameraDeviceType cameraDeviceType, int cameraOrientation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfCameraParameters);
                Detail.easyar_CameraParameters_tryCreateWithCustomIntrinsics(imageSize, Detail.ListOfFloat_to_c(ar, cameraParamList), cameraModel, cameraDeviceType, cameraOrientation, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<CameraParameters>(p.value, Detail.easyar_CameraParameters__typeName) : Optional<CameraParameters>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Get equivalent CameraParameters for a different camera image size.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取相机图像大小改变后的等效CameraParameters。
        /// </para>
        /// </summary>
        public virtual CameraParameters getResized(Vec2I imageSize)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_CameraParameters_getResized(cdata, imageSize, out _return_value_);
                return Detail.Object_from_c<CameraParameters>(_return_value_, Detail.easyar_CameraParameters__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Calculates the angle required to rotate the camera image clockwise to align it with the screen.
        /// screenRotation is the angle of rotation of displaying screen image against device natural orientation in clockwise in degrees.
        /// For iOS(UIInterfaceOrientationPortrait as natural orientation):
        /// * UIInterfaceOrientationPortrait: rotation = 0
        /// * UIInterfaceOrientationLandscapeRight: rotation = 90
        /// * UIInterfaceOrientationPortraitUpsideDown: rotation = 180
        /// * UIInterfaceOrientationLandscapeLeft: rotation = 270
        /// For Android:
        /// * Surface.ROTATION_0 = 0
        /// * Surface.ROTATION_90 = 90
        /// * Surface.ROTATION_180 = 180
        /// * Surface.ROTATION_270 = 270
        /// </para>
        /// <para xml:lang="zh">
        /// 计算图像需要相对于屏幕进行顺时针旋转以和屏幕对齐所需要的角度。
        /// screenRotation为屏幕图像相对于自然方向顺时针旋转的角度。
        /// 对于iOS，有
        /// * UIInterfaceOrientationPortrait: screenRotation = 0
        /// * UIInterfaceOrientationLandscapeRight: screenRotation = 90
        /// * UIInterfaceOrientationPortraitUpsideDown: screenRotation = 180
        /// * UIInterfaceOrientationLandscapeLeft: screenRotation = 270
        /// 对于Android，有
        /// * Surface.ROTATION_0: screenRotation = 0
        /// * Surface.ROTATION_90: screenRotation = 90
        /// * Surface.ROTATION_180: screenRotation = 180
        /// * Surface.ROTATION_270: screenRotation = 270
        /// </para>
        /// </summary>
        public virtual int imageOrientation(int screenRotation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_imageOrientation(cdata, screenRotation);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Calculates whether the image needed to be flipped horizontally. The image is rotated, then flipped in rendering. When cameraDeviceType is front, a flip is automatically applied. Pass manualHorizontalFlip with true to add a manual flip.
        /// </para>
        /// <para xml:lang="zh">
        /// 计算图像是否需要左右翻转。图像渲染时，先进行旋转，再进行翻转。当cameraDeviceType为前置摄像头时，会自动进行翻转，可在此基础上，传入manualHorizontalFlip再叠加一次手动翻转。
        /// </para>
        /// </summary>
        public virtual bool imageHorizontalFlip(bool manualHorizontalFlip)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_imageHorizontalFlip(cdata, manualHorizontalFlip);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Calculates the perspective projection matrix needed by virtual object rendering. The projection transforms points from camera coordinate system to clip coordinate system ([-1, 1]^4)  (including rotation around z-axis). The form of perspective projection matrix is the same as OpenGL, that matrix multiply column vector of homogeneous coordinates of point on the right, ant not like Direct3D, that matrix multiply row vector of homogeneous coordinates of point on the left. But data arrangement is row-major, not like OpenGL&#39;s column-major. Clip coordinate system and normalized device coordinate system are defined as the same as OpenGL&#39;s default.
        /// </para>
        /// <para xml:lang="zh">
        /// 计算渲染虚拟物体所需要的透视投影矩阵，将camera坐标系下的点变换到剪裁坐标系（[-1, 1]^4）中（包括绕z轴的旋转）。透视投影矩阵的形式和OpenGL相同，为矩阵右边乘以点的齐次坐标的列向量，而非Direct3D的矩阵左边乘以点的齐次坐标的列向量。但数据的排列方式为row-major，与OpenGL的column-major相反。剪裁坐标系和单位化设备坐标系的定义与OpenGL默认相同。
        /// </para>
        /// </summary>
        public virtual Matrix44F projection(float nearPlane, float farPlane, float viewportAspectRatio, int screenRotation, bool combiningFlip, bool manualHorizontalFlip)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_projection(cdata, nearPlane, farPlane, viewportAspectRatio, screenRotation, combiningFlip, manualHorizontalFlip);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Calculates the orthogonal projection matrix needed by camera background rendering. The projection transforms points from image quad coordinate system ([-1, 1]^2) to clip coordinate system ([-1, 1]^4) (including rotation around z-axis), with the undefined two dimensions unchanged. The form of orthogonal projection matrix is the same as OpenGL, that matrix multiply column vector of homogeneous coordinates of point on the right, ant not like Direct3D, that matrix multiply row vector of homogeneous coordinates of point on the left. But data arrangement is row-major, not like OpenGL&#39;s column-major. Clip coordinate system and normalized device coordinate system are defined as the same as OpenGL&#39;s default.
        /// </para>
        /// <para xml:lang="zh">
        /// 计算渲染摄像机背景图像所需要的正交投影矩阵，将图像矩形坐标系下（[-1, 1]^2）的点变换到剪裁坐标系（[-1, 1]^4）中（包括绕z轴的旋转），未定义的两维保持不变。正交投影矩阵的形式和OpenGL相同，为矩阵右边乘以点的齐次坐标，而非Direct3D的矩阵左边乘以点的齐次坐标。但数据的排列方式为row-major，与OpenGL的column-major相反。剪裁坐标系和单位化设备坐标系的定义与OpenGL默认相同。
        /// </para>
        /// </summary>
        public virtual Matrix44F imageProjection(float viewportAspectRatio, int screenRotation, bool combiningFlip, bool manualHorizontalFlip)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_imageProjection(cdata, viewportAspectRatio, screenRotation, combiningFlip, manualHorizontalFlip);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Transforms points from image coordinate system ([0, 1]^2) to screen coordinate system ([0, 1]^2). Both coordinate system is x-left, y-down, with origin at left-top.
        /// </para>
        /// <para xml:lang="zh">
        /// 从图像坐标系（[0, 1]^2）变换到屏幕坐标系（[0, 1]^2），两个坐标系均x朝右、y朝下，原点在左上角。
        /// </para>
        /// </summary>
        public virtual Vec2F screenCoordinatesFromImageCoordinates(float viewportAspectRatio, int screenRotation, bool combiningFlip, bool manualHorizontalFlip, Vec2F imageCoordinates)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_screenCoordinatesFromImageCoordinates(cdata, viewportAspectRatio, screenRotation, combiningFlip, manualHorizontalFlip, imageCoordinates);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Transforms points from screen coordinate system ([0, 1]^2) to image coordinate system ([0, 1]^2). Both coordinate system is x-left, y-down, with origin at left-top.
        /// </para>
        /// <para xml:lang="zh">
        /// 从屏幕坐标系（[0, 1]^2）变换到图像坐标系（[0, 1]^2），两个坐标系均x朝右、y朝下，原点在左上角。
        /// </para>
        /// </summary>
        public virtual Vec2F imageCoordinatesFromScreenCoordinates(float viewportAspectRatio, int screenRotation, bool combiningFlip, bool manualHorizontalFlip, Vec2F screenCoordinates)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_imageCoordinatesFromScreenCoordinates(cdata, viewportAspectRatio, screenRotation, combiningFlip, manualHorizontalFlip, screenCoordinates);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if two groups of parameters are equal.
        /// </para>
        /// <para xml:lang="zh">
        /// 判断两组参数是否相等。
        /// </para>
        /// </summary>
        public virtual bool equalsTo(CameraParameters other)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_CameraParameters_equalsTo(cdata, other.cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Signal input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 信号输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class SignalSink : RefBase
    {
        internal SignalSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SignalSink(cdata_new, deleter_, retainer_);
        }
        public new SignalSink Clone()
        {
            return (SignalSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SignalSink_handle(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Signal output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 信号输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class SignalSource : RefBase
    {
        internal SignalSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new SignalSource(cdata_new, deleter_, retainer_);
        }
        public new SignalSource Clone()
        {
            return (SignalSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SignalSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoid { has_value = true, value = Detail.FunctorOfVoid_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoid { has_value = false, value = default(Detail.FunctorOfVoid) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(SignalSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SignalSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_SignalSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Accelerometer result input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 加速度计结果输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class AccelerometerResultSink : RefBase
    {
        internal AccelerometerResultSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new AccelerometerResultSink(cdata_new, deleter_, retainer_);
        }
        public new AccelerometerResultSink Clone()
        {
            return (AccelerometerResultSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(AccelerometerResult inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AccelerometerResultSink_handle(cdata, inputData);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Accelerometer result output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 加速度计结果输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class AccelerometerResultSource : RefBase
    {
        internal AccelerometerResultSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new AccelerometerResultSource(cdata_new, deleter_, retainer_);
        }
        public new AccelerometerResultSource Clone()
        {
            return (AccelerometerResultSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<AccelerometerResult>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AccelerometerResultSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromAccelerometerResult { has_value = true, value = Detail.FunctorOfVoidFromAccelerometerResult_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromAccelerometerResult { has_value = false, value = default(Detail.FunctorOfVoidFromAccelerometerResult) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(AccelerometerResultSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AccelerometerResultSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AccelerometerResultSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Gyroscope result input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 陀螺仪结果输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class GyroscopeResultSink : RefBase
    {
        internal GyroscopeResultSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new GyroscopeResultSink(cdata_new, deleter_, retainer_);
        }
        public new GyroscopeResultSink Clone()
        {
            return (GyroscopeResultSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(GyroscopeResult inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_GyroscopeResultSink_handle(cdata, inputData);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Gyroscope result output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 陀螺仪结果输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class GyroscopeResultSource : RefBase
    {
        internal GyroscopeResultSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new GyroscopeResultSource(cdata_new, deleter_, retainer_);
        }
        public new GyroscopeResultSource Clone()
        {
            return (GyroscopeResultSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<GyroscopeResult>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_GyroscopeResultSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromGyroscopeResult { has_value = true, value = Detail.FunctorOfVoidFromGyroscopeResult_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromGyroscopeResult { has_value = false, value = default(Detail.FunctorOfVoidFromGyroscopeResult) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(GyroscopeResultSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_GyroscopeResultSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_GyroscopeResultSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Attitude sensor result input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 姿态传感器结果输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class AttitudeSensorResultSink : RefBase
    {
        internal AttitudeSensorResultSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new AttitudeSensorResultSink(cdata_new, deleter_, retainer_);
        }
        public new AttitudeSensorResultSink Clone()
        {
            return (AttitudeSensorResultSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(AttitudeSensorResult inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AttitudeSensorResultSink_handle(cdata, inputData);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Attitude sensor result output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 姿态传感器结果输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class AttitudeSensorResultSource : RefBase
    {
        internal AttitudeSensorResultSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new AttitudeSensorResultSource(cdata_new, deleter_, retainer_);
        }
        public new AttitudeSensorResultSource Clone()
        {
            return (AttitudeSensorResultSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<AttitudeSensorResult>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AttitudeSensorResultSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromAttitudeSensorResult { has_value = true, value = Detail.FunctorOfVoidFromAttitudeSensorResult_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromAttitudeSensorResult { has_value = false, value = default(Detail.FunctorOfVoidFromAttitudeSensorResult) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(AttitudeSensorResultSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AttitudeSensorResultSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_AttitudeSensorResultSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Magnetometer result input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 磁力计结果输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class MagnetometerResultSink : RefBase
    {
        internal MagnetometerResultSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MagnetometerResultSink(cdata_new, deleter_, retainer_);
        }
        public new MagnetometerResultSink Clone()
        {
            return (MagnetometerResultSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(MagnetometerResult inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MagnetometerResultSink_handle(cdata, inputData);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Magnetometer result output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 磁力计结果输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class MagnetometerResultSource : RefBase
    {
        internal MagnetometerResultSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MagnetometerResultSource(cdata_new, deleter_, retainer_);
        }
        public new MagnetometerResultSource Clone()
        {
            return (MagnetometerResultSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<MagnetometerResult>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MagnetometerResultSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromMagnetometerResult { has_value = true, value = Detail.FunctorOfVoidFromMagnetometerResult_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromMagnetometerResult { has_value = false, value = default(Detail.FunctorOfVoidFromMagnetometerResult) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(MagnetometerResultSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MagnetometerResultSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_MagnetometerResultSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Location result input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 位置结果输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class LocationResultSink : RefBase
    {
        internal LocationResultSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new LocationResultSink(cdata_new, deleter_, retainer_);
        }
        public new LocationResultSink Clone()
        {
            return (LocationResultSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(LocationResult inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_LocationResultSink_handle(cdata, inputData);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Location result output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 位置结果输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class LocationResultSource : RefBase
    {
        internal LocationResultSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new LocationResultSource(cdata_new, deleter_, retainer_);
        }
        public new LocationResultSource Clone()
        {
            return (LocationResultSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<LocationResult>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_LocationResultSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromLocationResult { has_value = true, value = Detail.FunctorOfVoidFromLocationResult_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromLocationResult { has_value = false, value = default(Detail.FunctorOfVoidFromLocationResult) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(LocationResultSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_LocationResultSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_LocationResultSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Proximity location result input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 邻近位置结果输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class ProximityLocationResultSink : RefBase
    {
        internal ProximityLocationResultSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ProximityLocationResultSink(cdata_new, deleter_, retainer_);
        }
        public new ProximityLocationResultSink Clone()
        {
            return (ProximityLocationResultSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(ProximityLocationResult inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ProximityLocationResultSink_handle(cdata, inputData);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Proximity location result output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 邻近位置结果输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class ProximityLocationResultSource : RefBase
    {
        internal ProximityLocationResultSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new ProximityLocationResultSource(cdata_new, deleter_, retainer_);
        }
        public new ProximityLocationResultSource Clone()
        {
            return (ProximityLocationResultSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<ProximityLocationResult>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ProximityLocationResultSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromProximityLocationResult { has_value = true, value = Detail.FunctorOfVoidFromProximityLocationResult_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromProximityLocationResult { has_value = false, value = default(Detail.FunctorOfVoidFromProximityLocationResult) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(ProximityLocationResultSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ProximityLocationResultSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_ProximityLocationResultSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class InputFrameSink : RefBase
    {
        internal InputFrameSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrameSink(cdata_new, deleter_, retainer_);
        }
        public new InputFrameSink Clone()
        {
            return (InputFrameSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(InputFrame inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InputFrameSink_handle(cdata, inputData.cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class InputFrameSource : RefBase
    {
        internal InputFrameSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrameSource(cdata_new, deleter_, retainer_);
        }
        public new InputFrameSource Clone()
        {
            return (InputFrameSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<InputFrame>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InputFrameSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromInputFrame { has_value = true, value = Detail.FunctorOfVoidFromInputFrame_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromInputFrame { has_value = false, value = default(Detail.FunctorOfVoidFromInputFrame) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(InputFrameSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InputFrameSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_InputFrameSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Output frame input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输出帧输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class OutputFrameSink : RefBase
    {
        internal OutputFrameSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new OutputFrameSink(cdata_new, deleter_, retainer_);
        }
        public new OutputFrameSink Clone()
        {
            return (OutputFrameSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(OutputFrame inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_OutputFrameSink_handle(cdata, inputData.cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Output frame output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输出帧输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class OutputFrameSource : RefBase
    {
        internal OutputFrameSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new OutputFrameSource(cdata_new, deleter_, retainer_);
        }
        public new OutputFrameSource Clone()
        {
            return (OutputFrameSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<OutputFrame>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_OutputFrameSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromOutputFrame { has_value = true, value = Detail.FunctorOfVoidFromOutputFrame_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromOutputFrame { has_value = false, value = default(Detail.FunctorOfVoidFromOutputFrame) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(OutputFrameSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_OutputFrameSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_OutputFrameSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Feedback frame input port.
    /// It is used to expose input port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 反馈帧输入端口。
    /// 用于暴露一个组件的输入端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class FeedbackFrameSink : RefBase
    {
        internal FeedbackFrameSink(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new FeedbackFrameSink(cdata_new, deleter_, retainer_);
        }
        public new FeedbackFrameSink Clone()
        {
            return (FeedbackFrameSink)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 传入一个数据。
        /// </para>
        /// </summary>
        public virtual void handle(FeedbackFrame inputData)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_FeedbackFrameSink_handle(cdata, inputData.cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Feedback frame output port.
    /// It is used to expose output port for a component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 反馈帧输出端口。
    /// 用于暴露一个组件的输出端口。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class FeedbackFrameSource : RefBase
    {
        internal FeedbackFrameSource(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new FeedbackFrameSource(cdata_new, deleter_, retainer_);
        }
        public new FeedbackFrameSource Clone()
        {
            return (FeedbackFrameSource)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets data handler.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置数据处理器。
        /// </para>
        /// </summary>
        public virtual void setHandler(Optional<Action<FeedbackFrame>> handler)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_FeedbackFrameSource_setHandler(cdata, handler.map(p => p.OnSome ? new Detail.OptionalOfFunctorOfVoidFromFeedbackFrame { has_value = true, value = Detail.FunctorOfVoidFromFeedbackFrame_to_c(p.Value) } : new Detail.OptionalOfFunctorOfVoidFromFeedbackFrame { has_value = false, value = default(Detail.FunctorOfVoidFromFeedbackFrame) }));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Connects to input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 连接输入端口。
        /// </para>
        /// </summary>
        public virtual void connect(FeedbackFrameSink sink)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_FeedbackFrameSource_connect(cdata, sink.cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Disconnects.
        /// </para>
        /// <para xml:lang="zh">
        /// 断开连接。
        /// </para>
        /// </summary>
        public virtual void disconnect()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_FeedbackFrameSource_disconnect(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame fork.
    /// It is used to branch and transfer input frame to multiple components in parallel.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧分流器。
    /// 用于将一个输入帧并行传输到多个组件中。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class InputFrameFork : RefBase
    {
        internal InputFrameFork(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrameFork(cdata_new, deleter_, retainer_);
        }
        public new InputFrameFork Clone()
        {
            return (InputFrameFork)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameFork_input(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource output(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameFork_output(cdata, index, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output count.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出个数。
        /// </para>
        /// </summary>
        public virtual int outputCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrameFork_outputCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static InputFrameFork create(int outputCount)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameFork_create(outputCount, out _return_value_);
                return Detail.Object_from_c<InputFrameFork>(_return_value_, Detail.easyar_InputFrameFork__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Output frame fork.
    /// It is used to branch and transfer output frame to multiple components in parallel.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输出帧分流器。
    /// 用于将一个输出帧并行传输到多个组件中。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class OutputFrameFork : RefBase
    {
        internal OutputFrameFork(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new OutputFrameFork(cdata_new, deleter_, retainer_);
        }
        public new OutputFrameFork Clone()
        {
            return (OutputFrameFork)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameFork_input(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSink>(_return_value_, Detail.easyar_OutputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource output(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameFork_output(cdata, index, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output count.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出个数。
        /// </para>
        /// </summary>
        public virtual int outputCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_OutputFrameFork_outputCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static OutputFrameFork create(int outputCount)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameFork_create(outputCount, out _return_value_);
                return Detail.Object_from_c<OutputFrameFork>(_return_value_, Detail.easyar_OutputFrameFork__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Output frame join.
    /// It is used to aggregate output frame from multiple components in parallel.
    /// All members of this class is thread-safe.
    /// It shall be noticed that connections and disconnections to the inputs shall not be performed during the flowing of data, or it may stuck in a state that no frame can be output. (It is recommended to complete dataflow connection before start a camera.)
    /// </para>
    /// <para xml:lang="zh">
    /// 输出帧合流器。
    /// 用于将多个组件的输出帧合并成一个输出帧。
    /// 本类的所有成员都是线程安全的。
    /// 需要注意其多个输入的连接和断开不应该在有数据流入的同时进行，否则可能会陷入不能输出的状态。（推荐在Camera启动之前完成数据流连接。）
    /// </para>
    /// </summary>
    internal class OutputFrameJoin : RefBase
    {
        internal OutputFrameJoin(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new OutputFrameJoin(cdata_new, deleter_, retainer_);
        }
        public new OutputFrameJoin Clone()
        {
            return (OutputFrameJoin)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSink input(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameJoin_input(cdata, index, out _return_value_);
                return Detail.Object_from_c<OutputFrameSink>(_return_value_, Detail.easyar_OutputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameJoin_output(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input count.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入个数。
        /// </para>
        /// </summary>
        public virtual int inputCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_OutputFrameJoin_inputCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance. The default joiner will be used, which takes input frame from the first input and first result or null of each input. The first result of every input will be placed at the corresponding input index of results of the final output frame.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。使用默认的合流函数，其实现为取第一个输入的输入帧，并取每个输入的第一个结果。对每个输入，如果没有结果，则取空结果。每个输入的第一个结果将被放在最终输出帧的results的对应输入索引处。
        /// </para>
        /// </summary>
        public static OutputFrameJoin create(int inputCount)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameJoin_create(inputCount, out _return_value_);
                return Detail.Object_from_c<OutputFrameJoin>(_return_value_, Detail.easyar_OutputFrameJoin__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance. A custom joiner is specified.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。使用自定义合流函数。
        /// </para>
        /// </summary>
        public static OutputFrameJoin createWithJoiner(int inputCount, Func<List<OutputFrame>, OutputFrame> joiner)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameJoin_createWithJoiner(inputCount, Detail.FunctorOfOutputFrameFromListOfOutputFrame_to_c(joiner), out _return_value_);
                return Detail.Object_from_c<OutputFrameJoin>(_return_value_, Detail.easyar_OutputFrameJoin__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Feedback frame fork.
    /// It is used to branch and transfer feedback frame to multiple components in parallel.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 反馈帧分流器。
    /// 用于将一个反馈帧并行传输到多个组件中。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class FeedbackFrameFork : RefBase
    {
        internal FeedbackFrameFork(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new FeedbackFrameFork(cdata_new, deleter_, retainer_);
        }
        public new FeedbackFrameFork Clone()
        {
            return (FeedbackFrameFork)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual FeedbackFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_FeedbackFrameFork_input(cdata, out _return_value_);
                return Detail.Object_from_c<FeedbackFrameSink>(_return_value_, Detail.easyar_FeedbackFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual FeedbackFrameSource output(int index)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_FeedbackFrameFork_output(cdata, index, out _return_value_);
                return Detail.Object_from_c<FeedbackFrameSource>(_return_value_, Detail.easyar_FeedbackFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output count.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出个数。
        /// </para>
        /// </summary>
        public virtual int outputCount()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_FeedbackFrameFork_outputCount(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static FeedbackFrameFork create(int outputCount)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_FeedbackFrameFork_create(outputCount, out _return_value_);
                return Detail.Object_from_c<FeedbackFrameFork>(_return_value_, Detail.easyar_FeedbackFrameFork__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame throttler.
    /// There is a input frame input port and a input frame output port. It can be used to prevent incoming frames from entering algorithm components when they have not finished handling previous workload.
    /// InputFrameThrottler occupies one buffer of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components.
    /// All members of this class is thread-safe.
    /// It shall be noticed that connections and disconnections to signalInput shall not be performed during the flowing of data, or it may stuck in a state that no frame can be output. (It is recommended to complete dataflow connection before start a camera.)
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧节流器。
    /// 有一个输入帧输入端口和输入帧输出端口，用于在算法组件未完成处理上一帧数据的时候阻止新的输入帧进入算法组件。
    /// InputFrameThrottler占用1个camera的buffer。应使用camera的setBufferCapacity设置不少于所有组件占用的camera的buffer数量。
    /// 本类的所有成员都是线程安全的。
    /// 需要注意其signalInput的连接和断开不应该在有数据流入的同时进行，否则可能会陷入不能输出的状态。（推荐在Camera启动之前完成数据流连接。）
    /// </para>
    /// </summary>
    internal class InputFrameThrottler : RefBase
    {
        internal InputFrameThrottler(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrameThrottler(cdata_new, deleter_, retainer_);
        }
        public new InputFrameThrottler Clone()
        {
            return (InputFrameThrottler)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameThrottler_input(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrameThrottler_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameThrottler_output(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSource>(_return_value_, Detail.easyar_InputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port for clearance signal.
        /// </para>
        /// <para xml:lang="zh">
        /// 放行信号输入端口。
        /// </para>
        /// </summary>
        public virtual SignalSink signalInput()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameThrottler_signalInput(cdata, out _return_value_);
                return Detail.Object_from_c<SignalSink>(_return_value_, Detail.easyar_SignalSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static InputFrameThrottler create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameThrottler_create(out _return_value_);
                return Detail.Object_from_c<InputFrameThrottler>(_return_value_, Detail.easyar_InputFrameThrottler__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Output frame buffer.
    /// There is an output frame input port and output frame fetching function. It can be used to convert output frame fetching from asynchronous pattern to synchronous polling pattern, which fits frame by frame rendering.
    /// OutputFrameBuffer occupies one buffer of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输出帧缓存。
    /// 有一个输出帧输入端口和输出帧获取函数，用于将输出帧的获取方式从异步转化为同步轮询，适合逐帧渲染。
    /// OutputFrameBuffer占用1个camera的buffer。应使用camera的setBufferCapacity设置不少于所有组件占用的camera的buffer数量。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class OutputFrameBuffer : RefBase
    {
        internal OutputFrameBuffer(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new OutputFrameBuffer(cdata_new, deleter_, retainer_);
        }
        public new OutputFrameBuffer Clone()
        {
            return (OutputFrameBuffer)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameBuffer_input(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSink>(_return_value_, Detail.easyar_OutputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_OutputFrameBuffer_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port for frame arrival. It can be connected to `InputFrameThrottler.signalInput`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 到达信号输出端口。可用于连接 `InputFrameThrottler.signalInput`_ 。
        /// </para>
        /// </summary>
        public virtual SignalSource signalOutput()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameBuffer_signalOutput(cdata, out _return_value_);
                return Detail.Object_from_c<SignalSource>(_return_value_, Detail.easyar_SignalSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Fetches the most recent `OutputFrame`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 获取最新的 `OutputFrame`_ 。
        /// </para>
        /// </summary>
        public virtual Optional<OutputFrame> peek()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfOutputFrame);
                Detail.easyar_OutputFrameBuffer_peek(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<OutputFrame>(p.value, Detail.easyar_OutputFrame__typeName) : Optional<OutputFrame>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static OutputFrameBuffer create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrameBuffer_create(out _return_value_);
                return Detail.Object_from_c<OutputFrameBuffer>(_return_value_, Detail.easyar_OutputFrameBuffer__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Pauses output of `OutputFrame`_ . After execution, all results of `OutputFrameBuffer.peek`_ will be empty. `OutputFrameBuffer.signalOutput`_  is not affected.
        /// </para>
        /// <para xml:lang="zh">
        /// 暂停输出 `OutputFrame`_ 。执行之后，`OutputFrameBuffer.peek`_ 的结果均为空。`OutputFrameBuffer.signalOutput`_ 不受影响。
        /// </para>
        /// </summary>
        public virtual void pause()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_OutputFrameBuffer_pause(cdata);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Resumes output of `OutputFrame`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 继续输出 `OutputFrame`_ 。
        /// </para>
        /// </summary>
        public virtual void resume()
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_OutputFrameBuffer_resume(cdata);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame to output frame adapter.
    /// There is an input frame input port and an output frame output port. It can be used to wrap an input frame into an output frame, which can be used for rendering without an algorithm component.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧到输出帧适配器。
    /// 有一个输入帧输入端口和一个输出帧输出端口，用于将输入帧包装成输出帧，实现不接入算法组件，直接进行渲染的功能。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class InputFrameToOutputFrameAdapter : RefBase
    {
        internal InputFrameToOutputFrameAdapter(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrameToOutputFrameAdapter(cdata_new, deleter_, retainer_);
        }
        public new InputFrameToOutputFrameAdapter Clone()
        {
            return (InputFrameToOutputFrameAdapter)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameToOutputFrameAdapter_input(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual OutputFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameToOutputFrameAdapter_output(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSource>(_return_value_, Detail.easyar_OutputFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static InputFrameToOutputFrameAdapter create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameToOutputFrameAdapter_create(out _return_value_);
                return Detail.Object_from_c<InputFrameToOutputFrameAdapter>(_return_value_, Detail.easyar_InputFrameToOutputFrameAdapter__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame to feedback frame adapter.
    /// There is an input frame input port, a historic output frame input port and a feedback frame output port. It can be used to combine an input frame and a historic output frame into a feedback frame, which is required by algorithm components such as `ImageTracker`_ .
    /// On every input of an input frame, a feedback frame is generated with a previously input historic feedback frame. If there is no previously input historic feedback frame, it is null in the feedback frame.
    /// InputFrameToFeedbackFrameAdapter occupies one buffer of camera. Use setBufferCapacity of camera to set an amount of buffers that is not less than the sum of amount of buffers occupied by all components.
    /// All members of this class is thread-safe.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧到反馈帧适配器。
    /// 有一个输入帧输入端口、一个历史输出帧输入端口和一个反馈帧输出端口，用于将输入帧和历史输出帧组合成反馈帧，传递给要求输入反馈帧的算法组件，例如 `ImageTracker`_ 。
    /// 每次输入帧输入时，会连带上一次输入的历史输出帧合成反馈帧。如果没有输入过历史输出帧，则反馈帧中的历史输出帧为空。
    /// InputFrameToFeedbackFrameAdapter占用1个camera的buffer。应使用camera的setBufferCapacity设置不少于所有组件占用的camera的buffer数量。
    /// 本类的所有成员都是线程安全的。
    /// </para>
    /// </summary>
    internal class InputFrameToFeedbackFrameAdapter : RefBase
    {
        internal InputFrameToFeedbackFrameAdapter(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrameToFeedbackFrameAdapter(cdata_new, deleter_, retainer_);
        }
        public new InputFrameToFeedbackFrameAdapter Clone()
        {
            return (InputFrameToFeedbackFrameAdapter)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入端口。
        /// </para>
        /// </summary>
        public virtual InputFrameSink input()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameToFeedbackFrameAdapter_input(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrameSink>(_return_value_, Detail.easyar_InputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera buffers occupied in this component.
        /// </para>
        /// <para xml:lang="zh">
        /// 当前组件占用camera buffer的数量。
        /// </para>
        /// </summary>
        public virtual int bufferRequirement()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrameToFeedbackFrameAdapter_bufferRequirement(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Side input port for historic output frame input.
        /// </para>
        /// <para xml:lang="zh">
        /// 旁路输入端口，用于输入历史输出帧。
        /// </para>
        /// </summary>
        public virtual OutputFrameSink sideInput()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameToFeedbackFrameAdapter_sideInput(cdata, out _return_value_);
                return Detail.Object_from_c<OutputFrameSink>(_return_value_, Detail.easyar_OutputFrameSink__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Output port.
        /// </para>
        /// <para xml:lang="zh">
        /// 输出端口。
        /// </para>
        /// </summary>
        public virtual FeedbackFrameSource output()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameToFeedbackFrameAdapter_output(cdata, out _return_value_);
                return Detail.Object_from_c<FeedbackFrameSource>(_return_value_, Detail.easyar_FeedbackFrameSource__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static InputFrameToFeedbackFrameAdapter create()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrameToFeedbackFrameAdapter_create(out _return_value_);
                return Detail.Object_from_c<InputFrameToFeedbackFrameAdapter>(_return_value_, Detail.easyar_InputFrameToFeedbackFrameAdapter__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Motion input data.
    /// It includes timestamp, transform matrix against world coordinate system, and tracking status.
    /// </para>
    /// <para xml:lang="zh">
    /// 运动输入数据。
    /// 包含、时间戳、相对于世界坐标系的变换和跟踪状态。
    /// </para>
    /// </summary>
    public class MotionInputData : RefBase
    {
        internal MotionInputData(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new MotionInputData(cdata_new, deleter_, retainer_);
        }
        public new MotionInputData Clone()
        {
            return (MotionInputData)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates with 6DOF transform of both rotation and position. rotation is quaternion, ordered as wxyz.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建包含旋转和平移的6DOF变换的数据。rotation为四元数，排列顺序为wxyz。
        /// </para>
        /// </summary>
        public static Optional<MotionInputData> tryCreateSixDof(double timestamp, Vec3F position, Vec4F rotation, MotionTrackingStatus tracking_status)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfMotionInputData);
                Detail.easyar_MotionInputData_tryCreateSixDof(timestamp, position, rotation, tracking_status, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<MotionInputData>(p.value, Detail.easyar_MotionInputData__typeName) : Optional<MotionInputData>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates with 5DOF transform of both rotation and 2D position. rotation is quaternion, ordered as wxyz.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建包含旋转和2D平移的5DOF变换的数据。rotation为四元数，排列顺序为wxyz。
        /// </para>
        /// </summary>
        public static Optional<MotionInputData> tryCreateFiveDofRotXZ(double timestamp, Vec3F position, Vec4F rotation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfMotionInputData);
                Detail.easyar_MotionInputData_tryCreateFiveDofRotXZ(timestamp, position, rotation, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<MotionInputData>(p.value, Detail.easyar_MotionInputData__typeName) : Optional<MotionInputData>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates with 3DOF transform of rotation only. rotation is quaternion, ordered as wxyz.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建仅包含3自由度旋转变换的数据。rotation为四元数，排列顺序为wxyz。
        /// </para>
        /// </summary>
        public static Optional<MotionInputData> tryCreateThreeDofRotOnly(double timestamp, Vec4F rotation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfMotionInputData);
                Detail.easyar_MotionInputData_tryCreateThreeDofRotOnly(timestamp, rotation, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<MotionInputData>(p.value, Detail.easyar_MotionInputData__typeName) : Optional<MotionInputData>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Timestamp. In seconds.
        /// </para>
        /// <para xml:lang="zh">
        /// 时间戳。单位为秒。
        /// </para>
        /// </summary>
        public virtual double timestamp()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionInputData_timestamp(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Transform type.
        /// </para>
        /// <para xml:lang="zh">
        /// 变换类型。
        /// </para>
        /// </summary>
        public virtual CameraTransformType transformType()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionInputData_transformType(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Transform matrix against world coordinate system.
        /// </para>
        /// <para xml:lang="zh">
        /// 相对于世界坐标系的变换。
        /// </para>
        /// </summary>
        public virtual Matrix44F transform()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionInputData_transform(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets device motion tracking status: `MotionTrackingStatus`_ . Only for 6DOF.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取设备运动跟踪状态: `MotionTrackingStatus`_ 。仅用于6DOF。
        /// </para>
        /// </summary>
        public virtual MotionTrackingStatus trackingStatus()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_MotionInputData_trackingStatus(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Input frame.
    /// It includes image, camera parameters, timestamp, camera transform matrix against world coordinate system, and tracking status,
    /// among which, camera parameters, timestamp, camera transform matrix and tracking status are all optional, but specific algorithms may have special requirements on the input.
    /// </para>
    /// <para xml:lang="zh">
    /// 输入帧。
    /// 包含图像、camera参数、时间戳、相机相对于世界坐标系的变换和跟踪状态。
    /// 其中，camera参数、时间戳、相机相对于世界坐标系的变换和跟踪状态均为可选，但特定的算法组件会对输入有特定的要求。
    /// </para>
    /// </summary>
    public class InputFrame : RefBase
    {
        internal InputFrame(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new InputFrame(cdata_new, deleter_, retainer_);
        }
        public new InputFrame Clone()
        {
            return (InputFrame)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Index, an automatic incremental value, which is different for every input frame.
        /// </para>
        /// <para xml:lang="zh">
        /// 索引，一个自增量，每个输入帧不同。
        /// </para>
        /// </summary>
        public virtual int index()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_index(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets image.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取图像。
        /// </para>
        /// </summary>
        public virtual Image image()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrame_image(cdata, out _return_value_);
                return Detail.Object_from_c<Image>(_return_value_, Detail.easyar_Image__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if there are camera parameters.
        /// </para>
        /// <para xml:lang="zh">
        /// 是否包含camera参数。
        /// </para>
        /// </summary>
        public virtual bool hasCameraParameters()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_hasCameraParameters(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets camera parameters.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取camera参数。
        /// </para>
        /// </summary>
        public virtual CameraParameters cameraParameters()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrame_cameraParameters(cdata, out _return_value_);
                return Detail.Object_from_c<CameraParameters>(_return_value_, Detail.easyar_CameraParameters__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if there is temporal information (timestamp).
        /// </para>
        /// <para xml:lang="zh">
        /// 是否包含时间信息（时间戳）。
        /// </para>
        /// </summary>
        public virtual bool hasTemporalInformation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_hasTemporalInformation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Timestamp. In seconds.
        /// </para>
        /// <para xml:lang="zh">
        /// 时间戳。单位为秒。
        /// </para>
        /// </summary>
        public virtual double timestamp()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_timestamp(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Checks if there is spatial information (cameraTransform and trackingStatus).
        /// </para>
        /// <para xml:lang="zh">
        /// 是否包含空间信息（cameraTransform和trackingStatus）。
        /// </para>
        /// </summary>
        public virtual bool hasSpatialInformation()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_hasSpatialInformation(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera transform matrix against world coordinate system. Camera coordinate system and world coordinate system are all right-handed. For the camera coordinate system, the origin is the optical center, x-right, y-up, and z in the direction of light going into camera. (The right and up, is right and up in the camera image, which can be different from these in the natural orientation of the device.) The data arrangement is row-major, not like OpenGL&#39;s column-major.
        /// </para>
        /// <para xml:lang="zh">
        /// 相机相对于世界坐标系的变换。其中camera坐标系与世界坐标系均为右手坐标系。Camera坐标系的原点为相机光心，x轴正方向为右，y轴正方向为上，z轴正方向为光线进入相机的方向。（其中的右和上，指的是相机图像中的右和上，可能和设备自然方向的可能不同。）数据的排列方式为row-major，与OpenGL的column-major相反。
        /// </para>
        /// </summary>
        public virtual Matrix44F cameraTransform()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_cameraTransform(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Camera transform type.
        /// </para>
        /// <para xml:lang="zh">
        /// 相机变换类型。
        /// </para>
        /// </summary>
        public virtual CameraTransformType cameraTransformType()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_cameraTransformType(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets device motion tracking status: `MotionTrackingStatus`_ .
        /// </para>
        /// <para xml:lang="zh">
        /// 获取设备运动跟踪状态: `MotionTrackingStatus`_ 。
        /// </para>
        /// </summary>
        public virtual MotionTrackingStatus trackingStatus()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_InputFrame_trackingStatus(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Motion input data.
        /// </para>
        /// <para xml:lang="zh">
        /// 运动输入数据。
        /// </para>
        /// </summary>
        public virtual Optional<MotionInputData> motion()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfMotionInputData);
                Detail.easyar_InputFrame_motion(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<MotionInputData>(p.value, Detail.easyar_MotionInputData__typeName) : Optional<MotionInputData>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建。
        /// </para>
        /// </summary>
        public static Optional<InputFrame> tryCreate(Image image, CameraParameters cameraParameters, double timestamp, Matrix44F cameraTransform, CameraTransformType cameraTransformType, MotionTrackingStatus trackingStatus)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfInputFrame);
                Detail.easyar_InputFrame_tryCreate(image.cdata, cameraParameters.cdata, timestamp, cameraTransform, cameraTransformType, trackingStatus, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<InputFrame>(p.value, Detail.easyar_InputFrame__typeName) : Optional<InputFrame>.Empty);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with image, camera parameters, and timestamp.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建，只包含图像、camera参数和时间戳。
        /// </para>
        /// </summary>
        public static InputFrame createWithImageAndCameraParametersAndTemporal(Image image, CameraParameters cameraParameters, double timestamp)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrame_createWithImageAndCameraParametersAndTemporal(image.cdata, cameraParameters.cdata, timestamp, out _return_value_);
                return Detail.Object_from_c<InputFrame>(_return_value_, Detail.easyar_InputFrame__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with image and camera parameters. Nothing but `CloudRecognizer`_ can work with input created by this method.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建，只包含图像和camera参数。使用该方法创建的输入只能用于 `CloudRecognizer`_ ，其它功能都无法工作。
        /// </para>
        /// </summary>
        public static InputFrame createWithImageAndCameraParameters(Image image, CameraParameters cameraParameters)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrame_createWithImageAndCameraParameters(image.cdata, cameraParameters.cdata, out _return_value_);
                return Detail.Object_from_c<InputFrame>(_return_value_, Detail.easyar_InputFrame__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates an instance with image. Nothing but `CloudRecognizer`_ can work with input created by this method.
        /// </para>
        /// <para xml:lang="zh">
        /// 创建，只包含图像。使用该方法创建的输入只能用于 `CloudRecognizer`_ ，其它功能都无法工作。
        /// </para>
        /// </summary>
        public static InputFrame createWithImage(Image image)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_InputFrame_createWithImage(image.cdata, out _return_value_);
                return Detail.Object_from_c<InputFrame>(_return_value_, Detail.easyar_InputFrame__typeName);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// FrameFilterResult is the base class for result classes of all synchronous algorithm components.
    /// </para>
    /// <para xml:lang="zh">
    /// FrameFilterResult是所有使用同步算法组件结果的基类。
    /// </para>
    /// </summary>
    internal class FrameFilterResult : RefBase
    {
        internal FrameFilterResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new FrameFilterResult(cdata_new, deleter_, retainer_);
        }
        public new FrameFilterResult Clone()
        {
            return (FrameFilterResult)(CloneObject());
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Output frame.
    /// It includes input frame and results of synchronous components.
    /// </para>
    /// <para xml:lang="zh">
    /// 输出帧。
    /// 包含输入帧和同步处理组件的输出结果。
    /// </para>
    /// </summary>
    internal class OutputFrame : RefBase
    {
        internal OutputFrame(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new OutputFrame(cdata_new, deleter_, retainer_);
        }
        public new OutputFrame Clone()
        {
            return (OutputFrame)(CloneObject());
        }
        public OutputFrame(InputFrame inputFrame, List<Optional<FrameFilterResult>> results) : base(IntPtr.Zero, Detail.easyar_OutputFrame__dtor, Detail.easyar_OutputFrame__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_OutputFrame__ctor(inputFrame.cdata, Detail.ListOfOptionalOfFrameFilterResult_to_c(ar, results), out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Index, an automatic incremental value, which is different for every output frame.
        /// </para>
        /// <para xml:lang="zh">
        /// 索引，一个自增量，每个输出帧不同。
        /// </para>
        /// </summary>
        public virtual int index()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_OutputFrame_index(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Corresponding input frame.
        /// </para>
        /// <para xml:lang="zh">
        /// 对应的输入帧。
        /// </para>
        /// </summary>
        public virtual InputFrame inputFrame()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrame_inputFrame(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrame>(_return_value_, Detail.easyar_InputFrame__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Results of synchronous components.
        /// </para>
        /// <para xml:lang="zh">
        /// 算法组件的结果。
        /// </para>
        /// </summary>
        public virtual List<Optional<FrameFilterResult>> results()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_OutputFrame_results(cdata, out _return_value_);
                return Detail.ListOfOptionalOfFrameFilterResult_from_c(ar, _return_value_);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Feedback frame.
    /// It includes an input frame and a historic output frame for use in feedback synchronous components such as `ImageTracker`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// 反馈帧。
    /// 包含一个输入帧和一个历史输出帧，用于 `ImageTracker`_ 等反馈式同步处理组件。
    /// </para>
    /// </summary>
    internal class FeedbackFrame : RefBase
    {
        internal FeedbackFrame(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new FeedbackFrame(cdata_new, deleter_, retainer_);
        }
        public new FeedbackFrame Clone()
        {
            return (FeedbackFrame)(CloneObject());
        }
        public FeedbackFrame(InputFrame inputFrame, Optional<OutputFrame> previousOutputFrame) : base(IntPtr.Zero, Detail.easyar_FeedbackFrame__dtor, Detail.easyar_FeedbackFrame__retain)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = IntPtr.Zero;
                Detail.easyar_FeedbackFrame__ctor(inputFrame.cdata, previousOutputFrame.map(p => p.OnSome ? new Detail.OptionalOfOutputFrame { has_value = true, value = p.Value.cdata } : new Detail.OptionalOfOutputFrame { has_value = false, value = default(IntPtr) }), out _return_value_);
                cdata_ = _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Input frame.
        /// </para>
        /// <para xml:lang="zh">
        /// 输入帧。
        /// </para>
        /// </summary>
        public virtual InputFrame inputFrame()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_FeedbackFrame_inputFrame(cdata, out _return_value_);
                return Detail.Object_from_c<InputFrame>(_return_value_, Detail.easyar_InputFrame__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Historic output frame.
        /// </para>
        /// <para xml:lang="zh">
        /// 历史输出帧。
        /// </para>
        /// </summary>
        public virtual Optional<OutputFrame> previousOutputFrame()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(Detail.OptionalOfOutputFrame);
                Detail.easyar_FeedbackFrame_previousOutputFrame(cdata, out _return_value_);
                return _return_value_.map(p => p.has_value ? Detail.Object_from_c<OutputFrame>(p.value, Detail.easyar_OutputFrame__typeName) : Optional<OutputFrame>.Empty);
            }
        }
    }

    public enum PermissionStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// Permission granted
        /// </para>
        /// <para xml:lang="zh">
        /// 权限被允许
        /// </para>
        /// </summary>
        Granted = 0x00000000,
        /// <summary>
        /// <para xml:lang="en">
        /// Permission denied
        /// </para>
        /// <para xml:lang="zh">
        /// 权限被拒绝
        /// </para>
        /// </summary>
        Denied = 0x00000001,
        /// <summary>
        /// <para xml:lang="en">
        /// A error happened while requesting permission.
        /// </para>
        /// <para xml:lang="zh">
        /// 申请权限过程中发生了错误
        /// </para>
        /// </summary>
        Error = 0x00000002,
    }

    public class PoseUtility : RefBase
    {
        internal PoseUtility(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new PoseUtility(cdata_new, deleter_, retainer_);
        }
        public new PoseUtility Clone()
        {
            return (PoseUtility)(CloneObject());
        }
        public static Optional<Matrix44F> createPoseWithHardwareOffset(Vec3F position, Vec4F rotation, Optional<Vec3F> offsetPosition, Optional<Vec4F> offsetRotation)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_PoseUtility_createPoseWithHardwareOffset(position, rotation, offsetPosition.map(p => p.OnSome ? new Detail.OptionalOfVec3F { has_value = true, value = p.Value } : new Detail.OptionalOfVec3F { has_value = false, value = default(Vec3F) }), offsetRotation.map(p => p.OnSome ? new Detail.OptionalOfVec4F { has_value = true, value = p.Value } : new Detail.OptionalOfVec4F { has_value = false, value = default(Vec4F) }));
                return _return_value_.map(p => p.has_value ? p.value : Optional<Matrix44F>.Empty);
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// StorageType represents where the images, jsons, videos or other files are located.
    /// StorageType specifies the root path, in all interfaces, you can use relative path relative to the root path.
    /// </para>
    /// <para xml:lang="zh">
    /// StorageType表示图像、json文件、视频或其它文件的存放位置。
    /// StorageType指定了文件存放的根目录，你可以在所有相关接口中使用相对于这个根目录的相对路径。
    /// </para>
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The app path.
        /// Android: the application&#39;s `persistent data directory &lt;https://developer.android.google.cn/reference/android/content/pm/ApplicationInfo.html#dataDir&gt;`__
        /// iOS: the application&#39;s sandbox directory
        /// Windows: Windows: the application&#39;s executable directory
        /// Mac: the application’s executable directory (if app is a bundle, this path is inside the bundle)
        /// </para>
        /// <para xml:lang="zh">
        /// app路径
        /// Android: 程序 `持久化数据目录 &lt;https://developer.android.google.cn/reference/android/content/pm/ApplicationInfo.html#dataDir&gt;`__
        /// iOS: 程序沙盒目录
        /// Windows: 可执行文件（exe）目录
        /// Mac: 可执行文件目录（如果app是一个bundle，这个目录在bundle内部）
        /// </para>
        /// </summary>
        App = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// The assets path.
        /// Android: assets directory (inside apk)
        /// iOS: the application&#39;s executable directory
        /// Windows: EasyAR.dll directory
        /// Mac: libEasyAR.dylib directory
        /// **Note:** *this path is different if you are using Unity3D. It will point to the StreamingAssets folder.*
        /// </para>
        /// <para xml:lang="zh">
        /// assets路径
        /// Android: assets 目录（apk内部）
        /// iOS: 可执行文件目录
        /// Windows: EasyAR.dll所在目录
        /// Mac: libEasyAR.dylib所在目录
        /// **注意:** *如果你在使用Unity3D，这个路径是不同的。在Unity3D中它将会指向StreamingAssets目录。*
        /// </para>
        /// </summary>
        Assets = 1,
        /// <summary>
        /// <para xml:lang="en">
        /// The absolute path (json/image path or video path) or url (video only).
        /// </para>
        /// <para xml:lang="zh">
        /// 绝对路径（json/图片路径或视频文件路径）或url（仅视频文件）
        /// </para>
        /// </summary>
        Absolute = 2,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// Target is the base class for all targets that can be tracked by `ImageTracker`_ or other algorithms inside EasyAR.
    /// </para>
    /// <para xml:lang="zh">
    /// Target是EasyAR里面所有可以被 `ImageTracker`_ 或其它算法跟踪的目标的基类。
    /// </para>
    /// </summary>
    public class Target : RefBase
    {
        internal Target(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new Target(cdata_new, deleter_, retainer_);
        }
        public new Target Clone()
        {
            return (Target)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target id. A target id is a integer number generated at runtime. This id is non-zero and increasing globally.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target id。target id是运行时创建的整型数据，只有在成功的配置之后才是有效（非0）的。这个id是非0且全局递增的。
        /// </para>
        /// </summary>
        public virtual int runtimeID()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_Target_runtimeID(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target uid. A target uid is useful in cloud based algorithms. If no cloud is used, you can set this uid in the json config as a alternative method to distinguish from targets.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target uid。ImageTarget的uid在云识别算法中使用。在没有接入云识别的时候，你可以在json配置中设置这个uid，在自己的代码中作为另一种区分target的方法。
        /// </para>
        /// </summary>
        public virtual string uid()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Target_uid(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the target name. Name is used to distinguish targets in a json file.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取target名字。名字用来在json文件中区分target。
        /// </para>
        /// </summary>
        public virtual string name()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Target_name(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set name. It will erase previously set data or data from cloud.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置target名字。这个操作会覆盖上一次的设置或是服务器返回的数据。
        /// </para>
        /// </summary>
        public virtual void setName(string name)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Target_setName(cdata, Detail.String_to_c(ar, name));
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the meta data set by setMetaData. Or, in a cloud returned target, returns the meta data set in the cloud server.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取setMetaData所设置的meta data。或者在云识别返回的target中，获得服务器所设置的meta data。
        /// </para>
        /// </summary>
        public virtual string meta()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_Target_meta(cdata, out _return_value_);
                return Detail.String_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Set meta data. It will erase previously set data or data from cloud.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置meta data。这个操作会覆盖上一次的设置或是服务器返回的数据。
        /// </para>
        /// </summary>
        public virtual void setMeta(string data)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_Target_setMeta(cdata, Detail.String_to_c(ar, data));
            }
        }
    }

    internal enum TargetStatus
    {
        /// <summary>
        /// <para xml:lang="en">
        /// The target is not being tracking.
        /// </para>
        /// <para xml:lang="zh">
        /// 状态为不在跟踪
        /// </para>
        /// </summary>
        NotTracking = 0,
        /// <summary>
        /// <para xml:lang="en">
        /// The target is being tracking.
        /// </para>
        /// <para xml:lang="zh">
        /// 状态为正在跟踪
        /// </para>
        /// </summary>
        Tracking = 1,
    }

    /// <summary>
    /// <para xml:lang="en">
    /// TargetInstance is the tracked target by trackers.
    /// An TargetInstance contains a raw `Target`_ that is tracked and current status and pose of the `Target`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// TargetInstance是被tracker跟踪到的target。
    /// TargetInstance包括被跟踪上的原始 `Target`_ 以及这个 `Target`_ 当前的状态和姿态。
    /// </para>
    /// </summary>
    internal class TargetInstance : RefBase
    {
        internal TargetInstance(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new TargetInstance(cdata_new, deleter_, retainer_);
        }
        public new TargetInstance Clone()
        {
            return (TargetInstance)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns current status of the tracked target. Usually you can check if the status equals `TargetStatus.Tracked` to determine current status of the target.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前被跟踪target的状态。通常你可以status是否等于 `TargetStatus.Tracked` 来判断当前target的状态。
        /// </para>
        /// </summary>
        public virtual TargetStatus status()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_TargetInstance_status(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets the raw target. It will return the same `Target`_ you loaded into a tracker if it was previously loaded into the tracker.
        /// </para>
        /// <para xml:lang="zh">
        /// 获取原始target。如果曾经被加载到tracker中，会返回与load进 tracker 相同的 `Target`_ 。
        /// </para>
        /// </summary>
        public virtual Target target()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_TargetInstance_target(cdata, out _return_value_);
                return Detail.Object_from_c<Target>(_return_value_, Detail.easyar_Target__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns current pose of the tracked target. Camera coordinate system and target coordinate system are all right-handed. For the camera coordinate system, the origin is the optical center, x-right, y-up, and z in the direction of light going into camera. (The right and up, is right and up in the camera image, which can be different from these in the natural orientation of the device.) The data arrangement is row-major, not like OpenGL&#39;s column-major.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前被跟踪的target相对于Camera的位姿。其中camera坐标系与target坐标系均为右手坐标系。Camera坐标系的原点为相机光心，x轴正方向为右，y轴正方向为上，z轴正方向为光线进入相机的方向。（其中的右和上，指的是相机图像中的右和上，可能和设备自然方向的可能不同。）数据的排列方式为row-major，与OpenGL的column-major相反。
        /// </para>
        /// </summary>
        public virtual Matrix44F pose()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_TargetInstance_pose(cdata);
                return _return_value_;
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// TargetTrackerResult is the base class of `ImageTrackerResult`_ and `ObjectTrackerResult`_ .
    /// </para>
    /// <para xml:lang="zh">
    /// TargetTrackerResult是 `ImageTrackerResult`_ 和 `ObjectTrackerResult`_ 的基类。
    /// </para>
    /// </summary>
    internal class TargetTrackerResult : FrameFilterResult
    {
        internal TargetTrackerResult(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new TargetTrackerResult(cdata_new, deleter_, retainer_);
        }
        public new TargetTrackerResult Clone()
        {
            return (TargetTrackerResult)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Returns the list of `TargetInstance`_ contained in the result.
        /// </para>
        /// <para xml:lang="zh">
        /// 返回当前结果中包含的 `TargetInstance`_ 列表。
        /// </para>
        /// </summary>
        public virtual List<TargetInstance> targetInstances()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_TargetTrackerResult_targetInstances(cdata, out _return_value_);
                return Detail.ListOfTargetInstance_from_c(ar, _return_value_);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Sets the list of `TargetInstance`_ contained in the result.
        /// </para>
        /// <para xml:lang="zh">
        /// 设置当前结果中包含的 `TargetInstance`_ 列表。
        /// </para>
        /// </summary>
        public virtual void setTargetInstances(List<TargetInstance> instances)
        {
            using (var ar = new Detail.AutoRelease())
            {
                Detail.easyar_TargetTrackerResult_setTargetInstances(cdata, Detail.ListOfTargetInstance_to_c(ar, instances));
            }
        }
    }

    /// <summary>
    /// <para xml:lang="en">
    /// TextureId encapsulates a texture object in rendering API.
    /// For OpenGL/OpenGLES, getInt and fromInt shall be used. For Direct3D, getPointer and fromPointer shall be used.
    /// </para>
    /// <para xml:lang="zh">
    /// TextureId封装图形API中的纹理对象。
    /// 其中，OpenGL/OpenGLES应使用getInt和fromInt，Direct3D应使用getPointer和fromPointer。
    /// </para>
    /// </summary>
    internal class TextureId : RefBase
    {
        internal TextureId(IntPtr cdata, Action<IntPtr> deleter, Retainer retainer) : base(cdata, deleter, retainer)
        {
        }
        protected override object CloneObject()
        {
            var cdata_new = IntPtr.Zero;
            if (retainer_ != null) { retainer_(cdata, out cdata_new); }
            return new TextureId(cdata_new, deleter_, retainer_);
        }
        public new TextureId Clone()
        {
            return (TextureId)(CloneObject());
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets ID of an OpenGL/OpenGLES texture object.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得OpenGL/OpenGLES纹理对象的ID。
        /// </para>
        /// </summary>
        public virtual int getInt()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_TextureId_getInt(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Gets pointer of a Direct3D texture object.
        /// </para>
        /// <para xml:lang="zh">
        /// 获得Direct3D纹理对象的指针。
        /// </para>
        /// </summary>
        public virtual IntPtr getPointer()
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = Detail.easyar_TextureId_getPointer(cdata);
                return _return_value_;
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates from ID of an OpenGL/OpenGLES texture object.
        /// </para>
        /// <para xml:lang="zh">
        /// 从OpenGL/OpenGLES纹理对象的ID创建。
        /// </para>
        /// </summary>
        public static TextureId fromInt(int @value)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_TextureId_fromInt(@value, out _return_value_);
                return Detail.Object_from_c<TextureId>(_return_value_, Detail.easyar_TextureId__typeName);
            }
        }
        /// <summary>
        /// <para xml:lang="en">
        /// Creates from pointer of a Direct3D texture object.
        /// </para>
        /// <para xml:lang="zh">
        /// 从Direct3D纹理对象的指针创建。
        /// </para>
        /// </summary>
        public static TextureId fromPointer(IntPtr ptr)
        {
            using (var ar = new Detail.AutoRelease())
            {
                var _return_value_ = default(IntPtr);
                Detail.easyar_TextureId_fromPointer(ptr, out _return_value_);
                return Detail.Object_from_c<TextureId>(_return_value_, Detail.easyar_TextureId__typeName);
            }
        }
    }

}