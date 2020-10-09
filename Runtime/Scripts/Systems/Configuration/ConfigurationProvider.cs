using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Systems.Configuration
{
    /// <summary>
    /// Base helper class for implementing an <see cref="IConfigurationProvider"/>
    /// </summary>
    public abstract class ConfigurationProvider : IConfigurationProvider
    {
        protected ConfigurationProvider()
        {
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        protected IDictionary<string,string> Data { get; set; }

        /// <summary>
        /// Attempts to find a value with the given key, returns true if one is found, false otherwise.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool TryGet(string key, out string value)
            => this.Data.TryGetValue(key, out value);

        public virtual void Set(string key, string value)
            => this.Data[key] = value;

        public virtual void Load() { }


        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            var prefix = parentPath == null ? string.Empty : parentPath + ConfigurationPath.KeyDelimiter;

            return Data
                .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(kv => Segment(kv.Key, prefix.Length))
                .Concat(earlierKeys)
                .OrderBy(k => k, ConfigurationKeyComparer.Instance);
        }

        private static string Segment(string key, int prefixLength)
        {
            var indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
            return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
        }

        /// <summary>
        /// Triggers the reload change token and creates a new one.
        /// </summary>
        protected void OnReload() { }

        /// <summary>
        /// Generates a string representing this provider name and relevant details.
        /// </summary>
        /// <returns> The configuration name. </returns>
        public override string ToString() => $"{GetType().Name}";
    }
}
