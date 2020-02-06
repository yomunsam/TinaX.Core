using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TinaX.IO
{
    public static class XPath
    {
        /// <summary>
        /// Get Extension in path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="multiple">Multiple extension, eg: "hello.lua.txt" , if true, return ".lua.txt", if false , return ".txt"</param>
        /// <returns></returns>
        public static string GetExtension(string path,bool multiple = false)
        {
            if(!multiple)
                return Path.GetExtension(path);
            else
            {
                if (path.IsNullOrEmpty()) return string.Empty;

                var _path = path.Replace("\\", "/");
                int last_slash_index = _path.LastIndexOf("/");
                int first_dot_index = _path.IndexOf('.', (last_slash_index > -1) ? last_slash_index: 0);
                if (first_dot_index == -1)
                    return string.Empty;
                else
                    return path.Substring(first_dot_index, _path.Length - first_dot_index);

            }
        }
    }
}
