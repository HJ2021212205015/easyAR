//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

#if EASYAR_DISABLE_ARFOUNDATION && EASYAR_ENABLE_ARFOUNDATION
#undef EASYAR_ENABLE_ARFOUNDATION
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
#if EASYAR_ENABLE_ARFOUNDATION
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#endif

namespace easyar.arfoundation
{
    internal class CameraHandler
    {
#if EASYAR_ENABLE_ARFOUNDATION
        private Action<double, List<ImagePlane>, CameraDescription, bool> onCameraData;
        private ARCameraManager cameraManager;

        private double curTimestamp;
#endif

        private CameraHandler() { }

        public static CameraHandler Create(Camera camera, Action<double, List<ImagePlane>, CameraDescription, bool> onCameraData)
        {
#if EASYAR_ENABLE_ARFOUNDATION
            var cameraManager = camera.GetComponent<ARCameraManager>();
            if (!cameraManager)
            {
                throw new Exception($"{nameof(ARCameraManager)} not found in camera");
            }

            return new CameraHandler()
            {
                onCameraData = onCameraData,
                cameraManager = cameraManager,
            };
#else
            return new();
#endif
        }

        public static bool ValidateCamera(Camera camera)
        {
#if EASYAR_ENABLE_ARFOUNDATION
            var cameraManager = camera.GetComponent<ARCameraManager>();
            if (!cameraManager) { return false; }
            return cameraManager.requestedFacingDirection != CameraFacingDirection.User;
#else
            return false;
#endif
        }

        public static IEnumerator CheckAvailability(Action<bool> callback)
        {
#if EASYAR_ENABLE_ARFOUNDATION
            if (ARSession.state <= ARSessionState.CheckingAvailability)
            {
                yield return ARSession.CheckAvailability();
            }
            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                // wait AR Foundation to start install
                yield return null;
                yield return null;
                // install not enabled in AR Foundation
                if (ARSession.state == ARSessionState.NeedsInstall)
                {
                    callback?.Invoke(false);
                    yield break;
                }
            }
            while (ARSession.state == ARSessionState.Installing)
            {
                yield return null;
            }

            callback?.Invoke(ARSession.state >= ARSessionState.Ready);
#else
            callback?.Invoke(false);
            yield break;
#endif
        }

        public void Start()
        {
#if EASYAR_ENABLE_ARFOUNDATION
            if (!cameraManager) { return; }
            cameraManager.frameReceived += OnCameraFrameReceived;
#endif
        }

        public void Stop()
        {
#if EASYAR_ENABLE_ARFOUNDATION
            if (!cameraManager) { return; }
            cameraManager.frameReceived -= OnCameraFrameReceived;
#endif
        }

        public IEnumerator ChooseConfig(int minSize)
        {
#if EASYAR_ENABLE_ARFOUNDATION
            yield return new WaitUntil(() => cameraManager && cameraManager.currentConfiguration.HasValue);

            while (true)
            {
                if (!cameraManager) { yield break; }
                if (cameraManager.currentConfiguration.HasValue && cameraManager.currentConfiguration.Value.width >= minSize && cameraManager.currentConfiguration.Value.height >= minSize) { yield break; }

                using (var configurations = cameraManager.GetConfigurations(Allocator.Temp))
                {
                    if (configurations.IsCreated && configurations.Length > 0)
                    {
                        foreach (var config in configurations)
                        {
                            if (config.width >= minSize || config.height >= minSize)
                            {
                                try
                                {
                                    cameraManager.currentConfiguration = config;
                                    Debug.LogWarning($"Change camera configuration to {config.width}x{config.height} {config.framerate}fps");
                                }
                                catch (Exception) { }
                                break;
                            }
                        }
                        break;
                    }
                }
                yield return null;
            }
#else
            yield break;
#endif
        }

#if EASYAR_ENABLE_ARFOUNDATION
        private unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            if (!cameraManager) { return; }
            if (ARSession.state <= ARSessionState.Ready) { return; }

            if (cameraManager.currentFacingDirection == CameraFacingDirection.User)
            {
                throw new InvalidOperationException("AR Foundation CameraFacingDirection.User is not supported in EasyAR");
            }
            if (!cameraManager.TryGetIntrinsics(out var intrinsics)) { return; }
            if (!cameraManager.TryAcquireLatestCpuImage(out var cameraImage)) { return; }

            using (cameraImage)
            {
                var timestamp = eventArgs.timestampNs ?? (long)(cameraImage.timestamp * 1e9);
                if (timestamp == curTimestamp) { return; }

                curTimestamp = timestamp;

                var planeCount = cameraImage.format switch
                {
                    XRCpuImage.Format.OneComponent8 => 1,
                    XRCpuImage.Format.IosYpCbCr420_8BiPlanarFullRange => 2,
                    XRCpuImage.Format.AndroidYuv420_888 => 3,
                    _ => throw new InvalidOperationException($"Unsupported format: {cameraImage.format}"),
                };
                if (cameraImage.planeCount < planeCount)
                {
                    throw new InvalidOperationException($"Insufficient planeCount for {cameraImage.format}: {cameraImage.planeCount}");
                }

                var planeY = cameraImage.GetPlane(0);
                var planes = new List<ImagePlane>
                {
                    new ImagePlane
                    {
                        rowStride = planeY.rowStride,
                        pixelStride = planeY.pixelStride,
                        data = planeY.data
                    }
                };
                if (planeCount >= 2)
                {
                    var planeU = cameraImage.GetPlane(1);
                    planes.Add(new ImagePlane
                    {
                        rowStride = planeU.rowStride,
                        pixelStride = planeU.pixelStride,
                        data = planeU.data
                    });
                }
                if (planeCount >= 3)
                {
                    var planeV = cameraImage.GetPlane(2);
                    planes.Add(new ImagePlane
                    {
                        rowStride = planeV.rowStride,
                        pixelStride = planeV.pixelStride,
                        data = planeV.data
                    });
                }

                onCameraData?.Invoke(timestamp, planes, new CameraDescription
                {
                    size = new Vector2Int(cameraImage.width, cameraImage.height),
                    focalLength = intrinsics.focalLength,
                    principalPoint = intrinsics.principalPoint,
                    frameRate = cameraManager.currentConfiguration?.framerate ?? 30
                }, ARSession.state == ARSessionState.SessionTracking);
            }
        }
#endif
    }

    internal struct ImagePlane
    {
        public int rowStride { get; internal set; }
        public int pixelStride { get; internal set; }
        public NativeArray<byte> data { get; internal set; }
    }

    internal struct CameraDescription
    {
        public Vector2Int size { get; internal set; }
        public Vector2 focalLength { get; internal set; }
        public Vector2 principalPoint { get; internal set; }
        public int frameRate { get; internal set; }
    }
}
