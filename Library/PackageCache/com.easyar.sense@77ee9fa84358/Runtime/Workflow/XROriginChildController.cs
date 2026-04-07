//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which acts as root of objects under XROrigin need to be controlled in the scene. It will be automatically generated when needed if not manually set in the scene.</para>
    /// <para xml:lang="en">The XROrigin is a virtual node, representing the relative node when the camera moves in a motion tracking system. It will be selected or created automatically. XROrigin of Unity XR framework will be used if it exist.</para>
    /// <para xml:lang="zh">在场景中承担需要受到控制的XROrigin子物体的根节点<see cref="MonoBehaviour"/>。如果场景中没有手动设置这个节点，它将在被需要的时候自动被创建。</para>
    /// <para xml:lang="zh">XROrigin是一个虚拟的节点，它表示在运动跟踪的系统中，camera移动的相对节点，它将自动被选择或创建。如果Unity XR框架的XROrigin存在，它会被选择。</para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ActiveController))]
    public class XROriginChildController : MonoBehaviour
    {
        private ActiveController activeController;

        internal static List<XROriginChildController> AllControllers { get; private set; } = new();

        /// <summary>
        /// <para xml:lang="en"><see cref="GameObject.activeSelf"/> controller.</para>
        /// <para xml:lang="zh"><see cref="GameObject.activeSelf"/>控制器。</para>
        /// </summary>
        public ActiveController ActiveController => activeController;

        private void Awake()
        {
            AllControllers.Add(this);
            activeController = GetComponent<ActiveController>();
            if (!activeController)
            {
                // for backward compatibility
                activeController = gameObject.AddComponent<ActiveController>();
            }
        }

        private void Start()
        {
            activeController.OnStart(ActiveController.Strategy.ActiveAfterFirstTracked);
        }

        private void OnDestroy()
        {
            if (activeController)
            {
                activeController.enabled = false;
            }
            AllControllers.Remove(this);
        }

        internal void OnTracking(GameObject origin, MotionTrackingStatus status)
        {
            activeController.OnTracking(status != MotionTrackingStatus.NotTracking);
            if (origin)
            {
                if (transform.parent != origin.transform)
                {
                    transform.SetParent(origin.transform, false);
                }
                else if (transform.localPosition != Vector3.zero || transform.localRotation != Quaternion.identity)
                {
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                }
            }
        }

        internal void OnEmptyFrame()
        {
            var enabled = ActiveController.enabled;
            ActiveController.enabled = true;
            activeController.OnReset();
            ActiveController.enabled = enabled;
        }

        internal void OnSessionStop()
        {
            OnEmptyFrame();
        }
    }
}
