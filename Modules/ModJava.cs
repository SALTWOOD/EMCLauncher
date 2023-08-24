using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EMCL.Utils;

namespace EMCL.Modules
{
    internal static class ModJava
    {
        #region Java搜索
        public static Version Check(string javaPath)
        {
            string? res = null;
            try
            {
                //确定Java类型及可用性
                if (!(File.Exists($"{javaPath}javaw.exe") && File.Exists($"{javaPath}java.exe")))
                {
                    throw new FileNotFoundException($"在 \"{javaPath}\" 中未找到 Java 可执行文件或 Java 可执行文件缺失/损坏！");
                }
                //确定Java版本
                res = ModRun.RunProcess($"{javaPath}java.exe", "-version", 15000, $"{ModPath.path}EMCL/Temp").ToLower();
                bool isDev = File.Exists($"{javaPath}javac.exe");
                bool is64B = res.Contains("64-bit");
                if (res == "") { throw new Exception("尝试运行该 Java 失败"); }
                if (Metadata.DEBUG)
                {
                    ModLogger.Log("[Java] Java 检查输出：{PathFolder}{java.exe}{vbCrLf}{res}");
                }
                ModLogger.Log(res);
                //获取详细信息
                string verStr = (ModString.RegexMatch(res, "(?<=version \")[^\"\"]+") != null ?
                    ModString.RegexMatch(res, "(?<=version \")[^\"\"]+") :
                    (ModString.RegexMatch(res, "(?<=openjdk )[0-9]+") != null ? ModString.RegexMatch(res, "(?<=openjdk )[0-9]+") : ""))
                    .Replace("_", ".").Split("-").First();
                ModLogger.Log(verStr);
                while (verStr.Split(".").Count() < 4)
                {
                    if (verStr.StartsWith("1.")) { verStr = $"{verStr}.0"; }
                    else { verStr = $"1.{verStr}"; }
                }
                //ModLogger.Log(verStr);
                Version version = new Version(verStr);
                if (version.Minor == 0)
                {
                    version = new Version(1, version.Major, version.Build, version.Revision);
                }
                if (version.Minor <= 4 || version.Minor >= 25) { throw new Exception($"分析详细信息失败，获取的版本为 {version.ToString()}"); }
                return version;
            }
            catch (Exception ex)
            {
                ModLogger.Log($"[Java] 检查失败的 Java 输出：{javaPath}java.exe\r\n{(res!= null ? res : "无程序输出")}\r\n{ex}");
                throw new Exception($"检查 Java 失败（{(javaPath != null ? javaPath : "null")}）", ex);
            }
        }

        public static Dictionary<string, bool> javaSearch()
        {
            ModLogger.Log($"[Java] 开始搜索 Java");
            Dictionary<string, bool> javaDic = new Dictionary<string, bool>();
            foreach (string i in ModString.Split(ModString.SlashReplace($"{ModPath.pathEnv};{ModPath.pathJavaHome}"), ";"))
            {
                string pathInEnv = i.Trim("\"\"".ToCharArray());
                if (pathInEnv == "") { continue; }
                if (!pathInEnv.EndsWith("/")) { pathInEnv += "/"; }
                //粗略检查有效性
                if (File.Exists($"{pathInEnv}javaw.exe")) { javaDic[pathInEnv] = false; }
            }
            //查找磁盘中的 Java
            foreach (DriveInfo disk in DriveInfo.GetDrives())
            {
                javaDic = JavaSearchFolder(ModString.SlashReplace(disk.Name), javaDic, false);
            }
            //查找 APPDATA 文件夹中的 Java
            javaDic = JavaSearchFolder($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/", javaDic, false);
            javaDic = JavaSearchFolder($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/", javaDic, false);
            //查找启动器目录中的 Java
            javaDic = JavaSearchFolder(ModPath.path!, javaDic, false, isFullSearch: true);
            //查找所选 Minecraft 文件夹中的 Java
            if (!(string.IsNullOrWhiteSpace(ModPath.pathMCFolder) && (ModPath.path == ModPath.pathMCFolder))) { javaDic = JavaSearchFolder(ModPath.pathMCFolder!, javaDic, false, isFullSearch: true); }
            //若不全为符号链接，则清除符号链接的地址
            foreach (var i in javaDic) Console.WriteLine(i);
            Dictionary<string, bool> javaToCheck = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, bool> pair in javaDic)
            {
                string folder = ModString.SlashReplace(pair.Key);
                FileSystemInfo? info = new FileInfo($"{folder}javaw.exe");
                ModLogger.Log($"[Java] 发现 Java {pair.Key}");
                do
                {
                    if ((!info.Attributes.HasFlag(FileAttributes.ReparsePoint) && !(pair.Key.Contains("javapath_target_") || pair.Key.Contains("javatmp"))))
                    {
                        javaToCheck[pair.Key] = pair.Value;
                    }
                    info = (info is FileInfo) ? ((FileInfo)info).Directory : ((DirectoryInfo)info).Parent;
                }
                while (info != null);
            }
            Dictionary<string, bool> temp = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, bool> pair in javaToCheck)
            {
                try
                {
                    Check(pair.Key);
                    temp[pair.Key] = pair.Value;
                }
                catch
                {
                    ModLogger.Log($"[Java] 位于 {pair.Key} 的 Java 无效，忽略！");
                }
            }
            if (temp.Count > 0) { javaDic = temp; }
            ModLogger.Log($"[Java] Java 扫描完毕！");
            return javaDic;
        }
        //public Dictionary<string, bool> JavaSearchFolder(string originalPath, ref Dictionary<string, bool> results, bool source, bool isFullSearch = false)
        public static Dictionary<string, bool> JavaSearchFolder(string originalPath, Dictionary<string, bool> results, bool source, bool isFullSearch = false)
        {
            try
            {
                ModLogger.Log($@"[Java] 开始{(isFullSearch ? """完全""" : """部分""")}遍历查找：{originalPath}");
                results = JavaSearchFolder(new DirectoryInfo(originalPath), results, source, isFullSearch);
            }
            catch (UnauthorizedAccessException ex)
            {
                ModLogger.Log($"[Java] 遍历查找 Java 时遭遇无权限的文件夹：{originalPath}");
            }
            catch (Exception ex)
            {
                ModLogger.Log(ex, $"遍历查找 Java 时出错（{originalPath}）");
            }
            return results;
        }

        public static Dictionary<string,bool> JavaSearchFolder(DirectoryInfo originPath, Dictionary<string, bool> result, bool source, bool isFullSearch = false)
        {
            try
            {
                if (originPath != null && originPath.Exists)
                {
                    string path = ModString.SlashReplace(originPath.FullName);
                    if (!path.EndsWith("/")) path += "/";
                    if (File.Exists($"{path}javaw.exe")) { result[path] = source; ModLogger.Log($"[Java] 找到一个 Java: {path}"); }
                    foreach (DirectoryInfo folder in originPath.EnumerateDirectories())
                    {
                        if (!folder.Exists) continue;
                        if (folder.Attributes.HasFlag(FileAttributes.ReparsePoint)) continue;
                        string searchEntry = GetFileNameFromPath(folder.Name).ToLower();
                        if (ModString.ReturnIfSus(isFullSearch, folder, searchEntry))
                        {
                            result = JavaSearchFolder(folder, result, false);
                        }
                    }
                    return result;
                }
                else
                {
                    try
                    {
                        if (originPath == null) { throw new ArgumentNullException("Expected a non null value, but received a null value as a parameter."); }
                        else if (!originPath.Exists) { throw new FileNotFoundException($"The specified file ({originPath.Name}) does not exist."); }
                        else { throw new ModExceptions.UnknownException(); }
                    }
                    catch (Exception ex)
                    {
                        ModLogger.Log(ex, ex.Message, LogLevel.Normal);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                //Console.WriteLine(ex.Message);
            }
            return result;
        }

        public static Dictionary<string, bool> JavaCacheGen(Config? config = null)
        {
            Dictionary<string,bool> result = javaSearch();
            List<List<object>> json = new List<List<object>>();
            foreach (KeyValuePair<string, bool> i in result)
            {
                json.Add(new List<object>() { i.Key, i.Value });
            }
            ModLogger.Log($"[Java] Java 缓存生成完毕！");
            if (config != null)
            {
                config.java = json;
                WriteConfig(config);
                ModLogger.Log($"[Java] Java 缓存写入完毕！");
            }
            return result;
        }
        #endregion
    }
}
