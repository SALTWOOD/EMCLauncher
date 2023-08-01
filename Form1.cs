using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.AccessControl;
using System.Threading;
using System.Windows.Forms;
using static EMCL.Utils;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace EMCL
{
    public partial class Form1 : Form
    {
        string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        string pathMCFolder = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}.minecraft";
        //string path = "";
        //string path = "";
        private string _pathEnv = null;
        private string _pathJavaHome = null;

        public string pathEnv
        {
            get
            {
                if (_pathEnv is null) { _pathEnv = Environment.GetEnvironmentVariable("Path") != null? Environment.GetEnvironmentVariable("Path"):""; }
                return _pathEnv;
            }
        }
        public string pathJavaHome
        {
            get
            {
                if (_pathJavaHome is null) { _pathJavaHome = Environment.GetEnvironmentVariable("JAVA_HOME") != null? Environment.GetEnvironmentVariable("JAVA_HOME"):""; }
                return _pathJavaHome;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            //Person laiya = new Person("Łaiya",Person.HasDick.No);
            //laiya.Kill();
            try
            {
                StreamReader sr = new StreamReader("./args.txt");//目前直接读取程序目录下args.txt内的内容
                string args = sr.ReadToEnd();
                StreamReader jr = new StreamReader("./java.txt");//目前直接读取程序目录下args.txt内的内容
                string java = jr.ReadToEnd();
                sr.Close();
                jr.Close();
                Thread t = new Thread(() => launchGame(java,args));//创建MC启动线程
                t.Start();
                MessageBox.Show("启动成功","提示",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}\n{ex.Message}\n{ex.StackTrace}\n\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！");
                if (MessageBox.Show($"{ex.GetType()}\n{ex.Message}\n{ex.StackTrace}\n\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！","无法处理的异常",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    Process.Start("https://github.com/SALTWOOD/EMCLauncher/issues/new/choose");
                }
            }
        }

        public string[] Split(string fullStr, string splitStr)
        {
            if (splitStr.Length == 1)
            {
                return fullStr.Split(splitStr[0]);
            }
            else
            {
                return fullStr.Split(new string[] { splitStr },StringSplitOptions.None);
            }
        }

        public void Log(string info)
        {

        }

        public void Log(Exception ex,string info)
        {

        }

        public Dictionary<string, bool> javaSearch()
        {
            Dictionary<string, bool> javaDic = new Dictionary<string, bool>();
            foreach (string i in Split(($"{pathEnv};{pathJavaHome}").Replace("\\\\", "\\").Replace(" \\ ", "/"), ";"))
            {
                string pathInEnv = i.Trim("\"\"".ToCharArray());
                if (pathInEnv == "") { continue; }
                if (!pathInEnv.EndsWith("/")) { pathInEnv += "/"; }
                //粗略检查有效性
                if (System.IO.File.Exists($"{pathInEnv} javaw.exe")) { javaDic[pathInEnv] = false; }
            }
            //查找磁盘中的 Java
            foreach (DriveInfo disk in DriveInfo.GetDrives())
            {
                JavaSearchFolder(disk.Name, javaDic, false);
            }
            //查找 APPDATA 文件夹中的 Java
            JavaSearchFolder($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/", javaDic, false);
            JavaSearchFolder($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/", javaDic, false);
            //查找启动器目录中的 Java
            JavaSearchFolder(path, javaDic, false, isFullSearch: true);
            //查找所选 Minecraft 文件夹中的 Java
            if (!(string.IsNullOrWhiteSpace(pathMCFolder) && (path == pathMCFolder))){ JavaSearchFolder(pathMCFolder, javaDic, false, isFullSearch: true); }
            //若不全为符号链接，则清除符号链接的地址
            Dictionary<string, bool> JavaWithoutReparse = new Dictionary<string, bool>();
            foreach (KeyValuePair<string,bool> pair in javaDic)
            {
                string folder = pair.Key.Replace("\\\\", "\\").Replace("\\", "/");
                FileSystemInfo info = new FileInfo($"{folder}javaw.exe");
                do
                {
                    if (info.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        Log($"[Java] 位于 {folder} 的 Java 包含符号链接");
                        continue;
                    }
                    info = (info is FileInfo)?((FileInfo)info).Directory:((DirectoryInfo)info).Parent;
                }
                while (info != null);
                Log($"[Java] 位于 {folder} 的 Java 不含符号链接");
                JavaWithoutReparse.Add(pair.Key, pair.Value);
            }
            if (JavaWithoutReparse.Count > 0) { javaDic = JavaWithoutReparse; }
            //若不全为特殊引用，则清除特殊引用的地址
            Dictionary<string,bool> JavaWithoutInherit = new Dictionary<string,bool>();
            foreach (KeyValuePair<string,bool> pair in javaDic)
            {
                if (pair.Key.Contains("javapath_target_") || pair.Key.Contains("javatmp"))
                {
                    Log($"[Java] 位于 {pair.Key} 的 Java 包含特殊引用");
                }
                else
                {
                    Log($"[Java] 位于 {pair.Key} 的 Java 不含特殊引用");
                    JavaWithoutInherit.Add(pair.Key, pair.Value);
                }
            }
            if (JavaWithoutInherit.Count > 0) { javaDic = JavaWithoutInherit; }
            return javaDic;
        }

        //public Dictionary<string, bool> JavaSearchFolder(string originalPath, ref Dictionary<string, bool> results, bool source, bool isFullSearch = false)
        public void JavaSearchFolder(string originalPath, Dictionary<string, bool> results, bool source, bool isFullSearch = false)
        {
            try
            {
                Log($@"[Java] 开始{(isFullSearch ? """完全""" : """部分""")}遍历查找：{originalPath}");
                JavaSearchFolder(new DirectoryInfo(originalPath), results, source, isFullSearch);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log($"[Java] 遍历查找 Java 时遭遇无权限的文件夹：{originalPath}");
            }
            catch (Exception ex)
            {
                Log(ex, $"遍历查找 Java 时出错（{originalPath}）");
            }
        }

        //public Dictionary<string,bool> JavaSearchFolder(DirectoryInfo originPath, Dictionary<string,bool> result, bool source, bool isFullSearch = false)
        public void JavaSearchFolder(DirectoryInfo originPath, Dictionary<string,bool> result, bool source, bool isFullSearch = false)
        {
            try
            {
                //TODO:实现查找Java的功能
                if (originPath != null && originPath.Exists)
                {
                    string path = originPath.FullName.Replace("\\\\", "\\").Replace("\\", "/");
                    if (!path.EndsWith("/")) path += "/";
                    if (System.IO.File.Exists($"{path}javaw.exe")) { result[path] = source; }
                    foreach (DirectoryInfo folder in originPath.EnumerateDirectories())
                    {
                        if (!folder.Exists) continue;
                        if (folder.Attributes.HasFlag(FileAttributes.ReparsePoint)) continue;
                        string searchEntry = GetFileNameFromPath(folder.Name).ToLower();
                        if (isFullSearch || (folder.Parent.Name == "users") ||
                            searchEntry.Contains("java") || searchEntry.Contains("jdk") || searchEntry.Contains("env") ||
                            searchEntry.Contains("环境") || searchEntry.Contains("run") || searchEntry.Contains("软件") ||
                            searchEntry.Contains("jre") || searchEntry == "bin" || searchEntry.Contains("mc") ||
                            searchEntry.Contains("software") || searchEntry.Contains("cache") || searchEntry.Contains("temp") ||
                            searchEntry.Contains("corretto") || searchEntry.Contains("roaming") || searchEntry.Contains("users") ||
                            searchEntry.Contains("craft") || searchEntry.Contains("program") || searchEntry.Contains("世界") ||
                            searchEntry.Contains("net") || searchEntry.Contains("游戏") || searchEntry.Contains("oracle") ||
                            searchEntry.Contains("game") || searchEntry.Contains("file") || searchEntry.Contains("data") ||
                            searchEntry.Contains("jvm") || searchEntry.Contains("服务") || searchEntry.Contains("server") ||
                            searchEntry.Contains("客户") || searchEntry.Contains("client") || searchEntry.Contains("整合") ||
                            searchEntry.Contains("应用") || searchEntry.Contains("运行") || searchEntry.Contains("前置") ||
                            searchEntry.Contains("mojang") || searchEntry.Contains("官启") || searchEntry.Contains("新建文件夹") ||
                            searchEntry.Contains("eclipse") || searchEntry.Contains("microsoft") || searchEntry.Contains("hotspot") ||
                            searchEntry.Contains("runtime") || searchEntry.Contains("x86") || searchEntry.Contains("x64") ||
                            searchEntry.Contains("forge") || searchEntry.Contains("原版") || searchEntry.Contains("optifine") ||
                            searchEntry.Contains("官方") || searchEntry.Contains("启动") || searchEntry.Contains("hmcl") ||
                            searchEntry.Contains("mod") || searchEntry.Contains("高清") || searchEntry.Contains("download") ||
                            searchEntry.Contains("launch") || searchEntry.Contains("程序") || searchEntry.Contains("path") ||
                            searchEntry.Contains("version") || searchEntry.Contains("baka") || searchEntry.Contains("pcl") ||
                            searchEntry.Contains("local") || searchEntry.Contains("packages") || searchEntry.Contains("4297127D64EC6") ||
                            searchEntry.Contains("国服") || searchEntry.Contains("网易") || searchEntry.Contains("ext") ||
                            searchEntry.Contains("netease") || searchEntry.Contains("1.") || searchEntry.Contains("启动"))
                        {
                            JavaSearchFolder(folder, result, false);
                        }
                    }
                    //return result;
                }
                else
                {
                    if (originPath == null) { throw new ArgumentNullException("Expected a non null value, but received a null value as a parameter."); }
                    else if (!originPath.Exists) { throw new FileNotFoundException($"The specified file ({originPath.Name}) does not exist."); }
                    else { throw new UnknownException(); }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                //Console.WriteLine(ex.Message);
            }
            //return result;
        }

        public int launchGame(string javaPath, string launchArgs)
        {
            //Console.WriteLine(GetFileNameFromPath("C:\\Program Files\\Java\\jdk-17.0.5\\bin"));
            //Console.WriteLine(JavaSearchFolder(new DirectoryInfo("C:\\Program Files\\Java\\jdk-17.0.5\\bin"))[0]);
            Process mc = new Process();
            mc.StartInfo.FileName = javaPath;//使用传入的Java Path
            mc.StartInfo.Arguments = launchArgs;//使用传入的参数
            mc.StartInfo.UseShellExecute = false;//不使用命令行启动
            mc.StartInfo.RedirectStandardOutput = true;
            mc.StartInfo.RedirectStandardError = true;
            mc.StartInfo.RedirectStandardInput = true;
           mc.Start();//MC，启动！
            return 0;
        }

        public string GetFileNameFromPath(string filePath)
        {
            string path = filePath.Replace("\\", "/");
            if (path.EndsWith("/")){ throw new Exception($"不包含文件名：{filePath}"); }
            string name = filePath.Split('/').Last().Split('?').First();
            if (name.Length == 0) { throw new Exception($"不包含文件名：{filePath}"); }
            if (name.Length > 250) { throw new PathTooLongException($"文件名过长：{filePath}"); }
            return name;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    /*
    public class Person
    {
        public string name;
        private HasDick sex;
        private bool isDead;
        public enum HasDick{Yes, No};
        public Person(string name,HasDick sex)
        {
            this.name = name;
            this.sex = sex;
        }

        public void Kill()
        {
            if (this.isDead) return;
            Console.WriteLine($"{this.name}:awsl");
            this.isDead = true;
        }
    }*/
}
