namespace TinaX.Core.Platforms
{
    public enum XRuntimePlatform : short
    {
        Unknow = 0,

        #region Microsoft
        /// <summary>
        /// Microsoft Windows (Desktop)
        /// </summary>
        Windows = 100,

        Windows32 = 101,

        /// <summary>
        /// Microsoft Universal Windows Platform (UWP)
        /// </summary>
        UniversalWindowsPlatform_x64 = 111,
        UniversalWindowsPlatform_x86 = 112,
        UniversalWindowsPlatform_arm = 113,

        XBox = 121,

        #endregion

        /// <summary>
        /// GNU Linux
        /// </summary>
        Linux = 201,

        #region Apple

        /// <summary>
        /// Apple OSX
        /// </summary>
        MacOS = 301,
        /// <summary>
        /// Apple iOS
        /// </summary>
        iOS = 311,

        /// <summary>
        /// Apple tvOS
        /// </summary>
        tvOS = 321,

        #endregion

        /// <summary>
        /// Google Android
        /// </summary>
        Android = 401,

        #region SONY
        /// <summary>
        /// Sony Playstation 4
        /// </summary>
        PS4 = 501,
        PS5 = 502,
        #endregion

        /// <summary>
        /// Nintendo Switch
        /// </summary>
        NSwitch = 601,


    }
}
