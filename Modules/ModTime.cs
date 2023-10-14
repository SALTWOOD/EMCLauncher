using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltLib.Modules
{
    internal class ModTime
    {
        //获取当前时间
        public static string GetTimeNow() { return DateTime.Now.ToString("HH:mm:ss.fff"); }
    }
}
