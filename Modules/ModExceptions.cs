using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltLib.Modules
{
    internal class ModExceptions
    {
        //未知异常
        public class UnknownException : Exception
        {
            public UnknownException(string message = "Unknown error occurred.")
               : base(message)
            {
                return;
            }
            public UnknownException(string message, Exception innerException)
            : base(message, innerException)
            {
                return;
            }
        }
    }
}
