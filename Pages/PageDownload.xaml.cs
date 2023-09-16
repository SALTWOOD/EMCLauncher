using EMCL.Modules;
using EMCL.WinComps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
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
using static EMCL.Modules.MinecraftJson;

namespace EMCL.Pages
{
    /// <summary>
    /// PageDownload.xaml 的交互逻辑
    /// </summary>
    public partial class PageDownload : Window
    {
        MinecraftVersionList? versionList = null;

        public PageDownload()
        {
            InitializeComponent();
            DownloadVersionList();
            UpdateVersionList();
        }

        public void DownloadVersionList()
        {
            versionList = ModDownload.GetMinecraftVersionList();
        }

        public string GetWebsiteCode(string URL)
        {
            string result = null!;
            HttpClient request = null!;
            HttpResponseMessage response = null!;
            StreamReader sr = null!;
            result = "";
            try
            {
                request = new HttpClient();
                response = request.GetAsync(URL).Result;
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (!(sr == null))
                    sr.Dispose();
                if (!(response == null))
                    response.Dispose();
                if (!(request == null))
                    request.Dispose();
            }
            return result;
        }



        public void UpdateVersionList()
        {
            if (versionList != null)
            {
                for (int i = 0; i < versionList.versions.Count; i++)
                {
                    MinecraftVersionInfo current = versionList.versions[i];
                    WinComps.VersionItem versionItem = new WinComps.VersionItem();
                    versionItem.lblVersionName.Content = current.id;
                    if (current.type == "release")
                    {
                        BitmapImage imgSource = new BitmapImage(new Uri("/Images/block_grass.png", UriKind.Relative));
                        versionItem.imgVersionType.Source = imgSource;
                    }
                    else
                    {
                        BitmapImage imgSource = new BitmapImage(new Uri("/Images/block_command_block.png", UriKind.Relative));
                        versionItem.imgVersionType.Source = imgSource;
                        versionItem.versionJson = current;
                    }
                    lstVersions.Items.Add(versionItem);
                }
            }
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

        private void lstVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VersionItem item = (this.lstVersions.Items[this.lstVersions.SelectedIndex] as VersionItem)!;
            if (item != null)
            {
                item.Download(sender);
            }
        }
    }
}
