/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */
using System;
using System.Collections.Generic;

namespace TinaX.Systems.Configuration
{
    /// <summary>
    /// 命令行 - 配置提供者
    /// </summary>
    internal class CommandLineConfigurationProvider : ConfigurationProvider
    {
        private ICommandLineArgs m_Args;

        public CommandLineConfigurationProvider(ICommandLineArgs args)
        {
            this.m_Args = args;
        }

        public override void Load()
        {
            var data = new Dictionary<string, string>(this.m_Args.GetAllKeyValueArgs(), StringComparer.OrdinalIgnoreCase);
            var single_data = this.m_Args.GetAllSingleArgs();
            foreach(var item in single_data)
            {
                if (!data.ContainsKey(item))
                {
                    data.Add(item, "true");
                }
            }

            this.Data = data;
        }

        

        ///// <summary>
        ///// 获取String格式
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public bool TryGet(string key, out string value)
        //    => m_Args.TryGetValue(key, out value);

        //void IConfigurationProvider.Set(string key, string value) { } //不作实现，命令行配置参数应该是只读的
    }
}
