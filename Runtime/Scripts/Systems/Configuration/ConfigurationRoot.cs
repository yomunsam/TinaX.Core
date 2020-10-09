using System;
using System.Collections.Generic;
using System.Linq;

namespace TinaX.Systems.Configuration.Internal
{
    /// <summary>
    /// TinaX.Core 内置配置服务
    /// </summary>
    public class ConfigurationRoot : IConfigurationRoot , IDisposable
    {

        public string this[string key]
        {
            get
            {
                return this.GetString(key, null);
            }

            set
            {
                if (!m_ConfigurationProviders.Any())
                {
                    throw new InvalidOperationException("No Configuration Provider");
                }

                foreach(var provider in m_ConfigurationProviders)
                {
                    provider.Set(key, value);
                }
            }
        }

        private IList<IConfigurationProvider> m_ConfigurationProviders;

        public IEnumerable<IConfigurationProvider> Providers => m_ConfigurationProviders;

        public ConfigurationRoot(IList<IConfigurationProvider> providers)
        {
            if (providers == null)
                throw new ArgumentNullException(nameof(providers));

            m_ConfigurationProviders = providers;
            foreach(var p in m_ConfigurationProviders)
            {
                p.Load();
            }
        }


        /// <summary>
        /// 添加配置提供者
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        internal IConfiguration AddConfigurationProvider(IConfigurationProvider provider)
        {
            if (!m_ConfigurationProviders.Contains(provider))
                m_ConfigurationProviders.Add(provider);

            return this;
        }


        internal string GetString(string key, string defaultValue = null)
        {
            int len = m_ConfigurationProviders.Count;
            if (len < 1)
                return defaultValue;

            for (int i = m_ConfigurationProviders.Count - 1; i >= 0; i--)
            {
                if (m_ConfigurationProviders[i].TryGet(key, out string _v))
                {
                    return _v;
                }
            }

            return defaultValue;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
            => this.GetChildrenImplementation(null);

        public IConfigurationSection GetSection(string key)
            => new ConfigurationSection(this, key);


        public void Dispose()
        {
            foreach(var provider in m_ConfigurationProviders)
            {
                (provider as IDisposable)?.Dispose();
            }
        }

    }
}
