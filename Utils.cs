using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;
using EMCL.Modules;

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
            "netease","1.","启动",
            "files"
        };
        public static List<string> folders = new List<string>()
        {
            $"{ModPath.path}EMCL/",
            $"{ModPath.path}EMCL/Logs/",
            $"{ModPath.path}EMCL/Temp/",
            $"{ModPath.path}EMCL/CrashReports/"
        };
        public class Config
        {
            public List<List<object>>? java;
            public long tempTime;
            public bool forceDisableJavaAutoSearch;
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

        //啥也不干
        public static void DoNothing()
        {
            return;
        }

        public enum LogLevel
        {
            Debug = 1,
            Normal = 2,
            Information = 3,
            Hint = 4,
            Message = 5,
            Error = 6,
            Fatal = 7
        }

        #region 配置读取
        public static void WriteConfig(Config config)
        {
            StreamWriter sw = new StreamWriter($"{ModPath.path}EMCL/settings.json");
            sw.Write(JsonConvert.SerializeObject(config));
            sw.Close();
        }

        public static Config ReadConfig()
        {
            Config result;
            using (StreamReader sr = new StreamReader($"{ModPath.path}EMCL/settings.json"))
            {
                result = JsonConvert.DeserializeObject<Config>(sr.ReadToEnd())!;
            }
            return result;
        }
        #endregion
    }
}
