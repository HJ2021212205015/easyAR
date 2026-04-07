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
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls tracking target in the scene, providing a few extensions in the Unity environment.</para>
    /// <para xml:lang="zh">在场景中控制跟踪目标的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。</para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ActiveController))]
    public abstract class TargetController : MonoBehaviour
    {
        private ActiveController activeController;

        /// <summary>
        /// <para xml:lang="en">Tracking target found event.</para>
        /// <para xml:lang="zh">找到跟踪目标的事件。</para>
        /// </summary>
        public event Action TargetFound;
        /// <summary>
        /// <para xml:lang="en">Tracking target lost event.</para>
        /// <para xml:lang="zh">丢失跟踪目标的事件。</para>
        /// </summary>
        public event Action TargetLost;

        /// <summary>
        /// <para xml:lang="en"><see cref="GameObject.activeSelf"/> controller.</para>
        /// <para xml:lang="en">Set <see cref="MonoBehaviour"/>.enabled to false to turn off control.</para>
        /// <para xml:lang="zh"><see cref="GameObject.activeSelf"/>控制器。</para>
        /// <para xml:lang="zh"><see cref="MonoBehaviour"/>.enabled为false可关闭控制。</para>
        /// </summary>
        public ActiveController ActiveController => activeController;

        /// <summary>
        /// <para xml:lang="en">Is target being tracked.</para>
        /// <para xml:lang="zh">目标是否被跟踪。</para>
        /// </summary>
        public bool IsTracked { get; private set; }

        private protected virtual void Awake()
        {
            activeController = GetComponent<ActiveController>();
            if (!activeController)
            {
                // for backward compatibility
                activeController = gameObject.AddComponent<ActiveController>();
            }
        }

        private protected virtual void Start()
        {
            activeController.OnStart(ActiveController.Strategy.ActiveWhileTracked);
        }

        private protected virtual void OnDestroy()
        {
            if (activeController)
            {
                activeController.enabled = false;
            }
        }

        internal virtual bool OnTracking(bool status)
        {
            activeController.OnTracking(status);
            if (IsTracked != status)
            {
                IsTracked = status;
                if (status)
                {
                    TargetFound?.Invoke();
                }
                else
                {
                    TargetLost?.Invoke();
                }
                return true;
            }
            return false;
        }
    }
}
