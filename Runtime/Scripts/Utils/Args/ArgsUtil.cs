using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX
{
    public static class ArgsUtil
    {
        /// <summary>
        /// 将一串字符串解析成Args数组
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> ParseArgsText(string source)
        {
            if (source.IsNullOrEmpty())
                return Array.Empty<string>();
            return source.Split('"')
                .Select((element, index) => index % 2 == 0
                    ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { element })
                .SelectMany(element => element)
                .ToList();
        }
    }
}
