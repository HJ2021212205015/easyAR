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
    [DisallowMultipleComponent]
    internal class RenderCameraController : MonoBehaviour
    {
        private Camera targetCamera;
        private Matrix4x4 currentDisplayCompensation = Matrix4x4.identity;
        private CameraParameters cameraParameters;
        private bool projectHFilp;
        private ARSession arSession;
        Optional<float> targetCameraFOV;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            cameraParameters?.Dispose();
            cameraParameters = null;
            if (arSession)
            {
                if (targetCamera && targetCameraFOV.OnSome)
                {
                    targetCamera.fieldOfView = targetCameraFOV.Value;
                    targetCameraFOV = Optional<float>.Empty;
                }
            }
        }

        private void OnDestroy()
        {
            OnSessionStop();
        }

        internal void OnSessionStart(ARSession session)
        {
            arSession = session;
            targetCamera = session.Assembly.Camera;
            targetCameraFOV = targetCamera.fieldOfView;
            if (enabled)
            {
                OnEnable();
            }
        }

        internal void OnSessionStop()
        {
            OnDisable();
            arSession = null;
            targetCamera = null;
        }

        internal void SetProjectHFlip(bool hFlip)
        {
            projectHFilp = hFlip;
        }

        internal void UpdateCamera(Optional<Tuple<OutputFrame, Quaternion>> frame)
        {
            if (frame.OnSome)
            {
                OnFrameChange(frame.Value.Item1, frame.Value.Item2);
            }
            OnFrameUpdate();
        }

        private void OnFrameChange(OutputFrame outputFrame, Quaternion displayCompensation)
        {
            if (!enabled) { return; }
            currentDisplayCompensation = Matrix4x4.Rotate(Quaternion.Inverse(displayCompensation));

            using (var frame = outputFrame.inputFrame())
            {
                cameraParameters?.Dispose();
                if (frame.hasCameraParameters())
                {
                    cameraParameters = frame.cameraParameters();
                }
            }
        }

        private void OnFrameUpdate()
        {
            if (!enabled || cameraParameters == null) { return; }
            var projection = cameraParameters.projection(targetCamera.nearClipPlane, targetCamera.farClipPlane, targetCamera.aspect, arSession.Assembly.Display.Rotation, false, false).ToUnityMatrix();
            projection *= currentDisplayCompensation;
            if (projectHFilp)
            {
                var translateMatrix = Matrix4x4.identity;
                translateMatrix.m00 = -1;
                projection = translateMatrix * projection;
            }
            targetCamera.projectionMatrix = projection;
            targetCamera.fieldOfView = Mathf.Atan(1 / projection.m11) * 2 * Mathf.Rad2Deg;
        }
    }
}
