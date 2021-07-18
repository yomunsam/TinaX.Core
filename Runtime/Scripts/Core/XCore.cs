using TinaX.Container;
using UnityEngine;

namespace TinaX
{
    public class XCore : IXCore
    {
        #region Builds

        #endregion

        #region Instances

        private static IXCore _MainInstance;
        private static object _lock_obj = new object();
        public static IXCore MainInstance
        {
            get => _MainInstance;
            set
            {
                Debug.LogFormat("[{0}]The singleton \"MainInstance\" is modified: {1} -> {2}",
                    nameof(XCore),
                    _MainInstance == null ? "*Empty" : _MainInstance.GetHashCode().ToString(),
                    value == null ? "*Empty" : value.GetHashCode().ToString());
                _MainInstance = value;
            }
        }
        #endregion

        #region CreateInstances

        public static IXCore CreateDefault()
        {
            var core = new XCore();

            return core;
        }

        #endregion


        #region Status

        public bool IsRunning { get; private set; }

        #endregion

    }
}
