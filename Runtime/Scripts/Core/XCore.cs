using TinaX.Catlib;
using TinaX.Container;
using UnityEngine;

namespace TinaX
{
    public class XCore : IXCore
    {
        private readonly ServiceContainer m_ServiceContainer;
        public XCore()
        {
            m_ServiceContainer = new ServiceContainer(this);
        }

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

        #region ServiceContainer
        public IServiceContainer Services => m_ServiceContainer;
        #endregion

    }
}
