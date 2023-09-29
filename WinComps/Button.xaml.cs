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
    /// Button.xaml 的交互逻辑
    /// </summary>
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualBasic;

    public partial class Button : UserControl
    {

        // 声明
        public event ClickEventHandler? Click = null; // 自定义事件

        public delegate void ClickEventHandler(object sender, EventArgs e);

        // 自定义属性 
        public string Text
        {
            get
            {
                return (string)lblText.Content;
            }
            set
            {
                lblText.Content = value;
            }
        } // 显示文本
        public Thickness TextPadding
        {
            get
            {
                return lblText.Padding;
            }
            set
            {
                lblText.Padding = value;
            }
        }
        private State _ColorType = State.Normal;  // 配色方案
        public State ColorType
        {
            get
            {
                return _ColorType;
            }
            set
            {
                _ColorType = value;
                RefreshColor();
            }
        }
        public enum State : byte
        {
            Normal = 0,
            Highlight = 1,
            Warning = 2
        }

        // 自定义事件
        private void RefreshColor()
        {
            // TODO
        }

        // 默认事件
        private void Button_Mouse(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke(sender, e);
        }
    }

}
