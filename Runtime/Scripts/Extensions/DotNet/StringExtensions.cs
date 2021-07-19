/*
 * TinaX.Core Library
 * Copy Right (C) Nekonya Studio | nekonya.io
 * 
 * 本页代码部分参考：https://github.com/CatLib/Core/blob/2.0/src/CatLib.Core/Util/Str.cs
 */

using System;
using System.Text;
using System.Text.RegularExpressions;
using TinaX.Helper.String;
using TinaX.Utils.Encry;

namespace TinaX
{
    public static class StringExtensions
    {
        /// <summary>
        /// 是否为邮箱地址
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public static bool IsMail(this string _string)
        {
            return Regex.IsMatch(_string,
                @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$");
        }

        /// <summary>
        /// 高级比较，可设定是否忽略大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        /// <summary>
        /// 是否含有中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IncludeChinese(this string str)
        {
            bool flag = false;
            foreach (var a in str)
            {
                if (a >= 0x4e00 && a <= 0x9fbb)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// Whitout dot | 没有"."号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidFileName(this string str)
        {
            if (str.IsNullOrEmpty()) return false;
            return !Regex.IsMatch(str, @"\W+");
        }

        /// <summary>
        /// Reverse specified string. | 反转字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            return CatLib.Util.Str.Reverse(str);
        }

        /// <summary>
        /// 是否为空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// string Base64加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Obsolete("Use EncodeBase64()")]
        public static string ToBase64(this string str)
        {
            var b = System.Text.Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(b);
        }

        [Obsolete("Use DecodeBase64()")]
        public static string Base64ToStr(this string str)
        {
            var b = Convert.FromBase64String(str);
            return System.Text.Encoding.Default.GetString(b);
        }

        /// <summary>
        /// UTF8 string to base64 | 字符串编码到Base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeBase64(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// UTF8 from base64 to string | Base64文本解码到字符串
        /// </summary>
        /// <param name="base64_str"></param>
        /// <returns></returns>
        public static string DecodeBase64(this string base64_str)
        {
            var b = Convert.FromBase64String(base64_str);
            return Encoding.UTF8.GetString(b);
        }

        public static string GetMD5(this string str, bool lower = true, bool shortMD5 = false)
        {
            return EncryUtil.GetMD5(str, lower, shortMD5);
        }

        /// <summary>
        /// UTF8 string encode to bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str)
        {
            try
            {
                return Encoding.UTF8.GetBytes(str);
            }
            catch (EncoderFallbackException e)
            {
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Does the current character conform to a given regular expression | 测试当前字符是否符合给定的正则表达式。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool Is(this string str, string pattern)
        {
            return pattern.IsNullOrEmpty() || Regex.IsMatch(str, "^" + pattern.AsteriskWildcard() + "$");
        }

        /// <summary>
        /// Translate the specified string into an asterisk match expression. | 
        /// </summary>
        /// <param name="str">The match pattern.</param>
        /// <returns></returns>
        public static string AsteriskWildcard(this string str)
        {
            return Regex.Escape(str).Replace(@"\*", ".*?");
        }

        public static void LogConsole(this string str)
        {
            UnityEngine.Debug.Log(str);
        }

        public static string RemoveUTF8DOM(this string str)
        {
            var bytes = str.GetBytes();
            if (StringHelper.HaveUTF8BOM(ref bytes))
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            else
                return str;
        }
    }
}
