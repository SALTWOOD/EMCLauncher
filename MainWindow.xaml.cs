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
using System.Windows.Threading;
using EMCL.Pages;

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
        public ModSerial.FingerPrint fingerprint = new ModSerial.FingerPrint();
        public List<Window> windows = new List<Window>();

        #region 异常处理与线程运行
        public void HandleException(Exception ex)
        {
            ModLogger.Log(ex, "", LogLevel.Fatal);
            //Console.WriteLine($"{ex.GetType()}\r\n{ex.Message}\r\n{ex.StackTrace}\r\n\r\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！");
            if (MessageBox.Show($"{ex.GetType()}\r\n{ex.Message}\r\n{ex.StackTrace}\r\n\r\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！", "无法处理的异常", MessageBoxButton.YesNoCancel, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                Process.Start("https://github.com/SALTWOOD/EMCLauncher/issues/new/choose");
            }
        }

        public void HandleException(Exception ex1, Exception ex2)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{ex1.GetType()}\r\n{ex1.Message}\r\n{ex1.StackTrace}\r\n\r\n");
            stringBuilder.Append($"在处理以上异常的过程中，抛出了另一个异常:\r\n");
            stringBuilder.Append($"{ex2.GetType()}\r\n{ex2.Message}\r\n{ex2.StackTrace}\r\n\r\n");
            ModLogger.Log($"[System] 捕获多重异常！\n{stringBuilder}", LogLevel.Fatal);
            //Console.WriteLine($"{ex.GetType()}\r\n{ex.Message}\r\n{ex.StackTrace}\r\n\r\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！");
            if (MessageBox.Show($"{stringBuilder}警告: 通常情况下，在处理一个异常的过程中出现另一个异常属于一个极大的 Bug！\r\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！", "多个无法处理的异常", MessageBoxButton.YesNoCancel, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                Process.Start("https://github.com/SALTWOOD/EMCLauncher/issues/new/choose");
            }
        }

        public void HandleException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.Message.Contains("System.Windows.Threading.Dispatcher.Invoke") ||
                e.Exception.Message.Contains("MS.Internal.AppModel.ITaskbarList.HrInit") ||
                e.Exception.Message.Contains(".NET") ||
                e.Exception.Message.Contains("未能加载文件或程序集"))
            {
                Process.Start("https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/sdk-6.0.413-windows-x64-installer");
                MessageBox.Show("你的 .NET 版本过低或损坏，请在打开的网页中重新下载并安装 .NET 6 后重试！",".NET 错误", MessageBoxButton.OK, MessageBoxImage.Error);
                AppExit();
            }
            else
            {
                try
                {
                    e.Handled = true;
                    HandleException(e.Exception);
                }
                catch (Exception ex)
                {
                    ModLogger.Log("[System] 程序发生致命错误，即将终止！");
                    HandleException(e.Exception, ex);
                }
            }
        }

        public void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            ModLogger.Log("[System] 其他线程内捕获到未处理异常:");
            StringBuilder sb = new StringBuilder();
            if (e.ExceptionObject is Exception)
            {
                Exception ex = (Exception)e.ExceptionObject;
                sb.Append($"{ex.GetType()}\r\n{ex.Message}\r\n{ex.StackTrace}");
            }
            else
            {
                sb.Append($"{e.ExceptionObject}\r\n无法显示详细信息");
            }
            if (e.IsTerminating)
            {
                ModLogger.Log("[System] 程序发生致命错误，即将终止！");
                HandleException((e.ExceptionObject as Exception)!);
                return;
            }
            ModLogger.Log(sb.ToString());
            HandleException((e.ExceptionObject as Exception)!);
        }

        public void HandleException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获
            ModLogger.Log("[System] 捕获线程内未处理异常：" + e.Exception.Message);
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
            HandleException(e.Exception);
        }

        private void RunProtected(Action function)
        {
            try
            {
                function();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        #endregion

        #region 游戏启动
        public int LaunchGame(string javaPath, string launchArgs)
        {
            int result;
            try
            {
                ModLogger.Log($"[Main] ~ ~ ~ S U M M A R Y ~ ~ ~\r\nJava Path: {javaPath}\r\nArgs: {launchArgs}");
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
                    MessageBox.Show($"Minecraft 异常退出！\r\n" +
                        $"程序退出代码：{mc.ExitCode}\r\n" +
                        $"错误日志已保存至{file}", "Minecraft 崩溃！", MessageBoxButton.OK, MessageBoxImage.Error);
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
            //UI线程捕获异常处理事件
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(HandleException);

            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += HandleException;

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleException);

            ModLogger.Log("[Main] 主程序启动中！");
            ModLogger.Log($"[App] {Metadata.name}, 版本 {Metadata.version}");
            ModLogger.Log($"[App] 网络协议版本号 {Metadata.protocol} (0x{Metadata.protocol.ToString("X").PadLeft(8, '0')})");
            //ModLogger.Log($"[System] 计算机基础信息:\n{computer}", LogLevel.Debug);
            //ModLogger.Log($"[System] 计算机唯一识别码: {fingerprint}");
            InitializeComponent();
            ModLogger.Log("[Main] InitializeComponent() 执行完毕！");
            this.Title = Metadata.title;
            this.lblTitle.Content = Metadata.title;
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
                HandleException(ex);
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
                Thread t = ModThread.RunThread(() => LaunchGame(java, args), "MinecraftLaunchThread", ThreadPriority.AboveNormal);//创建MC启动线程
                ModThread.threads.Add(t);
                ModLogger.Log("[Launcher] 启动 Minecraft 成功！");
                MessageBox.Show("启动成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (OutOfMemoryException ex)
            {
                ModLogger.Log(ex, "内存不足", LogLevel.Message);
            }
            catch (Exception ex)
            {
                HandleException(ex);
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
                    Content = $"{ex.Message}\r\n\r\n此 Java 可能无效，继续使用此 Java吗？\r\n\r\n按\"中止\"使用其他 Java\r\n按\"重试\"重新检查此 Java\r\n按\"忽略\"强制使用 Java"
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
                                MessageBox.Show("此 Java 无法使用。", "检查失败", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnDownloadWindow_Click(object sender, RoutedEventArgs e)
        {
            Window newWindow = new PageDownload();
            windows.Add(newWindow);
            newWindow.Show();
        }
        #endregion

        #region 程序退出
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
        #endregion

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            Classes.Tree<int> tree = new Classes.Tree<int>();
            tree.Insert(1);
            tree.Insert(-2);
            tree.Insert(-42);
            tree.Insert(-1);
            //tree.Insert(0);
            tree.Insert(5);
            tree.Insert(6785);
            tree.Insert(453);
            tree.Insert(23);
            MessageBox.Show(string.Join(" ",tree.WalkTree()));
        }
    }
}
