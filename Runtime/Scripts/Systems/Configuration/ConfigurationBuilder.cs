using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinaX.Systems.Configuration.Internal;

namespace TinaX.Systems.Configuration
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        public IList<IConfigurationSource> Sources { get; } = new List<IConfigurationSource>();

        /// <summary>
        /// Adds a new configuration source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!Sources.Contains(source))
                Sources.Add(source);
            return this;
        }

        public async Task<IConfigurationRoot> BuildAsync()
        {
            var providers = new List<IConfigurationProvider>();
            foreach(var source in Sources)
            {
                var provider = await source.BuildAsync(this); //注意，ConfigurationRoot需要确保最终Providers的顺序与代码注册时一致，所以这里最好一个个依次await,
                providers.Add(provider);
            }
            return new ConfigurationRoot(providers);
        }
    }
}
