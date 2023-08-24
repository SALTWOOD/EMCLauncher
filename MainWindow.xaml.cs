using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EMCL.Modules;
using static EMCL.Utils;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Ookii.Dialogs.Wpf;
using Microsoft.Win32;
using Microsoft.VisualBasic.Devices;

namespace EMCL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, bool> javaList = new Dictionary<string, bool>();
        public Config config = new Config();
        public ModSerial.CInfo computer = new ModSerial.CInfo();

        #region 日志记录器
        #endregion

        #region 异常处理与线程运行
        /*
        public void handleException(Exception ex)
        {
            ModLogger.Log(ex, "未知错误", LogLevel.Fatal);
            Console.WriteLine($"{ex.GetType()}{ModString.newLine}{ex.Message}{ModString.newLine}{ex.StackTrace}{ModString.newLine}{ModString.newLine}现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！");
            if (MessageBox.Show($"{ex.GetType()}{ModString.newLine}{ex.Message}{ModString.newLine}{ex.StackTrace}{ModString.newLine}{ModString.newLine}现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！", "无法处理的异常", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                Process.Start("https://github.com/SALTWOOD/EMCLauncher/issues/new/choose");
            }
        }*/

        public void handleException(Exception ex)
        {

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
            int result;
            try
            {
                ModLogger.Log($"[Main] ~ ~ ~ S U M M A R Y ~ ~ ~{ModString.newLine}Java Path: {javaPath}{ModString.newLine}Args: {launchArgs}");
                Process mc = new Process();
                ProcessStartInfo info = new ProcessStartInfo()
                {
                    FileName = javaPath,//使用传入的Java Path
                    Arguments = launchArgs,//使用传入的参数
                    UseShellExecute = false,//不使用命令行启动
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    WorkingDirectory = ModPath.pathMCFolder
                };
                mc.StartInfo = info;
                mc.Start();//MC，启动！
                ModLogger.Log($"[Main] Minecraft 启动成功！");
                mc.WaitForExit(-1);
                string file = $"{ModPath.path}EMCL/CrashReports/crash-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log";
                if (mc.ExitCode != 0)
                {
                    MessageBox.Show($"Minecraft 异常退出！{ModString.newLine}" +
                        $"程序退出代码：{mc.ExitCode}{ModString.newLine}" +
                        $"错误日志已保存至{file}", "Minecraft 崩溃！");
                }
                result = mc.ExitCode;
                StreamWriter sr = new StreamWriter(file);
                sr.Write(mc.StandardOutput.ReadToEnd());
                sr.Close();
            }
            catch (Exception ex)
            {
                ModLogger.Log(ex, "Minecraft 启动失败", LogLevel.Normal);
                result = -1;
            }
            return result;
        }
        #endregion

        #region 窗口初始化
        public MainWindow()
        {
            ModLogger.Log("[Main] 主程序启动中！");
            ModLogger.Log($"[App] {Metadata.name}, 版本 {Metadata.version}");
            ModLogger.Log($"[App] 网络协议版本号 {Metadata.protocol} (0x{Metadata.protocol.ToString("X").PadLeft(8,'0')})");
            ModLogger.Log($"[System] 计算机基础信息:\n{computer}");
            InitializeComponent();
            ModLogger.Log("[Main] InitializeComponent() 执行完毕！");
            this.Title = Metadata.title;
            lblTitle.Content = Metadata.title;
            ModLogger.Log("[Main] 主程序组件成功加载！");
            ModLogger.LoggerStart();
        }
        
        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            try
            {
                LoadApp();
                lblTips.Content = tips[random.Next(tips.Count)];
                ModApril.IsAprilFool(() =>
                {
                    lblTips.Visibility = Visibility.Visible;
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
            foreach (string fold in folders)
            {
                if (!Directory.Exists(fold))
                {
                    Directory.CreateDirectory(fold);
                    break;
                }
            }
            if (File.Exists($"{ModPath.path}EMCL/settings.json"))
            {
                ModLogger.Log($"[Config] 正在加载配置文件 {ModPath.path}EMCL/settings.json");
                ModLogger.Log($"[Java] 开始读取 Java 缓存");
                config = ReadConfig();
                if (!(DateTimeOffset.Now.ToUnixTimeSeconds() - 604800 > config.tempTime))
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
        #endregion

        #region 窗口控件行为
        private void btnJavaSearch_Click(object sender, RoutedEventArgs e)
        {
            cmbJavaList.Items.Clear();
            javaList = ModJava.JavaCacheGen(config);
            foreach (KeyValuePair<string, bool> i in javaList)
            {
                cmbJavaList.Items.Add(i.Key);
            }
        }

        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //生成且传入参数
                string args = ModLaunch.GetLaunchArgs();
                //现在可以自己选择 Java 了
                string java = $"{cmbJavaList.Text}javaw.exe";
                Thread t = ModThread.RunThread(() => launchGame(java, args), "MinecraftLaunchThread", ThreadPriority.AboveNormal);//创建MC启动线程
                ModThread.threads.Add(t);
                ModLogger.Log("[Launcher] 启动 Minecraft 成功！");
                //MessageBox.Show("启动成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        
        private void cmbJavaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModLogger.Log($"[Java] 选择的 Java 更改为 {(string)cmbJavaList.SelectedValue}");
            try
            {
                Version ver = ModJava.Check((string)cmbJavaList.SelectedValue);
                lblJavaVer.Content = $"当前 Java 版本：{ver}";
            }
            catch (Exception ex)
            {
                TaskDialog choice = new TaskDialog
                {
                    WindowTitle = "Java 错误!",
                    Content = $"{ex.Message}{ModString.newLine}{ModString.newLine}此 Java 可能无效，继续使用此 Java吗？{ModString.newLine}{ModString.newLine}按\"中止\"使用其他 Java{ModString.newLine}按\"重试\"重新检查此 Java{ModString.newLine}按\"忽略\"强制使用 Java"
                };
                choice.Buttons.Add(new TaskDialogButton(ButtonType.Yes));
                choice.Buttons.Add(new TaskDialogButton(ButtonType.Retry));
                choice.Buttons.Add(new TaskDialogButton(ButtonType.No));
                TaskDialogButton result = choice.ShowDialog();
                switch (result.ButtonType)
                {
                    case ButtonType.No:
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
                    case ButtonType.Retry:
                        {
                            try
                            {
                                ModJava.Check(cmbJavaList.Text);
                            }
                            catch
                            {
                                MessageBox.Show("此 Java 无法使用。", "检查失败");
                                lblJavaVer.Content = $"当前 Java 版本：未知";
                            }
                            break;
                        }
                    case ButtonType.Yes:
                    default:
                        {
                            lblJavaVer.Content = $"当前 Java 版本：未知";
                            break;
                        }
                }
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ModApril.IsAprilFool(() =>
            {
                //愚人节彩蛋罢了（
                ModLogger.Log("[2YHLrd] 有人在关掉我！", LogLevel.Normal);
            });
            ModLogger.Log("[Main] 程序正在关闭！", LogLevel.Normal);
            ModLogger.LoggerFlush();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.AppExit();
        }

        private void AppExit()
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
            Close();
            Application.Current.Shutdown();
        }

        private void btnChooseJava_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择 Java";
            dialog.Filter = "Java Executable File (javaw.exe)|javaw.exe";
            if (dialog.ShowDialog() == true)
            {
                string selected = ModString.SlashReplace(new FileInfo(dialog.FileName).DirectoryName!);
                if (!selected.EndsWith("/")) { selected += "/"; }
                cmbJavaList.Items.Add(selected);
                cmbJavaList.SelectedItem = ModString.SlashReplace(selected);
            }
        }

        private void btnMinimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.AppExit();
        }

        private void brdTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion
    }
}
