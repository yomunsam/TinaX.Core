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

namespace TinaX.Exceptions
{
    public class XException : ApplicationException
    {
        public bool ServiceException { get; protected set; } = false;
        public string ServiceName { get; protected set; } = string.Empty;
        public int ErrorCode { get; protected set; }
        public XException(string msg) : base(msg) { }
        public XException(string msg, int errorCode) : base(msg) { ErrorCode = errorCode; }
        public XException(int errorCode) { ErrorCode = errorCode; }
        public XException() { }
    }
}
