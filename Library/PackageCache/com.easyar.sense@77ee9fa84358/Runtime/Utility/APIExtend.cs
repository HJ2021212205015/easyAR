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
    /// <para xml:lang="en">Axis system type.</para>
    /// <para xml:lang="zh">坐标轴系统类型。</para>
    /// </summary>
    public enum AxisSystemType
    {
        /// <summary>
        /// <para xml:lang="en">Unity axis system.</para>
        /// <para xml:lang="zh">Unity坐标轴系统。</para>
        /// </summary>
        Unity,
        /// <summary>
        /// <para xml:lang="en">EasyAR axis system.</para>
        /// <para xml:lang="zh">EasyAR坐标轴系统。</para>
        /// </summary>
        EasyAR,
    }

    /// <summary>
    /// <para xml:lang="en">Extend EasyAR Sense API and Unity API for convenience to do operations like data conversion.</para>
    /// <para xml:lang="zh">扩展EasyAR Sense API 及 Unity API，为数据转换等操作提供便利。</para>
    /// </summary>
    public static class APIExtend
    {
        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Matrix44F"/> to <see cref="Matrix4x4"/>.</para>
        /// <para xml:lang="zh">将<see cref="Matrix44F"/>转成<see cref="Matrix4x4"/>。</para>
        /// </summary>
        public static Matrix4x4 ToUnityMatrix(this Matrix44F matrix44F)
        {
            var matrix4x4 = new Matrix4x4();
            matrix4x4.SetRow(0, new Vector4(matrix44F.data_0, matrix44F.data_1, matrix44F.data_2, matrix44F.data_3));
            matrix4x4.SetRow(1, new Vector4(matrix44F.data_4, matrix44F.data_5, matrix44F.data_6, matrix44F.data_7));
            matrix4x4.SetRow(2, new Vector4(matrix44F.data_8, matrix44F.data_9, matrix44F.data_10, matrix44F.data_11));
            matrix4x4.SetRow(3, new Vector4(matrix44F.data_12, matrix44F.data_13, matrix44F.data_14, matrix44F.data_15));
            return matrix4x4;
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Matrix4x4"/> to <see cref="Matrix44F"/>.</para>
        /// <para xml:lang="zh">将<see cref="Matrix4x4"/>转成<see cref="Matrix44F"/>。</para>
        /// </summary>
        public static Matrix44F ToEasyARMatrix(this Matrix4x4 matrix4x4)
        {
            return new Matrix44F(
                matrix4x4[0, 0], matrix4x4[0, 1], matrix4x4[0, 2], matrix4x4[0, 3],
                matrix4x4[1, 0], matrix4x4[1, 1], matrix4x4[1, 2], matrix4x4[1, 3],
                matrix4x4[2, 0], matrix4x4[2, 1], matrix4x4[2, 2], matrix4x4[2, 3],
                matrix4x4[3, 0], matrix4x4[3, 1], matrix4x4[3, 2], matrix4x4[3, 3]
                );
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Matrix44F"/> to <see cref="Pose"/> and transform to Unity coordinate system. <paramref name="pose"/> must represent a <see cref="Pose"/>.</para>
        /// <para xml:lang="zh">将<see cref="Matrix44F"/>转成<see cref="Pose"/>并同时转换到Unity坐标系。<paramref name="pose"/>必须表示一个<see cref="Pose"/>。</para>
        /// </summary>
        public static Pose ToUnityPose(this Matrix44F pose)
        {
            float m00 = pose.data_0; float m01 = pose.data_1; float m02 = pose.data_2;
            float m10 = pose.data_4; float m11 = pose.data_5; float m12 = pose.data_6;
            float m20 = pose.data_8; float m21 = pose.data_9; float m22 = pose.data_10;

            float trace = m00 + m11 + m22;
            Quaternion q;
            if (trace > 0f)
            {
                float s = Mathf.Sqrt(trace + 1f) * 0.5f;
                float inv = 0.25f / s;
                q = new Quaternion((m21 - m12) * inv, (m02 - m20) * inv, (m10 - m01) * inv, s);
            }
            else if (m00 > m11 && m00 > m22)
            {
                float s = Mathf.Sqrt(1f + m00 - m11 - m22) * 0.5f;
                float inv = 0.25f / s;
                q = new Quaternion(s, (m01 + m10) * inv, (m02 + m20) * inv, (m21 - m12) * inv);
            }
            else if (m11 > m22)
            {
                float s = Mathf.Sqrt(1f - m00 + m11 - m22) * 0.5f;
                float inv = 0.25f / s;
                q = new Quaternion((m01 + m10) * inv, s, (m12 + m21) * inv, (m02 - m20) * inv);
            }
            else
            {
                float s = Mathf.Sqrt(1f - m00 - m11 + m22) * 0.5f;
                float inv = 0.25f / s;
                q = new Quaternion((m02 + m20) * inv, (m12 + m21) * inv, s, (m10 - m01) * inv);
            }

            return new Pose(new Vector3 { x = pose.data_3, y = pose.data_7, z = pose.data_11 }, q).ToUnityAxisSystem();
        }

        /// <summary>
        /// <para xml:lang="en">Transform from Unity axis system to EasyAR axis system.</para>
        /// <para xml:lang="zh">从Unity坐标轴系统转换到EasyAR坐标轴系统。</para>
        /// </summary>
        public static Pose ToEasyARAxisSystem(this Pose pose)
        {
            var p = pose.position;
            var q = pose.rotation;
            return new Pose(new Vector3 { x = p.x, y = p.y, z = -p.z }, new Quaternion { w = q.w, x = -q.x, y = -q.y, z = q.z });
        }

        /// <summary>
        /// <para xml:lang="en">Transform from EasyAR axis system to Unity axis system.</para>
        /// <para xml:lang="zh">从EasyAR坐标轴系统转换到Unity坐标轴系统。</para>
        /// </summary>
        public static Pose ToUnityAxisSystem(this Pose pose)
        {
            var p = pose.position;
            var q = pose.rotation;
            return new Pose(new Vector3 { x = p.x, y = p.y, z = -p.z }, new Quaternion { w = q.w, x = -q.x, y = -q.y, z = q.z });
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Pose"/> to <see cref="Matrix44F"/> and transform to EasyAR coordinate system.</para>
        /// <para xml:lang="zh">将<see cref="Pose"/>转成<see cref="Matrix44F"/>并同时转换到EasyAR坐标系。</para>
        /// </summary>
        public static Matrix44F ToEasyARPose(this Pose pose)
        {
            var ePose = pose.ToEasyARAxisSystem();
            return Matrix4x4.TRS(ePose.position, ePose.rotation, Vector3.one).ToEasyARMatrix();
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Vector2"/> to <see cref="Vec2F"/>.</para>
        /// <para xml:lang="zh">将<see cref="Vector2"/>转成<see cref="Vec2F"/>。</para>
        /// </summary>
        public static Vec2F ToEasyARVector(this Vector2 vec2)
        {
            return new Vec2F(vec2.x, vec2.y);
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Vector3"/> to <see cref="Vec3F"/>.</para>
        /// <para xml:lang="zh">将<see cref="Vector3"/>转成<see cref="Vec3F"/>。</para>
        /// </summary>
        public static Vec3F ToEasyARVector(this Vector3 vec3)
        {
            return new Vec3F(vec3.x, vec3.y, vec3.z);
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Quaternion"/> to <see cref="Vec4F"/> represented Quaternion.</para>
        /// <para xml:lang="zh">将<see cref="Quaternion"/>转成<see cref="Vec4F"/>表达的四元数。</para>
        /// </summary>
        public static Vec4F ToEasyARQuaternion(this Quaternion quat)
        {
            return new Vec4F(quat.w, quat.x, quat.y, quat.z);
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Vector2Int"/> to <see cref="Vec2I"/>.</para>
        /// <para xml:lang="zh">将<see cref="Vector2Int"/>转成<see cref="Vec2I"/>。</para>
        /// </summary>
        public static Vec2I ToEasyARVector(this Vector2Int vec2)
        {
            return new Vec2I(vec2.x, vec2.y);
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Vec2F"/> to <see cref="Vector2"/>.</para>
        /// <para xml:lang="zh">将<see cref="Vec2F"/>转成<see cref="Vector2"/>。</para>
        /// </summary>
        public static Vector2 ToUnityVector(this Vec2F vec2)
        {
            return new Vector2(vec2.data_0, vec2.data_1);
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Vec3F"/> to <see cref="Vector3"/>.</para>
        /// <para xml:lang="zh">将<see cref="Vec3F"/>转成<see cref="Vector3"/>。</para>
        /// </summary>
        public static Vector3 ToUnityVector(this Vec3F vec3)
        {
            return new Vector3(vec3.data_0, vec3.data_1, vec3.data_2);
        }

        /// <summary>
        /// <para xml:lang="en">Convert <see cref="Vec2I"/> to <see cref="Vector2Int"/>.</para>
        /// <para xml:lang="zh">将<see cref="Vec2I"/>转成<see cref="Vector2Int"/>。</para>
        /// </summary>
        public static Vector2Int ToUnityVector(this Vec2I vec2)
        {
            return new Vector2Int(vec2.data_0, vec2.data_1);
        }

        internal static Pose Inverse(this Pose pose)
        {
            return new Pose
            {
                position = Quaternion.Inverse(pose.rotation) * (-pose.position),
                rotation = Quaternion.Inverse(pose.rotation)
            };
        }

        internal static Pose FlipX(this Pose pose, bool flip)
        {
            if (!flip)
            {
                return pose;
            }
            return new Pose
            {
                position = new Vector3(-pose.position.x, pose.position.y, pose.position.z),
                rotation = new Quaternion(pose.rotation.x, -pose.rotation.y, -pose.rotation.z, pose.rotation.w)
            };
        }
    }

    internal static class InputFrameHelper
    {
        public static Optional<InputFrame> TryCreateSixDof(double timestamp, Image image, CameraParameters cameraParameters, Pose cameraTransform, MotionTrackingStatus trackingStatus) => TryCreateSixDof(timestamp, image, cameraParameters, cameraTransform.ToEasyARPose(), trackingStatus);

        public static Optional<InputFrame> TryCreateSixDof(double timestamp, Image image, CameraParameters cameraParameters, Matrix44F cameraTransform, MotionTrackingStatus trackingStatus)
        {
            CameraParameters cameraParametersResized = null;
            try
            {
                var imageSize = new Vector2Int(image.width(), image.height());
                if (cameraParameters.size().ToUnityVector() != imageSize)
                {
                    cameraParametersResized = cameraParameters.getResized(imageSize.ToEasyARVector());
                }
                return InputFrame.tryCreate(image, cameraParametersResized ?? cameraParameters, timestamp, cameraTransform, CameraTransformType.SixDof, trackingStatus);
            }
            finally
            {
                cameraParametersResized?.Dispose();
            }
        }

        public static Optional<InputFrame> TryCreateThreeDofRotOnly(double timestamp, Image image, CameraParameters cameraParameters, Quaternion rotation)
        {
            CameraParameters cameraParametersResized = null;
            try
            {
                var imageSize = new Vector2Int(image.width(), image.height());
                if (cameraParameters.size().ToUnityVector() != imageSize)
                {
                    cameraParametersResized = cameraParameters.getResized(imageSize.ToEasyARVector());
                }
                return InputFrame.tryCreate(image, cameraParametersResized ?? cameraParameters, timestamp, new Pose(Vector3.zero, rotation).ToEasyARPose(), CameraTransformType.ThreeDofRotOnly, MotionTrackingStatus.NotTracking);
            }
            finally
            {
                cameraParametersResized?.Dispose();
            }
        }

        public static InputFrame CreateZeroDof(double timestamp, Image image, CameraParameters cameraParameters)
        {
            CameraParameters cameraParametersResized = null;
            try
            {
                var imageSize = new Vector2Int(image.width(), image.height());
                if (cameraParameters.size().ToUnityVector() != imageSize)
                {
                    cameraParametersResized = cameraParameters.getResized(imageSize.ToEasyARVector());
                }
                return InputFrame.createWithImageAndCameraParametersAndTemporal(image, cameraParametersResized ?? cameraParameters, timestamp);
            }
            finally
            {
                cameraParametersResized?.Dispose();
            }
        }
    }

    internal class BufferPoolWrapper : IDisposable
    {
        private BufferPool bufferPool;
        private int bufferSize;
        private int bufferCapacity;

        public int BufferCapacity
        {
            private get => bufferCapacity;
            set
            {
                bufferCapacity = value;
                if (bufferPool == null || bufferPool.capacity() == value) { return; }
                bufferPool.Dispose();
                bufferPool = new BufferPool(bufferSize, value);
            }
        }

        ~BufferPoolWrapper()
        {
            bufferPool?.Dispose();
        }

        public void Dispose()
        {
            bufferPool?.Dispose();
            GC.SuppressFinalize(this);
        }

        public Optional<Buffer> TryAcquireBuffer(int size)
        {
            if (size <= 0) { return null; }
            if (bufferSize != size)
            {
                bufferSize = size;
                bufferPool?.Dispose();
                bufferPool = new BufferPool(bufferSize, BufferCapacity);
            }
            return bufferPool.tryAcquire();
        }
    }
}
