using System;
using System.Collections.Generic;

namespace TinaX.Core.ReflectionProvider
{
#nullable enable
    public class ReflectionProviderManager
    {
        private readonly List<IReflectionProvider> m_ReflectionProviders = new List<IReflectionProvider>();

        public void Register(IReflectionProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (!m_ReflectionProviders.Contains(provider))
                m_ReflectionProviders.Add(provider);
        }

        public void Remove(IReflectionProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (m_ReflectionProviders.Contains(provider))
                m_ReflectionProviders.Remove(provider);
        }

        public bool TryGetType(ref object sourceObject, out Type? type)
        {
            for (int i = 0; i < m_ReflectionProviders.Count; i++)
            {
                if (m_ReflectionProviders[i].TryGetType(ref sourceObject, out type))
                    return true;
            }

            type = default;
            return false;
        }
    }
#nullable restore
}
