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
        public static Random random = new Random();
        public static List<string> tips = new List<string>()
        {
            "636 lines of code!",
            "Not PCL2!",
            "Try our sister game, Minceraft!",
            $"Today is {DateTime.Now.ToString("yyyy-MM-dd")}!",
            $"{random.Next(0,114514)}!",
            "EMCL '͟͝͞Ⅱ́̕!"
        };
        public static List<string> suspiciousWords = new List<string>()
        {
            "java","jdk","env",
            "环境","run","软件",
            "jre","mc",
            "software","cache","temp",
            "corretto","roaming","users",
            "craft","program","世界",
            "net","游戏","oracle",
            "game","file","data",
            "jvm","服务","server",
            "客户","client","整合",
            "应用","运行","前置",
            "mojang","官启","新建文件夹",
            "eclipse","microsoft","hotspot",
            "runtime","x86","x64",
            "forge","原版","optifine",
            "官方","启动","hmcl",
            "mod","高清","download",
            "launch","程序","path",
            "version","baka","pcl",
            "local","packages","4297127D64EC6",
            "国服","网易","ext",
            "netease","1.","启动"
        };
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
                return;
            }
            public UnknownException(string message, Exception innerException)
            : base(message, innerException)
            {
                return;
            }
        }

        public static bool ContainsSuspiciousWords(string s)
        {
            foreach (string i in suspiciousWords)
            {
                if (s.Contains(i)) { return true; }
            }
            return false;
        }

        public static string GetTimeNow() { return DateTime.Now.ToString("HH:mm:ss.fff"); }

        public static string RegexReplace(string input, string replacement, string regex, RegexOptions options = RegexOptions.None) { return Regex.Replace(input, regex, replacement, options); }
    }
}
