//================================================================================================================================
//
//  Copyright (c) 2020-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which manages <see cref="GameObject.activeSelf"/>.</para>
    /// <para xml:lang="en">Default strategy is <see cref="Strategy.ActiveWhileTracked"/> for <see cref="TargetController"/>, and <see cref="Strategy.ActiveAfterFirstTracked"/> for <see cref="XROriginChildController"/>.</para>
    /// <para xml:lang="en">Use <see cref="OverrideStrategy"/> to override default strategy. Set <see cref="MonoBehaviour"/>.enabled to false to turn off control.</para>
    /// <para xml:lang="zh">管理<see cref="GameObject.activeSelf"/>的 <see cref="MonoBehaviour"/> 。</para>
    /// <para xml:lang="zh">默认策略如下：<see cref="TargetController"/>默认使用<see cref="Strategy.ActiveWhileTracked"/>，<see cref="XROriginChildController"/>默认使用<see cref="Strategy.ActiveAfterFirstTracked"/>。</para>
    /// <para xml:lang="zh">可以使用<see cref="OverrideStrategy"/>来覆盖默认策略。设置<see cref="MonoBehaviour"/>.enabled为false可关闭控制。</para>
    /// </summary>
    [DisallowMultipleComponent]
    public class ActiveController : MonoBehaviour
    {
        [HideInInspector, SerializeField]
        private bool hasOverrideStrategy;
        [HideInInspector, SerializeField]
        Strategy overrideStrategy;
        private Strategy defaultStrategy;
        private bool isTracked;
        private bool hasEverTracked;
        private bool hasEverStarted;

        /// <summary>
        /// <para xml:lang="en">The control strategy for <see cref="GameObject.activeSelf"/>.</para>
        /// <para xml:lang="zh"><see cref="GameObject.activeSelf"/>的控制策略。</para>
        /// </summary>
        public enum Strategy
        {
            /// <summary>
            /// <para xml:lang="en">Active is false when not tracked, and true when tracked..</para>
            /// <para xml:lang="zh">当没有被跟踪时Active为false，当被跟踪时Active为true。</para>
            /// </summary>
            ActiveWhileTracked,
            /// <summary>
            /// <para xml:lang="en">Active is false before being tracked for the first time, and true afterwards.</para>
            /// <para xml:lang="zh">在第一次被跟踪到之前Active为false，之后为true。</para>
            /// </summary>
            ActiveAfterFirstTracked,
        }

        /// <summary>
        /// <para xml:lang="en">Override strategy, which can override default behavior.</para>
        /// <para xml:lang="en">Empty value indicates using the default strategy, which is related to other components on the GameObject. If set, this will override the default strategy.</para>
        /// <para xml:lang="zh">覆盖策略，可用于覆盖默认行为。</para>
        /// <para xml:lang="zh">空值表达使用默认策略，策略模式与物体上其它组件相关。有值会覆盖默认策略。</para>
        /// </summary>
        public Optional<Strategy> OverrideStrategy
        {
            get => hasOverrideStrategy ? overrideStrategy : Optional<Strategy>.Empty;
            set
            {
                hasOverrideStrategy = value.OnSome;
                if (value.OnSome) { overrideStrategy = value.Value; }
                if (hasEverStarted)
                {
                    OnTracking(isTracked);
                }
            }
        }

        /// <summary>
        /// <para xml:lang="en">Enable/Disable active control. When disabled, <see cref="GameObject.activeSelf"/> will not be changed by EasyAR.</para>
        /// <para xml:lang="zh"><see cref="ARSession"/>运行时启用/停用active控制。禁用时，EasyAR将不再修改<see cref="GameObject.activeSelf"/>。</para>
        /// </summary>
        private void OnEnable()
        {
            if (hasEverStarted)
            {
                OnTracking(isTracked);
            }
        }

        internal void OnStart(Strategy defaultStrategy)
        {
            this.defaultStrategy = defaultStrategy;
            hasEverStarted = true;
            ControlActive();
        }

        internal void OnTracking(bool status)
        {
            isTracked = status;
            if (!hasEverTracked && status) { hasEverTracked = true; }
            ControlActive();
        }

        internal void OnReset()
        {
            hasEverTracked = false;
            isTracked = false;
            ControlActive();
        }

        private void ControlActive()
        {
            if (!enabled) { return; }
            var active = OverrideStrategy.ValueOrDefault(defaultStrategy) switch
            {
                Strategy.ActiveWhileTracked => isTracked,
                Strategy.ActiveAfterFirstTracked => hasEverTracked,
                _ => throw new ArgumentOutOfRangeException(),
            };
            if (gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}
