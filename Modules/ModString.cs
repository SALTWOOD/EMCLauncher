using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EMCL.Modules
{
    internal class ModString
    {
        //替换字符串
        public static string RegexReplace(string input, string replacement, string regex, RegexOptions options = RegexOptions.None) { return Regex.Replace(input, regex, replacement, options); }

        //搜索Java时代替if判断
        public static bool ReturnIfSus(bool isFullSearch, DirectoryInfo folder, string searchEntry)
        {
            return isFullSearch || (folder.Parent!.Name == "users") || ModString.ContainsSuspiciousWords(searchEntry) ||
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
            foreach (string i in Utils.suspiciousWords)
            {
                if (s.Contains(i)) { return true; }
            }
            return false;
        }
    }
}
