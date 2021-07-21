using System;
using System.Collections.Generic;
using System.Linq;

namespace TinaX.Behaviours.Internal
{
    public class XBootstrapManager
    {
        private readonly List<IXBootstrap> m_XBootstraps = new List<IXBootstrap>();

        public XBootstrapManager()
        {
            var _xbs_type = typeof(IXBootstrap);
            var types_ixbootstrap = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(_xbs_type)))
                .ToArray();
            foreach(var type in types_ixbootstrap)
            {
                m_XBootstraps.Add((IXBootstrap)Activator.CreateInstance(type));
            }
        }

        public IList<IXBootstrap> XBootstraps => m_XBootstraps;
    }
}
