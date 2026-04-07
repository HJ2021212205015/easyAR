//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Linq;
using UnityEngine;

namespace easyar
{
    internal class XROriginCache
    {
        private static Data data;

        private enum OriginType
        {
            Default,
            XROrigin,
            External,
        }

        public static GameObject DefaultOrigin(bool tryCreate) => Get(OriginType.Default)?.Origin || (tryCreate && SetupOrigin(OriginType.Default)) ? Get(OriginType.Default)?.Origin : null;
        public static Camera DefaultCamera(bool tryCreate) => DefaultOrigin(tryCreate) && Get(OriginType.Default)?.Camera ? Get(OriginType.Default)?.Camera : null;

        public static GameObject XROrigin(bool tryCreate) => Get(OriginType.XROrigin)?.Origin || (tryCreate && SetupOrigin(OriginType.XROrigin)) ? Get(OriginType.XROrigin)?.Origin : null;
        public static Camera XRCamera(bool tryCreate) => XROrigin(tryCreate) && Get(OriginType.XROrigin)?.Camera ? Get(OriginType.XROrigin)?.Camera : null;

        public static GameObject ExternalOrigin(GameObject o, bool setup) => (Get(OriginType.External)?.Origin && Get(OriginType.External)?.Origin == o) || (setup && SetupExternalOrigin(o)) ? Get(OriginType.External)?.Origin : null;

        public static XROriginChildController CurrentOriginChild => data?.OriginChild;

        private static Data Get(OriginType type) => data != null && data.OriginType == type ? data : null;

        private static bool SetupOrigin(OriginType type)
        {
#if !EASYAR_ENABLE_XRORIGIN
            if (type == OriginType.XROrigin)
            {
                throw new InvalidOperationException("missing package: com.unity.xr.core-utils >= 2.0.0");
            }
#endif
            ValidateCache(type);
            if (data != null && data.Origin) { return true; }
            var (o, c) = GetOrCreateOrigin(type);
            if (!o) { return false; }
            var oc = SetupOrigin(o);
            if (!oc) { return false; }
            data = new Data
            {
                OriginType = type,
                Origin = o,
                Camera = c,
                OriginChild = oc,
            };
            return true;
        }

        private static bool SetupExternalOrigin(GameObject o)
        {
            ValidateCache(OriginType.External);
            if (data != null && data.Origin == o) { return true; }
            data = null;
            var oc = SetupOrigin(o);
            if (!oc) { return false; }
            data = new Data
            {
                OriginType = OriginType.XROrigin,
                Origin = o,
                OriginChild = oc,
            };
            return true;
        }

        private static void ValidateCache(OriginType type)
        {
            if (data != null && data.OriginType == type) { return; }
            data = null;
        }

        private static XROriginChildController SetupOrigin(GameObject origin)
        {
            XROriginChildController originChild = null;
            if (!origin) { return originChild; }
            if (origin.GetComponent<XROriginChildController>())
            {
                UnityEngine.Object.Destroy(origin.GetComponent<XROriginChildController>());
            }
            foreach (var child in origin.GetComponentsInChildren<XROriginChildController>(true))
            {
                child.transform.SetParent(origin.transform, false);
                if (!originChild)
                {
                    Debug.Log($"Use origin child: {child.gameObject} ({child.gameObject.GetInstanceID()})");
                    originChild = child;
                }
            }
            if (!originChild)
            {
                var child = ARSessionFactory.AddOriginChild(origin);
                if (child)
                {
                    Debug.Log($"Create origin child: {child} ({child.GetInstanceID()})");
                    originChild = child.GetComponent<XROriginChildController>();
                }
            }
            return originChild;
        }

        private static (GameObject, Camera) GetOrCreateOrigin(OriginType originType)
        {
#if EASYAR_ENABLE_XRORIGIN
            var xrOrigin = ObjectUtil.FindAnyObjectByType<Unity.XR.CoreUtils.XROrigin>();
            if (xrOrigin)
            {
                if (!xrOrigin.Camera)
                {
                    throw new InvalidOperationException($"Missing Camera in XROrigin: {xrOrigin} ({xrOrigin.gameObject.GetInstanceID()})");
                }
                if (xrOrigin.RequestedTrackingOriginMode != Unity.XR.CoreUtils.XROrigin.TrackingOriginMode.Device || xrOrigin.CameraYOffset != 0)
                {
                    xrOrigin.RequestedTrackingOriginMode = Unity.XR.CoreUtils.XROrigin.TrackingOriginMode.Device;
                    xrOrigin.CameraYOffset = 0;
                    Debug.LogWarning($"force XROrigin (RequestedTrackingOriginMode = {xrOrigin.RequestedTrackingOriginMode}, CameraYOffset = {xrOrigin.CameraYOffset}) when using EasyAR");
                }
                Debug.Log($"Use origin: {xrOrigin} ({xrOrigin.gameObject.GetInstanceID()})");
                return (xrOrigin.gameObject, xrOrigin.Camera);
            }
#endif
            if (originType == OriginType.XROrigin) { return (null, null); }

            var camera = ObjectUtil.FindObjectsByType<Camera>().Where(c => c.transform.parent && c.transform.parent.name == "Camera Offset" && c.transform.parent.parent).FirstOrDefault();
            if (camera)
            {
                var o = camera.transform.parent.parent.gameObject;
                Debug.Log($"Use origin: {o} ({o.GetInstanceID()})");
                return (o, camera);
            }

            var originChild = XROriginChildController.AllControllers.Where(c => c && c.transform.parent && c.transform.parent.gameObject && c.transform.parent.gameObject.activeInHierarchy).FirstOrDefault();
            if (originChild)
            {
                var o = originChild.transform.parent.gameObject;
                Debug.Log($"Use origin: {o} ({o.GetInstanceID()})");
                return (o, null);
            }
            originChild = XROriginChildController.AllControllers.FirstOrDefault();

            var origin = ARSessionFactory.CreateOrigin(originChild);
            Debug.Log($"Create origin: {origin} ({origin.GetInstanceID()})");
            return (origin, null);
        }

        private class Data
        {
            public OriginType OriginType;
            public GameObject Origin;
            public Camera Camera;
            public XROriginChildController OriginChild;
        }
    }
}
