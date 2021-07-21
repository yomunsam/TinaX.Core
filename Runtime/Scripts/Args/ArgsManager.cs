/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <me@yomunchan.moe> <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */


/*
 * 备注一些要说明的东西：
 * 1. 如果命令行里有用到字符串的引号格式的话：xxx "xxxx" 这种，.net底层会自己处理，不需要我们管
 */

using System.Collections.Generic;

namespace TinaX.Internal
{
    /// <summary>
    /// 用于处理命令行启动参数
    /// Used to process command line startup parameters
    /// </summary>
    public class ArgsManager : ICommandLineArgs
    {
        /// <summary>
        /// 存放以key-value形式传入的参数
        /// </summary>
        private Dictionary<string, string> m_ArgsKeyValue = new Dictionary<string, string>();

        /// <summary>
        /// 存放非key-value的参数
        /// </summary>
        private List<string> m_ArgsSingle = new List<string>();

        //处理Args传入的主要方法
        public void AddArgs(string[] args)
        {
            int args_len = args.Length;
            //处理成双成对 | handle key-value args
            List<int> kv_index = new List<int>(); //这里存储在key-value判断的过程中被占用掉的index
            string key = string.Empty;
            string value = string.Empty;

            for (int i = 0; i < args_len; i++)
            {
                var arg = args[i];
                //key-value在同一个数组item里，eg: --name="Yui" | -name="Azusa"
                if (this.ParseArg_KV_Merged(arg, out key, out value))
                {
                    kv_index.Add(i);
                    m_ArgsKeyValue.AddOrOverride(key, value);
                }

                //判断 key-value分散在两个数组里的情况， eg: --name= "Tsumugi" | -name= "Mio"
                //达成这种判断的前置条件，1. 它不能是数组的最后一个成员
                if (i + 1 < args_len && !kv_index.Contains(i) && !kv_index.Contains(i + 1))
                {
                    if (this.ParseArg_KV_Two(arg, args[i + 1], out key, out value))
                    {
                        kv_index.Add(i);
                        kv_index.Add(i + 1);
                        m_ArgsKeyValue.AddOrOverride(key, value);
                    }
                }

                //判断 key-value分散在三个数组里的情况, eg: --name = "Ritsu" | -name = "Sawako"
                //达成这种判断的前置条件：1. 它后面至少要有两个数组成员
                if (i + 2 < args_len && !kv_index.Contains(i) && !kv_index.Contains(i + 1) && !kv_index.Contains(i + 2))
                {
                    if (this.ParseArg_KV_Three(arg, args[i + 1], args[i + 2], out key, out value))
                    {
                        kv_index.Add(i);
                        kv_index.Add(i + 1);
                        kv_index.Add(i + 2);
                        m_ArgsKeyValue.AddOrOverride(key, value);
                    }
                }

            }

            //剩下的，没有被k/v判断占用掉的东西，都存到list里
            for (int i = 0; i < args_len; i++)
            {
                if (!kv_index.Contains(i))
                {
                    var arg = args[i];
                    if (arg.StartsWith("-"))
                    {
                        if (arg.StartsWith("--"))
                            m_ArgsSingle.AddIfNotExist(arg.Substring(2, arg.Length - 2));
                        else
                            m_ArgsSingle.AddIfNotExist(arg.Substring(1, arg.Length - 1));

                        continue;
                    }
                    m_ArgsSingle.AddIfNotExist(args[i]);
                }
            }

        }

        public bool TryGetValue(string key, out string value) => m_ArgsKeyValue.TryGetValue(key, out value);

        public string GetValue(string key, string defaultValue = null)
        {
            if (this.TryGetValue(key, out var v))
                return v;
            else
                return defaultValue;
        }

        public bool IsExistKey(string key) => m_ArgsKeyValue.ContainsKey(key);

        public bool IsExistSingle(string name) => m_ArgsSingle.Contains(name);

        public IDictionary<string, string> GetAllKeyValueArgs() => this.m_ArgsKeyValue;

        public IList<string> GetAllSingleArgs() => this.m_ArgsSingle;


        public bool GetBool(string key)
        {
            if (m_ArgsKeyValue.TryGetValue(key, out var _v))
            {
                if (bool.TryParse(_v, out bool _b))
                    return _b;
            }
            if (m_ArgsSingle.Contains(key))
                return true;
            return false;
        }

        /// <summary>
        /// key-value在同一个数组item里，eg: --name="yui" | -name="Azusa"
        /// 如果命中了这种形式的判断，就返回true, 并out传出解析结果，否则返回false
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ParseArg_KV_Merged(string arg, out string key, out string value)
        {
            string _arg = arg;
            if (!_arg.StartsWith("-")) // 以"-"开头？
            {
                key = string.Empty;
                value = string.Empty;
                return false;
            }

            if (_arg.StartsWith("--")) //以"--"开头
                _arg = _arg.Substring(2, _arg.Length - 2);
            else
                _arg = _arg.Substring(1, _arg.Length - 1);

            //判断中间是否有等于号
            int equalIndex = _arg.IndexOf('=');
            if (equalIndex == -1 | equalIndex == 0 | equalIndex == _arg.Length - 1) //如果没有等于号、等于号在开头 、等于号在结尾 ，都不符合我们要判断的形式
            {
                key = string.Empty;
                value = string.Empty;
                return false;
            }

            var kv_arr = _arg.Split('=');
            if (kv_arr.Length < 2)
            {
                key = string.Empty;
                value = string.Empty;
                return false;
            }
            key = kv_arr[0];
            value = kv_arr[1];
            return true;
        }

        /// <summary>
        /// 判断 key-value分散在两个数组里的情况， eg: --name= "Tsumugi" | -name= "Mio"
        /// 如果命中了这种形式的判断，就返回true, 并out传出解析结果，否则返回false
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="arg_next"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ParseArg_KV_Two(string arg, string arg_next, out string key, out string value)
        {
            string _arg = arg;
            if (!_arg.StartsWith("-")) // 以"-"开头？
            {
                key = string.Empty;
                value = string.Empty;
                return false;
            }

            if (_arg.StartsWith("--")) //以"--"开头
                _arg = _arg.Substring(2, _arg.Length - 2);
            else
                _arg = _arg.Substring(1, _arg.Length - 1);

            //这种形式，等号应该在最右边
            if (!_arg.EndsWith("="))
            {
                //不符合哇
                key = string.Empty;
                value = string.Empty;
                return false;
            }

            key = _arg.Substring(0, _arg.Length - 1);
            value = arg_next;
            return true;
        }

        /// <summary>
        /// 判断 key-value分散在三个数组里的情况， eg: --name = "Ritsu" | -name = "Sawako"
        /// 如果命中了这种形式的判断，就返回true, 并out传出解析结果，否则返回false
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="arg_next"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ParseArg_KV_Three(string arg, string arg_middle, string arg_last, out string key, out string value)
        {
            string _arg = arg;
            if (!_arg.StartsWith("-")) // 以"-"开头？
            {
                key = string.Empty;
                value = string.Empty;
                return false;
            }

            if (_arg.StartsWith("--")) //以"--"开头
                _arg = _arg.Substring(2, _arg.Length - 2);
            else
                _arg = _arg.Substring(1, _arg.Length - 1);

            //这种形式，中间一个arg应该是等号
            if (!arg_middle.Equals("="))
            {
                //不符合哇
                key = string.Empty;
                value = string.Empty;
                return false;
            }
            key = _arg;
            value = arg_last;
            return true;
        }
    }
}
