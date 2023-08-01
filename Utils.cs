using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMCL
{
    internal static class Utils
    {
        public class UnknownException : Exception
        {
            public UnknownException(string message = "Unknown error occurred.")
               : base(message)
            {

            }
            public UnknownException(string message, Exception innerException)
            : base(message, innerException)
            {

            }
        }
        public static string GetTimeNow() { return DateTime.Now.ToString("HH:mm:ss.fff"); }
    }
}
