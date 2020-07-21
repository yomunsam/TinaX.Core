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

        public static bool IsFolder(string path)
        {
            var system_path = Path.GetFullPath(path);
            return Directory.Exists(system_path);
        }

        /// <summary>
        /// 是否为子路径， 如果相等也返回false
        /// </summary>
        /// <param name="path1">路径1</param>
        /// <param name="path2">路径2</param>
        /// <param name="mutual">是否互相检测，如果为false，只检查path1是否为path2的子路径；如果为true，互相判断两者是否为对方的子路径</param>
        /// <returns></returns>
        public static bool IsSubpath(string path1, string path2, bool mutual = false)
        {
            string p1 = (path1.EndsWith("/") || path1.EndsWith("\\")) ? path1.Replace("\\", "/") : path1.Replace("\\", "/") + "/";
            string p2 = (path2.EndsWith("/") || path2.EndsWith("\\")) ? path2.Replace("\\", "/") : path2.Replace("\\", "/") + "/";

            if (p1 == p2) return false;

            if (mutual)
            {
                //相互判断
                return (p1.StartsWith(p2) || p2.StartsWith(p1));
            }
            else
            {
                //判断p1是否是p2的子路径
                return p1.StartsWith(p2);
            }

        }


        /// <summary>
        /// 是否为子路径或相同路径
        /// </summary>
        /// <param name="path1">路径1</param>
        /// <param name="path2">路径2</param>
        /// <param name="mutual">是否互相检测，如果为false，只检查path1是否为path2的子路径；如果为true，互相判断两者是否为对方的子路径</param>
        /// <returns></returns>
        public static bool IsSameOrSubPath(string path1, string path2, bool mutual = false)
        {
            string p1 = (path1.EndsWith("/") || path1.EndsWith("\\")) ? path1.Replace("\\", "/") : path1.Replace("\\", "/") + "/";
            string p2 = (path2.EndsWith("/") || path2.EndsWith("\\")) ? path2.Replace("\\", "/") : path2.Replace("\\", "/") + "/";

            if (p1 == p2) return true;

            if (mutual)
            {
                //相互判断
                return (p1.StartsWith(p2) || p2.StartsWith(p1));
            }
            else
            {
                //判断p1是否是p2的子路径
                return p1.StartsWith(p2);
            }
        }


    }
}
