//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Mega service access source type.</para>
    /// <para xml:lang="zh">Mega 服务访问数据源类型。</para>
    /// </summary>
    public enum MegaServiceAccessSourceType
    {
        /// <summary>
        /// <para xml:lang="en">Use global service config. Use <see cref="EasyARSettings.GlobalMegaBlockLocalizationServiceConfig"/> or <see cref="EasyARSettings.GlobalMegaLandmarkLocalizationServiceConfig"/> according to ServiceType. The global service config can be changed on the inspector after click Unity menu EasyAR -> Sense -> Configuration.</para>
        /// <para xml:lang="zh">使用全局服务器配置。根据ServiceType选择<see cref="EasyARSettings.GlobalMegaBlockLocalizationServiceConfig"/>或<see cref="EasyARSettings.GlobalMegaLandmarkLocalizationServiceConfig"/>。全局配置可以点击Unity菜单EasyAR -> Sense -> Configuration后在属性面板里面进行填写。</para>
        /// </summary>
        GlobalConfig,
        /// <summary>
        /// <para xml:lang="en">Use <see cref="APIKeyAccessData"/> type of access data.</para>
        /// <para xml:lang="zh">使用<see cref="APIKeyAccessData"/>类型的访问数据。</para>
        /// </summary>
        APIKey,
        /// <summary>
        /// <para xml:lang="en">Use <see cref="TokenAccessData"/> type of access data.</para>
        /// <para xml:lang="zh">使用<see cref="TokenAccessData"/>类型的访问数据。</para>
        /// </summary>
        Token,
    }

    /// <summary>
    /// <para xml:lang="en">Mega location input mode.</para>
    /// <para xml:lang="zh">Mega 位置输入模式。</para>
    /// </summary>
    public enum MegaLocationInputMode
    {
        /// <summary>
        /// <para xml:lang="en">Input mode for onsite use. Location data is usually get from device and is put into Mega which is handled by the FrameFilter.</para>
        /// <para xml:lang="zh">在现场使用的情况的输入模式。位置数据通常从设备获取并输入到Mega，通常由FrameFilter内部处理。</para>
        /// </summary>
        Onsite,
        /// <summary>
        /// <para xml:lang="en">Input mode for remote debug use. Location data usually should be simulated to onsite data and put into Mega. Optional.</para>
        /// <para xml:lang="zh">远程使用的情况的输入模式，位置数据需要模拟成现场数据并通过对应接口输入Mega。可选。</para>
        /// </summary>
        Simulator,
        /// <summary>
        /// <para xml:lang="en">Input mode for <see cref="easyar.FramePlayer"/>. This mode is read only.</para>
        /// <para xml:lang="zh">在使用<see cref="easyar.FramePlayer"/>时的输入模式。这个模式是只读的。</para>
        /// </summary>
        FramePlayer,
    }

    /// <summary>
    /// <para xml:lang="en">Equivalent dof <see cref="CameraTransformType"/> of input frame when using Mega.</para>
    /// <para xml:lang="zh">使用Mega功能时输入帧的等价<see cref="CameraTransformType"/>的等价自由度。</para>
    /// </summary>
    public enum MegaInputFrameLevel
    {
        /// <summary>
        /// <para xml:lang="en">0DOF, camera transform without rotation or translation.</para>
        /// <para xml:lang="zh">0DOF，没有任何相机外参的旋转、平移参数。</para>
        /// </summary>
        ZeroDof,
        /// <summary>
        /// <para xml:lang="en">ThreeDofRotOnly, camera transform with 3DOF rotation only.</para>
        /// <para xml:lang="zh">ThreeDofRotOnly，提供部分相机外参，仅包含3自由度的旋转。</para>
        /// </summary>
        ThreeDof,
        /// <summary>
        /// <para xml:lang="en">5DOF, camera transform with rotation and 2D translation (without translation in the gravity direction).</para>
        /// <para xml:lang="zh">5DOF，提供相机外参，包括旋转和2D平移(不包括重力方向平移)。</para>
        /// </summary>
        FiveDof,
        /// <summary>
        /// <para xml:lang="en">6DOF, camera transform with fully 6DOF of both rotation and position.</para>
        /// <para xml:lang="zh">6DOF，提供完整的相机外参，包括旋转和平移。</para>
        /// </summary>
        SixDof,
    }

    /// <summary>
    /// <para xml:lang="en">Mega request time parameters.</para>
    /// <para xml:lang="zh">Mega 请求时间参数。</para>
    /// </summary>
    [Serializable]
    public class MegaRequestTimeParameters
    {
        /// <summary>
        /// <para xml:lang="en">Timeout in milliseconds when communicating with server.</para>
        /// <para xml:lang="zh">与服务器通信的超时时间（毫秒）。</para>
        /// </summary>
        [Tooltip("Timeout in milliseconds when communicating with server.")]
        public int Timeout = 6000;
        /// <summary>
        /// <para xml:lang="en">Expected request interval in milliseconds, with a longer value results a larger overall error.</para>
        /// <para xml:lang="zh">期望的请求间隔时间（毫秒），值越大整体误差越大。</para>
        /// </summary>
        [Tooltip("Expected request interval in milliseconds, with a longer value results a larger overall error.")]
        public int RequestInterval = 1000;
    }

    [Serializable]
    public struct MegaBlockPrior
    {
        public MegaBlockPrior(List<string> blocks, BlockPriorMode mode)
        {
            Blocks = blocks;
            Mode = mode;
        }
        public List<string> Blocks;
        public BlockPriorMode Mode;

        internal BlockPriorResult ToBlockPriorResult() => BlockPriorResult.create(Blocks ?? new(), Mode);
    }
}
