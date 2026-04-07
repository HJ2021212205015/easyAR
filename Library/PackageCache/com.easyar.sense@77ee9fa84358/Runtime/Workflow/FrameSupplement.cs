//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace easyar
{
    [Serializable]
    internal class FrameSupplement
    {
        public Application application;
        public Device device;
        public List<DeviceInput> deviceInput = new List<DeviceInput>();

        public static FrameSupplement Create(FrameSource source)
        {
            return new FrameSupplement
            {
                application = new Application
                {
                    product = UnityEngine.Application.productName,
                    unityVersion = UnityEngine.Application.unityVersion,
                    platform = UnityEngine.Application.platform.ToString(),
                    easyarSenseVersion = Engine.versionString(),
                    easyarSenseUnityPluginVersion = UnityPackage.Version,
                },
                device = new Device
                {
                    vioDevice = ARSessionFactory.DefaultName(source.GetType()).Replace(" ", ""),
                    operatingSystem = SystemInfo.operatingSystem,
                    deviceModel = SystemUtil.DeviceModel,
                    isHMD = source.IsHMD,
                },
            };
        }

        [Serializable]
        public class Application
        {
            public string product;
            public string unityVersion;
            public string platform;
            public string easyarSenseVersion;
            public string easyarSenseUnityPluginVersion;
        }

        [Serializable]
        public class Device
        {
            public string vioDevice;
            public string operatingSystem;
            public string deviceModel;
            public bool isHMD;
        }

        [Serializable]
        public class DeviceInput
        {
            public double frameTimestamp;
            public LocationResult location;
            public AccelerometerResult accelerometer;

            [Serializable]
            public class LocationResult
            {
                public double latitude;
                public double longitude;
                public double altitude;
                public double horizontalAccuracy;
                public double verticalAccuracy;
            }

            [Serializable]
            public class AccelerometerResult
            {
                public float x;
                public float y;
                public float z;
                public double timestamp;
            }
        }
    }

    internal class FrameSupplementRecorder : IDisposable
    {
        private FrameSupplement supplement;
        private StreamWriter writer;

        public FrameSupplementRecorder(string path, FrameSource source)
        {
            supplement = FrameSupplement.Create(source);
            writer = new StreamWriter(path, false, Encoding.UTF8, 8 * 1024);
            writer?.Write("{" +
                $"\"application\":{JsonUtility.ToJson(supplement.application)}" +
                "," +
                $"\"device\":{JsonUtility.ToJson(supplement.device)}" +
                "," +
                "\"deviceInput\":["
            );
        }

        public void Dispose()
        {
            writer?.Write("]}");
            writer?.Close();
            writer = null;
        }

        public void Record(double time, Optional<AccelerometerResult> acc, Optional<LocationResult> loc)
        {
            if (acc.OnNone && loc.OnNone) { return; }

            var input = new FrameSupplement.DeviceInput
            {
                frameTimestamp = time,
                accelerometer = acc.OnSome ? new FrameSupplement.DeviceInput.AccelerometerResult
                {
                    x = acc.Value.x,
                    y = acc.Value.y,
                    z = acc.Value.z,
                    timestamp = acc.Value.timestamp,
                } : null,
                location = loc.OnSome ? new FrameSupplement.DeviceInput.LocationResult
                {
                    latitude = loc.Value.latitude,
                    longitude = loc.Value.longitude,
                    altitude = loc.Value.altitude,
                    horizontalAccuracy = loc.Value.horizontalAccuracy,
                    verticalAccuracy = loc.Value.verticalAccuracy,
                } : null,
            };
            var str = supplement.deviceInput.Count == 0 ? string.Empty : ",";
            str += "{" +
                $"\"frameTimestamp\":{input.frameTimestamp}";
            if (acc.OnSome)
            {
                str += "," + $"\"accelerometer\":{JsonUtility.ToJson(input.accelerometer)}";
            }
            if (loc.OnSome)
            {
                str += "," + $"\"location\":{JsonUtility.ToJson(input.location)}";
            }
            str += "}";
            writer?.Write(str);
            supplement.deviceInput.Add(input);
        }
    }

    internal class FrameSupplementPlayer : IDisposable
    {
        [Obsolete]
        private static List<string> knownDeviceTypes;
        [Obsolete]
        private static List<string> knownHMDDeviceTypes;
        private FrameSupplement supplement;
        private bool isManualPaused;
        private bool isSystemPaused;
        private Thread thread;
        private bool finished;
        private Func<double> time;
        private int index;
        private double startTimestamp;
        private bool isHMD;
        private object timeLock = new object();

        public event Action<FrameSupplement.DeviceInput.AccelerometerResult> AccelerometerOutput;
        public event Action<FrameSupplement.DeviceInput.LocationResult> LocationOutput;

        public bool IsHMD => isHMD;

        public FrameSupplementPlayer()
        {
#pragma warning disable 612, 618
            knownHMDDeviceTypes = new List<string> { "Nreal", "Rokid", "RokidUMR", "RokidUXR", "Pico", "Qiyu", };
            knownDeviceTypes = new List<string> { "MotionTracker", "ARCore", "ARKit", "HuaweiAREngine", "AREngine", "CameraDevice", "ARFoundation", }.Concat(knownHMDDeviceTypes).ToList();
#pragma warning restore 612, 618
        }

        ~FrameSupplementPlayer()
        {
            Finish();
        }

        public void Dispose()
        {
            Finish();
            GC.SuppressFinalize(this);
        }

        public void Start(string path, Func<double> timeFunc)
        {
            if (!File.Exists(path)) { throw new FileNotFoundException(path); }
            var fileData = File.ReadAllText(path);
            supplement = JsonUtility.FromJson<FrameSupplement>(fileData);
            if (supplement.device == null || supplement.application == null || supplement.deviceInput == null)
            {
                throw new InvalidDataException("Invalid content: " + path);
            }

            var packageVer = supplement.application.easyarSenseUnityPluginVersion;
            if (!string.IsNullOrEmpty(packageVer) && UnityPackage.ParsedVersion > new Version(4, 7, 0, 3700))
            {
                isHMD = supplement.device.isHMD;
            }
            else
            {
#pragma warning disable 612, 618
                if (!knownDeviceTypes.Contains(supplement.device.vioDevice))
                {
                    throw new InvalidDataException($"Unknown VIO device type '{supplement.device.vioDevice}' from: {path}");
                }
                isHMD = knownHMDDeviceTypes.Contains(supplement.device.vioDevice);
#pragma warning restore 612, 618
            }

            if (supplement.deviceInput.Count <= 0)
            {
                if (!isHMD)
                {
                    throw new InvalidOperationException("empty deviceInput");
                }
                return;
            }

            time = timeFunc;

            Finish();
            index = 0;
            startTimestamp = supplement.deviceInput[0].frameTimestamp;
            isManualPaused = false;
            isSystemPaused = false;
            finished = false;
            thread = new Thread(() =>
            {
                double preT = 0;
                while (!finished)
                {
                    double t = 0;
                    Monitor.Enter(timeLock);
                    try
                    {
                        while (!finished && (isManualPaused || isSystemPaused))
                        {
                            Monitor.Wait(timeLock);
                        }
                        t = time();
                    }
                    finally
                    {
                        Monitor.Exit(timeLock);
                    }

                    try
                    {
                        if (!finished)
                        {
                            if (t < preT)
                            {
                                index = 0;
                                for (var i = index; i < supplement.deviceInput.Count - 1; ++i)
                                {
                                    if (supplement.deviceInput[i + 1].frameTimestamp - startTimestamp > t) { break; }
                                    index++;
                                }
                            }
                            preT = t;
                            for (var i = index; i < supplement.deviceInput.Count; ++i)
                            {
                                if (supplement.deviceInput[i].frameTimestamp - startTimestamp <= t)
                                {
                                    if (supplement.deviceInput[i].accelerometer != null && supplement.deviceInput[i].accelerometer.timestamp >= 0)
                                    {
                                        AccelerometerOutput?.Invoke(supplement.deviceInput[i].accelerometer);
                                    }
                                    if (supplement.deviceInput[i].location != null && supplement.deviceInput[i].location.latitude > -1000)
                                    {
                                        LocationOutput?.Invoke(supplement.deviceInput[i].location);
                                    }
                                    index++;
                                }
                                else
                                {
                                    Thread.Sleep((int)((supplement.deviceInput[i].frameTimestamp - startTimestamp - t) * 1000));
                                    break;
                                }
                            }

                            if (index >= supplement.deviceInput.Count)
                            {
                                Thread.Sleep(50);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                }
            });
            thread.Start();
        }

        public void Stop()
        {
            Finish();
        }

        public void Pause()
        {
            Monitor.Enter(timeLock);
            try
            {
                isManualPaused = true;
            }
            finally
            {
                Monitor.Exit(timeLock);
            }
        }

        public void Resume()
        {
            Monitor.Enter(timeLock);
            try
            {
                if (!isManualPaused) { return; }
                isManualPaused = false;
                if (isSystemPaused) { return; }
                Monitor.PulseAll(timeLock);
            }
            finally
            {
                Monitor.Exit(timeLock);
            }
        }

        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Monitor.Enter(timeLock);
                try
                {
                    isSystemPaused = true;
                }
                finally
                {
                    Monitor.Exit(timeLock);
                }
            }
            else
            {
                Monitor.Enter(timeLock);
                try
                {
                    if (!isSystemPaused) { return; }
                    isSystemPaused = false;
                    if (isManualPaused) { return; }
                    Monitor.PulseAll(timeLock);
                }
                finally
                {
                    Monitor.Exit(timeLock);
                }
            }
        }

        private void Finish()
        {
            if (thread == null || !thread.IsAlive)
            {
                return;
            }

            Monitor.Enter(timeLock);
            try
            {
                finished = true;
                isSystemPaused = false;
                isManualPaused = false;
                Monitor.PulseAll(timeLock);
            }
            finally
            {
                Monitor.Exit(timeLock);
            }
            thread.Join();
        }

    }

    internal interface IFrameSupplementFilter
    {
        event Action<ProximityLocationResult> ProximityLocationProvider;
        void Connect(VideoInputFramePlayer player, bool isHMD);
        void Disconnect(VideoInputFramePlayer player);
        void Connect(FrameSupplementPlayer player, bool isHMD);
        void Disconnect(FrameSupplementPlayer player);
    }
}
