using System.Collections.Generic;

namespace TinaX.Systems.Configuration.Internal
{
    /// <summary>
    /// TinaX.Core 内置配置服务
    /// </summary>
    public class Configuration : IConfiguration
    {

        public string this[string key]
        {
            get
            {
                return this.GetString(key, null);
            }
        }

        private List<IConfigurationProvider> m_ConfigurationProviders = new List<IConfigurationProvider>();


        /// <summary>
        /// 添加配置提供者
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IConfiguration AddConfigurationProvider(IConfigurationProvider provider)
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
                if (m_ConfigurationProviders[i].TryGetString(key, out string _v))
                {
                    return _v;
                }
            }

            return defaultValue;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            int colon_index = key.IndexOf(':');
        }
    }
}
