using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace TinaX.Utils
{
    public static class ZipUtil
    {
        public static void ZipDirectory(string directory,string output_filename)
        {
            var fastzip = new FastZip();
            fastzip.CreateZip(output_filename, directory, true, "");
        }

        /// <summary>
        /// Create Zip file by directory , with process callback
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="output_filename"></param>
        /// <param name="callback">string arg: file name</param>
        public static void ZipDirectory(string directory, string output_filename, Action<string> callback) //带数据回调
        {
            FastZipEvents events = new FastZipEvents();
            events.ProcessFile = (sender, args) =>
            {
                callback?.Invoke(args.Name);
            };
            var fastzip = new FastZip(events);
            fastzip.CreateZip(output_filename, directory, true, "");
        }
    }
}
