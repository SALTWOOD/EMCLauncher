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
using System.Windows.Shapes;
using EMCL;
using EMCL.Modules;
using EMCL.Classes;
using EMCL.WinComps;

namespace EMCL.Pages
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window
    {
        List<ValueTuple<string, object?>> configList;

        public Settings()
        {
            InitializeComponent();
            configList = ConfigDiscoverer.GetAllField(MainWindow._mainWindow!.config);
            ShowConfig(configList);
        }

        private void btnExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mainWindow.Close();
        }

        private void brdTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnMinimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mainWindow.WindowState = WindowState.Minimized;
        }

        private void ShowConfig(List<ValueTuple<string,object?>> values)
        {
            foreach (ValueTuple<string, object?> value in values)
            {
                string name = value.Item1;
                object? obj = value.Item2;
                if (obj != null)
                {
                    SettingItem item = new SettingItem();
                    item.lblSettingItemName.Content = LanguageHelper.Get($"setting.{name}");
                    item.txtValue.Text = obj.ToString();
                    if (obj is string)
                    {
                        string i = (string)obj;
                        item.lblType.Content = "字符串";
                        this.lstSettings.Items.Add(item);
                    }
                    else if (obj is int)
                    {
                        int i = (int)obj;
                        item.lblType.Content = "整型";
                        this.lstSettings.Items.Add(item);
                    }
                    else if (obj is long)
                    {
                        long i = (long)obj;
                        item.lblType.Content = "长整型";
                        this.lstSettings.Items.Add(item);
                    }
                    else if (obj is double)
                    {
                        double i = (double)obj;
                        item.lblType.Content = "双精度浮点数";
                        this.lstSettings.Items.Add(item);
                    }
                    else if (obj is float)
                    {
                        double i = (double)obj;
                        item.lblType.Content = "单精度浮点数";
                        this.lstSettings.Items.Add(item);
                    }
                    else if (obj is bool)
                    {
                        bool i = (bool)obj;
                        item.lblType.Content = "布尔值";
                        this.lstSettings.Items.Add(item);
                    }
                    else if (obj is List<object>)
                    {
                        //NOT PLANNED
                    }
                    else if (obj is Dictionary<object,object>)
                    {
                        //NOT PLANNED
                    }
                    else
                    {

                    }
                    item.key = name;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (SettingItem item in this.lstSettings.Items)
            {
                object? value = ConfigDiscoverer.ConvertFieldValue(MainWindow._mainWindow!.config, item.key, item.txtValue.Text);
                if (value != null)
                {
                    ConfigDiscoverer.SetFieldValue(MainWindow._mainWindow!.config, item.key, value);
                    ModConfig.WriteConfig(MainWindow._mainWindow!.config);
                }
            }
            LanguageHelper.Initialize(MainWindow._mainWindow!.config.language);
            MessageBox.Show("设置保存成功！", "设置项", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
