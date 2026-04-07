//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Linq;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">EasyAR Sense Unity Plugin settings.</para>
    /// <para xml:lang="zh">EasyAR Sense Unity Plugin的配置信息。</para>
    /// </summary>
    public class EasyARSettings : ScriptableObject
    {
        /// <summary>
        /// <para xml:lang="en">Initialize EasyAR Sense on startup. EasyAR initialize does not result noticeable extra resource usages, so usually you can keep this option on.</para>
        /// <para xml:lang="zh">在启动时初始化EasyAR。EasyAR的初始化不会造成额外的明显资源消耗，因此通常可以保持这个选项打开。</para>
        /// </summary>
        [Tooltip("Initialize EasyAR Sense on startup. EasyAR initialize does not result extra resource usages, so usually you can keep this option on.")]
        public bool InitializeOnStartup = true;
        /// <summary>
        /// <para xml:lang="en">EasyAR Sense License Key. Used for validation of EasyAR Sense functions. Please visit https://www.easyar.com for more details.</para>
        /// <para xml:lang="zh">EasyAR Sense License Key。用于验证EasyAR Sense内部各种功能是否可用。详见 https://www.easyar.cn 。</para>
        /// </summary>
        [Tooltip("EasyAR Sense License Key. Used for validation of EasyAR Sense functions. Please visit https://www.easyar.com for more details.")]
        [SerializeField]
        [TextArea(1, 4)]
        public string LicenseKey = string.Empty;
        /// <summary>
        /// <para xml:lang="en">Verify license when build Unity Project. When this option is on, license key will be verified during Unity build process and build will fail if license key is invalid on the platform or the license key does not contain package name / bundle identifier setup in the Unity Player Settings. Turn off this option if you setup license in a difference place, or if you need to change package name or bundle identifier after Unity build process.</para>
        /// <para xml:lang="zh">在构建Unity工程时验证license Key。当这个选项打开时，Unity项目构建过程会验证license Key，如果license在构建平台上无效或不包含Unity Player Settings中设置的包名，构建过程将会失败。如果你需要使用其它地方配置的license key或者需要在Unity构建过程之后修改包名，可以关闭这个选项。</para>
        /// </summary>
        [Tooltip("Verify license when build Unity Project. When this option is on, license key will be verified during Unity build process and build will fail if license key is invalid on the platform or the license key does not contain package name / bundle identifier setup in the Unity Player Settings. Turn off this option if you setup license in a difference place, or if you need to change package name or bundle identifier after Unity build process.")]
        [SerializeField]
        public bool VerifyLicenseWhenBuild = true;

        /// <summary>
        /// <para xml:lang="en">Configuration options related to the Unity XR Framework.</para>
        /// <para xml:lang="zh">与Unity XR Framework相关的配置项。</para>
        /// </summary>
        [Tooltip("Configuration options related to the Unity XR Framework.")]
        public UnityXROptions UnityXR = new();

        /// <summary>
        /// <para xml:lang="en">Global Mega Block localization service config.</para>
        /// <para xml:lang="zh">全局Mega Block定位服务器配置。</para>
        /// </summary>
        [Tooltip("Global Mega Block localization service config.")]
        public APIKeyAccessData GlobalMegaBlockLocalizationServiceConfig = new();
        /// <summary>
        /// <para xml:lang="en">Global Mega Landmark localization service config.</para>
        /// <para xml:lang="zh">全局Mega Landmark定位服务器配置。</para>
        /// </summary>
        [Tooltip("Global Mega Landmark localization service config.")]
        public APIKeyAccessData GlobalMegaLandmarkLocalizationServiceConfig = new();
        /// <summary>
        /// <para xml:lang="en">Global spatial map service config.</para>
        /// <para xml:lang="zh">全局稀疏地图服务器配置。</para>
        /// </summary>
        [Tooltip("Global spatial map service config.")]
        public FixedAddressAPIKeyAccessData GlobalSpatialMapServiceConfig = new();
        /// <summary>
        /// <para xml:lang="en">Global cloud Recognition service config.</para>
        /// <para xml:lang="zh">全局云识别服务器配置。</para>
        /// </summary>
        [Tooltip("Global cloud Recognition service config.")]
        public APIKeyAccessData GlobalCloudRecognizerServiceConfig = new();
        /// <summary>
        /// <para xml:lang="en"><see cref="Gizmos"/> configuration for <see cref="ImageTarget"/> and <see cref="ObjectTarget"/>.</para>
        /// <para xml:lang="zh"><see cref="ImageTarget"/> 和 <see cref="ObjectTarget"/>的<see cref="Gizmos"/>配置。</para>
        /// </summary>
        [Tooltip("Gizmos configuration for ImageTarget and ObjectTarget.")]
        public TargetGizmoConfig GizmoConfig = new();

        /// <summary>
        /// <para xml:lang="en">ARCore SDK configuration. If you are using AR Foundation, use <see cref="ARCoreType.ARFoundationOrOptional"/> to let the plugin decide which one to use, or use <see cref="ARCoreType.External"/>. If other ARCore SDK distributions is desired, use <see cref="ARCoreType.External"/>.</para>
        /// <para xml:lang="zh">ARCore SDK配置。如果你在使用AR Foundation，可以使用 <see cref="ARCoreType.ARFoundationOrOptional"/> 来让插件自动决定使用的ARCore分发，也可以使用<see cref="ARCoreType.External"/>。如果期望使用其它ARCore SDK分发，需要设置为<see cref="ARCoreType.External"/>。</para>
        /// </summary>
        [Tooltip("ARCore SDK configuration. If you are using AR Foundation, use ARFoundationOrOptional to let the plugin decide which one to use, or use External. If other ARCore SDK distributions is desired, use External.")]
        public ARCoreType ARCoreSDK;
        /// <summary>
        /// <para xml:lang="en">Warn 32-bit-only ARCore-enabled build. ARCore has removed support for 32-bit-only ARCore-enabled apps running on 64-bit devices. Support for 32-bit apps running on 32-bit devices is unaffected. 32-bit-only ARCore-enabled apps that are not updated may crash when attempting to start an (ARCore) AR session. See https://developers.google.com/ar/64bit for further details.</para>
        /// <para xml:lang="zh">在构建仅有32位且使用ARCore的应用时产生警告。ARCore已经移除了对64位设备上的只有32位库的ARCore的应用的支持。在32位设备上的32位应用不受影响。未更新的仅有32位库的ARCore应用可能会在尝试启动（ARCore的）AR session时崩溃。详情可以参考 https://developers.google.com/ar/64bit 。</para>
        /// </summary>
        [Tooltip("Warn 32-bit-only ARCore-enabled build. ARCore has removed support for 32-bit-only ARCore-enabled apps running on 64-bit devices. Support for 32-bit apps running on 32-bit devices is unaffected. 32-bit-only ARCore-enabled apps that are not updated may crash when attempting to start an (ARCore) AR session. See https://developers.google.com/ar/64bit for further details.")]
        public bool Verify32bitOnlyARCoreWhenBuild = true;
        /// <summary>
        /// <para xml:lang="en">AREngine SDK configuration. Set it to <see cref="AREngineType.AREngineInterop"/> if you want to use EasyAR AREngineInterop with AREngine SDK distribution along with plugin, <see cref="AREngineType.External"/> if other AREngine distributions is desired, <see cref="AREngineType.Disabled"/> if you do not want AREngine to appear in your app.</para>
        /// <para xml:lang="zh">AREngine SDK配置。如需使用EasyAR AREngineInterop以及与其一起分发的AREngine，设置为<see cref="AREngineType.AREngineInterop"/>，如需使用其它AREngine分发，需要设置为<see cref="AREngineType.External"/>。如果你不希望AREngine被打包到app中，需设置为<see cref="AREngineType.Disabled"/>。</para>
        /// </summary>
        [Tooltip("AREngine SDK configuration. Set it to AREngineInterop if you want to use EasyAR AREngineInterop with AREngine SDK distribution along with plugin, External if other AREngine distributions is desired, Disabled if you do not want AREngine to appear in your app.")]
        public AREngineType AREngineSDK;

        /// <summary>
        /// <para xml:lang="en">Workaround for Unity.</para>
        /// <para xml:lang="zh">Workaround for Unity.</para>
        /// </summary>
        public Workaround WorkaroundForUnity = new();

#if EASYAR_ENABLE_MEGA
        [SerializeField]
        private LibVariantConfig libVariantsMega = new() { Android = LibVariantConfig.AndroidVariant.Full, IOS = LibVariantConfig.IOSVariant.Full, };
#else
        [SerializeField]
        private LibVariantConfig libVariants = new() { Android = LibVariantConfig.AndroidVariant.Normal, IOS = LibVariantConfig.IOSVariant.Normal, };
#endif
        [SerializeField]
        private Permission permissions = new();
        [SerializeField]
        [Tooltip("If bundled ONNX runtime is used. ONNX is required when LibVariants.Android is Full. Suggest to use bundled ONNX or get latest version from official ONNX website. Set to false if non-bundled ONNX is being used.")]
        private bool useBundledONNXRuntime = true;

        /// <summary>
        /// <para xml:lang="en">ARCore SDK configuration.</para>
        /// <para xml:lang="zh">ARCore SDK配置。</para>
        /// </summary>
        public enum ARCoreType
        {
            /// <summary>
            /// <para xml:lang="en">Either ARCore SDK distributed with EasyAR or AR Foundation will be included in the build according to the settings of ARCore XR Plugin.</para>
            /// <para xml:lang="en">If ARCore SDK distributed with EasyAR is selected, ARCore features will be activated only on ARCore supported devices that have Google Play Services for AR installed.</para>
            /// <para xml:lang="en">Please visit https://developers.google.com/ar/develop/java/enable-arcore for more details and configurations required for your app.</para>
            /// <para xml:lang="zh">随EasyAR或AR Foundation一起分发的ARCore SDK将会被包含在应用中，根据ARCore XR Plugin的设置决定。</para>
            /// <para xml:lang="zh">如果随EasyAR一起分发的ARCore SDK被选中，ARCore 功能只在支持ARCore并安装了Google Play Services for AR的设备上可以使用。</para>
            /// <para xml:lang="zh">更多细节及应用所需要的配置请访问 https://developers.google.com/ar/develop/java/enable-arcore 。</para>
            /// </summary>
            [Tooltip("Either ARCore SDK distributed with EasyAR or AR Foundation will be included in the build according to the settings of ARCore XR Plugin. If ARCore SDK distributed with EasyAR is selected, ARCore features will be activated only on ARCore supported devices that have Google Play Services for AR installed.")]
            ARFoundationOrOptional,
            /// <summary>
            /// <para xml:lang="en">ARCore SDK distributed with EasyAR will be included in the build.</para>
            /// <para xml:lang="en">ARCore features are activated only on ARCore supported devices that have Google Play Services for AR installed.</para>
            /// <para xml:lang="en">Please visit https://developers.google.com/ar/develop/java/enable-arcore for more details and configurations required for your app.</para>
            /// <para xml:lang="zh">随EasyAR一起分发的ARCore SDK将会被包含在应用中。</para>
            /// <para xml:lang="zh">ARCore 功能只在支持ARCore并安装了Google Play Services for AR的设备上可以使用。</para>
            /// <para xml:lang="zh">更多细节及应用所需要的配置请访问 https://developers.google.com/ar/develop/java/enable-arcore 。</para>
            /// </summary>
            [Tooltip("ARCore SDK distributed with EasyAR will be included in the build. ARCore features are activated only on ARCore supported devices that have Google Play Services for AR installed.")]
            Optional,
            /// <summary>
            /// <para xml:lang="en">ARCore SDK distributed with EasyAR will be included in the build.</para>
            /// <para xml:lang="en">Your app will require an ARCore Supported Device that has Google Play Services for AR installed on it.</para>
            /// <para xml:lang="en">Please visit https://developers.google.com/ar/develop/java/enable-arcore for more details and configurations required for your app.</para>
            /// <para xml:lang="zh">随EasyAR一起分发的ARCore SDK将会被包含在应用中。</para>
            /// <para xml:lang="zh">应用将只能在支持ARCore并安装了Google Play Services for AR的设备上可以运行。</para>
            /// <para xml:lang="zh">更多细节及应用所需要的配置请访问 https://developers.google.com/ar/develop/java/enable-arcore 。</para>
            /// </summary>
            [Tooltip("ARCore SDK distributed with EasyAR will be included in the build. Your app will require an ARCore Supported Device that has Google Play Services for AR installed on it.")]
            Required,
            /// <summary>
            /// <para xml:lang="en">ARCore SDK distributed with EasyAR will not be used.</para>
            /// <para xml:lang="zh">随EasyAR一起分发的ARCore SDK将不会使用。</para>
            /// </summary>
            [Tooltip("ARCore SDK distributed with EasyAR will not be used.")]
            External,
        }

        /// <summary>
        /// <para xml:lang="en">AREngine SDK configuration.</para>
        /// <para xml:lang="zh">AREngine SDK配置。</para>
        /// </summary>
        public enum AREngineType
        {
            /// <summary>
            /// <para xml:lang="en">AREngineInterop is enabled. AREngine SDK distributed with EasyAR will be included in the build.</para>
            /// <para xml:lang="zh">AREngineInterop可用。随EasyAR一起分发的AREngine SDK将会被包含在应用中。</para>
            /// </summary>
            [Tooltip("AREngineInterop is enabled. AREngine SDK distributed with EasyAR will be included in the build.")]
            AREngineInterop,
            /// <summary>
            /// <para xml:lang="en">AREngineInterop is enabled. AREngine SDK distributed with EasyAR will not be used.</para>
            /// <para xml:lang="zh">AREngineInterop可用。随EasyAR一起分发的AREngine SDK将不会使用。</para>
            /// </summary>
            [Tooltip("AREngineInterop is enabled. AREngine SDK distributed with EasyAR will not be used.")]
            External,
            /// <summary>
            /// <para xml:lang="en">AREngineInterop is disalbed. AREngine SDK distributed with EasyAR will not be used.</para>
            /// <para xml:lang="zh">AREngineInterop不可用。随EasyAR一起分发的AREngine SDK将不会使用。</para>
            /// </summary>
            [Tooltip("AREngineInterop is disabled. AREngine SDK distributed with EasyAR will not be used.")]
            Disabled,
        }

        /// <summary>
        /// <para xml:lang="en">Global settings instance.</para>
        /// <para xml:lang="zh">全局<see cref="EasyARSettings"/>。</para>
        /// </summary>
        public static EasyARSettings Instance { get; private set; }

        /// <summary>
        /// <para xml:lang="en">EasyAR Sense Library variant configuration. Please reference <see cref="LibVariantConfig.Android"/> and <see cref="LibVariantConfig.IOS"/> to set.</para>
        /// <para xml:lang="zh">EasyAR Sense 库变种配置。配置建议请参考<see cref="LibVariantConfig.Android"/>及<see cref="LibVariantConfig.IOS"/>。</para>
        /// </summary>
        public LibVariantConfig LibVariants =>
#if EASYAR_ENABLE_MEGA
            libVariantsMega;
#else
            libVariants;
#endif

        /// <summary>
        /// <para xml:lang="en">Current application permissions configuration. Make sure to set camera permission if camera is used. Other permissions will be automatically on or off according to <see cref="LibVariants"/> values and whether Mega feature is on.</para>
        /// <para xml:lang="zh">当前应用权限配置。请确保在使用相机时打开相机权限。其它权限将根据<see cref="LibVariants"/>配置以及Mega功能是否打开自动开启或关闭。</para>
        /// </summary>
        public Permission Permissions => new()
        {
            Camera = permissions.Camera,
            AndroidMicrophone = LibVariants.Android == LibVariantConfig.AndroidVariant.VideoRecording,
#if EASYAR_ENABLE_MEGA
            Location = true,
#else
            Location = false,
#endif
        };

        /// <summary>
        /// <para xml:lang="en">If bundled ONNX runtime is used.</para>
        /// <para xml:lang="en">ONNX is required when <see cref="LibVariants"/> Android value is <see cref="LibVariantConfig.AndroidVariant.Full"/>. Suggest to use bundled ONNX or get latest version from official ONNX website. Set to false if non-bundled ONNX is being used.</para>
        /// <para xml:lang="zh">是否使用捆绑的ONNX runtime。</para>
        /// <para xml:lang="zh"><see cref="LibVariants"/>的Android配置为<see cref="LibVariantConfig.AndroidVariant.Full"/>时，需要使用ONNX。建议使用捆绑的版本，或从ONNX官方获取更新版本。使用非捆绑版本时，可以设置为false。</para>
        /// </summary>
        public bool UseBundledONNXRuntime => HasBundledONNXRuntime && useBundledONNXRuntime;

        internal bool HasBundledONNXRuntime => LibVariants.Android == LibVariantConfig.AndroidVariant.Full;
        internal static EasyARSettings ConfigObject
        {
            get
            {
                EasyARSettings settings = null;
#if UNITY_EDITOR
                UnityEditor.EditorBuildSettings.TryGetConfigObject("EasyAR.Settings", out settings);
                if (settings == null)
                {
                    settings = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(EasyARSettings)}").Select(asset => UnityEditor.AssetDatabase.LoadAssetAtPath<EasyARSettings>(UnityEditor.AssetDatabase.GUIDToAssetPath(asset))).SingleOrDefault();
                    if (settings != null)
                    {
                        ConfigObject = settings;
                    }
                }
#endif
                return settings;
            }
            set
            {
#if UNITY_EDITOR
                UnityEditor.EditorBuildSettings.AddConfigObject("EasyAR.Settings", value, true);
                Instance = value;
#endif
            }
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void CheckInstance()
        {
            var settings = ConfigObject;
            if (settings)
            {
                Instance = settings;
            }
        }

        void OnEnable()
        {
            if (Instance && Instance != this)
            {
                Debug.LogError($"Multiple {nameof(EasyARSettings)} asset NOT allowed!");
            }
            var settings = ConfigObject;
            if (settings)
            {
                Instance = settings;
            }
        }
#else
        void Awake()
        {
            Debug.Log("EasyAR Settings awakening...");
            if (Instance && Instance != this)
            {
                Debug.LogError($"Multiple {nameof(EasyARSettings)} instance NOT allowed!");
            }
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
        }
#endif

        /// <summary>
        /// <para xml:lang="en"><see cref="Gizmos"/> configuration for target.</para>
        /// <para xml:lang="zh">Target的<see cref="Gizmos"/>配置。</para>
        /// </summary>
        [Serializable]
        public class TargetGizmoConfig
        {
            /// <summary>
            /// <para xml:lang="en"><see cref="Gizmos"/> configuration for <see cref="easyar.ImageTarget"/>.</para>
            /// <para xml:lang="zh"><see cref="easyar.ImageTarget"/>的<see cref="Gizmos"/>配置。</para>
            /// </summary>
            [Tooltip("Gizmos configuration for ImageTarget.")]
            public ImageTargetConfig ImageTarget = new();
            /// <summary>
            /// <para xml:lang="en"><see cref="Gizmos"/> configuration for <see cref="easyar.ObjectTarget"/>.</para>
            /// <para xml:lang="zh"><see cref="easyar.ObjectTarget"/>的<see cref="Gizmos"/>配置。</para>
            /// </summary>
            [Tooltip("Gizmos configuration for ObjectTarget.")]
            public ObjectTargetConfig ObjectTarget = new();

            /// <summary>
            /// <para xml:lang="en"><see cref="Gizmos"/> configuration for <see cref="easyar.ImageTarget"/>.</para>
            /// <para xml:lang="zh"><see cref="easyar.ImageTarget"/>的<see cref="Gizmos"/>配置。</para>
            /// </summary>
            [Serializable]
            public class ImageTargetConfig
            {
                /// <summary>
                /// <para xml:lang="en">Enable <see cref="Gizmos"/> of target which <see cref="ImageTargetController.Source"/> is type of <see cref="ImageTargetController.ImageFileSourceData"/>. Enable this option will load image file and display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.</para>
                /// <para xml:lang="zh">开启<see cref="ImageTargetController.Source"/>类型为<see cref="ImageTargetController.ImageFileSourceData"/>的target的<see cref="Gizmos"/>。打开这个将会在Unity Editor中加载图像文件并显示对应gizmo，如果场景中该类target过多，可能会影响编辑器中的启动性能。在设备上运行时，Unity运行时的性能不会受到影响。</para>
                /// </summary>
                [Tooltip("Enable Gizmos of target which ImageTargetController.Source is type of ImageTargetController.ImageFileSourceData. Enable this option will load image file and display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.")]
                public bool EnableImageFile = true;
                /// <summary>
                /// <para xml:lang="en">Enable <see cref="Gizmos"/> of target which <see cref="ImageTargetController.Source"/> is type of <see cref="ImageTargetController.ImageFileSourceData"/>. Enable this option will target data file and display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.</para>
                /// <para xml:lang="zh">开启<see cref="ImageTargetController.Source"/>类型为<see cref="ImageTargetController.TargetDataFileSourceData"/>的target的<see cref="Gizmos"/>。打开这个将会在Unity Editor中加载target数据文件并显示显示对应gizmo，如果场景中该类target过多，可能会影响编辑器中的启动性能。在设备上运行时，Unity运行时的性能不会受到影响。</para>
                /// </summary>
                [Tooltip("Enable Gizmos of target which ImageTargetController.Source is type of ImageTargetController.TargetDataFileSourceData. Enable this option will target data file and display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.")]
                public bool EnableTargetDataFile = true;
                /// <summary>
                /// <para xml:lang="en">Enable <see cref="Gizmos"/> of target which <see cref="ImageTargetController.Source"/> is type of <see cref="ImageTargetController.TargetSourceData"/>. Enable this option will display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.</para>
                /// <para xml:lang="zh">开启<see cref="ImageTargetController.Source"/>类型为<see cref="ImageTargetController.TargetSourceData"/>的target的<see cref="Gizmos"/>。打开这个将会在Unity Editor中显示对应gizmo，如果场景中该类target过多，可能会影响编辑器中的启动性能。在设备上运行时，Unity运行时的性能不会受到影响。</para>
                /// </summary>
                [Tooltip("Enable Gizmos of target which ImageTargetController.Source is type of ImageTargetController.TargetSourceData. Enable this option will display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.")]
                public bool EnableTarget = true;
                /// <summary>
                /// <para xml:lang="en">Enable <see cref="Gizmos"/> of target which <see cref="ImageTargetController.Source"/> is type of <see cref="ImageTargetController.Texture2DSourceData"/>. Enable this option will display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.</para>
                /// <para xml:lang="zh">开启<see cref="ImageTargetController.Source"/>类型为<see cref="ImageTargetController.Texture2DSourceData"/>的target的<see cref="Gizmos"/>。打开这个将会在Unity Editor中显示对应gizmo，如果场景中该类target过多，可能会影响编辑器中的启动性能。在设备上运行时，Unity运行时的性能不会受到影响。</para>
                /// </summary>
                [Tooltip("Enable Gizmos of target which ImageTargetController.Source is type of ImageTargetController.Texture2DSourceData. Enable this option will display gizmo in Unity Editor, the startup performance of the Editor will be affected if there are too much target of this kind in the scene, but the Unity runtime will not be affected when running on devices.")]
                public bool EnableTexture2D = true;
            }

            /// <summary>
            /// <para xml:lang="en"><see cref="Gizmos"/> configuration for <see cref="easyar.ObjectTarget"/>.</para>
            /// <para xml:lang="zh"><see cref="easyar.ObjectTarget"/>的<see cref="Gizmos"/>配置。</para>
            /// </summary>
            [Serializable]
            public class ObjectTargetConfig
            {
                /// <summary>
                /// <para xml:lang="en">Enable <see cref="Gizmos"/>.</para>
                /// <para xml:lang="zh">开启<see cref="Gizmos"/>。</para>
                /// </summary>
                public bool Enable = true;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Permission configuration.</para>
        /// <para xml:lang="zh">权限配置。</para>
        /// </summary>
        [Serializable]
        public class Permission
        {
            /// <summary>
            /// <para xml:lang="en">Camera permission. Permission required for <see cref="easyar.CameraDevice"/> and other frame sources which require camera device usages.</para>
            /// <para xml:lang="en">cameraUsageDescription must be set when build iOS apps when this option is on.</para>
            /// <para xml:lang="zh">相机权限。使用<see cref="easyar.CameraDevice"/>及其它需要使用相机设备的frame source需要的权限。</para>
            /// <para xml:lang="zh">该权限打开时，打包iOS应用需要设置cameraUsageDescription。</para>
            /// </summary>
            [Tooltip("Camera permission. Permission required for easyar.CameraDevice and other frame sources which require camera device usages. cameraUsageDescription must be set when build iOS apps when this option is on.")]
            public bool Camera = true;
            /// <summary>
            /// <para xml:lang="en">Microphone permission, Android only. Permission required for <see cref="easyar.VideoRecorder"/>.</para>
            /// <para xml:lang="zh">麦克风权限，仅适用于Android。使用<see cref="easyar.VideoRecorder"/>需要的权限。</para>
            /// </summary>
            [Tooltip("Microphone permission, Android only. Permission required for easyar.VideoRecorder.")]
            public bool AndroidMicrophone = true;
            /// <summary>
            /// <para xml:lang="en">(fine) Location permission. Required for <see cref="easyar.MegaTrackerFrameFilter"/> and <see cref="easyar.CloudLocalizerFrameFilter"/>.</para>
            /// <para xml:lang="en">locationUsageDescription must be set when build iOS apps when this option is on.</para>
            /// <para xml:lang="zh">（fine）定位权限。使用<see cref="easyar.MegaTrackerFrameFilter"/>和<see cref="easyar.CloudLocalizerFrameFilter"/>需要的权限。</para>
            /// <para xml:lang="zh">该权限打开时，打包iOS应用需要设置locationUsageDescription。</para>
            /// </summary>
            [Tooltip("(fine) Location permission. Permission required for easyar.MegaTrackerFrameFilter and easyar.CloudLocalizerFrameFilter. locationUsageDescription must be set when build iOS apps when this option is on.")]
            public bool Location = true;
        }

        /// <summary>
        /// <para xml:lang="en">Workaround for Unity.</para>
        /// <para xml:lang="zh">Workaround for Unity.</para>
        /// </summary>
        [Serializable]
        public class Workaround
        {
            /// <summary>
            /// <para xml:lang="en">Generate XML document when script reload to make intelliSense for API document work.</para>
            /// <para xml:lang="zh">在脚本重新加载时生成XML文档，以使API文档的intelliSense可以工作。</para>
            /// </summary>
            [Tooltip("Generate XML document when script reload to make intelliSense for API document work.")]
            public bool GenerateXMLDoc = true;
#if !UNITY_6000_2_OR_NEWER
            /// <summary>
            /// <para xml:lang="en">Workaround URP 17 Render Graph DX11 scene rendering ruined.</para>
            /// <para xml:lang="zh">Workaround URP 17 Render Graph DX11 场景渲染被毁损.</para>
            /// </summary>
            [Tooltip("Workaround URP 17 Render Graph DX11 scene rendering ruined.")]
            public bool URP17RG_DX11_RuinedScene = true;
#endif
            /// <summary>
            /// <para xml:lang="en">Partial workaround for https://issuetracker.unity3d.com/issues/ios-visual-artifacts-are-visible-when-the-cameras-feed-is-rendered-to-a-texture.</para>
            /// <para xml:lang="zh">Partial workaround for https://issuetracker.unity3d.com/issues/ios-visual-artifacts-are-visible-when-the-cameras-feed-is-rendered-to-a-texture.</para>
            /// </summary>
            [Tooltip("Partial workaround for https://issuetracker.unity3d.com/issues/ios-visual-artifacts-are-visible-when-the-cameras-feed-is-rendered-to-a-texture.")]
            public bool URP17RG_IOS_Glitches_Partial = true;
        }

        /// <summary>
        /// <para xml:lang="en">EasyAR Sense Library variant configuration.</para>
        /// <para xml:lang="zh">EasyAR Sense 库变种配置。</para>
        /// </summary>
        [Serializable]
        public class LibVariantConfig
        {
            /// <summary>
            /// <para xml:lang="en">EasyAR Sense Android Library variant.</para>
            /// <para xml:lang="en">Set to <see cref="AndroidVariant.Normal"/> if InertialCameraDeviceFrameSource/VideoRecorder are not used, this will reduce your apk size. Set to <see cref="AndroidVariant.Full"/> if InertialCameraDeviceFrameSource is to be used, suggest to use in Mega. Set to VideoRecording if VideoRecorder is to be used.</para>
            /// <para xml:lang="zh">EasyAR Sense Android库变种。</para>
            /// <para xml:lang="zh">如不需使用<see cref="InertialCameraDeviceFrameSource"/>/<see cref="VideoRecorder"/>，设为<see cref="AndroidVariant.Normal"/>，这会降低apk大小。如需使用<see cref="InertialCameraDeviceFrameSource"/>，设为<see cref="AndroidVariant.Full"/>，建议在使用Mega时使用。如需使用<see cref="VideoRecorder"/>，设为VideoRecording。</para>
            /// </summary>
            [Tooltip("EasyAR Sense Android Library variant. Set to Normal if InertialCameraDevice/VideoRecording are not used, this will reduce your apk size. Set to Full if InertialCameraDevice is to be used, suggest to use in Mega. Set to VideoRecording if VideoRecorder is to be used.")]
            public AndroidVariant Android;
            /// <summary>
            /// <para xml:lang="en">EasyAR Sense iOS Library variant.</para>
            /// <para xml:lang="en">Set to <see cref="IOSVariant.Normal"/> if InertialCameraDevice is not used, this will reduce your ipa size. Set to <see cref="IOSVariant.Full"/> if InertialCameraDevice is to be used, suggest to use in Mega.</para>
            /// <para xml:lang="zh">EasyAR Sense iOS库变种。</para>
            /// <para xml:lang="zh">如不需使用<see cref="InertialCameraDeviceFrameSource"/>，设为<see cref="AndroidVariant.Normal"/>，这会降低ipa大小。如需使用<see cref="InertialCameraDeviceFrameSource"/>，设为<see cref="AndroidVariant.Full"/>，建议在使用Mega时使用。</para>
            /// </summary>
            [Tooltip("EasyAR Sense iOS Library variant. Set to Normal if InertialCameraDeviceFrameSource is not used, this will reduce your ipa size. Set to Full if InertialCameraDeviceFrameSource is to be used, suggest to use in Mega.")]
            public IOSVariant IOS;

            /// <summary>
            /// <para xml:lang="en">EasyAR Sense Android Library variant.</para>
            /// <para xml:lang="zh">EasyAR Sense Android库变种。</para>
            /// </summary>
            public enum AndroidVariant
            {
                /// <summary>
                /// <para xml:lang="en">EasyAR Sense Community/Enterprise variant.</para>
                /// <para xml:lang="en">These features are not included: InertialCameraDevice/VideoRecording.</para>
                /// <para xml:lang="zh">EasyAR Sense Community/Enterprise variant.</para>
                /// <para xml:lang="zh">这些功能未包含在内：InertialCameraDevice/VideoRecording。</para>
                /// </summary>
                [Tooltip("EasyAR Sense Community/Enterprise variant. These features are not included: InertialCameraDevice/VideoRecording.")]
                Normal,
                /// <summary>
                /// <para xml:lang="en">EasyAR Sense CommunityFull/EnterpriseFull variant.</para>
                /// <para xml:lang="en">These features are included: InertialCameraDevice. These features are not included: VideoRecording. Library size is larger than other variants.</para>
                /// <para xml:lang="zh">EasyAR Sense CommunityFull/EnterpriseFull variant.</para>
                /// <para xml:lang="zh">这些功能包含在内：InertialCameraDevice。这些功能未包含在内：VideoRecording。库文件大小比其它变体大。</para>
                /// </summary>
                [Tooltip("EasyAR Sense CommunityFull/EnterpriseFull variant. These features are included: InertialCameraDevice. These features are not included: VideoRecording. Library size is larger than other variants.")]
                Full,
                /// <summary>
                /// <para xml:lang="en">EasyAR Sense CommunityR variant.</para>
                /// <para xml:lang="en">These features are not included: InertialCameraDevice.</para>
                /// <para xml:lang="zh">EasyAR Sense CommunityR variant.</para>
                /// <para xml:lang="zh">这些功能未包含在内：InertialCameraDevice。</para>
                /// </summary>
                [Tooltip("EasyAR Sense CommunityR variant. These features are included: VideoRecording. These features are not included: InertialCameraDevice.")]
                VideoRecording,
            }

            /// <summary>
            /// <para xml:lang="en">EasyAR Sense iOS Library variant.</para>
            /// <para xml:lang="zh">EasyAR Sense iOS库变种。</para>
            /// </summary>
            public enum IOSVariant
            {
                /// <summary>
                /// <para xml:lang="en">EasyAR Sense Community/Enterprise variant.</para>
                /// <para xml:lang="en">These features are not included: InertialCameraDevice/VideoRecording.</para>
                /// <para xml:lang="zh">EasyAR Sense Community/Enterprise variant.</para>
                /// <para xml:lang="zh">这些功能未包含在内：InertialCameraDevice/VideoRecording。</para>
                /// </summary>
                [Tooltip("EasyAR Sense Community/Enterprise variant. These features are not included: InertialCameraDevice/VideoRecording.")]
                Normal,
                /// <summary>
                /// <para xml:lang="en">EasyAR Sense CommunityFull/EnterpriseFull variant.</para>
                /// <para xml:lang="en">These features are included: InertialCameraDevice. These features are not included: VideoRecording. Library size is larger than other variants.</para>
                /// <para xml:lang="zh">EasyAR Sense CommunityFull/EnterpriseFull variant.</para>
                /// <para xml:lang="zh">这些功能包含在内：InertialCameraDevice。这些功能未包含在内：VideoRecording。库文件大小比其它变体大。</para>
                /// </summary>
                [Tooltip("EasyAR Sense CommunityFull/EnterpriseFull variant. These features are included: InertialCameraDevice. These features are not included: VideoRecording. Library size is larger than other variants.")]
                Full,
            }
        }

        /// <summary>
        /// <para xml:lang="en">Configuration options related to the Unity XR Framework.</para>
        /// <para xml:lang="zh">与Unity XR Framework相关的配置项。</para>
        /// </summary>
        [Serializable]
        public class UnityXROptions
        {
            /// <summary>
            /// <para xml:lang="en">EasyAR AR Foundation Support. It is recommended to keep this enabled; otherwise, EasyAR may not work properly with AR Foundation. Disable only if a future AR Foundation update causes incompatibility and EasyAR has not yet provided an update.</para>
            /// <para xml:lang="zh">EasyAR AR Foundation支持。建议常开否则使用ARFoundation时EasyAR可能无法正常工作。仅建议在未来AR Foundation更新产生不兼容且EasyAR暂未推出更新时关闭。</para>
            /// </summary>
            [Tooltip("EasyAR AR Foundation Support. It is recommended to keep this enabled; otherwise, EasyAR may not work properly with AR Foundation. Disable only if a future AR Foundation update causes incompatibility and EasyAR has not yet provided an update.")]
            public bool ARFoundationSupport = true;

            /// <summary>
            /// <para xml:lang="en">Auto switch Unity XR (like AR Foundation) objects. This option is primarily designed for mobile AR. On head-mounted devices, the feature will be disabled by default.</para>
            /// <para xml:lang="en">If you need to manually control the switching of these components, or if EasyAR's behavior interferes with the normal operation of certain components, make sure to disable these options.</para>
            /// <para xml:lang="en">You need to add `ARSession` and `XR Origin` to your scene using its context menu when you need to switch AR Foundation automatically in runtime, and use camera from AR Foundation.</para>
            /// <para xml:lang="en">In play mode, the <see cref="ARSession"/> will disable all Unity XR Core components and all AR Foundation components on awake.</para>
            /// <para xml:lang="en">In play mode, if the selected frame source is derived from <see cref="ARFoundationFrameSource"/> or <see cref="ExternalDeviceFrameSource"/> using XROrigin, all disabled Unity XR Core components and AR Foundation components (not including those not disabled by EasyAR) will be enabed in <see cref="ARSession.StartSession()"/>. If other frame source is selected, all Unity XR Core components and AR Foundation components will be disabled in <see cref="ARSession.StartSession()"/>.</para>
            /// <para xml:lang="en">In play mode, all Unity XR Core components and all AR Foundation components will be disabled in <see cref="ARSession.StopSession(bool)"/>.</para>
            /// <para xml:lang="en">Please notices that XR Interaction Toolkit components are not under control of this option, and whether they can work with EasyAR is unverified. Theoretically, compoents use only XROrigin GameObject and its camera can work, but you may need to set <see cref="ARSession.CenterMode"/> to <see cref="ARSession.ARCenterMode.SessionOrigin"/>. If the components are not functional, you need to control XR Interaction Toolkit componets, and disable them when frame source is not derived from <see cref="ARFoundationFrameSource"/>.</para>
            /// <para xml:lang="zh">自动切换Unity XR（比如AR Foundation）物体。这个选项主要为移动AR设计，头显上默认配置下功能会被禁用。</para>
            /// <para xml:lang="zh">如果你需要自己控制这些组件的切换，或是EasyAR的行为干扰了某些组件的正常工作，请确保关闭这些选项。</para>
            /// <para xml:lang="zh">如果你需要在运行时自动切换AR Foundation，你需要通过Unity的右键菜单在场景中创建AR Foundation的 `ARSession` 及 `XR Origin` ，并使用AR Foundation的相机。</para>
            /// <para xml:lang="zh">运行时，<see cref="ARSession"/>会在awake时禁用所有Unity XR Core组件及AR Foundation的组件。</para>
            /// <para xml:lang="zh">运行时，如果被选择的frame source继承自<see cref="ARFoundationFrameSource"/>或是实现了XROrigin原点的<see cref="ExternalDeviceFrameSource"/>，则被禁用的Unity XR Core组件及AR Foundation组件将在<see cref="ARSession.StartSession()"/>时启用（未被EasyAR禁用的不会启用）。如果其他frame source被选择，则在<see cref="ARSession.StartSession()"/>时会禁用所有Unity XR Core组件及AR Foundation的组件。</para>
            /// <para xml:lang="zh">运行时，所有Unity XR Core组件及AR Foundation的组件会在<see cref="ARSession.StopSession(bool)"/>时禁用。</para>
            /// <para xml:lang="zh">需要注意，XR Interaction Toolkit的组件不受该选项控制，但其在EasyAR中是否可用未经验证。理论上对于只使用XROrigin GameObject和其Camera的功能应该可以正常使用，但你可能需要设置<see cref="ARSession.CenterMode"/>为<see cref="ARSession.ARCenterMode.SessionOrigin"/>。如果功能不正常，你需要自己管理XR Interaction Toolkit的组件，在frame source不是继承自<see cref="ARFoundationFrameSource"/>时禁用相关组件。</para>
            /// </summary>
            [Tooltip("Auto switch Unity XR (like AR Foundation) objects. This option is primarily designed for mobile AR. On head-mounted devices, the feature will be disabled by default. If you need to manually control the switching of these components, or if EasyAR's behavior interferes with the normal operation of certain components, make sure to disable these options. Please read API documents for more details.")]
            public AutoSwitchOptions UnityXRAutoSwitch = new();

            /// <summary>
            /// <para xml:lang="en">Options to auto switch Unity XR (like AR Foundation) objects.</para>
            /// <para xml:lang="zh">自动切换Unity XR（比如AR Foundation）物体的选项。</para>
            /// </summary>
            [Serializable]
            public class AutoSwitchOptions
            {
                /// <summary>
                /// <para xml:lang="en">Edit mode options.</para>
                /// <para xml:lang="zh">编辑模式选项。</para>
                /// </summary>
                [Tooltip("Edit mode options.")]
                public EditorOptions Editor = new();
                /// <summary>
                /// <para xml:lang="en">Play mode options.</para>
                /// <para xml:lang="zh">运行模式选项。</para>
                /// </summary>
                [Tooltip("Play mode options.")]
                public PlayerOptions Player = new();

                /// <summary>
                /// <para xml:lang="en">Edit mode options.</para>
                /// <para xml:lang="zh">编辑模式选项。</para>
                /// </summary>
                [Serializable]
                public class EditorOptions
                {
                    /// <summary>
                    /// <para xml:lang="en">In edit mode, disable ARSession of AR Foundation when <see cref="ARSession"/> exists.</para>
                    /// <para xml:lang="zh">存在<see cref="ARSession"/>时，编辑时禁用AR Foundation的ARSession。</para>
                    /// </summary>
                    [Tooltip("In edit mode, disable ARSession of AR Foundation when easyar.ARSession exists.")]
                    public bool DisableARSession = true;
                }

                /// <summary>
                /// <para xml:lang="en">Play mode options.</para>
                /// <para xml:lang="zh">运行模式选项。</para>
                /// </summary>
                [Serializable]
                public class PlayerOptions
                {
                    /// <summary>
                    /// <para xml:lang="en">Enable play mode control. Note: If this option is disabled, components that are disabled during edit mode will not be restored at play mode.</para>
                    /// <para xml:lang="zh">启用运行时控制。注意：关闭该选项，编辑模式被禁用的组件在运行时不会被恢复。</para>
                    /// </summary>
                    [Tooltip("Enable play mode control. Note: If this option is disabled, components that are disabled during edit mode will not be restored at play mode.")]
                    public bool Enable = true;
                    /// <summary>
                    /// <para xml:lang="en">Enable if on Windows/Mac.</para>
                    /// <para xml:lang="zh">在Windows/Mac上启用。</para>
                    /// </summary>
                    [Tooltip("Enable if on Windows/Mac.")]
                    public bool EnableIfDesktop = true;
                    /// <summary>
                    /// <para xml:lang="en">Enable if mobile AR (ARKit/ARCore) loader is active when the switch starts. This option usually require 'Initialize XR on Startup' of XR Plug-in Management is checked.</para>
                    /// <para xml:lang="zh">切换器启动时，如果移动AR（ARKit/ARCore）的loader是激活的，则启用。通常这个选项需要 XR Plug-in Management中的 'Initialize XR on Startup' 是选中的。</para>
                    /// </summary>
                    [Tooltip("Enable if mobile AR (ARKit/ARCore) loader is active when the switch starts. This option usually require 'Initialize XR on Startup' of XR Plug-in Management is checked.")]
                    public bool EnableIfMobileAROnStartup = true;
                    /// <summary>
                    /// <para xml:lang="en">Disable if non-mobile AR (ARKit/ARCore) loader is found but no active loader exist. This option is usually used when 'Initialize XR on Startup' of XR Plug-in Management is not checked.</para>
                    /// <para xml:lang="zh">切换器启动时，如果存在移动AR（ARKit/ARCore）之外的其它loader，但没有任何一个loader是激活的，则禁用。通常这个选项会在 XR Plug-in Management中的 'Initialize XR on Startup' 未选中时被使用。</para>
                    /// </summary>
                    [Tooltip("Disable if non-mobile AR (ARKit/ARCore) loader is found but no active loader exist. This option is usually used when 'Initialize XR on Startup' of XR Plug-in Management is not checked.")]
                    public bool DisableIfNonMobileARPostStartup = true;
                    /// <summary>
                    /// <para xml:lang="en">When function disabled, restore (enable) all disabled ARSessions of AR Foundation (whether they are disabled by EasyAR or not). This options is usually used to restore disabled components in edit mode.</para>
                    /// <para xml:lang="zh">功能禁用时，恢复（启用）所有被禁用的AR Foundation的ARSession（无论它是否由EasyAR所禁用）。这个选项通常用于恢复编辑时被禁用的组件。</para>
                    /// </summary>
                    [Tooltip("When function disabled, restore (enable) all disabled ARSessions of AR Foundation (whether they are disabled by EasyAR or not). This options is usually used to restore disabled components in edit mode.")]
                    public bool RestoreARSessionWhenDisabled = true;
                }
            }
        }
    }
}
