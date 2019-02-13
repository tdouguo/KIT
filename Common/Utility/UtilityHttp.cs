// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;

namespace Kit
{
    public static partial class Utility
    {
        public static class Http
        {
            public static string EscapeString(string stringToEscape)
            {
                return Uri.EscapeDataString(stringToEscape);
            }

            public static string UnescapeString(string stringToUnescape)
            {
                return Uri.UnescapeDataString(stringToUnescape);
            }


            //        static string s_ReuqestInfo = string.Empty;

            //        /// <summary>
            //        /// 获取请求信息
            //        /// </summary>
            //        /// <returns></returns>
            //        public static string GetRequestInfo()
            //        {
            //            if (s_ReuqestInfo == string.Empty)
            //            {
            //                string deviceId = SystemInfo.deviceUniqueIdentifier;
            //                string deviceName = SystemInfo.deviceName;
            //                string deviceModel = SystemInfo.deviceModel;
            //                string processorType = SystemInfo.processorType;
            //                string processorCount = SystemInfo.processorCount.ToString();
            //                string memorySize = SystemInfo.systemMemorySize.ToString();
            //                string operatingSystem = SystemInfo.operatingSystem;
            //                string iOSGeneration = string.Empty;
            //                string iOSSystemVersion = string.Empty;
            //                string iOSVendorIdentifier = string.Empty;
            //#if UNITY_IOS && !UNITY_EDITOR
            //                iOSGeneration = UnityEngine.iOS.Device.generation.ToString();
            //                iOSSystemVersion = UnityEngine.iOS.Device.systemVersion;
            //                iOSVendorIdentifier = UnityEngine.iOS.Device.vendorIdentifier ?? string.Empty;
            //#endif
            //                string gameVersion = GameFramework.Version.GameVersion;
            //                string platform = Application.platform.ToString();
            //                string language = FrameworkEntry.Localization.Language.ToString();
            //                string unityVersion = Application.unityVersion;
            //                string installMode = Application.installMode.ToString();
            //                string sandboxType = Application.sandboxType.ToString();
            //                string screenWidth = Screen.width.ToString();
            //                string screenHeight = Screen.height.ToString();
            //                string screenDpi = Screen.dpi.ToString();
            //                string screenOrientation = Screen.orientation.ToString();
            //                string screenResolution = Utility.Text.Format("{0} x {1} @ {2}Hz", Screen.currentResolution.width.ToString(), Screen.currentResolution.height.ToString(), Screen.currentResolution.refreshRate.ToString());
            //                string useWifi = (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork).ToString();

            //                string graphicsDeviceID = SystemInfo.graphicsDeviceID.ToString();    // 图形设备标识Id
            //                string graphicsDeviceName = SystemInfo.graphicsDeviceName;       // 图形设备名称
            //                string graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();    // 图形设备类型
            //                string graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;   // 图形设备供应商
            //                string graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID.ToString();   // 图形设备供应商标识Id
            //                string graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;  // 图形设备API 类型和驱动程序版本
            //                string graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();    // 图形设备内存大小 

            //                JsonData headerJson = new JsonData();
            //                headerJson["DeviceId"] = deviceId;
            //                headerJson["DeviceName"] = deviceName;
            //                headerJson["DeviceModel"] = deviceModel;
            //                headerJson["ProcessorType"] = processorType;
            //                headerJson["ProcessorCount"] = processorCount;
            //                headerJson["MemorySize"] = memorySize;
            //                headerJson["OperatingSystem"] = operatingSystem;
            //                headerJson["IOSGeneration"] = iOSGeneration;
            //                headerJson["IOSSystemVersion"] = iOSSystemVersion;
            //                headerJson["IOSVendorIdentifier"] = iOSVendorIdentifier;
            //                headerJson["GameVersion"] = gameVersion;
            //                headerJson["Platform"] = platform;
            //                headerJson["Language"] = language;
            //                headerJson["UnityVersion"] = unityVersion;
            //                headerJson["InstallMode"] = installMode;
            //                headerJson["SandboxType"] = sandboxType;
            //                headerJson["ScreenWidth"] = screenWidth;
            //                headerJson["ScreenHeight"] = screenHeight;
            //                headerJson["ScreenDPI"] = screenDpi;
            //                headerJson["ScreenOrientation"] = screenOrientation;
            //                headerJson["ScreenResolution"] = screenResolution;
            //                headerJson["UseWifi"] = useWifi;
            //                headerJson["GraphicsDeviceID"] = graphicsDeviceID;
            //                headerJson["GraphicsDeviceName"] = graphicsDeviceName;
            //                headerJson["GraphicsDeviceType"] = graphicsDeviceType;
            //                headerJson["GraphicsDeviceVendor"] = graphicsDeviceType;
            //                headerJson["GraphicsDeviceVendorID"] = graphicsDeviceVendorID;
            //                headerJson["GraphicsDeviceVersion"] = graphicsDeviceVersion;
            //                headerJson["GraphicsMemorySize"] = graphicsMemorySize;

            //                headerJson["os_type"] = FrameworkEntry.BuiltinData.BuildInfo.OsType;
            //                headerJson["version"] = FrameworkEntry.BuiltinData.BuildInfo.AppVersion;

            //                s_ReuqestInfo = headerJson.ToJson();
            //                Log.Info("[WebRequest] Request Info : " + s_ReuqestInfo);
            //            }
            //            return s_ReuqestInfo;
            //        }
        }
    }
}
