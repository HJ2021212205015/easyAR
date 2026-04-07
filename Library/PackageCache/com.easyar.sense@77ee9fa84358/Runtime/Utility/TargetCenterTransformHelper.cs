//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace easyar
{
    internal class TargetCenterTransformHelper : FrameFilter.ICenterTransform
    {
        public Optional<List<Tuple<MonoBehaviour, Pose>>> Results { get; set; }
        public bool HorizontalFlip { get; set; }

        public Optional<Tuple<GameObject, Pose>> TryGetCenter(GameObject center)
        {
            if (Results.OnNone) { return null; }

            if (center)
            {
                foreach (var result in Results.Value.Where(r => r.Item1.gameObject == center))
                {
                    return Tuple.Create(center, result.Item2);
                }
            }
            else
            {
                foreach (var result in Results.Value)
                {
                    return Tuple.Create(result.Item1.gameObject, result.Item2);
                }
            }
            return null;
        }

        public void UpdateTransform(GameObject center, Pose centerPose)
        {
            if (Results.OnNone) { return; }
            foreach (var result in Results.Value.Where(r => r.Item1.gameObject != center))
            {
                var targetToCamera = result.Item2;
                var cameraToWorld = centerPose.Inverse()
                    .FlipX(HorizontalFlip)
                    .GetTransformedBy(new Pose(center.transform.position, center.transform.rotation));
                var targetToWorld = targetToCamera
                    .FlipX(HorizontalFlip)
                    .GetTransformedBy(cameraToWorld);

                result.Item1.transform.position = targetToWorld.position;
                result.Item1.transform.rotation = targetToWorld.rotation;
            }
        }
    }
}
