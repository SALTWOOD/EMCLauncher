using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMCL.Modules
{
    internal class ModPath
    {
        public static string path = ModString.SlashReplace(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!);
        public static string pathMCFolder = $"{ModString.SlashReplace(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!)}.minecraft/";

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

        //根据路径获取文件名
        public static string GetFileNameFromPath(string filePath)
        {
            string path = filePath.Replace("\\", "/");
            if (path.EndsWith("/")) { throw new Exception($"不包含文件名：{filePath}"); }
            string name = filePath.Split('/').Last().Split('?').First();
            if (name.Length == 0) { throw new Exception($"不包含文件名：{filePath}"); }
            if (name.Length > 250) { throw new PathTooLongException($"文件名过长：{filePath}"); }
            return name;
        }
    }
}
