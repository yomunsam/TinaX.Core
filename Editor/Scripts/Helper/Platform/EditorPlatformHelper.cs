using TinaX.Core.Platforms;

namespace TinaXEditor.Core.Helper.Platform
{
    public static class EditorPlatformHelper
    {
        public static UnityEditor.BuildTarget GetBuildTarget(XRuntimePlatform platform)
        {
            switch (platform)
            {
                default:
                    return UnityEditor.BuildTarget.NoTarget;

                #region Microsoft
                case XRuntimePlatform.Windows:
                    return UnityEditor.BuildTarget.StandaloneWindows64;
                case XRuntimePlatform.Windows32:
                    return UnityEditor.BuildTarget.StandaloneWindows;

                case XRuntimePlatform.UniversalWindowsPlatform_x64:
                case XRuntimePlatform.UniversalWindowsPlatform_x86:
                case XRuntimePlatform.UniversalWindowsPlatform_arm:
                    return UnityEditor.BuildTarget.WSAPlayer;

                case XRuntimePlatform.XBox:
                    return UnityEditor.BuildTarget.XboxOne;
                #endregion

                case XRuntimePlatform.Linux:
                    return UnityEditor.BuildTarget.StandaloneLinux64;

                #region Apple
                case XRuntimePlatform.MacOS:
                    return UnityEditor.BuildTarget.StandaloneOSX;

                case XRuntimePlatform.iOS:
                    return UnityEditor.BuildTarget.iOS;

                case XRuntimePlatform.tvOS:
                    return UnityEditor.BuildTarget.tvOS;
                #endregion

                case XRuntimePlatform.Android:
                    return UnityEditor.BuildTarget.Android;

                #region SONY
                case XRuntimePlatform.PS4:
                    return UnityEditor.BuildTarget.PS4;
                case XRuntimePlatform.PS5:
                    return UnityEditor.BuildTarget.PS5;
                #endregion

                case XRuntimePlatform.NSwitch:
                    return UnityEditor.BuildTarget.Switch;
            }
        }

        public static UnityEditor.BuildTargetGroup GetBuildTargetGroup(XRuntimePlatform platform)
        {
            switch (platform)
            {
                default:
                    return UnityEditor.BuildTargetGroup.Unknown;

                #region Microsoft
                case XRuntimePlatform.Windows:
                case XRuntimePlatform.Windows32:
                    return UnityEditor.BuildTargetGroup.Standalone;

                case XRuntimePlatform.UniversalWindowsPlatform_x64:
                case XRuntimePlatform.UniversalWindowsPlatform_arm:
                case XRuntimePlatform.UniversalWindowsPlatform_x86:
                    return UnityEditor.BuildTargetGroup.WSA;

                case XRuntimePlatform.XBox:
                    return UnityEditor.BuildTargetGroup.XboxOne;
                #endregion

                #region Apple
                case XRuntimePlatform.MacOS:
                    return UnityEditor.BuildTargetGroup.Standalone;
                case XRuntimePlatform.iOS:
                    return UnityEditor.BuildTargetGroup.iOS;
                case XRuntimePlatform.tvOS:
                    return UnityEditor.BuildTargetGroup.tvOS;
                #endregion

                case XRuntimePlatform.Android:
                    return UnityEditor.BuildTargetGroup.Android;

                case XRuntimePlatform.PS4:
                    return UnityEditor.BuildTargetGroup.PS4;
                case XRuntimePlatform.PS5:
                    return UnityEditor.BuildTargetGroup.PS5;

                case XRuntimePlatform.NSwitch:
                    return UnityEditor.BuildTargetGroup.Switch;
            }
        }
    }
}
