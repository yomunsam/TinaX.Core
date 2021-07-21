using System;
using System.Collections.Generic;
using TinaX.Systems.Configuration.Internal;

namespace TinaX.Systems.Configuration
{
    public class ConfigurationSection : IConfigurationSection
    {
        private readonly IConfigurationRoot _root;
        private readonly string _path;
        private string _key;

        public ConfigurationSection(IConfigurationRoot root, string path)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            _path = path;
            _root = root;
        }

        public string this[string key]
        {
            get => _root[ConfigurationPath.Combine(Path, key)];
            set
            {
                _root[ConfigurationPath.Combine(Path, key)] = value;
            }
        }

        public string Key
        {
            get
            {
                if (_key == null)
                    _key = ConfigurationPath.GetSectionKey(_path); //key是路径的最后一段，惰性计算
                return _key;
            }
        }

        public string Value
        {
            get => _root[Path];
            set
            {
                _root[Path] = value;
            }
        }

        public string Path => _path;

        public IConfigurationSection GetSection(string key) => _root.GetSection(ConfigurationPath.Combine(Path, key));

        public IEnumerable<IConfigurationSection> GetChildren() => _root.GetChildrenImplementation(Path);


    }
}
