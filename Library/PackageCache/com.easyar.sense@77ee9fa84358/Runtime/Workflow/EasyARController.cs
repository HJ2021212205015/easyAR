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
    /// <para xml:lang="en">Static delegate of EasyAR Sense Engine class, mainly used for Sense initialization.</para>
    /// <para xml:lang="zh">EasyAR Sense Engine类的静态代理，主要用于Sense初始化。</para>
    /// </summary>
    [DisallowMultipleComponent]
    public class EasyARController : MonoBehaviour
    {
        private static readonly LicenseValidation validation = new LicenseValidation();
        private static Action<LogLevel, string> logFunc;

        /// <summary>
        /// <para xml:lang="en">EasyAR Sense is ready or not, false if license key validation fails or not run.</para>
        /// <para xml:lang="zh">EasyAR Sense 是否已经准备好。如果license key验证失败或未执行会是false。</para>
        /// </summary>
        public static bool IsReady => validation.IsReady;

        internal static DelayedCallbackScheduler Scheduler { get; private set; }

        internal static LicenseValidation LicenseValidation => validation;
        internal static DefaultSystemDisplayProvider DefaultSystemDisplay { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void AttemptInitializeOnLoad()
        {
            validation.OnSceneLoad();
            if (!EasyARSettings.Instance || !EasyARSettings.Instance.InitializeOnStartup) { return; }
            Initialize();
        }

        private void Awake()
        {
            RunScheduler();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Engine.onPause();
            }
            else
            {
                Engine.onResume();
            }
        }

        /// <summary>
        /// <para xml:lang="en">EasyAR Sense initialization with <see cref="EasyARSettings.LicenseKey"/>. Usually no need to call manually if <see cref="EasyARSettings.InitializeOnStartup"/> is true.</para>
        /// <para xml:lang="zh">使用<see cref="EasyARSettings.LicenseKey"/>初始化EasyAR Sense。<see cref="EasyARSettings.InitializeOnStartup"/>为true时通常不需要手动调用。</para>
        /// </summary>
        public static bool Initialize() => Initialize(EasyARSettings.Instance != null ? EasyARSettings.Instance.LicenseKey : string.Empty);

        /// <summary>
        /// <para xml:lang="en">EasyAR Sense initialization with <paramref name="licenseKey"/>. Usually no need to call manually if <see cref="EasyARSettings.InitializeOnStartup"/> is true.</para>
        /// <para xml:lang="zh">使用<paramref name="licenseKey"/>初始化EasyAR Sense。<see cref="EasyARSettings.InitializeOnStartup"/>为true时通常不需要手动调用。</para>
        /// </summary>
        public static bool Initialize(string licenseKey)
        {
            var licenseUsed = licenseKey == null ? string.Empty : licenseKey.Trim();
            try
            {
                Debug.Log($"{UnityPackage.DisplayName} Version {UnityPackage.Version}");

                PackageChecker.CheckPath();
                PackageChecker.CheckOldAssets();
                Scheduler?.Dispose();
                Scheduler = new DelayedCallbackScheduler();
                if (DefaultSystemDisplay == null)
                {
                    DefaultSystemDisplay = new DefaultSystemDisplayProvider();
                }
                HandleSenseLog(logFunc);
                AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
#if UNITY_ANDROID && !UNITY_EDITOR
                using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (var easyarEngineClass = new AndroidJavaClass("cn.easyar.Engine"))
                {
                    var activityclassloader = currentActivity.Call<AndroidJavaObject>("getClass").Call<AndroidJavaObject>("getClassLoader");
                    if (activityclassloader == null)
                    {
                        Debug.Log("ActivityClassLoader is null");
                    }
                    easyarEngineClass.CallStatic("loadLibraries");
                    if (!easyarEngineClass.CallStatic<bool>("setupActivity", currentActivity))
                    {
                        throw new Exception("EasyAR Engine setupActivity fail");
                    }
                }
#endif
                var success = Engine.initialize(licenseUsed);

#if UNITY_ANDROID && !UNITY_EDITOR
                var isAREngineEnabled = true;
                if (EasyARSettings.Instance)
                {
                    isAREngineEnabled = EasyARSettings.Instance.AREngineSDK != EasyARSettings.AREngineType.Disabled;
                }
                if (success && isAREngineEnabled)
                {
                    using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using (var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                    using (var arengineinteropClass = new AndroidJavaClass("cn.easyar.arengineinterop.Engine"))
                    {
                        arengineinteropClass.CallStatic("loadLibraries");
                        if (!arengineinteropClass.CallStatic<bool>("setupActivity", currentActivity))
                        {
                            throw new Exception("EasyAR AREngineInterop setupActivity fail");
                        }
                    }
                    success = arengineinterop.Engine.initialize(licenseUsed);
                }
#endif
                validation.OnInitialize(licenseUsed, success, null);
            }
            catch (Exception e)
            {
                validation.OnInitialize(licenseUsed, false, e);
                throw e;
            }
            return validation.InitializationState == ValidationState.Successful;
        }

        /// <summary>
        /// <para xml:lang="en">EasyAR Sense deinitialize.</para>
        /// <para xml:lang="en">This method has nothihng to do with resource dispose. Usually do not require to call manually. Use it if you want to initialize and deinitialize EasyAR multiple times.</para>
        /// <para xml:lang="zh">反初始化EasyAR Sense。</para>
        /// <para xml:lang="zh">这个方法与资源释放无关。通常不需要手动调用。在需要初始化与反初始化多次的时候调用。</para>
        /// </summary>
        public static void Deinitialize()
        {
            easyar.Log.resetLogFunc();
            Scheduler?.Dispose();
            Scheduler = null;
            DefaultSystemDisplay?.Dispose();
            DefaultSystemDisplay = null;
            validation.Clear();
            AppDomain.CurrentDomain.DomainUnload -= OnDomainUnload;
        }

        /// <summary>
        /// <para xml:lang="en">Handle Sense log. Pass in null will stop receive log.</para>
        /// <para xml:lang="en">EasyAR Sense library usually output log using system interface. Use this function to display log on UI or get log data. Notice, using this function will not remove log output.</para>
        /// <para xml:lang="zh">处理Sense日志。传入空可以停止接收日志。</para>
        /// <para xml:lang="zh">EasyAR Sense库通常通过系统接口直接输出日志，如需要显示在UI或获取日志内容，向可以使用这个接口。需要注意的是，使用这个接口并不会删除日志本身的输出。</para>
        /// </summary>
        public static void HandleSenseLog(Action<LogLevel, string> func)
        {
            logFunc = func;
            if (Scheduler == null) { return; }

            if (func == null)
            {
#if UNITY_EDITOR
                easyar.Log.setLogFuncWithScheduler(Scheduler, (LogLevel, msg) => Log(LogLevel, msg));
#else
                easyar.Log.resetLogFunc();
#endif
            }
            else
            {
                easyar.Log.setLogFuncWithScheduler(Scheduler, (LogLevel, msg) =>
                {
                    Log(LogLevel, msg);
                    logFunc?.Invoke(LogLevel, msg);
                });
            }
        }

        internal static void RunScheduler()
        {
            while (Scheduler?.runOne() ?? false) { }
        }

        private static void OnDomainUnload(object sender, EventArgs args)
        {
            try
            {
                Deinitialize();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private static void Log(LogLevel LogLevel, string msg)
        {
            switch (LogLevel)
            {
                case LogLevel.Error:
                    Debug.LogError("[EasyAR] " + msg);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning("[EasyAR] " + msg);
                    break;
                case LogLevel.Info:
                    Debug.Log("[EasyAR] " + msg);
                    break;
                default:
                    break;
            }
        }
    }

    internal class LicenseValidation
    {
        private static bool hasSceneLoaded;
        private ValidationState initState = ValidationState.Pending;
        private static Optional<string> initLicense;
        private static string initException = string.Empty;
        private Optional<string> runtimeFail;
        private static Optional<string> error;

        public bool IsReady => !IsAssembliesReloaded && InitializationState == ValidationState.Successful && ValidationState != ValidationState.Failed && runtimeFail.OnNone;
        public ValidationState InitializationState => initState;
        public ValidationState ValidationState => Engine.validationState();
        public bool IsLicenseInvalid => ValidationState == ValidationState.Failed || runtimeFail.OnSome;
        public bool IsAssembliesReloaded => Application.isEditor && !hasSceneLoaded && initLicense.OnNone;
        public int InitializeCount { get; private set; }
        public int ErrorIndex { get; private set; }
        public Optional<string> Error
        {
            get
            {
                if (!IsReady)
                {
                    if (error.OnNone)
                    {
                        error = ComposeErrorMessage();
                        ErrorIndex++;
                    }
                }
                return error;
            }
        }

        public void OnSceneLoad()
        {
            hasSceneLoaded = true;
        }

        public void OnInitialize(string licenseKey, bool success, Optional<Exception> exception)
        {
            InitializeCount++;
            error = null;
            runtimeFail = null;
            initLicense = licenseKey;
            initState = success ? ValidationState.Successful : ValidationState.Failed;
            hasSceneLoaded = true;

            initException = string.Empty;
            if (exception.OnSome)
            {
                var e = exception.Value;
                if (e is DllNotFoundException
#if UNITY_ANDROID && !UNITY_EDITOR
                    || e is AndroidJavaException
#endif
                    )
                {
                    initException += "Fail to load EasyAR library." + Environment.NewLine;
                }
                if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
                {
                    Version version;
                    if (Version.TryParse(SystemInfo.operatingSystem.ToLower().Replace("mac", "").Replace("os", "").Replace("x", "").Trim(), out version))
                    {
                        if (version.Major < 10 || (version.Major == 10 && version.Minor < 15))
                        {
                            initException += $"EasyAR Sense does not run on {SystemInfo.operatingSystem} (require 10.15 or later)." + Environment.NewLine;
                        }
                    }
                }
                initException += "Exception caught in Initialize:" + Environment.NewLine;
                initException += $"{e.GetType()}: {e.Message}";
            }
        }

        public void Clear()
        {
            initLicense = null;
            initState = ValidationState.Pending;
        }

        public void OnResultSyncModeFail(Type type)
        {
            runtimeFail = $"Eyewear support not enabled or license does not apply to eyewear/{type}" + Environment.NewLine;
        }

        public string ComposeErrorMessage()
        {
            if (IsAssembliesReloaded)
            {
                return "EasyAR stops after script change in play mode. EasyAR may not work correctly before restart playing. Please restart playing to use EasyAR again.";
            }
            if (initLicense.OnNone)
            {
                var missingAsset = $" Did you forget to setup EasyAR Sense in Project Settings of Unity Editor?";
                if (!EasyARSettings.Instance)
                {
                    return $"{nameof(EasyARSettings)} is not found." + missingAsset;
                }
                return $"{nameof(EasyARController)}.{nameof(EasyARController.Initialize)} is not called (InitializeOnStartup = {EasyARSettings.Instance.InitializeOnStartup})." + (EasyARSettings.Instance.InitializeOnStartup ? missingAsset + $" Or did you called {nameof(EasyARController)}.{nameof(EasyARController.Deinitialize)}?" : string.Empty);
            }

            var message = initException;
            if (string.IsNullOrEmpty(message))
            {
                message = Engine.errorMessage();
            }
            if (string.IsNullOrEmpty(message))
            {
                message = runtimeFail.ValueOrDefault(string.Empty);
            }
            if (string.IsNullOrEmpty(message))
            {
                if (InitializationState != ValidationState.Successful)
                {
                    message = $"EasyAR Initialize {InitializationState}";
                }
                else if (ValidationState == ValidationState.Failed)
                {
                    message = $"EasyAR License Validation {ValidationState}";
                }
                else
                {
                    message = $"Unknown State ({InitializationState}, {ValidationState}, {runtimeFail})";
                }
            }

            if (string.IsNullOrEmpty(initLicense.Value))
            {
                message += Environment.NewLine + "License Key is empty" + Environment.NewLine +
                    "Get from EasyAR Develop Center (www.easyar.com) -> SDK Authorization" +
                    (Application.isEditor ? " and fill it into \"Project Settings > EasyAR\"." : "");
            }
            else
            {
                var key = initLicense.Value;
                if (key.Length > 10)
                {
                    key = key.Substring(0, 5) + "..." + key.Substring(key.Length - 5, 5);
                }
                message += Environment.NewLine + $"License key in use: {key}";
            }
            return message;
        }
    }
}
