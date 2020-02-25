using TinaX;

namespace TinaXEditor.Utils
{
    public static class XPlatformEditorUtil
    {
        public static UnityEditor.BuildTarget GetBuildTarget(XRuntimePlatform xPlatform)
        {
            switch (xPlatform) // !! Visual Studio 会给这地方建议改成C#8的模式匹配表达式，不要动，Unity目前只支持C#7.2语法！！
            {
                default:
                    return UnityEditor.BuildTarget.NoTarget;
                case XRuntimePlatform.Windows:
                    return UnityEditor.BuildTarget.StandaloneWindows64;
                case XRuntimePlatform.UniversalWindowsPlatform:
                    return UnityEditor.BuildTarget.WSAPlayer;
                case XRuntimePlatform.Linux:
                    return UnityEditor.BuildTarget.StandaloneLinux64;
                case XRuntimePlatform.OSX:
                    return UnityEditor.BuildTarget.StandaloneOSX;
                case XRuntimePlatform.iOS:
                    return UnityEditor.BuildTarget.iOS;
                case XRuntimePlatform.Android:
                    return UnityEditor.BuildTarget.Android;
                case XRuntimePlatform.XBox:
                    return UnityEditor.BuildTarget.XboxOne;
                case XRuntimePlatform.PS4:
                    return UnityEditor.BuildTarget.PS4;
                case XRuntimePlatform.NSwitch:
                    return UnityEditor.BuildTarget.Switch;
                case XRuntimePlatform.Windows32:
                    return UnityEditor.BuildTarget.StandaloneWindows;
            }
        }
    }
}
