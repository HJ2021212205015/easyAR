//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Display for general camera device. You can emulate screen rotation on PC or in Unity Editor to satisfy some special screen setups.</para>
    /// <para xml:lang="zh">常规相机的显示。你可以在Unity编辑器或PC上模拟屏幕旋转，以便配合特殊的屏幕摆放需求。</para>
    /// </summary>
    [DisallowMultipleComponent]
    public class CameraDeviceDisplay : MonoBehaviour, IDisplay
    {
        /// <summary>
        /// <para xml:lang="en">Display mode.</para>
        /// <para xml:lang="zh">显示模式。</para>
        /// </summary>
        public DisplayMode Mode;
        private readonly DisplayEmulator emulator = new DisplayEmulator();
        [SerializeField, HideInInspector]
        private int rotation;

        /// <summary>
        /// <para xml:lang="en">Display mode.</para>
        /// <para xml:lang="zh">显示模式。</para>
        /// </summary>
        public enum DisplayMode
        {
            /// <summary>
            /// <para xml:lang="en">System default mode.</para>
            /// <para xml:lang="zh">系统默认模式。</para>
            /// </summary>
            SystemDefault,
            /// <summary>
            /// <para xml:lang="en">Emulator mode.</para>
            /// <para xml:lang="zh">模拟器模式。</para>
            /// </summary>
            Emulator,
        }

        public int Rotation { get => Mode == DisplayMode.SystemDefault ? EasyARController.DefaultSystemDisplay.Rotation : emulator.Rotation; }

        private void Awake()
        {
            emulator.EmulateRotation(rotation);
        }

        /// <summary>
        /// <para xml:lang="en">Emulate screen rotation to <paramref name="mode"/>.</para>
        /// <para xml:lang="zh">模拟屏幕旋转为<paramref name="mode"/>。</para>
        /// </summary>
        public void EmulateRotation(DisplayEmulator.RotationMode mode)
        {
            emulator.EmulateRotation(mode);
            rotation = emulator.Rotation;
        }
    }
}
