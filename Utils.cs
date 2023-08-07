using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static EMCL.winMain;
using System.Xml.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;

namespace EMCL
{
    public static class Utils
    {
        public class Config
        {
            public List<List<object>> java;
            public long tempTime;
            public bool forceDisableJavaAutoSearch;
        }

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

        public static string RegexReplace(string input, string replacement, string regex, RegexOptions options = RegexOptions.None) { return Regex.Replace(input, regex, replacement, options); }
    }
}
