using EMCL.Modules;
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

        public void UpdateVersionList()
        {
            if (versionList != null)
            {
                for (int i = 0;i < versionList.versions.Count;i++)
                {
                    MinecraftVersionInfo current = versionList.versions[i];
                    WinComps.VersionItem versionItem = new WinComps.VersionItem();
                    versionItem.lblVersionName.Content = current.id;
                    if (current.type == "release")
                    {
                        BitmapImage imgSource = new BitmapImage(new Uri("/Images/block_grass_block", UriKind.Relative));
                        versionItem.imgVersionType.Source = imgSource;
                    }
                    else
                    {
                        BitmapImage imgSource = new BitmapImage(new Uri("/Images/block_command_block", UriKind.Relative));
                        versionItem.imgVersionType.Source = imgSource;
                    }
                    stkVersions.Children.Add(versionItem);
                }
            }
        }
    }
}
