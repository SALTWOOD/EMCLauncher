using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;

namespace SaltLib.Modules
{
    internal static class ModString
    {
        public static string newLine = "\r\n";

        //替换字符串
        public static string RegexReplace(string input, string replacement, string regex, RegexOptions options = RegexOptions.None) { return Regex.Replace(input, regex, replacement, options); }
        public static string RegexMatch(string input, string reg, RegexOptions options = RegexOptions.None)
        {
            try
            {
                string result = Regex.Match(input, reg, options).Value;
                return result == "" ? null! : result;
            }
            catch (Exception ex)
            {
                ModLogger.Log(ex, "正则匹配第一项出错");
                return null!;
            }
        }

        //搜索Java时代替if判断
        public static bool ReturnIfSus(bool isFullSearch, DirectoryInfo folder, string searchEntry)
        {
            return isFullSearch || (folder.Parent!.Name == "users") || ModString.ContainsSuspiciousWords(searchEntry.ToLower()) ||
                searchEntry == "bin";
        }

        //分割字符串
        public static string[] Split(string fullStr, string splitStr)
        {
            if (splitStr.Length == 1)
            {
                return fullStr.Split(splitStr[0]);
            }
            else
            {
                return fullStr.Split(new string[] { splitStr }, StringSplitOptions.None);
            }
        }

        //路径搜索检索关键词
        public static bool ContainsSuspiciousWords(string s)
        {
            foreach (string i in Constants.suspiciousWords)
            {
                if (s.Contains(i)) { return true; }
            }
            return false;
        }

        //将"\"、"\\"替换为"/"
        public static string SlashReplace(string s) { return s.Replace("\\\\", "\\").Replace("\\", "/"); }
    }
}
