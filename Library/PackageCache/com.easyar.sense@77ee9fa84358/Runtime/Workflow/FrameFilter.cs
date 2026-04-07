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
    /// <para xml:lang="en">A frame filter represents EasyAR feature runs using input frame data.</para>
    /// <para xml:lang="zh">一个frame filter代表使用frame输入数据运行的EasyAR功能。</para>
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class FrameFilter : MonoBehaviour
    {
        internal abstract bool IsAvailable { get; }
        internal abstract int BufferRequirement { get; }

        internal abstract void OnSessionStart(ARSession session);
        internal abstract void OnSessionStop();

        internal virtual string DumpLite() => string.Empty;

        internal interface ICenterTransform
        {
            Optional<Tuple<GameObject, Pose>> TryGetCenter(GameObject center);
            void UpdateTransform(GameObject center, Pose centerPose);
        }

        internal interface IFeedbackFrameSink
        {
            FeedbackFrameSink FeedbackFrameSink();
        }

        internal interface IInputFrameSink
        {
            InputFrameSink InputFrameSink();
        }

        internal interface IOutputFrameSource
        {
            OutputFrameSource OutputFrameSource();
            void OnResult(Optional<FrameFilterResult> frameFilterResult, Optional<MotionInputData> motion);
            void RetrieveSyncResults(Optional<MotionInputData> motion);
        }

        internal interface IHFlipHandler
        {
            void SetHFlip(bool flip);
        }
    }
}
