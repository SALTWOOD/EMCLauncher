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
using SaltLib.Modules;

namespace EMCL
{
    public static class Constants
    {

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
            $"{ModPath.path}EMCL/CrashReports/",
            $"{ModPath.path}.minecraft/",
            $"{ModPath.path}.minecraft/libraries",
            $"{ModPath.path}.minecraft/assets",
            $"{ModPath.path}.minecraft/versions"
        };
    }
}
