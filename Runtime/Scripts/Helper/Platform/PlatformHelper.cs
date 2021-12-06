using TinaX.Core.Platforms;
using UnityEngine;

namespace TinaX.Core.Helper.Platform
{
    public static class PlatformHelper
    {
        //2021.11 此时此刻市面上常见的Unity版本，对 C# 8 的 模式匹配兼容不完美，所以还算先传统的switch吧


        public static string GetName(XRuntimePlatform platform)
        {
            //return platform switch
            //{
            //    XRuntimePlatform.Windows => "windows_x64",
            //    XRuntimePlatform.UniversalWindowsPlatform => "uwp",
            //    XRuntimePlatform.Linux => "linux_x64",
            //    XRuntimePlatform.OSX => "macos",
            //    XRuntimePlatform.Android => "android",
            //    XRuntimePlatform.iOS => "ios",
            //    XRuntimePlatform.XBox => "xbox",
            //    XRuntimePlatform.PS4 => "ps4",
            //    XRuntimePlatform.NSwitch => "switch",
            //    XRuntimePlatform.Windows32 => "windows_x86",
            //    _ => platform.ToString().ToLower()
            //};


            switch (platform)
            {
                default:
                    return platform.ToString().ToLower();

                #region Microsoft
                case XRuntimePlatform.Windows:
                    return "windows_x64";
                case XRuntimePlatform.UniversalWindowsPlatform_x64:
                    return "uwp_x64";
                case XRuntimePlatform.UniversalWindowsPlatform_x86:
                    return "uwp_x86";
                case XRuntimePlatform.UniversalWindowsPlatform_arm:
                    return "uwp_arm";
                case XRuntimePlatform.XBox:
                    return "xbox";
                #endregion

                case XRuntimePlatform.Linux:
                    return "linux_x64";

                #region Apple
                case XRuntimePlatform.MacOS:
                    return "macos";
                case XRuntimePlatform.iOS:
                    return "ios";
                case XRuntimePlatform.tvOS:
                    return "tvos";
                #endregion

                case XRuntimePlatform.Android:
                    return "android";
                
                case XRuntimePlatform.PS4:
                    return "ps4";
                case XRuntimePlatform.PS5:
                    return "ps5";

                case XRuntimePlatform.NSwitch:
                    return "switch";
            }
        }

        public static XRuntimePlatform GetXRuntimePlatform(RuntimePlatform platform)
        {
            switch(platform)
            {
                default:
                    return XRuntimePlatform.Unknow;

                #region Microsoft
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return XRuntimePlatform.Windows;

                case RuntimePlatform.WSAPlayerX64:
                    return XRuntimePlatform.UniversalWindowsPlatform_x64;
                case RuntimePlatform.WSAPlayerX86:
                    return XRuntimePlatform.UniversalWindowsPlatform_x86;
                case RuntimePlatform.WSAPlayerARM:
                    return XRuntimePlatform.UniversalWindowsPlatform_arm;

                case RuntimePlatform.XboxOne:
                case RuntimePlatform.GameCoreXboxOne:
                case RuntimePlatform.GameCoreXboxSeries:
                    return XRuntimePlatform.XBox;
                #endregion

                #region Apple
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return XRuntimePlatform.MacOS;

                case RuntimePlatform.IPhonePlayer:
                    return XRuntimePlatform.iOS;

                case RuntimePlatform.tvOS:
                    return XRuntimePlatform.tvOS;
                #endregion

                #region Linux
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    return XRuntimePlatform.Linux;
                #endregion

                case RuntimePlatform.Android:
                    return XRuntimePlatform.Android;
                

                case RuntimePlatform.PS4:
                    return XRuntimePlatform.PS4;
                case RuntimePlatform.PS5:
                    return XRuntimePlatform.PS5;
            }
        }

    }
}
