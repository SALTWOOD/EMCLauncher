using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMCL.Modules
{
    internal class ModPath
    {
        public static string path = ModString.SlashReplace(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!);
        public static string pathMCFolder = $"{ModString.SlashReplace(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!)}.minecraft";

        public static string? _pathEnv = null;
        public static string? _pathJavaHome = null;

        public static string pathEnv
        {
            get
            {
                if (_pathEnv is null) { _pathEnv = Environment.GetEnvironmentVariable("Path") != null ? Environment.GetEnvironmentVariable("Path") : ""; }
                return _pathEnv!;
            }
        }
        public static string pathJavaHome
        {
            get
            {
                if (_pathJavaHome is null) { _pathJavaHome = Environment.GetEnvironmentVariable("JAVA_HOME") != null ? Environment.GetEnvironmentVariable("JAVA_HOME") : ""; }
                return _pathJavaHome!;
            }
        }
    }
}
