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

namespace EMCL.WinComps
{
    /// <summary>
    /// VersionItem.xaml 的交互逻辑
    /// </summary>
    public partial class VersionItem : UserControl
    {
        public MinecraftJson.MinecraftVersionInfo? versionJson;

        public VersionItem()
        {
            InitializeComponent();
        }

        public void Download(object sender)
        {
            ModThread.RunThread(() =>
            {
                if (versionJson != null)
                {
                    ModDownload.DownloadMinecraft(versionJson);
                }
            }, "MinecraftDownloaderThread");
        }
    }
}
