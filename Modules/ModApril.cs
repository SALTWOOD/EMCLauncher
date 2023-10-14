using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltLib.Modules
{
    internal class ModApril
    {
        //愚人节
        public static void IsAprilFool(Action? func, Action? defaultFunc = null)
        {
            if (func != null && DateTime.Now.ToString("MM-dd") == "04-01")
            {
                func();
            }
            else if (defaultFunc != null)
            {
                defaultFunc();
            }
        }
    }
}
