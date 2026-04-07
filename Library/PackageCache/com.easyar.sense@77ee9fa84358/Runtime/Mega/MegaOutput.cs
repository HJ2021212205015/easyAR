//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

#if EASYAR_ENABLE_MEGA
using EasyAR.Mega.Scene;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">The response of Mega localization request.</para>
    /// <para xml:lang="zh">Mega定位请求的响应。</para>
    /// </summary>
    public class MegaLocalizationResponse
    {
        /// <summary>
        /// <para xml:lang="en"><see cref="InputFrame"/> timestamp when sending the request.</para>
        /// <para xml:lang="zh">发送请求时的<see cref="InputFrame"/>时间戳。</para>
        /// </summary>
        public double Timestamp;
        /// <summary>
        /// <para xml:lang="en">Localization status.</para>
        /// <para xml:lang="zh">定位状态。</para>
        /// </summary>
        public MegaTrackerLocalizationStatus Status;
        /// <summary>
        /// <para xml:lang="en">The spot version ID. Only available with <see cref="MegaApiType.Landmark"/>.</para>
        /// <para xml:lang="zh">地点版本ID。仅当开启<see cref="MegaApiType.Landmark"/>时可用。</para>
        /// </summary>
        public Optional<string> SpotVersionId;
#if EASYAR_ENABLE_MEGA
        /// <summary>
        /// <para xml:lang="en">Localized <see cref="BlockController"/>. There is only one block in the list if localzied. Objects transform and status in the scene does not change with event data in the same time the event is triggered.</para>
        /// <para xml:lang="zh">定位到的<see cref="BlockController"/>。定位到block时，列表中只会有一个数据。事件发生时场景中物体的位置和状态与事件中的数据无对应关系。</para>
        /// </summary>
        public List<BlockController> Blocks = new List<BlockController>();
#endif
        /// <summary>
        /// <para xml:lang="en">The duration in seconds for server response.</para>
        /// <para xml:lang="zh">服务器响应耗时（秒）。</para>
        /// </summary>
        public Optional<double> ServerResponseDuration;
        /// <summary>
        /// <para xml:lang="en">The duration in seconds for server internal calculation.</para>
        /// <para xml:lang="zh">服务器内部计算耗时（秒）。</para>
        /// </summary>
        public Optional<double> ServerCalculationDuration;
        /// <summary>
        /// <para xml:lang="en">Error message.</para>
        /// <para xml:lang="zh">错误信息。</para>
        /// </summary>
        public Optional<string> ErrorMessage;
        public string ExtraInfo;

        internal MegaLocalizationResponse() { }

        internal MegaLocalizationResponse(string error)
        {
            Timestamp = Time.realtimeSinceStartup;
            Status = MegaTrackerLocalizationStatus.UnknownError;
            ErrorMessage = $"Skip underlying request: {error}";
        }
    }

    /// <summary>
    /// <para xml:lang="en">The response of Mega Landmark filter request.</para>
    /// <para xml:lang="zh">Mega Landmark filter请求的响应。</para>
    /// </summary>
    public class MegaLandmarkFilterResponse
    {
        /// <summary>
        /// <para xml:lang="en">Timestamp when sending the request.</para>
        /// <para xml:lang="zh">发送请求时的时间戳。</para>
        /// </summary>
        public double Timestamp;
        /// <summary>
        /// <para xml:lang="en">Filter status.</para>
        /// <para xml:lang="zh">Filter 状态。</para>
        /// </summary>
        public MegaLandmarkFilterStatus Status;
        /// <summary>
        /// <para xml:lang="en">The spot version ID.</para>
        /// <para xml:lang="zh">地点版本ID。</para>
        /// </summary>
        public Optional<string> SpotVersionId;
        /// <summary>
        /// <para xml:lang="en">Error message.</para>
        /// <para xml:lang="zh">错误信息。</para>
        /// </summary>
        public Optional<string> ErrorMessage;

        internal MegaLandmarkFilterResponse() { }

        internal MegaLandmarkFilterResponse(string error)
        {
            Timestamp = Time.realtimeSinceStartupAsDouble;
            Status = MegaLandmarkFilterStatus.UnknownError;
            ErrorMessage = $"Skip underlying request: {error}";
        }
    }
}
