using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.AccessControl;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Windows.Forms;
using static EMCL.Utils;
using EMCL.Modules;
using System.Runtime.ConstrainedExecution;

namespace EMCL
{
    public partial class Form1 : Form
    {
        Dictionary<string, bool> javaList = new Dictionary<string, bool>();
        public Config config = new Config();

        #region 日志记录器
        #endregion

        #region 异常处理与线程运行

        public void handleException(Exception ex)
        {
            ModLogger.Log(ex, "未知错误", LogLevel.Fatal);
            Console.WriteLine($"{ex.GetType()}{ModString.newLine}{ex.Message}{ModString.newLine}{ex.StackTrace}{ModString.newLine}{ModString.newLine}现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！");
            if (MessageBox.Show($"{ex.GetType()}{ModString.newLine}{ex.Message}{ModString.newLine}{ex.StackTrace}{ModString.newLine}{ModString.newLine}现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！", "无法处理的异常", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                Process.Start("https://github.com/SALTWOOD/EMCLauncher/issues/new/choose");
            }
        }

        private void RunProtected(Action function)
        {
            try
            {
                function();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }
        #endregion

        #region 游戏启动
        public int launchGame(string javaPath, string launchArgs)
        {
            try
            {
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
            catch (Exception ex)
            {
                ModLogger.Log(ex, "Minecraft 启动失败", LogLevel.Normal);
                return -1;
            }
        }
        #endregion

        #region 窗口初始化
        public Form1()
        {
            ModLogger.Log("[Main] 主程序启动中！");
            ModLogger.Log($"[App] {Metadata.name}, 版本 {Metadata.version}");
            InitializeComponent();
            ModLogger.Log("[Main] InitializeComponent() 执行完毕！");
            this.Text = Metadata.title;
            ModLogger.Log("[Main] 主程序组件成功加载！");
            ModLogger.LoggerStart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadApp();
                lblTips.Text = tips[random.Next(tips.Count)];
                ModApril.IsAprilFool(() =>
                {
                    lblTips.Visible = true;
                });
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            ModLogger.Log("[Main] 主程序窗口框架加载完毕！");
        }

        public void LoadApp()
        {
            if (Directory.Exists($"{ModPath.path}EMCL/"))
            {
                if (System.IO.File.Exists($"{ModPath.path}EMCL/settings.json"))
                {
                    ModLogger.Log($"[Config] 正在加载配置文件 {ModPath.path}EMCL/settings.json");
                    ModLogger.Log($"[Java] 开始读取 Java 缓存");
                    config = ReadConfig();
                    if (!(DateTimeOffset.Now.ToUnixTimeSeconds() - 604800 < config.tempTime))
                    {
                        cmbJavaList.Items.Clear();
                        foreach (List<object> i in config.java!)
                        {
                            javaList.Add((string)i[0], (bool)i[1]);
                            cmbJavaList.Items.Add(i[0]);
                        }
                        ModLogger.Log($"[Java] Java 缓存读取完毕！");
                    }
                    else
                    {
                        ModLogger.Log($"[Java] Java 缓存已过期，开始重新生成缓存！");
                        cmbJavaList.Items.Clear();
                        javaList = ModJava.JavaCacheGen(config);
                        foreach (string i in javaList.Keys)
                        {
                            cmbJavaList.Items.Add(i);
                        }
                        WriteConfig(config);
                    }
                }
                else
                {
                    ModLogger.Log($"[Config] 没有找到配置文件，开始生成默认配置文件！");
                    cmbJavaList.Items.Clear();
                    javaList = ModJava.JavaCacheGen(config);
                    foreach (string i in javaList.Keys)
                    {
                        cmbJavaList.Items.Add(i);
                    }
                    config.tempTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                    config.forceDisableJavaAutoSearch = false;
                    WriteConfig(config);
                    ModLogger.Log($"[Config] 默认配置文件生成完毕！");
                }
            }
            else
            {
                ModLogger.Log($"[Main] 未找到EMCL 文件夹，自动创建！！");
                Directory.CreateDirectory($"{ModPath.path}EMCL/");
                Directory.CreateDirectory($"{ModPath.path}EMCL/Logs/");
                LoadApp();
            }
        }
        #endregion

        #region 窗口控件行为
        private void btnJavaSearch_Click(object sender, EventArgs e)
        {
            cmbJavaList.Items.Clear();
            javaList = ModJava.JavaCacheGen(config);
            foreach (KeyValuePair<string, bool> i in javaList)
            {
                cmbJavaList.Items.Add(i.Key);
            }
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            //Person laiya = new Person("Łaiya",Person.HasDick.No);
            //laiya.Kill();
            try
            {
                StreamReader sr = new StreamReader("./args.txt");//目前直接读取程序目录下args.txt内的内容
                string args = sr.ReadToEnd();
                //现在可以自己选择 Java 了
                string java = $"{cmbJavaList.Text}javaw.exe";
                sr.Close();
                Thread t = ModThread.RunThread(() => launchGame(java, args), "MinecraftLaunchThread", ThreadPriority.AboveNormal);//创建MC启动线程
                ModLogger.Log("[Launcher] 启动 Minecraft 成功！");
                MessageBox.Show("启动成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OutOfMemoryException ex)
            {
                ModLogger.Log(ex, "内存不足", LogLevel.Message);
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        private void cmbJavaList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModLogger.Log($"[Java] 选择的 Java 更改为 {cmbJavaList.Text}");
            try
            {
                Version ver = ModJava.Check(cmbJavaList.Text);
                lblJavaVer.Text = $"当前 Java 版本：{ver}";
            }
            catch (Exception ex)
            {
                DialogResult choice = MessageBox.Show($"{ex.Message}{ModString.newLine}{ModString.newLine}此 Java 可能无效，继续使用此 Java吗？{ModString.newLine}{ModString.newLine}按\"中止\"使用其他 Java{ModString.newLine}按\"重试\"重新检查此 Java{ModString.newLine}按\"忽略\"强制使用 Java" + "", "Java 错误!"
                    , MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                switch (choice)
                {
                    case DialogResult.Abort:
                        {
                            cmbJavaList.Items.Remove(cmbJavaList.SelectedItem);
                            if (cmbJavaList.Items.Count == 0)
                            {
                                cmbJavaList.Text = "Java 列表";
                            }
                            else
                            {
                                cmbJavaList.SelectedIndex = 0;
                            }
                            break;
                        }
                    case DialogResult.Retry:
                        {
                            try
                            {
                                ModJava.Check(cmbJavaList.Text);
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("此 Java 无法使用。", "检查失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                lblJavaVer.Text = $"当前 Java 版本：未知";
                            }
                            break;
                        }
                    case DialogResult.Ignore:
                    default:
                        {
                            lblJavaVer.Text = $"当前 Java 版本：未知";
                            break;
                        }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ModApril.IsAprilFool(() =>
            {
                //愚人节彩蛋罢了（
                ModLogger.Log("[2YHLrd] 有人在关掉我！", LogLevel.Normal);
            });
            ModLogger.Log("[Main] 程序正在关闭！", LogLevel.Normal);
            ModLogger.LoggerFlush();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ModLogger.Log("[Main] 准备开始关闭其他线程", LogLevel.Normal);
            ModRun.isExited = true;
            ModLogger.Log("[Option] isExited 状态被设为 true", LogLevel.Normal);
            ModLogger.Log("[Thread]<Main> 开始关闭其他线程", LogLevel.Normal);
            foreach (Thread t in ModThread.threads)
            {
                t.Join();
            }
            ModLogger.Log("[Thread]<Main> 其他线程已关闭！", LogLevel.Normal);
            ModLogger.Log("[Main] 程序已关闭！", LogLevel.Normal);
            ModLogger.LoggerFlush();
            System.Windows.Forms.Application.Exit();
        }

        private void btnChooseJava_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择 Java";
            dialog.Filter = "Java Executable File (javaw.exe)|javaw.exe";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selected = ModString.SlashReplace(new FileInfo(dialog.FileName).DirectoryName!);
                if (!selected.EndsWith("/")) { selected += "/"; }
                cmbJavaList.Items.Add(selected);
                cmbJavaList.SelectedItem = ModString.SlashReplace(selected);
            }
        }
        #endregion
    }
}
