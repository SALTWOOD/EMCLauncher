using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static EMCL.Utils;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace EMCL
{
    public partial class Form1 : Form
    {
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

        public Dictionary<string,bool> javaSearch()
        {
            /*
            foreach (string pathInEnv in Split((PathEnv & ";" & PathJavaHome).Replace("\\", "\").Replace(" / ", "\"), "; ")
                PathInEnv = PathInEnv.Trim(" """.ToCharArray())
                If PathInEnv = "" Then Continue For
                If Not PathInEnv.EndsWith("\") Then PathInEnv += "\"
                '粗略检查有效性
                If File.Exists(PathInEnv & "javaw.exe") Then JavaPreList(PathInEnv) = False
            Next
            '查找磁盘中的 Java
            For Each Disk As DriveInfo In DriveInfo.GetDrives()
                JavaSearchFolder(Disk.Name, JavaPreList, False)
            Next
            '查找 APPDATA 文件夹中的 Java
            JavaSearchFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\", JavaPreList, False)
            JavaSearchFolder(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\", JavaPreList, False)
            '查找启动器目录中的 Java
            JavaSearchFolder(Path, JavaPreList, False, IsFullSearch:= True)
            '查找所选 Minecraft 文件夹中的 Java
            If Not String.IsNullOrWhiteSpace(PathMcFolder) AndAlso Path <> PathMcFolder Then
                JavaSearchFolder(PathMcFolder, JavaPreList, False, IsFullSearch:= True)
            End If

            '若不全为符号链接，则清除符号链接的地址
            Dim JavaWithoutReparse As New Dictionary(Of String, Boolean)
            For Each Pair In JavaPreList
                Dim Folder As String = Pair.Key.Replace("\\", "\").Replace(" / ", "\")
                Dim Info As FileSystemInfo = New FileInfo(Folder & "javaw.exe")
                Do
                    If Info.Attributes.HasFlag(FileAttributes.ReparsePoint) Then
                        Log("[Java] 位于 " & Folder & " 的 Java 包含符号链接")
                        Continue For
                    End If
                    Info = If(TypeOf Info Is FileInfo, CType(Info, FileInfo).Directory, CType(Info, DirectoryInfo).Parent)
                Loop While Info IsNot Nothing
                Log("[Java] 位于 " & Folder & " 的 Java 不含符号链接")
                JavaWithoutReparse.Add(Pair.Key, Pair.Value)
            Next
            If JavaWithoutReparse.Count > 0 Then JavaPreList = JavaWithoutReparse

            '若不全为特殊引用，则清除特殊引用的地址
            Dim JavaWithoutInherit As New Dictionary(Of String, Boolean)
            For Each Pair In JavaPreList
                If Pair.Key.Contains("javapath_target_") OrElse Pair.Key.Contains("javatmp") Then
                    Log("[Java] 位于 " & Pair.Key & " 的 Java 包含特殊引用")
                Else
                    Log("[Java] 位于 " & Pair.Key & " 的 Java 不含特殊引用")
                    JavaWithoutInherit.Add(Pair.Key, Pair.Value)
                End If
            Next
            If JavaWithoutInherit.Count > 0 Then JavaPreList = JavaWithoutInherit*/
            return null;
        }

        public Dictionary<string,bool> JavaSearchFolder(DirectoryInfo originPath, Dictionary<string,bool> result, bool isFullSearch = false)
        {
            try
            {
                //TODO:实现查找Java的功能
                if (originPath != null && originPath.Exists)
                {
                    string path = originPath.FullName.Replace("\\\\", "\\").Replace("\\", "/");
                    if (!path.EndsWith("/")) path += "/";
                    if (System.IO.File.Exists($"{path}javaw.exe")) { result[path] = true; }
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
                            JavaSearchFolder(folder, result);
                        }
                    }
                    return result;
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
            return result;
        }

        public int launchGame(string javaPath, string launchArgs)
        {
            //Console.WriteLine(GetFileNameFromPath("C:\\Program Files\\Java\\jdk-17.0.5\\bin"));
            //Console.WriteLine(JavaSearchFolder(new DirectoryInfo("C:\\Program Files\\Java\\jdk-17.0.5\\bin"))[0]);
            foreach (DriveInfo j in DriveInfo.GetDrives())
            {
                try
                {
                    foreach (KeyValuePair<string, bool> i in JavaSearchFolder(new DirectoryInfo(j.Name), new Dictionary<string, bool>()))
                    {
                        Console.WriteLine(i.Key);
                    }
                }
                catch (FileNotFoundException ex) { Console.WriteLine($"{j.Name} failed."); }
                finally
                {
                    Console.WriteLine($"{j.Name} succeeded");
                }
            }
            Process mc = new Process();
            mc.StartInfo.FileName = javaPath;//使用传入的Java Path
            mc.StartInfo.Arguments = launchArgs;//使用传入的参数
            mc.StartInfo.UseShellExecute = false;//不使用命令行启动
            mc.StartInfo.RedirectStandardOutput = true;
            mc.StartInfo.RedirectStandardError = true;
            mc.StartInfo.RedirectStandardInput = true;
            //mc.Start();//MC，启动！
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
