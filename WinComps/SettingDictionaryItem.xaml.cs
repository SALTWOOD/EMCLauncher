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
    public partial class SettingDictionaryItem : UserControl
    {
        public string key = "";
        public Dictionary<object, object> dict = new Dictionary<object, object>();

        public SettingDictionaryItem()
        {
            InitializeComponent();
        }

        private void cmbItemIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ValueTuple<object, object>> pair = dict.Select(x => new ValueTuple<object, object>(x.Key, x.Value)).ToList();
            this.txtKey.Text = pair[cmbItemIndex.SelectedIndex].Item1.ToString();
            this.txtValue.Text = pair[cmbItemIndex.SelectedIndex].Item2.ToString();
        }
    }
}
