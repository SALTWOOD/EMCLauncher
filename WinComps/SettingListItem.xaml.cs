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

namespace EMCL.WinComps
{
    /// <summary>
    /// SettingItem.xaml 的交互逻辑
    /// </summary>
    public partial class SettingListItem : UserControl
    {
        public string key = "";
        public List<object> list = new List<object>();

        public SettingListItem()
        {
            InitializeComponent();
        }

        private void cmbItemIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.txtKey.Text = this.list[cmbItemIndex.SelectedIndex].ToString();
        }
    }
}
