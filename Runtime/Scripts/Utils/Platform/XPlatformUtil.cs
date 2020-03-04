using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX;
using UnityEngine;

namespace TinaX.Utils
{
    public static class XPlatformUtil
    {
        public static string GetNameText(XRuntimePlatform xPlatform)
        {
            switch (xPlatform)
            {
                default:
                    return xPlatform.ToString().ToLower();
                case XRuntimePlatform.Windows:
                    return "windows_amd64"; //标注amd64， 区别于Windows后搞出来的arm64
                case XRuntimePlatform.UniversalWindowsPlatform:
                    return "uwp";
                case XRuntimePlatform.Linux:
                    return "gnu_linux_64";
                case XRuntimePlatform.OSX:
                    return "osx";
                case XRuntimePlatform.iOS:
                    return "ios";
                case XRuntimePlatform.Android:
                    return "android";
                case XRuntimePlatform.XBox:
                    return "xbox";
                case XRuntimePlatform.PS4:
                    return "ps4";
                case XRuntimePlatform.NSwitch:
                    return "switch";
                case XRuntimePlatform.Windows32:
                    return "windows_x86_32";
            }

        }
    
        public static XRuntimePlatform GetXRuntimePlatform(RuntimePlatform platform)
        {
            switch (platform)
            {
                default:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return XRuntimePlatform.Windows;

                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return XRuntimePlatform.OSX;

                case RuntimePlatform.WSAPlayerARM:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerX86:
                    return XRuntimePlatform.UniversalWindowsPlatform;

                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    return XRuntimePlatform.Linux;

                case RuntimePlatform.IPhonePlayer:
                    return XRuntimePlatform.iOS;

                case RuntimePlatform.Android:
                    return XRuntimePlatform.Android;

                case RuntimePlatform.XboxOne:
                    return XRuntimePlatform.XBox;

                case RuntimePlatform.Switch:
                    return XRuntimePlatform.NSwitch;
            }
        }
    }
}
